using UnityEngine;
using System.Collections.Generic;

public class Interactable : MonoBehaviour
{
    /*perPlayerInteractCooldown is a cooldown unique to each player, whereas interactCooldown
    is a cooldown shared by all players*/
    [SerializeField] float perPlayerInteractCooldown = 0;
    [SerializeField] float interactCooldown = 0;
    [SerializeField] bool isInteractable = true;
    //maps a unique player id (TODO) to last interaction time
    Dictionary<Player, float> lastInteractions = new Dictionary<Player, float>();

    public virtual void Interact(Player player) {
        InteractLogic(player);
        RegisterInteraction(player);
    }

    public virtual void InteractLogic(Player player) {}

    public virtual void RegisterInteraction(Player player) {
        lastInteractions[player] = Time.time;
    }

    public virtual bool CanInteract(Player player) {
        if (!isInteractable) return false;
        if (!lastInteractions.ContainsKey(player)) return true;
        if (Time.time - lastInteractions[player] < interactCooldown) return false;
        return true;
    }
}
