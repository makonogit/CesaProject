//-------------------------------------
//�@�S���F�����S
//�@���e�F�Z���N�g��ʂ̃J�����Y�[��
//-------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectZoom : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    [SerializeField,Header("��b������̕ω���")]
    private float ChangeVolume = 0.1f; // �J�����T�C�Y�̕ω���

    [Header("�Y�[����̕`��T�C�Y")]
    public float ZoomCameraSize = 2.5f;

    [SerializeField, Header("�ʏ펞�̕`��T�C�Y")]
    private float DefaultCameraSize; //�ʏ펞�̃J�����T�C�Y

    [Header("���݂̃J�����T�C�Y")]
    public float NowCameraSize = 5.0f; // ���݂̃J�����T�C�Y

    // �O���擾
    private GameObject Camera; // �Q�[���I�u�W�F�N�gMainCamera
    Camera Cam; // �J�����X�N���v�g���擾
    VibrationCamera vibration;

    //�@�J�����̈ړ������pCollider
    private EdgeCollider2D HorizonLimit;    //��������
    private EdgeCollider2D VerticalLimit;   //��������

    private Transform cameraTransform; // �J�����̍��W

    [SerializeField, Header("�Ǐ]�^�[�Q�b�g")]
    private GameObject Target;

    [SerializeField, Header("�Ǐ]�^�[�Q�b�g�̍��W")]
    private Transform TargetTrans;

    [SerializeField]
    private SelectArea _selectarea; //�@�G���A�ړ��p�X�N���v�g�擾

   // private GameObject player;      // �v���C���[
   // private Transform playertans;   // �v���C���[��Transform

    public bool Select = false;     // �X�e�[�W��I�����Ă��邩
    public bool ZoomEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        // MainCamera�T��
        Camera = GameObject.Find("Main Camera");
        // �J�����X�N���v�g���擾
        Cam = Camera.GetComponent<Camera>();
        vibration = Camera.GetComponent<VibrationCamera>();

        cameraTransform = Camera.transform;


        //player = GameObject.Find("player");
        //playertans = player.transform;

        //�@�����J�����̃T�C�Y���擾
        DefaultCameraSize = Cam.orthographicSize;

        //�@�J�����̈ړ������pEdgeCollider���擾
        HorizonLimit = transform.GetChild(0).gameObject.GetComponent<EdgeCollider2D>();
        VerticalLimit = transform.GetChild(1).gameObject.GetComponent<EdgeCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // �X�e�[�W�I��������Y�[��
        if (Select)
        {
            // �Y�[����̃J�����`��T�C�Y�ɂȂ�܂ŏ��X�ɃY�[���C�����Ă���
            if (NowCameraSize > ZoomCameraSize)
            {
                // �`��T�C�Y�v�Z
                NowCameraSize -= ChangeVolume * Time.deltaTime;
            }
            else
            {
                NowCameraSize = ZoomCameraSize;
                ZoomEnd = true;
            }

            // �J�����̈ʒu�̓v���C���[�̒��S�ɌŒ�
            cameraTransform.position = new Vector3(TargetTrans.position.x,cameraTransform.position.y);

            //�J�����T�C�Y�ύX
            Cam.orthographicSize = NowCameraSize;

           
        }

        // ���݂̍��W���擾
        Vector3 NowPos = new Vector3(TargetTrans.position.x, TargetTrans.position.y, transform.position.z);

        if (!_selectarea.LeftMove && !_selectarea.RightMove && !_selectarea.PlayerLeftMove)
        {
            // �J�����̍��W���^�[�Q�b�g����ɍX�V
            cameraTransform.position = new Vector3(NowPos.x, cameraTransform.position.y, cameraTransform.position.z);

            //----------------------------------------------------------------------
            // ��ʒ[�̍��W���擾
            float Max_x = HorizonLimit.points[1].x - Cam.orthographicSize * 1.78f;
            float Min_x = HorizonLimit.points[0].x + Cam.orthographicSize * 1.78f;
            float Max_y = VerticalLimit.points[0].y - Cam.orthographicSize;
            float Min_y = VerticalLimit.points[1].y + Cam.orthographicSize;

            //Vector2 CameraPos = cameraTransform.position;

            // �J�����̈ړ�����

            NowPos.x = Mathf.Clamp(NowPos.x, Min_x, Max_x);
            NowPos.y = Mathf.Clamp(NowPos.y, Min_y, Max_y);

            // �J�����̍��W���^�[�Q�b�g����ɍX�V
            cameraTransform.position = new Vector3(NowPos.x, cameraTransform.position.y, cameraTransform.position.z);

        }
        

    }
}
