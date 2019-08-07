using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour {

    [System.Serializable]
    public class MainMenu
    {
        public GameObject Play, CreateMap, Container;
    }

    [System.Serializable]
    public class GamePlay
    {
        public GameObject Container;
        public Image HealthImg;
        public int health;
        public GameObject DescriptionBox, UpgradeButton, ResulBox, ChooseBuild;
        public Text NameText, DestriptionTex, DemageText, RangeText, AttackSpeedText, EffectText, LevelText, HealthText, CoinText, WavesText, CostText;

        public WinLoseConditon SetwinLoseConditon;

        public Text[] TowerCost;
    }

    [System.Serializable]
    public class CreateMapMenu {
        public GameObject ChooseLevelContainer, AddLevelorStage, CreatingBar, Container, PanelCreateBar, Comment;
        public InputField WitdhText, LenghtText;
        public Level NewLevel = new Level();
        public bool isAddLevel;
        public int Width, Lenght;
        public int selectedLevel;
    }

    [System.Serializable]
    public class PlayMenu
    {
        public GameObject Container, ScroolLevelContainer, ScroolStageContainer;
        public int IndexLevel, IndexStage;
    }
    [System.Serializable]
    public class QuitGame
    {
        public GameObject Container;
    }

    [System.Serializable]
    public class WinLoseConditon
    {
        public bool Iswin;
        public GameObject[] Light;
        public Text Comenttext;
    }


    public MainMenu mainMenu;
    public CreateMapMenu createMapMenu;
    public PlayMenu playMenu;
    public GamePlay gamePlay;
    public static GamePlay getObjUI;
    public QuitGame quit, ExittoMainMenu;
    public GameManager.ChooseObj ChooseObjtoCreate;

    public Transform alienParen;
    public GameObject Ground;
    Vector3 beginCam;

    public GameObject spawnManager;
  
	// Use this for initialization
	void Start () {
       
        getObjUI = gamePlay;
        beginCam = Camera.main.transform.position;
        for(int i = 0; i < DataBaseManager.ins.TowerDB.ListTowers.Count; i++)
        {
            gamePlay.TowerCost[i].text = "" + DataBaseManager.ins.TowerDB.ListTowers[i].TowerData.Upgrade[0].Cost;
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (ChooseObjtoCreate.objSpwan != null)
        {

           GameManager.ins.ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(GameManager.ins.ray, out GameManager.ins.hit, Mathf.Infinity))
            {
                if (ChooseObjtoCreate.objSpwan.GetComponent<CheckGround>().isClipping)
                {
                    foreach (Outline button in ChooseObjtoCreate.ButtonOutline)
                    {
                        button.enabled = false;
                    }
                    Destroy(ChooseObjtoCreate.objSpwan.GetComponent<CheckGround>());
                    ChooseObjtoCreate.objSpwan.GetComponent<CheckGround>().hit.transform.tag = GameManager.ins.setTag.ToString();
                    ChooseObjtoCreate.objSpwan.GetComponent<CheckGround>().hit.transform.gameObject.SetActive(false);
                    ChooseObjtoCreate.objSpwan = null;
                    return;
                }
                if (GameManager.ins.hit.point != ChooseObjtoCreate.clipingpos)
                {
                    ChooseObjtoCreate.objSpwan.transform.position = new Vector3(GameManager.ins.hit.point.x, GameManager.ins.grassSpawner.originalPos.y + 10, GameManager.ins.hit.point.z);
                    
                }


                if (GameManager.ins.Isclipping)
                {
                   ChooseObjtoCreate.clipingpos = GameManager.ins.hit.point;
                    GameManager.ins.Isclipping = false;

                }


            }

        }

        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (playMenu.Container.activeInHierarchy)
            {
                if (playMenu.ScroolLevelContainer.activeInHierarchy)
                {
                    mainMenu.CreateMap.SetActive(true);
                    playMenu.Container.SetActive(false);
                    playMenu.ScroolLevelContainer.SetActive(false);
                }
                else
                {
                    playMenu.ScroolLevelContainer.SetActive(true);
                    playMenu.ScroolStageContainer.SetActive(false);
                }
            }else if (createMapMenu.Container.activeInHierarchy)
            {
                if (createMapMenu.AddLevelorStage.activeInHierarchy)
                {
                    createMapMenu.AddLevelorStage.SetActive(false);
                    mainMenu.Container.SetActive(true);
                    createMapMenu.Container.SetActive(false);
                }else if (createMapMenu.CreatingBar.activeInHierarchy)
                {

                    if (createMapMenu.isAddLevel)
                    {
                        createMapMenu.AddLevelorStage.SetActive(true);
                        createMapMenu.CreatingBar.SetActive(false);
                    }
                    else
                    {
                        createMapMenu.CreatingBar.SetActive(false);
                        createMapMenu.ChooseLevelContainer.SetActive(true);
                    }
                }else if (createMapMenu.PanelCreateBar.activeInHierarchy)
                {
                    GameManager.ins.DestroyMap();
                    createMapMenu.CreatingBar.SetActive(true);
                    createMapMenu.PanelCreateBar.SetActive(false);
                }
                else if (createMapMenu.ChooseLevelContainer.activeInHierarchy)
                {
                    
                    createMapMenu.ChooseLevelContainer.SetActive(false);
                    createMapMenu.AddLevelorStage.SetActive(true);


                }
            }
            else if (mainMenu.Container.activeInHierarchy)
            {

                quit.Container.SetActive(true);
            }
            else if (gamePlay.Container.activeInHierarchy)
            {
                ExittoMainMenu.Container.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    #region QuiitButton
    public void YesQuit()
    {
        if (GameManager.ins.CurrentGameState == GameManager.GameState.MainMenu)
        {
            Application.Quit();
        }
        else{

            mainMenu.Container.SetActive(true);
            GameManager.ins.DestroyMap();
            foreach(Transform obj in GameManager.ins.setChoosing.Parent)
            {
                Destroy(obj.gameObject);
            }
            foreach(Transform obj in alienParen)
            {
                Destroy(obj.gameObject);
            }
            gamePlay.DescriptionBox.SetActive(false);
            gamePlay.Container.SetActive(false);
            GameManager.ins.CurrentGameState = GameManager.GameState.MainMenu;
            ExittoMainMenu.Container.SetActive(false);
            mainMenu.CreateMap.SetActive(true);
            Ground.transform.localScale = new Vector3(110, 110, 110);
            StopAllCoroutines();
           
            GameManager.ins.grassSpawner.Grass = new List<List<GameObject>>();
            Camera.main.transform.position = beginCam;
            Time.timeScale = 1;
            spawnManager.GetComponent<SpawnerManager>().StopAllCoroutines();
        }
    }

    public void NotQuit()
    {
        if (GameManager.ins.CurrentGameState == GameManager.GameState.MainMenu)
        {
            quit.Container.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            ExittoMainMenu.Container.SetActive(false);
            
        }
    }
    #endregion

    #region ResultButton
    public void mainmenuButton()
    {
        YesQuit();
  
        getObjUI.ResulBox.SetActive(false);
        getObjUI.ChooseBuild.SetActive(true);
       
    }

    public void retryButton()
    {
        GameManager.ins.DestroyMap();
        foreach (Transform obj in GameManager.ins.setChoosing.Parent)
        {
            Destroy(obj.gameObject);
        }
        foreach (Transform obj in alienParen)
        {
            Destroy(obj.gameObject);
        }
  
        mainMenu.CreateMap.SetActive(true);
        Ground.transform.localScale = new Vector3(110, 110, 110);
        StopAllCoroutines();
        GameManager.ins.grassSpawner.Grass = new List<List<GameObject>>();
        Camera.main.transform.position = beginCam;
        callInsideGame();
        getObjUI.ResulBox.SetActive(false);
        getObjUI.ChooseBuild.SetActive(true);
        Time.timeScale = 1;
    }


   #endregion


    #region MainMenuButton
    public void PlayButton()
    {
        mainMenu.CreateMap.SetActive(false);
        playMenu.Container.SetActive(true);
        playMenu.ScroolLevelContainer.SetActive(true);
        playMenu.Container.GetComponent<LevelChoose>().callObj();
        StartCoroutine(playMenu.Container.GetComponent<LevelChoose>().InitializedObj());
    }


    public void setCreateMap()
    {
        mainMenu.Container.SetActive(false);
        createMapMenu.Container.SetActive(true);
        createMapMenu.AddLevelorStage.SetActive(true);
    }



    #endregion

    #region PlayMenuButton
    public void SelectLevel(int index)
    {
        playMenu.ScroolLevelContainer.SetActive(false);
        playMenu.ScroolStageContainer.SetActive(true);
        playMenu.IndexLevel = index;
        playMenu.Container.GetComponent<ChooseStage>().spawnButton();

       
    }

    public void SelectStage(int index)
    {
        playMenu.ScroolStageContainer.SetActive(false);
        mainMenu.Container.SetActive(false);
        playMenu.Container.SetActive(false);
        gamePlay.Container.SetActive(true);
        playMenu.IndexStage = index;
        callInsideGame();
    }

    void callInsideGame()
    {
        int lenght = DataBaseManager.ins.DataXMLMap.dataLevel[playMenu.IndexLevel].dataStage[playMenu.IndexStage].Lenghtstage;
        int width = DataBaseManager.ins.DataXMLMap.dataLevel[playMenu.IndexLevel].dataStage[playMenu.IndexStage].Widthstage;
        GameManager.ins.grassSpawner.curentLevel = playMenu.IndexLevel;
        GameManager.ins.grassSpawner.currentStage = playMenu.IndexStage;
        GameManager.ins.SetGrassSpawner(lenght, width);
        if (lenght > 9 || width > 9)
        {
            Ground.transform.localScale = new Vector3(12.2f * lenght, 1, 12.2f * width);
        }
        GameManager.ins.CurrentGameState = GameManager.GameState.Ingame;
        DataBaseManager.ins.Coin = 15;
       
        getObjUI.HealthImg.fillAmount = 1;
        SpawnerManager.Waves = 1;
        getObjUI.WavesText.text = "Waves : " + 1;
        getObjUI.HealthText.text = "10";
        getObjUI.health = 10;
        getObjUI.CoinText.text = "" +    DataBaseManager.ins.Coin;
        SpawnerManager.killCount = 0;
        SpawnerManager.isSpawn = true;
        SpawnerManager.delay = 0;
        

    }
    #endregion

    #region CreateMapButton

    public void AddLevel()
    {
        createMapMenu.LenghtText.text = "";
        createMapMenu.WitdhText.text = "";
        createMapMenu.AddLevelorStage.SetActive(false);
        createMapMenu.CreatingBar.SetActive(true);
        createMapMenu.NewLevel = new Level();
        createMapMenu.NewLevel.LevelDesign.Add(new Stage());
       
        createMapMenu.isAddLevel = true;
    }

    public void AddStage()
    {
        createMapMenu.AddLevelorStage.SetActive(false);
        createMapMenu.ChooseLevelContainer.SetActive(true);
        createMapMenu.isAddLevel = false;
        createMapMenu.ChooseLevelContainer.GetComponent<LevelChoose>().spawnButton();
        StartCoroutine(createMapMenu.ChooseLevelContainer.GetComponent<LevelChoose>().InitializedObjCreateObj());
    }

    public void SetAddStage(int index)
    {
        createMapMenu.LenghtText.text = "";
        createMapMenu.WitdhText.text = "";
        createMapMenu.ChooseLevelContainer.SetActive(false);
        createMapMenu.selectedLevel = index;
        createMapMenu.NewLevel = new Level();
        createMapMenu.NewLevel.LevelDesign.Add(new Stage());
        createMapMenu.CreatingBar.SetActive(true);
        
    }

    public void SubmitDimension()
    {

        int.TryParse(createMapMenu.LenghtText.text, out createMapMenu.Lenght);
        int.TryParse(createMapMenu.WitdhText.text, out createMapMenu.Width);
        createMapMenu.CreatingBar.SetActive(false);
        createMapMenu.PanelCreateBar.SetActive(true);
        GameManager.ins.CreateMap(createMapMenu.Lenght, createMapMenu.Width);
        if (createMapMenu.Lenght > 9 || createMapMenu.Width > 9)
        {
            Ground.transform.localScale = new Vector3(12.2f * createMapMenu.Width, 1, 12.2f * createMapMenu.Lenght);
        }
    }

    

    public void refreshButton()
    {
        GameManager.ins.DestroyMap();

        StartCoroutine(WaitAllObjectDestroy());
    }

    public void SubmitButton()
    {
        translateMaptoXML();
        createMapMenu.PanelCreateBar.SetActive(false);
        createMapMenu.Container.SetActive(false);
        StartCoroutine(waitComment(2f));
    }

    IEnumerator waitComment(float delay)
    {

        if (createMapMenu.isAddLevel)
        {
            createMapMenu.Comment.transform.GetChild(0).GetComponent<Text>().text = "Map has created at Level " + DataBaseManager.ins.MapsDB.MapDetail.Count + " Stage 1";
        }
        else {
            createMapMenu.Comment.transform.GetChild(0).GetComponent<Text>().text = "Map has created at Level " + (createMapMenu.selectedLevel+1) + " Stage " + DataBaseManager.ins.MapsDB.MapDetail[createMapMenu.selectedLevel].LevelDesign.Count;
        }

        GameManager.ins.DestroyMap();
        Ground.transform.localScale = new Vector3(110, 110, 110);
        createMapMenu.Comment.SetActive(true);
        yield return new WaitForSeconds(delay);

        createMapMenu.Comment.SetActive(false);
        mainMenu.Container.SetActive(true);


    }

    void translateMaptoXML()
    {
       
        for (int i = 0; i < createMapMenu.Lenght; i++)
        {
            createMapMenu.NewLevel.LevelDesign[0].stageMapDetail += '(';
            for (int j = 0; j < createMapMenu.Width; j++)
            {
                for (int k = 0; k < (int)GameManager.stringTag.NUM_STARTS; k++)
                {
                    GameManager.ins.setTag = (GameManager.stringTag) k;
                    if (GameManager.ins.grassSpawner.Grass[i][j].transform.tag ==  GameManager.ins.setTag.ToString())
                    {
                        createMapMenu.NewLevel.LevelDesign[0].stageMapDetail += k;
                    }
                    
                }

                if(j < createMapMenu.Width - 1)
                {
                    createMapMenu.NewLevel.LevelDesign[0].stageMapDetail += ',';
                }
                else
                {
                    createMapMenu.NewLevel.LevelDesign[0].stageMapDetail += ')';
                }

            }
        }
        if (createMapMenu.isAddLevel) {
            createMapMenu.NewLevel.name = "Level" + (DataBaseManager.ins.MapsDB.MapDetail.Count + 1);
            createMapMenu.NewLevel.LevelDesign[0].name = "Stage" + 0;
            DataBaseManager.ins.MapsDB.MapDetail.Add(createMapMenu.NewLevel);
            DataBaseManager.ins.SaveDataMapsXmL();
            DataBaseManager.ins.TranslateAllDataMap();
        }
        else
        {
            createMapMenu.NewLevel.name = "Level" + createMapMenu.selectedLevel;
            createMapMenu.NewLevel.LevelDesign[0].name = "Stage" + (DataBaseManager.ins.MapsDB.MapDetail[createMapMenu.selectedLevel].LevelDesign.Count);
            DataBaseManager.ins.MapsDB.MapDetail[createMapMenu.selectedLevel].LevelDesign.Add(createMapMenu.NewLevel.LevelDesign[0]);
            DataBaseManager.ins.SaveDataMapsXmL();
            DataBaseManager.ins.TranslateAllDataMap();
        }
    }

    IEnumerator WaitAllObjectDestroy()
    {
        while (GameManager.ins.grassSpawner.Parent.transform.childCount > 0)
        {
            yield return null;

        }
        GameManager.ins.grassSpawner.Grass = new List<List<GameObject>>();
        GameManager.ins.CreateMap(createMapMenu.Lenght, createMapMenu.Width);
    }

    public void ButtonChoose1(int index)
    {

        if (ChooseObjtoCreate.objSpwan != null)
        {
            Destroy(ChooseObjtoCreate.objSpwan);
        }
        for (int i = 0; i < ChooseObjtoCreate.ButtonOutline.Length; i++)
        {
            if (i == index)
            {
                ChooseObjtoCreate.ButtonOutline[i].enabled = true;
            }
            else
            {
                ChooseObjtoCreate.ButtonOutline[i].enabled = false;
            }


        }

        if (index != 0)
        {
            
            ChooseObjtoCreate.objSpwan = Instantiate(ChooseObjtoCreate.ObjSpawns[index - 1], Input.mousePosition, transform.rotation, GameManager.ins.grassSpawner.Parent);
            ChooseObjtoCreate.objSpwan.AddComponent<CheckGround>();
            GameManager.ins.setTag = (GameManager.stringTag) index + 1;
        }
    }

    #endregion
}
