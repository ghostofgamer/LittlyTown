using CameraContent;
using CountersContent;
using EnvironmentContent;
using SaveAndLoad;
using TMPro;
using UnityEngine;

namespace UI.Screens
{
    public class EndScreen : AbstractScreen
    {
        private const string MaxRecord = "MaxRecord";
        
        [SerializeField] private ParticleSystem _fireworkEffect;
        [SerializeField]private CameraMovement _cameraMovement;
        [SerializeField]private EnvironmentMovement _environmentMovement;
        [SerializeField]private TurnEnvironment _turnEnvironment;
        [SerializeField] private TMP_Text _currentRecordText;
        [SerializeField] private TMP_Text _maxRecordText;
        [SerializeField]private ScoreCounter _scoreCounter;
        [SerializeField] private Load _load;
        [SerializeField] private Save _save;
        [SerializeField]private Initializator _initializator;
        [SerializeField] private GameObject _newRecordContent;
        [SerializeField] private GameObject _oldRecordContent;
        private int _currentRecord;
        private int _maxRecord;
        private int _startValue;
        
        public override void Open()
        {
            base.Open();
            _fireworkEffect.Play();
            _cameraMovement.ZoomOut();
            // _environmentMovement.StartRotate();
            _turnEnvironment.StartRotate();
            CheckRecord();
        }

        public override void Close()
        {
            base.Close();
            _fireworkEffect.Stop();
            // _environmentMovement.StopRotate();
            _turnEnvironment.StopRotate();
        }

        private void CheckRecord()
        {
            _currentRecord = _scoreCounter.CurrentScoreRecord;
            _currentRecordText.text = _currentRecord.ToString();
            int maxRecord = _load.Get(MaxRecord + _initializator.Index, _startValue);

            if (_currentRecord > maxRecord)
            {
                _maxRecordText.text = _currentRecord.ToString();
                _newRecordContent.SetActive(true);
                _oldRecordContent.SetActive(false);
                _save.SetData(MaxRecord+_initializator.Index, _currentRecord);
                
            }
            else
            {
                _newRecordContent.SetActive(false);
                _oldRecordContent.SetActive(true);
                _maxRecordText.text = maxRecord.ToString(); 
            }
        }
    }
}
