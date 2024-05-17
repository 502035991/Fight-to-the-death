using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CX
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        protected override void Awake()
        {
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
        }
        protected override void Update()
        {
            base .Update();
            if(!IsOwner)
            {
                return;
            }
            playerLocomotionManager.HandleAllMovement();
            playerStatsManager.RegenerateStamina();
        }
        protected override void LateUpdate()
        {
            if (!IsOwner)
                return;

            base.LateUpdate();
            PlayerCamera.instance.HandleAllCameraActions();

        }       
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;
                WorldSaveGameManager.instance.player = this;

                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.PlayerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenerationTimer;

                playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEndurancelevel(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerStatsManager.CalculateStaminaBasedOnEndurancelevel(playerNetworkManager.endurance.Value);
                PlayerUIManager.instance.PlayerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);

            }
        }
        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;
        }
        public void LoadGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;

            Vector3 myPos = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition,currentCharacterData.zPosition);
            transform.position = myPos;
        }

    }
}

