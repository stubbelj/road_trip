using UnityEngine;

public class CarDoor : Interactable
{
    [SerializeField] Car car;
    public override void InteractLogic(Player player) {
        if (!car.IsFull()) {
            player.EnterCar(car);
        }
    }
}
