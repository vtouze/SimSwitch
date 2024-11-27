using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    public int segments = 3;
    private int publicWorksState = 0;

    public Sprite normalSprite;
    public Sprite publicWorksSprite;

    private GameObject[] segmentObjects;

    private void Start()
    {
        GenerateSegments();
    }

    private void GenerateSegments()
    {
        segmentObjects = new GameObject[segments];
        float segmentWidth = 1.0f / segments;

        for (int i = 0; i < segments; i++)
        {
            GameObject segment = new GameObject($"Segment_{i}");
            segment.transform.SetParent(transform);
            segment.transform.localScale = new Vector3(segmentWidth, 1, 1);
            segment.transform.localPosition = new Vector3(
                (i - (segments - 1) / 2.0f) * segmentWidth,
                0, 
                0
            );

            SpriteRenderer renderer = segment.AddComponent<SpriteRenderer>();
            renderer.sprite = normalSprite;
            renderer.sortingOrder = 1;

            segmentObjects[i] = segment;
        }
    }

    public void ApplyPublicWorks()
    {
        if (publicWorksState < segments)
        {
            SpriteRenderer renderer = segmentObjects[publicWorksState].GetComponent<SpriteRenderer>();
            renderer.sprite = publicWorksSprite;

            publicWorksState++;
        }
    }
}