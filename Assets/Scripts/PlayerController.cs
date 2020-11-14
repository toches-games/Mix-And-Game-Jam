using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    #region inspector variables

    [SerializeField, Range(1f, 5f)] private float initialSpeed = 3f;
    [SerializeField, Range(5f, 20f)] private float maxSpeed = 10f;
    [SerializeField, Range(0.1f, 1f)] private float acceleration = 0.5f;

    #endregion

    #region private variables

    private Rigidbody2D rig;
    private float currentSpeed;

    // for smoothdamp
    private float currentVelocity;
    private float xSmoothDirection;

    private bool hit;
    private float hitTime;

    #endregion

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Physics2D.gravity = Vector2.zero;
        currentSpeed = initialSpeed;
    }

    private void Update()
    {
        if (hit)
        {
            HitChecker();
        }
    }

    private void FixedUpdate()
    {
        if (!hit)
        {
            Movement();
            Rotation();
        }
    }

    private void HitChecker()
    {
        rig.angularVelocity = 0f;
        hitTime += Time.deltaTime;

        if (hitTime >= 0.3f)
        {
            hitTime = 0f;
            hit = false;
        }
    }

    private void Rotation()
    {
        rig.rotation = currentSpeed * 2f * -MovementDirection().x;
    }

    private void Movement()
    {
        if(Input.GetAxisRaw("Vertical") < 0)
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }

        else
        {
            currentSpeed += acceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, initialSpeed, maxSpeed);
        rig.velocity = MovementDirection() * currentSpeed;
    }

    private Vector2 MovementDirection()
    {
        xSmoothDirection = Mathf.SmoothDamp(xSmoothDirection, Input.GetAxisRaw("Horizontal"), ref currentVelocity, 0.2f);
        return new Vector2(xSmoothDirection, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            rig.velocity = direction * -currentSpeed;
            hit = true;
        }
    }
}
