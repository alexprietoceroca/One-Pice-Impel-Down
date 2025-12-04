using UnityEngine;
using UnityEngine.InputSystem;

public class NauJugador : MonoBehaviour
{
    public float velocidad = 7f;
    public float velocidadAgachado = 4f;
    public float velocidadSuelo = 2f; 

    public float alturaNormal = 3f;
    public float alturaAgachado = 2f;
     

    public float sensibilidadMouse = 500f;
    public float velocidadTransicion = 5f; 

    private enum EstadoJugador { Normal, Agachado }
    private EstadoJugador estadoActual = EstadoJugador.Normal;

    private float rotacionX = 0f;
    private float rotacionY = 0f;

    public Transform CameraPj;

    public GameObject playerCapsule;
    private CapsuleCollider playerCollider;

    void Update()
    {
        MoverJugador();
        ControlarEstados();
        ControlarCamara();
    }

    void MoverJugador()
    {
        var teclado = Keyboard.current;
        Vector3 direccion = Vector3.zero;


        if (teclado.wKey.isPressed) direccion += transform.forward;
        if (teclado.sKey.isPressed) direccion -= transform.forward;
        if (teclado.dKey.isPressed) direccion += transform.right;
        if (teclado.aKey.isPressed) direccion -= transform.right;

        float velocidadActual = velocidad;
        if (estadoActual == EstadoJugador.Agachado){ velocidadActual = velocidadAgachado;
            
          //  playerCollider = playerCapsule.GetComponent<Collider>();
           // playerCollider.height = 1.34f;
        }
       

        transform.position += direccion.normalized * velocidadActual * Time.deltaTime;
    }

    void ControlarEstados()
    {
        var teclado = Keyboard.current;

        if (teclado.leftShiftKey.isPressed)
        {
           
            estadoActual = EstadoJugador.Agachado;
        }
      
        else
        {
            estadoActual = EstadoJugador.Normal;
           // playerCapsule.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);   
        }

        float alturaObjetivo = alturaNormal;
        if (estadoActual == EstadoJugador.Agachado) alturaObjetivo = alturaAgachado;
        //if (estadoActual == EstadoJugador.Suelo) alturaObjetivo = alturaSuelo;

        Vector3 posActual = CameraPj.localPosition;
        Vector3 posObjetivo = new Vector3(posActual.x, alturaObjetivo, posActual.z);
        playerCapsule.gameObject.transform.localScale = new Vector3(1f,  alturaObjetivo/2, 1f);
        playerCollider = playerCapsule.GetComponent<CapsuleCollider>();
        playerCollider.height = alturaObjetivo;
        CameraPj.localPosition = Vector3.Lerp(posActual, posObjetivo, Time.deltaTime * velocidadTransicion);
    }

    void ControlarCamara()
    {
        var raton = Mouse.current;
        if (raton == null) return;

        if (raton.rightButton.isPressed)
        {
            Vector2 delta = raton.delta.ReadValue();

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
