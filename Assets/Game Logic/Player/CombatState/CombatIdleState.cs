using UnityEngine;

public class CombatMovementState : IState
{
    private PlayerController player;
    //private float shootCooldown = 0.3f;
    private float shootTimer;

    public CombatMovementState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.SetCombatMode(true);
        player.SetDefend(false);
        //player.pistolWeapon.gameObject.SetActive(true);
        shootTimer = 0f;
    }

    public void Update()
    {
        if(player.isDriving) return;
        player.UpdateLocomotionAnimation();
        // Shooting
        if (player.WantsToShoot() )
        {
            player.Shoot();
        }

    }

    public void Exit()
    {
         player.SetCombatMode(false);
    }
}
