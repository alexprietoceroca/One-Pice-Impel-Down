using UnityEngine;
using UnityEngine.InputSystem;

public class NauJugador : MonoBehaviour
{
    public float velocidad = 7f;
    public float velocidadAgachado = 4f;
    public float alturaNormal = 3f;
    public float alturaAgachado = 2f;
    public float sensibilidadMouse = 500f;

    private bool agachado = false;
    private float rotacionX = 0f;
    private float rotacionY = 0f;

    public Transform CameraPj;

    void Update()
    {
        MoverJugador();
        ControlarAgacharse();
        ControlarCamara();
    }

    void MoverJugador()
    {
        var teclado = Keyboard.current;
        Vector3 direccion = Vector3.zero;

        // WASD
        if (teclado.wKey.isPressed) direccion += transform.forward;
        if (teclado.sKey.isPressed) direccion -= transform.forward;
        if (teclado.dKey.isPressed) direccion += transform.right;
        if (teclado.aKey.isPressed) direccion -= transform.right;

        float velocidadActual = agachado ? velocidadAgachado : velocidad;
        transform.position += direccion.normalized * velocidadActual * Time.deltaTime;
    }

    void ControlarAgacharse()
    {
        var teclado = Keyboard.current;

        if (teclado.leftShiftKey.isPressed && !agachado)
        {
            transform.localScale = new Vector3(1, alturaAgachado / alturaNormal, 1);
            agachado = true;
        }
        else if (!teclado.leftShiftKey.isPressed && agachado)
        {
            transform.localScale = new Vector3(1, 1, 1);
            agachado = false;
        }
    }

void ControlarCamara()
{
    var raton = Mouse.current;

    if (raton == null) return;

    if (raton.rightButton.isPressed)
    {
        Vector2 delta = raton.delta.ReadValue();

        // Multiplicamos por deltaTime para suavizar la velocidad
        float moverX = delta.y * sensibilidadMouse * Time.deltaTime;
        float moverY = delta.x * sensibilidadMouse * Time.deltaTime;

        rotacionX -= moverX;
        rotacionY += moverY;

        rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);

        CameraPj.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, rotacionY, 0f);
    }
}


}
