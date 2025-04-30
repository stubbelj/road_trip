using UnityEngine;

public class SteeringWheel : Interactable
{
    [SerializeField] CarController car;
    [SerializeField] Player driver;
    public override void InteractLogic(Player player) {
        if (!driver) {
            driver = player;
            player.StartDriving();
        } else if (driver == player) {
            driver = null;
            player.StopDriving();
        }
    }
}
