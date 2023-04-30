//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�{�X���j��̉��o
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;  // Light2D�p��using

public class Directing_BossLight : MonoBehaviour
{
    // �ϐ��錾

    public int LightNum = 5; // ���o�Ɏg�����C�g�̐� 
    public bool FlashStart = false; // true�Ō��鉉�o���n�܂�
    public bool BreakStart = false; // true�ŉ��鉉�o�n�܂�
    private float DirectingTimer = 0f; // ���o�J�n����̌o�ߎ���
    private int FlashNum = 0; // ���������Ă��邩

    public GameObject PieceOfBoss; // ���U����p�[�e�B�N��

    public List<float> FlashTiming = new List<float>(); // float�^�̃��X�g���`

    // �Q�[���I�u�W�F�N�g�i�[�z��
    private List<GameObject> BossLight = new List<GameObject>(); //GameObject�^�̃��X�g���`

    // Light2D�i�[�z��
    private List<Light2D> sc_light = new List<Light2D>(); // Light2D�^�̃��X�g���`

    private Transform thisTransform;

    // Start is called before the first frame update
    void Start()
    {
        // ���X�g�ɗv�f�ǉ�
        for(int i = 0; i < LightNum; i++)
        {
            // �Q�[���I�u�W�F�N�g�擾
            BossLight.Add(transform.GetChild(i).gameObject);

            // Light2D�擾
            sc_light.Add(BossLight[i].GetComponent<Light2D>());

            //Debug.Log("BossLight" + BossLight[i]);
            //Debug.Log("sc_light" + sc_light[i]);
        }

        thisTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���o�J�n
        if(FlashStart == true)
        {
            // ���C�g�̐��Ԃ����܂�
            if (FlashNum < LightNum)
            {
                // ���o�J�n���Ă���̌o�ߎ��Ԃ����C�g�����点��^�C�~���O���߂�����
                if (DirectingTimer > FlashTiming[FlashNum])
                {
                    // ���C�g�����点��
                    sc_light[FlashNum].intensity = 1;

                    // ���点�����C�g�̐������Z
                    FlashNum++;
                }
            }
            else
            {
                if (DirectingTimer > FlashTiming[LightNum - 1] + 0.5f)
                {
                    BreakStart = true;
                }
            }

            DirectingTimer += Time.deltaTime;
        }
        else
        {
            // ������
            DirectingTimer = 0f;
            FlashNum = 0;

            for(int i = 0;i < LightNum; i++)
            {
                sc_light[i].intensity = 0;
            }
        }

        // ���U����
        if (BreakStart)
        {
            // �p�[�e�B�N������
            var Obj = Instantiate(PieceOfBoss);
            Obj.transform.position = thisTransform.position;

            // �{�X����
            Destroy(transform.parent.gameObject);
        }
    }

    // ���̃X�N���v�g�ŌĂяo���ĉ��o�J�n
    public void Flash()
    {
        FlashStart = true;
    }

    public void Break()
    {
        BreakStart = true;
    }
}
