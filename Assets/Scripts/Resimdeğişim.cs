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

    void Start()
    {
        // Başlangıçta paneli kapat
        if (altyaziPaneli != null) altyaziPaneli.SetActive(false);
    }

    public void ResimDegiş()
    {
        resimsayacı++;
        
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
        // 1. ADIM: Dil İngilizce mi ve son 3 resimde miyiz?
        if (LocalizationManager.Instance.currentLanguage == "EN" && resimsayacı >= 10 && resimsayacı <= 12)
        {
            if (altyaziPaneli != null) altyaziPaneli.SetActive(true); // Altyazı bandını aç
            
            int altyaziIndex = resimsayacı - 10; 
            if (altyaziIndex < altyaziKeyleri.Length)
            {
                // Excel'deki İngilizce karşılığını yazdır
                string anahtar = altyaziKeyleri[altyaziIndex];
                altyaziMetni.text = LocalizationManager.Instance.GetValue(anahtar);
            }
        }
        else
        {
            // Dil Türkçeyse veya son 3 resimde değilsek altyazıyı kapat
            if (altyaziPaneli != null) altyaziPaneli.SetActive(false);
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
}