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
        [SerializeField] Button startBtn;

        private void Start()
        {
            startBtn.onClick.AddListener(StartBtnFunction);
        }
        private void StartBtnFunction()
        {
            StartNetworkAsHost();
            StartGame();
        }
        private void StartNetworkAsHost()
        {
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
        private void StartGame()
        {
            YooAssets.LoadSceneAsync("Assets/Scenes/GameScene.unity", LoadSceneMode.Single);
        }
    }
}

