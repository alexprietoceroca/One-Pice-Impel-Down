using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DetectorPuerta : MonoBehaviour
{
    [Header("CONFIGURACIÓN")]
    public string nombreEscenaDestino = "sala_3_final";
    public KeyCode teclaInteraccion = KeyCode.E;
    
    [Header("REFERENCIAS VISUALES")]
    public GameObject indicadorUI;        // Texto "Presiona E para entrar"
    public Renderer rendererObjeto;       // Para cambiar color/material
    public Material materialBloqueado;
    public Material materialDesbloqueado;
    
    [Header("SONIDOS")]
    public AudioSource audioSource;
    public AudioClip sonidoBloqueado;
    public AudioClip sonidoDesbloqueado;
    public AudioClip sonidoActivacion;
    
    private bool jugadorEnRango = false;
    private bool puertaDesbloqueada = false;
    
    void Start()
    {
        // Obtener referencia al renderer si no está asignado
        if (rendererObjeto == null)
        {
            rendererObjeto = GetComponent<Renderer>();
        }
        
        // Ocultar indicador UI al inicio
        if (indicadorUI != null)
        {
            indicadorUI.SetActive(false);
        }
        
        // Estado inicial: puerta bloqueada
        ActualizarEstadoPuerta();
        
        Debug.Log("Puerta inicializada. Estado: " + (puertaDesbloqueada ? "DESBLOQUEADA" : "BLOQUEADA"));
    }
    
    void Update()
    {
        // Si el jugador está en rango y la puerta está desbloqueada
        if (jugadorEnRango && puertaDesbloqueada)
        {
            // Mostrar indicador UI
            if (indicadorUI != null && !indicadorUI.activeSelf)
            {
                indicadorUI.SetActive(true);
            }
            
            // Detectar tecla E para cambiar de escena
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                CambiarEscena();
            }
        }
        else
        {
            // Ocultar indicador si no se cumplen las condiciones
            if (indicadorUI != null && indicadorUI.activeSelf)
            {
                indicadorUI.SetActive(false);
            }
        }
        
        // Verificar constantemente si la puerta se ha desbloqueado
        VerificarEstadoDesbloqueo();
    }
    
    void VerificarEstadoDesbloqueo()
    {
        // Actualizar estado según GameManager
        bool nuevoEstado = GameManager.Instance.PuertaDesbloqueada();
        
        if (nuevoEstado != puertaDesbloqueada)
        {
            puertaDesbloqueada = nuevoEstado;
            ActualizarEstadoPuerta();
            
            // Reproducir sonido apropiado
            if (audioSource != null)
            {
                if (puertaDesbloqueada)
                {
                    if (sonidoDesbloqueado != null)
                        audioSource.PlayOneShot(sonidoDesbloqueado);
                }
                else
                {
                    if (sonidoBloqueado != null)
                        audioSource.PlayOneShot(sonidoBloqueado);
                }
            }
        }
    }
    
    void ActualizarEstadoPuerta()
    {
        // Cambiar apariencia visual según estado
        if (rendererObjeto != null)
        {
            if (puertaDesbloqueada)
            {
                if (materialDesbloqueado != null)
                    rendererObjeto.material = materialDesbloqueado;
                else
                    rendererObjeto.material.color = Color.green;
            }
            else
            {
                if (materialBloqueado != null)
                    rendererObjeto.material = materialBloqueado;
                else
                    rendererObjeto.material.color = Color.red;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            Debug.Log("Jugador entró en área de puerta");
            indicadorUI.SetActive(true);
            // Si la puerta está bloqueada, mostrar mensaje
            if (!puertaDesbloqueada)
            {
                Debug.Log("Puerta bloqueada - Busca la pista primero");
                
                // Opcional: Mostrar mensaje temporal
                // StartCoroutine(MostrarMensajeTemporal("¡Encuentra la pista primero!"));
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            Debug.Log("Jugador salió del área de puerta");
            
            // Ocultar indicador UI
            if (indicadorUI != null)
            {
                indicadorUI.SetActive(false);
            }
        }
    }
    
   public  void CambiarEscena()
    {
        if (string.IsNullOrEmpty(nombreEscenaDestino))
        {
            Debug.LogError("Nombre de escena destino vacío");
            return;
        }
        
        Debug.Log("Cambiando a escena: " + nombreEscenaDestino);
        
        // Reproducir sonido de activación
        if (audioSource != null && sonidoActivacion != null)
        {
            audioSource.PlayOneShot(sonidoActivacion);
        }
        
        // Verificar si la escena existe
        if (EscenaExiste(nombreEscenaDestino))
        {
            // Cambiar de escena
            SceneManager.LoadScene(nombreEscenaDestino);
        }
        else
        {
            Debug.LogError("Escena no encontrada: " + nombreEscenaDestino);
            MostrarEscenasDisponibles();
        }
    }
    
    bool EscenaExiste(string nombre)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string ruta = SceneUtility.GetScenePathByBuildIndex(i);
            string nombreEscena = System.IO.Path.GetFileNameWithoutExtension(ruta);
            
            if (nombreEscena == nombre)
                return true;
        }
        return false;
    }
    
    void MostrarEscenasDisponibles()
    {
        Debug.Log("Escenas disponibles:");
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string ruta = SceneUtility.GetScenePathByBuildIndex(i);
            string nombre = System.IO.Path.GetFileNameWithoutExtension(ruta);
            Debug.Log($"- {nombre}");
        }
    }
    
    // Para debug visual en el editor
    void OnDrawGizmos()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            if (puertaDesbloqueada)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            
            if (collider is BoxCollider boxCollider)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
                Gizmos.matrix = Matrix4x4.identity;
            }
        }
    }
}