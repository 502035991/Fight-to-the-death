using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CX
{
    public class PlayerEffectManager : CharacterEffectManager
    {

        [SerializeField] bool processEffect = false;

        private void Update()
        {
            if(processEffect)
            {
                processEffect = false;

/*                InstantCharacterEffect effect = Instantiate();
                ProcessInstantEffect(effect);*/
            }    
        }
    }
}

