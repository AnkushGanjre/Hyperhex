using UnityEngine;

namespace DonzaiGamecorp.HyperHex
{
    public class PlayerScript : MonoBehaviour
    {
        [SerializeField] VariableJoystick joystick;
        [SerializeField] Color[] playerColor;

        float _moveSpeed = 300f;
        float _movement = 0f;
        bool _isHeldDown = false;
        int _timeTaken = 0;

        private void Start()
        {
            InvokeRepeating("IncTime", 1f, 1f);
        }

        private void IncTime()
        {
            if (GameManager.Instance.IsPlaying)
            {
                _timeTaken += 1;
            }
        }

        void Update()
        {
            if (GameManager.Instance.IsPlaying)
            {
                if (_timeTaken % 10 == 0)
                {
                    int ranC = Random.Range(0, 6);
                    transform.GetComponent<SpriteRenderer>().color = playerColor[ranC];
                }

                CheckJoystickInput();
            }
        }

        private void FixedUpdate()
        {
            if (_isHeldDown)
            {
                transform.RotateAround(Vector3.zero, Vector3.forward, _movement * Time.fixedDeltaTime * -_moveSpeed);
            }
        }

        private void CheckJoystickInput()
        {
            _isHeldDown = Mathf.Abs(joystick.Direction.x + joystick.Direction.y) > 0.1f ? true : false;

            if (_isHeldDown)
            {
                if (joystick.Direction.x > 0)
                {
                    _movement = 1;
                }
                else if (joystick.Direction.x < 0)
                {
                    _movement = -1;
                }
            }
        }
    }
}

