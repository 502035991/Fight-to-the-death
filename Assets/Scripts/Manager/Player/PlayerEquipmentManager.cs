using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CX
{
    //玩家装备管理器
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        public WeaponModelInstantiationSlot rightHandSlot;
        public WeaponModelInstantiationSlot leftHandSlot;

        [SerializeField] WeaponManager rightWeaponManager;
        [SerializeField] WeaponManager leftWeaponManager;

        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();

            InitializeWeaponSlot();
        }
        protected override void Start()
        {
            base.Start();
            LoadWeaponsOnBothHands();
        }
        private void InitializeWeaponSlot()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();
            foreach (var weaponSlot in weaponSlots)
            {
                if(weaponSlot.weaonSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
                else if(weaponSlot.weaonSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = weaponSlot; 
                }
            }
        }

        public void LoadWeaponsOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }
        public void SwitchRightWeapon()
        {
            if (!player.IsOwner)
                return;
            player.playerAnimatorManager.PlayerTargetActionAnimation("Swap_Right_Weapon", true ,false);

            WeaponItem selectedWeapon = null;
            player.playerInventoryManager.rightHandWeaponIndex += 1;
            if(player.playerInventoryManager.rightHandWeaponIndex <0 || player.playerInventoryManager.rightHandWeaponIndex >2)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;
            }
            foreach(WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                    player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                    return;
                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }
            else
            {
                float weaponCount = 0;
                WeaponItem firstWeapon =null;
                int firstWeaponPosition = 0;
                for(int i =0;i<player.playerInventoryManager.weaponsInRightHandSlots.Length;i++)
                {
                    if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;

                        if(firstWeapon ==null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponPosition = 1;
                        }
                    }
                }
                if(weaponCount<=1)
                {
                    player.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon =Instantiate(WorldItemDatabase.instance.unarmedWeapon);
                    player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
                }
            }
        }
        public void LoadRightWeapon()
        {
            if(player.playerInventoryManager.currentRightHandWeapon !=null)
            {
                rightHandSlot.UnloadWeapon();
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(player,player.playerInventoryManager.currentRightHandWeapon);
            }
        }
        public void SwitchLeftWeapon()
        {

        }
        public void LoadLeftWeapon()
        {
            if (player.playerInventoryManager.currentLeftHandWeapon != null)
            {
                leftHandSlot.UnloadWeapon();

                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftWeaponManager.SetWeaponDamage(player,player.playerInventoryManager.currentLeftHandWeapon);
            }
        }
    }
}
