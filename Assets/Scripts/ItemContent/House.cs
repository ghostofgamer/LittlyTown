using UnityEngine;

namespace ItemContent
{
    [RequireComponent(typeof(Item))]
    public class House : MonoBehaviour
    {
        [SerializeField] private int _income = 10;

        private Item _item;
        private int _baseIncome;
        private int _maxIncome;
        private int _factor = 2;

        private void Start()
        {
            _item = GetComponent<Item>();
            _baseIncome = _income;
            _maxIncome = _income * _factor;
        }

        public void IncreaseIncome()
        {
            SetIncome(_maxIncome);
        }

        public void ResetIncome()
        {
            SetIncome(_baseIncome);
        }

        private void SetIncome(int value)
        {
            _income = value;
            _item.SetGold(_income);
        }
    }
}