using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public class StartScene : MonoBehaviour
{
    enum Select
    {
        start, option
    }

    Select select;

    RunTaskList runTaskList;

    [SerializeField] GameObject startObject;
    RectTransform startRect;
    SlideUIObject startSlideSc;
    Vector2 startDefault;

    [SerializeField] GameObject optionObject;
    RectTransform optionRect;
    SlideUIObject optionSlideSc;
    Vector2 optionDefault;

    Vector2 diffacence = new Vector2(-50, 0);
    float effectDuration = 0.05f;

    List<Func<Task>> startEffect;
    List<Func<Task>> optionEffect;

    CancellationTokenSource cancellationTokenSource;


    // Start is called before the first frame update
    void Start()
    {
        startRect = startObject.GetComponent<RectTransform>();
        startSlideSc = startObject.GetComponent<SlideUIObject>();
        startDefault = startRect.anchoredPosition;

        optionRect = optionObject.GetComponent<RectTransform>();
        optionSlideSc = optionObject.GetComponent<SlideUIObject>();
        optionDefault = optionRect.anchoredPosition;


        startEffect = new List<Func<Task>>()
        {
            () => startSlideSc.MoveUIObjectToPosition(startDefault + diffacence, effectDuration, cancellationTokenSource.Token),
            () => optionSlideSc.MoveUIObjectToPosition(optionDefault, effectDuration, cancellationTokenSource.Token),
        };

        optionEffect = new List<Func<Task>>()
        {
            () => startSlideSc.MoveUIObjectToPosition(startDefault, effectDuration, cancellationTokenSource.Token),
            () => optionSlideSc.MoveUIObjectToPosition(optionDefault + diffacence, effectDuration, cancellationTokenSource.Token),
        };


        runTaskList = new RunTaskList();
    }

    void DecadeStart()
    {
        SceneManager.LoadScene("ActionStage");
    }

    void DecadeOption()
    {

    }

    public void PushStartButton()
    {
        DecadeStart();
    }

    public void PushHowToPlayButton()
    {
        DecadeOption();
    }

    public void OnUp()
    {
        select = Select.start;
        cancellationTokenSource = new CancellationTokenSource();
        runTaskList.EffectAnim(startEffect, optionEffect, cancellationTokenSource);

    }

    public void OnDown()
    {
        select = Select.option;
        cancellationTokenSource = new CancellationTokenSource();
        runTaskList.EffectAnim(optionEffect, startEffect, cancellationTokenSource);
    }
    public void OnDecade()
    {
        if (select == default) return;
        if (select == Select.start) { DecadeStart(); return; } 
        if (select == Select.option) {  DecadeOption(); return; }
    }
}
