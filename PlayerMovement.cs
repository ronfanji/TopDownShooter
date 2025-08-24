using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    private Camera mainCam;
    private Vector3 mousePos;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded;
    private float horizontal;
    private float vertical;
    [SerializeField] Rigidbody2D rb;


    private bool canDash = true;
    private bool isDashing = false;
    public float dashingPower = 15f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (canDash && Input.GetKey(KeyCode.LeftShift)){
            StartCoroutine(Dash());
        }

    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.linearVelocity = new Vector2(horizontal, vertical).normalized * moveSpeed;
    }

    IEnumerator Dash()
    {   
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos-transform.position;
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector2(0, 0);
        rb.linearVelocity = new Vector2(hori, vert).normalized * dashingPower;

        isDashing = true;
        canDash = false;

        yield return new WaitForSeconds(dashingTime);
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }
}
