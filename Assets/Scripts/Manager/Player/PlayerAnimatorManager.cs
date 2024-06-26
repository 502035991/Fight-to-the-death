using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CX
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        private void OnAnimatorMove()
        {
            if(player.applyRootMotion)
            {
                Vector3 velocity = player.anim.deltaPosition;
                player.characterController.Move(velocity);
                player.transform.rotation *= player.anim.deltaRotation;
            }
        }
    }
}


