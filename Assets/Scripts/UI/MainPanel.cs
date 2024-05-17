using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YooAsset;

namespace CX
{
    public class MainPanel : MonoBehaviour
    {
        public static MainPanel instance;
        [Header("Buttons")]
        [SerializeField] Button startBtn;
        [SerializeField] Button pressStartBtn;
        [SerializeField] Button mainMenuLoadGameBtn;
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button deleteCharacterPopUpConfirmButton;

        [Header("Menus")]
        [SerializeField] GameObject titlePressStartMenu;//第一个界面
        [SerializeField] GameObject titleScreenMainMenu;//开始菜单界面
        [SerializeField] GameObject titleScreenLoadMenu;//load界面

        [Header("Pop Ups")]
        [SerializeField] GameObject noCharacterSlotsPopup;
        [SerializeField] Button noCharacterSlotsButton;
        [SerializeField] GameObject deleteCharacterSlotPopUp;

        [Header("Character Slots")]
        public CharacterSlot currentCharacterSlot = CharacterSlot.NO_SLOT;

        [Header("Title Screen Inputs")]
        [SerializeField] bool deleteCharacterSlot =false;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            startBtn.onClick.AddListener(StartBtnFunction);
            pressStartBtn.onClick.AddListener(StartNetworkAsHost);
            mainMenuLoadGameBtn.onClick.AddListener(OpenLoadGameMenum);
            loadMenuReturnButton.onClick.AddListener(CloseLoadGameMenum);
        }
        private void StartBtnFunction()
        {
            WorldSaveGameManager.instance.AttempToCreatNewGame();
        }
        private void OpenLoadGameMenum()
        {
            titleScreenMainMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }
        private void CloseLoadGameMenum()
        {
            titleScreenMainMenu.SetActive(true);
            titleScreenLoadMenu.SetActive(false);

            mainMenuLoadGameBtn.Select();
        }
        private void StartNetworkAsHost()
        {
            titlePressStartMenu.SetActive(false);
            titleScreenMainMenu.SetActive(true);

            if (ParrelSync.ClonesManager.IsClone())
            {
                NetworkManager.Singleton.Shutdown();

                NetworkManager.Singleton.StartClient();
            }
            else
            {
                NetworkManager.Singleton.StartHost();
            }
        }
        public void SelectCharacterSlot(CharacterSlot slot)
        {
            currentCharacterSlot = slot;
        }
        public void SelectNoSlot()
        {
            currentCharacterSlot = CharacterSlot.NO_SLOT;
        }
        public void AttemptToDeleteCharacterSlot()
        {
            if(currentCharacterSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }
        }
        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            loadMenuReturnButton.Select();
            WorldSaveGameManager.instance.DeleteGame(currentCharacterSlot);
        }
        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            loadMenuReturnButton.Select();
        }
        //没有剩余存档插槽时弹窗
        private void DisplayDisplayNoFreeCharacterSlotsPopUp() 
        {
            noCharacterSlotsPopup.SetActive(true);
            noCharacterSlotsButton.Select();
        }

    }
}

