using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathScreenController : MonoBehaviour
{
    [Header("Configuración de la Muerte")]
    public float tiempoTransicion = 2f; // Tiempo en segundos para la transición
    
    [Header("Componentes UI")]
    public Image imagenMuerte; // Arrastra aquí la Image del Canvas
    public GameObject canvasCompleto; // Arrastra aquí el GameObject del Canvas
    
    private bool estaActivo = false;
    private float tiempoTranscurrido = 0f;
    
    void Start()
    {
        // Asegurarse de que la pantalla de muerte está desactivada al inicio
        if (canvasCompleto != null)
        {
            canvasCompleto.SetActive(false);
        }
        
        // Configurar la imagen transparente al inicio
        if (imagenMuerte != null)
        {
            Color colorInicial = imagenMuerte.color;
            colorInicial.a = 0f; // Transparente
            imagenMuerte.color = colorInicial;
        }
    }
    
    public void ActivarMuerte()
    {
        if (!estaActivo)
        {
            estaActivo = true;
            
            // Activar el canvas
            if (canvasCompleto != null)
            {
                canvasCompleto.SetActive(true);
            }
            
            // Iniciar la corrutina de transición
            StartCoroutine(TransicionMuerte());
        }
    }
    
    IEnumerator TransicionMuerte()
    {
        tiempoTranscurrido = 0f;
        
        // Realizar la transición durante el tiempo especificado
        while (tiempoTranscurrido < tiempoTransicion)
        {
            tiempoTranscurrido += Time.deltaTime;
            
            // Calcular el progreso (0 a 1)
            float progreso = Mathf.Clamp01(tiempoTranscurrido / tiempoTransicion);
            
            // Actualizar el alpha de la imagen
            if (imagenMuerte != null)
            {
                Color nuevoColor = imagenMuerte.color;
                nuevoColor.a = progreso; // Va de 0 (transparente) a 1 (completamente visible)
                imagenMuerte.color = nuevoColor;
            }
            
            yield return null; // Esperar al siguiente frame
        }
        
        // Asegurarse de que al final está completamente visible
        if (imagenMuerte != null)
        {
            Color colorFinal = imagenMuerte.color;
            colorFinal.a = 1f;
            imagenMuerte.color = colorFinal;
        }
        
        Debug.Log("Transición de muerte completada");
        
        // Aquí puedes agregar más lógica, como reiniciar el nivel
        // o mostrar opciones al jugador
    }
    
    // Método para reiniciar la pantalla de muerte
    public void ReiniciarPantalla()
    {
        estaActivo = false;
        tiempoTranscurrido = 0f;
        
        if (imagenMuerte != null)
        {
            Color colorReset = imagenMuerte.color;
            colorReset.a = 0f;
            imagenMuerte.color = colorReset;
        }
        
        if (canvasCompleto != null)
        {
            canvasCompleto.SetActive(false);
        }
    }
}