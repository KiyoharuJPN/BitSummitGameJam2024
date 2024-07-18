using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullSkill : MonoBehaviour, ISkill
{
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(0, "�Ȃ�", "�X�L���@���@����܂���", 0, 1, skillIcon, Skill.SkillType.NullSkill);

    public void RunStartActionScene()
    {
        Debug.Log("NullSkill���v���C���[�ɃA�^�b�`����Ă��܂�");
    }
}

