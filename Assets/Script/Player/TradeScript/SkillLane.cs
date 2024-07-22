using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

[RequireComponent(typeof(SpriteRenderer))]
public class SkillLane : MonoBehaviour, I_SelectedLane
{
    PlayerData playerData;

    public bool canSelect;


    ISkill ITradeSkill;
    ISkill NullSkill; //Error�p,��p�̃X�L��

    [SerializeField] SkillManager skillManager;

    [SerializeField] float effectDuration;

    [SerializeField] GameObject iconGameObject;
    SpriteRenderer iconSpriteRenderer;
    SlideGameobject iconSlideSc;
    FadeInOut iconFedeSc;
    Vector2 defaultPosi;
    [SerializeField] Vector2 unsetDistance  = new Vector2(0, 40);
    [SerializeField] Vector2 highlightDistance = new Vector2(-72, 0);
    [SerializeField] Vector2 dicadePosi;

    [SerializeField] GameObject contextGameObject;
    [SerializeField] GameObject nameGameObject;
    RectTransform contextRect;
    RectTransform nameRect;
    TMP_Text contextTMP;
    TMP_Text nameTMP;
    SlideUIObject contextSlideSc;
    SlideUIObject nameSlideSc;
    FadeInOut contextFedeSc;
    FadeInOut nameFadeSc;
    Vector2 contextDefaultPosi;
    Vector2 nameDefaultPosi;
    [SerializeField] Vector2 contextsHighlightDistance = new Vector2(0, -40);

    [SerializeField] GameObject speedGameObject;
    RectTransform speedRect;
    TMP_Text speedTMP;
    SlideUIObject speedSlideSc;
    FadeInOut speedFedeSc;
    Vector2 speedDefaultPosi;
    [SerializeField] Vector2 speedUnsetDistance = new Vector2(0, 40);

    [SerializeField] GameObject decadeGameObject;
    TMP_Text decadeTMP;
    FadeInOut decadeFadeSc;

    RunTaskList RunHighlights;
    RunTaskList runTaskOther;

    List<Func<Task>> SetEffects;
    List<Func<Task>> HighlightEffects;
    List<Func<Task>> UnHightlightEffects;
    List<Func<Task>> DecadeEffects;
    List<Func<Task>> UnSetEffects;

    CancellationTokenSource highlightCancellationTokenSource;

    Color cleanness = new Color(255, 255, 255, 0);

    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
        skillManager = SkillManager.instance;

        NullSkill = skillManager.NullSkill; //NullSkill���擾
        ITradeSkill = NullSkill; //nullSkill���r�������ď�����

        iconSpriteRenderer = iconGameObject.GetComponent<SpriteRenderer>();
        iconSlideSc = iconGameObject.GetComponent<SlideGameobject>();
        iconFedeSc = iconGameObject.GetComponent<FadeInOut>();
        defaultPosi = iconGameObject.transform.position;

        contextTMP = contextGameObject.GetComponent<TMP_Text>();
        nameTMP = nameGameObject.GetComponent<TMP_Text>();
        contextRect = contextGameObject.GetComponent<RectTransform>();
        nameRect = nameGameObject.GetComponent<RectTransform>();
        contextSlideSc = contextGameObject.GetComponent<SlideUIObject>();
        nameSlideSc = nameGameObject.GetComponent<SlideUIObject>();
        contextFedeSc = contextGameObject.GetComponent<FadeInOut>();
        nameFadeSc = nameGameObject.GetComponent<FadeInOut>();
        contextDefaultPosi = contextRect.anchoredPosition;
        nameDefaultPosi = nameRect.anchoredPosition;

        speedTMP = speedGameObject.GetComponent<TMP_Text>();
        speedRect = speedGameObject.GetComponent<RectTransform>();
        speedSlideSc = speedGameObject.GetComponent<SlideUIObject>();
        speedFedeSc = speedGameObject.GetComponent<FadeInOut>();
        speedDefaultPosi = speedRect.anchoredPosition;

