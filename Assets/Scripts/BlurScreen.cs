using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlurScreen : MonoBehaviour
{
    [SerializeField] private Button _blurButton;
    [SerializeField] private Button _dontBlurButton;

    public void BlurActivation()
    {
        _blurButton.onClick.Invoke();
    }

    public void BlurDeactivation()
    {
        _dontBlurButton.onClick.Invoke();
    }
}
