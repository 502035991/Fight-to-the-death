using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace CX
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        CharacterManager character;

        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero ,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public Vector3 networkPositionVelocity;

        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;

        [Header("Animator")]
        public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> moveAmount = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Flags")]
        public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [Header("Stats")]
        public NetworkVariable<int> endurance = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> currentStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxStamina = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        //�ӿͻ��˵��õ�������
        [ServerRpc]
        public void NotifyTheServerOfACtionAnimationServerRpc(ulong clentId , string animationID , bool applyRootMotion)
        {
            //���������/������ ������ͻ���RPC
            if(IsServer)
            {
                PlayActionAnimationForAllClentsClientRpc(clentId , animationID , applyRootMotion);
            }
        }
        //�ӷ��������͵��ͻ���
        [ClientRpc]
        public void PlayActionAnimationForAllClentsClientRpc(ulong clentId , string animationID ,bool applyRootMotion)
        {
            //ȷ�������ڷ��Ͷ�ִ��
            if(clentId != NetworkManager.Singleton.LocalClientId)
            {
                PerformActionAnimationFromServer(animationID , applyRootMotion);
            }
        }

        private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.applyRootMotion = applyRootMotion;
            character.anim.CrossFade(animationID, 0.2f);
        }
    }

}

