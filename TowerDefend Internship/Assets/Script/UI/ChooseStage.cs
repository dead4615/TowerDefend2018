using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChooseStage : MonoBehaviour {

    public GameObject prefabs;
    public Transform parent;
    Vector3 spawnPos;

    Vector3 distance = new Vector3(0, -2.5f, -9.3f);
    public GameObject mainmenuManager;

    int Level;
    // Use this for initialization
    void Start()
    {
        
        prefabs.GetComponent<Button>().onClick.AddListener(delegate { mainmenuManager.GetComponent<MainMenuManager>().SelectStage(prefabs.GetComponent<ButtonBlock>().index); });
       
    }



    public void spawnButton()
    {
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, 41.7f);
        for (int i = 1; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }

        StartCoroutine(waitChildZero());


    }

    IEnumerator waitChildZero()
    {
        while(parent.childCount > 1)
        {
            yield return null;
        }

        Level = mainmenuManager.GetComponent<MainMenuManager>().playMenu.IndexLevel;

        for (int i = 0; i < DataBaseManager.ins.MapsDB.MapDetail[Level].LevelDesign.Count - 1; i++)
        {

            parent.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, parent.GetComponent<RectTransform>().sizeDelta.y + 40f);
            Vector3 spawnPos = parent.transform.GetChild(i).position + distance;
            GameObject newobj = Instantiate(prefabs, spawnPos, parent.transform.GetChild(0).rotation, parent);
            newobj.GetComponent<ButtonBlock>().display(i + 1, "Stage ");
            newobj.GetComponent<Button>().onClick.AddListener(delegate { mainmenuManager.GetComponent<MainMenuManager>().SelectStage(newobj.GetComponent<ButtonBlock>().index); });
        }

    }
}
