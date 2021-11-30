using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeBaseButton : MonoBehaviour
{
    BaseManager baseManager;

    public TextMeshProUGUI costText;

    private void Start()
    {
        baseManager = BaseManager._instance;
    }

    private void Update()
    {
        if (baseManager.baseLevel < 3)
            costText.text = baseManager.baseUpgradeCost[baseManager.baseLevel - 1].ToString();
        else
            gameObject.SetActive(false);
    }

    // Build the structure @ selected plot
    public void OnButtonPress()
    {
        baseManager.UpgradeBase();
    }
}
