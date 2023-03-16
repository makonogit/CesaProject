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


    private GameObject CameraArea;          //�J�����̒Ǐ]�G���A
    private PolygonCollider2D AreaCollider; //�Ǐ]�G���A�̃R���C�_�[

    private AreaManager _AreaManager;       //�G���A�Ǘ��I�u�W�F�N�g

    private GameObject ZoomArea;            //�J�����̃Y�[���G���A
    private CameraZoom zoom;                //�J�����Y�[���p�X�N���v�g

    private Camera MainCam;                 //���C���J����


    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�̏��擾
        Target = GameObject.Find("player");
        TargetTrans = Target.GetComponent<Transform>();

        // �J�����̒Ǐ]�G���A�̏����擾
        CameraArea = GameObject.Find("CameraArea");
        AreaCollider = CameraArea.GetComponent<PolygonCollider2D>();
        _AreaManager = CameraArea.GetComponent<AreaManager>();

        // �J�����Y�[���G���A�̏����擾
        ZoomArea = GameObject.Find("GoalArea");
        zoom = ZoomArea.GetComponent<CameraZoom>();

        // �J�����̏����擾
        MainCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // �Y�[���G���A�ɂ�����Ǐ]�^�[�Q�b�g��ύX����
        if (zoom.InArea)
        {
            if(Target.name == "player")
            {
                // �^�[�Q�b�g��ύX
                Target = GameObject.Find("GoalArea");
                TargetTrans = Target.transform;
            }
        }
        else
        {
            // �G���A�O�Ń^�[�Q�b�g���S�[���G���A�Ȃ�
            if (Target.name == "GoalArea")
            {
                // �^�[�Q�b�g��ύX
                Target = GameObject.Find("player");
                TargetTrans = Target.transform;
            }

        }

        // ���݂̍��W���擾
        Vector3 NowPos = TargetTrans.position;

        // �J�����̍��W���^�[�Q�b�g����ɍX�V
        transform.position = new Vector3(NowPos.x, NowPos.y, transform.position.z);

        //----------------------------------------------------------------------
        // �G���A�̏�񂩂�R���C�_�[�����T�C�Y
        //AreaCollider.points[0].Set(AreaCollider.points[1].x + _AreaManager.AreaSize, AreaCollider.points[0].y);
        //AreaCollider.points[3].Set(AreaCollider.points[1].x + _AreaManager.AreaSize, AreaCollider.points[3].y);
        //Debug.Log(_AreaManager.AreaSize);

        //----------------------------------------------------------------------
        // �R���C�_�[�̏�񂩂��ʒ[�̍��W���擾(X�����Ȃ񂩂��ꂠ�邩��1.77f)
        float Max_x = (AreaCollider.points[0].x + AreaCollider.offset.x) - MainCam.orthographicSize * 1.77f;
        float Min_x = (AreaCollider.points[1].x + AreaCollider.offset.x) + MainCam.orthographicSize * 1.77f;
        float Max_y = (AreaCollider.points[1].y + AreaCollider.offset.y) - MainCam.orthographicSize;
        float Min_y = (AreaCollider.points[2].y + AreaCollider.offset.y) + MainCam.orthographicSize;

        // �X�e�[�W��PorigonCollider����Ɉړ�����
        NowPos.x = Mathf.Clamp(NowPos.x, Min_x, Max_x);
        NowPos.y = Mathf.Clamp(NowPos.y, Min_y, Max_y);

        //�@�J�����̍��W���X�V
        transform.position = new Vector3(NowPos.x, NowPos.y, transform.position.z);

    }
}
