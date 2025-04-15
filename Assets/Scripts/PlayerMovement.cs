using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    static public int scene = 1;
    public float velocidad, jumpForce = 5f;
    public Rigidbody rb;
    private Vector3 offset, initialPosition;
    public Camera camara;
    public TMP_Text puntuacion;
    private int estrellas = 0;
    private bool suelo;
    public MovingPlatform movingPlatform; // Reference to the MovingPlatform script

    void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        offset = camara.transform.position - transform.position;
        Debug.Log("Initial Position: " + initialPosition);

        // Reset the number of estrellas to 0 at the beginning of the scene
        ResetEstrellas();
    }

    void ResetEstrellas()
    {
        estrellas = 0;
        UpdatePuntuacionText();
    }

    void UpdatePuntuacionText()
    {
        puntuacion.text = "Estrellas: " + estrellas;
        Debug.Log("Puntuacion updated: " + puntuacion.text);
    }

    void OnTriggerEnter(Collider other)
    {
        // Si el jugador toca una estrella, la estrella desaparece y se suma una estrella al contador
        if (other.gameObject.CompareTag("EstrellaPremio"))
        {
            other.gameObject.SetActive(false);
            Debug.Log("Estrella recogida\nEstrellas: " + estrellas + "\n");
            estrellas++;
            UpdatePuntuacionText();
            if(estrellas == 6)
            {
                if (movingPlatform != null)
                {
                    movingPlatform.StartMoving();
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            suelo = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if the player is no longer on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            suelo = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movimiento = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if(transform.position.y < -10) // Check the y position of the ball
        {
            Debug.Log("Resetting position to initial position");
            transform.position = initialPosition; // Reset the ball's position to the initial position
            rb.linearVelocity = Vector3.zero; // Reset the velocity to zero
        }

        rb.AddForce(movimiento * velocidad * Time.deltaTime * 100);

        // Check for jump input and apply jump force if grounded
        if (Input.GetButtonDown("Jump") && suelo)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        camara.transform.position = transform.position + offset;

        // Load the next scene if the player's y position is -2.5
        if (estrellas == 6 && transform.position.y <= -2)
        {
            if (scene == 1)
            {
                scene = 2;
                SceneManager.LoadScene("Nivel2");
            }
            else if (scene == 2)
            {
                scene = 3;
                SceneManager.LoadScene("Nivel3");
            }
        }
    }
}