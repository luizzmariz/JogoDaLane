// MatchManager.cs
using UnityEngine;
using System.Collections.Generic; // Para usar List
using System.Linq; // Para usar ToList()

public class MatchManager : MonoBehaviour
{
    [HideInInspector] public static MatchManager instance;

    [Header("Economia")]
    [SerializeField] private int startingMoney = 210; // Dinheiro inicial, como na sua imagem
    [SerializeField] private float moneyGenerationRate = 5f; // Dinheiro gerado por segundo
    private float nextMoneyGenerationTime; // Próximo tempo para gerar dinheiro

    [Header("Limite de Tropas")]
    [SerializeField] private int maxTroopsInField;
    private int currentTroopsInField = 0; // Quantidade atual de tropas em campo

    [Header("UI References")]
    [SerializeField] private UIManager uiManager; // Referência ao UIManager
    [SerializeField] private TroopButtonUI troopButtonUI1;
    [SerializeField] private TroopButtonUI troopButtonUI2;
    [SerializeField] private TroopButtonUI troopButtonUI3;
    [SerializeField] private TroopButtonUI troopButtonUI4;
    [SerializeField] private TroopButtonUI troopButtonUI5;

    [Header("Tropas Disponíveis para Compra (No máximo 5, como na UI)")]

    [SerializeField] private List<CardData> availableTroopsForMatch = new List<CardData>();
    [SerializeField] private CardData minerCardData;

    [Header("ACTION STATE BUTTONS")]
    TroopActionState currentPlayerTroopActionState = TroopActionState.Defend;
    TroopActionState currentEnemyTroopActionState = TroopActionState.Defend;
    [SerializeField] private List<ActionStateButtonUI> actionStateButtons = new List<ActionStateButtonUI>();


    [Header("Ponto de Invocação de Tropas")]
    [SerializeField] private Transform playerTroopSpawnPoint;
    [SerializeField] private Transform enemyTroopSpawnPoint;
    [SerializeField] private Transform playerDefendPoint;
    [SerializeField] private Transform enemyDefendPoint;
    [SerializeField] private List<BaseEnemyStateMachine> activePlayerTroops = new List<BaseEnemyStateMachine>();
    [SerializeField] private List<BaseEnemyStateMachine> activeEnemyTroops = new List<BaseEnemyStateMachine>();

    public int GetCurrentMoney() => currentMoney;


    private int currentMoney;

    [Header("dEBUG")]
    [SerializeField] private List<CardData> debugTroops = new List<CardData>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentMoney = startingMoney;
        nextMoneyGenerationTime = Time.time + 1f; // Inicia a geração 1 segundo após o início

