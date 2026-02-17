using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject ayarlarPaneli;

    [Header("Ses")]
    public AudioSource sesKaynagı;
    public AudioClip klikSesi;

    private bool oyunDurdu = false;

    void Start()
    {
        if (ayarlarPaneli != null) ayarlarPaneli.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (oyunDurdu) AyarlariKapat();
            else AyarlariAc();
        }
    }

    // --- AYARLAR KONTROL ---
    public void AyarlariAc()
    {
        if (sesKaynagı && klikSesi) sesKaynagı.PlayOneShot(klikSesi);
        ayarlarPaneli.SetActive(true);
        Time.timeScale = 0f; 
        oyunDurdu = true;
    }

    public void AyarlariKapat()
    {
        if (sesKaynagı && klikSesi) sesKaynagı.PlayOneShot(klikSesi);
        ayarlarPaneli.SetActive(false);
        Time.timeScale = 1f; 
        oyunDurdu = false;
    }

    // --- DİL BUTONLARI ---
    // Türkçe butonuna bunu bağla
    public void DilSecTR()
    {
        if (sesKaynagı && klikSesi) sesKaynagı.PlayOneShot(klikSesi);
        LocalizationManager.Instance.ChangeLanguage("TR");
        Debug.Log("Oyun içi dil TR yapıldı.");
    }

    // English butonuna bunu bağla
    public void DilSecEN()
    {
        if (sesKaynagı && klikSesi) sesKaynagı.PlayOneShot(klikSesi);
        LocalizationManager.Instance.ChangeLanguage("EN");
        Debug.Log("Oyun içi dil EN yapıldı.");
    }

    public void AnaMenuyeDon()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}