using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class MainPanel : MonoBehaviour
{
    [SerializeField] Button startBtn;

    private void Start()
    {
        startBtn.onClick.AddListener(StartGame);
    }
    private void StartGame()
    {
        YooAssets.LoadSceneAsync("Assets/Scenes/GameScene.unity");
    }
}
