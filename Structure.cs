using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Structure : MonoBehaviour
{
    public string structureName = "Struct";
    public Sprite structureIcon;

    protected GameManager gameManager;
    protected BaseManager baseManager;
    protected WaveManager waveManager;

    [HideInInspector]
    public Plot myPlot;

    public int tier = 1;
    public int level = 1;

    public int buildCost = 100;
    public int upgradeCost = 1000;
    public GameObject upgradePrefab;

    // Start is called before the first frame update
    public virtual void Start()
    {
        gameManager = GameManager._instance;
        baseManager = BaseManager._instance;
        waveManager = WaveManager._instance;
    }

    public Structure OnBuild(Plot parentPlot)
    {
        if (!GameManager._instance.DeductGold(buildCost))
        {
            Destroy(gameObject);
            return null;
        }

        myPlot = parentPlot;
        return this;
    }

    public bool OnRemove()
    {
        Destroy(gameObject);

        gameManager.AddGold((int)(buildCost * 0.6));
        return true;
    }    

    public Structure Upgrade()
    {
        print("Struct Up");

        if (gameManager.DeductGold(upgradeCost))
        {
            Structure newStruct = Instantiate(upgradePrefab, transform.position, transform.rotation).GetComponent<Structure>();
            myPlot.myStructure = newStruct;
            newStruct.myPlot = myPlot;
            Destroy(gameObject);

            return newStruct;
        }

        return null;
    }
}
