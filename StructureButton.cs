using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StructureButton : MonoBehaviour
{
    BaseManager baseManager;

    public Image StructureIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;

    public GameObject myStructureObj;
    Structure myStructure;

    public void Initialize(GameObject structObj)
    {
        myStructureObj = structObj;

        myStructure = myStructureObj.GetComponent<Structure>();

        StructureIcon.sprite = myStructure.structureIcon;
        nameText.text = myStructure.structureName;
        costText.text = myStructure.buildCost.ToString(); ;

        baseManager = BaseManager._instance;
    }

    // Build the structure @ selected plot
    public void OnButtonPress()
    {
        print("Button Up");

        baseManager.UpgradeStructure();
    }
}
