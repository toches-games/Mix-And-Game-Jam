using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class StraightEnemy : MonoBehaviour
{
    // Velocidad hacia adelante del coche enemigo
    [SerializeField, Range(1f, 5f)] private float speed = 3f;

    Rigidbody2D rig;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Siempre tendrá la misma velocidad hacia adelante
        rig.velocity = Vector2.up * speed;
    }

    private void Update()
    {
        // Hace que mantenga la velocidad hacia adelante asi sea chocado por algo
        if (rig.velocity.y > speed)
        {
            rig.velocity = new Vector2(rig.velocity.x, speed);
        }
    }
}
