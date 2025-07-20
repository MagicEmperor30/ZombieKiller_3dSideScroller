using UnityEngine;
public class WalkState : IState
{
    private PlayerController player;

    public WalkState(PlayerController player) => this.player = player;

    public void Enter()
    {
        player.Move(player.walkSpeed);
        player.SetAnimSpeed(0.5f); // midpoint blend
        Debug.Log("Walk State");
    }
    public void Update()
    {
        if (!player.IsMoving())
            player.stateMachine.SetState(new IdleState(player));
        else if (player.IsRunning())
            player.stateMachine.SetState(new RunState(player));

        player.Move(player.walkSpeed);
    }

    public void Exit() => Debug.Log("Exited Walk");
}
