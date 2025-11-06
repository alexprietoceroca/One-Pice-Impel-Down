using UnityEngine;
using UnityEngine.InputSystem;

public class NauJugador : MonoBehaviour
{
    public float velocidad = 5f;
    public float alturaNormal = 2f;
    public float alturaAgachado = 1f;
    private bool agachado = false;

    void Update()
    {
        MoverJugador();
        ControlarAgacharse();
    }

    void MoverJugador()
    {
        var teclado = Keyboard.current;

        Vector3 direccion = Vector3.zero;

        if (teclado.wKey.isPressed) direccion += transform.forward;
        if (teclado.sKey.isPressed) direccion -= transform.forward;
        if (teclado.dKey.isPressed) direccion += transform.right;
        if (teclado.aKey.isPressed) direccion -= transform.right;

        transform.position += direccion.normalized * velocidad * Time.deltaTime;
    }

    void ControlarAgacharse()
    {
        var teclado = Keyboard.current;

        if (teclado.leftShiftKey.isPressed)
        {
            if (!agachado)
            {
                transform.localScale = new Vector3(1, alturaAgachado / alturaNormal, 1);
                agachado = true;
            }
        }
        else
        {
            if (agachado)
            {
                transform.localScale = new Vector3(1, 1, 1);
                agachado = false;
            }
        }
    }
}
