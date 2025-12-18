using UnityEngine;
using UnityEngine.UI;

public class DetectorInteraccion : MonoBehaviour
{
    [Header("CONFIGURACIÓN")]
    public GameObject objetoInteractuable; // El objeto que tiene la funcionalidad
    public string mensajeInteraccion = "Presiona E";
    
    [Header("REFERENCIAS UI")]
    public GameObject panelInteraccion; // Panel que aparece cuando estás cerca
    public Text textoInteraccion;
    
    private bool jugadorEnRango = false;
    
    void Start()
    {
        if (panelInteraccion != null)
        {
            panelInteraccion.SetActive(false);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            
            if (panelInteraccion != null)
            {
                textoInteraccion.text = mensajeInteraccion;
                panelInteraccion.SetActive(true);
            }
            
            Debug.Log($"Jugador entró en rango de {gameObject.name}");
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            
            if (panelInteraccion != null)
            {
                panelInteraccion.SetActive(false);
            }
            
            Debug.Log($"Jugador salió del rango de {gameObject.name}");
        }
    }
    
    void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E))
        {
            Interactuar();
        }
    }
    
    protected virtual void Interactuar()
    {
        Debug.Log($"Interactuando con {gameObject.name}");
        
        // Esta función será sobrescrita por los hijos
    }
}