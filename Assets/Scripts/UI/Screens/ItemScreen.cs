using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class ItemScreen : AbstractScreen
    {
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField]private BuyItemButton[] _buyItemButtons;
        
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
        }
    }
}
