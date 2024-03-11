﻿using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent (typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speedMovement = 3f;

    [SerializeField]
    private float mouseSensitivity = 3f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    [SerializeField] private float trusterForce = 1000;

    [Header("Joint Options")]
    [SerializeField] private float jointSpring = 20f;
    [SerializeField] private float jointMaxForce = 50f;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();

        SetJointSettings(jointSpring);
    }

    private void Update()
    {
        if (PauseMenu.isOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(Vector3.zero);
            //motor.ApplyThruster(Vector3.zero);

            return;
        }

        if(Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Calculate the velocity of the player movement
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        // Normalize the velocity and merge it in 1 Vector3
        Vector3 velocity = (moveHorizontal + moveVertical) * speedMovement;

        // Play the animation of the truster
        animator.SetFloat("ForwardVelocity", zMov);

        motor.Move(velocity);

        // Calculate the rotation of the player to a Vector3
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivity;

        motor.Rotate(rotation);

        // Calculate the rotation of the camera to a Vector3
        float xRot = Input.GetAxisRaw("Mouse Y");

        float cameraRotationX = xRot * mouseSensitivity;

        motor.RotateCamera(cameraRotationX);


        // Calculate jetpack force
        Vector3 trusterVelocity = Vector3.zero;

        if (Input.GetButton("Jump"))
        {
            trusterVelocity = Vector3.up * trusterForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(jointSpring);
        }

        // Apply jetpack force
        motor.ApplyTruster(trusterVelocity);
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
    }
}
