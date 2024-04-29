using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPosition : MonoBehaviour
{
    private bool _isBusy = false;

    public bool IsBusy => _isBusy;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Item item))
            _isBusy = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Item item))
            _isBusy = false;
    }
}