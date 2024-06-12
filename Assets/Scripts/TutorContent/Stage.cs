using Dragger;
using ItemPositionContent;
using UI.Screens;
using UnityEngine;

namespace TutorContent
{
    public  abstract class Stage : MonoBehaviour
    {
        [SerializeField] private GameObject _descriptionGoalStage;
        [SerializeField] private GameObject _descriptionStage;
        [SerializeField] private Stage _nextStage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ItemDragger _itemDragger;
        [SerializeField] private InputItemDragger _inputItemDragger;

        protected InputItemDragger InputItemDragger => _inputItemDragger;
        
        protected ItemDragger ItemDragger => _itemDragger;
        
        protected CanvasGroup CanvasGroup => _canvasGroup;
        
        protected GameObject DescriptionGoalStage => _descriptionGoalStage;
        
        protected GameObject DescriptionStage => _descriptionStage;
        
        protected Stage NextStage => _nextStage;
        
        public void SwitchOffStage()
        {
            
        }

        public abstract void OpenStage();


        public void Open()
        {
            
        }

        public void Close()
        {
            
        }

        public void CloseCanvas()
        {
            _canvasGroup.gameObject.SetActive(false);
        }

        public void OpenCanvas()
        {
            _canvasGroup.gameObject.SetActive(true);
        }        
        
        
        public void ShowItem()
        {
            _itemDragger.SelectedObject.gameObject.SetActive(true);
            _itemDragger.SelectedObject.ItemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
            _inputItemDragger.enabled = true;
            
        }
        
        public void HideItem()
        {
            _itemDragger.SelectedObject.ItemPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
            _itemDragger.SelectedObject.gameObject.SetActive(false);
            _inputItemDragger.enabled = false;
        }
        
        
        
        public void ShowDescription()
        {
            DescriptionStage.SetActive(true);
        }
    }
}
