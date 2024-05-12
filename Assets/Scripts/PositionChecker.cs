using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ItemContent;
using ItemPositionContent;
using UnityEngine;
using Random = UnityEngine.Random;

public class PositionChecker : MonoBehaviour
{
    [SerializeField] private ItemPosition[] _positions;
    
    public event Action PositionsFilled;
    private ItemPosition _currentPosition;

    /*public ItemPosition GetPosition()
    {
    }*/
    
    private IEnumerator GetPositionCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        List<ItemPosition> freePositions = _positions.Where(p => !p.GetComponent<ItemPosition>().IsBusy).ToList();

        if (freePositions.Count <= 0)
        {
            PositionsFilled?.Invoke();
            _currentPosition = null;
            yield break; 
        }

        int randomIndex = Random.Range(0, freePositions.Count);
        _currentPosition = freePositions[randomIndex];
        yield return _currentPosition;
    }
}
