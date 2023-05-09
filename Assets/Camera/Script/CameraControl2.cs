//-------------------------------
//�S���F�����S
//���e�F�J�����̒Ǐ]�E�ړ�����
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl2 : MonoBehaviour
{
    [SerializeField, Header("�Ǐ]�^�[�Q�b�g")]
    private GameObject Target;

    [SerializeField,Header("�Ǐ]�^�[�Q�b�g�̍��W")]
    private Transform TargetTrans;

    private CrackAutoMove _AutoMove;        // �Ђт̈ړ��X�N���v�g

    private GameObject CameraArea;          // �J�����̒Ǐ]�G���A
    private PolygonCollider2D AreaCollider; // �Ǐ]�G���A�̃R���C�_�[

    private Vector2[] NextAreaPos;          // ���̃G���A�̍��W

    private AreaManager _AreaManager;       // �G���A�Ǘ��I�u�W�F�N�g
    private int NowAreaNum;                 // ���݂̃G���A�ԍ�
    private bool AreaMove = false;          // �G���A�̈ړ��C�x���g

    [Header("�J�����̈ړ��X�s�[�h")]
    public float CameraMoveSpeed;
    float NowMax_x;                         // ���݂̃J�����̉E�[

    private GameObject ZoomArea;            // �J�����̃Y�[���G���A
    private CameraZoom zoom;                // �J�����Y�[���p�X�N���v�g

    private Camera MainCam;                 // ���C���J����


    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�̏��擾
        Target = GameObject.Find("player");
        TargetTrans = Target.GetComponent<Transform>();

        // �Ђт̈ړ��X�N���v�g���擾
        _AutoMove = Target.GetComponent<CrackAutoMove>();

        // �J�����̒Ǐ]�G���A�̏����擾
        CameraArea = GameObject.Find("CameraArea");
        AreaCollider = CameraArea.GetComponent<PolygonCollider2D>();
        _AreaManager = CameraArea.GetComponent<AreaManager>();

        // �J�����Y�[���G���A�̏����擾
        //ZoomArea = GameObject.Find("GoalArea");
        //zoom = ZoomArea.GetComponent<CameraZoom>();

        // �J�����̏����擾
        MainCam = GetComponent<Camera>();

        // �T�C�Y���m�ۂ��Ă���
        NextAreaPos = new Vector2[4];

        // �G���A�}�l�[�W���[����G���A�̃T�C�Y���v�Z
        Vector2[] points = AreaCollider.points;
        points[0].x = points[1].x + _AreaManager.AreaSize;
        points[3].x = points[1].x + _AreaManager.AreaSize;
        AreaCollider.SetPath(0, points);

        NowMax_x = points[0].x; // ���݂̃J�����E�[��ݒ�

        NowAreaNum = 1;         // �ŏ��̃G���A���w��

    }

    private void LateUpdate()
    {
        // �Y�[���G���A�ɂ�����Ǐ]�^�[�Q�b�g��ύX����
        //if (zoom.InArea)
        //{
        //    if (Target.name == "player")
        //    {
        //        // �^�[�Q�b�g��ύX
        //        Target = GameObject.Find("GoalArea");
        //        TargetTrans = Target.transform;
        //    }
        //}
        //else
        //{
        //    // �G���A�O�Ń^�[�Q�b�g���S�[���G���A�Ȃ�
        //    if (Target.name == "GoalArea")
        //    {
        //        // �^�[�Q�b�g��ύX
        //        Target = GameObject.Find("player");
        //        TargetTrans = Target.transform;
        //    }

        //}

       
        // ���݂̍��W���擾
        Vector3 NowPos = new Vector3(TargetTrans.position.x, TargetTrans.position.y,transform.position.z);


        // �Ђт̈ړ����̓J�����̒Ǐ]���ɂ₩�ɂ���
        if (_AutoMove.movestate == CrackAutoMove.MoveState.CrackMove)
        {
            transform.position = Vector3.Lerp(transform.position, NowPos, 1.0f * Time.deltaTime);
        }
        else
        {
            // �J�����̍��W���^�[�Q�b�g����ɍX�V
            transform.position = new Vector3(NowPos.x, NowPos.y, transform.position.z);
        }

        //----------------------------------------------------------------------
        // �G���A�̏�񂩂�R���C�_�[�����T�C�Y
        //Vector2[] points = AreaCollider.points;
        //points[0].x = points[1].x + _AreaManager.AreaSize;
        //points[3].x = points[1].x + _AreaManager.AreaSize;
        //AreaCollider.SetPath(0, points);

        //----------------------------------------------------------------------
        // �R���C�_�[�̏�񂩂��ʒ[�̍��W���擾(X�����Ȃ񂩂��ꂠ�邩��1.77f)
        float Max_x = (AreaCollider.points[0].x + AreaCollider.offset.x) - MainCam.orthographicSize * 1.65f;
        float Min_x = (AreaCollider.points[1].x + AreaCollider.offset.x) + MainCam.orthographicSize * 1.65f;
        float Max_y = (AreaCollider.points[1].y + AreaCollider.offset.y) - MainCam.orthographicSize;
        float Min_y = (AreaCollider.points[2].y + AreaCollider.offset.y) + MainCam.orthographicSize;

        // �X�e�[�W��PorigonCollider����Ɉړ�����
        NowPos.x = Mathf.Clamp(NowPos.x, Min_x, Max_x);
        NowPos.y = Mathf.Clamp(NowPos.y, Min_y, Max_y);

        //----------------------------------------------
        //�v���C���[���G���A�O�ɏo���玟�̃G���A���w��
        if (TargetTrans.position.x > AreaCollider.points[0].x + AreaCollider.offset.x)
        {
             if (!AreaMove)
            {
                NextAreaPos[0].x = AreaCollider.points[0].x + _AreaManager.AreaSize / 5;
                NextAreaPos[3].x = AreaCollider.points[0].x + _AreaManager.AreaSize / 5;
                NextAreaPos[1].x = AreaCollider.points[1].x + _AreaManager.AreaSize + 2.0f;
                NextAreaPos[2].x = AreaCollider.points[2].x + _AreaManager.AreaSize + 2.0f;
                NowAreaNum++;
                AreaMove = true;
            }
        }
        else
        {
            //AreaMove = false;
        }

        if (AreaMove)
        {
            Vector2[] points = AreaCollider.points;

            points[1].x = NextAreaPos[1].x;
            points[2].x = NextAreaPos[2].x;

            //���̃G���A�ɓ��B����܂ŉE�[���W���X�V
            if (NowMax_x <= NextAreaPos[0].x)
            {
                NowMax_x += CameraMoveSpeed * Time.deltaTime;
                points[0].x = NowMax_x;
                points[3].x = NowMax_x;
                AreaCollider.SetPath(0, points);
            }
            else
            {
                //���B������ړ����I��
                AreaMove = false;
            }
        }


        if (_AutoMove.movestate == CrackAutoMove.MoveState.CrackMove)
        {
            transform.position = Vector3.Lerp(transform.position, NowPos, 1.0f * Time.deltaTime);
        }
        else
        {
            //�@�J�����̍��W���X�V
            transform.position = new Vector3(NowPos.x, NowPos.y, transform.position.z);
        }
           
    }

    // �Ǐ]����^�[�Q�b�g��ݒ肷��
    public void SetTarget(GameObject _obj)
    {
        Target = _obj;
        TargetTrans = _obj.GetComponent<Transform>();
    }

    public GameObject GetTarget()
    {
        return Target;
    }
}
