using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DetectorPuerta : MonoBehaviour
{
    [Header("REFERENCIAS UI")]
    public Button botonPuerta;           // El botón que aparecerá
    public GameObject textoInstruccion;  // Texto opcional de instrucción
    
    [Header("CONFIGURACIÓN ESCENA")]
    public string nombreEscenaDestino = "sala_3_final";
    
    [Header("AJUSTES DE DETECCIÓN")]
    public bool ocultarAlSalir = true;   // Ocultar botón al salir del área
    public float retrasoMostrar = 0.3f;  // Pequeño retraso para evitar parpadeos
    public bool usarTeclaE = true;       // Permitir activar con tecla E
    
    [Header("FEEDBACK VISUAL")]
    public Color colorNormal = Color.blue;
    public Color colorActivado = Color.green;
    public Material materialActivado;    // Material opcional para cambiar al detectar
    
    // Variables internas
    private bool jugadorEnRango = false;
    private bool botonVisible = false;
    private Renderer objetoRenderer;
    private Material materialOriginal;
    
    void Start()
    {
        // Obtener referencia al renderer para cambiar color/material
        objetoRenderer = GetComponent<Renderer>();
        if (objetoRenderer != null)
        {
            materialOriginal = objetoRenderer.material;
        }
        
        // Configurar estado inicial
        if (botonPuerta != null)
        {
            botonPuerta.gameObject.SetActive(false);
            botonPuerta.interactable = false;
            // Configurar el evento del botón
            botonPuerta.onClick.AddListener(CambiarEscena);
        }
        
        if (textoInstruccion != null)
        {
            textoInstruccion.SetActive(false);
        }
        
        Debug.Log($"DetectorPuerta inicializado. Escena destino: {nombreEscenaDestino}");
        
        // Verificar que la escena existe
        VerificarEscenaDestino();
    }
    
    void Update()
    {
        // Opción alternativa: activar con tecla E si está en rango
        if (usarTeclaE && jugadorEnRango && !botonVisible && Input.GetKeyDown(KeyCode.E))
        {
            MostrarBotonPuerta();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            Debug.Log("Jugador entró en área de puerta");
            
            // Cambiar feedback visual
            CambiarFeedbackVisual(true);
            
            // Mostrar botón con pequeño retraso para evitar parpadeos
            Invoke(nameof(MostrarBotonPuerta), retrasoMostrar);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            Debug.Log("Jugador salió del área de puerta");
            
            // Restaurar feedback visual
            CambiarFeedbackVisual(false);
            
            // Ocultar botón si está configurado
            if (ocultarAlSalir && botonVisible)
            {
                OcultarBotonPuerta();
            }
        }
    }
    
    void MostrarBotonPuerta()
    {
        // Solo mostrar si el jugador sigue en rango
        if (!jugadorEnRango || botonVisible) return;
        
        if (botonPuerta != null)
        {
            botonPuerta.gameObject.SetActive(true);
            botonPuerta.interactable = true;
            botonVisible = true;
            
            Debug.Log($"Botón de puerta mostrado. Preparado para: {nombreEscenaDestino}");
            
            // Feedback opcional: sonido, animación, etc.
        }
        
        if (textoInstruccion != null)
        {
            textoInstruccion.SetActive(true);
        }
    }
    
    void OcultarBotonPuerta()
    {
        if (botonPuerta != null && botonVisible)
        {
            botonPuerta.gameObject.SetActive(false);
            botonPuerta.interactable = false;
            botonVisible = false;
            Debug.Log("Botón de puerta ocultado");
        }
        
        if (textoInstruccion != null)
        {
            textoInstruccion.SetActive(false);
        }
    }
    
    void CambiarFeedbackVisual(bool activado)
    {
        if (objetoRenderer == null) return;
        
        if (activado)
        {
            if (materialActivado != null)
            {
                objetoRenderer.material = materialActivado;
            }
            else
            {
                objetoRenderer.material.color = colorActivado;
            }
        }
        else
        {
            if (materialOriginal != null)
            {
                objetoRenderer.material = materialOriginal;
            }
            else
            {
                objetoRenderer.material.color = colorNormal;
            }
        }
    }
    
    void CambiarEscena()
    {
        if (string.IsNullOrEmpty(nombreEscenaDestino))
        {
            Debug.LogError("Nombre de escena destino vacío");
            return;
        }
        
        Debug.Log($"Iniciando cambio a escena: {nombreEscenaDestino}");
        
        // Verificar que la escena existe
        if (!EscenaExiste(nombreEscenaDestino))
        {
            Debug.LogError($"Escena '{nombreEscenaDestino}' no encontrada en Build Settings");
            MostrarEscenasDisponibles();
            return;
        }
        
        // Aquí puedes añadir efectos de transición si quieres
        // Ejemplo: StartCoroutine(TransicionYCambio());
        
        // Cambiar escena directamente
        SceneManager.LoadScene(nombreEscenaDestino);
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
    
    void VerificarEscenaDestino()
    {
        if (!EscenaExiste(nombreEscenaDestino))
        {
            Debug.LogWarning($"Advertencia: Escena '{nombreEscenaDestino}' no encontrada en Build Settings");
            Debug.Log("Escenas disponibles:");
            MostrarEscenasDisponibles();
        }
    }
    
    void MostrarEscenasDisponibles()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string rutaEscena = SceneUtility.GetScenePathByBuildIndex(i);
            string nombre = System.IO.Path.GetFileNameWithoutExtension(rutaEscena);
            Debug.Log($"  [{i}] {nombre}");
        }
    }
    
    // Para debug visual en el editor
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = colorNormal;
        }
        else
        {
            Gizmos.color = jugadorEnRango ? colorActivado : colorNormal;
        }
        
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            if (collider is BoxCollider boxCollider)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
                Gizmos.matrix = Matrix4x4.identity;
            }
        }
    }
    
    // Mostrar información en pantalla (debug)
    void OnGUI()
    {
        if (Debug.isDebugBuild)
        {
            GUI.Label(new Rect(10, 100, 300, 20), $"Player_Detecter: Jugador en rango: {jugadorEnRango}");
            GUI.Label(new Rect(10, 120, 300, 20), $"Botón visible: {botonVisible}");
        }
    }
}