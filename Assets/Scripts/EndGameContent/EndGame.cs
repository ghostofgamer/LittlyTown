using System.Collections;
using UI.Screens;
using UnityEngine;

namespace EndGameContent
{
    public class EndGame : AbstractScreen
    {
        [SerializeField] private Spawner _spawner;

        private void OnEnable()
        {
            _spawner.PositionsFilled += Open;
        }

        private void OnDisable()
        {
            _spawner.PositionsFilled -= Open;
        }
    }
}