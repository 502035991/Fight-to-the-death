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
        [SerializeField] bool jumpInput;
        [SerializeField] bool RB_Input = false;

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
            if(playerControls!= null)
                playerControls.Disable();
        }
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.name == "GameScene")
            {
                instance.enabled = true;

                if (playerControls != null)
                    playerControls.Enable();
            }
            else
            {
                instance.enabled = false;

                if (playerControls != null)
                    playerControls.Disable();
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
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;

                //按住后
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
            HnadleSprintInput();
            HandleJumpInput();
            HandleRBInput();
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
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }
        private void HnadleSprintInput()
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
        private void HandleJumpInput()
        {
            if(jumpInput)
            {
                jumpInput =false;
                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }
        private void HandleRBInput()
        {
            if(RB_Input)
            {
                RB_Input = false;

                player.playerNetworkManager.SetCharacterActionHand(true);


                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.rb_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }
    }
}

