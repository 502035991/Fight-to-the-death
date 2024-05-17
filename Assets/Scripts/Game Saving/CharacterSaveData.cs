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
        [Header("游戏时间")]
        public float secondsPlayed;
        [Header("位置坐标")]
        public float xPosition = 0;
        public float yPosition = 1f;
        public float zPosition = 0;
    }

}
