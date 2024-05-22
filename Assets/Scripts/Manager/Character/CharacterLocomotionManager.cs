using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CX
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Ground Check & Jumping")]
        [SerializeField] protected float gravityForce = -5.55f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity;
        [SerializeField] protected float groundedVelocityY = -20;
        [SerializeField] protected float fallStartVelcityY = -10;
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;


        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();   

        }
        protected virtual void Update()
        {
            HandleGroundCheck();
            if(character.isGrounded)
            {
                if(yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedVelocityY;
                }
            }    
            else
            {
                if(!character.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartVelcityY;
                }
                inAirTimer += Time.deltaTime;
                character.anim.SetFloat("InAirTimer", inAirTimer);
                character.anim.SetFloat("yVelocity" , yVelocity.y);

                yVelocity.y += Time.deltaTime * gravityForce;
            }
            character.characterController.Move(yVelocity * Time.deltaTime);
        }
        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }
        protected void OnDrawGizmosSelected()
        {
            //Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
}

