//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�w�i��̗͂ɂ���ĕω�������
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBackGround : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -
    //[Header("�摜�������ς��Hp�̎Q�ƒl")]
    //public float[] HpLine =
    //{
    //    0.9f,
    //    0.6f,
    //    0.3f,
    //};

    //public enum BACKGROUNDSTATUS
    //{
    //    UN_DAMAGED,      // ����
    //    LITTLE_DAMAGED,  // �������Ђ�
    //    HALF_DAMAGED,    // �������炢�Ђ�
    //    ALMOST_DAMAGED,  // �قڂЂ�
    //}

    //// ���߂͖���
    //public BACKGROUNDSTATUS BackGroundStatus = BACKGROUNDSTATUS.UN_DAMAGED;

    //// �O���擾
    //private GameObject WallGauge;
    //private Wall_HP_System_Script wallHP;

    //private SpriteRenderer Sprite; // �摜��ύX���邽�߂̕ϐ�
    //[SerializeField] Sprite[] sprites; // �摜�������Ă���

    //// Start is called before the first frame update
    //void Start()
    //{
    //    WallGauge = GameObject.Find("Wall_Hp_Gauge");
    //    wallHP = WallGauge.GetComponent<Wall_HP_System_Script>();

    //    Sprite = GetComponent<SpriteRenderer>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // �摜��ύX���邩
    //    bool ChangeSprite = false;

    //    // ���ꂼ��̏�ԂŎw���HP�ȉ��ɂȂ�����
    //    switch (BackGroundStatus)
    //    {
    //        case BACKGROUNDSTATUS.UN_DAMAGED:

    //            if (wallHP._nowHP <= HpLine[(int)BackGroundStatus])
    //            {
    //                // ��Ԃ�ω�
    //                BackGroundStatus = BACKGROUNDSTATUS.LITTLE_DAMAGED;
    //                ChangeSprite = true;
    //            }
    //            break;

    //        case BACKGROUNDSTATUS.LITTLE_DAMAGED:
    //            if (wallHP._nowHP < HpLine[(int)BackGroundStatus])
    //            {
    //                // ��Ԃ�ω�
    //                BackGroundStatus = BACKGROUNDSTATUS.HALF_DAMAGED;
    //                ChangeSprite = true;
    //            }
    //            break;

    //        case BACKGROUNDSTATUS.HALF_DAMAGED:
    //            if (wallHP._nowHP < HpLine[(int)BackGroundStatus])
    //            {
    //                // ��Ԃ�ω�
    //                BackGroundStatus = BACKGROUNDSTATUS.ALMOST_DAMAGED;
    //                ChangeSprite = true;
    //            }
    //            break;
    //    }

    //    // �X�v���C�g�ύX���߂��łĂ�����
    //    if (ChangeSprite)
    //    {
    //        // �����Ђт̓������w�i�摜�ɍ����ւ�
    //        Sprite.sprite = sprites[(int)BackGroundStatus];
    //    }
    //}
}
