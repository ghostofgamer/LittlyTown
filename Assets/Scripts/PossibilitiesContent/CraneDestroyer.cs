using System.Collections;
using ItemContent;
using UnityEngine;

namespace PossibilitiesContent
{
    public class CraneDestroyer : MonoBehaviour
    {
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.3f);
        private BoxCollider _boxCollider;
        private Item _item;

        private void Start()
        {
            _boxCollider=GetComponent<BoxCollider>();
            _item = GetComponent<Item>();
        }

        public void Destroy()
        {
            if(_coroutine!=null)
                StopCoroutine(_coroutine);
        
            _coroutine = StartCoroutine(StartDestroy());
        }

        private IEnumerator StartDestroy()
        {
            _boxCollider.enabled = false;
            _item.ItemPosition.ClearingPosition();
            yield return _waitForSeconds;
            gameObject.SetActive(false);
        }
    }
}