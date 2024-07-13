using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class SkillLane : MonoBehaviour, I_SelectedLane
{
    Skill TradeSkill; //選択肢用スキルの保持用
    Skill NullSkill; //Error用,空用のスキル

    [SerializeField] SkillManage skillManage;

    [SerializeField] float effectDuration;

    [SerializeField] GameObject iconGameObject;
    SpriteRenderer iconSpriteRenderer;
    SlideGameobject iconSlideSc;
    FadeInOut iconFedeSc;
    Vector2 defaultPosi;
    [SerializeField] Vector2 unsetDistance;
    [SerializeField] Vector2 highlightDistance = new Vector2(-72, 0);
    [SerializeField] Vector2 dicadePosi;

    [SerializeField] GameObject contextGameObject;
    [SerializeField] GameObject nameGameObject;
    TMP_Text contextTMP;
    TMP_Text nameTMP;
    SlideGameobject contextSlideSc;
    SlideGameobject nameSlideSc;
    FadeInOut contextFedeSc;
    FadeInOut nameFadeSc;
    Vector2 contextDefaultPosi;
    Vector2 nameDefaultPosi;
    [SerializeField] Vector2 contextsHighlightDistance;

    [SerializeField] GameObject speedGameObject;
    TMP_Text speedTMP;
    SlideGameobject speedSlideSc;
    FadeInOut speedFedeSc;
    Vector2 speedDefaultPosi;
    [SerializeField] Vector2 speedUnsetDistance;

    RunTaskList RunHighlights;

    List<Func<Task>> SetEffects;
    List<Func<Task>> SetEffectsCancels;
    List<Func<Task>> HighlightEffects;
    List<Func<Task>> HighligtEffectsCancels;
    List<Func<Task>> UnHightlightEffects;
    List<Func<Task>> UnHightlightEffectsCancels;
    List<Func<Task>> DecadeEffects;
    List<Func<Task>> DecadeEffectsCancels;
    List<Func<Task>> UnSetEffects;
    List<Func<Task>> UnSetEffectsCancels;

    CancellationTokenSource highlightCancellationTokenSource;

    void Start()
    {
        NullSkill = skillManage.NullSkill;
        TradeSkill = NullSkill;

        iconSpriteRenderer = iconGameObject.GetComponent<SpriteRenderer>();
        iconSlideSc = iconGameObject.GetComponent<SlideGameobject>();
        iconFedeSc = iconGameObject.GetComponent<FadeInOut>();
        defaultPosi = iconGameObject.transform.position;
        iconSpriteRenderer.enabled = false;

        contextTMP = contextGameObject.GetComponent<TMP_Text>();
        nameTMP = nameGameObject.GetComponent<TMP_Text>();
        contextSlideSc = contextGameObject.GetComponent<SlideGameobject>();
        nameSlideSc = nameGameObject.GetComponent<SlideGameobject>();
        contextFedeSc = contextGameObject.GetComponent<FadeInOut>();
        nameFadeSc = nameGameObject.GetComponent<FadeInOut>();
        contextDefaultPosi = contextGameObject.transform.position;
        nameDefaultPosi = nameGameObject.transform.position;
        contextTMP.enabled = false;
        nameTMP.enabled = false;

        speedTMP = speedGameObject.GetComponent<TMP_Text>();
        speedSlideSc = speedGameObject.GetComponent<SlideGameobject>();
        speedFedeSc = speedGameObject.GetComponent<FadeInOut>();
        speedDefaultPosi = speedGameObject.transform.position;
        speedTMP.enabled = false;

        SetTaskLists();

        RunHighlights = new RunTaskList();
    }

    public void SetSkill()
    {
        TradeSkill = skillManage.RandomSelectSkill();
        if (TradeSkill == null) { TradeSkill = NullSkill; }
        SetEffect();
    }

    public void ReSetSkill()
    {
        TradeSkill = NullSkill;
        ReSetEffect();
    }

    public void SelectedAction() //選択された時のAction
    {
        HighlightEffect();
        Debug.Log(TradeSkill.skillName + "を選択");
    }

    public void UnSelectedAction()  //選択が外された時のAction
    {
        UnHighlightEffect();
        Debug.Log(TradeSkill.skillName + "を選択解除");
    }

    public void DecadedAction()
    {
        Debug.Log(TradeSkill.skillName + "が選ばれました");
        Debug.Log(TradeSkill.cost + "がsppedから引かれます");
        DecadeEffect();
    }

    public void UnDecadedAction() //選ばれなかったとき
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
        highlightCancellationTokenSource = new CancellationTokenSource();
        RunHighlights.EffectAnim(GenerateTasks(HighlightEffects), GenerateTasks(HighligtEffectsCancels), highlightCancellationTokenSource);
        Debug.Log("hili");
    }

    void UnHighlightEffect()
    {
        highlightCancellationTokenSource = new CancellationTokenSource();
        RunHighlights.EffectAnim(GenerateTasks(UnHightlightEffects), GenerateTasks(UnHightlightEffectsCancels), highlightCancellationTokenSource);
    }

    void DecadeEffect()
    {
    }

    void SetTaskLists()
    {
        SetEffects = new List<Func<Task>>()
        {
        };

        SetEffectsCancels = new List<Func<Task>>()
        {
        };

        HighlightEffects = new List<Func<Task>>()
        {
            () => iconSlideSc.MoveGameObjectToPosition(defaultPosi + highlightDistance, effectDuration, highlightCancellationTokenSource.Token)
        };

        HighligtEffectsCancels = new List<Func<Task>>()
        {
            () => iconSlideSc.MoveGameObjectToPosition(defaultPosi, effectDuration, highlightCancellationTokenSource.Token)
        };

        UnHightlightEffects = new List<Func<Task>>()
        {
            () => iconSlideSc.MoveGameObjectToPosition(defaultPosi, effectDuration, highlightCancellationTokenSource.Token)
        };

        UnHightlightEffectsCancels = new List<Func<Task>>()
        {
            () => iconSlideSc.MoveGameObjectToPosition(defaultPosi + highlightDistance, effectDuration, highlightCancellationTokenSource.Token)
        };

        DecadeEffects = new List<Func<Task>>()
        {
        };

        DecadeEffectsCancels = new List<Func<Task>>()
        {
        };

        UnSetEffects = new List<Func<Task>>()
        {
        };

        UnSetEffectsCancels = new List<Func<Task>>()
        {
        };
    }

    List<Task> GenerateTasks(List<Func<Task>> taskFactories)
    {
        List<Task> tasks = new List<Task>();
        foreach (var taskFactory in taskFactories)
        {
            tasks.Add(taskFactory());
        }
        return tasks;
    }
}
