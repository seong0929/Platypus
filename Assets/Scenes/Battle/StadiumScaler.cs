using UnityEngine;

// 경기장 크기 조절
public class StadiumScaler : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    private void Start()
    {
        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            RectTransform scalerRect = GetComponent<RectTransform>();

            if (canvasRect != null && scalerRect != null)
            {
                Vector2 newScale = new Vector2(scalerRect.sizeDelta.x * canvasRect.localScale.x, scalerRect.sizeDelta.y * canvasRect.localScale.y);

                scalerRect.sizeDelta = newScale;
            }
        }
    }
}