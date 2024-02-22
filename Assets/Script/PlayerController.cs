using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speedMovement = 3f;

    [SerializeField]
    private float mouseSensitivity = 3f;

    private PlayerMotor motor;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        // Calculate the velocity of the player movement
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        // Normalize the velocity and merge it in 1 Vector3
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speedMovement;

        motor.Move(velocity);

        // Calculate the rotation of the player to a Vector3
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivity;

        motor.Rotate(rotation);

        // Calculate the rotation of the camera to a Vector3
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 cameraRotation = new Vector3(xRot, 0, 0) * mouseSensitivity;

        motor.RotateCamera(cameraRotation);

    }
}
