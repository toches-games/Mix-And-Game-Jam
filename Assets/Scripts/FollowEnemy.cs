using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class FollowEnemy : MonoBehaviour
{
    [SerializeField, Range(1f, 5f)] private float speed = 3f;

    Rigidbody2D rig;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rig.velocity = Vector2.up * speed;
    }

    private void FixedUpdate()
    {
        if (rig.velocity.y > speed)
        {
            rig.velocity = new Vector2(rig.velocity.x, speed);
        }
    }
}
