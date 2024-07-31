using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Knockback : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;
    public ScreenShake screenShake;

    private CharacterController characterController;
    private Rigidbody rb;
    private XROrigin xrOrigin;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    private Vector3 knockbackDirection;

    void Start()
    {
        xrOrigin = GetComponent<XROrigin>();
        if (xrOrigin)
        {
            characterController = xrOrigin.GetComponentInChildren<CharacterController>();
            rb = xrOrigin.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
            else
            {
                Debug.LogError("Rigidbody not found!");
            }
        }
        else
        {
            Debug.LogError("XR Origin not found!");
        }

        if (!screenShake)
        {
            screenShake = FindObjectOfType<ScreenShake>();
            if (!screenShake)
            {
                Debug.LogError("ScreenShake component not found!");
            }
        }
    }

    void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
                Debug.Log("Knockback ended.");
            }
            else
            {
                ApplyKnockback();
            }
        }
    }

    public void KnockbackDir(Vector3 direction)
    {
        knockbackDirection = direction.normalized;
        knockbackTimer = knockbackDuration;
        isKnockedBack = true;
        Debug.Log($"Knockback initiated. Direction: {knockbackDirection}, Duration: {knockbackDuration}");
    }

    private void ApplyKnockback()
    {
        if (characterController)
        {
            Vector3 movement = knockbackDirection * knockbackForce * Time.deltaTime;
            characterController.Move(movement);
            Debug.Log($"Applying knockback. Movement: {movement}");
        }
        else
        {
            Debug.LogError("CharacterController not found!");
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            Vector3 knockbackDirection = (transform.position - other.transform.position).normalized;
            KnockbackDir(knockbackDirection);

            if (screenShake)
            {
                screenShake.StartShake();
                Debug.Log("Screen shake started.");
            }
            else
            {
                Debug.LogError("ScreenShake not assigned!");
            }
        }
    }
}
