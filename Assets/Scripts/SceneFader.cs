using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{

    public static SceneFader instance;
    public float fadeSpeed = 2.0f;

    private static bool isFirstLoad = true;
    private void Awake()
    {
        instance = this;
    }
    public Image image;

    public IEnumerator Fade(float duration)
    {
        yield return StartCoroutine(Fader(duration));
    }

    public IEnumerator Fader(float duration)
    {
        float t = 0;
        Color c = image.color;
        while (t < fadeSpeed)
        {
            t += Time.deltaTime;
            c.a = t / fadeSpeed;
            image.color = c;
            yield return null;
        }


    }

    public IEnumerator FadeOut()
    {
        float t = 0;
        Color c = image.color;
        while (t < fadeSpeed)
        {
            t += Time.deltaTime;
            c.a = 1f -(t/ fadeSpeed);
            image.color = c;
            yield return null;
        }
    }


    private void Start()
    {
        if (isFirstLoad)
        {
            // Hemen þeffaf yap ve efekt uygulama
            Color c = image.color;
            c.a = 0f;
            image.color = c;

            // Bir sonraki sahneler için bunu false yapýyoruz
            isFirstLoad = false;
        }
        else
        {

            Color c = image.color;
            c.a = 1f;
            image.color = c;
            StartCoroutine(FadeOut());
        }
    }
}

