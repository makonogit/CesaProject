//------------------------------
//�@�S��:�����S
//�@�G���A4�̃O���[�o��Light�ݒ�
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetStage stage = new SetStage();

        if(stage.GetAreaNum() == 3)
        {
            GetComponent<Light2D>().intensity = 0.0f;
        }
    }

}
