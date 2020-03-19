using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                Behaviour.OnFixedUpdate();
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
