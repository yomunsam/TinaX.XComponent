using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.XComponent
{
    public abstract class XBehaviour
    {
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }

        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void OnDestroy() { }


        public virtual void Update() { }
        public virtual void OnFixedUpdate() { }
        public virtual void LateUpdate() { }

        public virtual void OnApplicationFocus(bool focus) { }
        public virtual void OnApplicationPause(bool pause) { }
        public virtual void OnApplicationQuit() { }

    }
}
