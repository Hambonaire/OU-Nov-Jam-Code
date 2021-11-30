using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    GameManager gameManager;
    BaseManager baseManager;

    public Structure myStructure;

    [HideInInspector]
    public GameObject flagObj;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager._instance;
        baseManager = BaseManager._instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Structure BuildStructure(GameObject newStructure)
    {
        myStructure = Instantiate(newStructure, transform.position, transform.rotation).GetComponent<Structure>();

        var val = myStructure.OnBuild(this);

        flagObj.SetActive(!val);

        return val;
    }

    public bool RemoveStructure()
    {
        if (myStructure.gameObject != null)
        {
            Destroy(myStructure.gameObject);

            flagObj.SetActive(false);

            return true;
        }

        return false;
    }

}
