using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace RPG_FrameWork
{
    public class DataEditor 
    {
        [MenuItem("RPG_Tale/DataTool/Xml转Excel")]
        public static void XmlToExcel()
        {
            string RegPath = Application.dataPath + "/../Data/Data_Reg/";
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
    }
}