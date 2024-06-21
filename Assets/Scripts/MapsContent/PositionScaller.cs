using System.Collections;
using ItemPositionContent;
using Unity.VisualScripting;
using UnityEngine;

public class PositionScaller : MonoBehaviour
{
    [SerializeField] private Vector3 _scale;
    [SerializeField] private float _duration;
    [SerializeField] private Transform _tileTransform;
    [SerializeField] private ItemPosition _startTile;
    
    [SerializeField]  private ItemPosition _itemPosition;
    private float _elapsedTime;
    private float _offsetX = 1f;
    private float _offsetZ = 3f;
    private float _offsetY = 1.5f; 
    private Vector3 _startPosition;
    private Vector3 _target;
    
    private void Start()
    {
        // ScaleChanged();
        // _itemPosition = GetComponent<ItemPosition>();
    }

    public void ScaleChanged()
    {
        _itemPosition.SetFirstRoad(_startTile);
        
        /*if (_startTile != null)
            _startTile.gameObject.SetActive(true);*/

        StartCoroutine(Scaling());
    }

    private IEnumerator Scaling()
    {
        _target = transform.position;
        _startPosition = new Vector3(_target.x + _offsetX, _target.y + _offsetY, _target.z + _offsetZ);
        _elapsedTime = 0;

        while (_elapsedTime < _duration)
        {
            float progress = _elapsedTime / _duration;
            transform.localScale = Vector3.Lerp(Vector3.zero, _scale, progress);
            _tileTransform.position = Vector3.Lerp(_startPosition, transform.position, progress);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = _scale;
        _tileTransform.position = transform.position;
    }

    public void SetTransform()
    {
        transform.localScale = _scale;
        _tileTransform.position = transform.position;
    }
}