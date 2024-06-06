using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActivator : MonoBehaviour
{
    [SerializeField] private GameObject[] _maps;
    [SerializeField] private Initializator _initializator;

    public void ChangeActivityMaps()
    {
        for (int i = 0; i < _maps.Length; i++)
        {
            if (i != _initializator.Index)
                _maps[i].SetActive(false);
        }
    }

    public void ActivateAllMaps()
    {
        foreach (var map in _maps)
        {
            map.SetActive(true);
        }
    }

    public void DeactivationAllMaps()
    {
        foreach (var map in _maps)
        {
            map.SetActive(false);
        }
    }
}