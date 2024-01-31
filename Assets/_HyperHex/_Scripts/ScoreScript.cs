using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DonzaiGamecorp.HyperHex
{
    public class ScoreScript : MonoBehaviour
    {
        [Header("Menu UI")]
        [SerializeField] TextMeshProUGUI _bestEasyScoreText;
        [SerializeField] TextMeshProUGUI _bestNormalScoreText;
        [SerializeField] TextMeshProUGUI _bestHardScoreText;
        [SerializeField] TextMeshProUGUI _bestSurvivalScoreText;

        [Header("Game UI")]
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] TextMeshProUGUI _bestScoreText;

        [HideInInspector]
        public float PrevBestScore = 0f;
        bool _notFirst = false;

        void Start()
        {
            if (SceneManager.GetActiveScene().name == "MenuScene")
            {
                if (PlayerPrefs.HasKey("BestEasyScore"))
                {
                    float _prevScore = PlayerPrefs.GetFloat("BestEasyScore");
                    _bestEasyScoreText.text = _prevScore.ToString("00.00");
                }

                if (PlayerPrefs.HasKey("BestNormalScore"))
                {
                    float _prevScore = PlayerPrefs.GetFloat("BestNormalScore");
                    _bestNormalScoreText.text = _prevScore.ToString("00.00");
                }

                if (PlayerPrefs.HasKey("BestHardScore"))
                {
                    float _prevScore = PlayerPrefs.GetFloat("BestHardScore");
                    _bestHardScoreText.text = _prevScore.ToString("00.00");
                }

                if (PlayerPrefs.HasKey("BestSurvivalScore"))
                {
                    float _prevScore = PlayerPrefs.GetFloat("BestSurvivalScore");
                    _bestSurvivalScoreText.text = _prevScore.ToString("00.00");
                }
            }

            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                switch (GameManager.Instance.GmDataSO.CurrentGameMode)
                {
                    case GameMode.Easy:
                        if (PlayerPrefs.HasKey("BestEasyScore"))
                        {
                            PrevBestScore = PlayerPrefs.GetFloat("BestEasyScore");
                            _bestScoreText.text = PrevBestScore.ToString("00.00");
                            _notFirst = true;
                        }
                        break;
                    case GameMode.Normal:
                        if (PlayerPrefs.HasKey("BestNormalScore"))
                        {
                            PrevBestScore = PlayerPrefs.GetFloat("BestNormalScore");
                            _bestScoreText.text = PrevBestScore.ToString("00.00");
                            _notFirst = true;
                        }
                        break;
                    case GameMode.Hard:
                        if (PlayerPrefs.HasKey("BestHardScore"))
                        {
                            PrevBestScore = PlayerPrefs.GetFloat("BestHardScore");
                            _bestScoreText.text = PrevBestScore.ToString("00.00");
                            _notFirst = true;
                        }
                        break;
                    case GameMode.Survival:
                        if (PlayerPrefs.HasKey("BestSurvivalScore"))
                        {
                            PrevBestScore = PlayerPrefs.GetFloat("BestSurvivalScore");
                            _bestScoreText.text = PrevBestScore.ToString("00.00");
                            _notFirst = true;
                        }
                        break;
                }
            }
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                if (GameManager.Instance.IsPlaying)
                {
                    _scoreText.text = GameManager.Instance.TimeTaken.ToString("00.00");

                    if (GameManager.Instance.TimeTaken > PrevBestScore)
                    {
                        PrevBestScore += Time.deltaTime;
                        _bestScoreText.text = GameManager.Instance.TimeTaken.ToString("00.00");
                        if (_notFirst)
                        {
                            GameManager.Instance.OnPopupText("BEST");
                        }
                    }
                }
            }
        }
    }
}

