using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class FollowEnemy : MonoBehaviour
{
    // Velocidad hacia adelante del coche enemigo
    [SerializeField, Range(1f, 5f)] private float speed = 3f;

    private Rigidbody2D rig;
    // Tamaño del sprite de la chola en pantalla, para que algunos coches se vean rotados a la derecha
    // y otros a la izquierda
    private float[] xRandomSize = { -0.4f, 0.4f };
    // Hacia donde va a estar mirando el sprite
    private float xSize;
    // Velocidad horizontal
    private float xSpeed;

    //for smoothdamp
    private float currentVelocity;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rig.velocity = Vector2.up * speed;
        xSize = xRandomSize[Random.Range(0, xRandomSize.Length)];
        transform.localScale = new Vector2(xSize, 0.4f);
    }

    private void Update()
    {
        // Hace que mantenga la velocidad hacia adelante asi sea chocado por algo
        if (rig.velocity.y > speed)
        {
            rig.velocity = new Vector2(rig.velocity.x, speed);
        }
        
        // Distancia que tiene el coche enemigo hacia el jugador
        Vector2 playerDistance = Player.Instance.transform.position - transform.position;

        // Si el coche está cerca del jugador deja de perseguirlo
        if(playerDistance.y < -1f)
        {
            rig.velocity = new Vector2(speed * 2f * playerDistance.normalized.x, rig.velocity.y);
            xSpeed = rig.velocity.x;
        }

        else
        {
            xSpeed = Mathf.SmoothDamp(xSpeed, 0f, ref currentVelocity, 0.2f);
            rig.velocity = new Vector2(xSpeed, rig.velocity.y);
        }
        
        // Gira el sprite hacia donde se va a mover
        rig.rotation = 10f / playerDistance.x * xSpeed * -playerDistance.normalized.x;
    }
}
