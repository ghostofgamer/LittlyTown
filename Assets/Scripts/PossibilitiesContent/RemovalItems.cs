using System;
using System.Collections;
using CountersContent;
using ItemContent;
using ItemPositionContent;
using UI.Buttons.BonusesContent;
using UnityEngine;

namespace PossibilitiesContent
{
    public class RemovalItems : MonoBehaviour
    {
        [SerializeField] private DeleteButton _deleteButton;
        [SerializeField] private PossibilitiesCounter _positionsCounter;
        [SerializeField] private AudioSource _audioSource;

        private int _layerMask;
        private int _layer = 3;
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.15f);

        public event Action Removed;

        public event Action Removing;

        public event Action<Item> ItemRemoved;

        public bool IsWorking { get; private set; }

        private void OnEnable()
        {
            _deleteButton.RemovalActivated += ActivateWork;
            _deleteButton.RemovalDeactivated += DeactivateWork;
        }

        private void OnDisable()
        {
            _deleteButton.RemovalActivated -= ActivateWork;
            _deleteButton.RemovalDeactivated -= DeactivateWork;
        }

        private void Start()
        {
            _layerMask = 1 << _layer;
            _layerMask = ~_layerMask;
        }

        public void ActivateAnimation()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if ((Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask)))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (itemPosition.IsBusy && !itemPosition.IsWater)
                    {
                        itemPosition.Item.GetComponent<ItemAnimation>().BusyPositionAnimation();
                        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
                    }
                    else
                    {
                        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
                    }
                }
            }
        }

        public void StartRemove()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Remove());
        }

        private IEnumerator Remove()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if ((Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask)))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition) && itemPosition.IsBusy)
                {
                    ItemRemoved?.Invoke(itemPosition.Item);
                    itemPosition.Item.Deactivation();
                    itemPosition.Item.gameObject.SetActive(false);
                    itemPosition.ClearingPosition();
                    _audioSource.PlayOneShot(_audioSource.clip);
                    yield return null;
                    _positionsCounter.DecreaseCount();
                    Removing?.Invoke();
                    yield return _waitForSeconds;
                    Removed?.Invoke();
                }
            }
        }

        private void ActivateWork()
        {
            IsWorking = true;
        }

        private void DeactivateWork()
        {
            IsWorking = false;
        }
    }
}