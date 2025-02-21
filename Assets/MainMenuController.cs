using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.OnHideMainMenu += HideMenu;
    }

    private void OnDestroy()
    {
        UIManager.OnHideMainMenu -= HideMenu;
    }

    private void HideMenu()
    {
        gameObject.SetActive(false);
    }
}
