using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace CX
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        PlayerControls playerControls;

        [SerializeField] Vector2 movementInput;

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
            
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            //��ʱд�ģ������ƥ�䣬�ж�����ǲ�ϣ��input��Ч�Ľ��棬��ȡ������
/*            if(newScene.buildIndex == SceneManager.sceneCount)
            {
                instance.enabled = true;
            }
            else
            {
                instance.enabled =false;
            }*/
        }

        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += SetMovementInput;
            }

            playerControls.Enable();
        }
        private void SetMovementInput(InputAction.CallbackContext input)
        {
            movementInput = input.ReadValue<Vector2>();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

    }
}

