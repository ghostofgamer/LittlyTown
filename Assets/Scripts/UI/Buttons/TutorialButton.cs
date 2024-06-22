using System.Collections;
using System.Collections.Generic;
using TutorContent;
using UI.Buttons;
using UnityEngine;

public class TutorialButton : AbstractButton
{
    [SerializeField] private RestartTutorial _restartTutorial;
    
    protected override void OnClick()
    {
        AudioSource.PlayOneShot(AudioSource.clip);
        _restartTutorial.StartTutorial();
    }
}
