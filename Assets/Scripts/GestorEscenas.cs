using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GestorEscenas : MonoBehaviour
{
    [Header("CONFIGURACIÓN ESCENA")]
    public string nombreEscenaDestino = "sala_3_final";

    [Header("REFERENCIAS BOTONES")]
    public Button botonPuerta;
    public List<Button> botonesDesbloqueables = new List<Button>();
    
    [Header("CONFIGURACIÓN DESBLOQUEO")]
    public bool usarPlayerPrefs = true;
    
    private Dictionary<string, Button> botonesPorNombre = new Dictionary<string, Button>();

    void Start()
    {
        InicializarBotones();
        
        if (botonPuerta != null)
        {
            botonPuerta.onClick.AddListener(CambiarEscena);
            botonPuerta.interactable = false;
            botonPuerta.gameObject.SetActive(false);
        }

        Debug.Log($"Gestor de escenas configurado para: {nombreEscenaDestino}");
    }

    void InicializarBotones()
    {
        // Inicializar diccionario para búsqueda rápida
        foreach (Button boton in botonesDesbloqueables)
        {
            if (boton != null && !botonesPorNombre.ContainsKey(boton.name))
            {
                botonesPorNombre[boton.name] = boton;
                
                // Cargar estado guardado si usa PlayerPrefs
                if (usarPlayerPrefs)
                {
                    bool estado = PlayerPrefs.GetInt("boton_" + boton.name, 0) == 1;
                    boton.interactable = estado;
                    boton.gameObject.SetActive(estado);
                }
                else
                {
                    boton.interactable = false;
                    boton.gameObject.SetActive(false);
                }
            }
        }
    }

    // MÉTODO QUE NECESITAS AÑADIR
    public void DesbloquearBoton(string nombreBoton)
    {
        // Si es el botón de puerta principal
        if (nombreBoton == "botonPuerta" && botonPuerta != null)
        {
            botonPuerta.gameObject.SetActive(true);
            botonPuerta.interactable = true;
            Debug.Log("Botón de puerta desbloqueado");
            
            if (usarPlayerPrefs)
                PlayerPrefs.SetInt("boton_botonPuerta", 1);
            
            return;
        }
        
        // Buscar en la lista de botones desbloqueables
        if (botonesPorNombre.ContainsKey(nombreBoton))
        {
            Button boton = botonesPorNombre[nombreBoton];
            boton.gameObject.SetActive(true);
            boton.interactable = true;
            Debug.Log($"Botón '{nombreBoton}' desbloqueado");
            
            if (usarPlayerPrefs)
                PlayerPrefs.SetInt("boton_" + nombreBoton, 1);
        }
        else
        {
            Debug.LogWarning($"Botón '{nombreBoton}' no encontrado en GestorEscenas");
        }
    }

    // Método existente - manténlo
    public void ActivarBotonPuerta()
    {
        if (botonPuerta != null)
        {
            botonPuerta.gameObject.SetActive(true);
            botonPuerta.interactable = true;
            Debug.Log("Botón de puerta activado desde GestorEscenas");
            
            if (usarPlayerPrefs)
                PlayerPrefs.SetInt("boton_botonPuerta", 1);
        }
    }

    public void CambiarEscena()
    {
        // Tu código existente...
        if (string.IsNullOrEmpty(nombreEscenaDestino))
        {
            Debug.LogError("Nombre de escena destino vacío");
            return;
        }

        Debug.Log($"Iniciando cambio a escena: {nombreEscenaDestino}");

        if (EscenaExiste(nombreEscenaDestino))
        {
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

    public void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}