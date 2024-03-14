using UnityEngine;

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
    [SerializeField] private float trusterFuelBurnedSpeed = 1f;
    [SerializeField] private float trusterFuelRegenSpeed = 0.3f;
    private float trusterFuelAmount = 1f;

    public float GetThrusterFuelAmount()
    {
        return trusterFuelAmount;
    }

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
            motor.RotateCamera(0f);
            //motor.ApplyThruster(Vector3.zero);

            return;
        }

        if(Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Offset for normal jetpack
        RaycastHit _hit;
        if(Physics.Raycast(transform.position, Vector3.down, out _hit, 100f))
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 2f, 0f);
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

        if (Input.GetButton("Jump") && trusterFuelAmount > 0)
        {
            trusterFuelAmount -= trusterFuelBurnedSpeed * Time.deltaTime;

            if(trusterFuelAmount >= 0.01f)
            {
                trusterVelocity = Vector3.up * trusterForce;
                SetJointSettings(0f);
            }
        }
        else
        {
            trusterFuelAmount += trusterFuelRegenSpeed * Time.deltaTime;
            SetJointSettings(jointSpring);
        }

        trusterFuelAmount = Mathf.Clamp(trusterFuelAmount, 0f, 1f);

        // Apply jetpack force
        motor.ApplyTruster(trusterVelocity);
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
    }
}
