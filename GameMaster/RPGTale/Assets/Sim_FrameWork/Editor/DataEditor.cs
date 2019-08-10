using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using OfficeOpenXml;
using System.Reflection;
using System.ComponentModel;

namespace Sim_FrameWork
{
    public class DataEditor 
    {
        public static string XmlPath = "Assets/Resources/Data/DataFormat/Xml/";
        public static string BinaryPath = "Assets/Resources/Data/DataFormat/Binary/";
        public static string ScriptsPath = "";
        public static string ExcelPath = Application.dataPath + "/../Data/Data_Excel/";
        public static string RegPath = Application.dataPath + "/../Data/Data_Reg/";



        [MenuItem("SimPro/DataTool/类转xml")]
        public static void AssetsClassToXml()
        {
            UnityEngine.Object[] objs = Selection.objects;
            for (int i = 0; i < objs.Length; i++)
            {
                EditorUtility.DisplayProgressBar("文件下的类转成xml", "正在扫描" + objs[i].name + "... ...", 1.0f / objs.Length * i);
                ClassToXml(objs[i].name);
            }
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }


        [MenuItem("SimPro/DataTool/Xml转Excel")]
        public static void XmlToExcel()
        {
            if (!File.Exists(RegPath))
            {
                Debug.LogError("Xml Data not Exit!! Name=");
                return;
            }
            XmlDocument xml = new XmlDocument();
            XmlReader reader = XmlReader.Create(RegPath);
            //Ignore Comments
            XmlReaderSettings setting = new XmlReaderSettings();
            setting.IgnoreComments = true;
            xml.Load(reader);

            XmlNode xn = xml.SelectSingleNode("data");
            XmlElement xe = (XmlElement)xn;
            string className = xe.GetAttribute("name");
            string xmlName = xe.GetAttribute("to");
            string excelName = xe.GetAttribute("from");
            reader.Close();
        }

        /// <summary>
        /// 递归读取
        /// </summary>
        /// <param name="xe"></param>
        private static void ReadXmlNode(XmlElement xmlElement)
        {
            foreach(XmlNode node in xmlElement.ChildNodes)
            {
                XmlElement xe = (XmlElement)node;
                if (xe.GetAttribute("type") == "list")
                {
                    XmlElement list = (XmlElement)node.FirstChild;
                    ReadXmlNode(list);
                }
            }
        }

        [MenuItem("SimPro/DataTool/Xml转成二进制")]
        public static void AllXmlToBinary()
        {
            string path = Application.dataPath.Replace("Assets", "") + XmlPath;
            string[] filesPath = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < filesPath.Length; i++)
            {
                EditorUtility.DisplayProgressBar("查找文件夹下面的Xml", "正在扫描" + filesPath[i] + "... ...", 1.0f / filesPath.Length * i);
                if (filesPath[i].EndsWith(".xml"))
                {
                    string tempPath = filesPath[i].Substring(filesPath[i].LastIndexOf("/") + 1);
                    tempPath = tempPath.Replace(".xml", "");
                    XmlToBinary(tempPath);
                }
            }
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }
        [MenuItem("SimPro/Excel自动转二进制")]
        public static void ExcleTOBinary()
        {
            AllExcelToXml();
            AllXmlToBinary();
        }

