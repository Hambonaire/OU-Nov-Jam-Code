using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;
    GameManager gameManager;
    WaveManager waveManager;
    BaseManager baseManager;
    public EventSystem eventSystem;

    [HideInInspector]
    public bool mouseDownOverValid = false;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI gateHealth;
    public TextMeshProUGUI baseHealth;

    [Header("General")]
    public GameObject GeneralUI;


    [Header("Plot")]
    public GameObject plotUI;
    public List<PlotButton> plotButtons = new List<PlotButton>();

    [Header("Structure")]
    public GameObject structureUI;
    public List<StructureButton> structureButtons = new List<StructureButton>();

    [Header("Base")]
    public GameObject relicButtonPrefab;
    
    public GameObject baseUI;
    public TextMeshProUGUI baseUpgradeCost;
    public TextMeshProUGUI minionLevel;
    public TextMeshProUGUI minionLevelCost;
    public TextMeshProUGUI minionRateLevel;
    public TextMeshProUGUI minionRateCost;

    public GameObject inheritanceRelicContent;
    public GameObject inventoryRelicContent;
    public Button moveRelicButton;
    public TextMeshProUGUI InheritanceGold;
    public TextMeshProUGUI InventoryGold;
    public Slider InventoryGoldSlider;

    [HideInInspector]
    public int goldToMoveInherit;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    public void Start()
    {
        gameManager = GameManager._instance;
        waveManager = WaveManager._instance;
        baseManager = BaseManager._instance;
    }

    private void LateUpdate()
    {
        goldText.text = gameManager.gold.ToString();

        baseHealth.text = baseManager.health.ToString();

        gateHealth.text = baseManager.gateHealth.ToString();

        if (!eventSystem.IsPointerOverGameObject() && Input.GetKeyDown(KeyCode.Mouse0))
        {

            if (mouseDownOverValid)
                mouseDownOverValid = false;
            else
                CloseUI();
        }

        if (baseUI.activeInHierarchy)
        {
            goldToMoveInherit = (int)(gameManager.gold * InventoryGoldSlider.value);
            InventoryGold.text = goldToMoveInherit.ToString();

            InheritanceGold.text = gameManager.inheritanceGold.ToString();
        }
    }

    public void BuildPlotUI(Plot plot)
    {
        CloseUI();

        if (plot.myStructure != null)
        {
            BuildStructureUI(plot.myStructure);
            return;
        }

        plotUI.SetActive(true);

        foreach (PlotButton button in plotButtons)
        {
            button.gameObject.SetActive(false);
        }

        for (int index = 0; index < baseManager.buyableStructures.Count; index++)
        {
            plotButtons[index].gameObject.SetActive(true);
            plotButtons[index].Initialize(baseManager.buyableStructures[index]);
        }
    }

    public void BuildStructureUI(Structure structure)
    {
        CloseUI();

        structureUI.SetActive(true);

        foreach (StructureButton button in structureButtons)
        {
            button.gameObject.SetActive(false);
        }

        /**
        for (int index = 0; index < baseManager.selectedStructure.Count; index++)
        {
            plotButtons[index].gameObject.SetActive(true);
            plotButtons[index].Initialize(baseManager.buyableStructures[index]);
        }
        */

        if (baseManager.selectedStructure.upgradePrefab != null)
        {
            structureButtons[0].gameObject.SetActive(true);
            structureButtons[0].Initialize(baseManager.selectedStructure.upgradePrefab);
        }

    }

    public void BuildMinionUI(Minion minion)
    {
        CloseUI();

        print("Build Minion UI");

    }

    public void BuildBaseUI(bool resetSelectedRelics = true)
    {
        CloseUI(resetSelectedRelics);

        gameManager.Pause(true);
        baseUI.SetActive(true);

        if (baseManager.baseLevel < 3)
            baseUpgradeCost.text = baseManager.baseUpgradeCost[baseManager.baseLevel - 1].ToString();
        else
            baseUpgradeCost.text = "MAX";

        minionLevel.text = "lvl. " + baseManager.minionLevel.ToString();
        if (baseManager.minionLevel < 3)
            minionLevelCost.text = baseManager.minionUpgradeCost[baseManager.minionLevel - 1].ToString();
        else
        {
            minionLevelCost.text = "MAX";
        }

        minionRateLevel.text = "lvl. " + baseManager.minionSpawnrateLevel.ToString();
        if (baseManager.minionSpawnrateLevel < 3)
            minionRateCost.text = baseManager.minionRateUpgradeCost[baseManager.minionSpawnrateLevel].ToString();
        else
        {
            minionRateCost.text = "MAX";
        }

        CreateRelicButtons();

        InventoryGoldSlider.value = 0;
    }

    private void CreateRelicButtons()
    {
        foreach (Transform transform in inventoryRelicContent.transform)
            Destroy(transform.gameObject);
        foreach (Transform transform in inheritanceRelicContent.transform)
            Destroy(transform.gameObject);

        // Create buttons
        for (int x = 0; x < gameManager.inventoryRelics.Count; x++)
        {
            var newButton = Instantiate(relicButtonPrefab, inventoryRelicContent.transform);
            newButton.GetComponent<RelicButton>().Initialize(gameManager.inventoryRelics[x], x, true);
        }
        
        for (int y = 0; y < gameManager.inheritanceRelics.Count; y++)
        {
            var newButton = Instantiate(relicButtonPrefab, inheritanceRelicContent.transform);
            newButton.GetComponent<RelicButton>().Initialize(gameManager.inheritanceRelics[y], y, false);
        }
        
    }

    public void CloseUI(bool resetSelectedRelics = true)
    {
        if (resetSelectedRelics)
            gameManager.selectedInvRelicIndicies.Clear();

        gameManager.Pause(false);

        plotUI.SetActive(false);
        structureUI.SetActive(false);
        baseUI.SetActive(false);
    }
}
