using UnityEngine.SceneManagement;

namespace UI.Buttons
{
    public class ResumeButton : AbstractButton
    {
        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}