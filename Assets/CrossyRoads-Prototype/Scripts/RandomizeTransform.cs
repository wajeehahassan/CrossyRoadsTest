using UnityEngine;

public class RandomizeTransform : MonoBehaviour
{
    public Vector2 rotationRangeX = new Vector2(0, 360);
    public Vector2 rotationRangeY = new Vector2(0, 360);
    public Vector2 rotationRangeZ = new Vector2(0, 360);

    public Vector2 scaleRangeX = new Vector2(1, 1.3f);
    public Vector2 scaleRangeY = new Vector2(1, 1.3f);
    public Vector2 scaleRangeZ = new Vector2(1, 1.3f);

    public bool uniformScale = true;

    public Color[] colorList;
    void Start()
    {
        transform.localEulerAngles = new Vector3(Random.Range(rotationRangeX.x, rotationRangeX.y), Random.Range(rotationRangeY.x, rotationRangeY.y), Random.Range(rotationRangeZ.x, rotationRangeZ.y));

        if (uniformScale == true)
            scaleRangeY = scaleRangeZ = scaleRangeX;

        transform.localScale = new Vector3(Random.Range(scaleRangeX.x, scaleRangeX.y), Random.Range(scaleRangeY.x, scaleRangeY.y), Random.Range(scaleRangeZ.x, scaleRangeZ.y));

        int randomColor = Mathf.FloorToInt(Random.Range(0, colorList.Length));

        Component[] comps = GetComponentsInChildren<Renderer>();

        foreach (Renderer part in comps)
            part.GetComponent<Renderer>().material.color = colorList[randomColor];
    }
}
