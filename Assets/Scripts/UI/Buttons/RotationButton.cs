using EnvironmentContent;
using UnityEngine;

namespace UI.Buttons
{
    public class RotationButton : AbstractButton
    {
        [SerializeField] private int _index;
        [SerializeField] private TurnEnvironment _turnEnvironment;

        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            _turnEnvironment.ChangeRotation(_index);
        }
    }
}