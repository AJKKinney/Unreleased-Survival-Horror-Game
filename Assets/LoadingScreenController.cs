using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustenKinney.GameState;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private Slider loadingBar;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.OnShowLoadingScreen += ShowLoadingScreen;
        LoadingState.OnLoadingProgress += UpdateLoadingProgress;
    }

    private void OnDestroy()
    {
        UIManager.OnShowLoadingScreen -= ShowLoadingScreen;
        LoadingState.OnLoadingProgress -= UpdateLoadingProgress;
    }

    private void ShowLoadingScreen()
    {
        loadingScreenPanel.SetActive(true);
    }

    private void UpdateLoadingProgress(float progress)
    {
        loadingBar.value = progress;
    }
}
