using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CX
{
    [CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Test Action")]
    public class WeaponItemAction : ScriptableObject
    {
        public int actionID;

        public virtual void AttemptToPerformAction(PlayerManager player , WeaponItem weponPerformingAction)
        {
            if(player.IsOwner)
            {
                player.playerNetworkManager.currentWeaponBeingUsed.Value = weponPerformingAction.itemID;
            }
        }
    }

}
