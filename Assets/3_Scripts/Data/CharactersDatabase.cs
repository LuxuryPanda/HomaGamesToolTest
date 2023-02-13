/***
 *
 * @Author: Roman
 * @Created on: 10/02/23
 *
 ***/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3_Scripts.Data
{
    [CreateAssetMenu(fileName = "Database.Characters.asset", menuName = "Homa/Data/New Characters Database", order = 0)]
    public class CharactersDatabase : ScriptableObject
    {
        #region ## Fields ##
        
        [SerializeField] private List<StoreItem> storeCharacters = new();
        
        /// <summary>
        /// The paths to search for characters in, unused since the characters are now stored as StoreItems.
        /// </summary>
        [SerializeField] private List<string> characterDirectories = new();
        [SerializeField] private string defaultShader = "Toon/Lit";
        [SerializeField] private string materialsDirectory = "Assets/1_Graphics/Materials/";
        [SerializeField] private string prefabsDirectory = "Assets/2_Prefabs/";


        /// <summary>
        /// Variable used to store the current tab in the editor, so it can be restored when the editor is reopened.
        /// </summary>
        [SerializeField] private int currentTab = 0;

        #endregion

        #region ## Properties ##

        public List<StoreItem> StoreCharacters => storeCharacters;

        public string DefaultShader => defaultShader;
        
        public string MaterialsDirectory => materialsDirectory;
        
        public string PrefabsDirectory => prefabsDirectory;

        #endregion

        #region ## Core ##
        
        public bool AddNewCharacter(string characterName, int price, Sprite icon, GameObject prefab)
        {
            if (storeCharacters.Exists(character => character.Name == characterName))
            {
                return false;
            }

            storeCharacters.Add(new StoreItem
            {
                Id = storeCharacters.Count + 1,
                Name = characterName,
                Price = price,
                Icon = icon,
                Prefab = prefab
            });
            
            return true;
        }

        #endregion
    }
}