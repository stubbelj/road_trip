using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] GameObject[] wheels;
    [SerializeField] float turnSpeed; //degrees per second of rotation
    [SerializeField] float wheelStraightenMod;
    [SerializeField] float maxTurnAngle;
    [SerializeField] float carBodyTurnSpeed; //degrees per sec
    [SerializeField] float acceleration;
    [SerializeField] Transform wheelForward;

    public void LateUpdate() {
        PhysicsUtils.ClampToMaxObjectSpeed(GetComponent<Rigidbody>());
    }

    public void Drive(Vector2 inputs) {
        float currTurnVal = inputs.x * turnSpeed;
        wheelForward.Rotate(0, currTurnVal * Time.deltaTime, 0);
        float wheelForwardAngle = (wheelForward.localEulerAngles.y > 180) ? 360 - wheelForward.localEulerAngles.y : wheelForward.localEulerAngles.y;
        if (wheelForwardAngle > maxTurnAngle) {
            if (wheelForward.localEulerAngles.y < 180) {
                wheelForward.transform.localEulerAngles = new Vector3(0, maxTurnAngle, 0);
            } else if (wheelForward.localEulerAngles.y > 270) {
                wheelForward.transform.localEulerAngles = new Vector3(0, -maxTurnAngle, 0);
            }
        }

        StraightenWheels(currTurnVal);
        StraightCarBody();

        foreach (GameObject wheel in wheels) {
            wheel.transform.localRotation = wheelForward.transform.localRotation;
            wheel.transform.Rotate(0, 90, 90);
        }

        Vector3 forwardForceVec = wheelForward.forward * inputs.y * acceleration;
        forwardForceVec.y = 0;
        GetComponent<Rigidbody>().AddForce(forwardForceVec, ForceMode.Impulse);
    }

    void StraightenWheels(float currTurnVal) {
        // wheels straighten out over time
        if (currTurnVal == 0) {
            //to prevent jittering, snap to zero when near zero
            if (wheelForward.localEulerAngles.y < 1) wheelForward.localEulerAngles = Vector3.zero;
            //when wheels are turned
            if (wheelForward.localEulerAngles.y != 0) {
                if (wheelForward.localEulerAngles.y < 180) {
                    wheelForward.Rotate(0, -turnSpeed * wheelStraightenMod * Time.deltaTime, 0);
                } else if (wheelForward.localEulerAngles.y > 270) {
                    wheelForward.Rotate(0, turnSpeed * wheelStraightenMod * Time.deltaTime, 0);
                }
            }
        }
    }

    void StraightCarBody() {
        // when moving
        if (GetComponent<Rigidbody>().linearVelocity.magnitude > 1f) {
            // car rotates towards wheel Direction
            Quaternion targetRotation = Quaternion.LookRotation(wheelForward.forward, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, carBodyTurnSpeed * Time.deltaTime);
        }
    }
}
