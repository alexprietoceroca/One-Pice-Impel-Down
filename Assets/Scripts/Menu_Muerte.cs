using UnityEngine;

using UnityEngine.SceneManagement;
public class Menu_Muerte : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Reintentar()
    {
        SceneManager.LoadScene("Escena1");
    }

    // Update is called once per frame
    public void Salir()
    {
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
