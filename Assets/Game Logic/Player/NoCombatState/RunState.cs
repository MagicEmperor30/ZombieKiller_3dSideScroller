using UnityEngine;
public class RunState : IState
{
    private PlayerController player;

    public RunState(PlayerController player) => this.player = player;

    public void Enter()
    {
        player.Move(player.runSpeed);
        player.SetAnimSpeed(1f); // full blend
        Debug.Log("Run State");
    }

    public void Update()
    {
        if (!player.IsMoving())
            player.stateMachine.SetState(new IdleState(player));
        else if (!player.IsRunning())
            player.stateMachine.SetState(new WalkState(player));

        player.Move(player.runSpeed);
    }

    public void Exit() => Debug.Log("Exited Run");
}
