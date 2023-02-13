/***
 *
 * @Author: Roman
 * @Created on: 13/02/23
 *
 * @Copyright (c) by BorysProductions 2022. All rights reserved.
 *
 ***/

using UnityEditor;
using _3_Scripts.Editor.Data;
using _3_Scripts.Editor.Helpers;
using UnityEngine;

namespace _3_Scripts.Editor.CustomEditors
{
    [CustomEditor(typeof(AssetScannerData))]
    public class AssetScannerDataEditor : UnityEditor.Editor
    {
        #region ## Fields ##

        // Serialized
        private static SerializedProperty _requirePrefabNamings;
        private static SerializedProperty _requireTextureNamings;
        private static SerializedProperty _prefabNamings;
        private static SerializedProperty _textureNamings;
        
        private static Vector2 _currentPrefabNamingsScrollPosition = Vector2.zero;
        private static Vector2 _currentTextureNamingsScrollPosition = Vector2.zero;

        #endregion

        #region ## Properties ##



        #endregion

        #region ## Unity Lifecycle ##

        private void Awake()
        {
            CollectSerializedProperties();
        }

        #endregion

        #region ## GUI Core ##

        public override void OnInspectorGUI()
        {
            EditorHelpers.Header("Asset Scanner Data", "Configuration for the Asset Scanner.");

            DrawConfiguration();
            
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        #endregion

        #region ## Core ##
        
        private SerializedProperty GetProperty(string propertyName)
        {
            return serializedObject.FindProperty(propertyName);
        }
        
        private void CollectSerializedProperties()
        {
            _requirePrefabNamings = GetProperty("requirePrefabNamings");
            _requireTextureNamings = GetProperty("requireTextureNamings");
            _prefabNamings = GetProperty("prefabNamings");
            _textureNamings = GetProperty("textureNamings");
        }

        #endregion

        #region ## Drawer Core ##

        private static void DrawConfiguration()
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel, 
                GUILayout.MaxWidth(EditorHelpers.AutoSizeText("Configuration  ")));
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(5);
            _requirePrefabNamings.boolValue = EditorGUILayout.Toggle("Require Prefab Namings", _requirePrefabNamings.boolValue);
            _requireTextureNamings.boolValue = EditorGUILayout.Toggle("Require Texture Namings", _requireTextureNamings.boolValue);
            
            EditorGUILayout.HelpBox("Asset Namings are used to identify which assets are used for characters." +
                                    "They should be prefixes, or suffixes.", MessageType.Info);
            EditorGUILayout.Space(5);
            
            DrawNamings(_prefabNamings, "Prefab Namings", "Prefab Naming", ref _currentPrefabNamingsScrollPosition);
            EditorGUILayout.Space(10f);
            DrawNamings(_textureNamings, "Texture Namings", "Texture Naming", ref _currentTextureNamingsScrollPosition);
        }

        private static void DrawNamings(SerializedProperty property, string title, string generalName, ref Vector2 scrollPosition)
        {
            // If there are no characters, draw a button to add the first one.
            if (property.arraySize.Equals(0))
            {
                if (GUILayout.Button($"Add First {generalName}"))
                {
                    property.InsertArrayElementAtIndex(0);
                }

                return;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(450), GUILayout.Height(150));
            // Otherwise, nicely draw each element.
            for (var i = 0; i < property.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{title} {i}", EditorStyles.boldLabel, 
                    GUILayout.MaxWidth(EditorHelpers.AutoSizeText($"{title} {i}   ")));
                if (GUILayout.Button("X", GUILayout.MaxWidth(20f)))
                {
                    property.DeleteArrayElementAtIndex(i);
                    return;
                }
                EditorGUILayout.EndHorizontal();

                var element = property.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(element, new GUIContent("Name"));
                EditorGUILayout.EndHorizontal();


                // Draw a separator.
                EditorGUILayout.Space(10f);
            }
            EditorGUILayout.EndScrollView();
            
            // Draw a button to add a new element at the end.
            if (GUILayout.Button($"Add New {generalName}"))
            {
                property.InsertArrayElementAtIndex(property.arraySize);
            }
        }
        
        #endregion
    }
}