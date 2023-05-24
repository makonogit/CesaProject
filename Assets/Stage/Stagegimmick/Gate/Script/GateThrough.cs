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

    private GameObject MainCamera;
    private BGMFadeManager _BGMfadeMana;
    private PlayBgm _playBGM;

    private void Start()
    {
        // BGM�Đ��V�X�e���擾
        stage_bgm = GameObject.Find("BGM(Loop)").GetComponent<AudioSource>();
        boss_bgm = GameObject.Find("BossBGM").GetComponent<AudioSource>();

        PlayerTrans = GameObject.Find("player").transform;

        MainCamera = GameObject.Find("Main Camera");

        // �J��������擾
        camera = MainCamera.GetComponent<CameraControl2>();
        // BGM�t�F�[�h�}�l�[�W���[�擾
        _BGMfadeMana = MainCamera.GetComponent<BGMFadeManager>();
        _playBGM = MainCamera.GetComponent<PlayBgm>();

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
            boss_bgm.volume = 0f;

            // �X�e�[�WBGM�Đ��J�n
            stage_bgm.Play();

            stage_bgm.volume = 0f; // ��{�ǉ�
            _playBGM.StartBossBattle = false;

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

            // �X�e�[�WBGM�t�F�[�h�A�E�g(���Ƃ��Ɖ��ʂ�0)
            _BGMfadeMana.SmallStageBGM();
            //stage_bgm.GetComponent<AudioSource>().Stop();

            // �{�XBGM�Đ��J�n&�t�F�[�h�C��
            boss_bgm.Play();
            _BGMfadeMana.BigBossBGM();
            _playBGM.StartBossBattle = true;

            Gateobj = Instantiate(GateBlock, transform);
            //Gateobj.transform.parent = null;
        }
    }
}
