using ItemContent;
using MergeContent;
using TMPro;
using UnityEngine;

namespace CountersContent
{
    public class VisualScore : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private Merger _merger;
        [SerializeField] private Camera _camera;
    
        private Coroutine _coroutine;
        private Item _item;

        private void OnEnable()
        {
            _scoreCounter.ScoreIncomeChanged += OnShow;
            _merger.ItemMergered += SetItem;
        }

        private void OnDisable()
        {
            _scoreCounter.ScoreIncomeChanged -= OnShow;
            _merger.ItemMergered -= SetItem;
        }

        public void ScoreMove(int scoreValue)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _scoreText.text = scoreValue.ToString();
        }

        private void OnShow(int score)
        {
            _item.FlightScore.StartShow(_camera,score);
        }

        private void SetItem(Item item)
        {
            _item = item;
        }
    }
}