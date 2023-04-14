//---------------------------------
//�S���F��{��
//���e�F�p�[�����m�C�Y���g���ăX�N���v�g��
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationObject : MonoBehaviour
{
    // - �ϐ��錾 -

    // �P��̃p�[�����m�C�Y�����i�[����\����
    [System.Serializable]
    private struct NoiseParam
    {
        // �U��
        public float amplitude;

        // �U���̑���
        public float speed;

        // �p�[�����m�C�Y�̃I�t�Z�b�g
        [System.NonSerialized] public float offset;

        // �����̃I�t�Z�b�g�l���w�肷��
        public void SetRandomOffset()
        {
            offset = UnityEngine.Random.Range(0f, 256f);
        }

        // �w�莩���̃p�[�����m�C�Y�l���擾����
        public float GetValue(float time)
        {
            // �m�C�Y�ʒu���v�Z
            var noisePos = speed * time + offset;

            // -1����1�͈̔͂̃m�C�Y�l���擾
            var noiseValue = 2 * (Mathf.PerlinNoise(noisePos, 0) - 0.5f);

            // �U�����|�����l��Ԃ�
            return amplitude * noiseValue;
        }
    }

    // �p�[�����m�C�Y��XY���
    [System.Serializable]
    private struct NoiseTransform
    {
        public NoiseParam x, y;

        // xy�����ɗ����̃I�t�Z�b�g�l���w�肷��
        public void SetRandomOffset()
        {
            x.SetRandomOffset();
            y.SetRandomOffset();
        }

        // �w�莞���̃p�[�����m�C�Y���擾����
        public Vector3 GetValue(float time)
        {
            return new Vector3(
                x.GetValue(time),
                y.GetValue(time),
                0
                );
        }
    }

    // �ʒu�̗h����
    [SerializeField] private NoiseTransform _noisePosition;

    // ���g�̈ʒu���
    private Transform thisTransform;

    // Transform�̏������
    private Vector3 initLocalPosition;

    // �U������
    private bool Vibration = false;

    // �U�������鎞��
    private float VibrationTime = 0.0f;

    // �U�����n�܂�������
    private float StartVibrationTime = 0.0f;

    void Awake()
    {
        thisTransform = transform;

        // Transform�̏����l��ێ�
        initLocalPosition = thisTransform.localPosition;

        // �p�[�����m�C�Y�̃I�t�Z�b�g������
        _noisePosition.SetRandomOffset();
    }

    // Update is called once per frame
    void Update()
    {
        // �w�莞�Ԃ��o�߂�����
        if (Time.time - StartVibrationTime > VibrationTime)
        {
            if (Vibration == true)
            {
                // �U���I��
                Vibration = false;

                // �U�����I������珉���ʒu�ɖ߂�
                thisTransform.localPosition = initLocalPosition;
            }
        }

        // �U�����߂�����Έ�莞�ԐU��
        if (Vibration)
        {
            // �Q�[���J�n����̎��Ԏ擾
            var time = Time.time;

            // �p�[�����m�C�Y�̒l����������擾
            var noisePos = _noisePosition.GetValue(time);

            Debug.Log(initLocalPosition);
            Debug.Log(noisePos);

            // �eTransform�Ƀp�[�����m�C�Y�̒l�����Z
            thisTransform.localPosition = new Vector3(initLocalPosition.x + noisePos.x, initLocalPosition.y + noisePos.y, initLocalPosition.z);
        }
    }

    // �֐��Ăяo���Ŏw�莞�ԐU��������
    public void SetVibration(float time)
    {
        initLocalPosition = thisTransform.localPosition;

        Vibration = true; // �U�����߃Z�b�g
        VibrationTime = time; // �U�����Ԃ��Z�b�g
        StartVibrationTime = Time.time; // �U���J�n���Ԃ��Z�b�g
    }

    // �U��������Ԃ�
    public bool GetVibration()
    {
        return Vibration;
    }
}