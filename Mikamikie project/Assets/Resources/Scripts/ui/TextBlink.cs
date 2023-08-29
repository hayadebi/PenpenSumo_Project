using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextBlink : MonoBehaviour
{
    public Text textToBlink;
    public float fadeDuration = 1.0f; // フェードの持続時間（秒）
    public Animator anim;
    public GameObject offobj;
    private bool isFading = false;
    private bool onfade = true;
    private bool istask = false;

    private void Start()
    {
        if (!GManager.instance.title_anim) OnClickTitleAnim();
    }
    private void Update()
    {
        if (onfade && !istask)
        {
            istask = true;
            StartCoroutine(FadeBlinkText());
        }
    }
    public void OnClickTitleAnim()
    {
        GManager.instance.setrg = 0;
        onfade = false;
        anim.SetInteger("Atrg", 1);
        offobj.SetActive(false);
        GManager.instance.title_anim = false;
    }
    private IEnumerator FadeBlinkText()
    {
            isFading = !isFading;

            float startAlpha = isFading ? 1.0f : 0.0f;
            float targetAlpha = isFading ? 0.0f : 1.0f;
            float elapsedTime = 0.0f;

            while (elapsedTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                Color textColor = textToBlink.color;
                textColor.a = alpha;
                textToBlink.color = textColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // フェード完了後、一時的に非表示にするためのウェイト
            yield return new WaitForSeconds(0.5f);
            istask = false;
    }
}
