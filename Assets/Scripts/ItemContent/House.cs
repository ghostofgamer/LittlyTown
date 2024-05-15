using UnityEngine;

namespace ItemContent
{
    [RequireComponent(typeof(Item))]
    public class House : MonoBehaviour
    {
        [SerializeField ]private int _income = 10;

        private Item _item;
        private int _baseIncome;

        private void Start()
        {
            _item = GetComponent<Item>();
            _baseIncome = _income;
        }

        public void IncreaseIncome()
        {
            _income *= 2;
            _item.SetGold(_income);
        }

        public void ResetIncome()
        {
            _income = _baseIncome;
            _item.SetGold(_income);
        }

        /*public void CollectIncome()
    {
        GameManager.Instance.AddProfit(income);
    }*/
    }
}
