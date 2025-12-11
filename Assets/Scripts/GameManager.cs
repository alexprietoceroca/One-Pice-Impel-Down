using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Variable estática para acceder desde cualquier script
    public static GameManager Instance;
    
    [Header("ESTADO DEL JUEGO")]
    public bool pistaVista = false;  // ¿El jugador ya vio la pista?
    
    [Header("REFERENCIAS")]
    public GameObject indicadorPuerta;  // UI que muestra "Presiona E"
    
    void Awake()
    {
        // Patrón Singleton para acceso global
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Inicializar estado
        pistaVista = false;
        
        if (indicadorPuerta != null)
            indicadorPuerta.SetActive(false);
    }
    
    // Método para llamar cuando se ve la pista
    public void MarcarPistaVista()
    {
        pistaVista = true;
        Debug.Log("¡Pista vista! Puerta desbloqueada.");
        
        // Aquí podrías activar sonido, efectos, etc.
    }
    
    // Método para verificar si se puede usar la puerta
    public bool PuertaDesbloqueada()
    {
        return pistaVista;
    }
}