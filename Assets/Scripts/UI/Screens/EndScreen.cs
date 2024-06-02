using UnityEngine;

namespace UI.Screens
{
    public class EndScreen : AbstractScreen
    {
        [SerializeField] private ParticleSystem _fireworkEffect;
        [SerializeField]private CameraMovement _cameraMovement;
        [SerializeField]private EnvironmentMovement _environmentMovement;

        public override void Open()
        {
            base.Open();
            _fireworkEffect.Play();
            _cameraMovement.ZoomOut();
            // _environmentMovement.StartRotate();
            
        }

        public override void Close()
        {
            base.Close();
            _fireworkEffect.Stop();
            // _environmentMovement.StopRotate();
        }
    }
}
