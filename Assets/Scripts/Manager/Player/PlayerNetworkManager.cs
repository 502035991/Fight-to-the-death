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

        [Header("装备")]
        public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public void SetCharacterActionHand(bool rightHandedAction)
        {
            isUsingRightHand.Value = rightHandedAction;
            isUsingLeftHand.Value = !rightHandedAction;
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
        public void OnCurrentWeaponBeingUsedIDChange(int oldValue, int newValue)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newValue));
            player.playerCombatManager.PerformWeaponBasedAction(newWeapon.rb_Action, newWeapon);
        }

        //通知服务器进行武器对应的动作
        [ServerRpc]
        public void NotifyTheServerOfWeaponActionServerRpc(ulong clientID , int actionID , int weaponID)
        {
            if(IsServer)
            {
                NotifyTheServerOfWeaponActionClientRpc(clientID,actionID,weaponID);
            }
        }
        [ClientRpc]
        private void NotifyTheServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)
        {
            if(clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformWeaponBasedAction(actionID, weaponID);
            }
        }
        private void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);
            if(weaponAction != null)
            {
                weaponAction.AttemptToPerformAction(player, WorldItemDatabase.instance.GetWeaponByID(weaponID));
            }
            else
            {

            }
        }
    }
}


