using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CX
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] UI_StatBar staminaBar;
        public void SetNewStaminaValue(float oldValue , float newValue)
        {
            staminaBar.SetStat(newValue);
        }
        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }
    }

}
