using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class CollectionMovement : MonoBehaviour
{
    public Scrollbar scrollbar;
    public float stepSize;
    private float currentPosition;
    private float[] _positions;
    private bool _runIt = false;
    private float _time;
    private Button _takeButton;
    private int _buttonNumber;
    private float _scrollPosition = 0;
    private float _factor = 2f;
    public GameObject _scrollbar;
    private float _speedlerp = 1f;

    private float _centerPosition;

    private float _progress = 0.1f;
    private float _targetDistance = 0f;
    private float _localPositionZ = 0f;
    private Coroutine _coroutine;
    private int _currentIndex;

    void Start()
    {
        currentPosition = scrollbar.value;

        _positions = new float[transform.childCount];
        float distance = 1f / (_positions.Length - 1f);

        for (int i = 0; i < _positions.Length; i++)
        {
            _positions[i] = distance * i;
        }

        _centerPosition = scrollbar.size / 2f;
    }

    void Update()
    {
        // _positions = new float[transform.childCount];
        float distance = 1f / (_positions.Length - 1f);

        /*if (_runIt)
        {
            // GecisiDuzenle(1f / (_positions.Length - 1f), _positions, _takeButton);
            GecisiDuzenle(1f / (_positions.Length - 1f), _positions);
            _time += Time.deltaTime;

            if (_time > 1f)
            {
                _time = 0;
                _runIt = false;
            }
        }*/

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveScrollbar(stepSize);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveScrollbar(-stepSize);
        }


        /*for (int i = 0; i < _positions.Length; i++)
        {
            if (_scrollPosition < _positions[i] + (distance / _factor) &&
                _scrollPosition > _positions[i] - (distance / _factor))
            {
                transform.GetChild(i).localPosition = Vector3.Lerp(
                    transform.GetChild(i).localPosition,
                    new Vector3(transform.GetChild(i).localPosition.x, transform.GetChild(i).localPosition.y,
                        -_targetDistance),
                    _progress);
                
                for (int j = 0; j < _positions.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localPosition = Vector3.Lerp(transform.GetChild(j).localPosition,
                            new Vector3(
                                transform.GetChild(j).localPosition.x,
                                transform.GetChild(j).localPosition.y,
                                -_localPositionZ),
                            _progress);
                    }
                }
            }
        }*/
    }

    void MoveScrollbar(float step)
    {
        currentPosition += step;
        currentPosition = Mathf.Clamp(currentPosition, 0, 1);
        scrollbar.value = currentPosition;
    }

    public void WhichBtnClicked(Button button)
    {
        button.transform.name = "clicked";
        Debug.Log("Нажали " + button.transform.parent.transform.childCount);

        for (int i = 0; i < button.transform.parent.transform.childCount; i++)
        {
            if (button.transform.parent.transform.GetChild(i).transform.name == button.transform.name)
            {
                _buttonNumber = i;
                _takeButton = button;
                _time = 0;
                _scrollPosition = (_positions[_buttonNumber]);
                _runIt = true;
            }
        }
    }

    public void ButtonClick(int value)
    {
        _currentIndex += value;

        if (_currentIndex >= _positions.Length)
            _currentIndex = 0;

        if (_currentIndex < 0)
        {
            _currentIndex = _positions.Length - 1;
        }

        _buttonNumber = _currentIndex;
        _runIt = true;
        GecisiDuzenle(1f / (_positions.Length - 1f), _positions);
    }


    private void GecisiDuzenle(float distance, float[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (_scrollPosition < pos[i] + (distance / _factor) && _scrollPosition > pos[i] - (distance / _factor))
            {
                // _scrollbar.GetComponent<Scrollbar>().value = pos[_buttonNumber];
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(Scrolling(pos));
            }
        }
    }

    /*private void GecisiDuzenle(float distance, float[] pos, Button btn)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (_scrollPosition < pos[i] + (distance / _factor) && _scrollPosition > pos[i] - (distance / _factor))
            {
                // _scrollbar.GetComponent<Scrollbar>().value = pos[_buttonNumber];
                if(_coroutine!=null)
                    StopCoroutine(_coroutine);
                
                _coroutine = StartCoroutine(Scrolling(distance, pos, btn));
            }
        }

        for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        {
            btn.transform.name = ".";
        }
    }*/

    private IEnumerator Scrolling(float[] pos)
    {
        float elapsedTime = 0;
        float duration = 0.15f;
        float startvalue = _scrollbar.GetComponent<Scrollbar>().value;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _scrollbar.GetComponent<Scrollbar>().value =
                Mathf.Lerp(startvalue, pos[_buttonNumber], elapsedTime / duration);
            yield return null;
        }

        _scrollbar.GetComponent<Scrollbar>().value = pos[_buttonNumber];
    }


    /*
    public ScrollRect scrollRect;
    public float step = 1f;
    private float[] positions;
    private int currentIndex = 0;

    void Start()
    {
        RectTransform content = scrollRect.content;
        positions = new float[content.childCount];

        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform child = content.GetChild(i) as RectTransform;
            positions[i] = child.anchoredPosition.x;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            currentIndex++;
            if (currentIndex >= positions.Length)
                currentIndex = positions.Length - 1;

            ScrollToIndex(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = 0;

            ScrollToIndex(currentIndex);
        }
    }

    private void ScrollToIndex(int index)
    {
        float targetPosition = positions[index] - (scrollRect.viewport.rect.width * 0.5f);
        scrollRect.horizontalNormalizedPosition = targetPosition / (scrollRect.content.rect.width - scrollRect.viewport.rect.width);
    }
    */


    /*
    public HorizontalLayoutGroup layoutGroup;
    public int itemSpacing;
    private int currentSelectedItem = 0;

    void Start()
    {
        if (layoutGroup == null)
            layoutGroup = GetComponent<HorizontalLayoutGroup>();

        if (layoutGroup != null)
            itemSpacing = (int)layoutGroup.spacing;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentSelectedItem++;
            if (currentSelectedItem >= layoutGroup.transform.childCount)
                currentSelectedItem = 0;

            UpdateSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentSelectedItem--;
            if (currentSelectedItem < 0)
                currentSelectedItem = layoutGroup.transform.childCount - 1;

            UpdateSelectedItem();
        }
    }

    void UpdateSelectedItem()
    {
        for (int i = 0; i < layoutGroup.transform.childCount; i++)
        {
            RectTransform item = layoutGroup.transform.GetChild(i).GetComponent<RectTransform>();
            Vector2 pos = item.anchoredPosition;

            if (i == currentSelectedItem)
                pos.x += itemSpacing;
            else
                pos.x -= itemSpacing;

            item.anchoredPosition = pos;
        }
    }
    */


    /*[SerializeField] private float _objectSpacing = 1.0f;

    private List<GameObject> _objects;
    private int _currentIndex = 0;

    private void Start()
    {
        _objects = new List<GameObject>();

        foreach (Transform child in transform)
        {
            _objects.Add(child.gameObject);
        }

        _currentIndex = PlayerPrefs.GetInt("CollectionIndex", 0);

        /*for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].transform.localPosition = new Vector3((i - _currentIndex) * _objectSpacing, _objects[i].transform.localPosition.y, _objects[i].transform.localPosition.z);
        }#1#
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Set the next object as the current one
            _currentIndex++;
            if (_currentIndex >= _objects.Count)
            {
                _currentIndex = 0;
            }

            // Move all objects to their new positions
            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].transform.localPosition = Vector3.right * (i - _currentIndex) * _objectSpacing;
            }

            // Save the current index
            PlayerPrefs.SetInt("CollectionIndex", _currentIndex);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Set the previous object as the current one
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = _objects.Count - 1;
            }

            // Move all objects to their new positions
            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].transform.localPosition = Vector3.right * (i - _currentIndex) * _objectSpacing;
            }

            // Save the current index
            PlayerPrefs.SetInt("CollectionIndex", _currentIndex);
        }
    }*/
}