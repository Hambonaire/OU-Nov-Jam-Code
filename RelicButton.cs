using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelicButton : MonoBehaviour
{
    public Relic myRelic;
    public TextMeshProUGUI relicDescription1;
    public TextMeshProUGUI relicDescription2;
    public TextMeshProUGUI relicDescription3;
    public Image myIcon;
    public GameObject selectImage;

    [HideInInspector]
    int relicIndexInContent;

    bool isSelected = false;
    public bool inInv = false;

    public void Initialize(Relic newRelic, int index, bool inInv)
    {
        relicIndexInContent = index;
        this.inInv = inInv;

        myRelic = newRelic;
        myIcon.sprite = myRelic.relicIcon;
        relicDescription1.text = Relic.descriptions[myRelic.relicIndex].Split(' ')[0];

        string[] texts2 = Relic.descriptions[myRelic.relicIndex].Split(' ');
        string text2;
        text2 = texts2[1];
        if (texts2.Length > 2)
            text2 += " " +  texts2[2];
        relicDescription2.text = text2;

        if (myRelic.relicIndex == 5 || myRelic.relicIndex == 6)
            relicDescription3.text = "-";
        else
            relicDescription3.text = "+";

        relicDescription3.text += myRelic.relicValue.ToString("0.0") + "%";

    }

    public void Select()
    {
        if (!inInv)
            return;

        if (!isSelected)
        {
            if (!GameManager._instance.selectedInvRelicIndicies.Contains(relicIndexInContent))
                GameManager._instance.selectedInvRelicIndicies.Add(relicIndexInContent);
            selectImage.SetActive(true);
        }
        else
        {
            GameManager._instance.selectedInvRelicIndicies.Remove(relicIndexInContent);
            selectImage.SetActive(false);
        }

        isSelected = !isSelected;

    }
}
