using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    [SerializeField] private InputActionProperty jumpButton;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private CharacterController cc;
    [SerializeField] private LayerMask ground;

    private float gravity = Physics.gravity.y;
    private Vector3 movement;
    private void Update()
    {
        bool isGrounded = IsGrounded();
        if(jumpButton.action.WasPressedThisFrame() && isGrounded)
        {
            Jumpping();
        }
        movement.y += gravity * Time.deltaTime;
        cc.Move(movement * Time.deltaTime);
    }
    private void Jumpping()
    {
        movement.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position, 0.2f, ground);
    }
}
