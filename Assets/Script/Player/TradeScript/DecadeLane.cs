using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;


public class DecadeLane : MonoBehaviour, I_SelectedLane
{
    [SerializeField] float effectDuration;

    [SerializeField] GameObject textgameObject;
    RectTransform textRect;
    TMP_Text TMP;
    FadeInOut textFadeSc;
    SlideUIObject textSlideSc;
    Vector2 defaultPosi;
    [SerializeField] Vector2 highlightDistance;
    [SerializeField] Vector2 decadeDistance;

    RunTaskList runhlTaskList;
    RunTaskList runTaskList;

    List<Func<Task>> HighlightEffects;
    List<Func<Task>> UnHightlightEffects;
    List<Func<Task>> DecadeEffects;

    CancellationTokenSource highlightCancellationTokenSource;


    void Start()
    {
        textRect = textgameObject.GetComponent<RectTransform>();
        TMP = textgameObject.GetComponent<TMP_Text>();
        textFadeSc = textgameObject.GetComponent<FadeInOut>();
        textSlideSc = textgameObject.GetComponent<SlideUIObject>();
        defaultPosi = textRect.anchoredPosition;

        TMP.color = new Color(255, 255, 255, 0);
        textRect.anchoredPosition = defaultPosi + highlightDistance;

        runhlTaskList = new RunTaskList();
        runTaskList = new RunTaskList();

        SetTaskLists();
    }

    public void SelectedAction()
    {
        highlightCancellationTokenSource = new CancellationTokenSource();
        runhlTaskList.EffectAnim(HighlightEffects, UnHightlightEffects, highlightCancellationTokenSource);
        Debug.Log("DecadeLaneを選択");
    }

    public void UnSelectedAction()
    {
        highlightCancellationTokenSource = new CancellationTokenSource();
        runhlTaskList.EffectAnim(UnHightlightEffects, HighlightEffects, highlightCancellationTokenSource);
        Debug.Log("DecadeLaneを選択解除");
    }

    public void DecadedAction()
    {
        runTaskList.EffectAnim(DecadeEffects, UnHightlightEffects);
        Debug.Log("Skill選択なしで続行");
    }

    public void UnDecadedAction()
    {
        runTaskList.EffectAnim(UnHightlightEffects, DecadeEffects);
    }

    void SetTaskLists()
    {

        HighlightEffects = new List<Func<Task>>()
        {
            () => textSlideSc.MoveUIObjectToPosition(defaultPosi, effectDuration, highlightCancellationTokenSource.Token),
            () => textFadeSc.FedeObject(0, 1, effectDuration, highlightCancellationTokenSource.Token)
        };


        UnHightlightEffects = new List<Func<Task>>()
        {
            () => textSlideSc.MoveUIObjectToPosition(defaultPosi + highlightDistance, effectDuration, highlightCancellationTokenSource.Token),
            () => textFadeSc.FedeObject(1, 0, effectDuration, highlightCancellationTokenSource.Token)
        };



        DecadeEffects = new List<Func<Task>>()
        {
            () => textSlideSc.MoveUIObjectToPosition(defaultPosi + decadeDistance, effectDuration, highlightCancellationTokenSource.Token),
            () => textFadeSc.FedeObject(1, 0, effectDuration, highlightCancellationTokenSource.Token)
        };


    }
}
