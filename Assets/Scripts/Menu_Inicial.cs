using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Inicial : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Mapa_Principal");
    }

    public void Salir()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }
}
