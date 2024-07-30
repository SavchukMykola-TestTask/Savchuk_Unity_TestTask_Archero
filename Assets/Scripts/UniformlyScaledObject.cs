using UnityEngine;
using System;
using System.Collections;

public class UniformlyScaledObject : MonoBehaviour
{
    [SerializeField] private Vector3 DeltaScale;
    [SerializeField] private float Duration;

    public void ChangeScale(Action afterFinishingScaling = null)
    {
        StartCoroutine(ScaleUniformly(DeltaScale, Duration, afterFinishingScaling));
    }

    private IEnumerator ScaleUniformly(Vector3 deltaScale, float duration, Action afterFinishingScaling)
    {
        float Duration = duration;
        Vector3 StartScale = transform.localScale;
        Vector3 TargetScale = StartScale + deltaScale;
        float ElapsedTime = 0;
        while(ElapsedTime < Duration)
        {
            transform.localScale = Vector3.Lerp(StartScale, TargetScale, ElapsedTime / Duration);
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = TargetScale;
        afterFinishingScaling?.Invoke();
    }
}