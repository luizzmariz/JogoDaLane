
using UnityEngine;
using System.Collections.Generic;
using System; // Necessário para usar System.Array e System.Enum

public class EnemyManager : MonoBehaviour
{
    [Header("Economia do Oponente Virtual")]
    [SerializeField] private int startingEnemyMoney = 250; // Dinheiro inicial do inimigo
    [SerializeField] private float enemyMoneyGenerationRate = 25f; // Quantidade de dinheiro gerada por segundo
    private float nextEnemyMoneyGenerationTime; // Próximo tempo para gerar dinheiro
    private int currentEnemyMoney; // Dinheiro atual do inimigo

    [Header("Tropas Inimigas")]
    [SerializeField] private List<CardData> availableEnemyTroops; // As CardDatas das tropas que o inimigo pode invocar

    [Header("Comportamento da I.A.")]
    [SerializeField] private float minBehaviorChangeInterval = 5f; // Tempo mínimo para a I.A. mudar de comportamento
    [SerializeField] private float maxBehaviorChangeInterval = 15f; // Tempo máximo para a I.A. mudar de comportamento
    private float nextBehaviorChangeTime; // Próximo tempo para a I.A. mudar de comportamento

    [Header("Invocação de Tropas da I.A.")]
    [SerializeField] private float minTroopBuyInterval = 3f; // Tempo mínimo para a I.A. comprar uma tropa
    [SerializeField] private float maxTroopBuyInterval = 8f; // Tempo máximo para a I.A. comprar uma tropa
    private float nextTroopBuyTime; // Próximo tempo para a I.A. comprar uma tropa
    [SerializeField] private int maxEnemyTroopsInField = 10; // Limite de tropas inimigas em campo
    private int currentEnemyTroopsInField = 0; // Contagem atual de tropas inimigas em campo

    // Referência ao MatchManager (que é um Singleton) para interagir com o jogo

    void Awake()
    {
        currentEnemyMoney = startingEnemyMoney;
        nextEnemyMoneyGenerationTime = Time.time + 1f; // Começa a gerar dinheiro 1 segundo após Awake
        nextBehaviorChangeTime = Time.time + UnityEngine.Random.Range(minBehaviorChangeInterval, maxBehaviorChangeInterval);
        nextTroopBuyTime = Time.time + UnityEngine.Random.Range(minTroopBuyInterval, maxTroopBuyInterval);   
    }

    void Start()
    {
        if (MatchManager.instance == null)
        {
            enabled = false;
        }
        // Define um estado inicial aleatório para a I.A.
        Array allStates = Enum.GetValues(typeof(TroopActionState));
        SetEnemyTroopActionState(TroopActionState.Defend);
    }

    void Update()
    {
        HandleEnemyMoneyGeneration();
        HandleAIBehaviorChange();
        HandleAITroopBuying();
    }

    private void HandleEnemyMoneyGeneration()
    {
        if (Time.time >= nextEnemyMoneyGenerationTime)
        {
            currentEnemyMoney += Mathf.RoundToInt(enemyMoneyGenerationRate);
            nextEnemyMoneyGenerationTime = Time.time + 2f;
        }
    }

    private void HandleAIBehaviorChange()
    {
        if (Time.time >= nextBehaviorChangeTime)
        {
            Array allStates = Enum.GetValues(typeof(TroopActionState));
            TroopActionState newRandomState = (TroopActionState)allStates.GetValue(UnityEngine.Random.Range(0, allStates.Length));
            
            // Define o novo estado e notifica as tropas inimigas
            SetEnemyTroopActionState(newRandomState);

            nextBehaviorChangeTime = Time.time + UnityEngine.Random.Range(minBehaviorChangeInterval, maxBehaviorChangeInterval);
        }
    }

    // Define o estado de ação da I.A. inimiga e notifica suas tropas
    private void SetEnemyTroopActionState(TroopActionState newState)
    {
        if (MatchManager.instance != null)
        {
            MatchManager.instance.SetTroopActionState(newState, false);
        }
    }

    private void HandleAITroopBuying()
    {
        if (Time.time >= nextTroopBuyTime)
        {
            if (currentEnemyTroopsInField < maxEnemyTroopsInField && availableEnemyTroops.Count > 0)
            {
                // Filtra as tropas que o inimigo pode pagar
                List<CardData> affordableTroops = new List<CardData>();
                foreach(CardData troop in availableEnemyTroops)
                {
                    if (currentEnemyMoney >= troop.cost)
                    {
                        affordableTroops.Add(troop);
                    }
                }

                if (affordableTroops.Count > 0)
                {
                    // Escolhe uma tropa aleatoriamente entre as que pode pagar
                    CardData troopToBuy = affordableTroops[UnityEngine.Random.Range(0, affordableTroops.Count)];

                    currentEnemyMoney -= troopToBuy.cost;
                    SpawnEnemyTroop(troopToBuy.troopPrefab, troopToBuy); // Passa a CardData para fins de depuração
                    currentEnemyTroopsInField++;
                }
                else
                {
                    // Debug.Log("I.A. Inimiga não tem dinheiro para comprar nenhuma tropa disponível.");
                }
            }
            nextTroopBuyTime = Time.time + UnityEngine.Random.Range(minTroopBuyInterval, maxTroopBuyInterval);
        }
    }

    // Invocação de Tropa pelo inimigo
    private void SpawnEnemyTroop(GameObject troopPrefab, CardData cardData)
    {
        MatchManager.instance.SpawnTroop(troopPrefab, false);
    }
}