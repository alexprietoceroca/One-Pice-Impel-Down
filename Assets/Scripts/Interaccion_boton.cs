using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Interaccion_boton : MonoBehaviour
{
    [Header("REFERENCIAS UI")]
    public GameObject proximityPrompt;    // Panel "E Interact"
    public GameObject panelPista;         // Panel con la pista
    public Button botonCerrarPista;       // Botón X para cerrar panel
    
    [Header("REFERENCIA CLUE DETECTOR")]
    public Clue_Detector clueDetector;
    
    [Header("CONFIGURACIÓN")]
    public float distanciaInteraccion = 3f;
    
    [HideInInspector] public bool jugadorEnRango = false;
    
    void Start()
    {
        // Buscar Clue_Detector si no está asignado
        if (clueDetector == null)
        {
            clueDetector = GetComponent<Clue_Detector>();
        }
        
        // Ocultar UI al inicio
        if (proximityPrompt != null)
            proximityPrompt.SetActive(false);
            
        if (panelPista != null)
            panelPista.SetActive(false);
        
        // Configurar botón de cerrar pista
        if (botonCerrarPista != null)
        {
            botonCerrarPista.onClick.AddListener(CerrarPanelPista);
        }
        
        Debug.Log("Sistema de pista inicializado");
    }
    
    void Update()
    {
        // Usar playerCanPressE del Clue_Detector
        if (clueDetector != null)
        {
            jugadorEnRango = clueDetector.playerCanPressE;
        }
        
        // Actualizar prompt UI (solo mostrar si la pista no ha sido vista)
        if (proximityPrompt != null)
        {
            bool mostrarPrompt = jugadorEnRango && !GameManager.Instance.pistaVista;
            proximityPrompt.SetActive(mostrarPrompt);
        }
        
        // Detectar tecla E para abrir pista (solo si no se ha visto)
        if (jugadorEnRango && Keyboard.current.eKey.wasPressedThisFrame && !GameManager.Instance.pistaVista)
        {
            AbrirPanelPista();
        }
        
        // Cerrar con ESC si el panel está abierto
        if (panelPista != null && panelPista.activeSelf && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CerrarPanelPista();
        }
    }
    
    void AbrirPanelPista()
    {
        if (panelPista != null)
        {
            panelPista.SetActive(true);
            
            // MARCAR LA PISTA COMO VISTA EN EL GAME MANAGER
            GameManager.Instance.MarcarPistaVista();
            
            Debug.Log("Panel de pista abierto - Puerta ahora está desbloqueada");
            
            // Ocultar prompt
            if (proximityPrompt != null)
                proximityPrompt.SetActive(false);
        }
    }
    
    public void CerrarPanelPista()
    {
        if (panelPista != null)
        {
            panelPista.SetActive(false);
            Debug.Log("Panel de pista cerrado");
        }
    }
    
    // Para debug en pantalla
    void OnGUI()
    {
        if (Debug.isDebugBuild)
        {
            GUI.Label(new Rect(10, 10, 300, 20), $"Pista vista: {GameManager.Instance.pistaVista}");
            GUI.Label(new Rect(10, 30, 300, 20), $"Jugador en rango pista: {jugadorEnRango}");
        }
    }
}