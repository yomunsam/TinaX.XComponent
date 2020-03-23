using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace TinaXEditor.XComponent.Internal
{
    internal static class BaseTypeGUIHandlerInternal
    {
        public static List<BaseTypeGUIHandler> Handlers => new List<BaseTypeGUIHandler>()
        {
            new BaseTypeGUIHandler("string")
            {
                DrawEditorGUI = (rect, source) =>
                {
                    return EditorGUI.TextField(rect,(string)source);
                },
                EditorGUIHeight = obj => EditorGUIUtility.singleLineHeight
            },
            new BaseTypeGUIHandler("int")
            {
                DrawEditorGUI = (rect, source) =>
                {
                    return EditorGUI.IntField(rect,(int)source);
                },
                EditorGUIHeight = obj => EditorGUIUtility.singleLineHeight
            },
            new BaseTypeGUIHandler("float")
            {
                DrawEditorGUI = (rect, source) =>
                {
                    return EditorGUI.FloatField(rect,(float)source);
                },
                EditorGUIHeight = obj => EditorGUIUtility.singleLineHeight
            },
            new BaseTypeGUIHandler("boolean")
            {
                DrawEditorGUI = (rect, source) =>
                {
                    return EditorGUI.Toggle(rect,(bool)source);
                },
                EditorGUIHeight = obj => EditorGUIUtility.singleLineHeight
            },
            new BaseTypeGUIHandler("array string")
            {
                DrawEditorGUI = (rect, source) =>
                {
                    List<string> arr = new List<string>((string[])source);
                    if(arr.Count > 0)
                    {
                        for(int i = 0; i < arr.Count; i++)
                        {
                            var _rect = rect;
                            _rect.y += EditorGUIUtility.singleLineHeight * i;
                            var _rect_del_btn = _rect;
                            _rect.width -= 40;
                            _rect_del_btn.width = 35;
                            _rect_del_btn.x += _rect.width+5;

                            arr[i] = EditorGUI.TextField(_rect,arr[i]);
                            if (GUI.Button(_rect_del_btn, "X"))
                            {
                                arr.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    var rect_add_btn = rect;
                    rect_add_btn.y += arr.Count * EditorGUIUtility.singleLineHeight;
                    rect_add_btn.width = 85;
                    if(GUI.Button(rect_add_btn,"Add Element"))
                    {
                        arr.Add(string.Empty);
                    }
                    return arr.ToArray();
                },
                EditorGUIHeight = obj =>
                {
                    int arr_len = 0;
                    if(obj != null)
                    {
                        string[] arr = (string[])obj;
                        arr_len = arr.Length;
                    }
                    return (arr_len + 1) * EditorGUIUtility.singleLineHeight;
                }
            },




            //Unitys---------------------------------------------
            new BaseTypeGUIHandler("Vector2")
            {
                DrawEditorGUI = (rect, source) =>
                {
                    Vector2 v2 = (Vector2)source;
                    v2 = EditorGUI.Vector2Field(rect,GUIContent.none,v2);
                    return v2;
                },
                EditorGUIHeight = obj =>EditorGUIUtility.singleLineHeight
            },
            new BaseTypeGUIHandler("Vector3")
            {
                DrawEditorGUI = (rect, source) =>
                {
                    Vector3 v3 = (Vector3)source;
                    v3 = EditorGUI.Vector3Field(rect,GUIContent.none,v3);
                    return v3;
                },
                EditorGUIHeight = obj =>EditorGUIUtility.singleLineHeight
            },
            new BaseTypeGUIHandler("Color")
            {
                DrawEditorGUI = (rect, source) =>
                {
                    Color myColor = (Color)source;
                    myColor = EditorGUI.ColorField(rect,myColor);
                    return myColor;
                },
                EditorGUIHeight = obj =>EditorGUIUtility.singleLineHeight
            },
            new BaseTypeGUIHandler("AnimationCurve")
            {
                DrawEditorGUI = (rect, source) =>
                {
                    AnimationCurve curve = (AnimationCurve)source;
                    curve = EditorGUI.CurveField(rect,curve);
                    return curve;
                },
                EditorGUIHeight = obj =>EditorGUIUtility.singleLineHeight
            },
        };
    }
}
