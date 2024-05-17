using CX;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    public bool isSave;

    public PlayerManager player;
    [Header("保存角色写入器")]
    private SaveFileDataWniter saveFileDataWriter;

    [Header("当前角色数据")]
    public CharacterSaveData currentCharacterData;
    public CharacterSlot currentCharacterSlotBeingUsed;
    private string saveFileName;

    [Header("角色插槽")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;
    public CharacterSaveData characterSlot06;
    public CharacterSaveData characterSlot07;
    public CharacterSaveData characterSlot08;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        if(isSave)
        {
            isSave = false;
            SaveGame();
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadAllCharacterProfiles();
    }
    public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
    {
        string fileName ="";
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                fileName = "CharacterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "CharacterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "CharacterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "CharacterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "CharacterSlot_05";
                break;
            case CharacterSlot.CharacterSlot_06:
                fileName = "CharacterSlot_06";
                break;
            case CharacterSlot.CharacterSlot_07:
                break;
            case CharacterSlot.CharacterSlot_08:
                break;
            case CharacterSlot.CharacterSlot_09:
                break;
            case CharacterSlot.CharacterSlot_10:
                break;
        }
        return fileName;
    }
    //尝试创建一个新的游戏
    public void AttempToCreatNewGame()
    {
        saveFileDataWriter = new SaveFileDataWniter();

        saveFileDataWriter.saveDataDirectoryPath = Application.dataPath;
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }
    }
    public void LoadGame()
    {
        saveFileName =DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWriter = new SaveFileDataWniter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
    }
    public void SaveGame()
    {
        //将当前文件保存在一个文件名下
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWriter = new SaveFileDataWniter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
        //写入Json文件，保存到机器
        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
    }
    public void DeleteGame(CharacterSlot characterSlot)
    {
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

        saveFileDataWriter = new SaveFileDataWniter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        saveFileDataWriter.DeleteSaveFile();
    }
    private void LoadAllCharacterProfiles()
    {
        saveFileDataWriter =new SaveFileDataWniter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        characterSlot01 = saveFileDataWriter.LoadSaveFile();
    }
    private IEnumerator LoadWorldScene()
    {
        YooAssets.LoadSceneAsync("Assets/Scenes/GameScene.unity", LoadSceneMode.Single);

        player.LoadGameDataToCurrentCharacterData(ref currentCharacterData);

        yield return null;
    }
}
