//-------------------------------------
// �S���F�����S
// ���e�F�s���~�b�h�̃f�[�^
//-------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidData : MonoBehaviour
{
    //----------------------------------------
    // �ϐ��錾

    //----------------------------------------
    // �O���擾
    private DesertBossMove BossMove;    // �{�X�̍s���X�N���v�g
    private SpriteRenderer renderer;    // ���̃I�u�W�F�N�g��Spriterenderer
    private GameObject PyramidList;     // �s���~�b�h�Ǘ��I�u�W�F�N�g
    [SerializeField,Header("�R�A�I�u�W�F�N�g")]
    private GameObject CoreObj;         // �R�A�I�u�W�F�N�g
    [SerializeField, Header("�G�I�u�W�F�N�g")]
    private GameObject EnemyObj;        // �G�I�u�W�F�N�g
   

    [SerializeField, Header("��ꂽ�X�v���C�g")]
    private Sprite BreakPyramid;

    [SerializeField, Header("����A�j���[�V����")]
    Animator anim;

    [Header("���g 0:�G 1:�R�A�@�G��Ȃ���")]
    public int InsideNum = 0;

    [SerializeField, Header("�o���X�s�[�h")]
    private float MoveSpeed;

    public bool Breaked = false;   // ���Ă��邩�ǂ���
    public bool Clean = false;     // �Еt����t���O
    
    private bool HitTrigger = false;    // 1�񂠂��蔻��
    public bool MoveFlg = false;   // �ړ������ǂ���
    [SerializeField]
    private float Ypos;     // Y���W

    private void Start()
    {
        //--------------------------------
        // �{�X�̍s���X�N���v�g���擾
        GameObject obj = GameObject.Find("DesertBoss");
        BossMove = obj.GetComponent<DesertBossMove>();
        
        renderer = GetComponent<SpriteRenderer>();  // ���̃I�u�W�F�N�g�̃����_���[���擾

        PyramidList = GameObject.Find("PyramidList"); // �s���~�b�h�Ǘ��I�u�W�F�N�g�̎擾

        Ypos = transform.localPosition.y;

    }

    // Update is called once per frame
    void Update()
    {
        //---------------------
        // �ړ���
        if (MoveFlg)
        {
            Ypos += MoveSpeed * Time.deltaTime;  
        }

        //---------------------
        //�@�Еt�����
        if (Clean)
        {
            
            if(Ypos <= -0.5f)
            {
                Clean = false;
                renderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                transform.parent = PyramidList.transform;
                BossMove.Breaking = false;
                BossMove.Setvibratioin = false;

                //�@�I�����Ă��Ȃ�������X�V
                if (BossMove.BossState != DesertBossMove.DesertBossState.END)
                {
                    BossMove.BossState = DesertBossMove.DesertBossState.IDLE;
                }

            }
            else
            {
                Ypos -= MoveSpeed * Time.deltaTime;
            }
        }

        
        //�@���W�X�V
        transform.localPosition = new Vector3(transform.localPosition.x, Ypos, 0.0f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //-------------------------------------
        // �������̂Ђт������������
        if (collision.gameObject.tag == "Crack" && !Breaked && !MoveFlg)
        {
            CrackCreater creater = collision.GetComponent<CrackCreater>();
          
            if (creater != null &&
                (creater.GetState() == CrackCreater.CrackCreaterState.CREATING ||
                creater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATING))
            {
                Destroy(collision.gameObject);
                anim.SetBool("Break", true);
                //renderer.sprite = BreakPyramid;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //---------------------------------------
        //�@�n�ʂ̒��ɖ��܂��Ă�����ړ��J�n
        if (collision.gameObject.tag == "Ground")
        {
            if (renderer.color.a == 1.0f && !Clean)
            {
                MoveFlg = true;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //---------------------------------------
        //�@�n�ʂ̒�����o����ړ��I��
        if (collision.gameObject.tag == "Ground")
        {
            MoveFlg = false;

        }
    }

    void CreatePyramidCore()
    {
        Destroy(GetComponent<HitCollider>());
        BossMove.Breaking = true;
        Breaked = true;

        if (InsideNum == 0)
        {
            GameObject obj = Instantiate(EnemyObj, transform.position, Quaternion.identity);
            GameObject Enemy = GameObject.Find("BossEnemyManager");
            obj.transform.parent = Enemy.transform;
        }
        else
        {
            GameObject obj = Instantiate(CoreObj, transform.position, Quaternion.identity);
            //GameObject Core = GameObject.Find("Core");
            //obj.transform.parent = Core.transform;
            BossMove.BossState = DesertBossMove.DesertBossState.END;    // �U���I��
        }
    }

}
