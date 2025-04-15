using UnityEngine;

public class EnemyMovementZ : MonoBehaviour
{
    public float speed = 2f; // Velocidad
    public float zLimit = 5f; // Límite para girarse
    public float hitStrength = 10f; // Fuerza con la que golpea la bola
    private bool movingForward = true; // Dirección (true = delante, false = detrás)

    void Update()
    {
        // El enemigo se mueve por el eje z
        if (movingForward)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }

        // Comprueba si ha llegado al límite y se da la vuelta
        if (transform.position.z >= zLimit)
        {
            movingForward = false;
        }
        else if (transform.position.z <= -zLimit)
        {
            movingForward = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Comprueba si el enemigo ha colisionado con la bola
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Aplica una fuerza a la bola basada en la fuerza del golpe
                Vector3 forceDirection = (collision.transform.position - transform.position).normalized;
                rb.AddForce(forceDirection * hitStrength, ForceMode.Impulse);
            }
        }
    }
}