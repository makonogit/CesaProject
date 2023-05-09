//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F���X�|�[�����Ƀ��X�|�[���n�_�ɂ���ăN���X�^���̕\����\����؂�ւ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitScoreCrystal : MonoBehaviour
{
    // �ϐ��錾

    // �q�I�u�W�F�N�g�̃X�R�A�N���X�^�����i�[���郊�X�g
    private List<CrystalObj> CrystalList = new List<CrystalObj>();

    private int ChildNum; // �q�I�u�W�F�N�g�̐�

    private bool Init = false;

    private GameObject player;
    private PlayerStatas playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        // �q�I�u�W�F�N�g�̐��擾
        ChildNum = this.transform.childCount;

        // �q�̐��������X�g�ɃQ�[���I�u�W�F�N�g�i�[
        for(int i = 0; i < ChildNum; i++)
        {
            CrystalObj cryObj = new CrystalObj();

            // �q�I�u�W�F�N�g�擾
            cryObj._CrystalObj = this.transform.GetChild(i).gameObject;
            // �q�̍��W�擾
            cryObj._CrystalPos = cryObj._CrystalObj.transform.position;
            // �X�v���C�g�����_���[
            cryObj._renderer = cryObj._CrystalObj.GetComponent<SpriteRenderer>();
            // �擾�ς݂��ϐ������X�N���v�g�擾
            cryObj._CrystalNum = cryObj._CrystalObj.GetComponent<CrystalNum>();

            // ���Ɏq���擾�A�ǉ�
            CrystalList.Add(cryObj);

        }

        // �v���C���[�֌W
        player = GameObject.Find("player");
        playerStatus = player.GetComponent<PlayerStatas>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���������߂����������������s
        if(Init == true)
        {
            for (int i = 0;i < ChildNum; i++)
            {
                // ���X�|�[�����W��荶�Ȃ�false
                if (CrystalList[i]._CrystalPos.x < playerStatus.GetRespawn().x)
                {
                    // ���X�|�[������O�Ŏ擾�ς݂Ȃ�
                    if (CrystalList[i]._CrystalNum.Get == true)
                    {
                        // ��\��
                        CrystalList[i]._renderer.enabled = false;
                        //Debug.Log("false�ɂ���");
                    }
                }
                else
                {
                    // ���X�|�[����艜�Ŏ擾�ς݂Ȃ�
                    if (CrystalList[i]._CrystalNum.Get == true)
                    {
                        // �\��
                        CrystalList[i]._renderer.enabled = true;
                        // �Ď擾�\�ɂ���
                        CrystalList[i]._CrystalNum.Get = false;
                        //Debug.Log("true�ɂ���");
                    }
                }
            }

            Init = false;
        }
    }

    // ���X�|�[�����ɃX�R�A�N���X�^���̏������X�^�[�g
    public void ScoreCrystalInitStart()
    {
        Init = true;
    }
}

// �N���X�^���I�u�W�F�N�g�����������邽�߂̕ϐ�������N���X
public class CrystalObj
{
    public GameObject _CrystalObj;
    public Vector3 _CrystalPos;
    public SpriteRenderer _renderer;
    public CrystalNum _CrystalNum;
}