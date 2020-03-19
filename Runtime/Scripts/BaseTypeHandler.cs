using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX.XComponent.Internal;

namespace TinaX.XComponent
{
    public class BaseTypeHandler
    {
        public string TypeName;
        public Func<TypeBindingInfo, object> GetValueFunc;
        public Action<object, TypeBindingInfo> SetValueFunc;

        public BaseTypeHandler() { }
        public BaseTypeHandler(string typeName) { this.TypeName = typeName; }
        public BaseTypeHandler(string typeName, Func<TypeBindingInfo, object> getValueFunc, Action<object,TypeBindingInfo> setValueFunc)
        {
            this.TypeName = typeName;
            this.GetValueFunc = getValueFunc;
            this.SetValueFunc = setValueFunc;
        }
    }
}
