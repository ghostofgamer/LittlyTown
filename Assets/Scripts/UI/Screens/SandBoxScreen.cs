using System.Collections;
using System.Collections.Generic;
using CameraContent;
using EnvironmentContent;
using ItemPositionContent;
using SandBoxContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class SandBoxScreen : AbstractScreen
    {
        [SerializeField] private List<ItemPosition> _itemPositions;
        [SerializeField] private VisualItemsDeactivator _visualItemsDeactivator;
        [SerializeField] private CameraScrolling _cameraScrolling;
        [SerializeField] private EnvironmentMovement _environmentMovement;
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private List<ItemButton> _itemButtons = new List<ItemButton>();
        [SerializeField] private CollectionScreen _collectionScreen;
        [SerializeField] private SandBoxMovement _sandboxMovement;

        public override void OnOpen()
        {
            base.OnOpen();
            _visualItemsDeactivator.SetPositions(_itemPositions);
            _cameraScrolling.enabled = true;
            _environmentMovement.GoAway();
            _sandboxMovement.ReturnPosition();
            _cameraMovement.ZoomIn();
            _scrollRect.enabled = (!(_scrollbar.size >= 1));
            _scrollbar.gameObject.SetActive(!(_scrollbar.size >= 1));
            StartCoroutine(ShowButtons());
        }

        public override void Close()
        {
            base.Close();
            _environmentMovement.ReturnPosition();
            _sandboxMovement.GoOffScreen();
            _cameraMovement.ResetZoom();
        }

        private IEnumerator ShowButtons()
        {
            yield return null;

            for (int i = 0; i < _itemButtons.Count; i++)
            {
                if (_collectionScreen.CollectedItems.Contains(_itemButtons[i].ItemName))
                    _itemButtons[i].UnblockButton();
                else
                    _itemButtons[i].BlockButton();
            }
        }
    }
}