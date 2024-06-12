using System.Collections;
using TutorContent;
using UnityEngine;

public class BulldozerStage : Stage
{
    [SerializeField]private RemovalItems _removalItems;
    
    private void OnEnable()
    {
        _removalItems.Removed += ActivatedFinalStage;
    }

    private void OnDisable()
    {
        _removalItems.Removed -= ActivatedFinalStage;
    }
    
    private void ActivatedFinalStage()
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
                    
        gameObject.SetActive(false);
            
    }
    
    public override void OpenStage()
    {
        throw new System.NotImplementedException();
    }
}
