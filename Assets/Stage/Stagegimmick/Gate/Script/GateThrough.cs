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

    [SerializeReference, Header("BGM用AudioSorce")]
    private AudioSource BGM;

    [SerializeReference, Header("ボスBGM用AudioSorce")]
    private AudioSource BossBGM;

    private void Start()
    {
        BGM = GameObject.Find("BGM(Loop)").GetComponent<AudioSource>();
        BossBGM = GameObject.Find("BossBGM").GetComponent<AudioSource>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // プレイヤーがすり抜けたら生成
        if(collision.tag == "Player")
        {
            Destroy(GetComponent<BoxCollider2D>());
            GameObject obj = Instantiate(GateBlock, Blocktransform);
            BGM.Stop();
            BossBGM.Play();
            obj.transform.parent = null;
        }
    }
}
