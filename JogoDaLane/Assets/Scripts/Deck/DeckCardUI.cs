using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckCardUI : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private Button swapButton; // Este será o botão "Trocar"

    private CardData currentCardData;
    private int deckSlotIndex;

    public void Setup(CardData data, int index)
    {
        currentCardData = data;
        deckSlotIndex = index;

        if (data != null)
        {
            cardNameText.text = data.cardName;
            if (data.cardImage != null)
            {
                cardImage.sprite = data.cardImage;
            }
            else
            {
                cardImage.sprite = null;
            }
            gameObject.SetActive(true); // Ativa o objeto se houver uma carta
        }
        else
        {
            // Se não houver carta neste slot, desativa o objeto ou mostra um slot vazio
            cardNameText.text = "Slot Vazio";
            cardImage.sprite = null; // Ou um sprite de slot vazio
            gameObject.SetActive(true); // Mantém ativo para mostrar slot vazio ou pode desativar
        }
        
        swapButton.onClick.RemoveAllListeners();
        swapButton.onClick.AddListener(OnSwapButtonClicked);
    }

    public void UpdateSwapButtonState()
    {
        if (DeckSelectionManager.instance != null)
        {
            if (!swapButton.gameObject.activeSelf)
            {
                swapButton.gameObject.SetActive(true);
            }
            else
            {
                swapButton.gameObject.SetActive(false);
            }

            // O botão "Trocar" só é interativo se o manager estiver no modo de troca (esperando uma carta para entrar)
            // E se este slot do deck não estiver vazio.
            swapButton.interactable = DeckSelectionManager.instance.IsAwaitingSwapSelection() && currentCardData != null;
            swapButton.GetComponentInChildren<TextMeshProUGUI>().text = "Trocar"; // Garante o texto correto
        }
    }

    private void OnSwapButtonClicked()
    {
        if (DeckSelectionManager.instance != null && DeckSelectionManager.instance.IsAwaitingSwapSelection())
        {
            DeckSelectionManager.instance.PerformSwap(deckSlotIndex);
        }
    }
}