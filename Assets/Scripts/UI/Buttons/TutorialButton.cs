using TutorContent;
using UnityEngine;

namespace UI.Buttons
{
    public class TutorialButton : AbstractButton
    {
        [SerializeField] private RestartTutorial _restartTutorial;
    
        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            _restartTutorial.StartTutorial();
        }
    }
}
