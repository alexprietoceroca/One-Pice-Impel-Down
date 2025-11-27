using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InteraccionPista : MonoBehaviour
{
    [Header("CONFIGURACIÓN")]
    public float distanciaInteraccion = 3f;
    
    [Header("REFERENCIAS UI")]
    public GameObject proximityPrompt;
    public GameObject panelPista;
    
    [Header("REFERENCIA GESTOR ESCENAS")]
    public GestorEscenas gestorEscenas;
    
    [Header("INPUT SYSTEM")]
    public PlayerInput playerInput; // Referencia al PlayerInput component
    
    private bool jugadorCerca = false;
    private GameObject jugador;
    private InputAction interactAction;

    void Start()
    {
        // Buscar jugador por tag
        jugador = GameObject.FindGameObjectWithTag("Player");
        
        // Configurar el Input System
        if (playerInput == null)
        {
            playerInput = FindObjectOfType<PlayerInput>();
            if (playerInput == null)
            {
                Debug.LogError("No se encontró PlayerInput en la escena");
            }
        }
        
        // Obtener la acción "Interact" del Input System
        if (playerInput != null)
        {
            interactAction = playerInput.actions.FindAction("Interact");
            if (interactAction != null)
            {
                interactAction.performed += OnInteract;
                interactAction.Enable();
                Debug.Log("Acción Interact configurada correctamente");
            }
            else
            {
                Debug.LogError("No se encontró la acción 'Interact' en el Input System");
            }
        }
        
        // Verificar referencias UI
        if (proximityPrompt == null)
            Debug.LogError("ProximityPrompt no asignado en el inspector");
        else
            proximityPrompt.SetActive(false);
            
        if (panelPista == null)
            Debug.LogError("PanelPista no asignado en el inspector");
        else
            panelPista.SetActive(false);
            
        if (gestorEscenas == null)
            Debug.LogError("GestorEscenas no asignado en el inspector");
            
        Debug.Log("Sistema de pista inicializado. Distancia: " + distanciaInteraccion);
    }

    void OnDestroy()
    {
        // Limpiar el evento cuando se destruye el objeto
        if (interactAction != null)
        {
            interactAction.performed -= OnInteract;
        }
    }

    void Update()
    {
        if (jugador == null) 
        {
            jugador = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        
        // Calcular distancia
        float distancia = Vector3.Distance(transform.position, jugador.transform.position);
        bool estabaCerca = jugadorCerca;
        jugadorCerca = distancia <= distanciaInteraccion;
        
        // DEBUG: Mostrar distancia en consola
        if (jugadorCerca && !estabaCerca)
        {
            Debug.Log($"Jugador ENTRÓ en rango. Distancia: {distancia:F2}");
        }
        else if (!jugadorCerca && estabaCerca)
        {
            Debug.Log($"Jugador SALIÓ del rango. Distancia: {distancia:F2}");
        }
        
        // Mostrar/ocultar prompt
        if (proximityPrompt != null)
        {
            proximityPrompt.SetActive(jugadorCerca);
        }
        
        // Cerrar con ESC (funciona con ambos sistemas de input)
        if (panelPista != null && panelPista.activeSelf && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Cerrando pista con ESC");
            CerrarPista();
        }
    }

    // Método que se llama cuando se presiona la tecla de Interact
    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Acción Interact detectada (Nuevo Input System)");
        
        if (jugadorCerca)
        {
            Debug.Log("Jugador cerca - Abriendo pista");
            AbrirPista();
        }
        else
        {
            Debug.Log("Acción Interact detectada pero jugador NO está en rango");
        }
    }

    public void AbrirPista()
    {
        if (panelPista != null)
        {
            panelPista.SetActive(true);
            Debug.Log("PanelPista activado");
            
            if (proximityPrompt != null)
            {
                proximityPrompt.SetActive(false);
                Debug.Log("ProximityPrompt desactivado");
            }
            
            // DESBLOQUEAR BOTÓN a través del GestorEscenas
            if (gestorEscenas != null)
            {
                gestorEscenas.DesbloquearBoton();
                Debug.Log("Botón de puerta desbloqueado a través de GestorEscenas");
            }
            else
            {
                Debug.LogError("GestorEscenas no asignado - no se puede desbloquear botón");
            }
        }
        else
        {
            Debug.LogError("PanelPista es null - no se puede abrir");
        }
    }

    public void CerrarPista()
    {
        if (panelPista != null)
        {
            panelPista.SetActive(false);
            Debug.Log("PanelPista cerrado");
        }
    }
    
    // Visualizar el rango de interacción en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaInteraccion);
    }
    
    // Visualizar siempre en el editor (opcional)
    void OnDrawGizmos()
    {
        Gizmos.color = jugadorCerca ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaInteraccion);
    }
}