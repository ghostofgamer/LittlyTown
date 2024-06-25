using Dragger;
using ItemPositionContent;
using Keeper;
using UnityEngine;

namespace TutorContent
{
    public abstract class Stage : MonoBehaviour
    {
        [SerializeField] private GameObject _descriptionGoalStage;
        [SerializeField] private GameObject _descriptionStage;
        [SerializeField] private Stage _nextStage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private InputItemDragger _inputItemDragger;
        [SerializeField] private ItemThrower _itemThrower;
        [SerializeField] private ItemKeeper _itemKeeper;

        protected ItemThrower ItemThrower => _itemThrower;

        protected InputItemDragger InputItemDragger => _inputItemDragger;

        protected GameObject DescriptionGoalStage => _descriptionGoalStage;

        protected GameObject DescriptionStage => _descriptionStage;

        protected Stage NextStage => _nextStage;

        public void OpenCanvas()
        {
            if (_canvasGroup != null)
                _canvasGroup.gameObject.SetActive(true);
        }

        public void ShowItem()
        {
            _itemKeeper.SelectedObject.gameObject.SetActive(true);
            _itemKeeper.SelectedObject.ItemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
            _inputItemDragger.enabled = true;
        }

        public void ShowDescription()
        {
            DescriptionStage.SetActive(true);
        }

        protected void CloseCanvas()
        {
            if (_canvasGroup != null)
                _canvasGroup.gameObject.SetActive(false);
        }

        protected void HideItem()
        {
            _itemKeeper.SelectedObject.ItemPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
            _itemKeeper.SelectedObject.gameObject.SetActive(false);
            _inputItemDragger.enabled = false;
        }
    }
}