using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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
        [SerializeField] float sprintSpeed = 6.5f;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] float sprintingStaminaCost = 2;

        [Header("Jump")]
        [SerializeField] float jumpStaminaCost = 5;
        [SerializeField] float jumpHeight = 4;
        [SerializeField] float jumpForwardSpeed = 5;
        [SerializeField] float freeFallSpeed = 2;
        protected Vector3 jumpDirection;

        [Header("dodge")]
        private Vector3 rollDirection;
        [SerializeField] float dodgeStaminaCost = 2;


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

                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmout, player.playerNetworkManager.isSprinting.Value);
            }
        }
        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();

            HandleJumpingMovement();
            HandleFreeFallMovement();
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
            
            if(player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * sprintSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount == 1)
                {
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount == 0.5f)
                {
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }

        }
        private void HandleJumpingMovement()
        {
            if (player.isJumping)
            {
                player.characterController.Move(jumpDirection* jumpForwardSpeed * Time.deltaTime);
            }
        }
        private void HandleFreeFallMovement()
        {
            if(!player.isGrounded)
            {
                Vector3 freeFallDirection;

                freeFallDirection =PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection = freeFallDirection + PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection *  freeFallSpeed *Time.deltaTime);
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
        //…¡±‹
        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction)
                return;
            if (player.playerNetworkManager.currentStamina.Value <= 0)
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

            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
        }
        //≥Â¥Ã
        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
            if(player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
            if(moveAmout >= 0.5)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
            if(player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }
        //Ã¯‘æ
        public void AttemptToPerformJump()
        {
            if (player.isPerformingAction)
                return;
            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return;
            if (player.isJumping)
                return;
            if (!player.isGrounded)
                return;

            player.playerAnimatorManager.PlayerTargetActionAnimation("Main_Jump_Start", false);

            player.isJumping = true;

            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.gameObject.transform.right * PlayerInputManager.instance.horizontalInput;
            jumpDirection.y = 0;
            if(jumpDirection != Vector3.zero)
            {
                if (player.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1;
                }
                else if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    jumpDirection *= 0.5f;
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    jumpDirection *= 0.25f;
                }
            }
        }
        //∂Øª≠µƒevent ¬º˛ Main Jump Up
        public void  ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 *  gravityForce);
        }
    }
}


