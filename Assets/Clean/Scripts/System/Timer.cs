using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private float sec;
    private int min;

    private void Update()
    {
        sec += Time.deltaTime;

        if (sec >= 60f)
        {
            min += 1;
            sec = 0;
        }

        timerText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
    }
}
