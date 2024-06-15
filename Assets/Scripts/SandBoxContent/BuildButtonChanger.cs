using UnityEngine;

public class BuildButtonChanger : MonoBehaviour
{
    [SerializeField] private BuildButton[] _buildButtons;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _notActiveSprite;

    private float _fullAlpha = 255f;
    private float _halfAlpha = 150f;

    public void Deactivation(int index)
    {
        for (int i = 0; i < _buildButtons.Length; i++)
        {
            if (i != index && _buildButtons[i].IsActive)
                _buildButtons[i].Deactivate();
        }
    }
}