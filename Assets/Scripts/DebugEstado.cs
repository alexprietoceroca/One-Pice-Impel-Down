using UnityEngine;

public class DebugEstado : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== DEBUG ESTADO DE OBJETOS ===");
        Debug.Log($"Este objeto: {gameObject.name} - Activo: {gameObject.activeSelf}");
        
        // Verificar padre
        if (transform.parent != null)
        {
            Debug.Log($"Padre: {transform.parent.name} - Activo: {transform.parent.gameObject.activeSelf}");
        }
        
        // Verificar todos los hijos
        foreach (Transform child in transform)
        {
            Debug.Log($"Hijo: {child.name} - Activo: {child.gameObject.activeSelf}");
        }
        
        // Verificar collider
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            Debug.Log($"Collider: {collider.GetType().Name} - Enabled: {collider.enabled} - IsTrigger: {collider.isTrigger}");
        }
    }
}