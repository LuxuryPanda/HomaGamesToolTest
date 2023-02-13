/***
 *
 * @Author: Roman
 * @Created on: 10/02/23
 *
 ***/

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using _3_Scripts.Data;
using _3_Scripts.Editor.Helpers;

namespace _3_Scripts.Editor.Windows
{
    public class CharacterBuilderWindow : EditorWindow
    {
        #region ## Fields ##

        private static readonly EditorTab[] Tabs =
        {
            new("Creation", DrawCreationTab),
            new("Edit", DrawEditTab)
        };

        // Character Graphics
        private static GameObject _characterModel;
        private static Shader _characterShader;
        private static List<Material> _characterMaterials;
        private static int _characterNumberOfMaterials;
        private static AnimatorController _characterAnimatorController;

        // Character Data
        private static string _characterName;
        private static int _characterPrice;
        private static Sprite _characterIcon;

        // Material Creation
        private static bool _showMaterialCreation;
        private static string _newMaterialName;
        private static Color _newMaterialColor;
        private static List<(string, Texture)> _newMaterialTexture;

        // Core
        private static CharactersDatabase _charactersDatabase;
        private int _currentTab = 0;

        #endregion

        #region ## Window Core ##

        [MenuItem("HomaGames/Character Builder", false, 0)]
        private static void ShowWindow()
        {
            var window = GetWindow<CharacterBuilderWindow>();
            window.titleContent = new GUIContent("Character Builder");
            Initialize();
            window.Show();
        }

        #endregion

        #region ## Initialization ##

        private static void Initialize()
        {
            _charactersDatabase = Resources.Load<CharactersDatabase>("Database.Characters");
            _characterShader = Shader.Find(_charactersDatabase.DefaultShader);
        }

        #endregion

        #region ## GUI Core ##

        private void OnGUI()
        {
            if (_charactersDatabase == null)
            {
                Initialize();
            }

            EditorHelpers.Header("Character Builder", "Easily create new characters!");
            EditorHelpers.DrawTabs(ref _currentTab, Tabs);
        }

        #endregion

        #region ## Tabs Core ##

        private static void DrawCreationTab()
        {
            EditorHelpers.Title("Model Configuration", 5);
            EditorGUILayout.Space();

            #region ## Model ##

            var shouldRebuildMaterials = false;

            EditorGUI.BeginChangeCheck();
            _characterModel =
                (GameObject)EditorGUILayout.ObjectField("Character model", _characterModel, typeof(GameObject), false);

            EditorGUILayout.Space(10);

            // In case the model changes, we need to rebuild the materials
            if (EditorGUI.EndChangeCheck())
            {
                shouldRebuildMaterials = true;
            }

            // Check if the object is a model
            if (_characterModel == null || PrefabUtility.GetPrefabAssetType(_characterModel) != PrefabAssetType.Model)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("You need to select a model for the character.", MessageType.Error);
                return;
            }

            #endregion

            #region ## Materials ##

            // Are the materials different from what we already have?
            if (shouldRebuildMaterials || _characterNumberOfMaterials == 0)
            {
                var prefab = PrefabUtility.InstantiatePrefab(_characterModel) as GameObject;
                // This should never happen, but just in case.
                if (prefab == null) return;

                _characterNumberOfMaterials = prefab.GetComponentInChildren<Renderer>().sharedMaterials.Length;
                DestroyImmediate(prefab);
                return;
            }

            // Once we have the number of materials, we need to make sure we have the correct amount of slots
            if (_characterMaterials == null || _characterMaterials.Count != _characterNumberOfMaterials)
            {
                _characterMaterials = new List<Material>(_characterNumberOfMaterials);
                for (int i = 0; i < _characterNumberOfMaterials; i++)
                {
                    _characterMaterials.Add(null);
                }
            }

            EditorHelpers.Title("Materials Configuration");

            // Draw the material slots
            for (int i = 0; i < _characterMaterials.Count; i++)
            {
                _characterMaterials[i] =
                    (Material)EditorGUILayout.ObjectField($"Material [{i}]", _characterMaterials[i], typeof(Material),
                        false);
            }

            // If any of the slots are empty, we can suggest to create a new material.
            if (_characterMaterials.Exists(x => x == null))
            {
                EditorGUILayout.Space(10f);
                if (GUILayout.Button("Create new Material"))
                {
                    _showMaterialCreation = !_showMaterialCreation;
                }

                EditorGUILayout.Space();
                EditorGUILayout.HelpBox(
                    "Some materials are missing. Please, create them or assign existing ones.",
                    MessageType.None);
            }
            else
            {
                // If all the materials are assigned, we can hide the material creation window
                _showMaterialCreation = false;
            }

            if (_showMaterialCreation)
            {
                DrawMaterialCreation();
            }

            #endregion

            #region ## Animator ##

            EditorHelpers.Title("Animator Configuration");

            _characterAnimatorController =
                (AnimatorController)EditorGUILayout.ObjectField("Animator Controller", _characterAnimatorController,
                    typeof(AnimatorController), false);

            #endregion

            #region ## Character Data ##

            EditorHelpers.Title("Character Data");

            _characterName = EditorGUILayout.TextField("Character Name", _characterName);
            _characterPrice = EditorGUILayout.IntField("Character Price", _characterPrice);
            _characterIcon =
                (Sprite)EditorGUILayout.ObjectField("Character Icon", _characterIcon, typeof(Sprite), false);

            #endregion

            #region ## Save ##

            EditorGUILayout.Space(15);

            if (CanSaveCharacter())
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Create"))
                {
                    SaveCharacter();
                }

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.HelpBox("Please, fill all the fields to create the character", MessageType.None);
            }

