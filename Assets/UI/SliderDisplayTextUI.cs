using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderDisplayTextUI : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI tmp; 

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInParent<Slider>();
        tmp = GetComponent<TextMeshProUGUI>();
        slider.onValueChanged.AddListener(delegate { UpdateUI(); });
    }

    private void UpdateUI()
    {
        tmp.text = (slider.value * 10).ToString("F1");
    }
}
