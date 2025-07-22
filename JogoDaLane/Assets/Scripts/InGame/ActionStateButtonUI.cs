// ActionStateButtonUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Usar TextMeshPro se seus botões de UI o utilizam

public enum TroopActionState
{
    Attack,  // As tropas avançam e atacam inimigos e a base inimiga
    Defend,  // As tropas mantêm sua posição (ou patrulham uma pequena área) e atacam inimigos que se aproximem
    Retreat  // As tropas recuam em direção à base do jogador
}
public class ActionStateButtonUI : MonoBehaviour
{
    [SerializeField] private Button actionButton; // Arraste o componente Button aqui
    [SerializeField] private TextMeshProUGUI buttonText; // Opcional: para feedback visual se for o estado ativo, arrastar o TextMeshProUGUI filho do botão
    [SerializeField] public TroopActionState thisButtonState; // NOVO: Defina qual estado este botão representa no Inspector

    private MatchManager matchManager; // Referência ao MatchManager

    void Awake()
    {
        // Tenta encontrar o MatchManager na cena.
        // É melhor atribuir manualmente no Inspector ou via MatchManager.Instance,
        // mas FindObjectOfType funciona se houver apenas um.
        matchManager = FindObjectOfType<MatchManager>();
        if (matchManager == null)
        {
            Debug.LogError("ActionStateButtonUI: MatchManager não encontrado na cena!");
            enabled = false;
        }
    }

    void OnEnable()
    {
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(OnActionButtonClicked);
        }
        // Opcional: Se quiser que o texto do botão mude, configure-o aqui
        if (buttonText != null)
        {
            buttonText.text = thisButtonState.ToString().ToUpper();
        }
    }

    void OnDisable()
    {
        if (actionButton != null)
        {
            actionButton.onClick.RemoveListener(OnActionButtonClicked);
        }
    }

    private void OnActionButtonClicked()
    {
        if (matchManager != null)
        {
            matchManager.SetTroopActionState(thisButtonState, true); // Notifica o MatchManager
        }
    }

    // Método para atualizar o feedback visual do botão (chamado pelo MatchManager)
    public void SetSelected(bool isSelected)
    {
        if (actionButton != null)
        {
            ColorBlock cb = actionButton.colors;
            // Se selecionado, use uma cor mais vibrante; caso contrário, use a cor padrão
            cb.normalColor = isSelected ? new Color(0.2f, 0.8f, 0.2f, 1f) : Color.white; // Verde para ativo, branco para inativo
            cb.highlightedColor = isSelected ? new Color(0.3f, 0.9f, 0.3f, 1f) : new Color(0.96f, 0.96f, 0.96f, 1f);
            cb.pressedColor = isSelected ? new Color(0.1f, 0.7f, 0.1f, 1f) : new Color(0.8f, 0.8f, 0.8f, 1f); // Cor quando pressionado
            actionButton.colors = cb;
        }
    }
}