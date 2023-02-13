/***
 *
 * @Author: Roman
 * @Created on: 11/02/23
 *
 ***/

using System;
using _3_Scripts.Data;
using _3_Scripts.Editor.Helpers;
using UnityEditor;
using UnityEngine;

namespace _3_Scripts.Editor.CustomEditors
{
    [CustomEditor(typeof(CharactersDatabase))]
    public class CharactersDatabaseEditor : UnityEditor.Editor
    {
        #region ## Fields ##

        // Editor-Only
        private static readonly EditorTab[] Tabs = 
        {
            new("Database", DrawDatabaseTab),
            new ("Configuration", DrawConfigurationTab)
        };

        // Serialized
        private static SerializedProperty _characterDirectories;
        private static SerializedProperty _materialsDirectory;
        private static SerializedProperty _prefabsDirectory;
        private static SerializedProperty _storeCharacters;
        private static SerializedProperty _defaultShader;
        private SerializedProperty _currentTab;

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
            EditorHelpers.Header("Characters Database", "Easily manage your characters in the game.");
            EditorHelpers.DrawTabs(ref _currentTab, Tabs);

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
            _characterDirectories = GetProperty("characterDirectories");
            _materialsDirectory = GetProperty("materialsDirectory");
            _prefabsDirectory = GetProperty("prefabsDirectory");
            _defaultShader = GetProperty("defaultShader");
            _storeCharacters = GetProperty("storeCharacters");
            _currentTab = GetProperty("currentTab");
        }

        #endregion

        #region ## Database Core ##

        private static void DrawDatabaseTab()
        {
            DrawCharacters();
        }
        
        private static void DrawCharacters()
        {
            EditorGUILayout.Space(10f);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Characters", EditorStyles.boldLabel, GUILayout.MaxWidth(120f));
            EditorGUILayout.EndHorizontal();

            // If there are no characters, draw a button to add the first one.
            if (_storeCharacters.arraySize.Equals(0))
            {
                if (GUILayout.Button("Add First Character"))
                {
                    _storeCharacters.InsertArrayElementAtIndex(0);
                }

                return;
            }

            // Otherwise, nicely draw each element.
            for (var i = 0; i < _storeCharacters.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Character {i}", EditorStyles.boldLabel, 
                    GUILayout.MaxWidth(EditorHelpers.AutoSizeText($"Character {i}   ")));
                if (GUILayout.Button("X", GUILayout.MaxWidth(20f)))
                {
                    _storeCharacters.DeleteArrayElementAtIndex(i);
                    return;
                }
                EditorGUILayout.EndHorizontal();

                var element = _storeCharacters.GetArrayElementAtIndex(i);
                var name = element.FindPropertyRelative("Name");
                var price = element.FindPropertyRelative("Price");
                var sprite = element.FindPropertyRelative("Icon");
                var prefab = element.FindPropertyRelative("Prefab");
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(name, new GUIContent("Name"));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(price, new GUIContent("Price"));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(sprite, new GUIContent("Icon"));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(prefab, new GUIContent("Prefab"));
                EditorGUILayout.EndHorizontal();
                
                
                // Draw a separator.
                EditorGUILayout.Space(10f);
            }
            
            // Draw a button to add a new element at the end.
            if (GUILayout.Button("Add New Character"))
            {
                _storeCharacters.InsertArrayElementAtIndex(_storeCharacters.arraySize);
            }
        }
        
        #endregion
        
        #region ## Configuration Core ##
        
        private static void DrawConfigurationTab()
        {
            DrawGraphicSettings();
            EditorGUILayout.Space(10f);
            DrawMainDirectories();
            EditorGUILayout.Space(10f);
            DrawDirectories();
            EditorGUILayout.Space(10f);
        }
        
        private static void DrawGraphicSettings()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Graphics", EditorStyles.boldLabel, 
                GUILayout.MaxWidth(EditorHelpers.AutoSizeText("Graphics ")));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_defaultShader, new GUIContent("Default Shader"));
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawMainDirectories()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Main Directories", EditorStyles.boldLabel, 
                GUILayout.MaxWidth(EditorHelpers.AutoSizeText("Main Directories ")));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_materialsDirectory, new GUIContent("Materials Directory"));
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_prefabsDirectory, new GUIContent("Prefabs Directory"));
            EditorGUILayout.EndHorizontal();
        }
        
        private static void DrawDirectories()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Character Directories", EditorStyles.boldLabel, 
                GUILayout.MaxWidth(EditorHelpers.AutoSizeText("Character Directories  ")));
            EditorGUILayout.EndHorizontal();

            // If there are no directories, draw a button to add the first one.
            if (_characterDirectories.arraySize.Equals(0))
            {
                if (GUILayout.Button("Add First Element"))
                {
                    _characterDirectories.InsertArrayElementAtIndex(0);
                }

                return;
            }
            
            // Otherwise, nicely draw each element.
            for (var i = 0; i < _characterDirectories.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"[{(i + 1).ToString()}]",
                    GUILayout.Width(EditorHelpers.AutoSizeText($"#{(i + 1).ToString()} ")));
                EditorGUILayout.PropertyField(_characterDirectories.GetArrayElementAtIndex(i), GUIContent.none);

                if (GUILayout.Button("X", GUILayout.Width(25f)))
                {
                    _characterDirectories.DeleteArrayElementAtIndex(i);
                }

                EditorGUILayout.EndHorizontal();
            }
            
            // Draw a button to add a new element at the end.
            if (GUILayout.Button("Add New Directory"))
            {
                _characterDirectories.InsertArrayElementAtIndex(_characterDirectories.arraySize);
            }
        }

        #endregion
    }
}