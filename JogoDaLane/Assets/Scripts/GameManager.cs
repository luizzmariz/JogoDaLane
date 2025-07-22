#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Screens")]
    public GameObject startScreen;
    public GameObject lobbyScreen;
    public GameObject optionsScreen;
    public GameObject deckEditScreen;
    public GameObject playMatchScreen;
    public GameObject matchOptions;

    [Header("ServerConnection")]
    public bool isConnected = false; 
    public bool isTryingToConnect = false; 
    public bool connectedAux = false; 
    public bool serverClosed = false; 
    public GameObject startButton;

    
    [Header("Match")]
    public bool isInMatch;
    public bool isQueueing;
    public bool matchVariablesAssigned = false;
    public GameObject onQueueListPanel;
    public GameObject matchController;
    CardData[] playerMatchDeck;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        startScreen.SetActive(true);
        optionsScreen.SetActive(false);
        lobbyScreen.SetActive(false);
        deckEditScreen.SetActive(false);
        playMatchScreen.SetActive(false);

        isInMatch = false;
        isQueueing = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScreenChanger("options");
        }
        if(connectedAux)
        {
            ScreenChanger("startGame");
            connectedAux = false;
        }
        if(serverClosed)
        {
            QuitGame();
        }
    }

    public void ScreenChanger(string screen)
    {
        switch(screen)
        {
            case "startGame":
                startScreen.SetActive(false);
                lobbyScreen.SetActive(true);

                ScreenChanger("playScreen");
            break;

            case "options":
            if((!isInMatch))
            {
                if(!optionsScreen.activeInHierarchy)
                {
                    optionsScreen.SetActive(true);
                }
                else if(optionsScreen.activeInHierarchy)
                {
                    optionsScreen.SetActive(false);
                }
            }
            break;

            case "exitGame":
            QuitGame();
            break;

            case "deckScreen":
            playMatchScreen.SetActive(false);
            deckEditScreen.SetActive(true);
            break;

            case "playScreen":
            deckEditScreen.SetActive(false);
            playMatchScreen.SetActive(true);
            // SocketEmit("getQueueInfo");
            break;

            case "createMatch":
            optionsScreen.SetActive(false);

            SetMatchDeck();
            SceneManager.LoadScene("Match");
            break;

            case "endMatch":
            SceneManager.LoadScene("LobbyMenu");
                Start();
            break;

            default:
            Debug.Log("some button pressed");
            break;
        }
    }

    void SetMatchDeck()
    {
        playerMatchDeck = DeckSelectionManager.instance.GetCurrentDeck();
    }

    public CardData[] GetMatchDeck()
    {
        return playerMatchDeck;
    }

    public void EndMatch()
    {
        ScreenChanger("endMatch");
    }

    public bool playerStatus()
    {
        return this.isInMatch;
    }
    
    public void QuitGame()
    {
        if(isInMatch)
        {
            ScreenChanger("endMatch");
        }
        if(isConnected)
        {
            ScreenChanger("disconnect");
        }

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}

