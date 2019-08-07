using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {
   
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;


    public LayerMask targetMask;
    public LayerMask obstaclesMask;

    public List<Transform> visibleTargets = new List<Transform>();

    LineRenderer LR;
    public int resolution;
    float eachvertex = 0;

    public int MaxTarget;
    TowerShoot TSdata;

    
    float timeAppearLR = 0;
    void Start()
    {
       
        LR = GetComponent<LineRenderer>();
        LR.SetVertexCount(resolution);
        eachvertex = 360f / (resolution - 1);

        TSdata = GetComponent<TowerShoot>();
        viewRadius = TSdata.TowerData.TowerData.Upgrade[TSdata.IndexLevel].Range;

        StartCoroutine(findTargetWithDelay(0.1f));    
    }


    public void DrawAreaAttack() {
      
      
        LR.enabled = true;
        

        
   
        float multipleindex = 0f;
        for (int i = 0; i < resolution; i++)
        {

            if (i < resolution - 1)
            {
                Vector3 ResolutionPos = DirfromAngle(multipleindex, false);
                ResolutionPos = gameObject.transform.position + ResolutionPos * viewRadius;
                ResolutionPos.y += 20f;

                LR.SetPosition(i, ResolutionPos);
                multipleindex += eachvertex;
            }
            else
            {
                LR.SetPosition(i, LR.GetPosition(0));
            }

        }


    }


    public IEnumerator NonActiveAttackArea(float delay)
    {
        timeAppearLR = delay;
        while(timeAppearLR > 0)
        {
            timeAppearLR -= Time.deltaTime;
            yield return null;
        }

        if (LR != null)
        {
            timeAppearLR = 0;
            LR.enabled = false;
        }
    }

    IEnumerator findTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            SetFindTargets();
        }
    }
   
    void SetFindTargets() {
        List<Transform> Targets = new List<Transform>();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirTorget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirTorget) < viewAngle/2)
            {
                float dstTotarget = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, dirTorget, dstTotarget, obstaclesMask))
                {
                    if (target.GetComponent<Enemies1>().Data.type == TSdata.TowerData.TowerData.enemyTarget)
                    {
                        Targets.Add(target);
                    }
                    //Debug.Log(target.name);
                }
            }
        }

        CheckSameTarget(Targets);

        
    }

    void CheckSameTarget(List<Transform> targets)
    {
        if (targets.Count > 0)
        {
            if (visibleTargets.Count == 0)
            {
                visibleTargets.Add(targets[0]);
            }
            else
            {
                for (int k = 0; k < visibleTargets.Count; k++)
                {
                    int indexcheck = 0;
                    for (int j = 0; j < targets.Count; j++)
                    {

                        if (visibleTargets[k] != targets[j])
                        {
                            indexcheck++;
                        }
                    }
                    if (indexcheck == targets.Count)
                    {
                        visibleTargets.Remove(visibleTargets[k]);
                        k--;
                    }
                }

                for (int j = 0; j < targets.Count; j++)
                {
                    int indexcheck = 0;
                    for (int k = 0; k < visibleTargets.Count; k++)
                    {
                       
                        if (visibleTargets[k] != targets[j])
                        {
                            indexcheck++;
                        }
                       
                    }
                    if (indexcheck == targets.Count)
                    {
                        visibleTargets.Add(targets[j]);
                       
                    }
                }
            }
        }

    }
    public Vector3 DirfromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
