//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：クリアしたステージを管理するnewSelectScene
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ClearStageManager : MonoBehaviour
{

    private SaveData _saveData;
    [SerializeField,Header("エリア")]
    private List<AreaCrack> _areas;
    private static int _clearStages = 0;
    private static int _oldClearStages = 0;
    // Use this for initialization
    void Start()
    {
        _saveData = GetComponent<SaveData>();
        _clearStages = _saveData.Data.ClearStages;
        if (_oldClearStages / 6 != _clearStages / 6 && _oldClearStages % 6 == 5)
        {
            _oldClearStages++;
        }
        _areas[_clearStages / 6].Set(_oldClearStages % 6, _clearStages % 6);
        for (int i = 0; i < _areas.Count; i++) 
        {
            _areas[i].CrystalDisp(!(i < _oldClearStages / 6));
        }
    }

    private void OnDestroy()
    {
        _saveData.Data.ClearStages = _clearStages;
        _saveData.Save(_saveData.Data);
        _oldClearStages = _clearStages;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTest(InputAction.CallbackContext _context) 
    {
        // 押された瞬間
        if (_context.phase == InputActionPhase.Started)
        {
            _clearStages++;
            if(_clearStages < 30) 
            {
                if(_oldClearStages/6 != _clearStages / 6 && _oldClearStages % 6 == 5) 
                {
                    _oldClearStages++;
                }
                _areas[_clearStages / 6].Set(_oldClearStages % 6, _clearStages % 6);
                //Debug.Log("エリア"+(_clearStages / 6)+"前のデータ"+(_oldClearStages % 6)+"今のデータ"+( _clearStages % 6));
            }
            for (int i = 0; i < _areas.Count; i++)
            {
                _areas[i].CrystalDisp(!(i < _oldClearStages / 6));
            }
            _oldClearStages = _clearStages;
        }
    }

}