using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] CarController carController;
    [SerializeField] Seat[] seats;
    
    public void Drive(Vector2 inputs) {
        carController.Drive(inputs);
    }

    public bool IsFull() {
        foreach(Seat seat in seats) {
            if (!seat.player) return false;
        }
        return true;
    }

    public void EnterCar(Player player) {
        foreach(Seat seat in seats) {
            if (!seat.player) {
                player.EnterSeat(seat.SitTransform);
            }
        }
    }

    public void ExitCar(Player player) {
        foreach (Seat seat in seats) {
            if (seat.player == player) {
                player.ExitSeat(seat.ExitTransform);
            }
        }
    }

    [System.Serializable]
    class Seat {
        public Transform SitTransform;
        public Transform ExitTransform;
        public Player player;
    }
}
