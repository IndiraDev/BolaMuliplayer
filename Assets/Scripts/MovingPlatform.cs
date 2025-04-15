using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 posfin; // The position to move to
    public float velocidad = 1f; // The speed of the movement
    private bool shouldMove = false; // Flag to control the movement

    void Update()
    {
        if (shouldMove)
        {
            // Move the platform towards the target position
            transform.position = Vector3.MoveTowards(transform.position, posfin, velocidad * Time.deltaTime);
        }
    }

    public void StartMoving()
    {
        shouldMove = true;
    }
}
