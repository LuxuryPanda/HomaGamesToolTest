/***
 *
 * @Author: Roman
 * @Created on: 11/02/23
 *
 ***/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace _3_Scripts.Editor.Helpers
{
    public static class EditorHelpers
    {
        #region ## Header ##

        public static void Title(string title, float paddingTop = 15, float paddingBottom = 10)
        {
            GUILayout.Space(paddingTop);
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel,
                GUILayout.Width(AutoSizeText($"{title}   ")));
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(paddingBottom);
        }
        
        public static void Header(string title, string subtitle)
        {
            GUILayout.Space(10f);
            
            // Title
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel,
                GUILayout.Width(AutoSizeText($"{title}   ")));
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            // Subtitle
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(subtitle, GUILayout.Width(AutoSizeText($"{subtitle}")));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(10f);
        }

        /// <summary>
        /// Automatically scales the size of a label to horizontally fit the given text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static float AutoSizeText(string text)
        {
            return GUI.skin.label.CalcSize(new GUIContent(text)).x;
        }

        #endregion
        
        #region ## Tabs ##
        
        public static void DrawTabs(ref int selectedTab, EditorTab[] tabs)
        {
            selectedTab = GUILayout.Toolbar(selectedTab, tabs.Select(tab => tab.Name).ToArray());
            if (selectedTab >= 0 && selectedTab < tabs.Length)
            {
                tabs[selectedTab].OnTabSelected();
            }
        }
        
        public static void DrawTabs(ref SerializedProperty selectedTab, EditorTab[] tabs)
        {
            var names = tabs.Select(tab => tab.Name).ToArray();
            selectedTab.intValue = GUILayout.Toolbar(selectedTab.intValue, names);
            if (selectedTab.intValue >= 0 && selectedTab.intValue < tabs.Length)
            {
                tabs[selectedTab.intValue].OnTabSelected();
            }
        }
        
        #endregion

        #region ## Utils ##

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            var component = obj.GetComponent<T>();
            return component != null ? component : obj.gameObject.AddComponent<T>();
        }

        #endregion
    }
}