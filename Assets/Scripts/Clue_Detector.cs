using UnityEngine;

public class Clue_Detector : MonoBehaviour
{
    [Header("ESTADO DE DETECCIÓN")]
    public bool playerCanPressE = false; // Cambiado a público
    
    [Header("REFERENCIA INTERACCION")]
    public Interaccion_boton interaccionBoton;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCanPressE = true;
            Debug.Log("Jugador detectado - Puede pulsar E");
            
            // Notificar a Interaccion_boton si existe
            if (interaccionBoton != null)
            {
                interaccionBoton.jugadorEnRango = true;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCanPressE = false;
            Debug.Log("Jugador salió del área");
            
            // Notificar a Interaccion_boton si existe
            if (interaccionBoton != null)
            {
                interaccionBoton.jugadorEnRango = false;
            }
        }
    }
}