using System;
using System.Reflection;

namespace TinaX.XComponent.Warpper.ReflectionProvider
{
#nullable enable
    /// <summary>
    /// 默认反射提供者
    /// </summary>
    public class DefaultWrapperReflectionProvider : IWrapperReflectionProvider
    {
        BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        public DefaultWrapperReflectionProvider()
        {
        }

        public Type GetSourceType(ref object sourceObject)
        {
            return sourceObject.GetType();
        }

        public Action? GetAwake(ref object sourceObject, ref Type sourceType)
        {
            var method = sourceType.GetMethod("Awake", bindingFlags);
            if(method == null)
                return null;
            return Delegate.CreateDelegate(typeof(Action), sourceObject, method) as Action;
        }

        public Action? GetStart(ref object sourceObject, ref Type sourceType)
        {
            var method = sourceType.GetMethod("Start", bindingFlags);
            if (method == null)
                return null;
            return Delegate.CreateDelegate(typeof(Action), sourceObject, method) as Action;
        }


        //绑定对象注入

    }
#nullable restore
}
