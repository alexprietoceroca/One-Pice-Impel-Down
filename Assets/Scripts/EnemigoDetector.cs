using UnityEngine;

public class EnemigoDetector : MonoBehaviour
{
    [Header("Configuración del Enemigo")]
    public float radioDeteccion = 5f; // Radio en el que detecta al jugador
    public float velocidadLimiteAgachado = 4f; // Velocidad máxima permitida cuando estás agachado
    
    [Header("Referencias")]
    public GameObject jugador; // Arrastra aquí el objeto del jugador
    public DeathScreenController deathScreen; // Arrastra aquí el Canvas de muerte
    
    private Mov_Personaje scriptJugador; // Para acceder a la velocidad del jugador
    private bool jugadorDetectado = false;
 
    
    void Start()
    {
        // Buscar automáticamente al jugador si no está asignado
        if (jugador == null)
        {
            jugador = GameObject.FindGameObjectWithTag("Player");
        }
        
        // Obtener el script del jugador
        if (jugador != null)
        {
            scriptJugador = jugador.GetComponent<Mov_Personaje>();
        }
        
        // Si no se encontró el jugador, mostrar advertencia
        if (scriptJugador == null)
        {
            Debug.LogWarning("No se encontró el script Mov_Personaje en el jugador");
        }
    }
    
    void Update()
    {
        // Si no hay jugador asignado, no hacer nada
        if (jugador == null || scriptJugador == null) return;
        
        // Calcular la distancia entre el enemigo y el jugador
        float distancia = Vector3.Distance(transform.position, jugador.transform.position);
        
        // Si el jugador está dentro del radio de detección
        if (distancia <= radioDeteccion)
        {
            // Verificar si la velocidad del jugador supera el límite permitido
            if (scriptJugador.velocidadCalculada > velocidadLimiteAgachado)
            {
                // Matar al jugador
                MatarJugador();
            }
        }
    }
    
    void MatarJugador()
    {
        Debug.Log("¡El enemigo te ha detectado moviéndote demasiado rápido!");
        
        // Activar la pantalla de muerte si existe
        if (deathScreen != null)
        {
            deathScreen.ActivarMuerte();
        }
        
        // Desactivar el jugador (o hacer lo que necesites)
        if (jugador != null)
        {
           scriptJugador.ResetPosition();
        }
    }
    
    // Método para dibujar el radio de detección en el editor (solo para visualización)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}