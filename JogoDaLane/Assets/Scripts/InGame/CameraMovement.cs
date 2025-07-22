using UnityEngine;
using UnityEngine.UI; // Para usar a classe Button
using UnityEngine.EventSystems; // Para IPointerDownHandler e IPointerUpHandler

public class CameraMovement : MonoBehaviour
{
    [Header("Configurações de Movimento da Câmera")]
    [SerializeField] private float moveSpeed = 5f; // Velocidade de movimento lateral da câmera
    [SerializeField] private float minXPosition = -10f; // Limite esquerdo (posição X mínima)
    [SerializeField] private float maxXPosition = 10f; // Limite direito (posição X máxima)

    [Header("Botões de Controle UI")]
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;

    // Flags para controlar o movimento contínuo enquanto o botão está pressionado
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    // Referência à câmera que será movida
    private Camera mainCamera;

    void Awake()
    {
        // Tenta encontrar a câmera principal na cena
        mainCamera = Camera.main; 
        if (mainCamera == null)
        {
            Debug.LogError("Nenhuma Main Camera encontrada na cena! Certifique-se de que sua câmera tem a tag 'MainCamera' ou atribua-a manualmente.");
            enabled = false; // Desativa o script se não encontrar a câmera
            return;
        }

        // Adiciona listeners para os eventos de Pressione/Solte nos botões de UI
        // Isso permite o movimento contínuo enquanto o botão é segurado
        if (moveLeftButton != null)
        {
            AddEventTrigger(moveLeftButton, EventTriggerType.PointerDown, OnMoveLeftButtonDown);
            AddEventTrigger(moveLeftButton, EventTriggerType.PointerUp, OnMoveLeftButtonUp);
            AddEventTrigger(moveLeftButton, EventTriggerType.PointerExit, OnMoveLeftButtonUp); // Para parar se o mouse sair do botão enquanto pressionado
        }
        if (moveRightButton != null)
        {
            AddEventTrigger(moveRightButton, EventTriggerType.PointerDown, OnMoveRightButtonDown);
            AddEventTrigger(moveRightButton, EventTriggerType.PointerUp, OnMoveRightButtonUp);
            AddEventTrigger(moveRightButton, EventTriggerType.PointerExit, OnMoveRightButtonUp); // Para parar se o mouse sair do botão enquanto pressionado
        }
    }

    // Método auxiliar para adicionar eventos ao EventTrigger de um botão
    private void AddEventTrigger(Button button, EventTriggerType eventType, UnityEngine.Events.UnityAction action)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener((data) => action());
        trigger.triggers.Add(entry);
    }

    void Update()
    {
        Vector3 currentPosition = mainCamera.transform.position;
        Vector3 targetPosition = currentPosition;

        // Calcula a nova posição X com base nos botões pressionados
        if (isMovingLeft)
        {
            targetPosition.x -= moveSpeed * Time.deltaTime;
        }
        if (isMovingRight)
        {
            targetPosition.x += moveSpeed * Time.deltaTime;
        }

        // Limita a posição X da câmera dentro dos limites definidos
        targetPosition.x = Mathf.Clamp(targetPosition.x, minXPosition, maxXPosition);

        // Atualiza a posição da câmera
        mainCamera.transform.position = targetPosition;
    }

    // --- Funções de Callback para os Eventos dos Botões ---

    public void OnMoveLeftButtonDown()
    {
        isMovingLeft = true;
    }

    public void OnMoveLeftButtonUp()
    {
        isMovingLeft = false;
    }

    public void OnMoveRightButtonDown()
    {
        isMovingRight = true;
    }

    public void OnMoveRightButtonUp()
    {
        isMovingRight = false;
    }
}