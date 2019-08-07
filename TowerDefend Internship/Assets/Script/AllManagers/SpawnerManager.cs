using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour {

    public GameObject[] Alien;


 

    public static Vector3[] posSpawn;
    public static Quaternion[] RotationSpawn;
    // Use this for initialization

    public static bool isSpawn;
    public static int Waves = 1;

    public static int killCount;
    public static float delay = 0;
    // Update is called once per frame
    void Update () {

        if (GameManager.ins.CurrentGameState == GameManager.GameState.Ingame)
        {
          
            if (posSpawn.Length > 0 && RotationSpawn.Length > 0)
            {
                if(delay < 3)
                {
                    delay += Time.deltaTime;
                }
                if (isSpawn && delay > 3)
                {
                    for (int i = 0; i < Waves; i++)
                    {
                        StartCoroutine(spawnWithdelay(i + 1));

                    }

             
                    isSpawn = false;
                }

                if (killCount == Waves)
                {
                    Waves++;
                    killCount = 0;
                    isSpawn = true;
                }

                if (Waves <= 10)
                {
                    MainMenuManager.getObjUI.WavesText.text = "Waves : " + Waves;
                }
                else
                {
                    MainMenuManager.getObjUI.SetwinLoseConditon.Iswin = true;
                    for (int i = 0; i < MainMenuManager.getObjUI.SetwinLoseConditon.Light.Length; i++)
                    {
                        MainMenuManager.getObjUI.SetwinLoseConditon.Light[i].SetActive(true);
                    }
                    MainMenuManager.getObjUI.SetwinLoseConditon.Comenttext.text = "StageComplete !!";
                    MainMenuManager.getObjUI.ResulBox.SetActive(true);
                    MainMenuManager.getObjUI.DescriptionBox.SetActive(false);
                    MainMenuManager.getObjUI.ChooseBuild.SetActive(false);
                    StopAllCoroutines();
                    Time.timeScale = 0;
                }



            }
          




        }
    }

    IEnumerator spawnWithdelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        int index = Random.Range(0, posSpawn.Length);
        int randomvalue = Random.Range(0, Alien.Length);

        GameObject NewObj = Instantiate(Alien[randomvalue], posSpawn[index], RotationSpawn[index], gameObject.transform);
        NewObj.GetComponent<Enemies1>().initializedData();
        NewObj.GetComponent<Enemies1>().SetDesirePos(GameManager.ins.PathReader[index]);
    }
}
