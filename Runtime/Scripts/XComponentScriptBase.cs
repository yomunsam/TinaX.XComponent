using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinaX.XComponent.Internal;

namespace TinaX.XComponent
{
    public class XComponentScriptBase : MonoBehaviour
    {
        public UnityObjectBindingInfo[] UObjectBindInfos;
        public TypeBindingInfo[] TypeBindInfos;


        public bool TryGetBindingUObject(string Name, out UnityEngine.Object obj)
        {
            foreach(var item in UObjectBindInfos)
            {
                if(item.Name == Name && item.Object != null)
                {
                    obj = item.Object;
                    return true;
                }
            }
            obj = null;
            return false;
        }

        public bool TryGetBindingType(string Name, out object obj)
        {
            foreach(var item in TypeBindInfos)
            {
                if(item.Name == Name)
                {
                    if(XComponents.TryGetValue(item,out obj))
                    {
                        return true;
                    }
                }
            }
            obj = null;
            return false;
        }

        public virtual void SendMsg(string messageName, params object[] param)
        {

        }
        
        public virtual void SendQueueMsg(string messageName, params object[] param)
        {

        }

    }
}

