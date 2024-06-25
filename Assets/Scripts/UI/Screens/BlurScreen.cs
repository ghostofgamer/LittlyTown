using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class BlurScreen : MonoBehaviour
    {
        [SerializeField] private Button _dontBlurButton;

        public void BlurDeactivation()
        {
            _dontBlurButton.onClick.Invoke();
        }
    }
}