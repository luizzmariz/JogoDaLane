using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // Não esqueça de importar o TMPro

public class DeckSelectionManager : MonoBehaviour
{
    public static DeckSelectionManager instance;


    [Header("Configuração de Cartas")]
    [SerializeField] private List<CardData> allAvailableCards; // Todas as cartas disponíveis para o jogo
    [SerializeField] private int deckSize = 4;

    [Header("Prefabs de UI")]
    [SerializeField] private GameObject availableCardUIPrefab;
    [SerializeField] private DeckCardUI deckCard1;
    [SerializeField] private DeckCardUI deckCard2;
    [SerializeField] private DeckCardUI deckCard3;
    [SerializeField] private DeckCardUI deckCard4;

    [Header("Containers de UI")]
    [SerializeField] private Transform availableCardsContentParent; // Painel para exibir as cartas disponíveis (dentro de um ScrollRect)
    [SerializeField] private Transform deckCardsContentParent; // Painel para exibir as cartas do deck (4 slots)

    private CardData[] currentDeck;
    private List<AvailableCardUI> instantiatedAvailableCardUIs = new List<AvailableCardUI>();

    // Variáveis para o modo de troca
    private CardData cardToSwapIn = null; // A carta selecionada da lista de disponíveis para ser trocada
    private bool awaitingSwapSelection = false; // Indica se estamos esperando o jogador selecionar um slot no deck para troca

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        currentDeck = new CardData[deckSize];
        InitializeUI();
    }

    void Start()
    {
        PopulateAvailableCardsUI();
    }

    private void InitializeUI()
    {
        currentDeck[0] = allAvailableCards[0];
        deckCard1.Setup(allAvailableCards[0], 0);

        currentDeck[1] = allAvailableCards[1];
        deckCard2.Setup(allAvailableCards[1], 1);

        currentDeck[2] = allAvailableCards[2];
        deckCard3.Setup(allAvailableCards[2], 2);

        currentDeck[3] = allAvailableCards[3];
        deckCard4.Setup(allAvailableCards[3], 3);

    }

    private void PopulateAvailableCardsUI()
    {
        // Limpa qualquer UI de cartas disponíveis anterior
        foreach (Transform child in availableCardsContentParent)
        {
            Destroy(child.gameObject);
        }
        instantiatedAvailableCardUIs.Clear();

        // Popula a UI com todas as cartas disponíveis
        foreach (CardData cardData in allAvailableCards)
        {
            if (!IsCardInDeck(cardData))
            {
                GameObject cardGO = Instantiate(availableCardUIPrefab, availableCardsContentParent);
                AvailableCardUI cardUI = cardGO.GetComponent<AvailableCardUI>();
                if (cardUI != null)
                {
                    cardUI.Setup(cardData);
                    instantiatedAvailableCardUIs.Add(cardUI);
                }
            }
        }
    }

    private void UpdateAllCardUIStates()
    {
        foreach (AvailableCardUI cardUI in instantiatedAvailableCardUIs)
        {
            cardUI.UpdateAddButtonState();
        }

        deckCard1.UpdateSwapButtonState();
        deckCard2.UpdateSwapButtonState();
        deckCard3.UpdateSwapButtonState();
        deckCard4.UpdateSwapButtonState();
    }

    // --- Métodos de Lógica do Deck ---

    public void OnAvailableCardSelected(CardData selectedCard)
    {
        if (!awaitingSwapSelection)
        {
            awaitingSwapSelection = true;

            cardToSwapIn = selectedCard;
        }

        UpdateAllCardUIStates();
    }

    private void AddCardToDeck(CardData cardToAdd)
    {
        if (IsCardInDeck(cardToAdd))
        {
            return;
        }

        for (int i = 0; i < deckSize; i++)
        {
            if (currentDeck[i] == null)
            {
                currentDeck[i] = cardToAdd;
                return;
            }
        }

        // Se chegamos aqui, o deck está cheio
        awaitingSwapSelection = true; // Entra no modo de troca
        cardToSwapIn = cardToAdd; // Define a carta que o jogador quer adicionar
    }

    public void PerformSwap(int deckSlotIndex)
    {
        if (awaitingSwapSelection && cardToSwapIn != null)
        {
            // Verifica se a carta a ser trocada já está no deck (para evitar duplicatas)
            if (IsCardInDeck(cardToSwapIn) && currentDeck[deckSlotIndex] != cardToSwapIn)
            {
                // Reseta o modo de troca, ou permite que o usuário escolha outra carta
                awaitingSwapSelection = false;
                cardToSwapIn = null;
                UpdateAllCardUIStates();
                return;
            }

            // Realiza a troca

            CardData formerDeckCard = currentDeck[deckSlotIndex];

            currentDeck[deckSlotIndex] = cardToSwapIn;

            switch (deckSlotIndex)
            {
                case 0:
                    deckCard1.Setup(cardToSwapIn, 0);
                    break;

                case 1:
                    deckCard2.Setup(cardToSwapIn, 1);
                    break;

                case 2:
                    deckCard3.Setup(cardToSwapIn, 2);
                    break;

                case 3:
                    deckCard4.Setup(cardToSwapIn, 3);
                    break;
            }

            foreach (AvailableCardUI availableCard in instantiatedAvailableCardUIs)
            {
                if (availableCard.GetCardData() == cardToSwapIn)
                {
                    availableCard.Setup(formerDeckCard);
                }
            }

            // Sai do modo de troca
            awaitingSwapSelection = false;
            cardToSwapIn = null;

            UpdateAllCardUIStates();
        }
    }

    // --- Métodos de Verificação ---

    public bool IsCardInDeck(CardData card)
    {
        foreach (CardData deckCard in currentDeck)
        {
            if (deckCard == card)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsDeckFull()
    {
        foreach (CardData card in currentDeck)
        {
            if (card == null)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsAwaitingSwapSelection()
    {
        return awaitingSwapSelection;
    }

    public CardData[] GetCurrentDeck()
    {
        return currentDeck;
    }
}