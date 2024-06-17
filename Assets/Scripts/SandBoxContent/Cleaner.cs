using ItemContent;
using ItemPositionContent;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
    
    private Item _item;
    private int _layerMask;
    private int _layer = 3;

    private void Start()
    {
        _layerMask = 1 << _layer;
        _layerMask = ~_layerMask;
    }
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                if (hit.transform.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (itemPosition.IsBusy)
                    {
                        itemPosition.Item.gameObject.SetActive(false);
                        itemPosition.ClearingPosition();
                        return;
                    }
                }
            }
        }
    }
}
