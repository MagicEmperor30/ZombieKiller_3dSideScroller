using UnityEngine;

public class ZombieIdleState : IZombieState
{
    private ZombieAI zombie;

    public ZombieIdleState(ZombieAI ai)
    {
        zombie = ai;
    }

    public void UpdateState()
    {
        zombie.SetAnimationState(idle: true, walking: false, running: false);
        if (zombie.IsPlayerInRange())
        {
            
            zombie.TransitionToState(zombie.attackState);
        }
        else
        {
            zombie.TransitionToState(zombie.walkState);
        }
    }
}
