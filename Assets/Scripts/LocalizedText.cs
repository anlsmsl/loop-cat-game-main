using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    public string key; // Excel'deki ID (Örn: oyun_menu_oyna)
    
    private string lastText = ""; // Metin değişimini takip etmek için
    private bool isUpdatingInternal = false;

    void Start()
    {
        UpdateText();
    }

    // Manager'ın "kırmızı yanıyor" dediği fonksiyon tam olarak bu!
    public void UpdateText()
    {
        if (LocalizationManager.Instance == null) return;

        // Eğer bir Key (ID) verilmişse onu kullan
        if (!string.IsNullOrEmpty(key))
        {
            string translation = LocalizationManager.Instance.GetValue(key);
            ApplyText(translation);
            lastText = translation;
        }
    }

    // Seçim oyunundaki dinamik metinleri takip eden kısım
    void Update()
    {
        // Eğer bir Key yoksa (Dinamik seçim metniyse) otomatik çeviri yap
        if (string.IsNullOrEmpty(key))
        {
            string currentText = GetText();
            if (currentText != lastText && !isUpdatingInternal)
            {
                string translated = LocalizationManager.Instance.GetValueByValue(currentText);
                if (translated != currentText)
                {
                    isUpdatingInternal = true;
                    ApplyText(translated);
                    lastText = translated;
                    isUpdatingInternal = false;
                }
            }
        }
    }

    // Ekrandaki metni okuyan yardımcı fonksiyon
    private string GetText()
    {
        if (GetComponent<TMP_Text>() != null) return GetComponent<TMP_Text>().text;
        if (GetComponent<Text>() != null) return GetComponent<Text>().text;
        return "";
    }

    // Ekrandaki metni değiştiren yardımcı fonksiyon
    private void ApplyText(string value)
    {
        if (GetComponent<TMP_Text>() != null) GetComponent<TMP_Text>().text = value;
        if (GetComponent<Text>() != null) GetComponent<Text>().text = value;
    }
}