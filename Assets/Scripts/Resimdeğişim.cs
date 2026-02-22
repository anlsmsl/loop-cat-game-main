using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Resimdeğişim : MonoBehaviour
{
    public Sprite[] kediler;
    public Image değişecekimage;
    private int resimsayacı = 0;

    [Header("Altyazı Ayarları")]
    public GameObject altyaziPaneli; // Altyazının arkasındaki siyah bant
    public TMP_Text altyaziMetni; 
    public string[] altyaziKeyleri; 
    private Coroutine daktiloRoutine;

    public AudioSource fonMuzigi; // Kısılacak olan ses kaynağı
    public float azalisMiktari = 0.12f; // Her tıklamada ne kadar azalacağı (1 / 8 ≈ 0.12)

    void Start()
    {
        // Başlangıçta paneli kapat
        if (altyaziPaneli != null) altyaziPaneli.SetActive(false);
    }

    public void ResimDegiş()
    {
        resimsayacı++;

        if (fonMuzigi != null && fonMuzigi.volume > 0)
        {
            // Sesi her basışta azaltır, 0'ın altına düşmesini engeller
            fonMuzigi.volume = Mathf.Max(0, fonMuzigi.volume - azalisMiktari);
        }

        if (resimsayacı < kediler.Length)
        {
            değişecekimage.sprite = kediler[resimsayacı];
            AltyaziKontrol();
        }

        if (resimsayacı >= 13)
        {
            StartCoroutine(SahneGecisSureci());
        }
    }

    void AltyaziKontrol()
    {
        if (LocalizationManager.Instance.currentLanguage == "EN" && resimsayacı >= 10 && resimsayacı <= 12)
        {
            if (altyaziPaneli != null) altyaziPaneli.SetActive(true);
        
            int altyaziIndex = resimsayacı - 10; 
            if (altyaziIndex < altyaziKeyleri.Length)
            {
                string anahtar = altyaziKeyleri[altyaziIndex];
                string cevrilmisMetin = LocalizationManager.Instance.GetValue(anahtar);

                // Eğer eski daktilo varsa durdur, yenisini başlat
                if (daktiloRoutine != null) StopCoroutine(daktiloRoutine);
                daktiloRoutine = StartCoroutine(DaktiloYaz(cevrilmisMetin));
            }
        }
        else
        {
            if (daktiloRoutine != null) StopCoroutine(daktiloRoutine);
            if (altyaziPaneli != null) altyaziPaneli.SetActive(false);
            if (altyaziMetni != null) altyaziMetni.text = "";
        }
    }
    IEnumerator SahneGecisSureci()
    {
        if (SceneFader.instance != null)
            yield return StartCoroutine(SceneFader.instance.Fade(2f));
        else
            yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator DaktiloYaz(string metin)
    {
        altyaziMetni.text = ""; // Temizle
        foreach (char harf in metin)
        {
            altyaziMetni.text += harf;
            yield return new WaitForSeconds(0.05f); // Yazı hızı
        }
    }
}