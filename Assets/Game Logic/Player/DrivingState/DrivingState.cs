using UnityEngine;

public class DrivingState : IState
{
    private PlayerController player;
    private VehicleControllerWithWheels vehicle;

    public DrivingState(PlayerController player, VehicleControllerWithWheels vehicle)
    {
        this.player = player;
        this.vehicle = vehicle;
    }

    public void Enter()
    {
        if (!player.isDriving)
        {
            player.EnterVehicle(vehicle);
            player.animator.SetLayerWeight(0, 0f); // Disable base layer
            player.animator.SetLayerWeight(1, 1f); // Enable driving layer
            Debug.Log("Enter Driving State");
        }
    }

    public void Update()
    {
        Vector2 input = player.GetInput();


            float throttle = 0f;
            bool brake = false;

            if (input.x > 0.1f)
            {
                throttle = 1f;   // Move forward
            }
            else if (input.x < -0.1f)
            {
                throttle = -1f;  // Move backward
            }
            brake = player.WantsToBrake();

        vehicle.SetInputs(throttle, 0f, brake);
    }

    public void Exit()
    {
        vehicle.SetInputs(0f, 0f, false);
        player.animator.SetLayerWeight(0, 1f);
        player.animator.SetLayerWeight(1, 0f);
        Debug.Log("Exit Driving State");
    }
}
