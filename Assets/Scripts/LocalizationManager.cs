using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // En üste ekle

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
    private Dictionary<string, string> localizedText;
    
    // Dinamik arama için Türkçe -> İngilizce sözlüğü (Performans için)
    private Dictionary<string, string> trToEnDictionary;

    public string currentLanguage; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        currentLanguage = PlayerPrefs.GetString("SelectedLanguage", "TR");
        LoadLanguageData();
    }

    public void LoadLanguageData()
    {
        localizedText = new Dictionary<string, string>();
        trToEnDictionary = new Dictionary<string, string>(); 

        TextAsset csvFile = Resources.Load<TextAsset>("Languages");
        if (csvFile == null) return;

        string[] lines = csvFile.text.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++) 
        {
            string[] columns = lines[i].Split(new char[] {  ';' });
            if (columns.Length < 3) continue;

            string key = columns[0].Trim(); 
            string trVal = columns[1].Trim();
            string enVal = columns[2].Trim();

            if (key.ToLower() == "id") continue;

            // ID bazlı normal çeviri
            string value = (currentLanguage == "TR") ? trVal : enVal;
            if (!localizedText.ContainsKey(key)) localizedText.Add(key, value);

            // ÖNEMLİ: Sadece gerçekten metin olanları dinamik sözlüğe ekle
            // Boş satırların veya anlamsız kısa verilerin karışmasını engeller
            if (!string.IsNullOrEmpty(trVal) && trVal.Length > 1) {
                if (!trToEnDictionary.ContainsKey(trVal)) trToEnDictionary.Add(trVal, enVal);
            }
        }
    }

    public void ChangeLanguage(string newLang) 
    {
        currentLanguage = newLang;
        PlayerPrefs.SetString("SelectedLanguage", newLang);
        PlayerPrefs.Save();

        LoadLanguageData();
    
        // Sahnedeki tüm metinleri güncelle
        LocalizedText[] allTexts = Resources.FindObjectsOfTypeAll<LocalizedText>();
        foreach(LocalizedText t in allTexts) 
        {
            if (t.gameObject.scene.name != null) 
            {
                t.UpdateText();
            }
        }
    }
    
    // Optimize edilmiş versiyon: Dosyayı değil, hafızadaki sözlüğü okur
    public string GetValueByValue(string searchWord)
    {
        if (currentLanguage == "TR") return searchWord; // Zaten Türkçe ise dokunma

        if (trToEnDictionary != null && trToEnDictionary.ContainsKey(searchWord))
        {
            return trToEnDictionary[searchWord];
        }
        return searchWord; 
    }

    public string GetValue(string key)
    {
        if (localizedText == null || !localizedText.ContainsKey(key)) return "HATA: " + key;
        return localizedText[key];
    }
    
    void OnEnable()
    {
        // Sahne her değiştiğinde bu fonksiyon çalışır
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Sahne değişince tüm metinleri bir kez daha zorla güncelle
        UpdateAllTexts();
    }

    public void UpdateAllTexts()
    {
        LocalizedText[] allTexts = Resources.FindObjectsOfTypeAll<LocalizedText>();
        foreach(LocalizedText t in allTexts) 
        {
            // Sadece aktif olan ve sahnede bulunan metinleri güncelle
            if (t.gameObject.scene.name != null) 
            {
                t.UpdateText();
            }
        }
    }
}