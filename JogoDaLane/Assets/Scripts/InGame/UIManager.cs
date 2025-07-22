// UIManager.cs
using UnityEngine;
using TMPro; // Para TextMeshPro

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText; // O 210 na sua imagem
    [SerializeField] private TextMeshProUGUI troopCountText; // O 5/10 na sua imagem (tropas em campo/limite)

    public void UpdateMoneyDisplay(int currentMoney)
    {
        moneyText.text = currentMoney.ToString();
    }

    public void UpdateTroopCountDisplay(int currentTroops, int maxTroops)
    {
        troopCountText.text = $"{currentTroops}/{maxTroops}";
    }
}