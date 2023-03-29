//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�Ђт����ꂽ���̔����_���[�W�����炤
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaged : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    [SerializeField, Unity.Collections.ReadOnly]private bool CrackFlg; // �Ђт������Ƃ��̃t���O
    [SerializeField, Unity.Collections.ReadOnly]private float CrackPower; // �Ђт������Ƃ��̃t���O

    // �O���擾
    private Crack crack; // CrackPower���擾���邽�߂̕ϐ�
    private GameObject GUI;  // GUI�I�u�W�F�N�g��T���Ċi�[���邽�߂̕ϐ�
    private GameOver gameOver; // GameOver�X�N���v�g���i�[
    private GameObject Health; // Alpha�l���炷���߂ɕK�v�ȃX�N���v�g�����Q�[���I�u�W�F�N�g��
    private DownAlpha alpha; // UI�̃X�N���v�g

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // Crack���擾
        crack = GetComponent<Crack>();

        //---------------------------------------------------------
        // GUI��T��
        GUI = GameObject.Find("GUI");

        //---------------------------------------------------------
        // GameOver�X�N���v�g���i�[
        gameOver = GUI.GetComponent<GameOver>();

        //---------------------------------------------------------
        // Health��T��
        Health = GameObject.Find("Health");

        //---------------------------------------------------------
        // DownAlpha�X�N���v�g���i�[
        //alpha = Health.GetComponent<DownAlpha>();
    }

    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------------------
        // �Ђт������u�Ԃ̂�true�ɂȂ�t���O�����ă_���[�W��^����
        if (CrackFlg == true)
        {
            //---------------------------------------------------------
            // HP�Ƀ_���[�W��^����
            gameOver.DecreaseHP(CrackPower);
            // �A���t�@�l�ύX
            alpha.SetAlpha(gameOver.HP, gameOver.maxHp);

            CrackFlg = false;
        }
    }

    public void SetCrackInfo(float _power)
    {
        CrackFlg = true;
        CrackPower = _power;
    }
}
