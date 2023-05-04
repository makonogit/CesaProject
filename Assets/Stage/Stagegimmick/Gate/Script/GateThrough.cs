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

    //[SerializeField, Header("ブロックのTransform")]
    //private Transform Blocktransform;

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        // プレイヤーがすり抜けたら生成、BGMの再生
        if(collision.tag == "Player" && collision.gameObject.transform.position.x > transform.position.x)
        {
            Destroy(GetComponent<BoxCollider2D>());
            GameObject.Find("BGM(Loop)").GetComponent<AudioSource>().Stop();
            GameObject.Find("BossBGM").GetComponent<AudioSource>().Play();
            GameObject obj = Instantiate(GateBlock, transform);
            obj.transform.parent = null;
        }
    }
}
