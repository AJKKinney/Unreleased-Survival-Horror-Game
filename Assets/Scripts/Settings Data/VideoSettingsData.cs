using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VideoSettingsData
{
    [Header("Video Settings")]

    public Vector2 gameResolution = new Vector2(1920, 1080);
    public QualityLevel graphicsQuality = QualityLevel.High;
    public QualityLevel lightingQuality = QualityLevel.High;
    public bool vSync = false;

    public VideoSettingsData()
    {

    }

    public VideoSettingsData(Vector2 setGameResolution, QualityLevel setGraphicsQuality, QualityLevel setLightingQuality, bool setVSync)
    {
        gameResolution = setGameResolution;
        graphicsQuality = setGraphicsQuality;
        lightingQuality = setLightingQuality;
        vSync = setVSync;
    }
}

public enum QualityLevel
{
    Ultra,
    High,
    Medium,
    Low
}
