/***
 *
 * @Author: Roman
 * @Created on: 11/02/23
 *
 * @Copyright (c) by BorysProductions 2022. All rights reserved.
 *
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using _3_Scripts.Data;
using _3_Scripts.Editor.Data;
using _3_Scripts.Editor.Helpers;
using UnityEditor;
using UnityEngine;

namespace _3_Scripts.Editor.Windows
{
    public class AssetScannerWindow : EditorWindow
    {
        #region ## Fields ##
        
        private static readonly EditorTab[] Tabs =
        {
            new("Scanner", DrawScannerTab),
            new("Configuration", DrawConfigurationTab)
        };
        
        /// <summary>
        /// The main paths to search for assets in, used if no custom paths are provided.
        /// </summary>
        private static List<string> _assetPaths = new()
        {
            "Assets/",
        };

        /// <summary>
        /// The main extensions to search for assets in, used if no custom extensions are provided.
        /// </summary>
        private static List<string> _assetExtensions = new()
        {
            "prefab", "FBX", "png", "jpg", "jpeg"
        };

        private static Vector2 _currentPrefabScrollPosition = Vector2.zero;
        private static Vector2 _currentTexturesScrollPosition = Vector2.zero;
        private static Vector2 _currentAssetsScrollPosition = Vector2.zero;
        
        private static CharactersDatabase _charactersDatabase;
        private static AssetScannerData _assetScannerData;
        
        private int _currentTab = 0;
        private static UnityEditor.Editor _assetScannerDataEditor;

        #endregion

        #region ## Window Core ##

        [MenuItem("HomaGames/Asset Scanner", false, 1)]
        private static void ShowWindow()
        {
            AssetScannerWindow window = (AssetScannerWindow)EditorWindow.GetWindow(typeof(AssetScannerWindow));
            window.titleContent = new GUIContent("Asset Scanner");
            Initialize();
            window.Show();
        }

        #endregion
        
        #region ## Initialization ##

        private static void Initialize()
        {
            _charactersDatabase = Resources.Load<CharactersDatabase>("Database.Characters");
            _assetScannerData = Resources.Load<AssetScannerData>("Data.AssetScanner");
        }

        #endregion

        #region ## GUI Core ##

        private void OnGUI()
        {
            if (_assetScannerData == null || _charactersDatabase == null)
            {
                Initialize();
            }

            EditorHelpers.Header("Assets Scanner", "Identify forgotten assets in the project!");
            EditorHelpers.DrawTabs(ref _currentTab, Tabs);
        }

        #endregion

        #region ## Tabs Core ##
        
        private static void DrawScannerTab()
        {
            EditorGUILayout.Space(20f);
            if (GUILayout.Button("Run Asset Scan"))
            {
                SearchPrefabs();
                SearchTextures();
            }
            
            EditorGUILayout.Space(5);
            DrawPrefabs();
            
            EditorGUILayout.Space(5);
            DrawTextures();
        }

        private static void DrawConfigurationTab()
        {
            if (_assetScannerDataEditor == null)
            {
                _assetScannerDataEditor = UnityEditor.Editor.CreateEditor(_assetScannerData);
            }
            
            _assetScannerDataEditor.OnInspectorGUI();
        }
        
        #endregion

        #region ## Drawers ##
        
        private static void DrawPrefabs()
        {
            EditorGUILayout.LabelField("Prefabs:");
            if (_assetScannerData.PrefabPaths.Count == 0)
            {
                EditorGUILayout.LabelField("No prefab found!");
                return;
            }
            
            _currentPrefabScrollPosition = EditorGUILayout.BeginScrollView(_currentPrefabScrollPosition, GUILayout.Width(500), GUILayout.Height(150));
            foreach (string prefabPath in _assetScannerData.PrefabPaths)
            {
                GameObject prefab = (GameObject) AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
                if (prefab == null) continue;
                EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
            }
            EditorGUILayout.EndScrollView();
        }
        
        private static void DrawTextures()
        {
            EditorGUILayout.LabelField("Textures:");
            if (_assetScannerData.TexturePaths.Count == 0)
            {
                EditorGUILayout.LabelField("No texture found");
                return;
            }
            
            _currentTexturesScrollPosition = EditorGUILayout.BeginScrollView(_currentTexturesScrollPosition, GUILayout.Width(500), GUILayout.Height(150));
            foreach (string texturePath in _assetScannerData.TexturePaths)
            {
                Texture2D texture = (Texture2D) AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
                if (texture == null) continue;
                EditorGUILayout.ObjectField(texture, typeof(Texture2D), false);
            }
            EditorGUILayout.EndScrollView();
        }

        #endregion

        #region ## Asset Finding Core ##
        
        private static void SearchPrefabs()
        {
            _assetScannerData.PrefabPaths = AssetDatabase.GetAllAssetPaths().Where(s => s.StartsWith("Assets/"))
                .Where(s => s.EndsWith(".prefab")).ToList();
            
            // Filter out the prefabs that are already in the database
            var filteredPaths = new List<string>();
            for (int i = 0; i < _assetScannerData.PrefabPaths.Count; i++)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(_assetScannerData.PrefabPaths[i]);
                if (prefab == null) continue;
                
                foreach (var prefabNaming in _assetScannerData.PrefabNamings)
                {
                    if (!prefab.name.Contains(prefabNaming) && _assetScannerData.RequirePrefabNamings) continue;
                    
                    // Filter out the prefabs that are already in the database
                    if (_charactersDatabase.StoreCharacters.Any(c => c.Prefab == prefab) == false) 
                    {
                        filteredPaths.Add(_assetScannerData.PrefabPaths[i]);
                        break;
                    }
                }
            }

            _assetScannerData.PrefabPaths = filteredPaths;
        }
        
        private static void SearchTextures()
        {
            _assetScannerData.TexturePaths = AssetDatabase.GetAllAssetPaths().Where(s => s.StartsWith("Assets/"))
                .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".jpeg")).ToList();
            
            // Get the textures that follow the naming configuration
            var filteredPaths = new List<string>();
            for (var i = 0; i < _assetScannerData.TexturePaths.Count; i++)
            {
                var path = _assetScannerData.TexturePaths[i];
                // If we are required to use the naming configuration, we filter out the textures that don't follow it
                if (_assetScannerData.RequireTextureNamings)
                {
                    foreach (var assetNaming in _assetScannerData.TextureNamings)
                    {
                        if (path.Contains(assetNaming) && !path.Contains("Editor"))
                        {
                            filteredPaths.Add(path);
                            break;
                        }
                    }
                }
                else
                {
                    filteredPaths.Add(path);
                }
            }
            
            _assetScannerData.TexturePaths = filteredPaths;
        }

        #endregion
    }
}