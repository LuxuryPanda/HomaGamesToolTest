/***
 *
 * @Author: Roman
 * @Created on: 12/02/23
 *
 ***/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3_Scripts.Editor.Data
{
    [CreateAssetMenu(fileName = "Data.AssetScanner.asset", menuName = "Homa/Data/New Asset Scanner Data", order = 0)]
    public class AssetScannerData : ScriptableObject
    {
        #region ## Fields ##
        
        [SerializeField] private bool requirePrefabNamings = true;
        [SerializeField] private bool requireTextureNamings = true;
        
        [SerializeField] private List<string> prefabNamings = new();
        [SerializeField] private List<string> textureNamings = new();

        [SerializeField] private List<string> prefabsPaths = new();
        [SerializeField] private List<string> texturesPaths = new();

        #endregion

        #region ## Properties ##
        
        public bool RequirePrefabNamings
        {
            get => requirePrefabNamings;
            set => requirePrefabNamings = value;
        }
        
        public bool RequireTextureNamings
        {
            get => requireTextureNamings;
            set => requireTextureNamings = value;
        }
        
        public List<string> PrefabNamings
        {
            get => prefabNamings;
            set => prefabNamings = value;
        }
        
        public List<string> TextureNamings
        {
            get => textureNamings;
            set => textureNamings = value;
        }
        
        public List<string> PrefabPaths
        {
            get => prefabsPaths;
            set => prefabsPaths = value;
        }
        
        public List<string> TexturePaths
        {
            get => texturesPaths;
            set => texturesPaths = value;
        }

        #endregion
    }
}