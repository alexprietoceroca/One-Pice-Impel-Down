using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DetectorPuerta : MonoBehaviour
{
    [Header("CONFIGURACIÓN PUERTA")]
    public string nombreEscenaDestino;
    public bool requierePista = true;
    
    [Header("REFERENCIAS UI")]
    public GameObject panelInteraccion;
    public Text textoInteraccion;
    
    private bool puertaDesbloqueada = false;
    private bool jugadorEnRango = false;
    
    void Start()
    {
        Debug.Log($"🚀 DetectorPuerta INICIADO en: {gameObject.name}");
        
        // NO desactivar el panel aquí
        if (panelInteraccion == null)
            Debug.LogError($"❌ panelInteraccion NO asignado");
            
        if (textoInteraccion == null)
            Debug.LogError($"❌ textoInteraccion NO asignado");
        
        if (!requierePista)
        {
            puertaDesbloqueada = true;
            Debug.Log("ℹ️ Puerta no requiere pista, desbloqueada desde inicio.");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            Debug.Log($"✅ Jugador ENTRÓ en trigger de puerta: {gameObject.name}");
            
            // Mostrar panel solo si está desbloqueada
            if (panelInteraccion != null && (puertaDesbloqueada || !requierePista))
            {
                panelInteraccion.SetActive(true);
                Debug.Log($"📱 PanelInteraccion ACTIVADO para puerta");
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            Debug.Log($"❌ Jugador SALIÓ del trigger de puerta: {gameObject.name}");
            
            if (panelInteraccion != null)
            {
                panelInteraccion.SetActive(false);
            }
        }
    }
    
    void Update()
    {
        if (!jugadorEnRango) return;
        
        bool teclaEPresionada = false;
        
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            teclaEPresionada = UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame;
        }
        
        if (!teclaEPresionada)
        {
            try { teclaEPresionada = Input.GetKeyDown(KeyCode.E); }
            catch (System.InvalidOperationException) { }
        }
        
        if (teclaEPresionada)
        {
            if (puertaDesbloqueada || !requierePista)
            {
                CambiarEscena();
            }
            else
            {
                Debug.Log("🚫 Puerta bloqueada. Encuentra la pista correcta.");
            }
        }
    }
    
    public void DesbloquearPuerta()
    {
        puertaDesbloqueada = true;
        Debug.Log($"🔓 ¡Puerta {gameObject.name} desbloqueada!");
        
        if (jugadorEnRango && panelInteraccion != null)
        {
            panelInteraccion.SetActive(true);
        }
    }
    
    void CambiarEscena()
    {
        if (string.IsNullOrEmpty(nombreEscenaDestino))
        {
            Debug.LogError("❌ Nombre de escena destino no asignado");
            return;
        }
        
        Debug.Log($"🔄 Cambiando a escena: {nombreEscenaDestino}");
        
        // Verificar si la escena existe
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
            if (sceneName == nombreEscenaDestino)
            {
                SceneManager.LoadScene(nombreEscenaDestino);
                return;
            }
        }
        
        Debug.LogError($"❌ Escena '{nombreEscenaDestino}' no encontrada en Build Settings");
    }
}