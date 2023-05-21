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

    public bool Throgh = false;    // �ʂ蔲������

    private Transform PlayerTrans;  //�@�v���C���[��Transform

    private AudioSource stage_bgm;  //�X�e�[�WBGM
    private AudioSource boss_bgm;   //�{�XBGM
    
    private GameObject Gateobj;     //��������Gate

    private CameraControl2 camera;

    private void Start()
    {
        // BGM�Đ��V�X�e���擾
        stage_bgm = GameObject.Find("BGM(Loop)").GetComponent<AudioSource>();
        boss_bgm = GameObject.Find("BossBGM").GetComponent<AudioSource>();

        PlayerTrans = GameObject.Find("player").transform;

        // �J��������擾
        camera = GameObject.Find("Main Camera").GetComponent<CameraControl2>();

    }

    //[SerializeField, Header("�u���b�N��Transform")]
    //private Transform Blocktransform;

    private void Update()
    {
        // ���X�|�[������
        if (Throgh && PlayerTrans.position.x < transform.position.x)
        {
            Destroy(Gateobj);   //���������Q�[�g���폜
            boss_bgm.Stop();    
            stage_bgm.Play();
            stage_bgm.volume = 0f; // ��{�ǉ�
            camera.InitCamera();    //�J�����ʒu������
            Throgh = false;     //�ʂ蔲���t���O�̏�����

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        // �v���C���[�����蔲�����琶���ABGM�̍Đ�
        if(collision.tag == "Player" && collision.gameObject.transform.position.x > transform.position.x && !Throgh)
        {
            Throgh = true;

            stage_bgm.GetComponent<AudioSource>().Stop();
            boss_bgm.Play();
            Gateobj = Instantiate(GateBlock, transform);
            //Gateobj.transform.parent = null;
        }
    }
}
