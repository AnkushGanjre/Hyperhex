using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DonzaiGamecorp.HyperHex
{
    public class SpawnerScript : MonoBehaviour
    {
        [SerializeField] GameObject _hexPrefab;
        [SerializeField] GameObject _hexPrefabV1;
        [SerializeField] GameObject _hexPrefabV2;
        [SerializeField] GameObject _hexPrefabV3;
        [SerializeField] GameObject _powerUpPrefab_1;
        [SerializeField] GameObject _powerUpPrefab_2;
        [SerializeField] GameObject _powerUpPrefab_3;

        [SerializeField] float _easySpeed = 1f;
        [SerializeField] float _normalSpeed = 1.5f;
        [SerializeField] float _hardSpeed = 2.25f;
        [SerializeField] float _survivalSpeed = 1f;

        float _currentShrinkSpeed = 0f;
        float _spawnRate = 0.5f;
        float _nextTimeToSpawn = 0f;
        float _timer = 0f;
        float _survivalInterval = 15f;

        int _hexSpawnCount = 10;

        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "MenuScene")
            {
                _currentShrinkSpeed = _easySpeed;
            }
            else if (SceneManager.GetActiveScene().name == "GameScene")
            {
                switch (GameManager.Instance.GmDataSO.CurrentGameMode)
                {
                    case GameMode.Easy:
                        _currentShrinkSpeed = _easySpeed;
                        _spawnRate = 0.35f;
                        break;
                    case GameMode.Normal:
                        _currentShrinkSpeed = _normalSpeed;
                        _spawnRate = 0.6f;
                        break;
                    case GameMode.Hard:
                        _currentShrinkSpeed = _hardSpeed;
                        _spawnRate = 0.75f;
                        break;
                    case GameMode.Survival:
                        _currentShrinkSpeed = _survivalSpeed;
                        _spawnRate = 0.65f;
                        break;
                }
            }

            _hexPrefab.transform.GetComponent<HexScript>().shrinkSpeed = _currentShrinkSpeed;
        }

        private void Update()
        {
            if (GameManager.Instance.IsPlaying)
            {
                if (GameManager.Instance.GmDataSO.CurrentGameMode == GameMode.Survival)
                {
                    _timer += Time.deltaTime;

                    if (_timer >= _survivalInterval)
                    {
                        if (_spawnRate < 1f)
                        {
                            _spawnRate += 0.1f;
                        }
                        if (_currentShrinkSpeed < 3f)
                        {
                            _currentShrinkSpeed += 0.1f;
                        }

                        GameObject[] hexClones = GameObject.FindGameObjectsWithTag("hexClone");

                        foreach (GameObject hexClone in hexClones)
                        {
                            hexClone.GetComponent<HexScript>().shrinkSpeed = _currentShrinkSpeed;
                        }
                        _hexPrefab.transform.GetComponent<HexScript>().shrinkSpeed = _currentShrinkSpeed;

                        // Reset the timer for the next interval
                        _timer = 0f;
                    }
                }

                // Increment the timer
                _nextTimeToSpawn += Time.deltaTime;

                if (_nextTimeToSpawn >= 1f / _spawnRate)
                {
                    InstantiateObjects();
                    _nextTimeToSpawn = 0f; // Reset the timer
                }
            }
            else
            {
                if (GameManager.Instance.TimeTaken == 0)
                {
                    // Increment the timer
                    _nextTimeToSpawn += Time.deltaTime;

                    if (_nextTimeToSpawn >= 1f / _spawnRate)
                    {
                        InstantiateObjects();
                        _nextTimeToSpawn = 0f; // Reset the timer
                    }
                }
            }
        }

        private void InstantiateObjects()
        {
            if (_hexSpawnCount == 0)
            {
                _hexSpawnCount = Random.Range(6, 15);
                int randomNumber = Random.Range(1, 4);

                switch (randomNumber)
                {
                    case 1:
                        SpawnPowerUp(_powerUpPrefab_1);
                        return;
                    case 2:
                        SpawnPowerUp(_powerUpPrefab_2);
                        return;
                    case 3:
                        SpawnPowerUp(_powerUpPrefab_3);
                        return;
                }
            }
            else
            {
                int randomNumber = Random.Range(0, 100);
                GameObject prefab;
                if (randomNumber < 75)
                {
                    prefab = _hexPrefab;
                }
                else
                {
                    int randomNum = Random.Range(1, 4);
                    switch (randomNum)
                    {
                        case 1:
                            prefab = _hexPrefabV1;
                            break;
                        case 2:
                            prefab = _hexPrefabV2;
                            break;
                        case 3:
                            prefab = _hexPrefabV3;
                            break;
                        default:
                            prefab = _hexPrefab;
                            break;
                    }
                }

                GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                go.GetComponent<HexScript>().shrinkSpeed = _currentShrinkSpeed;
                _hexSpawnCount--;
                return;
            }
        }

        private void SpawnPowerUp(GameObject prefab)
        {
            // Randomly choose whether to spawn on the X or Y edge
            bool spawnOnXEdge = Random.Range(0f, 1f) > 0.5f;

            // Randomly choose the sign of the edge (-1 or 1)
            int edgeSign = (Random.Range(0f, 1f) > 0.5f) ? 1 : -1;

            // Define random position on the chosen edge
            Vector3 randomPosition;
            if (spawnOnXEdge)
            {
                randomPosition = new Vector3(edgeSign * 5f, Random.Range(-5f, 5f), 0f);
            }
            else
            {
                randomPosition = new Vector3(Random.Range(-5f, 5f), edgeSign * 5f, 0f);
            }

            // Instantiate prefab at the random position
            GameObject newPrefab = Instantiate(prefab, randomPosition, Quaternion.identity);
            newPrefab.GetComponent<PowerUpScript>().Speed = _currentShrinkSpeed / 2;
            newPrefab.GetComponent<PowerUpScript>().ScaleDuration = _currentShrinkSpeed * 3;
        }

        public void OnSloMoActivate()
        {
            StartCoroutine(StartSloMO());
        }

        private IEnumerator StartSloMO()
        {
            float originalSpeed = _currentShrinkSpeed;
            float originalSpawnRate = _spawnRate;
            _currentShrinkSpeed = _currentShrinkSpeed / 2;
            _spawnRate = _spawnRate / 2;

            GameObject[] hexClones = GameObject.FindGameObjectsWithTag("hexClone");

            foreach (GameObject hexClone in hexClones)
            {
                hexClone.GetComponent<HexScript>().shrinkSpeed = _currentShrinkSpeed;
            }
            _hexPrefab.transform.GetComponent<HexScript>().shrinkSpeed = _currentShrinkSpeed;
            yield return new WaitForSeconds(10f);
            _currentShrinkSpeed = originalSpeed;
            _spawnRate = originalSpawnRate;
            GameObject[] hexClones2 = GameObject.FindGameObjectsWithTag("hexClone");
            foreach (GameObject hexClone in hexClones2)
            {
                hexClone.GetComponent<HexScript>().shrinkSpeed = _currentShrinkSpeed;
            }
            _hexPrefab.transform.GetComponent<HexScript>().shrinkSpeed = _currentShrinkSpeed;
        }
    }
}

