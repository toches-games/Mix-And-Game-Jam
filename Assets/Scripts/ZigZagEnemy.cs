using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class ZigZagEnemy : MonoBehaviour
{
    [SerializeField, Range(1f, 5f)] private float speed = 3f;

    private Rigidbody2D rig;
    private int[] xRandomDirection = { -1, 1 };
    private int[] xRandomAmplitude = { 3, 4, 5 };
    private int xDirection;
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

    private void FixedUpdate()
    {
        if (rig.velocity.y > speed)
        {
            rig.velocity = new Vector2(rig.velocity.x, speed);
        }

        float xVelocity = Mathf.Sin(Time.time * Mathf.Deg2Rad * 100f);
        rig.velocity = new Vector2(xVelocity * xAmplitude * xDirection, rig.velocity.y);
        rig.rotation = 6f * xAmplitude * xVelocity * -xDirection;
    }
}
