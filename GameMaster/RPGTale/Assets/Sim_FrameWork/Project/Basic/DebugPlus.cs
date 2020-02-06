using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class DebugPlus
    {
        public static bool enable = true;
        public enum debugColor
        {
            black,
            green,
            blue,
            red,
            yellow
        }

        static void SendDebugMsg(string content)
        {
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.DebugDialog, new UIMessage(UIMsgType.ShowDebugMsg,new List<object>() { content}));
        }

        public static void Log(object message, debugColor color = debugColor.blue)
        {
            if (!enable)
                return;
            Debug.Log(Message(message, color));
        }

        public static void LogError(object message, debugColor color = debugColor.red)
        {
            if (!enable)
                return;
            Debug.LogError(Message(message, color));
            SendDebugMsg(Message(message, color));
        }

        public static void LogBold(object message, debugColor color = debugColor.black)
        {
            if (!enable)
                return;
            Debug.Log(Message(message, color, true));
        }

        public static void LogTitle(object title, int level, debugColor color)
        {
            if (!enable)
                return;
            LogBold(Title(title, level, color));
        }

        /// <summary>
        /// Out Put Obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="color"></param>
        public static void LogObject<T>(T content, debugColor color = debugColor.black)
        {
            if (!enable)
                return;
            Debug.Log(Message(typeof(T).Name, color, true) + "\n" + ObjectMessage<T>(content));
        }

        /// <summary>
        /// Out put List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        public static void LogList<T>(List<T> content)
        {
            if (!enable || content == null || content.Count <= 0)
                return;

            var result = Title(typeof(T).Name + "List", 0, debugColor.green) + "\n\n";
            for(int i = 0; i < content.Count; i++)
            {
                result += Title(typeof(T).Name + "_" + i, 1, debugColor.green) + "\n";
                result += ObjectMessage<T>(content[i]) + "\n";
            }
            result += Title("End", 0, debugColor.green);

            Debug.Log(result);
        }

        /// <summary>
        /// Out put Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        public static void LogArray<T>(T[] content)
        {
            if (!enable || content != null || content.Length <= 0)
                return;

            var result = Title(typeof(T).Name + "List", 0, debugColor.green) + "\n";
            for(int i = 0; i < content.Length; i++)
            {
                result += Title(typeof(T).Name + "_" + i, 1, debugColor.green) + "\n";
                result += ObjectMessage<T>(content[i]) + "\n";
            }
            result += Title("End", 0, debugColor.green);

            Debug.Log(result);
        }

        private static string Message(object message, debugColor color = debugColor.black,bool blod = false)
        {
            var content = string.Format("<color={0}>{1}</color>", color.ToString(), message);
            content = blod ? string.Format("<b>{0}</b>", content) : content;

            return content;
        }

        private static string Title(object title, int level, debugColor color)
        {
            return Message(string.Format("{0}{1}{2}{1}", Space(level * 2), Character(16 - level * 2), title), color);
        }
        private static string ObjectMessage<T>(T content)
        {
            var result = "";

            var fields = typeof(T).GetFields();
            for(int i = 0; i < fields.Length; i++)
            {
                result += Space(4) + Message(fields[i].Name + ":", debugColor.green, true) + fields[i].GetValue(content)+"\n";
            }

            var properties = typeof(T).GetProperties();
            for(int i = 0; i < properties.Length; i++)
            {
                result += Space(4) + Message(properties[i].Name + ":", debugColor.green, true) + properties[i].GetValue(content, null) + "\n";
            }
            return result;
        }

        private static string Space(int number)
        {
            var space = "";
            for(int i = 0; i < number; i++)
            {
                space += "";
            }
            return space;
        }

        private static string Character(int num,string character = "=")
        {
            var result = "";
            for(int i = 0; i < num; i++)
            {
                result += "=";
            }
            return result;
        }
    }
}