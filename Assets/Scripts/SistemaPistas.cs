using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SistemaPistas : MonoBehaviour
{
    [Header("Configuración")]
    public float distanciaDeteccion = 3f;
    public KeyCode teclaInteraccion = KeyCode.E;
    public float velocidadZoom = 2f;

    [Header("Referencias UI")]
    public GameObject proximityPrompt;
    public GameObject panelZoom;
    public Image imagenPistaZoom;

    [Header("Configuración Cámara")]
    public Camera camaraJugador;
    public float zoomSize = 0.5f;

    private bool estaEnRango = false;
    private bool estaMirando = false;
    private bool enModoZoom = false;
    private Vector3 posicionOriginalCamara;
    private float tamañoOriginalCamara;

    void Start()
    {
        // Ocultar UI inicialmente
        if (proximityPrompt != null) proximityPrompt.SetActive(false);
        if (panelZoom != null) panelZoom.SetActive(false);

        // Obtener referencia a la cámara si no está asignada
        if (camaraJugador == null)
            camaraJugador = Camera.main;

        if (camaraJugador != null)
        {
            posicionOriginalCamara = camaraJugador.transform.position;
            tamañoOriginalCamara = camaraJugador.orthographic ? camaraJugador.orthographicSize : camaraJugador.fieldOfView;
        }
    }

    void Update()
    {
        if (enModoZoom)
        {
            ManejarSalidaZoom();
            return;
        }

        VerificarProximidadYMirada();
        ManejarInteraccion();
    }

    void VerificarProximidadYMirada()
    {
        // Verificar proximidad
        float distancia = Vector3.Distance(transform.position, camaraJugador.transform.position);
        estaEnRango = distancia <= distanciaDeteccion;

        if (estaEnRango)
        {
            // Verificar si está mirando hacia la pista
            RaycastHit hit;
            Vector3 direccion = (transform.position - camaraJugador.transform.position).normalized;
            
            if (Physics.Raycast(camaraJugador.transform.position, direccion, out hit, distanciaDeteccion))
            {
                estaMirando = (hit.collider.gameObject == gameObject);
            }
            else
            {
                estaMirando = false;
            }
        }
        else
        {
            estaMirando = false;
        }

        // Actualizar UI del prompt
        if (proximityPrompt != null)
            proximityPrompt.SetActive(estaEnRango && estaMirando);
    }

    void ManejarInteraccion()
    {
        if (estaEnRango && estaMirando && Input.GetKeyDown(teclaInteraccion))
        {
            ActivarModoZoom();
        }
    }

    void ManejarSalidaZoom()
    {
        // Salir del zoom con ESC, click derecho o click fuera del panel
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1) || 
            (Input.GetMouseButtonDown(0) && panelZoom != null && !RectTransformUtility.RectangleContainsScreenPoint(
                panelZoom.GetComponent<RectTransform>(), Input.mousePosition)))
        {
            DesactivarModoZoom();
        }
    }

    void ActivarModoZoom()
    {
        enModoZoom = true;
        
        // Ocultar prompt
        if (proximityPrompt != null) proximityPrompt.SetActive(false);
        
        // Mostrar panel de zoom
        if (panelZoom != null) panelZoom.SetActive(true);

        // Iniciar animación de zoom
        StartCoroutine(AnimacionZoom(true));
    }

    void DesactivarModoZoom()
    {
        enModoZoom = false;
        
        // Ocultar panel de zoom
        if (panelZoom != null) panelZoom.SetActive(false);

        // Restaurar cámara
        StartCoroutine(AnimacionZoom(false));
    }

    IEnumerator AnimacionZoom(bool zoomIn)
    {
        Vector3 posicionObjetivo;
        float tamañoObjetivo;

        if (zoomIn)
        {
            // Calcular posición para el zoom (más cerca de la pista)
            Vector3 direccion = (transform.position - camaraJugador.transform.position).normalized;
            posicionObjetivo = transform.position - direccion * 1f;
            tamañoObjetivo = zoomSize;
        }
        else
        {
            posicionObjetivo = posicionOriginalCamara;
            tamañoObjetivo = tamañoOriginalCamara;
        }

        float duracion = 0.5f;
        float tiempo = 0f;
        Vector3 posicionInicial = camaraJugador.transform.position;
        float tamañoInicial = camaraJugador.orthographic ? camaraJugador.orthographicSize : camaraJugador.fieldOfView;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracion;

            // Interpolación suave
            camaraJugador.transform.position = Vector3.Lerp(posicionInicial, posicionObjetivo, progreso);
            
            if (camaraJugador.orthographic)
                camaraJugador.orthographicSize = Mathf.Lerp(tamañoInicial, tamañoObjetivo, progreso);
            else
                camaraJugador.fieldOfView = Mathf.Lerp(tamañoInicial, tamañoObjetivo, progreso);

            yield return null;
        }

        // Asegurar valores finales
        camaraJugador.transform.position = posicionObjetivo;
        if (camaraJugador.orthographic)
            camaraJugador.orthographicSize = tamañoObjetivo;
        else
            camaraJugador.fieldOfView = tamañoObjetivo;
    }
}