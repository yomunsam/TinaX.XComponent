using System;
using UnityEngine;

namespace TinaX.XComponent
{
    [AddComponentMenu("TinaX/XComponent/XComponent")]
    public class XComponent : XComponentScriptBase
    {
        public string Name { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public XBehaviour Behaviour { get; private set; }

        private bool mAwaked = false;
        private bool mStarted = false;

        private bool mEnable = false;
        private bool mDisable = false;

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

        private void Awake()
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

        private void OnDestroy()
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
