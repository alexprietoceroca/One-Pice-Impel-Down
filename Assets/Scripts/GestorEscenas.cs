using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestorEscenas : MonoBehaviour
{
    [Header("CONFIGURACIÓN ESCENA")]
    public string nombreEscenaDestino = "sala_3_final";
    
    [Header("REFERENCIAS")]
    public Button botonPuerta;
    
    void Start()
    {
        if (botonPuerta == null)
        {
            Debug.LogError("Botón de puerta no asignado en GestorEscenas");
            return;
        }
        
        // Configurar evento del botón
        botonPuerta.onClick.AddListener(CambiarEscena);
        
        // Inicialmente desactivado (se activará desde Interaccion_boton)
        botonPuerta.interactable = false;
        botonPuerta.gameObject.SetActive(false);
        
        Debug.Log($"Gestor de escenas configurado para: {nombreEscenaDestino}");
    }
    
    public void CambiarEscena()
    {
        if (string.IsNullOrEmpty(nombreEscenaDestino))
        {
            Debug.LogError("Nombre de escena destino vacío");
            return;
        }
        
        Debug.Log($"Iniciando cambio a escena: {nombreEscenaDestino}");
        
        // Verificar si la escena existe
        if (EscenaExiste(nombreEscenaDestino))
        {
            // Añadir efecto de transición (opcional)
            StartCoroutine(CambiarEscenaConTransicion());
        }
        else
        {
            Debug.LogError($"Escena '{nombreEscenaDestino}' no encontrada en Build Settings");
            MostrarEscenasDisponibles();
        }
    }
    
    System.Collections.IEnumerator CambiarEscenaConTransicion()
    {
        Debug.Log("Preparando cambio de escena...");
        
        // Aquí podrías añadir una animación de fade out
        // Por ejemplo: screenFade.FadeOut(1f);
        // yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene(nombreEscenaDestino);
        yield return null;
    }
    
    bool EscenaExiste(string nombreEscena)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string rutaEscena = SceneUtility.GetScenePathByBuildIndex(i);
            string nombre = System.IO.Path.GetFileNameWithoutExtension(rutaEscena);
            
            if (nombre == nombreEscena)
                return true;
        }
        return false;
    }
    
    void MostrarEscenasDisponibles()
    {
        Debug.Log("Escenas disponibles en Build Settings:");
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string rutaEscena = SceneUtility.GetScenePathByBuildIndex(i);
            string nombre = System.IO.Path.GetFileNameWithoutExtension(rutaEscena);
            Debug.Log($"- {nombre} (Índice: {i})");
        }
    }
    
    // Método para activar el botón desde otros scripts
    public void ActivarBotonPuerta()
    {
        if (botonPuerta != null)
        {
            botonPuerta.gameObject.SetActive(true);
            botonPuerta.interactable = true;
            Debug.Log("Botón de puerta activado desde GestorEscenas");
        }
    }
}