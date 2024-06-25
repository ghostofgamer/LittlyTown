using ItemContent;
using UnityEngine;

namespace SaveAndLoad
{
    [System.Serializable]
    public class ItemDropData
    {
        public Sprite Icon;
        public Item PrefabItem;

        public ItemDropData(Sprite icon, Item prefabItem)
        {
            Icon = icon;
            PrefabItem = prefabItem;
        }
    }
}