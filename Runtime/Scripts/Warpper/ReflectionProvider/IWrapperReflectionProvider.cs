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

    }
#nullable restore
}
