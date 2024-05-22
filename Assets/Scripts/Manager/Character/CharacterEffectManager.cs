using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CX
{
    public class CharacterEffectManager : MonoBehaviour
    {
        CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }
    }
}
