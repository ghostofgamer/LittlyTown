using UnityEngine;

namespace SaveAndLoad
{
    public class Save : MonoBehaviour
    {
        public void SetData(string name, int number)
        {
            PlayerPrefs.SetInt(name, number);
            PlayerPrefs.Save();
        }

        public void SetData(string name, float number)
        {
            PlayerPrefs.SetFloat(name, number);
            PlayerPrefs.Save();
        }
        
        public void SetData(string name, string currentName)
        {
            PlayerPrefs.SetString(name, currentName);
            PlayerPrefs.Save();
        }
    }
}
