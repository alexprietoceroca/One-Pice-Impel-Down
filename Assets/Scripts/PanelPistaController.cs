using UnityEngine;
using UnityEngine.UI;

public class PanelPistaController : MonoBehaviour
{
    [Header("COMPONENTES DEL PANEL")]
    public Text textoPista;    // Arrastra el Text aquí
    public Button botonCerrar; // Arrastra el Button aquí
    
    void Start()
    {
        Debug.Log($"📋 PanelPistaController iniciado: {gameObject.name}");
        
        // Configurar botón de cerrar
        if (botonCerrar != null)
        {
            botonCerrar.onClick.AddListener(CerrarPanel);
            Debug.Log($"✅ Botón cerrar configurado");
        }
        
        // Asegurar que el panel está OCULTO al inicio
        gameObject.SetActive(false);
    }
    
    public void MostrarPista(string mensaje)
    {
        Debug.Log($"📝 Mostrando pista: {mensaje}");
        
        if (textoPista != null)
        {
            textoPista.text = mensaje;
        }
        
        gameObject.SetActive(true);
    }
    
    public void CerrarPanel()
    {
        Debug.Log($"❌ Cerrando panel de pista");
        gameObject.SetActive(false);
    }
    
    void Update()
    {
        // También cerrar con tecla Escape
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CerrarPanel();
        }
    }
}