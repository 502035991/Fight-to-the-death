using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CX
{
    [Serializable]
    public class CharacterSaveData 
    {
        public string characterName = "Character";
        [Header("��Ϸʱ��")]
        public float secondsPlayed;
        [Header("λ������")]
        public float xPosition = 0;
        public float yPosition = 1f;
        public float zPosition = 0;
    }

}
