using UnityEngine;

public class DefendState : IState
{
    private PlayerController player;
    private float defendDuration = 4f; // Time the player can defend (when shield is active)
    private float defendCooldown = 5f; // Time after which defend can be used again
    private float defendTimeRemaining;
    private float defendCooldownRemaining;
    private bool isDefendingCooldownActive;

    public DefendState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.SetAnimSpeed(0f); // Freeze animation
        player.SetDefend(true);  // Enable shield
        player.Move(0f);         // Stop movement

        // Initialize the timers
        defendTimeRemaining = defendDuration;
        defendCooldownRemaining = defendCooldown;
        isDefendingCooldownActive = false;

        Debug.Log("Entered Defend State");

        // Optionally, hide the defend button if we are in the cooldown period
        if (player.defendCooldownText != null)
            player.defendCooldownText.text = defendCooldownRemaining + "s"; // Show the cooldown on UI
    }

    public void Update()
    {
        // Handle the active defend timer
        if (defendTimeRemaining > 0f)
        {
            defendTimeRemaining -= Time.deltaTime;
            
            // Update the defend duration UI
            if (player.defendDurationText != null)
                player.defendDurationText.text = Mathf.Ceil(defendTimeRemaining) + "s";

            // If defend time is up, stop defending
            if (defendTimeRemaining <= 0f)
            {
                player.SetDefend(false);
                player.stateMachine.SetState(player.IsInCombat() ? new CombatMovementState(player) : new IdleState(player));
                Debug.Log("Exited Defend State (auto)");
            }
        }
        else
        {
            // If the defend state has ended, start cooldown if not already active
            if (!isDefendingCooldownActive)
            {
                isDefendingCooldownActive = true;
                StartCooldown();
            }
        }

        // Handle the cooldown timer
        if (isDefendingCooldownActive && defendCooldownRemaining > 0f)
        {
            defendCooldownRemaining -= Time.deltaTime;
            // Update cooldown timer UI
            if (player.defendCooldownText != null)
                player.defendCooldownText.text = Mathf.Ceil(defendCooldownRemaining) + "s";

            // Once cooldown is done, allow the player to defend again
            if (defendCooldownRemaining <= 0f)
            {
                isDefendingCooldownActive = false;
                defendCooldownRemaining = defendCooldown; // Reset the cooldown
                Debug.Log("Defend Cooldown Over");
            }
        }
    }

    public void Exit()
    {
        player.SetDefend(false); // End defend state
        Debug.Log("Exited Defend State");
    }

    private void StartCooldown()
    {
        // Logic to start the cooldown
        if (player.defendCooldownText != null)
        {
            player.defendCooldownText.text = Mathf.Ceil(defendCooldownRemaining) + "s"; // Show cooldown on UI
        }
    }
}
