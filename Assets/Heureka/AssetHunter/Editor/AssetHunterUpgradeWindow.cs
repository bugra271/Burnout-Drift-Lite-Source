using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace HeurekaGames.AssetHunter
{
    public class AssetHunterUpgradeWindow : EditorWindow
    {
        GUIStyle LinkStyle { get { return m_LinkStyle; } }
        [SerializeField] GUIStyle m_LinkStyle;

        GUIStyle TitleStyle { get { return m_TitleStyle; } }
        [SerializeField] GUIStyle m_TitleStyle;

        GUIStyle HeadingStyle { get { return m_HeadingStyle; } }
        [SerializeField] GUIStyle m_HeadingStyle;

        GUIStyle BodyStyle { get { return m_BodyStyle; } }
        [SerializeField] GUIStyle m_BodyStyle;

        bool initialized;

        public static void Init()
        {
            bool hasKey = EditorPrefs.HasKey(AssetHunterUpgradeTester.AllowUpdateWindowPrefKey);
            bool windowAllowed = EditorPrefs.GetInt(AssetHunterUpgradeTester.AllowUpdateWindowPrefKey) == 1;

            if (hasKey && windowAllowed)
                return;

            EditorPrefs.SetInt(AssetHunterUpgradeTester.UpdateWindowWasShown, 1);

            int windowWidth = 500;
            int windowHeight = 300;
            Rect windowRect = new Rect(Screen.width * .5f - (windowWidth * .5f), Screen.height * .5f - (windowHeight * .5f), windowWidth, windowHeight);
            AssetHunterUpgradeWindow window = GetWindowWithRect<AssetHunterUpgradeWindow>(windowRect, true, "MAJOR UPGRADE AVALIABLE", true);

            window.initialized = true;
        }

        private void initializeStyles()
        {
            m_BodyStyle = new GUIStyle(GUI.skin.label);
            m_BodyStyle.wordWrap = true;
            m_BodyStyle.fontSize = 12;

            m_TitleStyle = new GUIStyle(m_BodyStyle);
            m_TitleStyle.wordWrap = true;
            m_TitleStyle.fontSize = 18;

            m_HeadingStyle = new GUIStyle(m_BodyStyle);
            m_HeadingStyle.wordWrap = true;
            m_HeadingStyle.fontSize = 14;

            m_LinkStyle = new GUIStyle(m_BodyStyle);
            m_LinkStyle.wordWrap = true;
            // Match selection color which works nicely for both light and dark skins
            m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
            m_LinkStyle.stretchWidth = false;
        }

        bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
        {
            var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

            Handles.BeginGUI();
            Handles.color = LinkStyle.normal.textColor;
            Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
            Handles.color = Color.white;
            Handles.EndGUI();

            EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

            return GUI.Button(position, label, LinkStyle);
        }

        private void OnGUI()
        {
            if (!initialized)
            {
                Init();
                return;
            }
            if (TitleStyle == null)
                initializeStyles();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("!!Asset Hunter PRO now avaliable!!", TitleStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Asset Hunter PRO is a complete rewrite of Asset Hunter with new features, improved UI, workflow and huge performance upgrades", BodyStyle);
            GUILayout.Label("The Asset Hunter that has been avaliable for you since 2012 came to a point where I wanted to do a complete rewrite in order to keep evolving the featureset", BodyStyle);
            GUILayout.Label("The upgrade is free for all new customers from within 6 months of release, all other customers will be able to download for a minor fee (40% of normal price)", BodyStyle);

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (LinkLabel(new GUIContent("Download from Asset Store")))
            {
                Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/135296");
            }
            GUILayout.FlexibleSpace();
            bool allowWindow = EditorPrefs.GetInt(AssetHunterUpgradeTester.AllowUpdateWindowPrefKey, 0) == 1 ? true : false;
            EditorPrefs.SetInt(AssetHunterUpgradeTester.AllowUpdateWindowPrefKey, GUILayout.Toggle(allowWindow, "Dont show again") ? 1 : 0);
            GUILayout.EndHorizontal();
        }

    }

    [InitializeOnLoad]
    class AssetHunterUpgradeTester
    {
        public static readonly string UpdateWindowWasShown = "AH.UpdateWindowWasShown";
        public static readonly string AllowUpdateWindowPrefKey = "AH.AllowUpdateWindow";

        static AssetHunterUpgradeTester()
        {
            runShowUpdateInfoTest();
        }

        private static void runShowUpdateInfoTest()
        {
            bool windowWasShown = EditorPrefs.GetInt(UpdateWindowWasShown) == 1;

            //Make sure we haven't chosen NOT to import
            if (windowWasShown)
                return;
            else
                AssetHunterUpgradeWindow.Init();
        }
    }
}