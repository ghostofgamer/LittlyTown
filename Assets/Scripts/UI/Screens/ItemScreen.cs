using System;
using CountersContent;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class ItemScreen : AbstractScreen
    {
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private BuyItemButton[] _buyItemButtons;
        [SerializeField] private GameObject[] _items;
        [SerializeField] private ScoreCounter _scoreCounter;

        /*private void OnEnable()
        {
            _scoreCounter.FactorChanged
        }

        private void OnDisable()
        {
            
        }*/

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
            {
                buyItemButton.Show();
            }

            ShowItems();
        }

        private void ShowItems()
        {
            foreach (var item in _items)
                item.SetActive(false);
            
            for (int i = 0; i < _scoreCounter.Factor; i++)
                _items[i].SetActive(true);
        }
    }
}