        [MenuItem("SimPro/DataTool/Excel转Xml")]
        public static void AllExcelToXml()
        {
            string[] filePaths = Directory.GetFiles(RegPath, "*", SearchOption.AllDirectories);
            for (int i = 0; i < filePaths.Length; i++)
            {
                if (!filePaths[i].EndsWith(".xml"))
                    continue;
                EditorUtility.DisplayProgressBar("查找文件夹下的类", "正在扫描路径" + filePaths[i] + "... ...", 1.0f / filePaths.Length * i);
                string path = filePaths[i].Substring(filePaths[i].LastIndexOf("/") + 1);
                ExcelToXml(path.Replace(".xml", ""));
            }

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        private static void ExcelToXml(string name)
        {
            Dictionary<string, SheetData> sheetDataDic = new Dictionary<string, SheetData>();
            string className = "";
            string xmlName = "";
            string excelName = "";
            //第一步，读取Reg文件，确定类的结构
            Dictionary<string, SheetClass> allSheetClassDic = ReadReg(name, ref excelName, ref xmlName, ref className);

            //第二步，读取excel里面的数据
            string excelPath = ExcelPath + excelName;

            try
            {
                using (FileStream stream = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheets worksheetArray = package.Workbook.Worksheets;
                        for (int i = 0; i < worksheetArray.Count; i++)
                        {
                            SheetData sheetData = new SheetData();
                            ExcelWorksheet worksheet = worksheetArray[i + 1];
                            SheetClass sheetClass = allSheetClassDic[worksheet.Name];
                            int colCount = worksheet.Dimension.End.Column;
                            int rowCount = worksheet.Dimension.End.Row;

                            for (int n = 0; n < sheetClass.VarList.Count; n++)
                            {
                                sheetData.AllName.Add(sheetClass.VarList[n].Name);
                                sheetData.AllType.Add(sheetClass.VarList[n].Type);
                            }

                            for (int m = 1; m < rowCount; m++)
                            {
                                RowData rowData = new RowData();
                                int n = 0;
                                if (string.IsNullOrEmpty(sheetClass.SplitStr) && sheetClass.ParentVar != null
                                    && !string.IsNullOrEmpty(sheetClass.ParentVar.Foregin))
                                {
                                    rowData.ParnetVlue = worksheet.Cells[m + 1, 1].Value.ToString().Trim();
                                    n = 1;
                                }
                                for (; n < colCount; n++)
                                {
                                    ExcelRange range = worksheet.Cells[m + 1, n + 1];
                                    string value = "";
                                    if (range.Value != null)
                                    {
                                        value = range.Value.ToString().Trim();
                                    }
                                    string colValue = worksheet.Cells[1, n + 1].Value.ToString().Trim();
                                    rowData.RowDataDic.Add(GetNameFormCol(sheetClass.VarList, colValue), value);
                                }

                                sheetData.AllData.Add(rowData);
                            }
                            sheetDataDic.Add(worksheet.Name, sheetData);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return;
            }

            //根据类的结构，创建类，并且给每个变量赋值（从excel里读出来的值）
            object objClass = CreateClass(className);

            List<string> outKeyList = new List<string>();
            foreach (string str in allSheetClassDic.Keys)
            {
                SheetClass sheetClass = allSheetClassDic[str];
                if (sheetClass.Depth == 1)
                {
                    outKeyList.Add(str);
                }
            }

            for (int i = 0; i < outKeyList.Count; i++)
            {
                ReadDataToClass(objClass, allSheetClassDic[outKeyList[i]], sheetDataDic[outKeyList[i]], allSheetClassDic, sheetDataDic, null);
            }

            BinarySerializeOpt.Xmlserialize(XmlPath + xmlName, objClass);
            //BinarySerializeOpt.BinarySerilize(BinaryPath + className + ".bytes", objClass);
            Debug.Log(excelName + "表导入unity完成！");
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 根据列名获取变量名
        /// </summary>
        /// <param name="varlist"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private static string GetNameFormCol(List<VarClass> varlist, string col)
        {
            foreach (VarClass varClass in varlist)
            {
                if (varClass.Col == col)
                    return varClass.Name;
            }
            return null;
        }

        //Read Reg
        private static Dictionary<string, SheetClass> ReadReg(string name, ref string excelName, ref string xmlName, ref string className)
        {
            string regPath = RegPath + name + ".xml";
            if (!File.Exists(regPath))
            {
                Debug.LogError("此数据不存在配置变化xml：" + name);
            }
            XmlDocument xml = new XmlDocument();
            XmlReader reader = XmlReader.Create(regPath);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略xml里面的注释
            xml.Load(reader);
            XmlNode xn = xml.SelectSingleNode("data");
            XmlElement xe = (XmlElement)xn;
            className = xe.GetAttribute("name");
            xmlName = xe.GetAttribute("to");
            excelName = xe.GetAttribute("from");
            //储存所有变量的表
            Dictionary<string, SheetClass> allSheetClassDic = new Dictionary<string, SheetClass>();
            ReadXmlNode(xe, allSheetClassDic, 0);
            reader.Close();
            return allSheetClassDic;
        }


        /// <summary>
        /// 反序列化xml到类
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static object GetObjFormXml(string name)
        {
            Type type = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type tempType = asm.GetType(name);
                if (tempType != null)
                {
                    type = tempType;
                    break;
                }
            }
            if (type != null)
            {
                string xmlPath = XmlPath + name + ".xml";
                return BinarySerializeOpt.XmlDeserialize(xmlPath, type);
            }

            return null;
        }


        /// <summary>
        /// 递归读取类里面的数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sheetClass"></param>
        /// <param name="allSheetClassDic"></param>
        /// <param name="sheetDataDic"></param>
        private static void ReadData(object data, SheetClass sheetClass, Dictionary<string, SheetClass> allSheetClassDic, Dictionary<string, SheetData> sheetDataDic, string mainKey)
        {
            List<VarClass> varList = sheetClass.VarList;
            VarClass varClass = sheetClass.ParentVar;
            object dataList = GetMemberValue(data, varClass.Name);

            int listCount = System.Convert.ToInt32(dataList.GetType().InvokeMember("get_Count", BindingFlags.Default | BindingFlags.InvokeMethod, null, dataList, new object[] { }));

            SheetData sheetData = new SheetData();

            if (!string.IsNullOrEmpty(varClass.Foregin))
            {
                sheetData.AllName.Add(varClass.Foregin);
                sheetData.AllType.Add(varClass.Type);
            }

            for (int i = 0; i < varList.Count; i++)
            {
                if (!string.IsNullOrEmpty(varList[i].Col))
                {
                    sheetData.AllName.Add(varList[i].Col);
                    sheetData.AllType.Add(varList[i].Type);
                }
            }

            string tempKey = mainKey;
            for (int i = 0; i < listCount; i++)
            {
                object item = dataList.GetType().InvokeMember("get_Item", BindingFlags.Default | BindingFlags.InvokeMethod, null, dataList, new object[] { i });

                RowData rowData = new RowData();
                if (!string.IsNullOrEmpty(varClass.Foregin) && !string.IsNullOrEmpty(tempKey))
                {
                    rowData.RowDataDic.Add(varClass.Foregin, tempKey);
                }

                if (!string.IsNullOrEmpty(sheetClass.MainKey))
                {
                    mainKey = GetMemberValue(item, sheetClass.MainKey).ToString();
                }

                for (int j = 0; j < varList.Count; j++)
                {
                    if (varList[j].Type == "list" && string.IsNullOrEmpty(varList[j].SplitStr))
                    {
                        SheetClass tempSheetClass = allSheetClassDic[varList[j].ListSheetName];
                        ReadData(item, tempSheetClass, allSheetClassDic, sheetDataDic, mainKey);
                    }
                    else if (varList[j].Type == "list")
                    {
                        SheetClass tempSheetClass = allSheetClassDic[varList[j].ListSheetName];
                        string value = GetSplitStrList(item, varList[j], tempSheetClass);
                        rowData.RowDataDic.Add(varList[j].Col, value);
                    }
                    else if (varList[j].Type == "listStr" || varList[j].Type == "listFloat" || varList[j].Type == "listInt" || varList[j].Type == "listBool" || varList[j].Type == "listUshort")
                    {
                        string value = GetSpliteBaseList(item, varList[j]);
                        rowData.RowDataDic.Add(varList[j].Col, value);
                    }
                    else
                    {
                        object value = GetMemberValue(item, varList[j].Name);
                        if (varList != null)
                        {
                            rowData.RowDataDic.Add(varList[j].Col, value.ToString());
                        }
                        else
                        {
                            Debug.LogError(varList[j].Name + "反射出来为空！");
                        }
                    }
                }

                string key = varClass.ListSheetName;
                if (sheetDataDic.ContainsKey(key))
                {
                    sheetDataDic[key].AllData.Add(rowData);
                }
                else
                {
                    sheetData.AllData.Add(rowData);
                    sheetDataDic.Add(key, sheetData);
                }
            }
        }

        /// <summary>
        /// 获取本身是一个类的列表，但是数据比较少；（没办法确定父级结构的）
        /// </summary>
        /// <returns></returns>
        private static string GetSplitStrList(object data, VarClass varClass, SheetClass sheetClass)
        {
            string split = varClass.SplitStr;
            string classSplit = sheetClass.SplitStr;
            string str = "";
            if (string.IsNullOrEmpty(split) || string.IsNullOrEmpty(classSplit))
            {
                Debug.LogError("类的列类分隔符或变量分隔符为空！！！");
                return str;
            }
            object dataList = GetMemberValue(data, varClass.Name);
            int listCount = System.Convert.ToInt32(dataList.GetType().InvokeMember("get_Count", BindingFlags.Default | BindingFlags.InvokeMethod, null, dataList, new object[] { }));
            for (int i = 0; i < listCount; i++)
            {
                object item = dataList.GetType().InvokeMember("get_Item", BindingFlags.Default | BindingFlags.InvokeMethod, null, dataList, new object[] { i });
                for (int j = 0; j < sheetClass.VarList.Count; j++)
                {
                    object value = GetMemberValue(item, sheetClass.VarList[j].Name);
                    str += value.ToString();
                    if (j != sheetClass.VarList.Count - 1)
                    {
                        str += classSplit.Replace("\\n", "\n").Replace("\\r", "\r");
                    }
                }

                if (i != listCount - 1)
                {
                    str += split.Replace("\\n", "\n").Replace("\\r", "\r");
                }
            }
            return str;
        }

        private static void ReadDataToClass(object objClass, SheetClass sheetClass, SheetData sheetData, Dictionary<string, SheetClass> allSheetClassDic, Dictionary<string, SheetData> sheetDataDic, object keyValue)
        {
            object item = CreateClass(sheetClass.Name);//只是为了得到变量类型
            object list = CreateList(item.GetType());
            for (int i = 0; i < sheetData.AllData.Count; i++)
            {
                if (keyValue != null && !string.IsNullOrEmpty(sheetData.AllData[i].ParnetVlue))
                {
                    if (sheetData.AllData[i].ParnetVlue != keyValue.ToString())
                        continue;
                }
                object addItem = CreateClass(sheetClass.Name);
                for (int j = 0; j < sheetClass.VarList.Count; j++)
                {
                    VarClass varClass = sheetClass.VarList[j];
                    if (varClass.Type == "list" && string.IsNullOrEmpty(varClass.SplitStr))
                    {
                        ReadDataToClass(addItem, allSheetClassDic[varClass.ListSheetName], sheetDataDic[varClass.ListSheetName], allSheetClassDic, sheetDataDic, GetMemberValue(addItem, sheetClass.MainKey));
                    }
                    else if (varClass.Type == "list")
                    {
                        string value = sheetData.AllData[i].RowDataDic[sheetData.AllName[j]];
                        SetSplitClass(addItem, allSheetClassDic[varClass.ListSheetName], value);
                    }
                    else if (varClass.Type == "listStr" || varClass.Type == "listFloat" || varClass.Type == "listInt" || varClass.Type == "listBool" || varClass.Type=="listUshort")
                    {
                        string value = sheetData.AllData[i].RowDataDic[sheetData.AllName[j]];
                        SetSplitBaseClass(addItem, varClass, value);
                    }
                    else
                    {
                        string value = sheetData.AllData[i].RowDataDic[sheetData.AllName[j]];
                        if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(varClass.DefaultValue))
                        {
                            value = varClass.DefaultValue;
                        }
                        if (string.IsNullOrEmpty(value))
                        {
                            Debug.LogError("表格中有空数据，或者Reg文件未配置defaultValue！" + sheetData.AllName[j]);
                            continue;
                        }
                        SetValue(addItem.GetType().GetProperty(sheetData.AllName[j]), addItem, value, sheetData.AllType[j]);
                    }
                }
                list.GetType().InvokeMember("Add", BindingFlags.Default | BindingFlags.InvokeMethod, null, list, new object[] { addItem });
            }
            objClass.GetType().GetProperty(sheetClass.ParentVar.Name).SetValue(objClass, list);
        }

        /// <summary>
        /// 基础List赋值
        /// </summary>
        /// <param name="objClass"></param>
        /// <param name="varClass"></param>
        /// <param name="value"></param>
        private static void SetSplitBaseClass(object objClass, VarClass varClass, string value)
        {
            Type type = null;
            if (varClass.Type == "listStr")
            {
                type = typeof(string);
            }
            else if (varClass.Type == "listFloat")
            {
                type = typeof(float);
            }
            else if (varClass.Type == "listInt")
            {
                type = typeof(int);
            }
            else if (varClass.Type == "listBool")
            {
                type = typeof(bool);
            }else if (varClass.Type == "listUshort")
            {
                type = typeof(ushort);
            }
            object list = CreateList(type);
            string[] rowArray = value.Split(new string[] { varClass.SplitStr }, StringSplitOptions.None);
            for (int i = 0; i < rowArray.Length; i++)
            {
                object addItem = rowArray[i].Trim();
                try
                {
                    list.GetType().InvokeMember("Add", BindingFlags.Default | BindingFlags.InvokeMethod, null, list, new object[] { addItem });
                }
                catch
                {
                    Debug.Log(varClass.ListSheetName + "  里 " + varClass.Name + "  列表添加失败！具体数值是：" + addItem);
                }
            }
            objClass.GetType().GetProperty(varClass.Name).SetValue(objClass, list);
        }

        /// <summary>
        /// 自定义类List赋值
        /// </summary>
        /// <param name="objClass"></param>
        /// <param name="sheetClass"></param>
        /// <param name="value"></param>
        private static void SetSplitClass(object objClass, SheetClass sheetClass, string value)
        {
            object item = CreateClass(sheetClass.Name);
            object list = CreateList(item.GetType());
            if (string.IsNullOrEmpty(value))
            {
                Debug.Log("excel里面自定义list的列里有空值！" + sheetClass.Name);
                return;
            }
            else
            {
                string splitStr = sheetClass.ParentVar.SplitStr.Replace("\\n", "\n").Replace("\\r", "\r");
                string[] rowArray = value.Split(new string[] { splitStr }, StringSplitOptions.None);
                for (int i = 0; i < rowArray.Length; i++)
                {
                    object addItem = CreateClass(sheetClass.Name);
                    string[] valueList = rowArray[i].Trim().Split(new string[] { sheetClass.SplitStr }, StringSplitOptions.None);
                    for (int j = 0; j < valueList.Length; j++)
                    {
                        SetValue(addItem.GetType().GetProperty(sheetClass.VarList[j].Name), addItem, valueList[j].Trim(), sheetClass.VarList[j].Type);
                    }
                    list.GetType().InvokeMember("Add", BindingFlags.Default | BindingFlags.InvokeMethod, null, list, new object[] { addItem });
                }

            }
            objClass.GetType().GetProperty(sheetClass.ParentVar.Name).SetValue(objClass, list);
        }



        /// <summary>
        /// 获取基础List里面的所有值
        /// </summary>
        /// <returns></returns>
        private static string GetSpliteBaseList(object data, VarClass varClass)
        {
            string str = "";
            if (string.IsNullOrEmpty(varClass.SplitStr))
            {
                Debug.LogError("基础List的分隔符为空！");
                return str;
            }
            object dataList = GetMemberValue(data, varClass.Name);
            int listCount = System.Convert.ToInt32(dataList.GetType().InvokeMember("get_Count", BindingFlags.Default | BindingFlags.InvokeMethod, null, dataList, new object[] { }));

            for (int i = 0; i < listCount; i++)
            {
                object item = dataList.GetType().InvokeMember("get_Item", BindingFlags.Default | BindingFlags.InvokeMethod, null, dataList, new object[] { i });
                str += item.ToString();
                if (i != listCount - 1)
                {
                    str += varClass.SplitStr.Replace("\\n", "\n").Replace("\\r", "\r");
                }
            }
            return str;
        }

        /// <summary>
        /// 递归读取配置
        /// </summary>
        /// <param name="xe"></param>
        private static void ReadXmlNode(XmlElement xmlElement, Dictionary<string, SheetClass> allSheetClassDic, int depth)
        {
            depth++;
            foreach (XmlNode node in xmlElement.ChildNodes)
            {
                XmlElement xe = (XmlElement)node;
                if (xe.GetAttribute("type") == "list")
                {
                    XmlElement listEle = (XmlElement)node.FirstChild;

                    VarClass parentVar = new VarClass()
                    {
                        Name = xe.GetAttribute("name"),
                        Type = xe.GetAttribute("type"),
                        Col = xe.GetAttribute("col"),
                        DefaultValue = xe.GetAttribute("defaultValue"),
                        Foregin = xe.GetAttribute("foregin"),
                        SplitStr = xe.GetAttribute("split"),
                    };
                    if (parentVar.Type == "list")
                    {
                        parentVar.ListName = ((XmlElement)xe.FirstChild).GetAttribute("name");
                        parentVar.ListSheetName = ((XmlElement)xe.FirstChild).GetAttribute("sheetname");
                    }

                    SheetClass sheetClass = new SheetClass()
                    {
                        Name = listEle.GetAttribute("name"),
                        SheetName = listEle.GetAttribute("sheetname"),
                        SplitStr = listEle.GetAttribute("split"),
                        MainKey = listEle.GetAttribute("mainKey"),
                        ParentVar = parentVar,
                        Depth = depth,
                    };

                    if (!string.IsNullOrEmpty(sheetClass.SheetName))
                    {
                        if (!allSheetClassDic.ContainsKey(sheetClass.SheetName))
                        {
                            //获取该类下面所有变量
                            foreach (XmlNode insideNode in listEle.ChildNodes)
                            {
                                XmlElement insideXe = (XmlElement)insideNode;

                                VarClass varClass = new VarClass()
                                {
                                    Name = insideXe.GetAttribute("name"),
                                    Type = insideXe.GetAttribute("type"),
                                    Col = insideXe.GetAttribute("col"),
                                    DefaultValue = insideXe.GetAttribute("defaultValue"),
                                    Foregin = insideXe.GetAttribute("foregin"),
                                    SplitStr = insideXe.GetAttribute("split"),
                                };
                                if (varClass.Type == "list")
                                {
                                    varClass.ListName = ((XmlElement)insideXe.FirstChild).GetAttribute("name");
                                    varClass.ListSheetName = ((XmlElement)insideXe.FirstChild).GetAttribute("sheetname");
                                }

                                sheetClass.VarList.Add(varClass);
                            }
                            allSheetClassDic.Add(sheetClass.SheetName, sheetClass);
                        }
                    }

                    ReadXmlNode(listEle, allSheetClassDic, depth);
                }
            }
        }


        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool FileIsUsed(string path)
        {
            bool result = false;

            if (!File.Exists(path))
            {
                result = false;
            }
            else
            {
                FileStream fileStream = null;
                try
                {
                    fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

                    result = false;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    result = true;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 反射new一個list
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object CreateList(Type type)
        {
            Type listType = typeof(List<>);
            Type specType = listType.MakeGenericType(new System.Type[] { type });//确定list<>里面T的类型
            return Activator.CreateInstance(specType, new object[] { });//new出来这个list
        }

        /// <summary>
        /// 反射变量赋值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="var"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        private static void SetValue(PropertyInfo info, object var, string value, string type)
        {
            object val = (object)value;
            if (type == "int")
            {
                val = System.Convert.ToInt32(val);
            }
            else if (type == "bool")
            {
                val = System.Convert.ToBoolean(val);
            }
            else if (type == "float")
            {
                val = System.Convert.ToSingle(val);
            }
            else if (type == "enum")
            {
                val = TypeDescriptor.GetConverter(info.PropertyType).ConvertFromInvariantString(val.ToString());
            }else if(type == "ushort")
            {
                val = System.Convert.ToUInt16(val);
            }else if(type == "string")
            {
                val = System.Convert.ToString(val);
            }

            info.SetValue(var, val);
        }

        /// <summary>
        /// 反射类里面的变量的具体数值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="memeberName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        private static object GetMemberValue(object obj, string memeberName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
        {
            Type type = obj.GetType();
            MemberInfo[] members = type.GetMember(memeberName, bindingFlags);
            //while (members == null || members.Length == 0)
            //{
            //    type = type.BaseType;
            //    if (type == null)
            //        return;

            //    members = type.GetMember("Name",  BindingFlags.Public | BindingFlags.Default);
            //}

            switch (members[0].MemberType)
            {
                case MemberTypes.Field:
                    return type.GetField(memeberName, bindingFlags).GetValue(obj);
                case MemberTypes.Property:
                    return type.GetProperty(memeberName, bindingFlags).GetValue(obj);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 反射创建类的实例
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static object CreateClass(string name)
        {
            object obj = null;
            Type type = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type tempType = asm.GetType(name);
                if (tempType != null)
                {
                    type = tempType;
                    break;
                }
            }
            if (type != null)
            {
                obj = Activator.CreateInstance(type);
            }
            return obj;
        }

        /// <summary>
        /// xml转二进制
        /// </summary>
        /// <param name="name"></param>
        private static void XmlToBinary(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            try
            {
                Type type = null;
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type tempType = asm.GetType(name);
                    if (tempType != null)
                    {
                        type = tempType;
                        break;
                    }
                }
                if (type != null)
                {
                    string xmlPath = XmlPath + name + ".xml";
                    string binaryPath = BinaryPath + name + ".bytes";
                    object obj = BinarySerializeOpt.XmlDeserialize(xmlPath, type);
                    BinarySerializeOpt.BinarySerilize(binaryPath, obj);
                    Debug.Log(name + "xml转二进制成功，二进制路径为:" + binaryPath);
                }
            }
            catch
            {
                Debug.LogError(name + "xml转二进制失败！");
            }
        }

        /// <summary>
        /// 实际的类转XML
        /// </summary>
        /// <param name="name"></param>
        private static void ClassToXml(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            try
            {
                Type type = null;
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type tempType = asm.GetType(name);
                    if (tempType != null)
                    {
                        type = tempType;
                        break;
                    }
                }
                if (type != null)
                {
                    var temp = Activator.CreateInstance(type);
                    if (temp is ExcelBase)
                    {
                        (temp as ExcelBase).Construction();
                    }
                    string xmlPath = XmlPath + name + ".xml";
                    BinarySerializeOpt.Xmlserialize(xmlPath, temp);
                    Debug.Log(name + "类转xml成功，xml路径为:" + xmlPath);
                }
            }
            catch
            {
                Debug.LogError(name + "类转xml失败！");
            }
        }
    }

    public class SheetClass
    {
        //所属父级Var变量
        public VarClass ParentVar { get; set; }
        //深度
        public int Depth { get; set; }
        //类名
        public string Name { get; set; }
        //类对应的sheet名
        public string SheetName { get; set; }
        //主键
        public string MainKey { get; set; }
        //分隔符
        public string SplitStr { get; set; }
        //所包含的变量
        public List<VarClass> VarList = new List<VarClass>();
    }


    public class VarClass
    {
        //Origin class name
        public string Name { get; set; }
        public string Type { get; set; }
        public string Col { get; set; }
        public string DefaultValue { get; set; }
        public string Foregin { get; set; }
        public string SplitStr { get; set; }
        public string ListName { get; set; }
        //如果自己是list,对应的sheet名
        public string ListSheetName { get; set; }
    }

    public class SheetData
    {
        public List<string> AllName = new List<string>();
        public List<string> AllType = new List<string>();
        public List<RowData> AllData = new List<RowData>();
    }

    public class RowData
    {
        public string ParnetVlue = "";
        public Dictionary<string, string> RowDataDic = new Dictionary<string, string>();
    }


}