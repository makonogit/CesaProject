//----------------------------------------------------------
// �S���ҁF���쒼�o
// ���e  �F�^���炽�ʒu����Ђт����
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackCreater : MonoBehaviour
{
    //-----------------------------------------------------------------
    //�\�ϐ��\(��)Accessible variables

    // ���
    public enum CrackCreaterState
    {
        NONE,       // �����Ȃ�
        START,      // �쐬�J�n
        CREATING,   // �쐬��
        CRAETED,    // �쐬����
    }
    [System.NonSerialized]// ��\��
    public CrackCreaterState State; // �O���{���p

    [System.NonSerialized]// ��\��
    public EdgeCollider2D Edge2D;

    //-----------------------------------------------------------------
    //�\�ϐ��\(��)Inaccessible variables
    [SerializeField]
    private CrackCreaterState _nowState;// ���̏�Ԃ�����ϐ�
    
    [Header("�Ђт�obj")]
    [SerializeField]
    private GameObject _crackObject;

    [Header("�������鐔�͈̔�(X�`Y)")]
    [SerializeField]
    private Vector2Int _divisionNum;

    [Header("�M�U�M�U�͈̔�(0.0f�`)")]
    [SerializeField]
    private Vector2 _rangeNum;

    [SerializeField]
    private List<Vector2> _nailPoints;// �B�̍��W���X�g
    [SerializeField]
    private List<Vector2> _edgePoints;// �ӂ̍��W���X�g
    [SerializeField]
    private List<int> _nailPointCount;
    //-----------------------------------------------------------------
    //�\�X�^�[�g�����\
    void Start()
    {
        //--------------------------------------
        // �G�b�W�R���C�_�[2D�������Ă��邩
        Edge2D = GetComponent<EdgeCollider2D>();
        if(Edge2D == null) 
        {
            Debug.LogError("EdgeCollider2D���R���|�[�l���g����Ă܂���B");
        }

        //--------------------------------------
        // �B�̍��W���X�g�ɒl�������Ă��邩
        if (_nailPoints == null) 
        {
            Debug.LogError("�B�̍��W���X�g���n����Ă܂���B");
            _nowState = CrackCreaterState.NONE;
        }
        else 
        {
            _nowState = CrackCreaterState.START;
        }
        // ��Ԃ����L����
        State = _nowState;
    }


    //-----------------------------------------------------------------
    //�\�X�V�����\
    void Update()
    {
        //--------------------------------------
        // ��Ԃ��쐬�J�n
        if (_nowState == CrackCreaterState.START )
        {
            EdgeSetting();//�G�b�W�̐ݒ�
            _nowState = CrackCreaterState.CREATING;
        }

        //--------------------------------------
        // ��Ԃ��쐬��(���o����)
        if (_nowState == CrackCreaterState.CREATING)
        {

        }
        // ��Ԃ����L����
        State = _nowState;
    }


    //-------------------------------------------------------
    //�\�B���W���X�g�ݒ�֐��\(��)
    public void SetPointList(List<Vector2> _pointList) 
    {
        _nailPoints = _pointList;
    }

    //-------------------------------------------------------
    //�\�G�b�W�ݒ�֐��\(��)
    private void EdgeSetting()
    {
        //--------------------------------------
        // �ӂ̒��_��ݒ肷��
        for (int i = 0; i < _nailPoints.Count; i++)// �B�̐��J��Ԃ�
        {
            // �B�̍��W��ǉ�
            _edgePoints.Add(_nailPoints[i]);
            // �Ō�̍��W�łȂ����
            if (i != _nailPoints.Count-1) 
            {
                // ��������
                DivisionPositionSetting(i);
            }
        }

        //--------------------------------------
        // 2���_�̊ԂɃ|�C���g��u��
        for (int i = 0; i < _edgePoints.Count-1; i++)
        {
            // ���ԍ��W�����߂�
            Vector2 _center = (_edgePoints[i] + _edgePoints[i + 1])/2;
            Vector3 _point = new Vector3(_center.x, _center.y,0);
            
            // �Ăяo��
            GameObject obj = Instantiate(_crackObject,_point,Quaternion.identity,transform);
            
            // ��̓B���琂���Ȋp�x�����߂�
            Vector2 _vec = _edgePoints[i] - _edgePoints[i+1];
            float _angle = Mathf.Atan2(_vec.y, _vec.x)*Mathf.Rad2Deg;
            // �p�x�ݒ�
            obj.transform.eulerAngles = new Vector3(0, 0, _angle);
            // �T�C�Y�ݒ�
            obj.transform.localScale = new Vector3( _vec.magnitude, obj.transform.localScale.y, obj.transform.localScale.z);
        }
        // ���_��ݒ肷��
        Edge2D.SetPoints(_edgePoints);
    }

    //-------------------------------------------------------
    //�\�����ʒu�ݒ�֐��\(��)
    private void DivisionPositionSetting(int _num) 
    {
        if(_num == 0) 
        {
            _nailPointCount.Add(_num);
        }
        _nailPointCount.Add(_nailPointCount[_num] + 1);

        // ��̓B���琂���Ȋp�x�����߂�
        Vector2 _vec = _nailPoints[_num + 1] - _nailPoints[_num];
        float _angle = Mathf.Atan2(_vec.y, _vec.x) ;
        _angle += (90 * Mathf.Deg2Rad);
        // �����x�N�g��
        Vector2 _verticalVec = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle));

        // �����������肷��
        int _division = Random.Range(_divisionNum.x, _divisionNum.y);
        //--------------------------------------
        // �������钸�_���J��Ԃ�
        for (int j = 1; j < _division; j++)
        {
            //��Ȃ�|�����Ȃ�{
            float _odd = (j % 2 != 0 ? -1.0f : 1.0f);
            // ���������߂�
            float _percent = (float)j / ((float)_division);
            // �Ԃ̍��W�����߂�
            Vector2 _pos = _nailPoints[_num] * (1.0f - _percent);
            _pos += _nailPoints[_num + 1] *  _percent;
            if (j != 1 && j != _division - 1)// �ŏ��ƍŌ�ȊO
            {
                _pos += _verticalVec * _odd * Random.Range(_rangeNum.x, _rangeNum.y);
            }
            // �Ԃ�ǉ�
            _edgePoints.Add(_pos);
            // _edgePoins�̈ʒu���v�Z����
            _nailPointCount[_num +1] += 1;
        }
    }
}
