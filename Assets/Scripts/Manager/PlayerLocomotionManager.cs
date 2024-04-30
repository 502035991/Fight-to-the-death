using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace CX 
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmout;

        private Vector3 moveDirection;
        private Vector3 targetDirection;

        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float rotationSpeed = 15;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }
        private void GetVerticalAndHorizontalInputs()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
        }
        private void HandleGroundedMovement()
        {
            GetVerticalAndHorizontalInputs();

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
    }
}


