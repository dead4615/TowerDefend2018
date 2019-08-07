using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {


    [System.Serializable]
    public class GrassSpawner {
       // public GameObject GrassPrefab, StonePrefab;
        public GameObject[] Prefabs;
        [HideInInspector]
        public List<List<GameObject>> Grass = new List<List<GameObject>>();
        public int curentLevel, currentStage;
        public Transform Parent;
        [HideInInspector]
        public Vector3 originalPos;
        public bool Inspawn;
    }

    public GrassSpawner grassSpawner;
    public static GameManager ins;


    [System.Serializable]
    public class ChooseObj {
        public GameObject[] ObjSpawns;
        public Outline[] ButtonOutline;
        [HideInInspector]
        public GameObject objSpwan;
        public Vector3 clipingpos;
        public Transform Parent;
    }


    public enum GameState
    {
        MainMenu,
        CreateMap,
        Ingame
    }

    /*public enum SpawnSerialization
    {
        Null,
        GrassWay,
        StoneWay,


    }*/

    public enum stringTag
    {
        Null = 0,
        Grass,
        Stone,
        HoleSpawner,
        BaseCamp,
        NullCamp,
        Tower,
        NotNull,
        NUM_STARTS
    }


    public ChooseObj setChoosing;
    [Range(0, 200)]
    public float centerPoin;

    [HideInInspector]
    public Ray ray;
    [HideInInspector]
    public RaycastHit hit;
    public bool Isclipping;


   
    public GameState CurrentGameState;
    public stringTag setTag;

    public List<PathFollowingReader> PathReader = new List<PathFollowingReader>();

    GameObject Tower;
   
    public class FitureScroll
    {
        public Vector3 beginPos, UpdatePos;
        public bool move;
    }

    public FitureScroll fitureScroll = new FitureScroll();
	// Use this for initialization
	void Start () {
        ins = this;
        
	}
	
	// Update is called once per frame
	void Update () {
       
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform != null)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        fitureScroll.beginPos = Input.mousePosition;
                        fitureScroll.move = true;
                    }

                    if (Input.GetMouseButton(1) && fitureScroll.move)
                    {
                        fitureScroll.UpdatePos = Input.mousePosition;
                    }
                    float deltaPOsX = fitureScroll.beginPos.x - fitureScroll.UpdatePos.x;
                    float deltaPOsY = fitureScroll.beginPos.y - fitureScroll.UpdatePos.y;
                    if (Camera.main.transform.position.z + deltaPOsY > -(float)grassSpawner.Grass.Count * 100 && Camera.main.transform.position.z + deltaPOsY < (float)grassSpawner.Grass.Count * 50
                        && Camera.main.transform.position.x + deltaPOsX > -(float)grassSpawner.Grass.Count * 50 && Camera.main.transform.position.x + deltaPOsX < (float)grassSpawner.Grass.Count * 50)
                    {
                        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + deltaPOsX / 2, Camera.main.transform.position.y, Camera.main.transform.position.z + (deltaPOsY / 2));
                    }
                    fitureScroll.beginPos = fitureScroll.UpdatePos;
                    if (Input.GetMouseButtonUp(1))
                    {
                        fitureScroll.beginPos = Vector3.zero;
                        fitureScroll.UpdatePos = Vector3.zero;
                        fitureScroll.move = false;
                    }
                }


            if (CurrentGameState == GameState.Ingame)
            {
                if (setChoosing.objSpwan == null)
                {
                    if (hit.transform.tag == "Tower")
                    {
                        if (hit.transform.GetComponentInParent<CheckGround>().isClipping)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                MainMenuManager.getObjUI.DescriptionBox.SetActive(true);
                                InputDataDescBox();
                            }
                        }

                    }
                }
                else
                {

                    if (setChoosing.objSpwan.GetComponent<CheckGround>().isClipping)
                    {
                        foreach (Outline button in setChoosing.ButtonOutline)
                        {
                            button.enabled = false;
                        }
                        setChoosing.objSpwan.GetComponent<CheckGround>().hit.transform.tag = stringTag.NotNull.ToString();

                        if (setChoosing.objSpwan.GetComponent<CheckGround>().isClipping)
                        {
                            StartCoroutine(setChoosing.objSpwan.GetComponent<FieldOfView>().NonActiveAttackArea(2f));
                        }
                        DataBaseManager.ins.Coin -= setChoosing.objSpwan.GetComponent<TowerShoot>().TowerData.TowerData.Upgrade[0].Cost;
                        MainMenuManager.getObjUI.CoinText.text = "" + DataBaseManager.ins.Coin;
                        setChoosing.objSpwan = null;
                      
                        return;
                    }
                    if (hit.point != setChoosing.clipingpos)
                    {
                        setChoosing.objSpwan.transform.position = new Vector3(hit.point.x, grassSpawner.originalPos.y + 10, hit.point.z);

                    }


                    if (Isclipping)
                    {
                        setChoosing.clipingpos = hit.point;
                        Isclipping = false;

                    }




                }
            }

        }
            else
            {
                fitureScroll.beginPos = Vector3.zero;
                fitureScroll.UpdatePos = Vector3.zero;
                fitureScroll.move = false;
            }
        


    }

    public void InputDataDescBox()
    {
        if(Tower != null)
        {
            if (Tower != hit.transform.parent.gameObject)
            {
                StartCoroutine(Tower.GetComponent<FieldOfView>().NonActiveAttackArea(0.2f));
            }
        }
        Tower = hit.transform.parent.gameObject;
        MainMenuManager.getObjUI.DestriptionTex.text =Tower.GetComponent<TowerShoot>().TowerData.TowerDescription;
        MainMenuManager.getObjUI.NameText.text = "Name :" + Tower.GetComponentInParent<TowerShoot>().TowerData.towerName;
        
        viewDescBox();
        if (Tower.GetComponent<TowerShoot>().IndexLevel < 2)
        {
            MainMenuManager.getObjUI.UpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Upgrade";
            MainMenuManager.getObjUI.UpgradeButton.GetComponent<Button>().interactable = true;
            
        }

       
        
    }

    public void SetGrassSpawner(int indexLenght, int indexWidth)
    {
        // Debug.Log(DataBaseManager.ins.DataXMLMap.dataLevel[grassSpawner.curentLevel].dataStage[grassSpawner.currentStage].detailMapsInt[0].Count);
        grassSpawner.Grass = new List<List<GameObject>>();
        PathReader = new List<PathFollowingReader>();
        grassSpawner.originalPos = grassSpawner.Parent.position;
        for (int i = 0; i < indexLenght; i++) {
            grassSpawner.Grass.Add(new List<GameObject>());
            for(int j = 0; j < indexWidth; j++)
            {
                for (int k = 0; k < (int)stringTag.NUM_STARTS; k++)
                {
                    setTag = (stringTag) k;
                    if (DataBaseManager.ins.DataXMLMap.dataLevel[grassSpawner.curentLevel].dataStage[grassSpawner.currentStage].detailMapsInt[i][j] == (int)setTag)
                    {        
                        SpawnObj(grassSpawner.Prefabs[k-1], i, setTag.ToString());
                        if(setTag == stringTag.HoleSpawner)
                        {
                            PathFollowingReader pathFollowing = new PathFollowingReader();
                            pathFollowing.HolePos.IndexPosX = j;
                            pathFollowing.HolePos.IndexPosZ = i;

                            PathReader.Add(pathFollowing);
                           
                        }
                    }
                }
              

                grassSpawner.originalPos.x += 102;
            }

            grassSpawner.originalPos.x = grassSpawner.Parent.position.x;
            grassSpawner.originalPos.z -= 102f;
        }

        for (int i = 0; i < PathReader.Count; i++)
        {
            PathReader[i].SetFindObj();
            
        }

        SpawnerManager.posSpawn = new Vector3[PathReader.Count];
        SpawnerManager.RotationSpawn = new Quaternion[PathReader.Count];

        
        for(int i = 0; i < PathReader.Count; i++)
        {
            SpawnerManager.posSpawn[i] = grassSpawner.Grass[PathReader[i].HolePos.IndexPosZ][PathReader[i].HolePos.IndexPosX].transform.position;
            SpawnerManager.posSpawn[i].y += 30f;

            Vector3 DesiredRot = Vector3.zero;
            
            if (PathReader[i].Steps.Count > 0) {
                DesiredRot = grassSpawner.Grass[PathReader[i].Steps[1].IndexPosZ][PathReader[i].Steps[1].IndexPosX].transform.position;
            }
            DesiredRot.y = SpawnerManager.posSpawn[i].y;
            SpawnerManager.RotationSpawn[i] = Quaternion.LookRotation(DesiredRot - SpawnerManager.posSpawn[i]);
        }
        
    }


    public void CreateMap(int indexLenght, int indexWidth)
    {
        // Debug.Log(DataBaseManager.ins.DataXMLMap.dataLevel[grassSpawner.curentLevel].dataStage[grassSpawner.currentStage].detailMapsInt[0].Count);
        grassSpawner.Grass = new List<List<GameObject>>();
        grassSpawner.originalPos = grassSpawner.Parent.position;
        for (int i = 0; i < indexLenght; i++)
        {
            grassSpawner.Grass.Add(new List<GameObject>());
            for (int j = 0; j < indexWidth; j++)
            {
               
                    setTag = stringTag.Grass;
                    SpawnObj(grassSpawner.Prefabs[0], i, setTag.ToString());
                    grassSpawner.originalPos.x += 102;
            }

            grassSpawner.originalPos.x = grassSpawner.Parent.position.x;
            grassSpawner.originalPos.z -= 102f;
        }


    }


    void SpawnObj(GameObject objSpawn, int index, string tag)
    {
        GameObject newObj = Instantiate(objSpawn, grassSpawner.originalPos, grassSpawner.Parent.rotation, grassSpawner.Parent);
        newObj.tag = tag;
        grassSpawner.Grass[index].Add(newObj);


    }

    public void DestroyMap()
    {
        foreach(Transform Obj in grassSpawner.Parent)
        {
            Destroy(Obj.gameObject);
           
        }
        
    }

    public void Tower1(int index) {

        if (DataBaseManager.ins.Coin >= DataBaseManager.ins.TowerDB.ListTowers[index].TowerData.Upgrade[0].Cost) {

            if (setChoosing.objSpwan != null)
            {
                Destroy(setChoosing.objSpwan);
            }

            for (int i = 0; i < setChoosing.ButtonOutline.Length; i++)
            {
                if (i == index) {
                    setChoosing.ButtonOutline[i].enabled = true;
                }
                else
                {
                    setChoosing.ButtonOutline[i].enabled = false;
                }


            }


            setChoosing.objSpwan = Instantiate(setChoosing.ObjSpawns[index], Input.mousePosition, transform.rotation, setChoosing.Parent);
            MainMenuManager.getObjUI.DescriptionBox.SetActive(false);
        }
    }

    public void UpgradeButton()
    {
        if (Tower != null) {
            if (DataBaseManager.ins.Coin >= Tower.GetComponent<TowerShoot>().TowerData.TowerData.Upgrade[Tower.GetComponent<TowerShoot>().IndexLevel].Cost)
            {
                if (Tower.GetComponent<TowerShoot>().IndexLevel < 2)
                {
                    DataBaseManager.ins.Coin -= Tower.GetComponent<TowerShoot>().TowerData.TowerData.Upgrade[Tower.GetComponent<TowerShoot>().IndexLevel].Cost;
                    MainMenuManager.getObjUI.CoinText.text = "" + DataBaseManager.ins.Coin;
                    Tower.GetComponent<TowerShoot>().IndexLevel++;
                   
                }
                if (Tower.GetComponent<TowerShoot>().IndexLevel == 2)
                {
                    MainMenuManager.getObjUI.UpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "MaxLevel";
                    MainMenuManager.getObjUI.UpgradeButton.GetComponent<Button>().interactable = false;
                }
               
            }
           
            viewDescBox();
        }
    }

   void viewDescBox() {
        int level = Tower.GetComponent<TowerShoot>().IndexLevel;
        Tower.GetComponent<FieldOfView>().viewRadius = Tower.GetComponent<TowerShoot>().TowerData.TowerData.Upgrade[level].Range;
        MainMenuManager.getObjUI.LevelText.text = "Level " + (level+1);
        MainMenuManager.getObjUI.DemageText.text = "Demage : " + Tower.GetComponent<TowerShoot>().TowerData.TowerData.Upgrade[level].Demage;
        MainMenuManager.getObjUI.RangeText.text = "Range : " + Tower.GetComponent<TowerShoot>().TowerData.TowerData.Upgrade[level].Range;
        MainMenuManager.getObjUI.AttackSpeedText.text = "Attack Speed : " + Tower.GetComponent<TowerShoot>().TowerData.TowerData.Upgrade[level].AttackSpeed;
        MainMenuManager.getObjUI.EffectText.text = "Effect : " + Tower.GetComponent<TowerShoot>().TowerData.TowerData.Upgrade[level].setEffect.ToString();
        MainMenuManager.getObjUI.CostText.text = "" + Tower.GetComponent<TowerShoot>().TowerData.TowerData.Upgrade[level].Cost;
        Tower.GetComponent<FieldOfView>().DrawAreaAttack();
        //StartCoroutine(Tower.GetComponent<FieldOfView>().NonActiveAttackArea(2f));
    }

    public void QuitDesc()
    {
        MainMenuManager.getObjUI.DescriptionBox.SetActive(false);
        StartCoroutine(Tower.GetComponent<FieldOfView>().NonActiveAttackArea(0.2f));
    }

}
