namespace PostProcessContent
{
    public class Vignette : PostProcessChanger
    {
        private UnityEngine.Rendering.PostProcessing.Vignette _vignete;
        
        private void Start()
        {
            _vignete = PostProcessVolume.profile.GetSetting<UnityEngine.Rendering.PostProcessing.Vignette>();
            _vignete.intensity.value = DefaultValue;
            SetValue(DefaultValue);
        }

        protected override void ChangeValue()
        {
            _vignete.intensity.value = CurrentValue;
        }
    }
}