using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    float velocity = 0.0f;
    public float acceleration = 2f;
    public float deceleration = 6f;  // Augmenté pour stopper Run rapidement
    public float maxWalkSpeed = 0.5f;
    public float maxRunSpeed = 1.0f;
    public float stopThreshold = 0.3f; // Seuil pour arrêter Run directement
    public float rotationSpeed = 150f;

    int VelocityHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        VelocityHash = Animator.StringToHash("Velocity");
    }

    void Update()
    {
        bool movePressed = Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("d");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");

        // Accélération fluide
        if (movePressed)
        {
            velocity = Mathf.Lerp(velocity, maxRunSpeed, Time.deltaTime * acceleration);
        }
        else
        {
            if (velocity > maxWalkSpeed) // Si on a couru avant
            {
                velocity = Mathf.Lerp(velocity, stopThreshold, Time.deltaTime * deceleration);
                if (velocity <= stopThreshold) velocity = 0; // Arrêt direct de Run
            }
            else
            {
                velocity = Mathf.Lerp(velocity, 0, Time.deltaTime * deceleration);
            }
        }

        // Rotation fluide
        float rotationDirection = 0f;
        if (leftPressed) rotationDirection = -1f;
        if (rightPressed) rotationDirection = 1f;

        transform.Rotate(Vector3.up, rotationDirection * rotationSpeed * Time.deltaTime);

        // Appliquer la vélocité à l’Animator
        animator.SetFloat(VelocityHash, velocity);
        Debug.Log("Velocity: " + velocity);
    }
}