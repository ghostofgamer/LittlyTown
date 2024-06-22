using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace PostProcessContent
{
    public class Blur : PostProcessChanger
    {
        [SerializeField] private Vignette _vignette;

        private DepthOfField _depthOfField;
        
        private void Start()
        {
            _depthOfField = PostProcessVolume.profile.GetSetting<DepthOfField>();
            PostProcessVolume.weight = DefaultValue;
            SetValue(DefaultValue);
        }

        protected override void ChangeValue()
        {
            PostProcessVolume.weight = CurrentValue;
        }

        public override void TurnOn()
        {
            base.TurnOn();
            _depthOfField.active = true;
            _vignette.TurnOn();
        }

        public override void TurnOff()
        {
            base.TurnOff();
            _depthOfField.active = false;
            _vignette.TurnOff();
        }
    }
}