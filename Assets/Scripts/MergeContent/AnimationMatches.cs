using System.Collections.Generic;
using ItemContent;
using UnityEngine;

namespace MergeContent
{
    public class AnimationMatches : MonoBehaviour
    {
        [SerializeField] private LookMerger _lookMerger;

        public void StopMoveMatch()
        {
            foreach (var matchItem in _lookMerger.ItemsMoving)
                matchItem.StopMove();
        }

        public void StartMoveMatch(Vector3 target)
        {
            foreach (var matchItem in _lookMerger.ItemsMoving)
                matchItem.MoveCyclically(target);
        }

        public void StartMoveTarget(Item item, Vector3 target,List<ItemMoving>itemMoving)
        {
            
            foreach (var matchItem in itemMoving)
                matchItem.MoveTarget(target);
            
            /*foreach (var matchItem in _lookMerger.ItemsMoving)
                matchItem.MoveTarget(target);*/
            
            foreach (var matchItem in itemMoving)
                Debug.Log(matchItem.name);

            item.GetComponent<ItemMoving>().MoveTarget(target);
        }
    }
}