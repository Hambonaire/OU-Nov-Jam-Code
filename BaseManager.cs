using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public static BaseManager _instance;
    GameManager gameManager;
    WaveManager waveManager;
    UIManager uiManager;

    //public GameObject manorParent;
    //public GameObject campParent;
    //public GameObject castleParent;

    public GameObject[] baseParents;
    public Minion[] gateObjects;

    [Header("Stats")]
    public int health = 50;
    public int gateHealth = 10;

    [Header("Levels")]
    public int baseLevel = 1;
    public int minionLevel = 1;
    public int minionSpawnrateLevel = 0;
    float lastSpawnTime = 0;
    float[] minionSpawnRate = { 13, 8, 3};


    [Header("Upgrade Costs")]
    public int[] baseUpgradeCost = { 10000, 500000 };
    public int[] minionUpgradeCost = { 5000, 100000 };
    public int[] minionRateUpgradeCost = { 1000, 50000, 250000 };


    public List<GameObject> buyableStructures = new List<GameObject>();
    public List<GameObject> buyableMinions = new List<GameObject>();


    //public List<Plot> allPlots = new List<Plot>();
    public Plot selectedPlot;
    public Structure selectedStructure;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager._instance;
        uiManager = UIManager._instance;
        waveManager = WaveManager._instance;
    }

    // Update is called once per frame
    void Update()
    {
        gateHealth = (int)gateObjects[baseLevel - 1].health;

        if (minionSpawnrateLevel != 0)
        {
            if (Time.time >= lastSpawnTime + minionSpawnRate[minionSpawnrateLevel - 1])
            {
                Instantiate(buyableMinions[minionLevel - 1], waveManager.pathWaypoints[waveManager.pathWaypoints.Length - 1].position, waveManager.pathWaypoints[waveManager.pathWaypoints.Length - 1].rotation);

                lastSpawnTime = Time.time;
            }
        }
    }

    public void TakeHealthDamage(int damage)
    {
        health -= damage;
    }

    public void UpgradeBase()
    {
        if (baseLevel < 3 && gameManager.gold > baseUpgradeCost[baseLevel - 1])
        {
            gameManager.DeductGold(baseUpgradeCost[baseLevel - 1]);

            baseParents[baseLevel- 1].SetActive(false);

            baseLevel++;

            baseParents[baseLevel - 1].SetActive(true);
        }
    }

    public void RepairGate()
    {

    }

    public void ConstructStructure(GameObject newStructure)
    {

        if (selectedPlot != null &&
            selectedPlot.myStructure == null &&
            newStructure.GetComponent<Structure>().buildCost <= gameManager.gold)
        {
            selectedStructure = selectedPlot.BuildStructure(newStructure);

            //uiManager.BuildStructureUI(selectedStructure);
            uiManager.BuildStructureUI(selectedStructure);

        }
    }

    public void UpgradeStructure()
    {
        if (selectedPlot != null &&
            selectedPlot.myStructure != null &&
            selectedStructure != null &&
            selectedStructure.upgradeCost <= gameManager.gold)
        {
            print("Base Manager Up");

            selectedStructure = selectedStructure.Upgrade();

            //uiManager.BuildStructureUI(selectedStructure);
            uiManager.BuildStructureUI(selectedStructure);
        }
    }

    public void RemoveStructure()
    {
        if (selectedPlot != null &&
            selectedPlot.myStructure != null &&
            selectedStructure != null)
        {
            if (selectedPlot.RemoveStructure())
            {
                selectedStructure = null;

                uiManager.BuildPlotUI(selectedPlot);
            }
        }
    }

    public void UpgradeMinionLevel()
    {
        Debug.Log("Upgrade M level!");

        if (minionLevel < 3 && gameManager.gold > minionUpgradeCost[minionLevel - 1])
        {
            Debug.Log("Upgraded M level!");

            gameManager.DeductGold(minionUpgradeCost[minionLevel - 1]);
            minionLevel++;
        }

        uiManager.BuildBaseUI();
    }

    public void UpgradeMinionRate()
    {
        Debug.Log("Upgrade M speed!");

        if (minionSpawnrateLevel < 3 && gameManager.gold > minionRateUpgradeCost[minionSpawnrateLevel])
        {
            gameManager.DeductGold(minionRateUpgradeCost[minionSpawnrateLevel]);
            minionSpawnrateLevel++;
        }

        uiManager.BuildBaseUI();
    }

    public void Reset()
    {
        minionLevel = 1;
        minionSpawnrateLevel = 0;

        health = 50;

        gateObjects[0].health = gateObjects[0].maxHealth;
        gateObjects[1].health = gateObjects[1].maxHealth;
        gateObjects[2].health = gateObjects[2].maxHealth;

        gateObjects[0].gameObject.SetActive(true);
        gateObjects[1].gameObject.SetActive(true);
        gateObjects[2].gameObject.SetActive(true);
    }
}
