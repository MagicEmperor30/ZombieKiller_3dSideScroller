using UnityEngine;

public class MobileOptimizationManager : MonoBehaviour
{
    [Header("Frame Rate Settings")]
    [Tooltip("Target frame rate (30 is recommended for battery savings)")]
    public int targetFrameRate = 30;

    [Tooltip("Enable VSync (usually OFF on mobile)")]
    public bool enableVSync = false;

    [Header("Resolution Scaling")]
    [Range(0.5f, 1f)]
    [Tooltip("Render scale for resolution (1 = full, lower = better performance)")]
    public float resolutionScale = 0.8f;

    [Header("Memory Thresholds (MB)")]
    public int lowMemoryThreshold = 1500;  // RAM
    public int lowGraphicsMemoryThreshold = 512;  // VRAM

    [Header("Quality Settings")]
    public int lowQualityLevel = 0;
    public int highQualityLevel = 2;

    void Start()
    {
        OptimizePerformance();
    }

    void OptimizePerformance()
    {
        // 1. Set target frame rate and VSync
        QualitySettings.vSyncCount = enableVSync ? 1 : 0;
        Application.targetFrameRate = targetFrameRate;

        // 2. Adjust render scale
        if (resolutionScale < 1f)
        {
            ScalableBufferManager.ResizeBuffers(resolutionScale, resolutionScale);
            Debug.Log($"Resolution scaled to {resolutionScale * 100}%");
        }

        // 3. Choose quality level based on memory
        int systemRAM = SystemInfo.systemMemorySize;
        int graphicsRAM = SystemInfo.graphicsMemorySize;

        if (systemRAM <= lowMemoryThreshold || graphicsRAM <= lowGraphicsMemoryThreshold)
        {
            QualitySettings.SetQualityLevel(lowQualityLevel, true);
            Debug.Log("Low-end device detected. Setting low quality.");
        }
        else
        {
            QualitySettings.SetQualityLevel(highQualityLevel, true);
            Debug.Log("Mid/High-end device detected. Setting higher quality.");
        }

        Debug.Log($"Target FPS: {targetFrameRate}, VSync: {enableVSync}, RAM: {systemRAM}MB, VRAM: {graphicsRAM}MB");
    }
}
