using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashColor : MonoBehaviour
{
    public Text textComponent;
    public Color startColor = new Color(0.9725491f, 0.1960784f, 0.1333333f);
    public Color endColor = new Color(0.9725491f, 0.5f, 0.1333333f);
    public float pulseDuration = 1.0f;

    private float elapsedTime = 0.0f;
    private bool isPulsing = false;

    void Start()
    {
        textComponent = GetComponent<Text>();
        textComponent.color = startColor;
    }

    void Update()
    {
        if (!isPulsing)
        {
            StartCoroutine(PulseText());
            isPulsing = true;
        }
    }

    private IEnumerator PulseText()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / pulseDuration;
            textComponent.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / pulseDuration;
            textComponent.color = Color.Lerp(endColor, startColor, t);
            yield return null;
        }

        isPulsing = false;
    }
}
