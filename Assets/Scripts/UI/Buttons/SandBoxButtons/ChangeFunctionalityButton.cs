using System.Collections;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFunctionalityButton : AbstractButton
{
    private const string Open = "Open";
    private const string Close = "Close";

    [SerializeField] private Sprite _activeButton;
    [SerializeField] private Sprite _notActiveButton;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private ChangeFunctionalityButton[] _changeFunctionalityButtons;
    [SerializeField] private Animator _animator;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Builder _builder;

    private float _positionCloseY = -143f;
    private float _positionOpenY = 143f;
    private float _elapsedTime;
    private float _duration = 0.16f;
    private bool _isActive;
    private Coroutine _coroutine;

    protected override void OnClick()
    {
        _isActive = !_isActive;
        Debug.Log("Active " + _isActive + "   " + this.name);

        if (_isActive)
        {
            DeactivationButtons();
            Activation();
        }
        else
        {
            Deactivation();
        }
        /*Debug.Log(_content.anchoredPosition);
        
        Vector3 newPosition = _content.anchoredPosition;
        newPosition.y = _positionOpenY;
        _content.anchoredPosition = newPosition;

        // _content.anchoredPosition = new Vector3(_content.anchoredPosition.x, _positionOpenY);*/

        AudioSource.PlayOneShot(AudioSource.clip);
    }

    private void DeactivationButtons()
    {
        foreach (var changeFunctionalityButton in _changeFunctionalityButtons)
        {
            changeFunctionalityButton.Deactivation();
            changeFunctionalityButton.OffActivity();
        }
    }

    public void OffActivity()
    {
        _isActive = false;
    }

    public void Deactivation()
    {
        /*if (!_isActive)
            return;*/
        if (_builder != null)
            _builder.enabled = false;

        _buttonImage.sprite = _notActiveButton;
        _iconImage.color = Color.white;
        // _isActive = false;

        if (_content != null)
            CloseContent();
        /*if (_animator != null)
        {
            Debug.Log("имя " + this.name + _animator.gameObject.name);
            _animator.SetTrigger(Close);
        }*/
    }

    private void Activation()
    {
        if (_builder != null)
        {
            _builder.enabled = true;
            _builder.Open();
        }
        
        _buttonImage.sprite = _activeButton;
        _iconImage.color = Color.black;
        // _isActive = true;

        if (_content != null)
            OpenContent();

        /*if (_animator != null)
        {
            Debug.Log("OPEN " + this.name + _animator.gameObject.name);
             _animator.SetTrigger(Open);
        }*/
    }

    private void OpenContent()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(MoveContent(new Vector2(_content.anchoredPosition.x, _positionOpenY)));
        // _content.anchoredPosition = new Vector3(_content.anchoredPosition.x, _positionOpenY);
        // Debug.Log("OPENPOS " + _content.position);
    }

    private void CloseContent()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(MoveContent(new Vector2(_content.anchoredPosition.x, _positionCloseY)));
        // _content.anchoredPosition = new Vector3(_content.anchoredPosition.x, _positionCloseY);
        // Debug.Log("ClosePOS " + _content.position);
    }

    private IEnumerator MoveContent(Vector2 target)
    {
        _elapsedTime = 0;
        Vector2 startPosition = _content.anchoredPosition;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            _content.anchoredPosition = Vector2.Lerp(startPosition, target, _elapsedTime / _duration);
            yield return null;
        }

        // Установить точную конечную позицию после завершения интерполяции
        _content.anchoredPosition = target;
    }
}