// TroopButtonUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TroopButtonUI : MonoBehaviour
{
    [SerializeField] private Image troopImage; // O Image do quadrado branco
    [SerializeField] private TextMeshProUGUI costText; // Onde o custo da tropa será exibido
    [SerializeField] private Button buyButton; // O botão que o jogador clicará

    private CardData currentTroopCardData;
    private MatchManager matchManager; // Referência ao MatchManager para invocar e gerenciar dinheiro

    public void Setup(CardData data, MatchManager managerRef)
    {
        currentTroopCardData = data;
        matchManager = managerRef;

        if (data != null)
        {
            troopImage.sprite = data.cardImage;
            costText.text = data.cost.ToString();
            gameObject.SetActive(true); // Garante que o botão/slot está ativo
        }
        else
        {
            // Caso este slot não tenha uma CardData associada (útil se você tiver menos de 5 tropas)
            troopImage.sprite = null;
            costText.text = ""; // Não mostra custo se não houver tropa
            gameObject.SetActive(false); // Desativa o slot vazio
        }

        // Limpa listeners antigos para evitar chamadas duplicadas
        buyButton.onClick.RemoveAllListeners();
        // Adiciona um listener para quando o botão for clicado
        buyButton.onClick.AddListener(OnBuyButtonClicked);

        // Atualiza o estado inicial do botão
        UpdateInteractability(matchManager.GetCurrentMoney());
    }

    // Chamado pelo MatchManager sempre que o dinheiro muda
    public void UpdateInteractability(int currentMoney)
    {
        if (currentTroopCardData != null)
        {
            buyButton.interactable = currentMoney >= currentTroopCardData.cost;
            // Opcional: Mudar cor ou opacidade do botão se não puder ser comprado
            troopImage.color = buyButton.interactable ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f); // Exemplo: escurece se não interativo
        }
        else
        {
            buyButton.interactable = false;
        }
    }

    private void OnBuyButtonClicked()
    {
        if (matchManager != null && currentTroopCardData != null)
        {
            // Tenta comprar e invocar a tropa através do MatchManager
            matchManager.TryBuyTroop(currentTroopCardData);
        }
    }
}