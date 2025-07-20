using UnityEngine;

public class ZombieWalkState : IZombieState
{
    private ZombieAI zombie;

    public ZombieWalkState(ZombieAI ai)
    {
        zombie = ai;
    }

    public void UpdateState()
    {
        zombie.SetAnimationState(idle: false, walking: true, running: false);
        float distance = Vector3.Distance(zombie.transform.position, zombie.player.position);

        // // 🔁 Flip to face player
        // float yRotation = (zombie.player.position.x < zombie.transform.position.x) ? -90f : 90f;
        // zombie.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        //  Only move if player is within detection range
        if (!zombie.isChasing && distance > zombie.detectionRange)
        {
            zombie.TransitionToState(zombie.idleState);
            return;
        }
        float stopDistance = zombie.attackRange * 0.5f;
        if (distance > stopDistance)
        {
            //  Move toward player
            Vector3 direction = (zombie.player.position - zombie.transform.position).normalized;
            Vector3 move = zombie.transform.position + direction * zombie.walkSpeed* Time.deltaTime;

            zombie.rb.MovePosition(move); 
        }
        else
        {
            //  Close enough to attack
            zombie.TransitionToState(zombie.attackState);
        }

        // 💢 Enraged?
        if (zombie.zombieHealth.IsEnraged())
        {
            zombie.TransitionToState(zombie.runState);
        }
    }
}
