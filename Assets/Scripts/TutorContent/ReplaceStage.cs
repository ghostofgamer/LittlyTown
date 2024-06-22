using System.Collections;
using PossibilitiesContent;
using TutorContent;
using UnityEngine;

public class ReplaceStage : Stage
{
    [SerializeField]private ReplacementPosition _replacementPosition;
    [SerializeField] private GameObject _removeButton;
    
    private void OnEnable()
    {
        _replacementPosition.PositionChanging += ActivateRemoveStage;
    }

    private void OnDisable()
    {
        _replacementPosition.PositionChanging -= ActivateRemoveStage;
    }
    
    public void ActivateRemoveStage()
    {
        StartCoroutine(Active());
    }
    
    private IEnumerator Active()
    {
        yield return new WaitForSeconds(0.5f);
        HideItem();
        CloseCanvas();
         DescriptionGoalStage.SetActive(false);
                NextStage.gameObject.SetActive(true);
                NextStage.ShowDescription();
                _removeButton.SetActive(true);
                    
        gameObject.SetActive(false);
            
    }
    
    public override void OpenStage()
    {
    }
}
