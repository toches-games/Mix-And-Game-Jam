using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class ZigZagEnemy : MonoBehaviour
{
    [SerializeField, Range(1f, 5f)] private float speed = 3f;

    private Rigidbody2D rig;
    // Direcciónes hacia donde se podrá mover apartir del centro
    private int[] xRandomDirection = { -1, 1 };
    // Que tanto se podrá alejar del centro
    private int[] xRandomAmplitude = { 3, 4, 5 };
    // Dirección horizontal a donde se va a mover
    private int xDirection;
    // Que tanto se alejará del centro
    private int xAmplitude;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rig.velocity = Vector2.up * speed;
        xDirection = xRandomDirection[Random.Range(0, xRandomDirection.Length)];
        xAmplitude = xRandomAmplitude[Random.Range(0, xRandomAmplitude.Length)];
    }

    private void Update()
    {
        // Hace que mantenga la velocidad hacia adelante asi sea chocado por algo
        if (rig.velocity.y > speed)
        {
            rig.velocity = new Vector2(rig.velocity.x, speed);
        }

        // Velocidad horizontal
        float xVelocity = Mathf.Sin(Time.time * Mathf.Deg2Rad * 100f);
        rig.velocity = new Vector2(xVelocity * xAmplitude * xDirection, rig.velocity.y);

        // Rota el sprite hacia donde se va a mover
        rig.rotation = 6f * xAmplitude * xVelocity * -xDirection;
    }
}
