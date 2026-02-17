using UnityEngine;

public class LanguageSwitcher : MonoBehaviour
{
    public void TurkceYap()
    {
        
        LocalizationManager.Instance.ChangeLanguage("TR");
    }

    public void IngilizceYap()
    {
        
        LocalizationManager.Instance.ChangeLanguage("EN");
    }
}