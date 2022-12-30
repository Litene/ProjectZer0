using UnityEngine;

public class JoakimMoveTest : MonoBehaviour {
    public float speed, maxSpeed;
    public Rigidbody rb;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    public void FixedUpdate() {
        float X = UnityEngine.Input.GetAxis("Horizontal");
        float Z = UnityEngine.Input.GetAxis("Vertical");
        var targetVelocity = new Vector3(X, 0, Z);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;
        var velocity = rb.velocity;
        var velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxSpeed, maxSpeed);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxSpeed, maxSpeed);
        velocityChange.y = 0;
        GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);
    }
}

