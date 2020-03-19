using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TinaX.XComponent.Internal
{
    static class BaseTypeHandlerListInternal
    {
        public static BaseTypeHandler[] Handler => new BaseTypeHandler[]
        {
            new BaseTypeHandler("string")
            {
                SetValueFunc = (obj, info) =>
                {
                    info.Value_String = new string[1]{((obj is string)?(string)obj:string.Empty)};
                },
                GetValueFunc = (info) =>
                {
                    if(info.Value_String != null && info.Value_String.Length > 0)
                        return info.Value_String[0];
                    else
                        return string.Empty;
                }
            },
            new BaseTypeHandler("int")
            {
                SetValueFunc = (obj, info) =>
                {
                    if(obj is int)
                        info.Value_long = new long[1]{(int)obj};
                    else
                        info.Value_long = new long[1]{0};
                },
                GetValueFunc = info =>
                {
                    if(info.Value_long != null && info.Value_long.Length > 0)
                        return (int)info.Value_long[0];
                    else
                        return (int)0;
                }
            },
            new BaseTypeHandler("float")
            {
                SetValueFunc = (obj, info) =>
                {
                    info.Value_Double = new double[1]{(obj is float)?(float)obj:0f};
                },
                GetValueFunc = info =>
                {
                    if(info.Value_Double != null && info.Value_Double.Length > 0)
                        return (float)info.Value_Double[0];
                    else
                        return 0f;
                }
            },
            new BaseTypeHandler("array string")
            {
                SetValueFunc = (obj, info) =>
                {
                    info.Value_String = (string[])obj;
                },
                GetValueFunc = info =>
                {
                    if(info.Value_String == null)
                        return new string[0];
                    else
                        return info.Value_String;
                }
            },

            //Unitys
            new BaseTypeHandler("Vector2")
            {
                SetValueFunc = (obj, info) =>
                {
                    Vector2 v2 = (Vector2)obj;
                    info.Value_Double = new double[2]{v2.x,v2.y};
                },
                GetValueFunc = info =>
                {
                    if(info.Value_Double!= null && info.Value_Double.Length > 1)
                        return new Vector2((float)info.Value_Double[0],(float)info.Value_Double[1]);
                    else
                        return Vector2.zero;
                }
            },
            new BaseTypeHandler("Vector3")
            {
                SetValueFunc = (obj, info) =>
                {
                    Vector3 v3 = (Vector3)obj;
                    info.Value_Double = new double[3]{v3.x,v3.y,v3.z};
                },
                GetValueFunc = info =>
                {
                    if(info.Value_Double!= null && info.Value_Double.Length > 2)
                        return new Vector3((float)info.Value_Double[0],(float)info.Value_Double[1],(float)info.Value_Double[2]);
                    else
                        return Vector3.zero;
                }
            },
            new BaseTypeHandler("Color")
            {
                SetValueFunc = (obj, info) =>
                {
                    Color color = (Color)obj;
                    info.Value_Double = new double[4]{color.r,color.g,color.b,color.a};
                },
                GetValueFunc = info =>
                {
                    if(info.Value_Double!= null && info.Value_Double.Length > 3)
                        return new Color((float)info.Value_Double[0],(float)info.Value_Double[1],(float)info.Value_Double[2],(float)info.Value_Double[3]);
                    else
                        return Color.black;
                }
            },
            new BaseTypeHandler("AnimationCurve")
            {
                SetValueFunc = (obj, info) =>
                {
                    AnimationCurve curve = (AnimationCurve)obj;
                    info.Value_Curve = curve;
                },
                GetValueFunc = info =>
                {
                    return info.Value_Curve;
                }
            },
        };
    }
}
