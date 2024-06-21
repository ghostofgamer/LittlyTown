using CollectionContent;
using UnityEngine;

namespace UI.Buttons
{
    public class CollectionMoveButton : AbstractButton
    {
        [SerializeField] private int _value;
        [SerializeField] private CollectionMovement _collectionMovement;

        protected override void OnClick()
        {
            _collectionMovement.ChangeCurrentIndexItem(_value);
        }
    }
}