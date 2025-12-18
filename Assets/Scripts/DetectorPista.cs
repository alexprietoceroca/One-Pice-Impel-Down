using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DetectorPista : MonoBehaviour
{
    [Header("CONFIGURACIÓN PISTA")]
    public GameObject panelPista; // Arrastra el panel de pista aquí
    public string mensajePista = "¡Pista encontrada! Esta es la correcta.";
    public bool esPistaCorrecta = false;
    
    [Header("REFERENCIAS UI")]
    public GameObject panelInteraccion; // Panel "Presiona E"
    public Text textoInteraccion;       // Texto dentro del panel
    
    [Header("REFERENCIA PUERTA")]
    public DetectorPuerta puertaParaDesbloquear; // Puerta que desbloquea
    
    private bool jugadorEnRango = false;
    private bool pistaVista = false;
    
    void Start()
    {
        Debug.Log($"🔧 Iniciando DetectorPista en: {gameObject.name}");
        
        // Asegurar que el panel de interacción está OCULTO al inicio
        if (panelInteraccion != null)
        {
            panelInteraccion.SetActive(false);
            Debug.Log($"✅ PanelInteraccion desactivado al inicio");
        }
        else
        {
            Debug.LogError($"❌ ERROR: PanelInteraccion no asignado en {gameObject.name}");
        }
        
        // Asegurar que el panel de pista está OCULTO al inicio
        if (panelPista != null)
        {
            panelPista.SetActive(false);
            Debug.Log($"✅ PanelPista desactivado al inicio");
        }
        else
        {
            Debug.LogError($"❌ ERROR: PanelPista no asignado en {gameObject.name}");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Solo reaccionar al jugador
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            Debug.Log($"🎯 Jugador entró en área de: {gameObject.name}");
            
            // Mostrar panel de interacción
            if (panelInteraccion != null)
            {
                panelInteraccion.SetActive(true);
                Debug.Log($"📱 Mostrando: Presiona E");
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            Debug.Log($"👋 Jugador salió de área de: {gameObject.name}");
            
            // Ocultar panel de interacción
            if (panelInteraccion != null)
            {
                panelInteraccion.SetActive(false);
            }
        }
    }
    
    void Update()
    {
        // Solo procesar si el jugador está en rango
        if (!jugadorEnRango) return;
        
        // Detectar si se presiona la tecla E
        bool teclaPresionada = false;
        
        // Sistema de Input de Unity (nuevo)
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            teclaPresionada = UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame;
        }
        
        // Sistema de Input antiguo (respaldo)
        if (!teclaPresionada)
        {
            try
            {
                teclaPresionada = Input.GetKeyDown(KeyCode.E);
            }
            catch (System.InvalidOperationException)
            {
                // Ignorar si no está disponible
            }
        }
        
        // Si se presiona E, interactuar
        if (teclaPresionada)
        {
            Debug.Log($"⌨️ Tecla E presionada cerca de {gameObject.name}");
            Interactuar();
        }
    }
    
    void Interactuar()
    {
        Debug.Log($"🎮 Interactuando con pista: {gameObject.name}");
        
        // Verificar que tenemos el panel de pista
        if (panelPista == null)
        {
            Debug.LogError($"❌ No hay panel de pista asignado!");
            return;
        }
        
        // Activar el panel de pista
        panelPista.SetActive(true);
        Debug.Log($"📄 Abriendo panel de pista");
        
        // Ocultar el panel de interacción
        if (panelInteraccion != null)
        {
            panelInteraccion.SetActive(false);
        }
        
        // Si es la primera vez que se ve la pista
        if (!pistaVista)
        {
            pistaVista = true;
            
            // Si es la pista correcta, desbloquear la puerta
            if (esPistaCorrecta && puertaParaDesbloquear != null)
            {
                puertaParaDesbloquear.DesbloquearPuerta();
                Debug.Log($"🔓 ¡PISTA CORRECTA! Desbloqueando puerta...");
            }
        }
    }
}