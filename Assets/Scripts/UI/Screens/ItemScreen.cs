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

        private void Start()
        {
            if (Application.isMobilePlatform)
            {
                _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                _gridLayoutGroup.constraintCount = 4;
            }
            else
            {
                _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
                _gridLayoutGroup.constraintCount = 1;
            }
        }

        public override void Open()
        {
            base.Open();

            foreach (var buyItemButton in _buyItemButtons)
                buyItemButton.Show();

            ShowItems();
            CheckAvailability();
        }

        private void ShowItems()
        {
            foreach (var item in _items)
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
            foreach (var buyItemButton in _buyItemButtons)
                buyItemButton.CheckPossibilityPurchasing();
            
            foreach (var buyItemButton in _buyPossibilitieButtonsButtons)
                buyItemButton.CheckPossibilityPurchasing();
        }
    }
}