            #endregion
        }

        private static void DrawEditTab()
        {
            EditorGUILayout.LabelField(
                "This was planned to be a feature, but I then realized there is no need to since you can " +
                "edit characters directly from the database.");
        }

        #endregion

        #region ## Core ##

        private static bool CanSaveCharacter()
        {
            if (_characterModel == null || PrefabUtility.GetPrefabAssetType(_characterModel) != PrefabAssetType.Model)
            {
                return false;
            }

            if (_characterMaterials == null || _characterMaterials.Any(x => x == null))
            {
                return false;
            }

            if (_characterAnimatorController == null)
            {
                return false;
            }

            if (_characterPrice <= 0)
            {
                return false;
            }

            if (_characterIcon == null)
            {
                return false;
            }

            return true;
        }

        private static void CreatePrefab()
        {
            var prefab = PrefabUtility.InstantiatePrefab(_characterModel) as GameObject;
            // This should never happen, but just in case.
            if (prefab == null) return;

            var renderer = prefab.GetComponentInChildren<Renderer>();
            renderer.sharedMaterials = _characterMaterials.ToArray();

            var animator = prefab.GetComponentInChildren<Animator>();
            animator.runtimeAnimatorController = _characterAnimatorController;

            var collider = prefab.GetOrAddComponent<CapsuleCollider>();
            collider.center = new Vector3(0, 0.55f, 0);
            collider.radius = 0.2f;
            collider.height = 1.1f;

            if (_characterName.IsNullOrWhiteSpace())
            {
                _characterName = _characterModel.name;
            }

            var prefabPath = _charactersDatabase.PrefabsDirectory + _characterName + ".prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null)
            {
                if (!EditorUtility.DisplayDialog("Duplicate prefab",
                        "A character with the same name is already present in the prefabs folder. " +
                        "Do you want to continue?", "Yes", "No"))
                {
                    return;
                }
            }

            PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
            DestroyImmediate(prefab);
        }

        private static void SaveCharacter()
        {
            CreatePrefab();
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(_charactersDatabase.PrefabsDirectory + _characterName +
                                                          ".prefab");
            if (_charactersDatabase.AddNewCharacter(_characterName, _characterPrice, _characterIcon, prefab))
            {
                EditorUtility.DisplayDialog("Character created", "The character was created successfully", "Ok");
            }
            else
            {
                EditorUtility.DisplayDialog("Character not created", "The character could not be created", "Ok");
                return;
            }

            EditorUtility.SetDirty(_charactersDatabase);
            AssetDatabase.SaveAssets();
        }

        #endregion

        #region ## Drawers ##

        private static void DrawMaterialCreation()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Material Creation", EditorStyles.boldLabel,
                GUILayout.MaxWidth(EditorHelpers.AutoSizeText("Material Creation ")));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            _characterShader = (Shader)EditorGUILayout.ObjectField("Shader", _characterShader, typeof(Shader), false);
            _newMaterialName = EditorGUILayout.TextField("Material Name", _newMaterialName);
            _newMaterialColor = EditorGUILayout.ColorField("Material Color", _newMaterialColor);
            if (_characterShader == null)
            {
                EditorGUILayout.HelpBox("A shader is required to create a character.", MessageType.Error);
                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Material properties", EditorStyles.largeLabel);
            EditorGUILayout.Space();

            if (_characterShader == null || EditorGUI.EndChangeCheck())
            {
                if (_characterShader == null)
                {
                    _characterShader = Shader.Find(_charactersDatabase.DefaultShader);
                }

                _newMaterialTexture = new List<(string, Texture)>();
                for (int i = 0; i < ShaderUtil.GetPropertyCount(_characterShader); i++)
                {
                    if (ShaderUtil.GetPropertyType(_characterShader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                    {
                        _newMaterialTexture.Add((ShaderUtil.GetPropertyName(_characterShader, i), null));
                    }
                }
            }

            for (int i = 0; i < ShaderUtil.GetPropertyCount(_characterShader); i++)
            {
                if (ShaderUtil.GetPropertyType(_characterShader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    var property = ShaderUtil.GetPropertyName(_characterShader, i);
                    var index = _newMaterialTexture.FindIndex(x => x.Item1 == property);
                    var texture =
                        (Texture)EditorGUILayout.ObjectField(ShaderUtil.GetPropertyDescription(_characterShader, i),
                            _newMaterialTexture[index].Item2, typeof(Texture), false);

                    _newMaterialTexture[index] = (property, texture);
                }
            }

            if (_newMaterialName.IsNullOrWhiteSpace())
            {
                EditorGUILayout.HelpBox("A material name is required!", MessageType.Error);
                return;
            }

            if (GUILayout.Button("Create Material"))
            {
                var material = CreateMaterial(_newMaterialColor, _newMaterialTexture, _characterShader,
                    _newMaterialName);
                Selection.SetActiveObjectWithContext(material, material);
                _showMaterialCreation = false;
            }
        }

        private static Material CreateMaterial(Color color, List<(string, Texture)> textures, Shader shader,
            string materialName)
        {
            var material = new Material(shader)
            {
                name = materialName,
                color = color
            };

            foreach (var texture in textures)
            {
                material.SetTexture(texture.Item1, texture.Item2);
            }

            var materialPath = _charactersDatabase.MaterialsDirectory + materialName + ".mat";
            if (AssetDatabase.LoadAssetAtPath<Material>(materialPath) != null)
            {
                if (EditorUtility.DisplayDialog("Duplicate Material",
                        "A material with the same name is already present in the materials folder. " +
                        "Do you still want to continue?", "Yes", "No"))
                {
                }
                else
                {
                    return null;
                }
            }

            AssetDatabase.CreateAsset(material, materialPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return material;
        }

        #endregion
    }
}