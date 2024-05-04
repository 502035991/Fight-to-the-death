using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace CX
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        float horizontal;
        float vertical;
        
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue , float verticalValue)
        {
            character.anim.SetFloat("Horizontal", horizontalValue , 0.1f ,Time.deltaTime);
            character.anim.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
        }
        public virtual void PlayerTargetActionAnimation(string targetAnimation , bool isPerformingAction , bool applyRootMotion = true , bool canRotate =false ,bool canMove = false)
        {

            //�ں��� time ���ʱ����ڣ�ʹ����Ϊ animation �Ķ������룬ʹ��������������
            character.anim.CrossFade(targetAnimation, 0.2f);
            //���������������λ�ƣ������ｺ��������
            character.applyRootMotion = applyRootMotion;
            //ΪTrue������������
            character.isPerformingAction =isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;


            character.characterNetworkManager.NotifyTheServerOfACtionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}

