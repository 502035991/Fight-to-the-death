using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace CX
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int horizontal;
        int vertical;
        
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue , float verticalValue, bool isSprinting)
        {
            float horizontalAmount = horizontalValue;
            float verticalAmount = verticalValue;
            if (isSprinting)
            {
                verticalAmount = 2;
            }
            character.anim.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.anim.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
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

