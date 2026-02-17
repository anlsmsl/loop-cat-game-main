using UnityEngine;
using UnityEngine.EventSystems;

public class UIScaleChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float scaleFactor = 1.1f;
    public float speed = 15f;
    private Vector3 targetScale;

    public bool isLocked = false;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLocked) return;
        targetScale = originalScale * scaleFactor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isLocked) return;
        targetScale = originalScale;
    }

    public void LockScale()
    {
        isLocked = true;
        targetScale = originalScale * scaleFactor; // Büyük boyutta kilitle
    }

    public void ResetScale()
    {
        isLocked = false;
        targetScale = originalScale; // Normal boyuta dön ve kilidi aç
    }
}
