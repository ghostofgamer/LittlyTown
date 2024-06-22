using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    /*public void SelectItem()
    {
        if (_selectedObject == null)
            return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.TryGetComponent(out Item item) && !item.IsActive)
            {
                Vector3 position = _selectedObject.transform.position;
                position = new Vector3(position.x, position.y + _offset, position.z);
                _selectedObject.transform.position = position;
                _objectPlane = new Plane(Vector3.up, position);
                float distanceToPlane;
                _objectPlane.Raycast(ray, out distanceToPlane);
                _distance = distanceToPlane;
                Vector3 mouseWorldPosition = ray.GetPoint(_distance);
                _offsetObject = _selectedObject.transform.position - mouseWorldPosition;
                _offsetObject.y = 0;

                if (_selectedObject.transform.position == position)
                    IsObjectSelected = true;
            }

            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition) &&
                    !itemPosition.IsBusy)
                {
                    _objectPlane = new Plane(Vector3.up, _selectedObject.transform.position);
                    IsPositionSelected = true;
                }
            }
        }
    }*/
}
