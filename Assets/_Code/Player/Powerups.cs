using UnityEngine;
using static Pickups;

public class PowerUps : MonoBehaviour, IPickup
{
    public void Pickup(GameObject Player)
    {
        PlayerController pc = Player.transform.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.winCondition();
            pc.growPowerUp();
            pc.shrinkPowerUp();// Trigger the jump power-up effect
            Destroy(gameObject); // Destroys the power-up on collision with the player

            pc.healPlayer();
            Destroy(gameObject);
        }
    }
}

