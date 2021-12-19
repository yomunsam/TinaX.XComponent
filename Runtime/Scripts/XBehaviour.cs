using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX.Core.Utils;
using TinaX.XComponent.Internal;
using UnityEngine;

namespace TinaX.XComponent
{
    public abstract class XBehaviour : XBehaviourBase
    {
        public GameObject gameObject;
        public Transform transform;
        public XComponent xComponent;

        protected DisposableGroup DisposableGroup
        {
            get
            {
                if (m_DisposableGroup == null)
                    m_DisposableGroup = new DisposableGroup();
                return m_DisposableGroup;
            }
        }

        private DisposableGroup m_DisposableGroup;

        public virtual void OnEnable() { }
        public virtual void OnDisable() { }

        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void OnDestroy() { }

        public sealed override void BeforeDestroy()
        {
            m_DisposableGroup?.Dispose();
        }

        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }

        public virtual void OnApplicationFocus(bool focus) { }
        public virtual void OnApplicationPause(bool pause) { }
        public virtual void OnApplicationQuit() { }

        public virtual void OnMessage(string msgName, params object[] msgParams) { }

        //protected void EnableUpdate(int order = 0)
        //{
        //    this.DisposableGroup.RegisterUpdate(this.Update, order);
        //}

        //protected void EnableFixedUpdate(int order = 0)
        //{
        //    this.DisposableGroup.RegisterFixedUpdate(this.FixedUpdate, order);
        //}

        //protected void EnableLateUpdate(int order = 0)
        //{
        //    this.DisposableGroup.RegisterLateUpdate(this.LateUpdate, order);
        //}

    }
}
