using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InterfaceMediater<TInterface>
{
    //Monobehavior���p�������C���^�[�t�F�[�X��[SerializeFiled]�ł킽��
    //https://qiita.com/kaichi_tozawa/items/fa2327936d7e9d2e748f

    [SerializeField] GameObject _objectI;
    TInterface _interface;
    public TInterface Interface()
    {
        if (_objectI)
        {
            _objectI.TryGetComponent<TInterface>(out _interface);
        }
        return _interface;
    }
}

