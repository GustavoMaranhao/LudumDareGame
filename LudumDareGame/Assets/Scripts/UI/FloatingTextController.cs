using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    private static DamagePopText popupText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        popupText = Resources.Load<DamagePopText>("Prefabs/UI/DamageTextParent");
    }

    public static void CreateFloatingText(string text, Transform location)
    {
        DamagePopText instance = Instantiate(popupText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-0.5f, 0.5f), location.position.y + Random.Range(-0.5f,0.5f)));

        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;

        instance.SetText(text);
    }
}
