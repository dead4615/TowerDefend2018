using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesData{
    public enum Type
    {
        Walking,
        Fly
    }

    [System.Serializable]
    public class EnemyAttribute
    {
        public string name;
        public string description;
        public float Health;
        public float MoveSpeed;
        public Type type;
    }

  


    [System.Serializable]
    public class Enemies
    {
        public List<EnemyAttribute> ListEnemies = new List<EnemyAttribute>();
    }

}
