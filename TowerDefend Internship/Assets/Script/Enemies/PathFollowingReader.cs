using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowingReader {
    
    //List<List<int>> step = new List<List<int>>();

    public class BeginPos
    {
        public int IndexPosX, IndexPosZ;
    }

    


    public BeginPos HolePos = new BeginPos();
    public List<BeginPos> Steps = new List<BeginPos>();
        
   public void SetFindObj()
    {
 
        int StepCount = 0;
        Steps.Add(HolePos);
        bool isFinish = false;
       while (true)
       {
            isFinish = true;
            if (Steps[StepCount].IndexPosX + 1 < GameManager.ins.grassSpawner.Grass[Steps[StepCount].IndexPosZ].Count) {
                if (GameManager.ins.grassSpawner.Grass[Steps[StepCount].IndexPosZ][Steps[StepCount].IndexPosX + 1].tag == GameManager.stringTag.NullCamp.ToString())
                {
                    BeginPos NewPos = new BeginPos();
                    NewPos.IndexPosX = Steps[StepCount].IndexPosX + 1;
                    NewPos.IndexPosZ = Steps[StepCount].IndexPosZ;
                    Steps.Add(NewPos);
                    break;
                   
                }
                else if (GameManager.ins.grassSpawner.Grass[Steps[StepCount].IndexPosZ][Steps[StepCount].IndexPosX + 1].tag == GameManager.stringTag.Stone.ToString())
                {
                    BeginPos NewPos = new BeginPos();
                    NewPos.IndexPosX = Steps[StepCount].IndexPosX + 1;
                    NewPos.IndexPosZ = Steps[StepCount].IndexPosZ;
                    bool isSame = CheckisSame(NewPos);

                    if (!isSame)
                    {
                        AddSteps(NewPos,ref StepCount, ref isFinish);
                    }
                }
               
            }


            if (Steps[StepCount].IndexPosX - 1 >= 0)
            {
                if (GameManager.ins.grassSpawner.Grass[Steps[StepCount].IndexPosZ][Steps[StepCount].IndexPosX - 1].tag == GameManager.stringTag.NullCamp.ToString())
                {
                    BeginPos NewPos = new BeginPos();
                    NewPos.IndexPosX = Steps[StepCount].IndexPosX - 1;
                    NewPos.IndexPosZ = Steps[StepCount].IndexPosZ;
                    Steps.Add(NewPos);
                    break;
                }
                else if (GameManager.ins.grassSpawner.Grass[Steps[StepCount].IndexPosZ][Steps[StepCount].IndexPosX - 1].tag == GameManager.stringTag.Stone.ToString())
                {
                    BeginPos NewPos = new BeginPos();
                    NewPos.IndexPosX = Steps[StepCount].IndexPosX - 1;
                    NewPos.IndexPosZ = Steps[StepCount].IndexPosZ;
                    bool isSame = CheckisSame(NewPos);

                    if (!isSame)
                    {
                        AddSteps(NewPos, ref StepCount, ref isFinish);
                    }
                }
              
            }


            if (Steps[StepCount].IndexPosZ + 1 < GameManager.ins.grassSpawner.Grass.Count)
            {
                if(GameManager.ins.grassSpawner.Grass[Steps[StepCount].IndexPosZ + 1][Steps[StepCount].IndexPosX].tag == GameManager.stringTag.NullCamp.ToString())
                {
                    BeginPos NewPos = new BeginPos();
                    NewPos.IndexPosX = Steps[StepCount].IndexPosX;
                    NewPos.IndexPosZ = Steps[StepCount].IndexPosZ + 1;
                    Steps.Add(NewPos);
                    break;
                }
                if (GameManager.ins.grassSpawner.Grass[Steps[StepCount].IndexPosZ +1][Steps[StepCount].IndexPosX].tag == GameManager.stringTag.Stone.ToString())
                {
                    BeginPos NewPos = new BeginPos();
                    NewPos.IndexPosX = Steps[StepCount].IndexPosX;
                    NewPos.IndexPosZ = Steps[StepCount].IndexPosZ + 1;
                    bool isSame = CheckisSame(NewPos);


                    if (!isSame)
                    {
                        AddSteps(NewPos, ref StepCount, ref isFinish);
                    }
                }
             
            }


            if (Steps[StepCount].IndexPosZ - 1 >= 0)
            {
                if (GameManager.ins.grassSpawner.Grass[Steps[StepCount].IndexPosZ - 1][Steps[StepCount].IndexPosX].tag == GameManager.stringTag.NullCamp.ToString())
                {

                    BeginPos NewPos = new BeginPos();
                    NewPos.IndexPosX = Steps[StepCount].IndexPosX;
                    NewPos.IndexPosZ = Steps[StepCount].IndexPosZ - 1;
                    Steps.Add(NewPos);
                    break;
                }
                else if (GameManager.ins.grassSpawner.Grass[Steps[StepCount].IndexPosZ - 1][Steps[StepCount].IndexPosX].tag == GameManager.stringTag.Stone.ToString())
                {

                    BeginPos NewPos = new BeginPos();
                    NewPos.IndexPosX = Steps[StepCount].IndexPosX;
                    NewPos.IndexPosZ = Steps[StepCount].IndexPosZ - 1;
                    bool isSame = CheckisSame(NewPos);


                    if (!isSame)
                    {
                        AddSteps(NewPos, ref StepCount, ref isFinish);
                    }
                }
          
            }
          

            if (isFinish)
            {
               
                break;
            }
        }


        //Debug.Log(GameManager.ins.grassSpawner.Grass[8][4].name);
        
    }

    bool CheckisSame(BeginPos Pos) {
        bool isSame = false;
        for (int i = 0; i < Steps.Count; i++)
        {
            if(Pos.IndexPosX == Steps[i].IndexPosX && Pos.IndexPosZ == Steps[i].IndexPosZ)
            {
                isSame = true;
                break;
            }
        }

        return isSame;
    }

    void AddSteps(BeginPos Pos, ref int Count, ref bool Isfinish)
    {
        Count++;
        Steps.Add(new BeginPos());
        Steps[Count] = Pos;
        Isfinish = false;
    }
}
