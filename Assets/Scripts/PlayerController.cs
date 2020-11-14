using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    #region inspector variables

    [SerializeField, Range(1f, 5f)] private float initialSpeed = 3f;
    [SerializeField, Range(5f, 20f)] private float maxSpeed = 10f;
    [SerializeField, Range(5f, 10f)] private float acceleration = 5f;

    #endregion

    #region private variables

    private Rigidbody2D rig;
    private float currentSpeed;

    // for smoothdamp
    private float currentVelocity;
    private float xSmoothDirection;

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

    public void Move()
    {
        Movement();
        Rotation();
    }

    private void Rotation()
    {
        rig.MoveRotation(currentSpeed * 2f * -MovementDirection().x);
    }

    private void Movement()
    {
        currentSpeed += acceleration * Time.deltaTime * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        rig.velocity = MovementDirection() * currentSpeed;
    }

    private Vector2 MovementDirection()
    {
        xSmoothDirection = Mathf.SmoothDamp(xSmoothDirection, Input.GetAxisRaw("Horizontal"), ref currentVelocity, 0.2f);
        return new Vector2(xSmoothDirection, 1f);
    }
}
