using System;
using TinaX.XComponent.Warpper.ReflectionProvider;

namespace TinaX.XComponent.Warpper
{
#nullable enable
    /// <summary>
    /// XBehaviour 包装器
    /// </summary>
    public class XBehaviourWarpper : XBehaviour
    {
        public IWrapperReflectionProvider ReflectionProvider { get; protected set; }

        public readonly object SourceObject;
        public readonly Type SourceType;

        public readonly Action? SourceAwake;
        public readonly Action? SourceStart;

        public XBehaviourWarpper(object sourceObject, IWrapperReflectionProvider? reflectionProvider = null)
        {
            if(sourceObject == null)
                throw new ArgumentNullException(nameof(sourceObject));

            if (reflectionProvider == null)
                ReflectionProvider = XComponents.DefaultWrapperReflectionProvider;
            else
                ReflectionProvider = reflectionProvider;

            SourceObject = sourceObject;
            SourceType = ReflectionProvider.GetSourceType(ref SourceObject);

            SourceAwake = ReflectionProvider.GetAwake(ref SourceObject, ref SourceType);
            SourceStart = ReflectionProvider.GetStart(ref SourceObject, ref SourceType);
        }



        public override void Awake()
        {
            SourceAwake?.Invoke();
        }

        public override void Start()
        {
            SourceStart?.Invoke();
        }

    }

#nullable restore
}
