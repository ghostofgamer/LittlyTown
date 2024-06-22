using CameraContent;
using Dragger; 
using InitializationContent;
using UnityEngine;

namespace UI.Screens
{
    public class GameLevelScreen : AbstractScreen
    {
        private static readonly int OpenAnimation = Animator.StringToHash("Open");

        [SerializeField] private CameraScrolling _cameraScrolling;
        [SerializeField] private Animator _animator;
        [SerializeField] private InputItemDragger _inputItemDragger;
        [SerializeField] private Initializator _initializator;
        [SerializeField] private Transform _infoContent;

        private int _indexSceneNotGold = 9;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private float _factor = 30f;

        private void Start()
        {
            _startPosition = _infoContent.position;
            _targetPosition = new Vector3(_startPosition.x, _startPosition.y + _factor, _startPosition.z);
        }

        public override void Open()
        {
            base.Open();

            if (_initializator.Index == _indexSceneNotGold)
                _infoContent.position = _targetPosition;
            else
                _infoContent.position = _startPosition;

            _animator.SetTrigger(OpenAnimation);
            _inputItemDragger.enabled = true;
            _cameraScrolling.enabled = true;
        }

        public override void Close()
        {
            base.Close();
            _inputItemDragger.enabled = false;
            _cameraScrolling.enabled = false;
        }
    }
}