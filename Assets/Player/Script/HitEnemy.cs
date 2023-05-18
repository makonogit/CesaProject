//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v���C���[�̖��G���Ԃ��Ǘ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    public float NoDamageTime = 2f; //���G����
    [SerializeField] private float HitTime = 0.0f; // �O��_���[�W���󂯂�������̌o�ߎ���

    [SerializeField] private GameOver gameOver; // �Q�[���I�[�o�[��ʑJ�ڗp�X�N���v�g�擾�p�ϐ�
    [SerializeField] private KnockBack knocback; // �m�b�N�o�b�N�X�N���v�g�擾�p�ϐ�
    [SerializeField] private RenderOnOff _renderer; // �_�ŃX�N���v�g�擾�p�ϐ�
    [SerializeField] private PostEffectManager _postEffectMana; // ��_������vignette�p

    [SerializeField] private PlayEnemySound enemyse; //�G��SE

    [SerializeField]
    private DamageEffectSystem _effectSystem;

    private void Start()
    {
        // �n�܂��������G���ԂȂ̂�h�����ߏ�����
        HitTime = NoDamageTime;
    }

    void Update()
    {
        HitTime += Time.deltaTime;
    }

    // ���HitCollider�X�N���v�g����Ăяo��
    public void HitPlayer(Transform _trans)
    {
        // �ڐG���Ԃ����G���Ԃ��傫���Ȃ�HP���炷
        if (HitTime > NoDamageTime)
        {
            //---------------------------------------------------------
            //�@SE���Đ�
            enemyse.PlayEnemySE(PlayEnemySound.EnemySoundList.Attack);

            //---------------------------------------------------------
            

            // �m�b�N�o�b�N
            knocback.KnockBack_Func(_trans);

            // �_��
            _renderer.SetFlash(true);

            // ��ʌ���
            _postEffectMana.Damage();

            // �n���}�[�����
            _effectSystem.Creat();

            //---------------------------------------------------------
            // �ڐG���ԃ��Z�b�g
            HitTime = 0.0f;
        }
    }
}
