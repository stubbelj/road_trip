using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] bool isDriving = false;
    [SerializeField] Car car;

    public void EnterCar(Car newCar) {
        SetCar(newCar);
        foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(true)) {
            transform.gameObject.layer = LayerMask.NameToLayer("NotCar");
        }
        GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(newCar.transform);
        playerController.EnterCar(newCar);
        newCar.EnterCar(this);
    }

    public void ExitCar(Car newCar) {
        SetCar(null);
        foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(true)) {
            transform.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        GetComponent<Rigidbody>().isKinematic = false;
        transform.SetParent(null);
        playerController.ExitCar(newCar);
        newCar.ExitCar(this);
    }

    public void EnterSeat(Transform sitTarget) {
        SetTransform(sitTarget);
    }

    public void ExitSeat(Transform exitTarget) {
        SetTransform(exitTarget);
    }

    void SetTransform(Transform newTransform) {
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;
    }

    public void SetCar(Car newCar) {
        car = newCar;
    }

    public void StartDriving() {
        playerController.StartDriving();
    }

    public void StopDriving() {
        playerController.StopDriving();
    }
}
