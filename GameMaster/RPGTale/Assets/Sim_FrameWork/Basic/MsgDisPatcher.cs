using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG_FrameWork
{
    public abstract partial class MonoBehaviourSimplify
    {

        List<MsgRecord> m_MsgRecord = new List<MsgRecord>();

        class MsgRecord
        {
            private MsgRecord() { }

            static Stack<MsgRecord> m_MsgRecordPool = new Stack<MsgRecord>();

            public static MsgRecord Allocate(string msgName,Action<object> onMsgReceived)
            {
                MsgRecord retRecord=null;
                if (m_MsgRecordPool.Count > 0)
                {
                    retRecord = m_MsgRecordPool.Pop();
                }
                else
                {
                    retRecord = new MsgRecord();
                }
                retRecord.Name = msgName;
                retRecord.OnMsgReceived = onMsgReceived;

                return retRecord;
            }

            public void Recycle()
            {
                Name = null;
                OnMsgReceived = null;
                m_MsgRecordPool.Push(this);
            }

            public string Name;

            public Action<object> OnMsgReceived;
        }
        public void RegisterMsg(string msgName,Action<object> onMsgReceives)
        {
            MsgDisPatcher.Regist(msgName, onMsgReceives);
            m_MsgRecord.Add(MsgRecord.Allocate(msgName, onMsgReceives));
        }

        private void OnDestory()
        {
            OnBeforeDestory();

            foreach(var msg in m_MsgRecord)
            {
                MsgDisPatcher.UnRegistMsg(msg.Name, msg.OnMsgReceived);
                msg.Recycle();
            }
            m_MsgRecord.Clear();
        }

        //！！用这个 OnDestory 不然原来的OnDestory会被覆写
        protected abstract void OnBeforeDestory();

    }

    public class MsgDisPatcher : MonoBehaviourSimplify
    {
        static Dictionary<string, Action<object>> RegisterMsgsDic = new Dictionary<string, Action<object>>();

        protected override void OnBeforeDestory() { }

        //Register
        public static void Regist(string msgName, Action<object> onMsgReceived)
        {
            if (!RegisterMsgsDic.ContainsKey(msgName))
            {
                RegisterMsgsDic.Add(msgName, _ => { });
            }
            RegisterMsgsDic[msgName] += onMsgReceived;

        }

        //UnRegister
        public static void UnRegistMsgAll(string msgName)
        {
            RegisterMsgsDic.Remove(msgName);
        }

        public static void UnRegistMsg(string msgName, Action<object> onMsgReceived)
        {
            if (RegisterMsgsDic.ContainsKey(msgName))
            {
                RegisterMsgsDic[msgName] -= onMsgReceived;
            }
        }

        //Send
        public static void MsgSend(string msgName, object data)
        {
            if (RegisterMsgsDic.ContainsKey(msgName))
            {
                RegisterMsgsDic[msgName](data);
            }

        }

    }

   
}
