using UnityEngine;
using Utilities;

namespace Oscillators
{
    /// <summary>
    /// A dampened torsional oscillator using the objects transform local rotation and the rigibody.
    /// Unfortunately, the option of not requiring a rigidbody was proving more difficult than expected,
    /// due to difficulty working with quaternions whilst calculating angular velocity, torque and angular
    /// displacement to apply.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class TorsionalOscillator : MonoBehaviour
    {
        [Tooltip("The local rotation about which oscillations are centered.")]
        [SerializeField] private Vector3 _localEquilibriumRotation = Vector3.zero;
        [Tooltip("The axes over which the oscillator applies torque. Within range [0, 1].")] 
        [SerializeField] private Vector3 _torqueScale = Vector3.one;
        [Tooltip("The greater the stiffness constant, the lesser the amplitude of oscillations.")] 
        [SerializeField]private float _stiffness = 10f;
        [Tooltip("The greater the damper constant, the faster that oscillations will disappear.")] 
        [SerializeField] private float _damper = 1f;
        [Tooltip("The center about which rotations should occur.")] 
        [SerializeField] private Vector3 _localPivotPosition = Vector3.zero;

        private Rigidbody _rb;
        private float _angularDisplacementMagnitude;
        private Vector3 _rotAxis;

        /// <summary>
        /// Get the rigidbody component.
        /// </summary>
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.centerOfMass = _localPivotPosition;
        }

        /// <summary>
        /// Set the center of rotation.
        /// Update the rotation of the oscillator, by calculating and applying the restorative torque.
        /// </summary>
        private void FixedUpdate()
        {
            Vector3 restoringTorque = CalculateRestoringTorque();
            ApplyTorque(restoringTorque);
            
            _rb.centerOfMass = _localPivotPosition;
        }

        /// <summary>
        /// Returns the damped restorative torque of the oscillator.
        /// The magnitude of the restorative torque is 0 at the equilibrium rotation and maximum at the amplitude of the oscillation.
        /// </summary>
        /// <returns>Damped restorative torque of the oscillator.</returns>
        private Vector3 CalculateRestoringTorque()
        {
            var parent = transform.parent;
            var equilibriumRotation = Quaternion.Euler(_localEquilibriumRotation);
            if (parent != null)
            {
                equilibriumRotation = parent.rotation * equilibriumRotation ;
            }
            Quaternion deltaRotation = 
                MathsUtilities.ShortestRotation(transform.rotation, equilibriumRotation);
            deltaRotation.ToAngleAxis(out _angularDisplacementMagnitude, out _rotAxis);
            Vector3 angularDisplacement = _angularDisplacementMagnitude * Mathf.Deg2Rad * _rotAxis.normalized;
            Vector3 torque = AngularHookesLaw(angularDisplacement, _rb.angularVelocity);
            return (torque);
        }

        /// <summary>
        /// Returns the damped Hooke's torque for a given angularDisplacement and angularVelocity.
        /// </summary>
        /// <param name="angularDisplacement">The angular displacement of the oscillator from the equilibrium rotation.</param>
        /// <param name="angularVelocity">The local angular velocity of the oscillator.</param>
        /// <returns>Damped Hooke's torque</returns>
        private Vector3 AngularHookesLaw(Vector3 angularDisplacement, Vector3 angularVelocity)
        {
            Vector3 torque =
                (_stiffness * Mathf.Deg2Rad * angularDisplacement) + (_damper * Mathf.Deg2Rad * angularVelocity); // Damped angular Hooke's law
            torque = -torque; // Take the negative of the torque, since the torque is restorative (attractive)
            return (torque);
        }

        /// <summary>
        /// Adds a torque to the oscillator using the rigidbody.
        /// </summary>
        /// <param name="torque">The torque to be applied.</param>
        private void ApplyTorque(Vector3 torque)
        {
            _rb.AddTorque(Vector3.Scale(torque, _torqueScale));
        }

        /// <summary>
        /// Draws the pivot of rotation (wire sphere), the oscillator bob (sphere) and the equilibirum (wire sphere).
        /// </summary>
        void OnDrawGizmos()
        {
            Vector3 bob = transform.position;
            Vector3 axis = _rotAxis.normalized;
            float angle = _angularDisplacementMagnitude;

            // Draw (wire) pivot position
            Gizmos.color = Color.white;
            Vector3 pivotPosition =
                transform.TransformPoint(Vector3.Scale(_localPivotPosition, MathsUtilities.Invert(transform.localScale)));
            Gizmos.DrawWireSphere(pivotPosition, 0.3f);
            // Draw a cross at the pivot position;
            Vector3 cross1 = new Vector3(1, 0, 1) * 0.3f;
            Vector3 cross2 = new Vector3(1, 0, -1) * 0.3f;
            Gizmos.DrawLine(pivotPosition - cross1, pivotPosition + cross1);
            Gizmos.DrawLine(pivotPosition - cross2, pivotPosition + cross2);

            // Color goes from green (0,1,0,0) to yellow (1,1,0,0) to red (1,0,0,0).
            Color color = Color.green;
            float upperAmplitude = 90f; // Approximately the upper limit of the angle amplitude within regular use
            color.r = 2f * Mathf.Clamp(angle / upperAmplitude, 0f, 0.5f);
            color.g = 2f * (1f - Mathf.Clamp(angle / upperAmplitude, 0.5f, 1f));
            Gizmos.color = color;

            // Draw (arc) angle to equilibrium
            Vector3 equilibrium = GizmoUtilities.DrawArc(pivotPosition, bob, axis, 0f, -angle / 360f, 32, color);


            // Draw (solid) bob position
            Gizmos.DrawSphere(bob, 0.3f);

            // Draw (wire) equilibrium position
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(equilibrium, 0.3f);
        }
    }
}
