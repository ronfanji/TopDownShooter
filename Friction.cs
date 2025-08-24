using UnityEngine;

public class Friction : MonoBehaviour
{
    public float baseFriction = 2f;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>(); 
    }

    void FixedUpdate() {
        rb.linearVelocity = rb.linearVelocity * (1 - baseFriction * Time.fixedDeltaTime);
    }
}
