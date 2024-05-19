using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CX
{
    [CreateAssetMenu(menuName ="Character Effect/Instant Effects/Take Stamina Damage")]
    public class TakeStaminaDamageEffect : InstantCharacterEffect
    {
        public float staminaDamage;
        public override void ProcessEffect(CharacterManager character)
        {   

        }
        private void CalculateStaminaDamage(CharacterManager character)
        {
            if(character.IsOwner)
            {
                character.characterNetworkManager.currentStamina.Value -= staminaDamage;
            }    
        }

    }

}
