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

    public CanvasGroup canvasGroup;
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
    public void SonrakiSoruyaGec()
    {
        StopAllCoroutines();
        StartCoroutine(SoruGecisSureci());

        
    }
    private IEnumerator SoruGecisSureci()
    {

        float kapanmaSuresi = 0.3f;

        if (SecenekAÖzelResim.sprite != null && SecenekAÖzelResim.sprite.name == "öldün ekraný_0")
        {
            if (SecenekAÖzelResim.sprite != null && SecenekAÖzelResim.sprite.name == "öldün ekraný_0")
            {
                kapanmaSuresi = 0f;
            }
        }


        StartCoroutine(FadeCanvas(canvasGroup,0f, 0.3f));
        yield return StartCoroutine(FadeCanvas(ozelResimCanvasGroup, 0f, kapanmaSuresi));

        // 2. ADIM: UI ayarlarýný yap (Panel tamamen kapandýktan sonra)
        tamEkranDevamButonu.SetActive(false);
        AButonlarPaneli.SetActive(true);
        BButonlarPaneli.SetActive(true);
        AButonlarPaneli.GetComponent<UIScaleChanger>().ResetScale();
        BButonlarPaneli.GetComponent<UIScaleChanger>().ResetScale();

        // 3. ADIM: Soruyu güncelle
        if (sonrakiSoruDeposu == null)
            YenidenBaslat();
        else
            MevcutSoruyuGuncelle(sonrakiSoruDeposu);

        // 4. ADIM: Yeni soruyu yavaþça göster (0.2 saniye sürsün)
        yield return StartCoroutine(FadeCanvas(canvasGroup,1f, 0.3f));
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator FadeCanvas(CanvasGroup cg, float targetAlpha, float duration)
    {
        if (cg == null || !cg.gameObject.activeInHierarchy && targetAlpha <= 0)
        {
            yield break;
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
