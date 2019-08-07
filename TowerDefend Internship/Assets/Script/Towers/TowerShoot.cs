using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : MonoBehaviour {
    FieldOfView Fow;

    public GameObject headTower;
    Quaternion target;
    public int IndexTowerType, IndexLevel;

    public TowerDataBase.Tower TowerData;

    float delayattack = 2f;
    float time;


    public GameObject Ammo;
    public Transform shootSpawner;

   

    // Use this for initialization

    
    void Awake () {
        TowerData = DataBaseManager.ins.TowerDB.ListTowers[IndexTowerType];
        Fow = GetComponent<FieldOfView>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.GetComponent<CheckGround>().isClipping)
        {
            if (Fow.visibleTargets.Count > 0)
            {
                if (Fow.visibleTargets[0] != null)
                {
                    Vector3 difPos = Fow.visibleTargets[0].position - headTower.transform.position;
                    if (TowerData.TowerData.enemyTarget == EnemiesData.Type.Walking)
                    {
                        difPos.y += 80f;
                    }
                    target = Quaternion.LookRotation(difPos);

                    headTower.transform.rotation = Quaternion.Lerp(headTower.transform.rotation, target, 5f * Time.deltaTime);

                    time += (TowerData.TowerData.Upgrade[IndexLevel].AttackSpeed * Time.deltaTime);
                    if (time > delayattack)
                    {
                    
                        GameObject projectile = ShootEnemy(target);
                        projectile.GetComponent<Ammo1>().target = Fow.visibleTargets[0];
                        projectile.GetComponent<Ammo1>().demage = TowerData.TowerData.Upgrade[IndexLevel].Demage;
                        projectile.GetComponent<Ammo1>().setEffect = TowerData.TowerData.Upgrade[IndexLevel].setEffect;

                        projectile.transform.GetChild(0).GetComponent<MeshRenderer>().material = DataBaseManager.ins.MaterialAmmo[(int)TowerData.TowerData.Upgrade[IndexLevel].setEffect];
                        time = 0;
                    }
                }
               
                
            }
        }
	}


    GameObject ShootEnemy(Quaternion target)
    {
        
        return Instantiate(Ammo, shootSpawner.position, target, gameObject.transform);
    }
}
