using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;
    private float deltaTime;

    private void Start()
    {
        double refreshRate = Screen.currentResolution.refreshRateRatio.value;
        Debug.Log("현재 모니터 주사율: " + refreshRate);
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.RoundToInt(fps).ToString();
    }
}