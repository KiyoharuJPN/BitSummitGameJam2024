using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SkillLane : MonoBehaviour ,I_SelectedLane
{
    Skill TradeSkill; //�I�����p�X�L���̕ێ��p
    Skill NullSkill; //Error�p,��p�̃X�L��

    Image SkillIcon;
    string SkillContext;
    RectTransform imageRectTransform; // �ړ�������Image��RectTransform
    public Vector2 defaultposi; // �ړ��J�n�ʒu
    public Vector2 HighLightposi; // �ړ��I���ʒu
    private CancellationTokenSource _cancellationTokenSource;
    private Task _currentTask;

    void Start()
    {
        SkillIcon = TradeSkill.Icon;
        imageRectTransform = SkillIcon.rectTransform;
        SkillContext = SkillContext.ToString();
    }

    void SetSkill(Skill skill)
    {
        TradeSkill = skill;
        SetEffect();
    }

    void ReSetSkill(Skill skill)
    {
        TradeSkill = NullSkill;
        ReSetEffect();
    }

    public void SelectedAction() //�I�����ꂽ����Action
    {
        ReverseOrMoveBack();
    }

    public void UnSelectedAction ()  //�I�����O���ꂽ����Action
    {
        ReverseOrMoveBack();
    }

    public void DecadedAction()
    {
        Debug.Log(TradeSkill + "���I�΂�܂���");
        Debug.Log(TradeSkill.cost + "��spped���������܂�");
    }

    void SetEffect()
    {

    }

    void ReSetEffect()
    {

    }

    void DecadeEffect()
    {

    }

    void ReverseOrMoveBack()
    {
        // �A�j���[�V�������Ȃ�L�����Z�����ċt�Đ��A�����łȂ��Ȃ�t�����ɍĐ�
        if (_currentTask != null && !_currentTask.IsCompleted)
        {
            StopImageMovement();
        }
        else
        {
            if (imageRectTransform.anchoredPosition == HighLightposi)
            {
                HighlightEffect();
            }
            else if (imageRectTransform.anchoredPosition == defaultposi)
            {
                UnHighlightEffect();
            }
        }
    }


    public async void MoveImage(Vector2 start, Vector2 end, float duration)
    {
        // �����̈ړ��A�j���[�V�������L�����Z��
        if (_currentTask != null && !_currentTask.IsCompleted)
        {
            _cancellationTokenSource?.Cancel();
            await _currentTask; // �L�����Z��������҂�
            await MoveRectTransformToPosition(imageRectTransform, imageRectTransform.anchoredPosition, start, duration); // �t�Đ�
        }
        else
        {
            _cancellationTokenSource?.Cancel();

            // �V����CancellationTokenSource���쐬
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                _currentTask = MoveRectTransformToPosition(imageRectTransform, start, end, duration, _cancellationTokenSource.Token);
                await _currentTask;
            }
            catch (TaskCanceledException)
            {
                // �A�j���[�V�������L�����Z�����ꂽ�ꍇ�̏���
                Debug.Log("Image movement was canceled.");
                await MoveRectTransformToPosition(imageRectTransform, imageRectTransform.anchoredPosition, start, duration);
            }
        }
    }

    private async Task MoveRectTransformToPosition(RectTransform element, Vector2 start, Vector2 end, float duration, CancellationToken cancellationToken = default)
    {
        float distance = Vector2.Distance(start, end);
        float speed = distance / duration;

        for(float elapsed = 0f  +Time.deltaTime; elapsed < duration; elapsed += Time.deltaTime)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                // �L�����Z�����ꂽ�ꍇ�ɗ�O�𓊂���
                cancellationToken.ThrowIfCancellationRequested();
            }

            float t = Mathf.Clamp01(elapsed * speed / distance);
            element.anchoredPosition = Vector2.Lerp(start, end, t);
            await Task.Yield(); // �t���[���̏I���܂őҋ@
        }

        element.anchoredPosition = end; // �ŏI�ʒu���m��
    }

    void StopImageMovement()
    {
        // �O������A�j���[�V�������L�����Z��
        _cancellationTokenSource?.Cancel();
    }

    void HighlightEffect()
    {
        MoveImage(defaultposi, HighLightposi, 0.5f);
    }

    void UnHighlightEffect()
    {
        MoveImage(HighLightposi, defaultposi, 0.5f);
    }
}
