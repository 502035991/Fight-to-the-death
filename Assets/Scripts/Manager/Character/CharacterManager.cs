using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace CX
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController characterController;

        public CharacterNetworkManager characterNetworkManager;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update()
        {
            if (IsOwner)                
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networRotation.Value = transform.rotation;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    characterNetworkManager.networkPosition.Value ,
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);

                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    characterNetworkManager.networRotation.Value,
                    characterNetworkManager.networkRotationSmoothTime);
            }

        }
        protected virtual void LateUpdate()
        {

        }
    }
}

