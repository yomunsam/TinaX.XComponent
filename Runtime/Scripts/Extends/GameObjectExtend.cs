using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TinaX.XComponent
{
    public static class GameObjectExtend
    {
        public static XComponent AddXComponent(this GameObject go ,XBehaviour behaviour)
        {
            return go.AddComponent<XComponent>().SetBehaviour(behaviour);
        }

        public static XComponent AddXComponent<T>(this GameObject go) where T : XBehaviour
        {
            var obj = Activator.CreateInstance<T>();
            return go.AddComponent<XComponent>().SetBehaviour(obj);
        }

        public static XComponent GetXComponent<T>(this GameObject go) where T: XBehaviour
        {
            var cs = go.GetComponents<XComponent>();
            if (cs == null || cs.Length == 0)
                return null;
            string type_full_name = typeof(T).FullName;
            foreach(var c in cs)
            {
                if (c.Behaviour == null)
                    continue;
                if (c.FullName == type_full_name)
                    return c;
            }
            return null;
        }

        public static T GetXBehaviour<T>(this GameObject go) where T : XBehaviour
        {
            var cs = go.GetComponents<XComponent>();
            if (cs == null || cs.Length == 0)
                return null;
            string type_full_name = typeof(T).FullName;
            foreach (var c in cs)
            {
                if (c.Behaviour == null)
                    continue;
                if (c.FullName == type_full_name)
                    return c as T;
            }
            return null;
        }

        public static XComponent GetXComponent(this GameObject go, Type type)
        {
            var cs = go.GetComponents<XComponent>();
            if (cs == null || cs.Length == 0)
                return null;
            string type_full_name = type.FullName;
            foreach (var c in cs)
            {
                if (c.Behaviour == null)
                    continue;
                if (c.FullName == type_full_name)
                    return c;
            }
            return null;
        }

        public static XBehaviour GetXBehaviour(this GameObject go, Type type)
        {
            var cs = go.GetComponents<XComponent>();
            if (cs == null || cs.Length == 0)
                return null;
            string type_full_name = type.FullName;
            foreach (var c in cs)
            {
                if (c.Behaviour == null)
                    continue;
                if (c.FullName == type_full_name)
                    return c.Behaviour;
            }
            return null;
        }

        public static XComponent GetXComponent(this GameObject go, string typeName)
        {
            var cs = go.GetComponents<XComponent>();
            if (cs == null || cs.Length == 0)
                return null;
            foreach (var c in cs)
            {
                if (c.Behaviour == null)
                    continue;
                if (c.Name == typeName)
                    return c;
            }
            return null;
        }

        public static XBehaviour GetXBehaviour(this GameObject go, string typeName)
        {
            var cs = go.GetComponents<XComponent>();
            if (cs == null || cs.Length == 0)
                return null;
            foreach (var c in cs)
            {
                if (c.Behaviour == null)
                    continue;
                if (c.Name == typeName)
                    return c.Behaviour;
            }
            return null;
        }

    }
}
