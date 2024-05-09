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
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("camera input")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("Player Action Input")]
        [SerializeField] bool dodgeInput;
        [SerializeField] bool sprintInput;

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

                //��ס��
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            }

            playerControls.Enable();
        }
        private void Update()
        {
            HandleMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HnadleSprinting();
        }
        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
        //�к�̨�жϿ���
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
        //�õ�����ƶ�ֵ
        private void HandleMovementInput()
        {
           
            horizontalInput = movementInput.x;
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
            if(player ==null)            
                return;            
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount , player.playerNetworkManager.isSprinting.Value);
        }
        //�õ�����ƶ�ֵ
        private void HandleCameraMovementInput()
        {
            cameraHorizontalInput = cameraInput.x;
            cameraVerticalInput = cameraInput.y;
        }
        //���ݳ����������Ƿ���������ƶ�����

        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;
                player.playerLocomotionManager.AttenmpToPerformDodge();
            }
        }
        private void HnadleSprinting()
        {
            if(sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }
    }
}
