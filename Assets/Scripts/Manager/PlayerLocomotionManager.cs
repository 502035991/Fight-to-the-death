using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace CX 
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmout;

        [Header("MovementSettings")]
        private Vector3 moveDirection;
        private Vector3 targetDirection;

        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float rotationSpeed = 15;

        [Header("dodge")]
        private Vector3 rollDirection;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        protected override void Update()
        {
            base.Update();
            if(player.IsOwner)
            {
                player.characterNetworkManager.verticalMovement.Value = verticalMovement;
                player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                player.characterNetworkManager.moveAmount.Value = moveAmout;
            }    
            else
            {
                verticalMovement = player.characterNetworkManager.verticalMovement.Value;
                horizontalMovement =player.characterNetworkManager.horizontalMovement.Value;
                moveAmout = player.characterNetworkManager.moveAmount.Value;

                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmout);
            }
        }
        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }
        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmout = PlayerInputManager.instance.moveAmount;
        }
        private void HandleGroundedMovement()
        {
            if (!player.canMove)
                return;
            GetMovementValues();
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;
            
            if(PlayerInputManager.instance.moveAmount == 1)
            {
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if(PlayerInputManager.instance.moveAmount == 0.5f)
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }
        private void HandleRotation()
        {
            if (!player.canRotate)
                return;
            targetDirection = Vector3.zero;
            targetDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetDirection.Normalize();
            targetDirection.y = 0;


            if(targetDirection == Vector3.zero)
            {
                targetDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetDirection);
            quaternion targetRotation = Quaternion.Lerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
        public void AttenmpToPerformDodge()
        {
            if (player.isPerformingAction)
                return;

            if (PlayerInputManager.instance.moveAmount >0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                rollDirection.Normalize();
                rollDirection.y = 0;

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                player.playerAnimatorManager.PlayerTargetActionAnimation("Roll_Forward", true, true);
            }
            else
            {

            }

        }
    }
}


