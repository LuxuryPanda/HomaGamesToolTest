using HomaGames.Internal.BaseTemplate;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceCounter : MonoBehaviour
{
    private Text _text;
    [SerializeField] string format = "{0:0}";
    [SerializeField] string propId = "Distance";

    void Start()
    {
        _text = GetComponent<Text>();
        GameManagerBase.Instance.Context.OnContextChanged += OnContextChanged;
    }

    private void OnContextChanged(GameContext obj)
    {
        _text.text = string.Format(format, obj.GetValue(propId));
    }
}
