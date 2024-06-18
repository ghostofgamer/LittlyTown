using System.Collections;
using System.Collections.Generic;
using ItemPositionContent;
using UnityEngine;

public class SandBoxEnvironment : MonoBehaviour
{
    [SerializeField] private ItemPosition[] _itemPositions;

    private void Start()
    {
        foreach (var itemPosition in _itemPositions)
            itemPosition.GetComponent<FinderPositions>().FindNeighbor();

        gameObject.SetActive(false);
    }
}
