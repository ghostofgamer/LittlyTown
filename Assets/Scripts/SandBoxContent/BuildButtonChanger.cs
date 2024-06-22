using UnityEngine;

namespace SandBoxContent
{
    public class BuildButtonChanger : MonoBehaviour
    {
        [SerializeField] private BuildButton[] _buildButtons;

        public void Deactivation(int index)
        {
            for (int i = 0; i < _buildButtons.Length; i++)
            {
                if (i != index && _buildButtons[i].IsActive)
                    _buildButtons[i].Deactivate();
            }
        }
    }
}