        if (GameManager.instance != null)
        {
            foreach (CardData card in GameManager.instance.GetMatchDeck())
            {
                availableTroopsForMatch.Add(card);
            }
        }
        else
        {
            availableTroopsForMatch = debugTroops;
        }
        // availableTroopsForMatch.Add(minerCardData);
    }

    void Start()
    {
        SetupTroopBuyButtons();
        RefreshMatchUI(); // Atualiza toda a UI inicial
    }

    void Update()
    {
        HandleMoneyGeneration();
    }

    private void HandleMoneyGeneration()
    {
        if (Time.time >= nextMoneyGenerationTime)
        {
            currentMoney += Mathf.RoundToInt(moneyGenerationRate);
            nextMoneyGenerationTime = Time.time + 2f; // Próxima geração em 1 segundo
            RefreshMatchUI(); // Atualiza a UI para refletir o novo dinheiro
        }
    }

    private void SetupTroopBuyButtons()
    {
        troopButtonUI1.Setup(availableTroopsForMatch[0], this);
        troopButtonUI2.Setup(availableTroopsForMatch[1], this);
        troopButtonUI3.Setup(availableTroopsForMatch[2], this);
        troopButtonUI4.Setup(availableTroopsForMatch[3], this);
        // troopButtonUI5.Setup(availableTroopsForMatch[4], this);
    }

    // Chamado para atualizar toda a UI da partida
    public void RefreshMatchUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateMoneyDisplay(currentMoney);
            uiManager.UpdateTroopCountDisplay(currentTroopsInField, maxTroopsInField);
        }

        troopButtonUI1.UpdateInteractability(currentMoney);
        troopButtonUI2.UpdateInteractability(currentMoney);
        troopButtonUI3.UpdateInteractability(currentMoney);
        troopButtonUI4.UpdateInteractability(currentMoney);
        // troopButtonUI5.UpdateInteractability(currentMoney);
    }

    // Chamado pelo TroopButtonUI quando o botão é clicado
    public void TryBuyTroop(CardData troopCard)
    {
        if (currentTroopsInField >= maxTroopsInField)
        {
            // Opcional: Exibir mensagem na UI sobre o limite de tropas
            return;
        }

        if (currentMoney >= troopCard.cost)
        {
            currentMoney -= troopCard.cost;
            SpawnTroop(troopCard.troopPrefab, true);
            currentTroopsInField++; // Incrementa a contagem de tropas em campo
            RefreshMatchUI(); // Atualiza a UI após a compra e invocação
        }
    }

    public void SpawnTroop(GameObject troopPrefab, bool players)
    {
        if (players)
        {
            GameObject spawnedTroop = Instantiate(troopPrefab, playerTroopSpawnPoint.position, playerTroopSpawnPoint.rotation, playerTroopSpawnPoint);
            spawnedTroop.layer = LayerMask.NameToLayer("PlayerTroop");

            BaseEnemyStateMachine troop = spawnedTroop.GetComponent<BaseEnemyStateMachine>();
            troop.SetTroopSide(true, enemyTroopSpawnPoint, playerDefendPoint, playerTroopSpawnPoint);

            troop.SetActionState(currentPlayerTroopActionState);

            activePlayerTroops.Add(troop);
        }
        else
        {
            GameObject spawnedTroop = Instantiate(troopPrefab, enemyTroopSpawnPoint.position, enemyTroopSpawnPoint.rotation, enemyTroopSpawnPoint);
            spawnedTroop.layer = LayerMask.NameToLayer("EnemyTroop");

            spawnedTroop.transform.Find("Visual").GetComponent<SpriteRenderer>().flipX = true;

            BaseEnemyStateMachine troop = spawnedTroop.GetComponent<BaseEnemyStateMachine>();
            troop.SetTroopSide(false, playerTroopSpawnPoint, enemyDefendPoint, enemyTroopSpawnPoint);

            troop.SetActionState(currentEnemyTroopActionState);

            activeEnemyTroops.Add(troop);
        }
    }

    public void OnTroopDied(BaseEnemyStateMachine troop)
    {
        if (!troop.isEnemy)
        {
            activePlayerTroops.Remove(troop);
            currentTroopsInField--;
            if (currentTroopsInField < 0) currentTroopsInField = 0;
            RefreshMatchUI();
        }
        else
        {
            activeEnemyTroops.Remove(troop);
        }
    }

    public void SetTroopActionState(TroopActionState newState, bool players)
    {
        if (players)
        {
            if (currentPlayerTroopActionState == newState)
            {
                return;
            }

            currentPlayerTroopActionState = newState;

            foreach (BaseEnemyStateMachine troop in activePlayerTroops)
            {
                if (troop != null)
                {
                    troop.SetActionState(currentPlayerTroopActionState);
                }
            }

            UpdateActionButtonsVisuals();
        }
        else
        {
            if (currentEnemyTroopActionState == newState)
            {
                return;
            }

            currentEnemyTroopActionState = newState;
            Debug.Log($"inimigos: {currentEnemyTroopActionState}");

            foreach (BaseEnemyStateMachine troop in activeEnemyTroops)
            {
                if (troop != null)
                {
                    troop.SetActionState(currentEnemyTroopActionState);
                }
            }

            UpdateActionButtonsVisuals();
        }       
    }

    private void UpdateActionButtonsVisuals()
    {
        foreach (ActionStateButtonUI button in actionStateButtons)
        {
            button.SetSelected(button.thisButtonState == currentPlayerTroopActionState);
        }
    }

    public void EndGame(TowerStateMachine towerStateMachine)
    {
        if (towerStateMachine.players)
        {
            Debug.Log("ggperdeu");
        }
        else
        {
            Debug.Log("gg ganhou");
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.EndMatch();
        }
    }
}