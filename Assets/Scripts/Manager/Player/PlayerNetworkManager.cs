using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace CX 
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager player;
        public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("×°±¸")]
        public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public void SetNewMaxHealthValue(int oldValue , int newValue)
        {
            maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newValue);
            PlayerUIManager.instance.PlayerUIHudManager.SetMaxHealthValue(maxHealth.Value);
            currentHealth.Value =maxHealth.Value;
        }
        public void SetNewMaxStaminaValue(int oldValue, int newValue)
        {
            maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEndurancelevel(newValue);
            PlayerUIManager.instance.PlayerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
        }
        public void OnCurrentRightHandWeaponIDChange(int oldValue , int newValue)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newValue));
            player.playerInventoryManager.currentRightHandWeapon = newWeapon; 
            player.playerEquipmentManager.LoadRightWeapon();
        }
        public void OnCurrentLeftHandWeaponIDChange(int oldValue ,int newValue)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newValue));
            player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
            player.playerEquipmentManager.LoadLeftWeapon();

        }
    }
}


