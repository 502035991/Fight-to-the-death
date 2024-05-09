using CX;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [SerializeField] bool startGameAsClient;

    [HideInInspector] public PlayerUIHudManager PlayerUIHudManager;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        PlayerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if(startGameAsClient)
        {
            startGameAsClient = false;

            NetworkManager.Singleton.Shutdown();

            NetworkManager.Singleton.StartClient();
        }

    }
}
