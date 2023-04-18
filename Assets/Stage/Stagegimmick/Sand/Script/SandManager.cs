//-----------------------------------
//@SFแมS
//@เeFฌปฬว
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandManager : MonoBehaviour
{
    private List<Vector3> Sand = new List<Vector3>(1);   //@ฌป

    public void SetSand(Vector3 pos)
    {
        Sand.Add(pos);
    }

    public List<Vector3> GetSand()
    {
        return Sand;
    }
}
