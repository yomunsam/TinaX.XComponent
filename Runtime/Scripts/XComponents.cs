using System;
using System.Collections.Generic;
using System.Reflection;
using TinaX.XComponent.Internal;
using TinaX.XComponent.Warpper;
using TinaX.XComponent.Warpper.ReflectionProvider;

namespace TinaX.XComponent
{
    public static class XComponents
    {
        private static Dictionary<string, BaseTypeHandler> m_HandlerDict;
        private static List<string> mList_Type_Names;

        private static DefaultWrapperReflectionProvider _defaultWrapperReflectionProvider;

        static XComponents()
        {
            m_HandlerDict = new Dictionary<string, BaseTypeHandler>();
            mList_Type_Names = new List<string>();
            RegisterBaseTypeHandlers(BaseTypeHandlerListInternal.Handler);
        }

        public static IWrapperReflectionProvider DefaultWrapperReflectionProvider
        {
            get
            {
                if (_defaultWrapperReflectionProvider == null)
                    _defaultWrapperReflectionProvider = new DefaultWrapperReflectionProvider();
                return _defaultWrapperReflectionProvider;
            }
        }

        public static bool TryGetValue(TypeBindingInfo bindinfo, out object value)
        {
            if(m_HandlerDict.TryGetValue(bindinfo.TypeName,out var handler))
            {
                value = handler.GetValueFunc(bindinfo);
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }


        public static void RegisterBaseTypeHandler(BaseTypeHandler handler)
        {
            if (!m_HandlerDict.ContainsKey(handler.TypeName))
                m_HandlerDict.Add(handler.TypeName, handler);
            if (!mList_Type_Names.Contains(handler.TypeName))
                mList_Type_Names.Add(handler.TypeName);
        }

        public static void RegisterBaseTypeHandlers(IEnumerable<BaseTypeHandler> handlers)
        {
            foreach (var item in handlers)
                RegisterBaseTypeHandler(item);
        }

        public static string[] GetTypeNames()
        {
            return mList_Type_Names.ToArray();
        }

        public static bool TryGetHandler(string TypeName, out BaseTypeHandler handler)
        {
            return m_HandlerDict.TryGetValue(TypeName, out handler);
        }

        public static void InjectBindings(XComponentScriptBase component, object injected_obj)
        {
            if (component == null || injected_obj == null)
                return;
            if(injected_obj is XBehaviourWarpper)
            {
                //这是个包装器，
                return;
            }
            Type type_uobj = typeof(UnityEngine.Object);
            Type type_go = typeof(UnityEngine.GameObject);
            Type t_obj = injected_obj.GetType();
            foreach(var property in t_obj.GetRuntimeProperties())
            {
                string find_bind_name = property.Name;
                bool null_able = true;
                var attr = property.GetCustomAttribute<BindingAttribute>(true);
                if(attr != null)
                {
                    null_able = attr.Nullable;
                    if (!string.IsNullOrEmpty(attr.BindingName))
                        find_bind_name = attr.BindingName;
                }

                if (property.PropertyType.IsSubclassOf(type_uobj))
                {
                    if (component.TryGetBindingUObject(find_bind_name, out var uobj))
                    {
                        Type __t_uobj = uobj.GetType();
                        if (__t_uobj.IsSubclassOf(property.PropertyType) || __t_uobj.IsAssignableFrom(property.PropertyType))
                        {
                            property.SetValue(injected_obj, uobj);
                            continue;
                        }
                        else
                        {
                            if(__t_uobj == type_go) //如果绑定类型是GameObject,尝试寻找想要被注入的类型是不是这个GameObject上的Component
                            {
                                if(((UnityEngine.GameObject)uobj).TryGetComponent(property.PropertyType,out var _result))
                                {
                                    property.SetValue(injected_obj, _result);
                                    continue;
                                }
                            }
                        }
                    }
                }
                
                //BaseType Inject
                if(component.TryGetBindingType(find_bind_name,out var obj))
                {
                    if(obj.GetType() == property.PropertyType)
                    {
                        property.SetValue(injected_obj, obj);
                        continue;
                    }
                }

                if (!null_able)
                    throw new Exception($"Inject binding object failed: cannot found binding object \"{find_bind_name}\"");
            }

            foreach(var field in t_obj.GetRuntimeFields())
            {
                if (field.IsPrivate) continue;
                string find_bind_name = field.Name;
                bool null_able = true;
                var attr = field.GetCustomAttribute<BindingAttribute>(true);
                if(attr != null)
                {
                    null_able = attr.Nullable;
                    if (!string.IsNullOrEmpty(attr.BindingName))
                        find_bind_name = attr.BindingName;
                }

                if (field.FieldType.IsSubclassOf(type_uobj))
                {
                    if (component.TryGetBindingUObject(find_bind_name, out var uobj))
                    {
                        Type __t_uobj = uobj.GetType();
                        if (__t_uobj.IsSubclassOf(field.FieldType) || __t_uobj.IsAssignableFrom(field.FieldType))
                        {
                            field.SetValue(injected_obj, uobj);
                            continue;
                        }
                        else
                        {
                            if (__t_uobj == type_go) //如果绑定类型是GameObject,尝试寻找想要被注入的类型是不是这个GameObject上的Component
                            {
                                if (((UnityEngine.GameObject)uobj).TryGetComponent(field.FieldType, out var _result))
                                {
                                    field.SetValue(injected_obj, _result);
                                    continue;
                                }
                            }
                        }
                    }
                }

                //BaseType Inject
                if (component.TryGetBindingType(find_bind_name, out var obj))
                {
                    if (obj.GetType() == field.FieldType)
                    {
                        field.SetValue(injected_obj, obj);
                        continue;
                    }
                }

                if (!null_able)
                    throw new Exception($"Inject binding object failed: cannot found binding object \"{find_bind_name}\"");
            }

        }

    }
}
