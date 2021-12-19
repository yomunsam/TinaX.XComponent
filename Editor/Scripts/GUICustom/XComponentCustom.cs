using System;
using TinaX.XComponent.Warpper;
using TinaXEditor.Core.Utils.Localization;
using UnityEditor;
using UnityEngine;

namespace TinaXEditor.XComponent.GUICustom
{
    [CustomEditor(typeof(TinaX.XComponent.XComponent))]
    public class XComponentCustom : XComponentScriptBaseCustom
    {
        private TinaX.XComponent.XComponent _xComponentTarget;

        private static LocalizerCustom Localizer;
        private static StylesCustom Styles;

        private XBehaviourWarpper m_xBehaviourWarpper;

        /// <summary>
        /// 渲染“侧载模式”提示UI
        /// </summary>
        protected bool RenderSideLoadModeGUI { get; set; } = true;

        void OnEnable()
        {
            if(Localizer == null)
                Localizer = new LocalizerCustom();

            if(Styles == null)
                Styles = new StylesCustom();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(_xComponentTarget == null )
                _xComponentTarget = target as TinaX.XComponent.XComponent;

            if(RenderSideLoadModeGUI && Application.isPlaying && _xComponentTarget.Behaviour != null && _xComponentTarget.Behaviour is XBehaviourWarpper)
            {
                if(m_xBehaviourWarpper == null)
                {
                    m_xBehaviourWarpper = _xComponentTarget.Behaviour as XBehaviourWarpper;
                }

                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical("PreBackground");
                EditorGUILayout.LabelField(Localizer.SideLoadMode, Styles.Title);
                EditorGUILayout.DelayedTextField(m_xBehaviourWarpper.SourceType.FullName,EditorStyles.textField);
                EditorGUILayout.EndVertical();
            }
        }



        class LocalizerCustom
        {
            bool IsHans = EditorLocalizationUtil.IsHans();
            bool IsJp = EditorLocalizationUtil.IsJapanese();

            public string SideLoadMode
            {
                get
                {
                    if (IsHans)
                        return "侧载模式";
                    if (IsJp)
                        return "サイドロードモード";
                    return "Side load mode";
                }
            }
        }

        class StylesCustom
        {
            bool IsHans = EditorLocalizationUtil.IsHans();
            bool IsJp = EditorLocalizationUtil.IsJapanese();

            public GUIStyle Title
            {
                get
                {
                    if (_title == null)
                    {
                        _title = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
                        if (IsHans || IsJp)
                            _title.fontSize += 2;
                        else
                            _title.fontSize += 1;
                    }
                    return _title;
                }
            }

            private GUIStyle _title;
        }
    }




}
