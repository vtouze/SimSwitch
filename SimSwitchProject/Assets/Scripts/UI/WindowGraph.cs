using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    #region Fields
    [SerializeField] private Sprite _circleSprite;
    private RectTransform _graphContainer;
    #endregion Fields

    #region Methods
    private void Awake()
    {
        _graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();

        List<int> valueList = new List<int>() {5, 98, 56, 54, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33};
        ShowGraph(valueList);
    }

    private GameObject CreateCircle(Vector2 anhoredPosition)
    {
        GameObject gameObject = new GameObject("Circle", typeof(Image));
        gameObject.transform.SetParent(_graphContainer, false);
        gameObject.GetComponent<Image>().sprite = _circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anhoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<int> valueList)
    {
        float graphHeight = _graphContainer.sizeDelta.y;
        float yMaximum = 100f;
        float xSize = 50f;

        GameObject lastCircleGameObject = null;
        for(int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            lastCircleGameObject = circleGameObject;
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("DotConnection", typeof(Image));
        gameObject.transform.SetParent(_graphContainer, false);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(100, 3f);
        rectTransform.anchoredPosition = dotPositionA;
        rectTransform.localEulerAngles = new Vector3(0, 0, 0);
    }
    #endregion Methods
}