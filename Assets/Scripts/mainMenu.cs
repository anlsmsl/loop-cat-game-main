using System.Collections;
using UnityEngine; 
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    // Panel objelerini Unity Inspector üzerinden sürükleyip bağlayacağız
    public GameObject gelistiriciPaneli;
    public GameObject anaMenuObjeleri; // Opsiyonel: Panel açılınca ana butonları gizlemek istersen
    public GameObject ayarlarPaneli; // Ayarlar paneli için yeni referans
    public AudioSource sesKaynagı;
    public AudioClip klikSesi;
    

    public void SonrakiSahneyeGit()
    {
sesKaynagı.PlayOneShot(klikSesi);
            StartCoroutine(SahneGecisSureci());
    }
    IEnumerator SahneGecisSureci()
    {
        // 1. Ekranın kararmasını başlat ve BİTMESİNİ BEKLE
        yield return StartCoroutine(SceneFader.instance.Fade(2f));

        // 2. Kararma bitti (buraya ancak 2 saniye sonra gelir)
        int mevcutSahneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(mevcutSahneIndex + 1);
    }

    // Geliştiriciler panelini açar
    public void GelistiricileriAc()
    {
        if (gelistiriciPaneli != null)
        {
            sesKaynagı.PlayOneShot(klikSesi);

            gelistiriciPaneli.SetActive(true);
            anaMenuObjeleri.SetActive(false);

            // anaMenuObjeleri.SetActive(false); // İstersen ana butonları kapatabilirsin
        }
    }

    // Geliştiriciler panelini kapatır
    public void GelistiricileriKapat()
    {
        if (gelistiriciPaneli != null)
        {
            sesKaynagı.PlayOneShot(klikSesi);

            gelistiriciPaneli.SetActive(false);
            anaMenuObjeleri.SetActive(true);
            // anaMenuObjeleri.SetActive(true); // Ana butonları geri açarsın
        }
    }
    
    public void AyarlariAc()
    {
        if (ayarlarPaneli != null)
        {
            sesKaynagı.PlayOneShot(klikSesi);
            ayarlarPaneli.SetActive(true);
            anaMenuObjeleri.SetActive(false);
            
        }
    }

    public void AyarlariKapat()
    {
        if (ayarlarPaneli != null)
        {
            sesKaynagı.PlayOneShot(klikSesi);
            ayarlarPaneli.SetActive(false);
            anaMenuObjeleri.SetActive(true);
            
        }
    }

    // Oyundan çıkış yapar
    public void OyundanCik()
    {
        sesKaynagı.PlayOneShot(klikSesi);

        Debug.Log("Oyundan çıkılıyor..."); // Editörde çalıştığını anlamak için
        Application.Quit(); // Derlenmiş oyunda çalışır
    }
}