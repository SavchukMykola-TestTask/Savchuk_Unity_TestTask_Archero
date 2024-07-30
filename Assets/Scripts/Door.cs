using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float Amplitude;
    [SerializeField] private float Duration;

    private bool IsOpened = false;
    private bool IsOpeningNow = false;

    public void Open()
    {
        if (IsOpeningNow) return;

        StartCoroutine(OpenCoroutine());
    }

    private IEnumerator OpenCoroutine()
    {
        IsOpeningNow = true;

        for (float time = 0; time <= Duration; time += Time.deltaTime)
        {
            transform.rotation *= Quaternion.AngleAxis((IsOpened ? -1 : 1) * Time.deltaTime * Amplitude * (1 / Duration), Vector3.up);
            yield return new WaitForEndOfFrame();
        }

        IsOpened = !IsOpened;

        IsOpeningNow = false;
    }
}