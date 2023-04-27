//-----------------------------------
//　担当：菅眞心
//　内容：流砂の管理
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandManager : MonoBehaviour
{
    private List<Vector3> Sand = new List<Vector3>(1);   //　流砂

    public void SetSand(Vector3 pos)
    {
        Sand.Add(pos);
    }

    public List<Vector3> GetSand()
    {
        return Sand;
    }
}
