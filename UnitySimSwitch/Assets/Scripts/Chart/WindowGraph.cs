using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

public class WindowGraph : MonoBehaviour {

    #region Fields
    private static WindowGraph _instance;

    [SerializeField] private Sprite _dotSprite;
    private RectTransform _graphContainer;
    private RectTransform _labelTemplateX;
    private RectTransform _labelTemplateY;
    private RectTransform _dashContainer;
    private RectTransform _dashTemplateX;
    private RectTransform _dashTemplateY;
    private List<GameObject> _gameObjectList;
    private List<IGraphVisualObject> _graphVisualObjectList;
    private GameObject _tooltipGameObject;
    private List<RectTransform> _yLabelList;

    private List<int> _valueList;
    private IGraphVisual _graphVisual;
    private int _maxVisibleValueAmount;
    private Func<int, string> _getAxisLabelX;
    private Func<float, string> _getAxisLabelY;
    private float _xSize;
    private bool _startYScaleAtZero;
    #endregion Fields

    #region Methods
    private void Awake() {
        _instance = this;
        _graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        _labelTemplateX = _graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        _labelTemplateY = _graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        _dashContainer = _graphContainer.Find("dashContainer").GetComponent<RectTransform>();
        _dashTemplateX = _dashContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        _dashTemplateY = _dashContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        _tooltipGameObject = _graphContainer.Find("tooltip").gameObject;

        _startYScaleAtZero = true;
        _gameObjectList = new List<GameObject>();
        _yLabelList = new List<RectTransform>();
        _graphVisualObjectList = new List<IGraphVisualObject>();
        
        IGraphVisual lineGraphVisual = new LineGraphVisual(_graphContainer, _dotSprite, Color.blue, new Color(1, 1, 1, .5f));
        IGraphVisual barChartVisual = new BarChartVisual(_graphContainer, Color.blue, .8f);
        
        HideTooltip();

        List<int> _valueList = new List<int>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17};
        ShowGraph(_valueList, lineGraphVisual, -1, (int _i) => "Day " + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f));

        
        /*// Automatically modify graph values and visual
        bool useBarChart = true;
        FunctionPeriodic.Create(() => {
            for (int i = 0; i < _valueList.Count; i++) {
                _valueList[i] = Mathf.RoundToInt(_valueList[i] * UnityEngine.Random.Range(0.8f, 1.2f));
                if (_valueList[i] < 0) _valueList[i] = 0;
            }
            if (useBarChart) {
                ShowGraph(_valueList, barChartVisual, -1, (int _i) => "Day " + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f));
            } else {
                ShowGraph(_valueList, lineGraphVisual, -1, (int _i) => "Day " + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f));
            }
            useBarChart = !useBarChart;
        }, .5f);*/

        int index = 0;
        FunctionPeriodic.Create(() => {
            index = (index + 1) % _valueList.Count;
        }, .1f);
        FunctionPeriodic.Create(() => {
            //int index = UnityEngine.Random.Range(0, _valueList.Count);
            UpdateValue(index, _valueList[index] + UnityEngine.Random.Range(-1, 3));
        }, .02f);
    }

    public static void ShowTooltip_Static(string tooltipText, Vector2 anchoredPosition) {
        _instance.ShowTooltip(tooltipText, anchoredPosition);
    }

    private void ShowTooltip(string tooltipText, Vector2 anchoredPosition) {
        _tooltipGameObject.SetActive(true);

        _tooltipGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

        TMP_Text tooltipUIText = _tooltipGameObject.transform.Find("text").GetComponent<TMP_Text>();
        tooltipUIText.text = tooltipText;

        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(
            tooltipUIText.preferredWidth + textPaddingSize * 2f, 
            tooltipUIText.preferredHeight + textPaddingSize * 2f
        );

        _tooltipGameObject.transform.Find("background").GetComponent<RectTransform>().sizeDelta = backgroundSize;

        _tooltipGameObject.transform.SetAsLastSibling();
    }

    public static void HideTooltip_Static() {
        _instance.HideTooltip();
    }

    private void HideTooltip() {
        _tooltipGameObject.SetActive(false);
    }

    /*private void Set_getAxisLabelX(Func<int, string> _getAxisLabelX) {
        ShowGraph(this._valueList, this._graphVisual, this._maxVisibleValueAmount, _getAxisLabelX, this._getAxisLabelY);
    }

    private void Set_getAxisLabelY(Func<float, string> _getAxisLabelY) {
        ShowGraph(this._valueList, this._graphVisual, this._maxVisibleValueAmount, this._getAxisLabelX, _getAxisLabelY);
    }

    private void IncreaseVisibleAmount() {
        ShowGraph(this._valueList, this._graphVisual, this._maxVisibleValueAmount + 1, this._getAxisLabelX, this._getAxisLabelY);
    }

    private void DecreaseVisibleAmount() {
        ShowGraph(this._valueList, this._graphVisual, this._maxVisibleValueAmount - 1, this._getAxisLabelX, this._getAxisLabelY);
    }

    private void SetGraphVisual(IGraphVisual graphVisual) {
        ShowGraph(this._valueList, graphVisual, this._maxVisibleValueAmount, this._getAxisLabelX, this._getAxisLabelY);
    }*/

    private void ShowGraph(List<int> _valueList, IGraphVisual graphVisual, int _maxVisibleValueAmount = -1, Func<int, string> _getAxisLabelX = null, Func<float, string> _getAxisLabelY = null) {
        this._valueList = _valueList;
        this._graphVisual = graphVisual;
        this._getAxisLabelX = _getAxisLabelX;
        this._getAxisLabelY = _getAxisLabelY;

        if (_maxVisibleValueAmount <= 0) {
            _maxVisibleValueAmount = _valueList.Count;
        }
        if (_maxVisibleValueAmount > _valueList.Count) {
            _maxVisibleValueAmount = _valueList.Count;
        }

        this._maxVisibleValueAmount = _maxVisibleValueAmount;

        if (_getAxisLabelX == null) {
            _getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (_getAxisLabelY == null) {
            _getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        foreach (GameObject gameObject in _gameObjectList) {
            Destroy(gameObject);
        }
        _gameObjectList.Clear();
        _yLabelList.Clear();

        foreach (IGraphVisualObject graphVisualObject in _graphVisualObjectList) {
            graphVisualObject.CleanUp();
        }
        _graphVisualObjectList.Clear();

        graphVisual.CleanUp();
        
        float graphWidth = _graphContainer.sizeDelta.x;
        float graphHeight = _graphContainer.sizeDelta.y;

        float yMinimum, yMaximum;
        CalculateYScale(out yMinimum, out yMaximum);

        _xSize = graphWidth / (_maxVisibleValueAmount + 1);

        int xIndex = 0;
        for (int i = Mathf.Max(_valueList.Count - _maxVisibleValueAmount, 0); i < _valueList.Count; i++) {
            float xPosition = _xSize + xIndex * _xSize;
            float yPosition = ((_valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            string tooltipText = _getAxisLabelY(_valueList[i]);
            IGraphVisualObject graphVisualObject = graphVisual.CreateGraphVisualObject(new Vector2(xPosition, yPosition), _xSize, tooltipText);
            _graphVisualObjectList.Add(graphVisualObject);

            RectTransform labelX = Instantiate(_labelTemplateX);
            labelX.SetParent(_graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -25f);
            labelX.GetComponent<TMP_Text>().text = _getAxisLabelX(i);
            _gameObjectList.Add(labelX.gameObject);
            
            RectTransform dashX = Instantiate(_dashTemplateX);
            dashX.SetParent(_dashContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -4f);
            _gameObjectList.Add(dashX.gameObject);

            xIndex++;
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++) {
            RectTransform labelY = Instantiate(_labelTemplateY);
            labelY.SetParent(_graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-40f, normalizedValue * graphHeight);
            labelY.GetComponent<TMP_Text>().text = _getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            _yLabelList.Add(labelY);
            _gameObjectList.Add(labelY.gameObject);

            RectTransform dashY = Instantiate(_dashTemplateY);
            dashY.SetParent(_dashContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(0, normalizedValue * graphHeight);
            _gameObjectList.Add(dashY.gameObject);
        }
    }

    private void UpdateValue(int index, int value) {
        float yMinimumBefore, yMaximumBefore;
        CalculateYScale(out yMinimumBefore, out yMaximumBefore);

        _valueList[index] = value;

        float graphWidth = _graphContainer.sizeDelta.x;
        float graphHeight = _graphContainer.sizeDelta.y;
        
        float yMinimum, yMaximum;
        CalculateYScale(out yMinimum, out yMaximum);

        bool yScaleChanged = yMinimumBefore != yMinimum || yMaximumBefore != yMaximum;

        if (!yScaleChanged) {
            float xPosition = _xSize + index * _xSize;
            float yPosition = ((value - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            string tooltipText = _getAxisLabelY(value);
            _graphVisualObjectList[index].SetGraphVisualObjectInfo(new Vector2(xPosition, yPosition), _xSize, tooltipText);
        } else {
            int xIndex = 0;
            for (int i = Mathf.Max(_valueList.Count - _maxVisibleValueAmount, 0); i < _valueList.Count; i++) {
                float xPosition = _xSize + xIndex * _xSize;
                float yPosition = ((_valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

                string tooltipText = _getAxisLabelY(_valueList[i]);
                _graphVisualObjectList[xIndex].SetGraphVisualObjectInfo(new Vector2(xPosition, yPosition), _xSize, tooltipText);

                xIndex++;
            }

            for (int i = 0; i < _yLabelList.Count; i++) {
                float normalizedValue = i * 1f / _yLabelList.Count;
                _yLabelList[i].GetComponent<TMP_Text>().text = _getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            }
        }
    }

    private void CalculateYScale(out float yMinimum, out float yMaximum) {
        yMaximum = _valueList[0];
        yMinimum = _valueList[0];
        
        for (int i = Mathf.Max(_valueList.Count - _maxVisibleValueAmount, 0); i < _valueList.Count; i++) {
            int value = _valueList[i];
            if (value > yMaximum) {
                yMaximum = value;
            }
            if (value < yMinimum) {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0) {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        if (_startYScaleAtZero) {
            yMinimum = 0f;
        }
    }
    private interface IGraphVisual {

        IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
        void CleanUp();

    }
        private interface IGraphVisualObject {

        void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
        void CleanUp();

    }

    #region BarGraph
    private class BarChartVisual : IGraphVisual {

        private RectTransform _graphContainer;
        private Color _barColor;
        private float _barWidthMultiplier;

        public BarChartVisual(RectTransform _graphContainer, Color _barColor, float _barWidthMultiplier) {
            this._graphContainer = _graphContainer;
            this._barColor = _barColor;
            this._barWidthMultiplier = _barWidthMultiplier;
        }

        public void CleanUp() {
        }

        public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText) {
            GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth);

            BarChartVisualObject barChartVisualObject = new BarChartVisualObject(barGameObject, _barWidthMultiplier);
            barChartVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);

            return barChartVisualObject;
        }

        private GameObject CreateBar(Vector2 graphPosition, float barWidth) {
            GameObject gameObject = new GameObject("bar", typeof(Image));
            gameObject.transform.SetParent(_graphContainer, false);
            gameObject.GetComponent<Image>().color = _barColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
            rectTransform.sizeDelta = new Vector2(barWidth * _barWidthMultiplier, graphPosition.y);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(.5f, 0f);
            
            Button_UI barButtonUI = gameObject.AddComponent<Button_UI>();

            return gameObject;
        }


        public class BarChartVisualObject : IGraphVisualObject {

            private GameObject barGameObject;
            private float _barWidthMultiplier;

            public BarChartVisualObject(GameObject barGameObject, float _barWidthMultiplier) {
                this.barGameObject = barGameObject;
                this._barWidthMultiplier = _barWidthMultiplier;
            }

            public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText) {
                RectTransform rectTransform = barGameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
                rectTransform.sizeDelta = new Vector2(graphPositionWidth * _barWidthMultiplier, graphPosition.y);

                Button_UI barButtonUI = barGameObject.GetComponent<Button_UI>();
                barButtonUI.MouseOverOnceFunc = () => {
                    ShowTooltip_Static(tooltipText, graphPosition);
                };

                barButtonUI.MouseOutOnceFunc = () => {
                    HideTooltip_Static();
                };
            }

            public void CleanUp() {
                Destroy(barGameObject);
            }


        }
        #endregion BarGraph

    }
    #region LineGraph
    private class LineGraphVisual : IGraphVisual {

        private RectTransform _graphContainer;
        private Sprite _dotSprite;
        private LineGraphVisualObject _lastLineGraphVisualObject;
        private Color _dotColor;
        private Color _dotConnectionColor;

        public LineGraphVisual(RectTransform _graphContainer, Sprite _dotSprite, Color _dotColor, Color _dotConnectionColor) {
            this._graphContainer = _graphContainer;
            this._dotSprite = _dotSprite;
            this._dotColor = _dotColor;
            this._dotConnectionColor = _dotConnectionColor;
            _lastLineGraphVisualObject = null;
        }

        public void CleanUp() {
            _lastLineGraphVisualObject = null;
        }


        public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText) {
            GameObject _dotGameObject = CreateDot(graphPosition);


            GameObject _dotConnectionGameObject = null;
            if (_lastLineGraphVisualObject != null) {
                _dotConnectionGameObject = CreateDotConnection(_lastLineGraphVisualObject.GetGraphPosition(), _dotGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            
            LineGraphVisualObject lineGraphVisualObject = new LineGraphVisualObject(_dotGameObject, _dotConnectionGameObject, _lastLineGraphVisualObject);
            lineGraphVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);
            
            _lastLineGraphVisualObject = lineGraphVisualObject;

            return lineGraphVisualObject;
        }

        private GameObject CreateDot(Vector2 anchoredPosition) {
            GameObject gameObject = new GameObject("dot", typeof(Image));
            gameObject.transform.SetParent(_graphContainer, false);
            gameObject.GetComponent<Image>().sprite = _dotSprite;
            gameObject.GetComponent<Image>().color = _dotColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(15, 15);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            
            Button_UI dotButtonUI = gameObject.AddComponent<Button_UI>();

            return gameObject;
        }

        private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
            GameObject gameObject = new GameObject("dotConnection", typeof(Image));
            gameObject.transform.SetParent(_graphContainer, false);
            gameObject.GetComponent<Image>().color = _dotConnectionColor;
            gameObject.GetComponent<Image>().raycastTarget = false;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
            return gameObject;
        }


        public class LineGraphVisualObject : IGraphVisualObject {

            public event EventHandler OnChangedGraphVisualObjectInfo;

            private GameObject _dotGameObject;
            private GameObject _dotConnectionGameObject;
            private LineGraphVisualObject _lastVisualObject;

            public LineGraphVisualObject(GameObject _dotGameObject, GameObject _dotConnectionGameObject, LineGraphVisualObject _lastVisualObject) {
                this._dotGameObject = _dotGameObject;
                this._dotConnectionGameObject = _dotConnectionGameObject;
                this._lastVisualObject = _lastVisualObject;

                if (_lastVisualObject != null) {
                    _lastVisualObject.OnChangedGraphVisualObjectInfo += _lastVisualObject_OnChangedGraphVisualObjectInfo;
                }
            }

            private void _lastVisualObject_OnChangedGraphVisualObjectInfo(object sender, EventArgs e) {
                UpdateDotConnection();
            }

            public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText) {
                RectTransform rectTransform = _dotGameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = graphPosition;

                UpdateDotConnection();

                Button_UI dotButtonUI = _dotGameObject.GetComponent<Button_UI>();

                dotButtonUI.MouseOverOnceFunc = () => {
                    ShowTooltip_Static(tooltipText, graphPosition);
                };
            
                dotButtonUI.MouseOutOnceFunc = () => {
                    HideTooltip_Static();
                };

                if (OnChangedGraphVisualObjectInfo != null) OnChangedGraphVisualObjectInfo(this, EventArgs.Empty);
            }

            public void CleanUp() {
                Destroy(_dotGameObject);
                Destroy(_dotConnectionGameObject);
            }

            public Vector2 GetGraphPosition() {
                RectTransform rectTransform = _dotGameObject.GetComponent<RectTransform>();
                return rectTransform.anchoredPosition;
            }

            private void UpdateDotConnection() {
                if (_dotConnectionGameObject != null) {
                    RectTransform dotConnectionRectTransform = _dotConnectionGameObject.GetComponent<RectTransform>();
                    Vector2 dir = (_lastVisualObject.GetGraphPosition() - GetGraphPosition()).normalized;
                    float distance = Vector2.Distance(GetGraphPosition(), _lastVisualObject.GetGraphPosition());
                    dotConnectionRectTransform.sizeDelta = new Vector2(distance, 3f);
                    dotConnectionRectTransform.anchoredPosition = GetGraphPosition() + dir * distance * .5f;
                    dotConnectionRectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
                }
            }
        }
    }
    #endregion LineGraph
    #endregion Methods
}