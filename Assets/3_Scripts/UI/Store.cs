using HomaGames.Internal.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using _3_Scripts.Data;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class StoreItem
{
    public int Id;
    public string Name;
    public int Price;
    public Sprite Icon;
    public GameObject Prefab;
}

public class Store : Singleton<Store>
{
    #region ## Fields ##
    [Header("References")]
    [SerializeField] private CharactersDatabase charactersDatabase;

    #endregion
    
    #region ## Properties ##
    
    public List<StoreItem> StoreItems => charactersDatabase.StoreCharacters;

    #endregion
    
    #region ## Events ##
    
    public Action<StoreItem> OnItemSelected;
    
    #endregion
    
    #region ## Core ##
    
    public void SelectItem(StoreItem item)
    {
        OnItemSelected?.Invoke(item);
    }
    
    #endregion
}
