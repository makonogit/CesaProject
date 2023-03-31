//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v���C���[�̗̑͂�0�ɂȂ������ɃQ�[���I�[�o�[�ɃV�[���J�ڂ���
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    [Header("���݂�HP")]
    public int HP = 5; //�̗�

    [Header("�ő�HP")]
    public int maxHp = 5; //�\������HPUI�̌�

    //private float maxWallHp = 1.0f; // �ǂ̍ő�̗�
    //private float nowWallHp; // ���݂̕ǂ̍ő�̗�
    //private float Baseline1 = 0.2f / 3 * 2; // �ǂ̃X�v���C�g��ύX�����l
    //private float Baseline2 = 0.2f / 3 * 1; // �ǂ̃X�v���C�g��ύX�����l

    // �O���擾
    //private GameObject wallSystem;
    //private Wall_HP_System_Script wallHpSystem;

    //// HP�ɂ���ď�Ԃ��ς��
    //// ��FHP��5�̎���HIGH�AMIDDLE�ALOW
    //// �S15�i�K�܂ŕ\�L�킯�ł���
    //public enum SPRITESTATUS
    //{
    //    HIGH,   // �������(0.2)�`0.1333...
    //    MIDDLE, // 0.13333....�`0.06666...
    //    LOW     // 0.06666....�`0.0
    //}

    //private SPRITESTATUS spriteStatus = SPRITESTATUS.HIGH;

    //�\�ǉ��S���ҁF���쒼�o�\//
    [SerializeField, Header("�p�[�e�B�N��")]
    private ParticleSystem _particle;
    private ParticleSystem _createdParticle;
    [SerializeField]
    private float _particleTime = 5;
    private float _particleNowTime;
    //�\�\�\�\�\�\�\�\�\�\�\�\//

    private void Start()
    {
        //wallSystem = GameObject.Find("Wall_Hp_Gauge");
        //wallHpSystem = wallSystem.GetComponent<Wall_HP_System_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------------------
        // �ǂ�HP�ɍ��킹��UI�̐��A��Ԃ�ω�

        // �ǂ̗̑͂��擾
        //nowWallHp = wallHpSystem.GetHp();

        // �ǂ̗̑͂�UI�̗̑͂��r���ď�ԁA�����v�Z
        // maxWallHp(1.0f) �� maxHP(UI) �̒i�K(����5�i�K)�ɕ����A����(0.2)�Ɍ��݂�HPUI�̌�-1(�ŏ��Ȃ�4)���|������(�ŏ��Ȃ�0.8)�ƕǂ�HP���r
        //if(nowWallHp < (HP - 1) * (maxWallHp / ((float)maxHp)))
        //{
        //    // UI�̐����炷
        //    HP--;

        //    //�������
        //    spriteStatus = SPRITESTATUS.HIGH;
        //}

        // ����0.2����0.0000001���炢�܂ł̒l�ɂȂ�
        //float temp = nowWallHp - (HP - 1) * (maxWallHp / ((float)maxHp));

        // temp�̒l�ɂ���ď�Ԃ�ς���
        // 0.2���O�i�K�ɂ킯�ď�Ԃ�Ή��t��
        //if(temp > Baseline1)
        //{
        //    spriteStatus = SPRITESTATUS.HIGH;
        //}
        //else if(temp > Baseline2)
        //{
        //    spriteStatus = SPRITESTATUS.MIDDLE;
        //}
        //else
        //{
        //    spriteStatus = SPRITESTATUS.LOW;
        //}

        // HP��0�ɂȂ��ĂȂ���
        //if (HP != 0)
        //{
        //    // �擾���Ă���wallHP��0�Ȃ�
        //    if (nowWallHp == 0.0f)
        //    {
        //        // 0�ɂ���
        //        HP = 0;
        //    }
        //}

        //---------------------------------------------------------
        //HP��0�ȉ��ɂȂ�����
        if (HP <= 0)
        {
            //�\�ǉ��S���ҁF���쒼�o�\//
            // �p�[�e�B�N������������Ă��Ȃ��Ȃ�
            if (_createdParticle == null) 
            {
                GameObject cam = GameObject.Find("Main Camera");
                Vector3 pos = cam.transform.position;
                pos = new Vector3(pos.x, pos.y, 0);
                _createdParticle = Instantiate(_particle, pos, Quaternion.Euler(-90, 0, 0), cam.transform);
                _createdParticle.Play();
                _particleNowTime = 0;
            }
           
            // ��莞�Ԍo�߂�����
            if (_createdParticle.isStopped)
            {
                //---------------------------------------------------------
                // "GameOver"�V�[���ɑJ��
                SceneManager.LoadScene("GameOver");
            }
            //�\�\�\�\�\�\�\�\�\�\�\�\//
           
        }
    }

    public void DecreaseHP(float _hp)
    {
        HP = HP - (int)_hp;
        if(HP < 0)
        {
            HP = 0;
        }
    }

    //public SPRITESTATUS GetSpriteStatus()
    //{
    //    return spriteStatus;
    //}
}
