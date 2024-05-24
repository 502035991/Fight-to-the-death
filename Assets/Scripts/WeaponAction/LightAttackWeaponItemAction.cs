using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CX
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";
        public override void AttemptToPerformAction(PlayerManager player, WeaponItem weponPerformingAction)
        {
            base.AttemptToPerformAction(player, weponPerformingAction);
            if (!player.IsOwner)
                return;
            if(player.playerNetworkManager.currentStamina.Value <=0)            
                return;            
            if (!player.isGrounded)
                return;
            if (player.isPerformingAction)
                return;
            PerformLightAttack(player, weponPerformingAction);
        }

        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weponPerformingAction)
        {
            if(playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(light_Attack_01, true);
            }
            if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
            {

            }
        }
    }

}
