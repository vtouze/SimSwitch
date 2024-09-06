using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class WindowGraph : MonoBehaviour {

    #region Fields
    [SerializeField] private Sprite _circleSprite;
    private RectTransform _graphContainer;
    private RectTransform _labelTemplateX;
    private RectTransform _labelTemplateY;
    private RectTransform _dashTemplateX;
    private RectTransform _dashTemplateY;
    #endregion Fields

    #region Methods
    private void Awake() {
        _graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        _labelTemplateX = _graphContainer.Find("LabelTemplateX").GetComponent<RectTransform>();
        _labelTemplateY = _graphContainer.Find("LabelTemplateY").GetComponent<RectTransform>();
        _dashTemplateX = _graphContainer.Find("DashTemplateX").GetComponent<RectTransform>();
        _dashTemplateY = _graphContainer.Find("DashTemplateY").GetComponent<RectTransform>();

        List<int> valueList = new List<int>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33 };
        ShowGraph(valueList, (int _i) => "Day "+(_i+1), (float _f) => "$" + Mathf.RoundToInt(_f) + "k");
    }

    private GameObject CreateCircle(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("Circle", typeof(Image));
        gameObject.transform.SetParent(_graphContainer, false);
        gameObject.GetComponent<Image>().sprite = _circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<int> valueList, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
        if (getAxisLabelX == null) {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null) {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        float graphHeight = _graphContainer.sizeDelta.y;
        float yMaximum = 100f;
        float xSize = 50f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(_labelTemplateX);
            labelX.SetParent(_graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -25f);
            labelX.GetComponent<TMP_Text>().text = getAxisLabelX(i);
            
            RectTransform dashX = Instantiate(_dashTemplateX);
            dashX.SetParent(_graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -3f);
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++) {
            RectTransform labelY = Instantiate(_labelTemplateY);
            labelY.SetParent(_graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-40f, normalizedValue * graphHeight);
            labelY.GetComponent<TMP_Text>().text = getAxisLabelY(normalizedValue * yMaximum);
            
            RectTransform dashY = Instantiate(_dashTemplateY);
            dashY.SetParent(_graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
        GameObject gameObject = new GameObject("DotConnection", typeof(Image));
        gameObject.transform.SetParent(_graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }
    #endregion Methods
}