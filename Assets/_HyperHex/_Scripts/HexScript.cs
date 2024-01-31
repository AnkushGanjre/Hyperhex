using UnityEngine;

namespace DonzaiGamecorp.HyperHex
{
    public class HexScript : MonoBehaviour
    {
        public float shrinkSpeed = 1f;
        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.rotation = Random.Range(0f, 360f);
            transform.localScale = Vector3.one * 10f;
        }

        void Update()
        {
            if (GameManager.Instance.IsPlaying)
            {
                transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

                if (transform.localScale.x <= .05f)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (GameManager.Instance.TimeTaken == 0)
                {
                    transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

                    if (transform.localScale.x <= .05f)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (GameManager.Instance.ExtraLifeOn)
                {
                    GameManager.Instance.ExtraLifeOn = false;
                    GameManager.Instance.OnPopupText("LAST LIFE");
                    return;
                }
                GameManager.Instance.RecordBest();
            }
        }
    }
}

