using System;
using System.Collections;
using CountersContent;
using ItemContent;
using ItemPositionContent;
using UI.Buttons.BonusesContent;
using UnityEngine;

public class RemovalItems : MonoBehaviour
{
    [SerializeField] private DeleteButton _deleteButton;
    [SerializeField] private PossibilitiesCounter _positionsCounter;
    [SerializeField]private AudioSource _audioSource;
    
    private bool _isWorking;
    private bool _isLooking;
    private int _layerMask;
    private int _layer = 3;
    private Coroutine _coroutine;

    public event Action Removed;

    public event Action<Item> ItemRemoved;

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

    private void Update()
    {
        if (!_isWorking)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _isLooking = true;
        }

        if (_isLooking)
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

        if (Input.GetMouseButtonUp(0))
        {
            _isLooking = false;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Remove());
        }
    }

    private IEnumerator Remove()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if ((Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask)))
        {
            if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition) && itemPosition.IsBusy)
            {
                /*if (itemPosition.Item.IsLightHouse)
                {
                    LightHouseTrigger lightHouse = itemPosition.Item.GetComponent<LightHouseTrigger>();
                    lightHouse.RemoveHouses();
                    Debug.Log("Удаляем маяк");
                }*/

                ItemRemoved?.Invoke(itemPosition.Item);
                itemPosition.Item.Deactivation();
                itemPosition.Item.gameObject.SetActive(false);
                itemPosition.ClearingPosition();
                _audioSource.PlayOneShot(_audioSource.clip);
                yield return null;
                _positionsCounter.DecreaseCount();
                Removed?.Invoke();
            }
        }
    }

    private void ActivateWork()
    {
        _isWorking = true;
    }

    private void DeactivateWork()
    {
        _isWorking = false;
    }
}