using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public GameObject[] hearts; // Inspector'dan kalpleri buraya sürükle
    public GameObject[] meats;  // Inspector'dan etleri buraya sürükle

    public void UpdateUI(int health, int hunger)
    {
        // Can UI Güncelleme
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < health);
        }

        // Açlýk UI Güncelleme
        for (int i = 0; i < meats.Length; i++)
        {
            meats[i].SetActive(i < hunger);
        }
    }
}
