using UnityEngine;

public class ZombieAttackState : IZombieState
{
    private ZombieAI zombie;
    private float attackCooldown = 1f;
    private float lastAttackTime;

    public ZombieAttackState(ZombieAI ai)
    {
        zombie = ai;
    }

    public void UpdateState()
    {
        if (zombie.player == null || zombie.playerController.isDead)
        {
            zombie.StopAttackAnimation(); // prevent repeated attack animation
            zombie.TransitionToState(zombie.idleState);
            return;
        }

        if (zombie.playerController.IsDefending())
        {
            zombie.StopAttackAnimation();
            zombie.TransitionToState(zombie.idleState);
            return;
        }

        if (!zombie.IsPlayerInRange())
        {
            zombie.StopAttackAnimation();
            zombie.TransitionToState(zombie.walkState);
            return;
        }

        if (Time.time > lastAttackTime + attackCooldown)
        {
            zombie.SetAttackAnimation(); // Sets the "Attack" trigger
            lastAttackTime = Time.time;
        }

        if (zombie.zombieHealth.IsEnraged())
        {
            zombie.StopAttackAnimation();
            zombie.TransitionToState(zombie.runState);
        }
    }
}
