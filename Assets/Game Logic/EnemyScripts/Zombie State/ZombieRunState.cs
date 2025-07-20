using UnityEngine;

public class ZombieRunState : IZombieState
{
    private ZombieAI zombie;

    public ZombieRunState(ZombieAI ai)
    {
        zombie = ai;
    }

    public void UpdateState()
    {
        zombie.SetAnimationState(idle: false, walking: false, running: true);

        float distance = Vector3.Distance(zombie.transform.position, zombie.player.position);
        float stopDistance = zombie.attackRange * 0.5f; // stop slightly before actual attack range

        // Only move if not too close
        if (distance > stopDistance)
        {
            // Flip to face player
        // float yRotation = (zombie.player.position.x < zombie.transform.position.x) ? -90f : 90f;
        // zombie.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f)
            // Move toward player at run speed
        Vector3 direction = (zombie.player.position - zombie.transform.position).normalized;
        Vector3 targetPosition = zombie.transform.position + direction * zombie.runSpeed* Time.deltaTime;

        zombie.rb.MovePosition(targetPosition);;
        }

        // Transition to attack if inside range
        if (distance <= zombie.attackRange)
        {
            zombie.TransitionToState(zombie.attackState);
        }
    }
}
