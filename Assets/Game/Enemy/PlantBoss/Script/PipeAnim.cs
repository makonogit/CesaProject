//--------------------------------
// 担当：菅
// パイプアニメーション
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeAnim : MonoBehaviour
{
    Transform leftpipe;
    Transform rightpipe;

    // パイプの目的地
    Vector3 lefttarget;
    Vector3 righttarget;

    [SerializeField] private float MoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        leftpipe = transform.GetChild(0).transform;
        rightpipe = transform.GetChild(1).transform;

        lefttarget = new Vector3(leftpipe.position.x + 2, leftpipe.position.y, leftpipe.position.z);
        righttarget = new Vector3(rightpipe.position.x - 1.5f, rightpipe.position.y, rightpipe.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // 目的地まで移動
        leftpipe.position = Vector3.MoveTowards(leftpipe.position,lefttarget,MoveSpeed * Time.deltaTime);
        rightpipe.position = Vector3.MoveTowards(rightpipe.position, righttarget, MoveSpeed * Time.deltaTime);
    }
}
