using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TinaX.XComponent.Internal;

namespace TinaXEditor.XComponent
{
    public class BaseTypeGUIHandler
    {
        public string TypeName;
        public Func<Rect, object, object> DrawEditorGUI;
        public Func<object, float> EditorGUIHeight;

        public BaseTypeGUIHandler() { }
        public BaseTypeGUIHandler(string typeName)
        {
            this.TypeName = typeName;
        }
    }
}
