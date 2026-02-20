using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Sorular", menuName = "Scriptable Objects/Sorular")]
public class Sorular : ScriptableObject
{
    [TextArea] public string soruMetni;

    public string secenekAMetni;
    public Sorular secenekASonucu;
    [TextArea] public string secenekASonucMetni;
    public Sprite secenekAResmi;
    public int secenekA_CanEtkisi;
    public int secenekA_AcclikEtkisi;
    public Sprite SecenekAÖzelResim;


    public string secenekBMetni;
    public Sorular secenekBSonucu;
    [TextArea] public string secenekBSonucMetni;
    public Sprite secenekBResmi;
    public int secenekB_CanEtkisi;
    public int secenekB_AcclikEtkisi;
    public Sprite SecenekBÖzelResim;


    public bool olumculMu;



    public Sprite Background;




}
