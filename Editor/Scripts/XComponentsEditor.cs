using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaXEditor.XComponent.Internal;

namespace TinaXEditor.XComponent
{
    public static class XComponentsEditor
    {
        private static Dictionary<string, BaseTypeGUIHandler> mDict_GUIHandler;
        private static List<string> mList_Type_Names;

        static XComponentsEditor()
        {
            mDict_GUIHandler = new Dictionary<string, BaseTypeGUIHandler>();
            mList_Type_Names = new List<string>();

            RegisterTypeGUIHandlers(BaseTypeGUIHandlerInternal.Handlers);
        }


        public static void RegisterTypeGUIHandler(BaseTypeGUIHandler handler)
        {
            if (!mDict_GUIHandler.ContainsKey(handler.TypeName))
                mDict_GUIHandler.Add(handler.TypeName, handler);
            if (!mList_Type_Names.Contains(handler.TypeName))
                mList_Type_Names.Add(handler.TypeName);
        }

        public static void RegisterTypeGUIHandlers(IEnumerable<BaseTypeGUIHandler> handlers)
        {
            foreach (var item in handlers)
                RegisterTypeGUIHandler(item);
        }

        public static bool TryGetGUIHandler(string TypeName, out BaseTypeGUIHandler handler)
        {
            return mDict_GUIHandler.TryGetValue(TypeName, out handler);
        }

    }
}
