using UnityEngine.SceneManagement;

namespace UI.Buttons
{
    public class ResumeButton : AbstractButton
    {
        protected override void OnClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
