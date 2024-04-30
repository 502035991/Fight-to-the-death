using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace CX
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        PlayerControls playerControls;

        [Header("movement input")]
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("camera input")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;            
        }
        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += SetMovementInput;
                playerControls.PlayerCamera.CameraControls.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }
        private void Update()
        {
            HandleMovementInput();
            HandleCameraMovementInput();
        }
        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
        //�õ�����ƶ�ֵ
        private void HandleMovementInput()
        {
            horizontalInput =movementInput.x;
            verticalInput = movementInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) +Mathf.Abs(horizontalInput));

            if(moveAmount <= 0.5f && moveAmount >0)
            {
                moveAmount = 0.5f;
            }
            else if(moveAmount >0.5 && moveAmount<=1)
            {
                moveAmount = 1;
            }
        }
        //�õ�����ƶ�ֵ
        private void HandleCameraMovementInput()
        {
            cameraHorizontalInput = cameraInput.x;
            cameraVerticalInput = cameraInput.y;
        }
        //���ݳ����������Ƿ���������ƶ�����
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.name == "GameScene")
            {
                instance.enabled = true;
            }
            else
            {
                instance.enabled = false;
            }
        }

        private void SetMovementInput(InputAction.CallbackContext input)
        {
            movementInput = input.ReadValue<Vector2>();
        }
        //�к�̨�жϿ���
        private void OnApplicationFocus(bool focus)
        {
            if(enabled)
            {
                if(focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

    }
}

