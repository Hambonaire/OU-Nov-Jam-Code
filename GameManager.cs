using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    WaveManager waveManager;
    BaseManager baseManager;
    UIManager uiManager;

    public bool x2Speed = true;

    public Sprite[] relicIcons = new Sprite[13];

    [Header("Inheritance")]
    public int inheritanceIteration = 0;
    public int inheritanceGold = 0;
    public List<Relic> inheritanceRelics = new List<Relic>();

    [Header("Inventory")]
    public int gold = 100;
    public List<Relic> inventoryRelics = new List<Relic>();

    public List<int> selectedInvRelicIndicies = new List<int>();

    // Relic stats
    public float[] cumulativeRelics = new float[13];

    public void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    public void Start()
    {
        waveManager = WaveManager._instance;
        baseManager = BaseManager._instance;
        uiManager = UIManager._instance;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Time.timeScale > 0)
        {
            if (x2Speed)
                Time.timeScale = 3.0f;
            else
                Time.timeScale = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //AddRelicToInventory(new Relic());
        }

        if (baseManager.health <= 0)
        {
            Reset();
        }
    }

    public void IterateInheritanceCycle()
    {
        inheritanceIteration++;
    }

    public void AddRelicToInventory(Relic newRelic)
    {
        inventoryRelics.Add(newRelic);

        CheckRelics();

        if (uiManager.baseUI.activeInHierarchy)
            uiManager.BuildBaseUI(false);
    }

    public void MoveResourcesToInheritance()
    {
        MoveRelicToInheritance();
        AddGoldToInheritance();

        uiManager.BuildBaseUI();
    }

    void MoveRelicToInheritance()
    {
        selectedInvRelicIndicies.Sort((a, b) => b.CompareTo(a));
        
        foreach(int index in selectedInvRelicIndicies)
        {
            Relic newRelic = inventoryRelics[index];
            inventoryRelics.RemoveAt(index);
            inheritanceRelics.Add(newRelic);
        }

        CheckRelics();

        selectedInvRelicIndicies.Clear();
    
    }

    void AddGoldToInheritance()
    {
        DeductGold(uiManager.goldToMoveInherit);
        inheritanceGold += uiManager.goldToMoveInherit;
    }

    void CheckRelics()
    {
        for (int i = 0; i < cumulativeRelics.Length; i++)
            cumulativeRelics[i] = 0;

        foreach(Relic relic in inventoryRelics)
            cumulativeRelics[relic.relicIndex] += relic.relicValue;
    }

    public bool DeductGold(int _gold)
    {
        if (gold >= _gold)
        {
            gold -= _gold;
            print("deducted " + _gold);
            return true;
        }

        return false;
    }

    public void AddGold(int gold)
    {
        this.gold += gold;
    }

    public void Reset()
    {
        IterateInheritanceCycle();

        gold = 0;
        inventoryRelics.Clear();
        selectedInvRelicIndicies.Clear();
        cumulativeRelics = new float[13];

        gold = inheritanceGold;
        inheritanceGold = 0;
        
        inventoryRelics = inheritanceRelics;
        inheritanceRelics.Clear();

        CheckRelics();

        waveManager.Reset();
        baseManager.Reset();
        uiManager.CloseUI();
    }

    public void Pause(bool val)
    {
        if (val)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
