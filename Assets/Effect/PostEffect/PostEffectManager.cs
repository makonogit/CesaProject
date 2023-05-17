//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�X�e�[�W�ɂ���ă|�X�g�G�t�F�N�g�ɂ���ʌ��ʂ�ω�������
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostEffectManager : MonoBehaviour
{
    // �ϐ��錾

    // �_���[�W���󂯂��Ƃ��̉�ʌ��ʂ��悹��֐��ɓ��邽�߂̃t���O
    public bool damageFlg = false;
    private float Timer = 0f;
    [SerializeField] private float FilterTime = 1f; // ��ʌ��ʂ��J�n���Ă���I������܂ł̎���
    [SerializeField] private float ChangeVolume = 0.1f; // vignette�̒l�𑝂₷��



    // �|�X�g�G�t�F�N�g�̒����������X�e�[�^�X
    [SerializeField] private List<PostEffectStatus> _postESta;

    // globsl Volume
    [SerializeField] private Volume PostFXvolume;

    // effect����

    // vignette
    private Vignette vignette;

    SetStage setStage = new SetStage();

    // Start is called before the first frame update
    void Start()
    {
        // �|�X�g�G�t�F�N�g�v���Z�b�T�[�̓R���|�[�l���g�ł͂Ȃ�����GetComponent���g��Ȃ�
        // �����Ɋe�G�t�F�N�g�̍\����������profile�ϐ���TryGet���\�b�h�Ŋe�v���Z�b�T�[���擾

        //------------------------------------------
        //vignette

        // �G�t�F�N�g�擾
        PostFXvolume.profile.TryGet(out vignette);
        // �G���[�`�F�b�N
        if(vignette == null)
        {
            Debug.Log("No Vignette");
        }
        // �����ݒ�
        vignette.intensity.value = _postESta[setStage.GetAreaNum()]._vignette_intensity;
        // �J���[�ݒ�
        vignette.color.value = _postESta[setStage.GetAreaNum()]._vinette_color;


        //-------------------------------------------

        //Debug.Log(setStage.GetAreaNum());

    }

    private void Update()
    {
        //// �����ݒ�
        //vignette.intensity.value = _postESta[setStage.GetAreaNum()]._vignette_intensity;
        //// �J���[�ݒ�
        //vignette.color.value = _postESta[setStage.GetAreaNum()]._vinette_color;

        if(damageFlg == true)
        {
            DamageFilter();
        }
    }

    public void Damage()
    {
        // �_���[�W������������̏��������s���邽�߂�true
        damageFlg = true;
    }

    // ��_���[�W���ɉ�ʂ̎���ɐF�������
    private void DamageFilter()
    {
        // �o�ߎ��Ԃɉ������ω���
        float value;

        // ���o���Ԃ̔����ȉ��Ȃ�㏸
        if (Timer <= FilterTime / 2f)
        {
            value = ChangeVolume * Timer;
        }
        else
        {
            value = ChangeVolume * (FilterTime - Timer);
        }

        //Debug.Log(value);

        vignette.intensity.value = _postESta[setStage.GetAreaNum()]._vignette_intensity + value;

        if (Timer > FilterTime)
        {
            damageFlg = false;

            // ������
            Timer = 0f;
            vignette.intensity.value = _postESta[setStage.GetAreaNum()]._vignette_intensity;
        }

        Timer += Time.deltaTime;

        Debug.Log(vignette.intensity.value);
    }
}

[System.Serializable]
public class PostEffectStatus
{
    //�X�e�[�W���ƂɕύX������ �|�X�g�G�t�F�N�g�̐��l�����ϐ�
    public float _vignette_intensity; // ���B�l�b�g�̌��ʂ��ǂꂭ�炢�|���邩
    public Color _vinette_color; // ���B�l�b�g�̐F

    public PostEffectStatus(float _VineInten,Color _VineCol)
    {
        _vignette_intensity = _VineInten;
        _vinette_color = _VineCol;
    }
}
