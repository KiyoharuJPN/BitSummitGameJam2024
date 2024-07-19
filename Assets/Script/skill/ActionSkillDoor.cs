using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionSkillDoor : MonoBehaviour
{
    [SerializeField] SkillManager skillManager;

    private void Start()
    {
        skillManager = SkillManager.instance;
        if (SceneManager.GetActiveScene().name != "ActioStage") return;
        skillManager.SetSkillActionScene();
    }
}
