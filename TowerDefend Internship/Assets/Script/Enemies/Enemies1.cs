using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies1 : MonoBehaviour {

   int CurrentWayPointID = 0;
  

   public float reachDistance;
    //[HideInInspector]
    //public PathFollowingReader path;

   Vector3[] DesirePos;
    

    [Range(1, 10)]
   public float RotationSpeed;

    public int IndexEnemies;

    public EnemiesData.EnemyAttribute Data;


    public class DataEnemy
    {
        public float Health, MoveSpeed, sloowingTime;
        public bool Isslowing;
    }


    public DataEnemy GetData = new DataEnemy();

 


    void Update () {

        if (GetData.Isslowing)
        {
            setSlow();
        }

        if (GetData.MoveSpeed != Data.MoveSpeed)
        {
            if (GetData.sloowingTime > 0)
            {
                GetData.sloowingTime -= Time.deltaTime;

            }
            else
            {
                GetData.sloowingTime = 0;
                GetData.MoveSpeed = Data.MoveSpeed;
            }
        }

        if (DesirePos != null)
        {
            if (CurrentWayPointID < DesirePos.Length)
            {
              
                float distace = Vector3.Distance(DesirePos[CurrentWayPointID], transform.position);
                transform.position = Vector3.MoveTowards(transform.position, DesirePos[CurrentWayPointID], Time.deltaTime * GetData.MoveSpeed);
                Vector3 rot = DesirePos[CurrentWayPointID] - transform.position;


                if (rot != Vector3.zero)
                {
                    Quaternion newrotation = Quaternion.LookRotation(rot);
                    transform.rotation = Quaternion.Slerp(transform.rotation, newrotation, Time.deltaTime * RotationSpeed);
                }

                if (distace <= reachDistance)
                {
                    CurrentWayPointID++;
                }
            }
            else
            {
                if (MainMenuManager.getObjUI.health > 0)
                {
                    MainMenuManager.getObjUI.health--;
                    MainMenuManager.getObjUI.HealthText.text = "" + MainMenuManager.getObjUI.health;
                    MainMenuManager.getObjUI.HealthImg.fillAmount = (float)MainMenuManager.getObjUI.health / 10;
                    Destroy(gameObject);
                    SpawnerManager.isSpawn = true;
                }
                if(MainMenuManager.getObjUI.health == 0)
                {    
                    MainMenuManager.getObjUI.SetwinLoseConditon.Iswin = false;
                    for(int i = 0; i < MainMenuManager.getObjUI.SetwinLoseConditon.Light.Length; i++)
                    {
                        MainMenuManager.getObjUI.SetwinLoseConditon.Light[i].SetActive(false);
                    }
                    MainMenuManager.getObjUI.SetwinLoseConditon.Comenttext.text = "You Lose !!";
                    MainMenuManager.getObjUI.ResulBox.SetActive(true);
                    MainMenuManager.getObjUI.ChooseBuild.SetActive(false);
                    Time.timeScale = 0;
                }
            }
        }

	}

    public void SetDesirePos(PathFollowingReader path)
    {
        DesirePos = new Vector3[path.Steps.Count -1];
        for (int i = 0; i < DesirePos.Length; i++)
        {
            DesirePos[i] = GameManager.ins.grassSpawner.Grass[path.Steps[i + 1].IndexPosZ][path.Steps[i + 1].IndexPosX].transform.position;
            if (Data.type == EnemiesData.Type.Walking)
            {
                DesirePos[i].y += 30f;
            }
            else
            {
                DesirePos[i].y += 130f;
               
            }

           
            
        }

        Vector3 pos = transform.position;
        pos.y = DesirePos[0].y;
        transform.position = pos;
    }

    public void setSlow()
    {
        GetData.sloowingTime = 5f;
        GetData.MoveSpeed = Data.MoveSpeed/2;
        GetData.Isslowing = false;
    }

    public void initializedData()
    {
        Data = DataBaseManager.ins.EnemyDB.ListEnemies[IndexEnemies];
        GetData.Health = Data.Health;
        GetData.MoveSpeed = Data.MoveSpeed;
    }


    
}
