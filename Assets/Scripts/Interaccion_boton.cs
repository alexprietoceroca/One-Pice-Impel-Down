using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Interaccion_boton : MonoBehaviour
{
    [Header("REFERENCIAS UI")]
    public GameObject proximityPrompt;    // Panel "E Interact"
    public GameObject panelPista;         // Panel con la pista
    public Button botonPuerta;            // Botón para cambiar de escena
    
    [Header("BOTÓN CERRAR PISTA")]
    public Button botonCerrarPista;       // Botón X para cerrar panel
    
    [Header("REFERENCIA CLUE DETECTOR")]
    public Clue_Detector clueDetector;
    
    [Header("CONFIGURACIÓN")]
    public float distanciaInteraccion = 3f;
    
    [HideInInspector] public bool jugadorEnRango = false;
    private bool pistaVista = false;      // Para controlar si ya vio la pista
    
    void Start()
    {
        // Buscar Clue_Detector si no está asignado
        if (clueDetector == null)
        {
            clueDetector = GetComponent<Clue_Detector>();
        }
        
        // Ocultar todos los elementos UI al inicio
        if (proximityPrompt != null)
            proximityPrompt.SetActive(false);
            
        if (panelPista != null)
            panelPista.SetActive(false);
            
        if (botonPuerta != null)
        {
            botonPuerta.gameObject.SetActive(false);
            botonPuerta.interactable = false;
        }
        
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
        
        // Actualizar prompt UI
        if (proximityPrompt != null)
        {
            proximityPrompt.SetActive(jugadorEnRango && !pistaVista);
        }
        
        // Detectar tecla E para abrir pista
        if (jugadorEnRango && Keyboard.current.eKey.wasPressedThisFrame && !pistaVista)
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
            pistaVista = true;
            Debug.Log("Panel de pista abierto");
            
            // Ocultar prompt
            if (proximityPrompt != null)
                proximityPrompt.SetActive(false);
            
            // Activar botón de puerta
            if (botonPuerta != null)
            {
                botonPuerta.gameObject.SetActive(true);
                botonPuerta.interactable = true;
                Debug.Log("Botón de puerta ACTIVADO - Listo para cambiar a escena");
            }
        }
    }
    
    public void CerrarPanelPista()
    {
        if (panelPista != null)
        {
            panelPista.SetActive(false);
            Debug.Log("Panel de pista cerrado");
            
            // El botón de puerta PERMANECE visible después de ver la pista
            // No lo ocultamos porque el jugador necesita poder pulsarlo
        }
    }
    
    // Para debug en pantalla
    void OnGUI()
    {
        if (Debug.isDebugBuild)
        {
            GUI.Label(new Rect(10, 10, 300, 20), $"Pista vista: {pistaVista}");
            GUI.Label(new Rect(10, 30, 300, 20), $"Botón puerta activo: {botonPuerta != null && botonPuerta.gameObject.activeSelf}");
        }
    }
}