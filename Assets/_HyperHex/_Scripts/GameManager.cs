using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DonzaiGamecorp.HyperHex
{
    public enum GameMode
    {
        None,
        Easy,
        Normal,
        Hard,
        Survival
    }

    public class GameManager : MonoBehaviour
    {
        [HideInInspector] public static GameManager Instance;

        [Header("ScriptableObject")]
        public DataSO GmDataSO;

        [Header("UI Elements")]
        [SerializeField] Text _popUpText;
        [SerializeField] GameObject _playerObj;
        [SerializeField] GameObject _touchToStartBtn;
        [SerializeField] GameObject _pausePanel;
        [SerializeField] Button _pauseBtn;

        [HideInInspector] public float TimeTaken = 0f;
        [HideInInspector] public bool IsPlaying = false;
        [HideInInspector] public bool ExtraLifeOn = false;

        float _prevScore = 0f;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(Instance);
            }
        }

        private void Start()
        {
            //PlayerPrefs.DeleteAll();

            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                _playerObj.SetActive(false);
                _touchToStartBtn.SetActive(true);
                _popUpText.gameObject.SetActive(false);
                _pauseBtn.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                if (IsPlaying)
                    TimeTaken += Time.deltaTime;
            }
        }

        public void OnTouchtoStart()
        {
            IsPlaying = true;
            _playerObj.SetActive(true);
            _touchToStartBtn.SetActive(false);
            _pauseBtn.gameObject.SetActive(true);

            switch (GmDataSO.CurrentGameMode)
            {
                case GameMode.Easy:
                    StartCoroutine(PopText("EASY"));
                    break;
                case GameMode.Normal:
                    StartCoroutine(PopText("NORMAL"));
                    break;
                case GameMode.Hard:
                    StartCoroutine(PopText("HARD"));
                    break;
                case GameMode.Survival:
                    StartCoroutine(PopText("SURVIVAL"));
                    break;
            }

            GameObject[] hexClones = GameObject.FindGameObjectsWithTag("hexClone");

            foreach (GameObject hexClone in hexClones)
            {
                Destroy(hexClone);
            }
        }

        public void OnPopupText(string text)
        {
            StartCoroutine(PopText(text));
        }

        private IEnumerator PopText(string text)
        {
            _popUpText.text = text;
            _popUpText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            _popUpText.gameObject.SetActive(false);
        }

        public void RecordBest()
        {
            _prevScore = GetComponent<ScoreScript>().PrevBestScore;
            switch (GmDataSO.CurrentGameMode)
            {
                case GameMode.Easy:
                    PlayerPrefs.SetFloat("BestEasyScore", _prevScore);
                    break;
                case GameMode.Normal:
                    PlayerPrefs.SetFloat("BestNormalScore", _prevScore);
                    break;
                case GameMode.Hard:
                    PlayerPrefs.SetFloat("BestHardScore", _prevScore);
                    break;
                case GameMode.Survival:
                    PlayerPrefs.SetFloat("BestSurvivalScore", _prevScore);
                    break;
            }
            GmDataSO.CurrentGameMode = GameMode.None;
            LoadScene("MenuScene");
        }

        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        #region UI Buttons

        public void OnEasyMode()
        {
            GmDataSO.CurrentGameMode = GameMode.Easy;
            LoadScene("GameScene");
        }

        public void OnNormalMode()
        {
            GmDataSO.CurrentGameMode = GameMode.Normal;
            LoadScene("GameScene");
        }

        public void OnHardMode()
        {
            GmDataSO.CurrentGameMode = GameMode.Hard;
            LoadScene("GameScene");
        }

        public void OnSurvivalMode()
        {
            GmDataSO.CurrentGameMode = GameMode.Survival;
            LoadScene("GameScene");
        }

        public void OnApplicationQuit()
        {
            Application.Quit();
        }

        public void OnPauseBtn()
        {
            IsPlaying = false;
            _pausePanel.SetActive(true);
        }

        public void OnPauseReturnBtn()
        {
            IsPlaying = true;
            _pausePanel.SetActive(false);
        }

        #endregion
    }
}

