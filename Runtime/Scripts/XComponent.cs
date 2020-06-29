using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace TinaX.XComponent
{
    [AddComponentMenu("TinaX/XComponent/XComponent")]
    public class XComponent : XComponentScriptBase
    {
        struct MsgQueue
        {
            public string MessageName;
            public object[] Args;
        }

        public string Name { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public XBehaviour Behaviour { get; private set; }

        private bool mAwaked = false;
        private bool mStarted = false;

        private bool mEnable = false;
        private bool mDisable = false;

        private List<MsgQueue> m_MsgQueue = new List<MsgQueue>();

        internal protected XComponent SetBehaviour(XBehaviour behaviour)
        {
            if (Behaviour != null)
                throw new Exception("Behaviour has exist, cannot add repeatedly");

            this.Behaviour = behaviour;
            this.Name = behaviour.GetType().Name;
            this.FullName = behaviour.GetType().FullName;
            behaviour.gameObject = this.gameObject;
            behaviour.transform = this.transform;
            behaviour.xComponent = this;

            if (mAwaked)
                this.Behaviour.Awake();

            if (mEnable && this.enabled)
                this.Behaviour.OnEnable();
            
            if(m_MsgQueue.Count > 0)
            {
                foreach(var item in m_MsgQueue)
                {
                    this.Behaviour.OnMessage(item.MessageName, item.Args);
                }
                m_MsgQueue.Clear();
            }

            if (mStarted)
                this.Behaviour.Start();

            if (mDisable && !this.enabled)
                this.Behaviour.OnDisable();

            return this;
        }




        public override void SendMsg(string msgName, params object[] param)
        {
            if(Behaviour!= null)
            {
                this.Behaviour.OnMessage(msgName, param);
            }
        }

        /// <summary>
        /// 发送队列消息 （如果没有Behaviour时，则进入队列）
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="param"></param>
        public override void SendQueueMsg(string msgName, params object[] param)
        {
            if (Behaviour != null)
            {
                this.Behaviour.OnMessage(msgName, param);
            }
            else
            {
                this.m_MsgQueue.Add(new MsgQueue
                {
                    MessageName = msgName,
                    Args = param
                });
            }
        }

        protected virtual void Awake()
        {
            if (Behaviour != null)
                Behaviour.Awake();
            mAwaked = true;
        }

        private void Start()
        {
            if (Behaviour != null)
                Behaviour.Start();
            mStarted = true;
        }

        protected virtual void OnDestroy()
        {
            if (Behaviour != null)
            {
                Behaviour.BeforeDestroy();
                Behaviour.OnDestroy();
                Behaviour = null;
            }
        }


        private void OnEnable()
        {
            mEnable = true;
            if (Behaviour != null)
                Behaviour.OnEnable();
        }
        private void OnDisable()
        {
            mDisable = true;
            if (Behaviour != null)
                Behaviour.OnDisable();
        }

        public void InvokeUpdate()
        {
            if (Behaviour != null)
                Behaviour.Update();
        }

        public void InvokeFixedUpdate()
        {
            if (Behaviour != null)
                Behaviour.FixedUpdate();
        }

        public void InvokeLateUpdate()
        {
            if (Behaviour != null)
                Behaviour.LateUpdate();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (Behaviour != null)
                Behaviour.OnApplicationFocus(focus);
        }

        private void OnApplicationPause(bool pause)
        {
            if (Behaviour != null)
                Behaviour.OnApplicationPause(pause);
        }

        private void OnApplicationQuit()
        {
            if (Behaviour != null)
                Behaviour.OnApplicationQuit();
        }


    }
}
