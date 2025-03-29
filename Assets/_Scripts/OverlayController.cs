using UnityEngine;
using UnityEngine.UI;

public class OverlayController : MonoBehaviour
{
    public GameObject overlay;          
    public Image background;            
    public RectTransform asset;        

    public float fadeSpeed = 2f;
    public float dropSpeed = 100f;

    private bool isFading = false;

    void Update()
    {
        
        if (!isFading && Input.anyKeyDown)
        {
            isFading = true;
        }

        if (isFading)
        {
            
            asset.anchoredPosition -= new Vector2(0, dropSpeed * Time.deltaTime);

            
            Color c = background.color;
            c.a = Mathf.MoveTowards(c.a, 0, fadeSpeed * Time.deltaTime);
            background.color = c;

            if (c.a <= 0)
            {
                overlay.SetActive(false);
            }
        }
    }
}
