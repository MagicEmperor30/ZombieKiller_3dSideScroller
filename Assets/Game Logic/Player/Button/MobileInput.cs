using UnityEngine;
using UnityEngine.UI;

public class MobileInput : MonoBehaviour
{
    public static MobileInput Instance;

    public Button moveLeftButton;
    public Button moveRightButton;
    public Button shootButton;
    public Button defendButton;
    public Button driveButton;
    public Button combatButton;
    public Button runButton;
    public Button brakeButton;

    public bool isMovingLeft;
    public bool isMovingRight;

    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isShooting;
    [HideInInspector] public bool isDefending;
    [HideInInspector] public bool isBraking;
    [HideInInspector] public bool wantsToDrive;
    [HideInInspector] public bool toggleCombat;

    private PlayerController player;

    void Awake()
    {
        Instance = this;

        // Action buttons
        shootButton.onClick.AddListener(() => isShooting = true);
        driveButton.onClick.AddListener(() => wantsToDrive = true);
        combatButton.onClick.AddListener(() => toggleCombat = true);
        defendButton.onClick.AddListener(() => isDefending = true);

        player = FindFirstObjectByType<PlayerController>();
    }

    void LateUpdate()
    {
        isShooting = false;
        isDefending = false;
        wantsToDrive = false;
        toggleCombat = false;

        UpdateButtonVisibility();
    }

    public Vector2 GetMoveInput()
    {
        float x = 0;
        if (isMovingLeft) x = -1;
        else if (isMovingRight) x = 1;
        return new Vector2(x, 0);
    }

    public void OnDefendPressed() => isDefending = true;
    public void OnDefendReleased() => isDefending = false;

    private void UpdateButtonVisibility()
    {
        if (player == null) return;

        bool isDriving = player.isDriving;
        bool inCombat = player.inCombat;
        bool canDrive = player.currentVehicle != null && !isDriving;

        // Always visible
        moveLeftButton.gameObject.SetActive(true);
        moveRightButton.gameObject.SetActive(true);

        // Combat buttons only in combat mode
        shootButton.gameObject.SetActive(inCombat && !isDriving);
        defendButton.gameObject.SetActive(inCombat && !isDriving);
        combatButton.gameObject.SetActive(!isDriving); // Hide combat toggle in car

        // Drive only when near car and not already in it
        driveButton.gameObject.SetActive(canDrive || isDriving);

        // Brake only while driving
        brakeButton.gameObject.SetActive(isDriving);

        // Run only when not driving
        runButton.gameObject.SetActive(!isDriving);
    }
}
