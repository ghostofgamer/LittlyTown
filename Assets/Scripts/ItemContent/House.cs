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

        private void Start()
        {
            _item = GetComponent<Item>();
            _baseIncome = _income;
            _maxIncome = _income * 2;
        }

        public void IncreaseIncome()
        {
            _income = _maxIncome;
            _item.SetGold(_income);
        }

        public void ResetIncome()
        {
            _income = _baseIncome;
            _item.SetGold(_income);
        }
    }
}