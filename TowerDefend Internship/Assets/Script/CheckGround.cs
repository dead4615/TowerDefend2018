using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour {
    [HideInInspector]
    public bool isClipping;
    public RaycastHit hit;

   
	// Update is called once per frame
	void Update () {
        if (!isClipping) {
            if (Physics.Raycast(transform.position, -Vector3.up,out hit, 1000)) {
                if (!GameManager.ins.Isclipping)
                {
                    gameObject.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 10, hit.transform.position.z);
                    GameManager.ins.Isclipping = true;

                    PlantObj();
                    if (gameObject.GetComponent<FieldOfView>() != null)
                    {
                        gameObject.GetComponent<FieldOfView>().DrawAreaAttack();
                    }
                }
            }
        }
        
	}

    void PlantObj()
    {
        if (hit.transform.tag == "Grass")
        {
            Debug.DrawRay(transform.position, -Vector3.up * 200, Color.red);
         
            if (Input.GetMouseButtonDown(0))
            {
                if (gameObject.tag != GameManager.stringTag.BaseCamp.ToString())
                {
                    isClipping = true;
                    gameObject.transform.position = hit.transform.position;
                }
                else
                {
                    
                    int indexZ = 0;
                    int indexX = 0;
                    GameObject[] grasess = new GameObject[8];
                    for(int i = 0; i < GameManager.ins.grassSpawner.Grass.Count; i++)
                    {
                        for(int  j = 0; j < GameManager.ins.grassSpawner.Grass[i].Count; j++){
                            if(GameManager.ins.grassSpawner.Grass[i][j] == hit.transform.gameObject)
                            {
                                indexZ = i;
                                indexX = j;
                                break;
                            }
                        }
                    }

                    bool isAvailable = PlanIsAvailable(indexZ, indexX, grasess);

                

                    if (isAvailable)
                    {
                        isClipping = true;
                        for(int i = 0; i < grasess.Length; i++)
                        {
                            grasess[i].transform.tag = GameManager.stringTag.NullCamp.ToString();
                            grasess[i].SetActive(false);
                        }
                        gameObject.transform.position = hit.transform.position;
                    }
                }
            }
        }
    }

    bool PlanIsAvailable(int indexZ, int indexX, GameObject[] grasess)
    {
        bool isAvailable = true;
        if (indexZ + 1 < GameManager.ins.grassSpawner.Grass.Count)
        {
            if (GameManager.ins.grassSpawner.Grass[indexZ + 1][indexX].tag != GameManager.stringTag.Grass.ToString())
            {
                isAvailable = false;
            }
            else
            {
                grasess[0] = GameManager.ins.grassSpawner.Grass[indexZ + 1][indexX];
            }
        }
        else
        {

            isAvailable = false;
        }

        if (indexZ - 1 >= 0)
        {
            if (GameManager.ins.grassSpawner.Grass[indexZ - 1][indexX].tag != GameManager.stringTag.Grass.ToString())
            {
                isAvailable = false;
            }
            else
            {
                grasess[1] = GameManager.ins.grassSpawner.Grass[indexZ - 1][indexX];
            }
        }
        else
        {
            isAvailable = false;
        }

        if (indexX + 1 < GameManager.ins.grassSpawner.Grass[0].Count)
        {
            if (GameManager.ins.grassSpawner.Grass[indexZ][indexX + 1].tag != GameManager.stringTag.Grass.ToString())
            {
                isAvailable = false;
            }
            else
            {
                grasess[2] = GameManager.ins.grassSpawner.Grass[indexZ][indexX + 1];
            }
        }
        else
        {
            isAvailable = false;
        }

        if (indexX - 1 >= 0)
        {
            if (GameManager.ins.grassSpawner.Grass[indexZ][indexX - 1].tag != GameManager.stringTag.Grass.ToString())
            {
                isAvailable = false;
            }
            else
            {
                grasess[3] = GameManager.ins.grassSpawner.Grass[indexZ][indexX - 1];
            }
        }
        else
        {
            isAvailable = false;
        }

        if (indexX - 1 >= 0 && indexZ - 1 >= 0)
        {
            if (GameManager.ins.grassSpawner.Grass[indexZ - 1][indexX - 1].tag != GameManager.stringTag.Grass.ToString())
            {
                isAvailable = false;
            }
            else
            {
                grasess[4] = GameManager.ins.grassSpawner.Grass[indexZ - 1][indexX - 1];
            }
        }
        else
        {
            isAvailable = false;
        }

        if (indexX - 1 >= 0 && indexZ + 1 < GameManager.ins.grassSpawner.Grass.Count)
        {
            if (GameManager.ins.grassSpawner.Grass[indexZ + 1][indexX - 1].tag != GameManager.stringTag.Grass.ToString())
            {
                isAvailable = false;
            }
            else
            {
                grasess[5] = GameManager.ins.grassSpawner.Grass[indexZ + 1][indexX - 1];
            }
        }
        else
        {
            isAvailable = false;
        }

        if (indexX + 1 < GameManager.ins.grassSpawner.Grass[0].Count && indexZ + 1 < GameManager.ins.grassSpawner.Grass.Count)
        {
            if (GameManager.ins.grassSpawner.Grass[indexZ + 1][indexX + 1].tag != GameManager.stringTag.Grass.ToString())
            {
                isAvailable = false;
            }
            else
            {
                grasess[6] = GameManager.ins.grassSpawner.Grass[indexZ + 1][indexX + 1];
            }
        }
        else
        {
            isAvailable = false;
        }

        if (indexX + 1 < GameManager.ins.grassSpawner.Grass[0].Count && indexZ - 1 >= 0)
        {
            if (GameManager.ins.grassSpawner.Grass[indexZ - 1][indexX + 1].tag != GameManager.stringTag.Grass.ToString())
            {
                isAvailable = false;
            }
            else
            {
                grasess[7] = GameManager.ins.grassSpawner.Grass[indexZ - 1][indexX + 1];
            }
        }
        else
        {
            isAvailable = false;
        }

        return isAvailable;
    }

}
