//--------------------------------------------
//�@�S���F�����S
//�@���e�F�v���C���[���G���A�ɓ��������̏���
//--------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateThrough : MonoBehaviour
{
    [SerializeField, Header("��������u���b�N")]
    private GameObject GateBlock;

    [SerializeField, Header("�u���b�N��Transform")]
    private Transform Blocktransform;

    [SerializeReference, Header("BGM�pAudioSorce")]
    private AudioSource BGM;

    [SerializeReference, Header("�{�XBGM�pAudioSorce")]
    private AudioSource BossBGM;

    private void Start()
    {
        BGM = GameObject.Find("BGM(Loop)").GetComponent<AudioSource>();
        BossBGM = GameObject.Find("BossBGM").GetComponent<AudioSource>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �v���C���[�����蔲�����琶��
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
