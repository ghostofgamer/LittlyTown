using UnityEngine;

namespace SaveAndLoad
{
    public class Load : MonoBehaviour
    {
        public int Get(string name, int number)
        {
            if (PlayerPrefs.HasKey(name))
                return PlayerPrefs.GetInt(name);

            return number;
        }

        public float Get(string name, float number)
        {
            if (PlayerPrefs.HasKey(name))
                return PlayerPrefs.GetFloat(name);

            return number;
        }

        public string Get(string language, string autoFoundLanguage)
        {
            if (PlayerPrefs.HasKey(language))
                return PlayerPrefs.GetString(language);
            
            return autoFoundLanguage;
        }
    }
}