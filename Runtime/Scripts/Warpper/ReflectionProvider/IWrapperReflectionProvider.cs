using System;

namespace TinaX.XComponent.Warpper.ReflectionProvider
{
#nullable enable
    /// <summary>
    /// 包装器 反射提供者
    /// </summary>
    public interface IWrapperReflectionProvider
    {
        Type GetSourceType(ref object sourceObject);

        Action? GetAwake(ref object sourceObject, ref Type sourceType);
        Action? GetStart(ref object sourceObject, ref Type sourceType);
        void InjectBindings(ref XComponentScriptBase component, object sourceObject, Type sourceType);
    }
#nullable restore
}
