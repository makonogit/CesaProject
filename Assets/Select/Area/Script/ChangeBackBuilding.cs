//---------------------------------------
//�S���ҁF��{
//���e�@�F�Z���N�g�w�i�̃I�u�W�F�N�g��؂�ւ���
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackBuilding : MonoBehaviour
{
    // �G���A�N���A���Ă�����true
    private bool clear = false;

    [SerializeField] private AreaCrack _areaCrack;
    [SerializeField] private GameObject _crystal;
    [SerializeField] private GameObject _normal;

    // Update is called once per frame
    void Update()
    {
        if(clear == false)
        {
            if (_areaCrack._isAreClear == true)
            {

                Debug.Log(_areaCrack);
                _crystal.SetActive(false);
                _normal.SetActive(true);
            }
            else
            {
                _crystal.SetActive(true);
                _normal.SetActive(false);
            }

            clear = true;
        }
    }
}
