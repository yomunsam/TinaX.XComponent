using System;
using System.Reflection;
using UnityEngine;

namespace TinaX.XComponent.Warpper.ReflectionProvider
{
#nullable enable
    /// <summary>
    /// 默认反射提供者
    /// </summary>
    public class DefaultWrapperReflectionProvider : IWrapperReflectionProvider
    {
        readonly BindingFlags _BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        readonly Type _NullableType = typeof(Nullable<>);

        public DefaultWrapperReflectionProvider()
        {
        }

        public Type GetSourceType(ref object sourceObject)
        {
            return sourceObject.GetType();
        }

        public Action? GetAwake(ref object sourceObject, ref Type sourceType)
        {
            var method = sourceType.GetMethod("Awake", _BindingFlags);
            if(method == null)
                return null;
            return Delegate.CreateDelegate(typeof(Action), sourceObject, method) as Action;
        }

        public Action? GetStart(ref object sourceObject, ref Type sourceType)
        {
            var method = sourceType.GetMethod("Start", _BindingFlags);
            if (method == null)
                return null;
            return Delegate.CreateDelegate(typeof(Action), sourceObject, method) as Action;
        }


        //绑定对象注入
        public void InjectBindings(ref XComponentScriptBase component, object sourceObject, Type sourceType)
        {
            Type unityObjectType = typeof(UnityEngine.Object);
            Type gameObjectType = typeof(UnityEngine.GameObject);
            //Debug.Log("准备注入属性");
            foreach (var property in sourceType.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            {
                //Debug.Log("    " + property.Name+" , " + property.PropertyType.FullName);
                string bindName = property.Name;
                bool null_able = true;
                var attr = property.GetCustomAttribute<BindingAttribute>(true);
                if (attr != null)
                {
                    null_able = attr.Nullable;
                    if (!string.IsNullOrEmpty(attr.BindingName))
                        bindName = attr.BindingName;
                }

                if (property.PropertyType.IsSubclassOf(unityObjectType))
                {
                    if (component.TryGetBindingUObject(bindName, out var uobj))
                    {
                        Type __t_uobj = uobj.GetType();
                        if (__t_uobj.IsSubclassOf(property.PropertyType) || __t_uobj.IsAssignableFrom(property.PropertyType))
                        {
                            property.SetValue(sourceObject, uobj);
                            continue;
                        }
                        else
                        {
                            if (__t_uobj == gameObjectType) //如果绑定类型是GameObject,尝试寻找想要被注入的类型是不是这个GameObject上的Component
                            {
                                if (((UnityEngine.GameObject)uobj).TryGetComponent(property.PropertyType, out var _result))
                                {
                                    property.SetValue(sourceObject, _result);
                                    continue;
                                }
                            }
                        }
                    }
                }

                //BaseType Inject
                if (component.TryGetBindingType(bindName, out var obj))
                {
                    Type objType = obj.GetType();

                    //最简单的情况：直接相同类型
                    if (objType == property.PropertyType)
                    {
                        property.SetValue(sourceObject, obj);
                        continue;
                    }

                    //可空值类型
                    if(property.PropertyType.GetGenericTypeDefinition() == _NullableType)
                    {
                        var propertyType = property.PropertyType.GetGenericArguments()[0];
                        if(propertyType == objType)
                        {
                            property.SetValue(sourceObject, obj);
                            continue;
                        }
                    }

                }

                if (!null_able)
                    throw new Exception($"Inject binding object failed: cannot found binding object \"{bindName}\"");
            }

            //Debug.Log("准备注入字段");
            foreach (var field in sourceType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                //Debug.Log("    " + field.Name);
                if (field.IsPrivate) continue;
                string find_bind_name = field.Name;
                bool null_able = true;
                var attr = field.GetCustomAttribute<BindingAttribute>(true);
                if (attr != null)
                {
                    null_able = attr.Nullable;
                    if (!string.IsNullOrEmpty(attr.BindingName))
                        find_bind_name = attr.BindingName;
                }

                if (field.FieldType.IsSubclassOf(unityObjectType))
                {
                    if (component.TryGetBindingUObject(find_bind_name, out var uobj))
                    {
                        Type __t_uobj = uobj.GetType();
                        if (__t_uobj.IsSubclassOf(field.FieldType) || __t_uobj.IsAssignableFrom(field.FieldType))
                        {
                            field.SetValue(sourceObject, uobj);
                            continue;
                        }
                        else
                        {
                            if (__t_uobj == gameObjectType) //如果绑定类型是GameObject,尝试寻找想要被注入的类型是不是这个GameObject上的Component
                            {
                                if (((UnityEngine.GameObject)uobj).TryGetComponent(field.FieldType, out var _result))
                                {
                                    field.SetValue(sourceObject, _result);
                                    continue;
                                }
                            }
                        }
                    }
                }

                //BaseType Inject
                if (component.TryGetBindingType(find_bind_name, out var obj))
                {
                    Type objType = obj.GetType();

                    //最简单的情况：直接相同类型
                    if (objType == field.FieldType)
                    {
                        field.SetValue(sourceObject, obj);
                        continue;
                    }

                    //可空值类型
                    if (field.FieldType.GetGenericTypeDefinition() == _NullableType)
                    {
                        var propertyType = field.FieldType.GetGenericArguments()[0];
                        if (propertyType == objType)
                        {
                            field.SetValue(sourceObject, obj);
                            continue;
                        }
                    }
                }

                if (!null_able)
                    throw new Exception($"Inject binding object failed: cannot found binding object \"{find_bind_name}\"");
            }
        }

    }
#nullable restore
}
