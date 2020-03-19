using System;

namespace TinaX.XComponent.Internal
{
    [System.Serializable]
    public class TypeBindingInfo
    {
        public string Name;
        public string TypeName;
        public string[] Value_String;
        public double[] Value_Double;
        public long[] Value_long;
        public UnityEngine.AnimationCurve Value_Curve;
    }
}
