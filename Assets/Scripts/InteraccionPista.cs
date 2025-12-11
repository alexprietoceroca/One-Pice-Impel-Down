using UnityEngine;

public class InteraccionPista : MonoBehaviour
{
    public float distanciaInteraccion = 3f;
    public KeyCode teclaInteraccion = KeyCode.E;
    public GestorEscenas gestorEscenas;
    
    private GameObject jugador;
    private bool pistaRecogida = false;
    
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        
        if (gestorEscenas == null)
        {
            gestorEscenas = FindObjectOfType<GestorEscenas>();
        }
    }
    
    void Update()
    {
        if (pistaRecogida || jugador == null) return;
        
        float distancia = Vector3.Distance(jugador.transform.position, transform.position);
        
        if (distancia <= distanciaInteraccion && Input.GetKeyDown(teclaInteraccion))
        {
            RecogerPista();
        }
    }
    
    void RecogerPista()
    {
        pistaRecogida = true;
        
        Debug.Log("Pista recogida");
        
        // Desactivar este objeto
        gameObject.SetActive(false);
        
        // Activar el botón en el gestor de escenas
        if (gestorEscenas != null)
        {
            gestorEscenas.ActivarBotonPuerta();
        }
        else
        {
            Debug.LogWarning("No se encontró GestorEscenas");
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanciaInteraccion);
    }
}