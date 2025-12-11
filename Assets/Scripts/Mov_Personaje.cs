using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Mov_Personaje : MonoBehaviour
{
    public Transform puntoInicio;
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

    private Vector3 posicionAnterior;
    public float velocidadCalculada;

    public float timerRespawn = 1.2f; // Tiempo para respawn
    private bool respawning = false;
    public Image fadeInMuerte; // Referencia a la imagen de muerte

    void Start()
    {
          fadeInMuerte.gameObject.SetActive(false);
          transform.rotation = Quaternion.Euler(0f, 180, 0f);
    }
    void Update()
    {
        if(respawning)
        { // "Te ha atrapado el enemigo y la imagen parte de 0 alfa a 1 (fadein)
            // cuando vuelve al punto inical hace un fadeout de 1 a 0 (fadeout) "puedes añadirotro temporizador hasta que desaparezca la imagen antes de moverte. 
            fadeInMuerte.gameObject.SetActive(true);
            timerRespawn -= Time.deltaTime;
            fadeInMuerte.fillAmount = 0 + (timerRespawn / 1.2f);
            if (timerRespawn <= 0f)
            {
                
                transform.position = puntoInicio.position;
                transform.rotation = puntoInicio.rotation;
                fadeInMuerte.fillAmount = 0f;
                fadeInMuerte.gameObject.SetActive(false);
                respawning = false;
                timerRespawn = 1.2f; // Reiniciar el temporizador para la próxima vez
            }
        }
        else
        {
            MoverJugador();
            ControlarEstados();
            ControlarCamara();
            CalcularVelocidad();
        }
      
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
        if (estadoActual == EstadoJugador.Agachado)
        {
            velocidadActual = velocidadAgachado;

            //  playerCollider = playerCapsule.GetComponent<Collider>();
            // playerCollider.height = 1.34f;
        }


        transform.position += direccion.normalized * velocidadActual * Time.deltaTime;
    }
    void CalcularVelocidad()
    {
        velocidadCalculada = (transform.position - posicionAnterior).magnitude / Time.deltaTime;
        posicionAnterior = transform.position;
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
        playerCapsule.gameObject.transform.localScale = new Vector3(1f, alturaObjetivo / 2, 1f);
        playerCollider = playerCapsule.GetComponent<CapsuleCollider>();
        playerCollider.height = alturaObjetivo;
        CameraPj.localPosition = Vector3.Lerp(posActual, posObjetivo, Time.deltaTime * velocidadTransicion);
    }

    void ControlarCamara() //el personaje gira en la Y. 
    {
        var raton = Mouse.current;
        if (raton == null) return;
        
  
        if (raton.rightButton.isPressed) // esto de aqui al haerel primer click rota al personaje. 
        {
            Vector2 delta = raton.delta.ReadValue();

            float moverX = delta.y * sensibilidadMouse * Time.deltaTime;
            float moverY = delta.x * sensibilidadMouse * Time.deltaTime;

            rotacionX -= moverX;
            rotacionY += moverY;

            rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);

            CameraPj.localRotation = Quaternion.Euler(rotacionX, 0f, 0f); 
            transform.rotation = Quaternion.Euler(0f, rotacionY+180, 0f);
            
           
        }
    }
    public void ResetPosition()
    {
        respawning = true;
        /*if (puntoInicio != null)
        {
            transform.position = puntoInicio.position;
            transform.rotation = puntoInicio.rotation;
        }
        else
        {
            Debug.LogWarning("Punto de inicio no asignado en NauJugador.");
        }*/
    }
}
