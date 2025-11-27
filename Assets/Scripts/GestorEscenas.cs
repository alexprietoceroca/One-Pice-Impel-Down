using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestorEscenas : MonoBehaviour
{
    [Header("CONFIGURACIÓN CAMBIO ESCENA")]
    public string nombreEscenaDestino = "NombreDeTuEscena"; // ← Pones el nombre exacto aquí
    
    [Header("REFERENCIAS")]
    public Button botonPuerta;
    
    void Start()
    {
        // Verificar que el botón esté asignado
        if (botonPuerta == null)
        {
            Debug.LogError("Botón de puerta no asignado en GestorEscenas");
            return;
        }
        
        // Configurar el evento del botón
        botonPuerta.onClick.AddListener(CambiarEscena);
        
        // Asegurar que el botón empiece desactivado (por si acaso)
        botonPuerta.interactable = false;
        
        Debug.Log($"Gestor de escenas listo. Escena destino: {nombreEscenaDestino}");
    }
    
    // Método para cambiar de escena por NOMBRE
    public void CambiarEscena()
    {
        if (string.IsNullOrEmpty(nombreEscenaDestino))
        {
            Debug.LogError("Nombre de escena destino no asignado");
            return;
        }
        
        Debug.Log($"Intentando cambiar a escena: {nombreEscenaDestino}");
        
        // Verificar si la escena existe en Build Settings
        if (EscenaExiste(nombreEscenaDestino))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
        else
        {
            Debug.LogError($"Escena '{nombreEscenaDestino}' no encontrada en Build Settings");
            // Mostrar todas las escenas disponibles para debug
            Debug.Log("Escenas disponibles en Build Settings:");
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                Debug.Log($"- {sceneName} (Índice: {i})");
            }
        }
    }
    
    // Método para verificar si la escena existe
    private bool EscenaExiste(string nombreEscena)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
            if (sceneName == nombreEscena)
                return true;
        }
        return false;
    }
    
    // Método público para desbloquear el botón (llamado desde InteraccionPista)
    public void DesbloquearBoton()
    {
        if (botonPuerta != null)
        {
            botonPuerta.interactable = true;
            Debug.Log("Botón de puerta DESBLOQUEADO desde GestorEscenas");
        }
    }
}