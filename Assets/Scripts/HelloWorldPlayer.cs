using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        static public int scene = 1;
        public Rigidbody rb;
        public float velocidad, jumpForce = 5f;
        private Vector3 initialPosition;
        public TMP_Text puntuacion;
        private int estrellas = 0;
        private bool suelo;
        public MovingPlatform movingPlatform; // Reference to the MovingPlatform script
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public override void OnNetworkSpawn()
        {
            Move();
        }

        public void Move()
        {
            if (IsServer)
            {
                SubmitPositionRequestServerRpc();
            }
            else
            {
                SubmitPositionRequestOwnerRpc();
            }
        }

        [Rpc(SendTo.Server)]
        void SubmitPositionRequestServerRpc(RpcParams rpcParams = default)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }

        [Rpc(SendTo.Owner)]
        void SubmitPositionRequestOwnerRpc(RpcParams rpcParams = default)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
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

        void Update()
        {
            if (IsOwner)
            {
                var Horizontal = Input.GetAxis("Horizontal");
                var Vertical = Input.GetAxis("Vertical");
                var move = new Vector3(Horizontal, 0, Vertical) * 5 * Time.deltaTime;

                transform.position += move;

                if(transform.position.y < -10) // Check the y position of the ball
                {
                    Debug.Log("Resetting position to initial position");
                    transform.position = initialPosition; // Reset the ball's position to the initial position
                    rb.linearVelocity = Vector3.zero; // Reset the velocity to zero
                }

                rb.AddForce(move * velocidad * Time.deltaTime * 100);

                if (Input.GetButtonDown("Jump") && suelo)
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }

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
            if(IsServer)
            {
                Position.Value = transform.position;
            }
        }
    }
}