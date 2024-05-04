using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace CX
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        public PlayerManager player;
        PlayerControls playerControls;

        [Header("movement input")]
        [SerializeField] Vector2 movementInput;
        bool isPressSpecialKey;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("camera input")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("Player Action Input")]
        [SerializeField] bool dodgeInput;

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
        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.CameraControls.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            }

            playerControls.Enable();
        }
        private void Update()
        {
            HandleMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
        }
        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
        //切后台中断控制
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }
        //拿到玩家移动值
        private void HandleMovementInput()
        {
           
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) +Mathf.Abs(horizontalInput));

            if(moveAmount <= 0.5f && moveAmount >0)
            {
                if (isPressSpecialKey)
                    moveAmount = 1;
                else
                    moveAmount = 0.5f;
            }
            else if(moveAmount >0.5 && moveAmount<=1)
            {
                moveAmount = 1;
            }
            if(player ==null)            
                return;            
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
        }
        //拿到相机移动值
        private void HandleCameraMovementInput()
        {
            cameraHorizontalInput = cameraInput.x;
            cameraVerticalInput = cameraInput.y;
        }
        //根据场景来决定是否允许玩家移动输入

        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;
                player.playerLocomotionManager.AttenmpToPerformDodge();
            }
        }

        public void GetSpecialKeyStats(InputAction.CallbackContext input)
        {
            if(input.started)
            {
                isPressSpecialKey = true;
            }
            else if(input.canceled)
            {
                isPressSpecialKey =false;
            }
        }

    }
}

