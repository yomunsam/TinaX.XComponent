using System;
using System.Collections.Generic;
using System.Linq;
using TinaX.XComponent;
using TinaX.XComponent.Internal;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TinaXEditor.XComponent.GUICustom
{
    [CustomEditor(typeof(TinaX.XComponent.XComponentScriptBase),true)]
    public class XComponentScriptBaseCustom : Editor
    {
        private protected int mToolbar_Index;
        private protected string[] mToolbar_Text;

        private bool mChanged = false;

        private ReorderableList mReorderableList_UObject;
        private List<UnityObjectBindingInfo> mUObject_Cache;

        private ReorderableList mReorderableList_Types;


        private TinaX.XComponent.XComponentScriptBase _target;

        private string[] mBaseTypeNames;
        private Dictionary<string, int> mDict_baseTypeName;

        private bool refresh_data = false;

        private void refreshData()
        {
            if (mToolbar_Text == null || mToolbar_Text.Length == 0)
                mToolbar_Text = I18Ns.Toolbar_title;

            _target = (TinaX.XComponent.XComponentScriptBase)this.target;

            mBaseTypeNames = XComponents.GetTypeNames();
            mDict_baseTypeName = new Dictionary<string, int>();
            refresh_data = true;
        }

        private void OnDisable()
        {
            mReorderableList_UObject = null;
        }

        public override void OnInspectorGUI()
        {
            if (!refresh_data)
                refreshData();
            //base.OnInspectorGUI(); //Debug的时候用的

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(target.name, EditorStyles.centeredGreyMiniLabel);
            mToolbar_Index = GUILayout.Toolbar(mToolbar_Index, mToolbar_Text);
            GUILayout.Space(4);
            if(mToolbar_Index == 0)
            {
                DrawUnityObjectBindingGUI();
            }
            else if(mToolbar_Index == 1)
            {
                DrawTypeObjectBindingGUI();
            }
            GUILayout.Space(5);
            if (GUILayout.Button(I18Ns.OpenEditWindow))
            {
#if UNITY_2021_1_OR_NEWER
                EditorUtility.OpenPropertyEditor(_target);
#else
                XComponentBindingEditWindows.target = _target;
                XComponentBindingEditWindows.OpenUI();
#endif

            }

            EditorGUILayout.EndVertical();

            if (this.serializedObject.hasModifiedProperties)
                mChanged = true;
            else
                mChanged = false;
            this.serializedObject.ApplyModifiedProperties();
            if (mChanged)
            {
                mChanged = false;
                OnChanged();
            }
        }


        private void DrawUnityObjectBindingGUI()
        {
            if (mReorderableList_UObject == null)
            {
                mReorderableList_UObject = new ReorderableList(this.serializedObject,
                    this.serializedObject.FindProperty("UObjectBindInfos"),
                    true, //draggable
                    true, //display header 
                    true,
                    true);
                mReorderableList_UObject.drawElementCallback = (rect, index, selected, focused) =>
                {
                    rect.height = EditorGUIUtility.singleLineHeight;
                    rect.y += 2;
                    SerializedProperty itemData = mReorderableList_UObject.serializedProperty.GetArrayElementAtIndex(index);
                    SerializedProperty item_name = itemData.FindPropertyRelative("Name");
                    SerializedProperty item_object = itemData.FindPropertyRelative("Object");
                    var rect_name = rect;
                    rect_name.width = (rect.width - 5) / 2 - 3;
                    var rect_object = rect;
                    rect_object.width = (rect.width - 5) / 2 + 4;
                    rect_object.x += (rect.width - 5) / 2 + 3;

                    item_name.stringValue = EditorGUI.TextField(rect_name, item_name.stringValue);

                    item_object.objectReferenceValue = EditorGUI.ObjectField(rect_object, item_object.objectReferenceValue, typeof(UnityEngine.Object), true);
                };
                mReorderableList_UObject.onAddCallback = (list) =>
                {
                    if (list.serializedProperty != null)
                    {
                        list.serializedProperty.arraySize++;
                        list.index = list.serializedProperty.arraySize - 1;

                        SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
                        SerializedProperty item_name = itemData.FindPropertyRelative("Name");
                        SerializedProperty item_object = itemData.FindPropertyRelative("Object");
                        item_name.stringValue = null;
                        item_object.objectReferenceValue = null;
                    }
                    else
                    {
                        ReorderableList.defaultBehaviours.DoAddButton(list);
                    }
                };
                mReorderableList_UObject.drawHeaderCallback = (rect) =>
                {
                    GUI.Label(rect, I18Ns.UObject_Head);
                };
            }
            mReorderableList_UObject.DoLayoutList();
            if(mUObject_Cache == null)
            {
                mUObject_Cache = new List<UnityObjectBindingInfo>();
                if(_target.UObjectBindInfos != null)
                {
                    mUObject_Cache.AddRange(mUObject_Cache);
                }
            }
        }

        private void DrawTypeObjectBindingGUI()
        {
            if (mReorderableList_Types == null)
            {
                mReorderableList_Types = new ReorderableList(this.serializedObject,
                    this.serializedObject.FindProperty("TypeBindInfos"),
                    true, //draggable
                    false, //display header 
                    true,
                    true);
                mReorderableList_Types.drawHeaderCallback = rect =>
                {
                    GUI.Label(rect, I18Ns.Types_Head);
                };
                mReorderableList_Types.drawElementCallback = (rect, index, selected, focused) =>
                {
                    rect.height = EditorGUIUtility.singleLineHeight;
                    rect.y += 2;
                    SerializedProperty itemData = mReorderableList_Types.serializedProperty.GetArrayElementAtIndex(index);
                    SerializedProperty item_typeName = itemData.FindPropertyRelative("TypeName");
                    string typeName = item_typeName.stringValue;
                    SerializedProperty item_name = itemData.FindPropertyRelative("Name");
                    
                    var rect_name = rect;
                    var rect_type = rect;
                    rect_name.width = (rect.width - 5) / 2 - 3;
                    rect_type.width = (rect.width - 5) / 2 + 4;
                    rect_type.x += (rect.width - 5) / 2 + 3;

                    //rect_type.y += EditorGUIUtility.singleLineHeight + 2;

                    //Name ----------------------
                    item_name.stringValue = EditorGUI.TextField(rect_name, item_name.stringValue);

                    //Type Name ------------------
                    int type_index = -1;
                    if (string.IsNullOrEmpty(typeName))
                    {
                        //type_index = EditorGUI.Popup(rect_type, type_index, mBaseTypeNames);
                    }
                    else
                    {
                        if (!mDict_baseTypeName.TryGetValue(typeName, out type_index))
                        {
                            for (int i = 0; i < mBaseTypeNames.Length; i++)
                            {
                                if (mBaseTypeNames[i] == typeName)
                                {
                                    type_index = i;
                                    mDict_baseTypeName.Add(typeName, type_index);
                                    break;
                                }
                            }
                        }
                    }
                    type_index = EditorGUI.Popup(rect_type, type_index, mBaseTypeNames);
                    if(type_index != -1 && type_index <= mBaseTypeNames.Length - 1)
                    {
                        item_typeName.stringValue = mBaseTypeNames[type_index];
                        typeName = mBaseTypeNames[type_index];
                    }
                    else
                    {
                        typeName = string.Empty;
                    }

                    var rect_edit = rect;
                    rect_edit.y += EditorGUIUtility.singleLineHeight + 2;
                    // detail
                    if (!string.IsNullOrEmpty(typeName))
                    {
                        var info = _target.TypeBindInfos[index];
                        if(XComponents.TryGetHandler(typeName,out var typeHandler))
                        {
                            if(XComponentsEditor.TryGetGUIHandler(typeName,out var guiHandler))
                            {
                                var value = typeHandler.GetValueFunc(info);
                                typeHandler.SetValueFunc(guiHandler.DrawEditorGUI(rect_edit, value), info);
                            }
                        }
                    }
                    else
                    {
                        EditorGUI.LabelField(rect_edit, "Unknow Type");
                    }
                    
                    
                };
                mReorderableList_Types.onAddCallback = (list) =>
                {
                    if (list.serializedProperty != null)
                    {
                        list.serializedProperty.arraySize++;
                        list.index = list.serializedProperty.arraySize - 1;

                        SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
                        SerializedProperty item_name = itemData.FindPropertyRelative("Name");
                        SerializedProperty item_type = itemData.FindPropertyRelative("TypeName");
                        SerializedProperty item_arr_str = itemData.FindPropertyRelative("Value_String");
                        SerializedProperty item_arr_double = itemData.FindPropertyRelative("Value_Double");
                        SerializedProperty item_arr_long = itemData.FindPropertyRelative("Value_long");
                        SerializedProperty item_arr_curve = itemData.FindPropertyRelative("Value_Curve");
                        SerializedProperty item_bool = itemData.FindPropertyRelative("Value_bool");

                        item_name.stringValue = null;
                        item_type.stringValue = null;
                        item_arr_str.ClearArray();
                        item_arr_double.ClearArray();
                        item_arr_long.ClearArray();
                        item_arr_curve.animationCurveValue = null;
                        item_bool.boolValue = false;
                    }
                    else
                    {
                        ReorderableList.defaultBehaviours.DoAddButton(list);
                    }
                };
                mReorderableList_Types.elementHeightCallback  = index=>
                {
                    SerializedProperty itemData = mReorderableList_Types.serializedProperty.GetArrayElementAtIndex(index);
                    var info = _target.TypeBindInfos[index];
                    if(XComponents.TryGetValue(info, out object value))
                    {
                        SerializedProperty type = itemData.FindPropertyRelative("TypeName");
                        if (XComponentsEditor.TryGetGUIHandler(type.stringValue, out var handler))
                        {
                            return handler.EditorGUIHeight(value) + (EditorGUIUtility.singleLineHeight) + 6;
                        }
                    }
                    
                    return EditorGUIUtility.singleLineHeight * 2 + 6;
                };
            }
            mReorderableList_Types.DoLayoutList();
        }

        private void OnChanged()
        {
            HandleIfUObjectChanged();
        }

        /// <summary>
        /// 如果UObject的列表有变动，则进行处理
        /// </summary>
        /// <returns></returns>
        private void HandleIfUObjectChanged()
        {
            if (mReorderableList_UObject == null) return;
            if (_target.UObjectBindInfos == null) return; //它为null了也可能是修改后的结果，但是我们不需要才处理这种结果
            if (mUObject_Cache == null)
            {
                mUObject_Cache = new List<UnityObjectBindingInfo>();
                if (_target.UObjectBindInfos != null)
                    mUObject_Cache.AddRange(_target.UObjectBindInfos);
                return;
            }
            //对比
            for(var i = 0; i< _target.UObjectBindInfos.Length; i++)
            {
                if(string.IsNullOrEmpty(_target.UObjectBindInfos[i].Name) && _target.UObjectBindInfos[i].Object != null)
                {
                    if(!mUObject_Cache.Any(item => string.IsNullOrEmpty(item.Name) && item.Object == _target.UObjectBindInfos[i].Object ))
                    {
                        //自动命名
                        if (TryGetUObjectPrefix(_target.UObjectBindInfos[i].Object, out var prefix))
                        {
                            if (!string.IsNullOrEmpty(prefix))
                                _target.UObjectBindInfos[i].Name = $"{prefix}_{_target.UObjectBindInfos[i].Object.name}";
                            else
                                _target.UObjectBindInfos[i].Name = _target.UObjectBindInfos[i].Object.name;
                        }
                        else
                            _target.UObjectBindInfos[i].Name = _target.UObjectBindInfos[i].Object.name;
                    }
                }
            }
            mUObject_Cache.Clear();
            if (_target.UObjectBindInfos != null)
                mUObject_Cache.AddRange(_target.UObjectBindInfos);
        }


        private bool TryGetUObjectPrefix(UnityEngine.Object obj, out string prefix)
        {
            if (InternalUObjectTypePrefix.TryGetValue(obj.GetType(), out prefix))
                return true;

            prefix = string.Empty;
            return false;
        }

        /// <summary>
        /// 内置的GameObject前缀
        /// </summary>
        private Dictionary<Type, string> InternalUObjectTypePrefix = new Dictionary<Type, string>()
        {
            {typeof(GameObject),"go" },
            {typeof(Transform),"trans" },
            {typeof(RectTransform),"rectTrans" },
            {typeof(UnityEngine.UI.Text),"txt" },
            {typeof(UnityEngine.UI.Image),"img" },
            {typeof(UnityEngine.UI.Button),"btn" },
        };

        private static class Styles
        {
            private static GUIStyle _bg;

            public static GUIStyle Background
            {
                get
                {
                    if(_bg == null)
                    {
                        _bg = new GUIStyle(EditorStyles.helpBox);
                    }
                    return _bg;
                }
            }
        }

        internal static class I18Ns
        {
            private static bool? _isChinese;
            private static bool IsChinese
            {
                get
                {
                    if (_isChinese == null)
                    {
                        _isChinese = (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified);
                    }
                    return _isChinese.Value;
                }
            }

            private static bool? _nihongo_desuka;
            private static bool NihongoDesuka
            {
                get
                {
                    if (_nihongo_desuka == null)
                        _nihongo_desuka = (Application.systemLanguage == SystemLanguage.Japanese);
                    return _nihongo_desuka.Value;
                }
            }

            public static string[] Toolbar_title
            {
                get
                {
                    if (IsChinese)
                        return new string[] { "Unity对象", "类型绑定" };
                    return new string[] { "Unity Object", "Types" };
                }
            }

            public static string UObject_Head
            {
                get
                {
                    if (IsChinese) return "绑定Unity对象";
                    return "Bind unity object";
                }
            }

            public static string Types_Head
            {
                get
                {
                    if (IsChinese) return "常用基础值类型绑定";
                    return "Common base value type binding";
                }
            }

            public static string OpenEditWindow
            {
                get
                {
                    if (IsChinese) return "打开独立编辑窗口";
                    if (NihongoDesuka) return "編集ウィンドウを開く";
                    return "Open edit window";
                }
            }

        }

    }

    public class XComponentBindingEditWindows : EditorWindow
    {
        public static TinaX.XComponent.XComponentScriptBase target;

        private static XComponentBindingEditWindows wnd;

        public static void OpenUI()
        {
            if(wnd == null)
            {
                wnd = GetWindow<XComponentBindingEditWindows>();
            }
            else
            {
                wnd.Show();
                wnd.Focus();
            }
            wnd.titleContent = new GUIContent("Binding - " + target.name);
        }

        private SerializedObject serializedObject;

        private protected int mToolbar_Index;
        private protected string[] mToolbar_Text;

        private bool mChanged = false;


        private ReorderableList mReorderableList_UObject;
        private List<UnityObjectBindingInfo> mUObject_Cache;

        private ReorderableList mReorderableList_Types;


        private string[] mBaseTypeNames;
        private Dictionary<string, int> mDict_baseTypeName;

        private Vector2 v2_scroll_uobj;
        private Vector2 v2_scroll_types;

        private void OnEnable()
        {
            if(target != null)
            {
                serializedObject = new SerializedObject(target);
                if (mToolbar_Text == null || mToolbar_Text.Length == 0)
                    mToolbar_Text = I18Ns.Toolbar_title;


                mBaseTypeNames = XComponents.GetTypeNames();
                mDict_baseTypeName = new Dictionary<string, int>();
            }
        }


        private void OnGUI()
        {
            if(target == null)
            {
                GUILayout.Label("Object reference is missing, please reopen the editing window");
            }
            else
            {
                #region Invoke custom obj
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField(target.name, EditorStyles.centeredGreyMiniLabel);
                mToolbar_Index = GUILayout.Toolbar(mToolbar_Index, mToolbar_Text);
                GUILayout.Space(4);
                if (mToolbar_Index == 0)
                {
                    DrawUnityObjectBindingGUI();
                }
                else if (mToolbar_Index == 1)
                {
                    DrawTypeObjectBindingGUI();
                }
                GUILayout.Space(5);
                
                EditorGUILayout.EndVertical();

                if (serializedObject.hasModifiedProperties)
                    mChanged = true;
                else
                    mChanged = false;
                serializedObject.ApplyModifiedProperties();
                if (mChanged)
                {
                    mChanged = false;
                    OnChanged();
                }

                #endregion
            }
        }

        private void OnDestroy()
        {
            wnd = null;
        }


        private void DrawUnityObjectBindingGUI()
        {
            v2_scroll_uobj = EditorGUILayout.BeginScrollView(v2_scroll_uobj);
            if (mReorderableList_UObject == null)
            {
                mReorderableList_UObject = new ReorderableList(this.serializedObject,
                    this.serializedObject.FindProperty("UObjectBindInfos"),
                    true, //draggable
                    false, //display header 
                    true,
                    true);
                mReorderableList_UObject.drawHeaderCallback = (rect) =>
                {
                    GUI.Label(rect, I18Ns.UObject_Head);
                };
                mReorderableList_UObject.drawElementCallback = (rect, index, selected, focused) =>
                {
                    rect.height = EditorGUIUtility.singleLineHeight;
                    rect.y += 2;
                    SerializedProperty itemData = mReorderableList_UObject.serializedProperty.GetArrayElementAtIndex(index);
                    SerializedProperty item_name = itemData.FindPropertyRelative("Name");
                    SerializedProperty item_object = itemData.FindPropertyRelative("Object");
                    var rect_name = rect;
                    rect_name.width = (rect.width - 5) / 2 - 3;
                    var rect_object = rect;
                    rect_object.width = (rect.width - 5) / 2 + 4;
                    rect_object.x += (rect.width - 5) / 2 + 3;

                    item_name.stringValue = EditorGUI.TextField(rect_name, item_name.stringValue);

                    item_object.objectReferenceValue = EditorGUI.ObjectField(rect_object, item_object.objectReferenceValue, typeof(UnityEngine.Object), true);
                };
                mReorderableList_UObject.onAddCallback = (list) =>
                {
                    if (list.serializedProperty != null)
                    {
                        list.serializedProperty.arraySize++;
                        list.index = list.serializedProperty.arraySize - 1;

                        SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
                        SerializedProperty item_name = itemData.FindPropertyRelative("Name");
                        SerializedProperty item_object = itemData.FindPropertyRelative("Object");
                        item_name.stringValue = null;
                        item_object.objectReferenceValue = null;
                    }
                    else
                    {
                        ReorderableList.defaultBehaviours.DoAddButton(list);
                    }
                };
            }
            mReorderableList_UObject.DoLayoutList();
            if (mUObject_Cache == null)
            {
                mUObject_Cache = new List<UnityObjectBindingInfo>();
                if (target.UObjectBindInfos != null)
                {
                    mUObject_Cache.AddRange(mUObject_Cache);
                }
            }


            EditorGUILayout.EndScrollView();
        }

        private void DrawTypeObjectBindingGUI()
        {
            v2_scroll_types = EditorGUILayout.BeginScrollView(v2_scroll_types);
            if (mReorderableList_Types == null)
            {
                mReorderableList_Types = new ReorderableList(this.serializedObject,
                    this.serializedObject.FindProperty("TypeBindInfos"),
                    true, //draggable
                    false, //display header 
                    true,
                    true);
                mReorderableList_Types.drawHeaderCallback = rect =>
                {
                    GUI.Label(rect, I18Ns.Types_Head);
                };
                mReorderableList_Types.drawElementCallback = (rect, index, selected, focused) =>
                {
                    rect.height = EditorGUIUtility.singleLineHeight;
                    rect.y += 2;
                    SerializedProperty itemData = mReorderableList_Types.serializedProperty.GetArrayElementAtIndex(index);
                    SerializedProperty item_typeName = itemData.FindPropertyRelative("TypeName");
                    string typeName = item_typeName.stringValue;
                    SerializedProperty item_name = itemData.FindPropertyRelative("Name");

                    var rect_name = rect;
                    var rect_type = rect;
                    rect_name.width = (rect.width - 5) / 2 - 3;
                    rect_type.width = (rect.width - 5) / 2 + 4;
                    rect_type.x += (rect.width - 5) / 2 + 3;

                    //rect_type.y += EditorGUIUtility.singleLineHeight + 2;

                    //Name ----------------------
                    item_name.stringValue = EditorGUI.TextField(rect_name, item_name.stringValue);

                    //Type Name ------------------
                    int type_index = -1;
                    if (string.IsNullOrEmpty(typeName))
                    {
                        //type_index = EditorGUI.Popup(rect_type, type_index, mBaseTypeNames);
                    }
                    else
                    {
                        if (!mDict_baseTypeName.TryGetValue(typeName, out type_index))
                        {
                            for (int i = 0; i < mBaseTypeNames.Length; i++)
                            {
                                if (mBaseTypeNames[i] == typeName)
                                {
                                    type_index = i;
                                    mDict_baseTypeName.Add(typeName, type_index);
                                    break;
                                }
                            }
                        }
                    }
                    type_index = EditorGUI.Popup(rect_type, type_index, mBaseTypeNames);
                    if (type_index != -1 && type_index <= mBaseTypeNames.Length - 1)
                    {
                        item_typeName.stringValue = mBaseTypeNames[type_index];
                        typeName = mBaseTypeNames[type_index];
                    }
                    else
                    {
                        typeName = string.Empty;
                    }

                    var rect_edit = rect;
                    rect_edit.y += EditorGUIUtility.singleLineHeight + 2;
                    // detail
                    if (!string.IsNullOrEmpty(typeName))
                    {
                        var info = target.TypeBindInfos[index];
                        if (XComponents.TryGetHandler(typeName, out var typeHandler))
                        {
                            if (XComponentsEditor.TryGetGUIHandler(typeName, out var guiHandler))
                            {
                                var value = typeHandler.GetValueFunc(info);
                                typeHandler.SetValueFunc(guiHandler.DrawEditorGUI(rect_edit, value), info);
                            }
                        }
                    }
                    else
                    {
                        EditorGUI.LabelField(rect_edit, "Unknow Type");
                    }


                };
                mReorderableList_Types.onAddCallback = (list) =>
                {
                    if (list.serializedProperty != null)
                    {
                        list.serializedProperty.arraySize++;
                        list.index = list.serializedProperty.arraySize - 1;

                        SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
                        SerializedProperty item_name = itemData.FindPropertyRelative("Name");
                        SerializedProperty item_type = itemData.FindPropertyRelative("TypeName");
                        SerializedProperty item_arr_str = itemData.FindPropertyRelative("Value_String");
                        SerializedProperty item_arr_double = itemData.FindPropertyRelative("Value_Double");
                        SerializedProperty item_arr_long = itemData.FindPropertyRelative("Value_long");
                        SerializedProperty item_arr_curve = itemData.FindPropertyRelative("Value_Curve");
                        SerializedProperty item_bool = itemData.FindPropertyRelative("Value_bool");


                        item_name.stringValue = null;
                        item_type.stringValue = null;
                        item_arr_str.ClearArray();
                        item_arr_double.ClearArray();
                        item_arr_long.ClearArray();
                        item_arr_curve.animationCurveValue = null;
                        item_bool.boolValue = false;
                    }
                    else
                    {
                        ReorderableList.defaultBehaviours.DoAddButton(list);
                    }
                };
                mReorderableList_Types.elementHeightCallback = index =>
                {
                    SerializedProperty itemData = mReorderableList_Types.serializedProperty.GetArrayElementAtIndex(index);
                    var info = target.TypeBindInfos[index];
                    if (XComponents.TryGetValue(info, out object value))
                    {
                        SerializedProperty type = itemData.FindPropertyRelative("TypeName");
                        if (XComponentsEditor.TryGetGUIHandler(type.stringValue, out var handler))
                        {
                            return handler.EditorGUIHeight(value) + (EditorGUIUtility.singleLineHeight) + 6;
                        }
                    }

                    return EditorGUIUtility.singleLineHeight * 2 + 6;
                };
            }
            mReorderableList_Types.DoLayoutList();

            EditorGUILayout.EndScrollView();
        }

        private void OnChanged()
        {
            HandleIfUObjectChanged();
        }

        /// <summary>
        /// 如果UObject的列表有变动，则进行处理
        /// </summary>
        /// <returns></returns>
        private void HandleIfUObjectChanged()
        {
            if (mReorderableList_UObject == null) return;
            if (target.UObjectBindInfos == null) return; //它为null了也可能是修改后的结果，但是我们不需要才处理这种结果
            if (mUObject_Cache == null)
            {
                mUObject_Cache = new List<UnityObjectBindingInfo>();
                if (target.UObjectBindInfos != null)
                    mUObject_Cache.AddRange(target.UObjectBindInfos);
                return;
            }
            //对比
            for (var i = 0; i < target.UObjectBindInfos.Length; i++)
            {
                if (string.IsNullOrEmpty(target.UObjectBindInfos[i].Name) && target.UObjectBindInfos[i].Object != null)
                {
                    if (!mUObject_Cache.Any(item => string.IsNullOrEmpty(item.Name) && item.Object == target.UObjectBindInfos[i].Object))
                    {
                        //自动命名
                        if (TryGetUObjectPrefix(target.UObjectBindInfos[i].Object, out var prefix))
                        {
                            if (!string.IsNullOrEmpty(prefix))
                                target.UObjectBindInfos[i].Name = $"{prefix}_{target.UObjectBindInfos[i].Object.name}";
                            else
                                target.UObjectBindInfos[i].Name = target.UObjectBindInfos[i].Object.name;
                        }
                        else
                            target.UObjectBindInfos[i].Name = target.UObjectBindInfos[i].Object.name;
                        this.serializedObject.Update();
                    }
                }
            }
            mUObject_Cache.Clear();
            if (target.UObjectBindInfos != null)
                mUObject_Cache.AddRange(target.UObjectBindInfos);
        }


        private bool TryGetUObjectPrefix(UnityEngine.Object obj, out string prefix)
        {
            if (InternalUObjectTypePrefix.TryGetValue(obj.GetType(), out prefix))
                return true;

            prefix = string.Empty;
            return false;
        }

        /// <summary>
        /// 内置的GameObject前缀
        /// </summary>
        private Dictionary<Type, string> InternalUObjectTypePrefix = new Dictionary<Type, string>()
        {
            {typeof(GameObject),"go" },
            {typeof(Transform),"trans" },
            {typeof(RectTransform),"rectTrans" },
            {typeof(UnityEngine.UI.Text),"txt" },
            {typeof(UnityEngine.UI.Image),"img" },
            {typeof(UnityEngine.UI.Button),"btn" },
        };


        internal static class I18Ns
        {
            private static bool? _isChinese;
            private static bool IsChinese
            {
                get
                {
                    if (_isChinese == null)
                    {
                        _isChinese = (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified);
                    }
                    return _isChinese.Value;
                }
            }

            private static bool? _nihongo_desuka;
            private static bool NihongoDesuka
            {
                get
                {
                    if (_nihongo_desuka == null)
                        _nihongo_desuka = (Application.systemLanguage == SystemLanguage.Japanese);
                    return _nihongo_desuka.Value;
                }
            }

            public static string[] Toolbar_title
            {
                get
                {
                    if (IsChinese)
                        return new string[] { "Unity对象", "类型绑定" };
                    return new string[] { "Unity Object", "Types" };
                }
            }

            public static string UObject_Head
            {
                get
                {
                    if (IsChinese) return "绑定Unity对象";
                    return "Bind unity object";
                }
            }

            
            public static string Types_Head
            {
                get
                {
                    if (IsChinese) return "常用基础值类型绑定";
                    return "Common base value type binding";
                }
            }

            public static string OpenEditWindow
            {
                get
                {
                    if (IsChinese) return "打开独立编辑窗口";
                    if (NihongoDesuka) return "編集ウィンドウを開く";
                    return "Open edit window";
                }
            }

        }
    }

}

