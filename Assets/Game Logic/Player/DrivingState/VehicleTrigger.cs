using UnityEngine;

public class VehicleTrigger : MonoBehaviour
{
    private VehicleControllerWithWheels vehicle;

    void Awake()
    {
        vehicle = GetComponentInParent<VehicleControllerWithWheels>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            if (!vehicle) return;

            player.currentVehicle = vehicle;
            vehicle.attachedPlayer = player;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            if (player.currentVehicle == vehicle)
            {
                player.currentVehicle = null;
                vehicle.attachedPlayer = null;
            }
        }
    }
}
