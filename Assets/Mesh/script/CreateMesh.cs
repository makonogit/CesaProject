//-----------------------------------
//�S���F��{��
//���e�F���b�V���쐬
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour
{
    // �ϐ��錾
    //[SerializeField] private int _materialNumber;
    //[SerializeField] private Material _material;

    [SerializeField] MeshFilter _meshFilter;     //���b�V���t�B���^�[
    [SerializeField] MeshRenderer _meshRenderer; // ���b�V�������_���[

    [Header("�����ł͎l�p�`���`�悳���悤�ݒ�ς�")]
    // �����l(z���W�͓K��)
    // 0 : -5, 5, 0
    // 1 :  5, 5, 0
    // 2 :  5,-5, 0
    // 3 : -5,-5, 0
    [SerializeField] Vector3[] Verts;
    
    // �����l
    // 0 : 0
    // 1 : 1
    // 2 : 3
    // 3 : 1
    // 4 : 2
    // 5 : 3
    [SerializeField] int[] Triangles; // Verts�i���_�z��j�̃C���f�b�N�X

    // Start is called before the first frame update
    void Start()
    {
        // ���ɂ�����K�v����������

        //-----------------------------------------------------------------------------
        // �쐬���������b�V���̏����Z�b�g����i���_�ʒu�A�O�p�`�A�@���j
        // ���_�A�O�p�`�̓C���X�y�N�^�[�r���[�Őݒ肵�Ă�

        // Mesh�N���X
        Mesh mesh = new Mesh();
        // ���_�����Z�b�g
        mesh.vertices = Verts;
        // �O�p�`�����Z�b�g
        mesh.triangles = Triangles;
        // �����悭�킩��񂯂Ǒ����@��
        mesh.RecalculateNormals();

        //---------------------------------------------------------------------------------
        //�]���ŏ����Ƃ�������

        // ���b�V���t�B���^�[�ɂ������Z�b�g��������n��
        _meshFilter.sharedMesh = mesh;
        // �^�����}�e���A�����Z�b�g����
        //_meshRenderer.sharedMaterial = _material;
        // ���b�V�������_���[�ɃZ�b�g���ꂽ�}�e���A���̒�����w�肳�ꂽ�}�e���A�����Z�b�g����
        //_meshRenderer.sharedMaterial = _meshRenderer.materials[_materialNumber]; // �ł��Ȃ�����
    }
}
