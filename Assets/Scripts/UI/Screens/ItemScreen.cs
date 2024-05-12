using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class ItemScreen : AbstractScreen
    {
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        
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
    }
}
