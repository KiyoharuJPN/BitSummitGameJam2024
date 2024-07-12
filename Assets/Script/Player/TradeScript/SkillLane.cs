using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]

public class SkillLane : MonoBehaviour, I_SelectedLane
{
    Skill TradeSkill; //�I�����p�X�L���̕ێ��p
    Skill NullSkill; //Error�p,��p�̃X�L��


    [SerializeField] SkillManage skillManage;

    [SerializeField] float effectDuration;

    [SerializeField] GameObject iconGameObject;
    SpriteRenderer iconSpriteRenderer;
    SlideGameobject iconSlideSc;
    FadeInOut iconFedeSc;
    Vector2 defaultPosi;
    [SerializeField] Vector2 unsetDistance;
    [SerializeField] Vector2 highlightDistance;
    [SerializeField] Vector2 dicadeDistance;

    [SerializeField] GameObject contextGameObject;
    [SerializeField] GameObject nameGameObject;
    TextMeshPro contextTMP;
    TextMeshPro nameTMP;
    SlideGameobject contextSlideSc;
    SlideGameobject nameSlideSc;
    FadeInOut contextFedeSc;
    FadeInOut nameFadeSc;
    Vector2 contextDefaultPosi;
    Vector2 nameDefaultPosi;
    [SerializeField] Vector2 contextsHighlightDistance;

    [SerializeField] GameObject speedGameObject;
    TextMeshPro speedTMP;
    SlideGameobject speedSlideSc;
    FadeInOut speedFedeSc;
    Vector2 speedDefaultPosi;
    [SerializeField] Vector2 speedUnsetDistance;


    List<Task> SetEffects = new List<Task>()
    {

    };

    List<Task> SetEffectsCancels = new List<Task>()
    {

    };


    List<Task> HighlihtEffects = new List<Task>()
    {
        
    };

    List<Task> HightligtEffectsCancels = new List<Task>()
    {

    };


    List<Task> UnHightlightEffects = new List<Task>()
    {

    };

    List<Task> UnHightlightEffectsCancels = new List<Task>()
    {

    };


    List<Task> DecadeEffects = new List<Task>()
    {

    };

    List<Task> DecadeEffectsCancels = new List<Task>()
    {

    };


    List<Task> UnSetEffects = new List<Task>()
    {

    };

    List<Task> UnSetEffectsCancels = new List<Task>()
    {

    };



    void Start()
    {
        NullSkill = skillManage.NullSkill;
        TradeSkill = NullSkill;

        iconSpriteRenderer = iconGameObject.GetComponent<SpriteRenderer>();
        iconSlideSc = iconSpriteRenderer.GetComponent<SlideGameobject>();
        iconFedeSc = iconSlideSc.GetComponent<FadeInOut>();
        defaultPosi = iconGameObject.transform.position;
        iconSpriteRenderer.enabled = false;

        contextTMP = contextGameObject.GetComponent<TextMeshPro>();
        nameTMP = nameGameObject.GetComponent<TextMeshPro>();
        contextSlideSc = contextGameObject.GetComponent<SlideGameobject>();
        nameSlideSc = nameGameObject.GetComponent<SlideGameobject>();
        contextFedeSc = contextGameObject.GetComponent<FadeInOut>();
        nameFadeSc = nameGameObject.GetComponent<FadeInOut>();
        contextDefaultPosi = contextGameObject.transform.position;
        nameDefaultPosi = nameGameObject.transform.position;
        contextTMP.enabled = false;
        nameTMP.enabled = false;

        speedTMP = speedGameObject.GetComponent<TextMeshPro>();
        speedSlideSc = speedGameObject.GetComponent<SlideGameobject>();
        speedFedeSc = speedGameObject.GetComponent<FadeInOut>();
        speedDefaultPosi = speedGameObject.transform.position;
        speedTMP.enabled = false;

    }

    public void SetSkill()
    {
        TradeSkill = skillManage.RandomSelectSkill();
        if ( TradeSkill == null ) { TradeSkill = NullSkill; } 
        SetEffect();
    }

    public void ReSetSkill()
    {
        TradeSkill = NullSkill;
        ReSetEffect();
    }

    public void SelectedAction() //�I�����ꂽ����Action
    {
        HighlightEffect();
        Debug.Log(TradeSkill.skillName + "��I��");
    }

    public void UnSelectedAction()  //�I�����O���ꂽ����Action
    {
        UnHighlightEffect();
        Debug.Log(TradeSkill.skillName + "��I������");
    }

    public void DecadedAction()
    {
        Debug.Log(TradeSkill.skillName + "���I�΂�܂���");
        Debug.Log(TradeSkill.cost + "��spped���������܂�");
        DecadeEffect();
    }

    public void UnDecadedAction() //�I�΂�Ȃ������Ƃ�
    {
        skillManage.ReAddSkill(TradeSkill);
        //UnDecadeEffext
    }


    void SetEffect()
    {
        iconSpriteRenderer.sprite = TradeSkill.Icon;
        iconSpriteRenderer.enabled = true;



    }

    void ReSetEffect()
    {
        iconSpriteRenderer.sprite = null;
        iconSpriteRenderer.enabled = false;


    }

    void HighlightEffect()
    {

    }

    void UnHighlightEffect()
    {

    }

    void DecadeEffect()
    {

    }

}
