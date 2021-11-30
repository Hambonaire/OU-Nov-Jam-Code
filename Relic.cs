using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic
{
    /*
     * 0 - killGoldBonus = 0.0f;
     * 1 - baseMinionSpawnRateBonus = 0.0f;
     * 2 - towerDamageBonus = 0.0f;
     * 3 - towerFireRateBonus = 0.0f;
     * 4 - towerRangeBonus = 0.0f;
     * 5 - towerBuyReduction = 0.0f;
     * 6 - towerUpgradeReduction = 0.0f;
     * 7 - towerSellBonus = 0.0f;
     * 8 - minionDamageBonus = 0.0f;
     * 9 - minionHealthBonus = 0.0f;
     * 10 - minionAttackRateBonus = 0.0f;
     * 11 - minionRangeBonus = 0.0f;
     * 12 - minionHealthRegenBonus = 0.0f;
     */

    [Header("Base")]
    public static float killGoldBonus               = 0;
    public static float baseMinionSpawnRateBonus    = 1;
    [Header("Tower")]
    public static float towerDamageBonus            = 2;
    public static float towerFireRateBonus          = 3;
    public static float towerRangeBonus             = 4;
    public static float towerBuyReduction           = 5;
    public static float towerUpgradeReduction       = 6;
    public static float towerSellBonus              = 7;
    [Header("Minion")]
    public static float minionDamageBonus           = 8;
    public static float minionHealthBonus           = 9;
    public static float minionAttackRateBonus       = 10;
    public static float minionRangeBonus            = 11;
    public static float minionHealthRegenBonus      = 12;

    public static string[] descriptions =
    {
        "Bonus Gold",
        "Minion Spawn",
        "Tower Damage",
        "Tower Atk Spd",
        "Tower Range",
        "Tower Cost",
        "Tower Upgrade",
        "Tower Sale",
        "Troop Damage",
        "Troop Health",
        "Troop Atk Spd",
        "Troop Range",
        "Troop Regen"
    };

    //public float[] relicValues = new float[13];
    public float relicValue = 0f;
    public int relicIndex = -1;

    GameManager gameManager;
    WaveManager waveManager;

    public Sprite relicIcon;

    public Relic()
    {
        gameManager = GameManager._instance;
        waveManager = WaveManager._instance;

        // Generate this relic
        relicIndex = UnityEngine.Random.Range(0, 13);

        relicValue = (float)Math.Round(UnityEngine.Random.Range(waveManager.currentWave / 5f, waveManager.currentWave / 2f), 1);
        relicValue = Mathf.Clamp(relicValue, 1, 50);

        //relicValues[relicIndex] = relicValue;

        relicIcon = gameManager.relicIcons[relicIndex];
    }
}
