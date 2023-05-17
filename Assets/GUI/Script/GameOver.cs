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

    // �t�F�[�h�֌W
    [Header("�t�F�[�h�A�E�g�ƃt�F�[�h�C���̊Ԋu")]public float OutInTime = 0.5f;
    private float OutInTimer = 0; // �^�C�}�[
    private bool _fadeout = false; // �t�F�[�h�A�E�g��
    private bool WaitTime = false; // �t�F�[�h�C���ƃA�E�g�̑҂����Ԓ�
    private bool hell = false; // �ޗ�
    private bool death = false; // ���S

    private Transform playerTransform;
    private PlayerStatas playerStatus;
    private Fade fade;

    // ��{�ǉ�
    private GameObject StageData;
    private GameObject StagePrefab;
    private RespawnObjManager _respawnObjManager;

    //�\�ǉ��S���ҁF���쒼�o�\//
    [SerializeField, Header("�p�[�e�B�N��")]
    private ParticleSystem _particle;
    [SerializeField]
    private float _creatTime;
    private ParticleSystem _createdParticle;
    private bool Create = false;

    private bool _isGameOver;// �t���O
    [SerializeField, Header("�I���҂�����")]
    private float _waitTime;
    private float _nowTime;

    private SceneChange _scene;
    private GameObject cam;

    private PlayerMove _playerMove;
    private New_PlayerJump _playerJump;
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

        StageData = GameObject.Find("StageData");

        //SceneChange�̎擾
        _scene = GameObject.Find("SceneManager").GetComponent<SceneChange>();
        if (_scene == null) Debug.LogError("SceneChange�̃R���|�[�l���g���擾�ł��܂���ł����B");
        cam = GameObject.Find("Main Camera");
        if (cam == null) Debug.LogError("Main Camera��������܂���ł����B");
        _goalArea = GameObject.Find("StageData").transform.GetChild(0).gameObject;
        

        _playerMove = GetComponent<PlayerMove>();
        if (_playerMove == null) Debug.LogError("PlayerMove�̃R���|�[�l���g���擾�ł��܂���ł����B");
        _playerJump = GetComponent<New_PlayerJump>();
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

        // ���X�|�[���֌W
        playerTransform = GetComponent<Transform>();
        playerStatus = GetComponent<PlayerStatas>();
        StagePrefab = StageData.transform.GetChild(0).gameObject;
        //Debug.Log(StagePrefab);
        _respawnObjManager = StagePrefab.GetComponent<RespawnObjManager>();
        //Debug.Log(_respawnObjManager);

        // �t�F�[�h�֌W
        fade = GameObject.Find("SceneManager").GetComponent<Fade>();
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

        //------------------------------------------
        //�ޗ��ɗ�����
        if (transform.position.y < -15)
        {
            hell = true;
            _isGameOver = true;

        }
        // HP���Ȃ��Ȃ���
        if(HP <= 0)
        {
            death = true;
            _isGameOver = true;
        }

        //---------------------------------------------------------
        //HP��0���ޗ��ɗ������烊�X�|�[��
        if (death || hell)
        {
            //�\�ǉ��S���ҁF���쒼�o�\//

            //Deactivate();

            // �p�[�e�B�N������������Ă��Ȃ��Ȃ�
            if (_createdParticle == null && _nowTime>_creatTime && Create == false) 
            {
                Vector3 pos = cam.transform.position;
                pos = new Vector3(pos.x, pos.y, 0);
                _createdParticle = Instantiate(_particle, pos, Quaternion.Euler(-90, 0, 0), cam.transform);
                _createdParticle.Play();
                Create = true;
            }
           
            // ��莞�Ԍo�߂�����
            if (_nowTime >_waitTime)
            {
                //---------------------------------------------------------
                // "GameOver"�V�[���ɑJ��
                //SceneManager.LoadScene("GameOver");
                //_scene.LoadScene("newSelectScene");

                // �t�F�[�h�̏�Ԏ擾
                Fade.FadeState fadestate = fade.GetFadeState();

                // �t�F�[�h�A�E�g
                if (fadestate != Fade.FadeState.FadeOut && _fadeout == false)
                {
                    // �t�F�[�h�A�E�g�J�n
                    fade.FadeOut();

                    _fadeout = true;
                }

                // �҂����Ԓ��Ƀ��X�|�[���Ɗ����߂�����(������)
                if(fadestate == Fade.FadeState.FadeOut_Finish || WaitTime)
                {
                    // ����̂�
                    if(OutInTimer == 0f)
                    {
                        // ���X�|�[��

                        // ���X�|�[�����̃X�e�[�^�X���擾
                        //RespawnStatus _respawnSta = playerStatus.GetRespawnStatus();

                        Debug.Log("���X�|�[��");

                        // �ۑ�����Ă��郊�X�|�[�����W���v���C���[�ɑ��
                        playerTransform.position = playerStatus.respawnStatus.PlayerRespawnPos;

                        // �������x�������肷���ď����ђʂ��Ȃ��悤�ɂ��邽��
                        _playerJump.MoveY = 0f;

                        // HPUI������
                        drawHpUI.NowHPAnimationNumber = 0;
                        drawHpUI.InitImage();

                        //Debug.Log("SSS");

                        // �����߂�����
                        _respawnObjManager.RespawnInit();

                        //Debug.Log("SSS");
                        //Debug.Log(playerStatus.respawnStatus.RespawnCrystalNum);

                        // HP��
                        HP = maxHp;

                        WaitTime = true;
                        _isGameOver = false;
                    }

                    OutInTimer += Time.deltaTime;
                    if(OutInTimer > OutInTime)
                    {
                        // �t�F�[�h�C���J�n
                        fade.FadeIn();

                        // ���������p�[�e�B�N���폜
                        Destroy(_createdParticle);
                        //Debug.Log(playerStatus.respawnStatus.RespawnCrystalNum);
                        // �N���X�^�����������Z�b�g
                        playerStatus.SetCrystal(playerStatus.respawnStatus.RespawnCrystalNum);

                        // ������
                        OutInTimer = 0f;
                        _fadeout = false;
                        WaitTime = false;
                        Create = false;
                        hell = false;
                        death = false;
                    }
                }

            }
            _nowTime += Time.deltaTime;
            //�\�\�\�\�\�\�\�\�\�\�\�\//
           
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

    private void activate()
    {
        _playerMove.enabled = true;
        _playerJump.enabled = true;
        _crackAuto.enabled = true;
        _smashScript.enabled = true;
        _goalArea.SetActive(true);
        _cameraZoom.enabled = true;
        _cameraControl2.enabled = true;
    }
    //�\�\�\�\�\�\�\�\�\�\�\�\//
    //public SPRITESTATUS GetSpriteStatus()
    //{
    //    return spriteStatus;
    //}
}
