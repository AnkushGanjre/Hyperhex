using UnityEngine;
using UnityEngine.SceneManagement;

namespace DonzaiGamecorp.HyperHex
{
    public class CamRotatorScript : MonoBehaviour
    {
        bool randomZ = false;
        int timeTaken = 0;
        float rotationSpeed = 30f;

        private void Start()
        {
            InvokeRepeating("IncTime", 1f, 1f);
        }

        private void IncTime()
        {
            timeTaken += 1;
            randomZ = true;
        }


        void Update()
        {
            if (GameManager.Instance.IsPlaying)
            {
                Rotate();
            }
            else
            {
                if (GameManager.Instance.TimeTaken == 0)
                {
                    Rotate();
                }
            }
        }

        private void Rotate()
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                if (timeTaken % 5 == 0 && randomZ)
                {
                    int ranZ = Random.Range(0, 2);
                    //Debug.Log(ranZ.ToString());

                    if (ranZ == 1)
                    {
                        int ranX = Random.Range(0, 2);
                        if (ranX == 1)
                        {
                            transform.position = new Vector3(1f, 0f, -12f);
                        }

                        int ranY = Random.Range(0, 2);
                        if (ranY == 1)
                        {
                            transform.position = new Vector3(0f, 1f, -9f);
                        }
                    }
                    else
                    {
                        transform.position = new Vector3(0f, 0f, -10f);
                    }
                    randomZ = false;
                }
            }
        }
    }
}

