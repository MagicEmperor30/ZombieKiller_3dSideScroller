using UnityEngine;

public class VehicleControllerWithWheels : MonoBehaviour
{
    public float motorTorque = 150f;
    public float maxSteerAngle = 30f;
    public float brakeForce = 300f;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    public Transform meshFL;
    public Transform meshFR;
    public Transform meshBL;
    public Transform meshBR;

    public Transform seatPoint;
    public Transform frontExitPoint;
    public Transform backExitPoint;
    private float inputVertical;
    private float inputHorizontal;
    private float lastVerticalInput = 0f;
    private bool isBraking;
    public PlayerController attachedPlayer;

    void FixedUpdate()
    {
        ApplyMotor();
        ApplySteering();
        ApplyBraking();
        UpdateWheelMeshes();
    }

    public void Interact(PlayerController player)
    {
        player.EnterVehicle(this);
    }

    public void ExitVehicle()
    {
        if (attachedPlayer != null)
        {
            Transform chosenExit = lastVerticalInput >= 0 ? frontExitPoint : backExitPoint;
            if (chosenExit == null) chosenExit = frontExitPoint != null ? frontExitPoint : backExitPoint;
            attachedPlayer.ExitVehicle(chosenExit);
            attachedPlayer = null;
        }
    }


    public void SetInputs(float vertical, float horizontal, bool brake)
    {
        inputVertical = vertical;
        inputHorizontal = horizontal;
        isBraking = brake;

        if (Mathf.Abs(vertical) > 0.1f)
            lastVerticalInput = vertical;
    }

    private void ApplyMotor()
    {
        float torque = Mathf.Clamp(inputVertical, -1f, 1f) * motorTorque;
        wheelBL.motorTorque = torque;
        wheelBR.motorTorque = torque;
    }


    private void ApplySteering()
    {
        // Disable steering for side-scroller
        wheelFL.steerAngle = 0f;
        wheelFR.steerAngle = 0f;
    }


    private void ApplyBraking()
    {
        float brakeTorque = isBraking ? brakeForce : 0f;

        wheelFL.brakeTorque = brakeTorque;
        wheelFR.brakeTorque = brakeTorque;
        wheelBL.brakeTorque = brakeTorque;
        wheelBR.brakeTorque = brakeTorque;
    }

    private void UpdateWheelMeshes()
    {
        UpdateWheelPose(wheelFL, meshFL);
        UpdateWheelPose(wheelFR, meshFR);
        UpdateWheelPose(wheelBL, meshBL);
        UpdateWheelPose(wheelBR, meshBR);
    }

    private void UpdateWheelPose(WheelCollider col, Transform mesh)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        // Optional: flip rotation for right-side wheels
        if (mesh == meshFR || mesh == meshBR)
        {
            // Invert the rotation around Z axis
            rot *= Quaternion.Euler(0f, 180f, 0f);
        }

        mesh.rotation = rot;
    }
}
