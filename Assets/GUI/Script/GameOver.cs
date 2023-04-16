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

    private GameObject health; // HPUIManager�I�u�W�F�N�g
    private DrawHpUI drawHpUI; // HP�`��X�N���v�g

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
    [SerializeField]
    private float _creatTime;
    private ParticleSystem _createdParticle;

    private bool _isGameOver;// �t���O
    [SerializeField, Header("�I���҂�����")]
    private float _waitTime;
    private float _nowTime;

    private SceneChange _scene;
    private GameObject cam;

    private PlayerMove _playerMove;
    private PlayerJump _playerJump;
    private CrackAutoMove _crackAuto;
    private SmashScript _smashScript;
    [SerializeField,Header("�S�[���G���A�̃I�u�W�F")]
    private GameObject _goalArea;
    // GameOverCameraEvent�Ə���������̂Ŏ~�߂邽��
    private CameraZoom _cameraZoom;
    private CameraControl2 _cameraControl2;
    //�\�\�\�\�\�\�\�\�\�\�\�\//

    private void Start()
    {
        //wallSystem = GameObject.Find("Wall_Hp_Gauge");
        //wallHpSystem = wallSystem.GetComponent<Wall_HP_System_Script>();
        //�\�ǉ��S���ҁF���쒼�o�\//
        _isGameOver = false;
        _nowTime = 0.0f;
        //--------------------------------------
        //SceneChange�̎擾
        _scene = GameObject.Find("SceneManager").GetComponent<SceneChange>();
        if (_scene == null) Debug.LogError("SceneChange�̃R���|�[�l���g���擾�ł��܂���ł����B");
        cam = GameObject.Find("Main Camera");
        if (cam == null) Debug.LogError("Main Camera��������܂���ł����B");
        _goalArea = GameObject.Find("GoalArea");
        

        _playerMove = GetComponent<PlayerMove>();
        if (_playerMove == null) Debug.LogError("PlayerMove�̃R���|�[�l���g���擾�ł��܂���ł����B");
        _playerJump = GetComponent<PlayerJump>();
        if (_playerJump == null) Debug.LogError("PlayerJump�̃R���|�[�l���g���擾�ł��܂���ł����B");
        _crackAuto = GetComponent<CrackAutoMove>();
        if (_crackAuto == null) Debug.LogError("CrackAutoMove�̃R���|�[�l���g���擾�ł��܂���ł����B");

        _smashScript = GetComponent<SmashScript>();
        if (_smashScript == null) Debug.LogError("SmashScripte�̃R���|�[�l���g���擾�ł��܂���ł����B");

        _cameraZoom = _goalArea.GetComponent<CameraZoom>();
        if (_cameraZoom == null) Debug.LogError("CameraZoom�̃R���|�[�l���g���擾�ł��܂���ł����B");

        _cameraControl2 = cam.GetComponent<CameraControl2>();
        if (_cameraControl2 == null) Debug.LogError("CameraControl2�̃R���|�[�l���g���擾�ł��܂���ł����B");
        //�\�\�\�\�\�\�\�\�\�\�\�\//

        // DrawHpUI�X�N���v�g�擾
        health = GameObject.Find("Health");
        drawHpUI = health.GetComponent<DrawHpUI>();

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

            Deactivate();
            _isGameOver = true;

            // �p�[�e�B�N������������Ă��Ȃ��Ȃ�
            if (_createdParticle == null && _nowTime>_creatTime) 
            {
                Vector3 pos = cam.transform.position;
                pos = new Vector3(pos.x, pos.y, 0);
                _createdParticle = Instantiate(_particle, pos, Quaternion.Euler(-90, 0, 0), cam.transform);
                _createdParticle.Play();
            }
           
            // ��莞�Ԍo�߂�����
            if (_nowTime >_waitTime)
            {
                //---------------------------------------------------------
                // "GameOver"�V�[���ɑJ��
                //SceneManager.LoadScene("GameOver");
                _scene.LoadScene("newSelectScene");
            }
            _nowTime += Time.deltaTime;
            //�\�\�\�\�\�\�\�\�\�\�\�\//
           
        }

        //------------------------------------------
        //�@�ޗ��ɗ������烊���[�h
        if(transform.position.y < -15)
        {
            _scene.LoadScene("MainScene");
        }

    }

    public void StartHPUIAnimation()
    {
        drawHpUI.Set_HPAnim(true);
    }

    public void DecreaseHP(float _hp)
    {
        HP = HP - (int)_hp;
        if(HP < 0)
        {
            HP = 0;
        }
    }
    
    //�\�ǉ��S���ҁF���쒼�o�\//
    public bool IsGameOver // �O���{���p�\�O������̐ݒ�ύX�Ȃ�
    {
        get
        {
            return _isGameOver;
        }
    }

    // �v���C���[�̍s���n�X�N���v�g���A�N�e�B�u��
    private void Deactivate() 
    {
        _playerMove.enabled = false;
        _playerJump.enabled = false;
        _crackAuto.enabled = false;
        _smashScript.enabled = false;
        _goalArea.SetActive(false);
        _cameraZoom.enabled = false;
        _cameraControl2.enabled = false;
    }
    //�\�\�\�\�\�\�\�\�\�\�\�\//
    //public SPRITESTATUS GetSpriteStatus()
    //{
    //    return spriteStatus;
    //}
}
