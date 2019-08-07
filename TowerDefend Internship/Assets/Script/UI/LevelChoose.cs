using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelChoose : MonoBehaviour {

    public GameObject prefabs;
    public Transform parent;
    Vector3 spawnPos;

    Vector3 distance = new Vector3(0, -2.5f, -9.3f);
    public GameObject mainmenuManager;
    // Use this for initialization


    public bool isRespawn;
    public void callObj()
    {
        prefabs.GetComponent<Button>().onClick.RemoveAllListeners();
        prefabs.GetComponent<Button>().onClick.AddListener(delegate { mainmenuManager.GetComponent<MainMenuManager>().SelectLevel(prefabs.GetComponent<ButtonBlock>().index); });
        spawnButton();
    }
    public void spawnButton()
    {
        isRespawn = false;
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, 41.7f);
        for (int i = 1; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
        StartCoroutine(waitChildZero());
       
        
    }

    public IEnumerator InitializedObj()
    {
        while (!isRespawn)
        {
            yield return null;
        }

        for (int i = 1; i < parent.transform.childCount; i++)
        {
            GameObject br = parent.GetChild(i).gameObject;
            br.GetComponent<ButtonBlock>().display(i, "Level ");

            br.GetComponent<Button>().onClick.AddListener(delegate { mainmenuManager.GetComponent<MainMenuManager>().SelectLevel(br.GetComponent<ButtonBlock>().index); });
        }

        

    }


    IEnumerator waitChildZero()
    {
        
        while(parent.childCount > 1)
        {
            yield return null;
        }
        for (int i = 0; i < DataBaseManager.ins.MapsDB.MapDetail.Count - 1; i++)
        {
            parent.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, parent.GetComponent<RectTransform>().sizeDelta.y + 40f);
            Vector3 spawnPos = parent.transform.GetChild(i).position + distance;
             Instantiate(prefabs, spawnPos, parent.transform.GetChild(0).rotation, parent);
           
        }
        

        isRespawn = true;


    }

    public IEnumerator InitializedObjCreateObj()
    {
        while (!isRespawn)
        {
            yield return null;
        }

        prefabs.GetComponent<Button>().onClick.AddListener(delegate { mainmenuManager.GetComponent<MainMenuManager>().SetAddStage(prefabs.GetComponent<ButtonBlock>().index); });
        for (int i = 1; i < parent.transform.childCount; i++)
        {
            GameObject br = parent.GetChild(i).gameObject;
            br.GetComponent<ButtonBlock>().display(i, "Level ");
            br.GetComponent<Button>().onClick.AddListener(delegate { mainmenuManager.GetComponent<MainMenuManager>().SetAddStage(br.GetComponent<ButtonBlock>().index); });
        }
    }
}
