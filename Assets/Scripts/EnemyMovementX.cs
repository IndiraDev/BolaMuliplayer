using UnityEngine;

public class EnemyMovementX : MonoBehaviour
{
    public float speed = 2f; // Velocidad
    public float xLimit = 5f; // Límite para girarse
    private bool movingForward = true; // Dirección (true = delante, false = detrás)

    void Update()
    {
        // El enemigo se mueve por el eje x
        if (movingForward)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // Comprueba si ha llegado al límite y se da la vuelta
        if (transform.position.x >= xLimit)
        {
            movingForward = false;
        }
        else if (transform.position.x <= -xLimit)
        {
            movingForward = true;
        }
    }
}