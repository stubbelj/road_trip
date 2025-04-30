using UnityEngine;

public class TestingUtils : MonoBehaviour
{
    [SerializeField] CarController car;
    [SerializeField] Transform[] players;

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey("r")) {
        //     car.transform.position = Vector3.zero;
        //     car.transform.rotation = Quaternion.identity;
        //     foreach (Transform player in players) {
        //         player.position = Vector3.zero;
        //         player.position = Vector3.zero;
        //     }
        // }
    }
}
