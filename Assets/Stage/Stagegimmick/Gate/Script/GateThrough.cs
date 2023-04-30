//--------------------------------------------
//　担当：菅眞心
//　内容：プレイヤーがエリアに入った時の処理
//--------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateThrough : MonoBehaviour
{
    [SerializeField, Header("生成するブロック")]
    private GameObject GateBlock;

    [SerializeField, Header("ブロックのTransform")]
    private Transform Blocktransform;

    private void OnTriggerExit2D(Collider2D collision)
    {
        // プレイヤーがすり抜けたら生成
        if(collision.tag == "Player")
        {
            Destroy(GetComponent<BoxCollider2D>());
            GameObject obj = Instantiate(GateBlock, Blocktransform);
            obj.transform.parent = null;
        }
    }
}
