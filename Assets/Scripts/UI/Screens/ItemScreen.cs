using CountersContent;
using UI.Buttons;
using UI.Buttons.PossibilitiesFiles;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class ItemScreen : AbstractScreen
    {
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private BuyItemButton[] _buyItemButtons;
        [SerializeField] private BuyPossibilitieButton[] _buyPossibilitieButtonsButtons;
        [SerializeField] private GameObject[] _items;
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private RectTransform[] _gameObjects;

        private int _constrainCountMax = 4;
        private int _constrainCountMin = 1;
        private Vector3 _localScaleItemShopPc = new Vector3(0.8f, 0.8f, 0.8f);

        private void Start()
        {
            if (Application.isMobilePlatform)
            {
                _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                _gridLayoutGroup.constraintCount = _constrainCountMax;
            }
            else
            {
                foreach (RectTransform gameObject in _gameObjects)
                    gameObject.localScale = _localScaleItemShopPc;

                _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
                _gridLayoutGroup.constraintCount = _constrainCountMin;
            }
        }

        public override void OnOpen()
        {
            base.OnOpen();

            foreach (var buyItemButton in _buyItemButtons)
                buyItemButton.Show();

            ShowItems();
            CheckAvailability();
        }

        private void ShowItems()
        {
            foreach (GameObject item in _items)
                item.SetActive(false);

            for (int i = 0; i < _scoreCounter.Factor; i++)
            {
                if (i >= _items.Length)
                    return;

                _items[i].SetActive(true);
            }
        }

        private void CheckAvailability()
        {
            foreach (BuyItemButton buyItemButton in _buyItemButtons)
                buyItemButton.CheckPossibilityPurchasing();

            foreach (BuyPossibilitieButton buyItemButton in _buyPossibilitieButtonsButtons)
                buyItemButton.CheckPossibilityPurchasing();
        }
    }
}