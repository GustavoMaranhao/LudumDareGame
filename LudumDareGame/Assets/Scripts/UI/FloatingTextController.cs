using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    private static DamagePopText popupText;
    private static DamagePopText popupHealText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        popupText = Resources.Load<DamagePopText>("Prefabs/UI/DamageTextParent");
        popupHealText = Resources.Load<DamagePopText>("Prefabs/UI/HealTextParent");
    }

    public static void CreateFloatingText(string text, Transform location)
    {
        DamagePopText instance = Instantiate(popupText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-0.5f, 0.5f), location.position.y + Random.Range(-0.5f,0.5f)));

        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;

        instance.SetText(text);
    }

    public static void CreateFloatingHealText(string text, Transform location)
    {
        DamagePopText instance = Instantiate(popupHealText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-0.5f, 0.5f), location.position.y + Random.Range(-0.5f, 0.5f)));

        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;

        instance.SetHealText(text);
    }
}
