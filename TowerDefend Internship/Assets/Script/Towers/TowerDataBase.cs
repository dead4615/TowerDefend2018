using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDataBase {
    
    public enum effect
    {
        nothing,
        slow,
        burn
    }

    [System.Serializable]
    public class TowerAttribute
    {
        public string name;
        public string description;
        public float Demage;
        public float Range;
        public float AttackSpeed;
        public int Cost;
        public effect setEffect;
       

    }

    [System.Serializable]
    public class TowerUpgrade{
        public EnemiesData.Type enemyTarget;
        public TowerAttribute[] Upgrade;
        
    }

    [System.Serializable]
    public class Tower
    {
        public string towerName;
        public string TowerDescription;
        public TowerUpgrade TowerData;
    }


    [System.Serializable]
    public class Towers {
        public List<Tower> ListTowers = new List<Tower>();
    }
}
