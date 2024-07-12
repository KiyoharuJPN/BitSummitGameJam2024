using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public interface IUIEffect
{
    //使ってない

    Task AnimateEffect(float start, float end, float duration, CancellationToken cancellationToken = default);


    Task AnimateEffectCancel(float end, float duration);

    void StopAnimateEffect();

}

