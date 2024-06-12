using TutorContent;
using UI.Buttons;
using UnityEngine;

public class ChangeTutorialContentButton : AbstractButton
{
    [SerializeField] private GameObject _closeStage;
    [SerializeField] private GameObject _openStage;
    [SerializeField] private TutorialScreen _tutorialScreen;

    protected override void OnClick()
    {
        AudioSource.PlayOneShot(AudioSource.clip);
        _tutorialScreen.ChangeTutorialStage(_closeStage, _openStage);
    }
}