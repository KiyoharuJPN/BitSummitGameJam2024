using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]

public class SkillLane : MonoBehaviour ,I_SelectedLane
{
    Skill TradeSkill; //選択肢用スキルの保持用
    Skill NullSkill; //Error用,空用のスキル

    Sprite SkillIcon;
    string SkillContext;
    RectTransform imageRectTransform; // 移動させるImageのRectTransform
    public Vector2 defaultposi; // 移動開始位置
    public Vector2 HighLightposi; // 移動終了位置
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

        imagePosi = transform.position; //テスト用仮

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

    public void SelectedAction() //選択された時のAction
    {
        //ReverseOrMoveBack();
        Debug.Log(TradeSkill.skillName + "を選択");
    }

    public void UnSelectedAction()  //選択が外された時のAction
    {
        //ReverseOrMoveBack();
        Debug.Log(TradeSkill.skillName + "を選択解除");
    }

    public void DecadedAction()
    {
        Debug.Log(TradeSkill.skillName + "が選ばれました");
        Debug.Log(TradeSkill.cost + "がsppedから引かれます");
        //DecadeEffect
    }

    public void UnDecadedAction() //選ばれなかったとき
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
        // アニメーション中ならキャンセルして逆再生、そうでないなら逆方向に再生
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
        // 既存の移動アニメーションをキャンセル
        if (_currentTask != null && !_currentTask.IsCompleted)
        {
            _cancellationTokenSource?.Cancel();
            await _currentTask; // キャンセル完了を待つ
            await MoveRectTransformToPosition(recttransform, recttransform.anchoredPosition, start, duration); // 逆再生
        }
        else
        {
            _cancellationTokenSource?.Cancel();

            // 新しいCancellationTokenSourceを作成
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                _currentTask = MoveRectTransformToPosition(recttransform, start, end, duration, _cancellationTokenSource.Token);
                await _currentTask;
            }
            catch (TaskCanceledException)
            {
                // アニメーションがキャンセルされた場合の処理
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
                // キャンセルされた場合に例外を投げる
                cancellationToken.ThrowIfCancellationRequested();
            }

            float t = Mathf.Clamp01(elapsed * speed / distance);
            element.anchoredPosition = Vector2.Lerp(start, end, t);
            await Task.Yield(); // フレームの終わりまで待機
        }

        element.anchoredPosition = end; // 最終位置を確定
    }

    void StopImageMovement()
    {
        // 外部からアニメーションをキャンセル
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
