using UnityEngine;

public class JoakimCamMoveTest : MonoBehaviour {
    public Transform target;
    Vector3 velocity = Vector3.zero;
    [SerializeField] private float mouseSensitivity = 0.5f;
    void Update() {
        CameraLook();
    }
    private void CameraLook() {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        var rotationLR = target.transform.localEulerAngles;
        rotationLR.y += mouseX * mouseSensitivity;
        target.transform.rotation = Quaternion.AngleAxis(rotationLR.y, Vector3.up);
        var cameraRot = Camera.main.gameObject.transform.localEulerAngles;
        cameraRot.x -= mouseY * mouseSensitivity;
        Camera.main.gameObject.transform.localRotation = Quaternion.AngleAxis(cameraRot.x, Vector3.right);
    }
}
