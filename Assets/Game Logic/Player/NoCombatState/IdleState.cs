using UnityEngine;

public class IdleState : IState
{
    private PlayerController player;

    public IdleState(PlayerController player) => this.player = player;

    public void Enter()
    {
        player.Move(0f);
        player.SetAnimSpeed(0f);
        Debug.Log("Idle State");
    }

    public void Update()
    {
        if(player.isDriving) return;
        if (player.IsRunning())
            player.stateMachine.SetState(new RunState(player));
        else if (player.IsMoving())
            player.stateMachine.SetState(new WalkState(player));
    }

    public void Exit() => Debug.Log("Exited Idle");
}
