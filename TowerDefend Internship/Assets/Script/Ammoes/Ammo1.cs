using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo1 : MonoBehaviour {
    [HideInInspector]
    public Transform target;
    float Speed = 5f;

    public float demage;
    public TowerDataBase.effect setEffect;

   
	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Speed);
        }
        else
        {
            transform.position += transform.forward * Speed;

        }

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Alien")
        {
            if(setEffect == TowerDataBase.effect.slow)
            {
                other.gameObject.GetComponent<Enemies1>().GetData.Isslowing = true;
            }

            if (other.gameObject.GetComponent<Enemies1>().GetData.Health > 0) {
                other.gameObject.GetComponent<Enemies1>().GetData.Health -= demage;
            }

            if (other.gameObject.GetComponent<Enemies1>().GetData.Health <= 0)
            {
                Destroy(other.gameObject);
                DataBaseManager.ins.Coin += 10;
                MainMenuManager.getObjUI.CoinText.text = "" + DataBaseManager.ins.Coin;
                SpawnerManager.killCount++;
            }

            Destroy(gameObject);
           
            
        }
    }
}
