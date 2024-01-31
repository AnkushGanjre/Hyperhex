using UnityEngine;
using UnityEngine.UI;

namespace DonzaiGamecorp.HyperHex
{
    public class MusicScript : MonoBehaviour
    {
        [SerializeField] Button _musicBtn;

        AudioSource _audioSource;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            if (PlayerPrefs.HasKey("Music"))
            {
                string value = PlayerPrefs.GetString("Music");
                if (value == "MusicOn")
                {
                    _audioSource.mute = false;
                    _musicBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }
                else
                {
                    _audioSource.mute = true;
                    _musicBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
                }
            }
        }


        public void OnMusicBtn()
        {
            if (_audioSource.mute == true)
            {
                _audioSource.mute = false;
                _musicBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                PlayerPrefs.SetString("Music", "MusicOn");
            }
            else
            {
                _audioSource.mute = true;
                _musicBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
                PlayerPrefs.SetString("Music", "MusicOff");
            }
        }
    }
}

