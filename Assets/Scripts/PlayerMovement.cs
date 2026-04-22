using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float sprintSpeed = 10f;
    private float currentSpeed;
    public float groundDrag = 5f;

    [Header("Jumping & Air Control")]
    public float jumpForce = 12f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.6f; // Semakin tinggi, semakin lincah di udara
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Settings")]
    public float rotationSpeed = 10f;
    Rigidbody rb;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    void Update()
    {
        // Ground check menggunakan Raycast
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        RotatePlayer();

        // Handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Logic Sprint
        if (grounded && Input.GetKey(sprintKey))
            currentSpeed = sprintSpeed;
        else
            currentSpeed = moveSpeed;

        // Logic Jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // Hitung moveDirection berdasarkan arah input saat ini (selalu terupdate)
        moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Jika di tanah
        if (grounded)
        {
            rb.AddForce(moveDirection * currentSpeed * 10f, ForceMode.Force);
        }
        // Jika di udara (Air Control)
        else
        {
            // Kita tetap kasih force di udara agar player bisa merubah arah momentum
            rb.AddForce(moveDirection * currentSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Membatasi kecepatan velocity agar tidak "over-speed" karena AddForce
        if (flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void RotatePlayer()
    {
        // Menggunakan input arah agar karakter selalu menghadap ke mana kita tekan tombol
        Vector3 inputDir = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (inputDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Jump()
    {
        // 1. Benar-benar nol-kan velocity Y biar gak ada sisa gravitasi/momentum jatuh
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // 2. Gunakan gaya Impulse yang lebih besar
        // Kalau masih kurang tinggi, lu bisa kalikan jumpForce di sini atau naikin di Inspector
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}