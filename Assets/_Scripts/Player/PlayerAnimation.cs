using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;
    float velocity = 0.0f;
    public float acceleration = 0.1f;
    int VelocityHash;

    void Start()
    {
        animator = GetComponent<Animator>();

        VelocityHash = Animator.StringToHash("Velocity");
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        if (hor != 0 || ver != 0)
        {
            velocity = 1; 
        }

        // if (forwardPressed && velocity > 0.0f)
        // {
        //     velocity -= Time.deltaTime * acceleration;
        // }

        if(hor == 0 && ver == 0)
        {
            velocity = 0.0f;
        }

        animator.SetFloat("Velocity", velocity);

    }
}
