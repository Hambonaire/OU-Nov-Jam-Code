using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OnClickHandler : MonoBehaviour
{
    UIManager uiManager;
    BaseManager baseManager;

    private enum ClickType { Minion, Structure, Plot , Base }
    
    [SerializeField]
    ClickType myType;

    private Minion myMinion;
    private Plot myPlot;
    private Structure myStructure;

    private void Start()
    {
        uiManager = UIManager._instance;
        baseManager = BaseManager._instance;

        myMinion = GetComponent<Minion>();
        myPlot = GetComponent<Plot>();
        myStructure = GetComponent<Structure>();

        if (myMinion != null)
            myType = ClickType.Minion;
        else if (myPlot != null)
            myType = ClickType.Plot;
        else if (myStructure != null)
            myType = ClickType.Structure;
    }

    private void OnMouseDown()
    {
        if (uiManager.eventSystem.IsPointerOverGameObject())
            return;

        uiManager.mouseDownOverValid = true;

        // Pass to UIManager
        if (myType == ClickType.Base)
            uiManager.BuildBaseUI();
        else if (myType == ClickType.Minion)
            uiManager.BuildMinionUI(myMinion);
        else if (myType == ClickType.Plot)
        {
            baseManager.selectedPlot = myPlot;
            if (myPlot.myStructure != null)
                baseManager.selectedStructure = myPlot.myStructure;
            else
                baseManager.selectedStructure = null;

            uiManager.BuildPlotUI(myPlot);
        }
        else if (myType == ClickType.Structure)
        {
            baseManager.selectedStructure = myStructure;
            if (myStructure.myPlot != null)
            {
                baseManager.selectedPlot = myStructure.myPlot;
            }

            uiManager.BuildStructureUI(myStructure);
        }
    }
}
