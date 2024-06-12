using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]

public class SkillLane : MonoBehaviour ,I_SelectedLane
{
    Skill TradeSkill; //�I�����p�X�L���̕ێ��p
    Skill NullSkill; //Error�p,��p�̃X�L��

    Sprite SkillIcon;
    string SkillContext;
    RectTransform imageRectTransform; // �ړ�������Image��RectTransform
    public Vector2 defaultposi; // �ړ��J�n�ʒu
    public Vector2 HighLightposi; // �ړ��I���ʒu
    private CancellationTokenSource _cancellationTokenSource;
    private Task _currentTask;

    Vector3 imagePosi;

    SpriteRenderer spriteRenderer;

    [SerializeField] SkillManage skillManage;

    void Start()
    {
        NullSkill = skillManage.NullSkill;

        TradeSkill = NullSkill;

        //imageRectTransform = SkillIcon.rectTransform;
        //SkillContext = SkillContext.ToString();

        imagePosi = transform.position; //�e�X�g�p��

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    public void SetSkill()
    {
        TradeSkill = skillManage.RandomSelectSkill();
        SetEffect();
    }

    public void ReSetSkill()
    {
        TradeSkill = NullSkill;
        ReSetEffect();
    }

    public void SelectedAction() //�I�����ꂽ����Action
    {
        //ReverseOrMoveBack();
        Debug.Log(TradeSkill.skillName + "��I��");
    }

    public void UnSelectedAction()  //�I�����O���ꂽ����Action
    {
        //ReverseOrMoveBack();
        Debug.Log(TradeSkill.skillName + "��I������");
    }

    public void DecadedAction()
    {
        Debug.Log(TradeSkill.skillName + "���I�΂�܂���");
        Debug.Log(TradeSkill.cost + "��spped���������܂�");
        //DecadeEffect
    }

    public void UnDecadedAction() //�I�΂�Ȃ������Ƃ�
    {
        skillManage.ReAddSkill(TradeSkill);
        //UnDecadeEffext
    }

    void SetEffect()
    {
        spriteRenderer.sprite = TradeSkill.Icon;
        spriteRenderer.enabled = true;
    }

    void ReSetEffect()
    {
        spriteRenderer.sprite = null;
        spriteRenderer.enabled = false;
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


    public async void MoveImage(Vector2 start, Vector2 end, float duration, RectTransform recttransform)
    {
        // �����̈ړ��A�j���[�V�������L�����Z��
        if (_currentTask != null && !_currentTask.IsCompleted)
        {
            _cancellationTokenSource?.Cancel();
            await _currentTask; // �L�����Z��������҂�
            await MoveRectTransformToPosition(recttransform, recttransform.anchoredPosition, start, duration); // �t�Đ�
        }
        else
        {
            _cancellationTokenSource?.Cancel();

            // �V����CancellationTokenSource���쐬
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                _currentTask = MoveRectTransformToPosition(recttransform, start, end, duration, _cancellationTokenSource.Token);
                await _currentTask;
            }
            catch (TaskCanceledException)
            {
                // �A�j���[�V�������L�����Z�����ꂽ�ꍇ�̏���
                Debug.Log("Image movement was canceled.");
                await MoveRectTransformToPosition(recttransform, recttransform.anchoredPosition, start, duration);
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
        MoveImage(defaultposi, HighLightposi, 0.5f, imageRectTransform);
    }

    void UnHighlightEffect()
    {
        MoveImage(HighLightposi, defaultposi, 0.5f, imageRectTransform);
    }

}
