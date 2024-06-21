using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneVisual : MonoBehaviour
{
    [SerializeField] private Image _image;

    private void Update()
    {
        _image.transform.Rotate(0,0,-1);
    }
}
