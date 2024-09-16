using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SubmitData : MonoBehaviour
{
    #region Fields
    [Header("Game Objects")]
    public GameObject _inputName;
    public GameObject _inputCountry;
    public GameObject _inputAge;
    public ToggleGroup _colorOptions;

    private string _name;
    private string _country;
    private string _age;
    private string _color;

    private string _url = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSe5twZraCfzEH2l4ia6OQI5PGV_y7Dd174hI8mnkumyOs7IOw/formResponse";
    #endregion Fields

    #region Methods
    public void Submit()
    {
        _name = _inputName.GetComponent<TMP_InputField>().text;
        _country = _inputCountry.GetComponent<TMP_InputField>().text;
        _age = _inputAge.GetComponent<TMP_InputField>().text;
        _color = _colorOptions.GetFirstActiveToggle().transform.GetChild(1).GetComponent<Text>().text;

        StartCoroutine(Post(_name, _country, _age, _color));
        Debug.Log("Name: " + _name + " / " + "Country: " + _country + " / " + "Age: " + _age + " / " + "Color: " + _color);
    }
    IEnumerator Post(string name, string country, string age, string color)
    {
        Debug.Log("Post Started");
        WWWForm form = new WWWForm();

        form.AddField("entry.1076880842", name);
        form.AddField("entry.5829197", country);
        form.AddField("entry.1527904835", age);
        form.AddField("entry.720532905", color);

        UnityWebRequest www = UnityWebRequest.Post(_url, form);

        yield return www.SendWebRequest();
    }
    #endregion Methods
}