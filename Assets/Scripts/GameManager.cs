using System;
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

        tamEkranDevamButonu.SetActive(false);
        AButonlarPaneli.SetActive(true);
        BButonlarPaneli.SetActive(true);

        AButonlarPaneli.GetComponent<UIScaleChanger>().ResetScale();
        BButonlarPaneli.GetComponent<UIScaleChanger>().ResetScale();

        if (sonrakiSoruDeposu == null)
            YenidenBaslat();
        else
            MevcutSoruyuGuncelle(sonrakiSoruDeposu);
    }
}
