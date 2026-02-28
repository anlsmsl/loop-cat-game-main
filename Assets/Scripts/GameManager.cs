using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI soruText;
    public TextMeshProUGUI butonAText;
    public TextMeshProUGUI butonBText;
    public Image ButtonAImage;
    public Image ButtonBImage;
    public Image Background;
    public Image SecenekAÖzelResim;
    public Image SecenekBÖzelResim;

    public GameObject özelResimPaneli;

    public TextMeshProUGUI Yukardakiyazý;



    public PlayerStats playerStats;

    public Sorular baslangicSorusu;
    private Sorular mevcutSoru;

    private Sorular sonrakiSoruDeposu;

    public Sorular AçlýktanÖl;
    public Sorular HastalýktanÖl;

    public GameObject AButonlarPaneli;
    public GameObject BButonlarPaneli;
    public GameObject tamEkranDevamButonu;

    public AudioSource sesKaynagý;
    public AudioClip klikSesi;

    public CanvasGroup oyunCanvas;
    public CanvasGroup ozelResimCanvasGroup;




    void Start()
    {
        YenidenBaslat();
    }

    private void YenidenBaslat()
    {
        if (SceneFader.instance != null)
    {
        StartCoroutine(SceneFader.instance.FadeOut());
    }
        MevcutSoruyuGuncelle(baslangicSorusu);

        playerStats.ChangeHealth(playerStats.maxHealth);
        playerStats.ChangeHunger(playerStats.maxHunger);


    }



    private void MevcutSoruyuGuncelle(Sorular yeniSoru)
    {

        mevcutSoru = yeniSoru;
        soruText.text = mevcutSoru.soruMetni;
        butonAText.text = mevcutSoru.secenekAMetni;
        ButtonAImage.sprite = mevcutSoru.secenekAResmi;
        ButtonBImage.sprite = mevcutSoru.secenekBResmi;
        butonBText.text = mevcutSoru.secenekBMetni;
        Background.sprite = mevcutSoru.Background;

        switch (Background.sprite.name)
        {
            case "Orman_0":
                Yukardakiyazý.text = "ORMAN";
                break;
            case "Sanayi_0":
                Yukardakiyazý.text = "SANAYÝ";
                break;
            case "ssokak_0":
                Yukardakiyazý.text = "MAHALLE";
                break;
        }

;
        
    }

    public void SecimYap(bool secenekA_Mi)
    {
        if (mevcutSoru == AçlýktanÖl || mevcutSoru == HastalýktanÖl)
        {
            YenidenBaslat();
            return; 
        }
        if (secenekA_Mi)
        {
            sesKaynagý.PlayOneShot(klikSesi);

            playerStats.ChangeHealth(mevcutSoru.secenekA_CanEtkisi);
            playerStats.ChangeHunger(mevcutSoru.secenekA_AcclikEtkisi);
            if (playerStats.currentHunger <=0)
            {
                MevcutSoruyuGuncelle(AçlýktanÖl);
                return;
            }
            if (playerStats.currentHealth <= 0)
            {
                MevcutSoruyuGuncelle(HastalýktanÖl);
                return;
            }
            if (mevcutSoru.SecenekAÖzelResim != null)
            {
                StartCoroutine(FadeCanvas(ozelResimCanvasGroup, 1f, 0.3f));
                SecenekAÖzelResim.sprite = mevcutSoru.SecenekAÖzelResim;
            }
            CevabýGöster(false);
        }
        else
        {
            sesKaynagý.PlayOneShot(klikSesi);

            playerStats.ChangeHealth(mevcutSoru.secenekB_CanEtkisi);
            playerStats.ChangeHunger(mevcutSoru.secenekB_AcclikEtkisi);
            if (playerStats.currentHunger <= 0)
            {
                MevcutSoruyuGuncelle(AçlýktanÖl);
                return;
            }
            if (playerStats.currentHealth <= 0)
            {
                MevcutSoruyuGuncelle(HastalýktanÖl); 
                return;
            }
            if (mevcutSoru.SecenekBÖzelResim != null)
            {
                StartCoroutine(FadeCanvas(ozelResimCanvasGroup, 1f, 0.3f));
                SecenekBÖzelResim.sprite = mevcutSoru.SecenekBÖzelResim;

            }
            CevabýGöster(true);

        }

        sonrakiSoruDeposu = secenekA_Mi ? mevcutSoru.secenekASonucu : mevcutSoru.secenekBSonucu;


        tamEkranDevamButonu.SetActive(true);


    }

    private void CevabýGöster(bool Aise0Bise1)
    {
        if (Aise0Bise1 == false)
        {
            soruText.text = mevcutSoru.secenekASonucMetni;
            BButonlarPaneli.SetActive(false);

            AButonlarPaneli.GetComponent<UIScaleChanger>().LockScale();

        }
        if (Aise0Bise1 == true)
        {
            soruText.text = mevcutSoru.secenekBSonucMetni;
            AButonlarPaneli.SetActive(false);
            BButonlarPaneli.GetComponent <UIScaleChanger>().LockScale();
        }
        


    }
    private Coroutine gecisSüreci;

    public void SonrakiSoruyaGec()
    {
        // Eðer zaten bir geçiþ varsa (coroutine çalýþýyorsa) yeni bir tane baþlatma!
        if (gecisSüreci != null) return;

        // Coroutine referansýný burada ata
        gecisSüreci = StartCoroutine(SoruGecisSureci());
    }

    private IEnumerator SoruGecisSureci()
    {
        // BURADAKÝ StartCoroutine satýrýný sildik! 
        // Çünkü metodu yukarýda zaten baþlattýk.

        // 1. ADIM: Her þeyi karart
        oyunCanvas.interactable = false;
        oyunCanvas.blocksRaycasts = false;

        // Fade iþlemlerini baþlat ve bitmelerini bekle
        Coroutine f1 = StartCoroutine(FadeCanvas(oyunCanvas, 0f, 0.3f));
        Coroutine f2 = StartCoroutine(FadeCanvas(ozelResimCanvasGroup, 0f, 0.3f));

        yield return f1;
        yield return f2;

        // 2. ADIM: UI ayarlarýný yap (Görünmezken arkada hazýrla)
        tamEkranDevamButonu.SetActive(false);
        AButonlarPaneli.SetActive(true);
        BButonlarPaneli.SetActive(true);

        if (AButonlarPaneli.TryGetComponent(out UIScaleChanger sA)) sA.ResetScale();
        if (BButonlarPaneli.TryGetComponent(out UIScaleChanger sB)) sB.ResetScale();

        // 3. ADIM: Soruyu güncelle
        if (sonrakiSoruDeposu == null)
            YenidenBaslat();
        else
            MevcutSoruyuGuncelle(sonrakiSoruDeposu);

        yield return new WaitForSecondsRealtime(0.1f);

        // 4. ADIM: Yeni soruyu yavaþça göster
        yield return StartCoroutine(FadeCanvas(oyunCanvas, 1f, 0.3f));

        oyunCanvas.interactable = true;
        oyunCanvas.blocksRaycasts = true;

        // Ýþlem bittiði için referansý null yapýyoruz ki bir sonraki týklama çalýþabilsin
        gecisSüreci = null;
    }
    private IEnumerator FadeCanvas(CanvasGroup cg, float targetAlpha, float duration)
    {
        if (targetAlpha > 0 && !cg.gameObject.activeInHierarchy)
        {
            cg.gameObject.SetActive(true);
        }

        if (cg == null) yield break;

        float currentTime = 0f;
        float startAlpha = cg.alpha;

        if (duration <= 0)
        {
            cg.alpha = targetAlpha;
        }

        // Eðer beliriyorsa (Fade In)
        if (targetAlpha > 0)
        {
            cg.gameObject.SetActive(true); // Kapalýysa aç
            cg.interactable = true;
            cg.blocksRaycasts = true;

        }

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime; // Oyun durursa bile çalýþsýn
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / duration);
            yield return null;
        }

        cg.alpha = targetAlpha;

        // Eðer yok olduysa (Fade Out)
        if (targetAlpha <= 0)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
            // Ýstersen tamamen kapatabilirsin: 
            // canvasGroup.gameObject.SetActive(false); 
        }
    }

}
