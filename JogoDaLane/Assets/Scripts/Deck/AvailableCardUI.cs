using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AvailableCardUI : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private Button addButton;

    private CardData currentCardData;

    public void Setup(CardData data)
    {
        currentCardData = data;

        cardNameText.text = data.cardName;
        if (data.cardImage != null)
        {
            cardImage.sprite = data.cardImage;
        }
        else
        {
            cardImage.sprite = null; // Garante que não haja imagem antiga se não houver nova
        }

        addButton.onClick.RemoveAllListeners(); // Limpa listeners anteriores
        addButton.onClick.AddListener(OnAddButtonClicked);
        UpdateAddButtonState();
    }

    public CardData GetCardData()
    {
        return currentCardData;
    }

    public void UpdateAddButtonState()
    {
        if (DeckSelectionManager.instance != null)
        {
            // Desabilita o botão se a carta já estiver no deck
            addButton.interactable = !DeckSelectionManager.instance.IsCardInDeck(currentCardData);
            addButton.GetComponentInChildren<TextMeshProUGUI>().text = "Colocar no Deck";

            // Se o deck estiver cheio E esta carta não estiver no deck,
            // e o manager estiver esperando uma seleção para troca,
            // esta carta se torna a "candidata a troca". O botão não muda de texto aqui.
            if (DeckSelectionManager.instance.IsDeckFull() && !DeckSelectionManager.instance.IsCardInDeck(currentCardData) && DeckSelectionManager.instance.IsAwaitingSwapSelection())
            {
                // O botão continua como "Colocar no Deck" ou pode ser "Selecionar para Troca"
                // mas a lógica principal para troca é tratada quando o botão "Trocar" no deck é clicado.
                // Aqui apenas garantimos que ainda é clicável se for uma opção para trocar POR ela.
                addButton.interactable = true; // Permite selecionar ela como a nova carta
            }
        }
    }

    private void OnAddButtonClicked()
    {
        if (DeckSelectionManager.instance != null)
        {
            DeckSelectionManager.instance.OnAvailableCardSelected(currentCardData);
        }
    }
}