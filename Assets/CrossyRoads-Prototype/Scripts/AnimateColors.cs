using UnityEngine;

public class AnimateColors : MonoBehaviour
{
    public Color[] colorList;
    public int colorIndex = 0;
    public float changeTime = 1;
    public float changeTimeCount = 0;
    public float changeSpeed = 1;
    public bool isPaused = false;
    public bool isLooping = true;
    void Start()
    {
        SetColor();
    }
    void Update()
    {
        if (isPaused == false)
        {
            if (changeTime > 0)
            {
                if (changeTimeCount < changeTime)
                {
                    changeTimeCount += Time.deltaTime;
                }
                else
                {
                    changeTimeCount = 0;
                    if (colorIndex < colorList.Length - 1)
                    {
                        colorIndex++;
                    }
                    else
                    {
                        if (isLooping == true)
                            colorIndex = 0;
                    }
                }
            }

            TextMesh textMesh = GetComponent<TextMesh>();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            if (textMesh)
            {
                textMesh.color = Color.Lerp(textMesh.color, colorList[colorIndex], changeSpeed * Time.deltaTime);
            }

            if (spriteRenderer)
            {
                spriteRenderer.color = Color.Lerp(spriteRenderer.color, colorList[colorIndex], changeSpeed * Time.deltaTime);
            }

            if (GetComponent<Renderer>().sharedMaterial)
            {
                GetComponent<Renderer>().sharedMaterial.color = Color.Lerp(GetComponent<Renderer>().sharedMaterial.color, colorList[colorIndex], changeSpeed * Time.deltaTime);
            }
        }
        else
        {
            SetColor();
        }
    }

    void SetColor()
    {
        TextMesh textMesh = GetComponent<TextMesh>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (textMesh)
        {
            textMesh.color = colorList[colorIndex];
        }

        if (spriteRenderer)
        {
            spriteRenderer.color = colorList[colorIndex];
        }
    }
}
