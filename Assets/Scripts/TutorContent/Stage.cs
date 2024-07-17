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
            _inputItemDragger.enabled = false;
        }
    }
}