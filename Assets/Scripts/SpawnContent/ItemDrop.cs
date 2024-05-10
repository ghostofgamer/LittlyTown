using ItemContent;
using UnityEngine;

namespace SpawnContent
{
    public class ItemDrop : MonoBehaviour
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _itemName;
        [SerializeField] private float _baseDropChance;
        [SerializeField] private float _levelIncreaseFactor;
        [SerializeField] private float _levelDecreaseFactor;
        [SerializeField] private Item _prefabItem;

        public Sprite Icon => _icon;
        public string ItemName => _itemName;
        public float BaseDropChance => _baseDropChance;
        
        public float LevelIncreaseFactor => _levelIncreaseFactor;
        
        public float LevelDecreaseFactor => _levelDecreaseFactor;
        
        public Item PrefabItem => _prefabItem;
        
    }
}
