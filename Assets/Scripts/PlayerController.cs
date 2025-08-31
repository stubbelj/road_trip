using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Rotate Params")]
    [SerializeField] float turnSpeedX;
    [SerializeField] float turnSpeedY;
    [SerializeField] Vector3 maxLookRotation;
    [Header("Interaction Params")]
    [SerializeField] float interactCooldown;
    [SerializeField] float lastInteraction;
    [SerializeField] float interactRange;
    [SerializeField] Transform interactCheckOrigin;
    [Header("Driving Params")]
    [SerializeField] bool isDriving;
    [Header("Walking Params")]
    [SerializeField] float walkSpeed;
    [Header("Other")]
    [SerializeField] Camera playerCam;
    [SerializeField] ControlType controlType;
    [SerializeField] Player player;
    
    Car car;
    PlayerInputs playerInputs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.lockCursor = true;
	    Cursor.visible = false;
        playerInputs = new PlayerInputs(controlType);
    }

    void Update() {
        playerInputs.UpdatePlayerInputs();

        Look(playerInputs.LookValue);

        if (!isDriving) {
            if (!car) {
                Walk(playerInputs.MoveValue);
            }
            if (playerInputs.InteractValue) TryInteraction();
        } else {
            car.Drive(playerInputs.MoveValue);
        }
    }

    public void LateUpdate() {
        if (!isDriving) PhysicsUtils.ClampToMaxObjectSpeed(GetComponent<Rigidbody>());
    }

    public void EnterCar(Car newCar) {
        SetCar(newCar);
    }

    public void ExitCar(Car newCar) {
        SetCar(null);
    }

    public void SetCar(Car newCar) {
        car = newCar;
    }

    public void StartDriving() {
        isDriving = true;
    }

    public void StopDriving() {
        isDriving = false;
    }

    void Look(Vector2 inputs) {
        Vector3 newLocalRotation = transform.localEulerAngles;

        newLocalRotation += new Vector3(
            inputs.y * -1 * turnSpeedX * Time.deltaTime * 0.35f,
            inputs.x * turnSpeedY * Time.deltaTime,
            0);

        //clamp x rot
        if ((newLocalRotation.x > 180 ? 360 - newLocalRotation.x : newLocalRotation.x) > maxLookRotation.x) {
            if (newLocalRotation.x < 180) {
                newLocalRotation.x = maxLookRotation.x;
            } else if (newLocalRotation.x > 180) {
                newLocalRotation.x = -maxLookRotation.x;
            }
        }
        //clamp z rot
        if ((newLocalRotation.z > 180 ? 360 - newLocalRotation.z : newLocalRotation.z) > maxLookRotation.z) {
            if (newLocalRotation.z < 180) {
                newLocalRotation.z = maxLookRotation.z;
            } else if (newLocalRotation.z > 180) {
                newLocalRotation.z = maxLookRotation.z;
            }
        }

        transform.localEulerAngles = newLocalRotation;
    }

    void Walk(Vector2 inputs) {
        Vector3 walkForce = (transform.right * inputs.x * walkSpeed) + (transform.forward * inputs.y * walkSpeed);
        walkForce.y = 0;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(walkForce);
        rb.linearVelocity =
        new Vector3(Mathf.Clamp(rb.linearVelocity.x, -walkSpeed, walkSpeed),
                    rb.linearVelocity.y,
                    Mathf.Clamp(rb.linearVelocity.z, -walkSpeed, walkSpeed));
    }
    void TryInteraction() {
        print("TryInteraction");
        if (!CanInteract()) return;

        RaycastHit hit;
        if (Physics.Raycast(interactCheckOrigin.position, interactCheckOrigin.forward, out hit, interactRange, LayerMask.GetMask("Interactable"))) {
            print(hit.collider.transform.gameObject);
            Interactable interactable = hit.collider.transform.GetComponent<Interactable>();
            if (CanInteractWithInteractable(interactable)) {
                if (interactable.CanInteract(player)) {
                    interactable.Interact(player);
                    lastInteraction = Time.time;
                }
            }
        }
    }

    public bool CanInteract() {
        return (Time.time - lastInteraction > interactCooldown);
    }

    public bool CanInteractWithInteractable(Interactable interactable) {
        if (!CanInteract()) return false;
        return interactable.CanInteract(player);
    }

    public PlayerInputs GetPlayerInputs() {
        return playerInputs;
    }

    public enum ControlType {
            KEYBOARD, GAMEPAD
        }

    public class PlayerInputs {
        public ControlType ControlType;
        public Vector2 MoveValue, LookValue;
        public bool InteractValue, JumpValue;
        public PlayerInputs(ControlType newControlType) {
            ControlType = newControlType;
        }

        public void UpdatePlayerInputs() {
            MoveValue = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
            LookValue = InputSystem.actions.FindAction("Look").ReadValue<Vector2>();
            InteractValue = Mathf.Approximately(InputSystem.actions.FindAction("Interact").ReadValue<float>(), 1f);
            JumpValue = Mathf.Approximately(InputSystem.actions.FindAction("Jump").ReadValue<float>(), 1f);
        }
    }
}