        decadeTMP = decadeGameObject.GetComponent<TMP_Text>();
        decadeFadeSc = decadeGameObject.GetComponent<FadeInOut>();


        SetTaskLists(); //List�̓��e������

        SetNutral(); //�t�h�Ȃǂ̏�Ԃ�������

        RunHighlights = new RunTaskList();
        runTaskOther = new RunTaskList();
    }

    Skill TradeSkill()
    {
        if(ITradeSkill == null) {  return NullSkill.SkillData(); }
        return ITradeSkill.SkillData();
    }

    public void SetSkill()
    {
        ITradeSkill = skillManager.RandomSelectSkill(); //�����_���łr�����������k����������Skill������
        if (ITradeSkill == null) { ITradeSkill = NullSkill; } //�����ł��Ȃ�����������NullSkill
        canSelect = playerData.baseHP > TradeSkill().cost;�@//�����X�s�[�h�ƃR�X�g���r���āA�I���ł��邩
        if(ITradeSkill == NullSkill) { canSelect = false; } //nullskill�̏ꍇ�I���ł��Ȃ�
        SetEffect();
        if (!canSelect) { cantSelectEffect(); } //���o�I�ɑI���ł��Ȃ���Ԃ����
    }

    public void ReSetSkill()
    {
        ITradeSkill = NullSkill;
        ReSetEffect();
    }

    public void SelectedAction() //�I�����ꂽ����Action
    {
        HighlightEffect();

    }

    public void UnSelectedAction()  //�I�����O���ꂽ����Action
    {
        UnHighlightEffect();

    }

    public void DecadedAction()
    {
        Debug.Log(TradeSkill().skillName + "���I�΂�܂���");
        playerData.baseHP -= TradeSkill().cost;
        Debug.Log(TradeSkill().cost + "��spped���������܂�");
        skillManager.AddDecadedSkill(ITradeSkill);
        DecadeEffect();
    }

    public void UnDecadedAction() //�I�΂�Ȃ������Ƃ�
    {
        if (ITradeSkill != NullSkill) { skillManager.ReAddSkill(ITradeSkill); }
        UnDecadeEffect();
    }

    void SetEffect()
    {
        SetSkillUI(TradeSkill());
        runTaskOther.EffectAnim(SetEffects, UnSetEffects);
    }


    void ReSetEffect()
    {
        runTaskOther.EffectAnim(UnSetEffects, SetEffects);
        //SetNutral();
    }

    void HighlightEffect()
    {
        highlightCancellationTokenSource = new CancellationTokenSource();
        RunHighlights.EffectAnim(HighlightEffects, UnHightlightEffects, highlightCancellationTokenSource);
    }

    void UnHighlightEffect()
    {
        highlightCancellationTokenSource = new CancellationTokenSource();
        RunHighlights.EffectAnim(UnHightlightEffects, HighlightEffects, highlightCancellationTokenSource);
    }

    void DecadeEffect()
    {
        runTaskOther.EffectAnim(DecadeEffects, UnSetEffects);
    }

    void UnDecadeEffect()
    {
        runTaskOther.EffectAnim(UnSetEffects, SetEffects);
    }

    void SetSkillUI(Skill skill)
    {
        iconSpriteRenderer.sprite = skill.Icon;
        nameTMP.SetText(skill.skillName);
        contextTMP.SetText(skill.skillexlantion);
        speedTMP.SetText(skill.cost.ToString());
    }

    void SetNutral() //�t�h�Ȃǂ̏�Ԃ�������
    {
        iconSpriteRenderer.color = cleanness;
        contextTMP.color = cleanness;
        nameTMP.color = cleanness;
        speedTMP.color = cleanness;
        decadeTMP.color = cleanness;

        iconGameObject.transform.position = defaultPosi + unsetDistance;
        contextRect.anchoredPosition = contextDefaultPosi + contextsHighlightDistance;
        nameRect.anchoredPosition = nameDefaultPosi + contextsHighlightDistance;
        speedRect.anchoredPosition = speedDefaultPosi + speedUnsetDistance;

        ITradeSkill = NullSkill;

        SetSkillUI(NullSkill.SkillData());
    }

    void cantSelectEffect()
    {
        Debug.Log("�I�ׂȂ���I");
        speedTMP.color = new Color(255, 0, 0, 255);
    }

    void SetTaskLists()
    {
        SetEffects = new List<Func<Task>>()
        {
            () => iconSlideSc.MoveGameObjectToPosition(defaultPosi, effectDuration),
            () => iconFedeSc.FedeObject(0, 1, effectDuration),
            () => speedSlideSc.MoveUIObjectToPosition(speedDefaultPosi, effectDuration),
            () => speedFedeSc.FedeObject(0, 1, effectDuration)
        };


        HighlightEffects = new List<Func<Task>>()
        {
            () => iconSlideSc.MoveGameObjectToPosition(defaultPosi + highlightDistance, effectDuration, highlightCancellationTokenSource.Token),
            () => contextSlideSc.MoveUIObjectToPosition(contextDefaultPosi, effectDuration, highlightCancellationTokenSource.Token),
            () => nameSlideSc.MoveUIObjectToPosition(nameDefaultPosi, effectDuration, highlightCancellationTokenSource.Token),
            () => contextFedeSc.FedeObject(0, 1, effectDuration, highlightCancellationTokenSource.Token),
            () => nameFadeSc.FedeObject(0, 1, effectDuration, highlightCancellationTokenSource.Token),
            () => decadeFadeSc.FedeObject(0, 1, effectDuration, highlightCancellationTokenSource.Token)
        };


        UnHightlightEffects = new List<Func<Task>>()
        {
            () => iconSlideSc.MoveGameObjectToPosition(defaultPosi, effectDuration, highlightCancellationTokenSource.Token),
            () => contextSlideSc.MoveUIObjectToPosition(contextDefaultPosi + contextsHighlightDistance, effectDuration, highlightCancellationTokenSource.Token),
            () => nameSlideSc.MoveUIObjectToPosition(nameDefaultPosi + contextsHighlightDistance, effectDuration, highlightCancellationTokenSource.Token),
            () => contextFedeSc.FedeObject(1, 0, effectDuration, highlightCancellationTokenSource.Token),
            () => nameFadeSc.FedeObject(1, 0, effectDuration, highlightCancellationTokenSource.Token),
            () => decadeFadeSc.FedeObject(1, 0, effectDuration, highlightCancellationTokenSource.Token)
        };



        DecadeEffects = new List<Func<Task>>()
        {
            () => iconSlideSc.MoveGameObjectToPosition(dicadePosi, effectDuration),
            () => iconFedeSc.FedeObject(1, 0, effectDuration),
            () => contextSlideSc.MoveUIObjectToPosition(contextDefaultPosi + contextsHighlightDistance, effectDuration, highlightCancellationTokenSource.Token),
            () => nameSlideSc.MoveUIObjectToPosition(nameDefaultPosi + contextsHighlightDistance, effectDuration, highlightCancellationTokenSource.Token),
            () => contextFedeSc.FedeObject(1, 0, effectDuration, highlightCancellationTokenSource.Token),
            () => nameFadeSc.FedeObject(1, 0, effectDuration, highlightCancellationTokenSource.Token),
            () => speedSlideSc.MoveUIObjectToPosition(speedDefaultPosi + speedUnsetDistance, effectDuration),
            () => speedFedeSc.FedeObject(1, 0, effectDuration),
            () => decadeFadeSc.FedeObject(1, 0, effectDuration)
        };


        UnSetEffects = new List<Func<Task>>()
        {
            () => iconSlideSc.MoveGameObjectToPosition(defaultPosi + unsetDistance, effectDuration),
            () => iconFedeSc.FedeObject(1, 0, effectDuration),
            () => speedSlideSc.MoveUIObjectToPosition(speedDefaultPosi + speedUnsetDistance, effectDuration),
            () => speedFedeSc.FedeObject(1, 0, effectDuration)
        };

    }
}