using UnityEngine;

namespace DonzaiGamecorp.HyperHex
{
    public class PowerUpScript : MonoBehaviour
    {
        public float Speed = 1f;
        public float ScaleDuration = 2.5f; // Duration in seconds
        float minScale = 0.001f;
        [SerializeField] string _powerName;


        void Update()
        {
            // Move towards Vector3.zero
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Speed * Time.deltaTime);
            float distance = Vector3.Distance(transform.position, Vector3.zero);

            if (distance < 2)
            {
                // Gradually scale the object based on distance over time
                float currentScale = transform.localScale.x; // Assuming uniform scaling
                float targetScale = Mathf.Lerp(currentScale, minScale, Time.deltaTime / ScaleDuration);
                transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            }

            // Destroy the object when it reaches Vector3.zero
            if (transform.position == Vector3.zero)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                switch (_powerName)
                {
                    case "Destroyer":
                        GameManager.Instance.OnPopupText("DESTROYER");
                        GameObject[] hexClones = GameObject.FindGameObjectsWithTag("hexClone");
                        foreach (GameObject hexClone in hexClones)
                        {
                            Destroy(hexClone);
                        }
                        break;

                    case "SloMo":
                        GameManager.Instance.OnPopupText("SLO MO");
                        SpawnerScript script = FindObjectOfType<SpawnerScript>();
                        script.OnSloMoActivate();
                        break;

                    case "ExtraLife":
                        if (GameManager.Instance.ExtraLifeOn)
                        {
                            GameManager.Instance.OnPopupText("LIVES FULL");
                        }
                        else
                        {
                            GameManager.Instance.OnPopupText("EXTRA LIFE");
                            GameManager.Instance.ExtraLifeOn = true;
                        }
                        break;
                }

                Destroy(gameObject);
            }
        }
    }
}

