using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SubmitData : MonoBehaviour
{
    #region Fields

    [Header("Game Objects")]
    
    [Tooltip("Input field for the user's name.")]
    public GameObject _inputName;
    
    [Tooltip("Input field for the user's country.")]
    public GameObject _inputCountry;
    
    [Tooltip("Input field for the user's age.")]
    public GameObject _inputAge;
    
    [Tooltip("Toggle group for the user's color preference.")]
    public ToggleGroup _colorOptions;

    private string _name;
    private string _country;
    private string _age;
    private string _color;

    private string _url = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSe5twZraCfzEH2l4ia6OQI5PGV_y7Dd174hI8mnkumyOs7IOw/formResponse";  // URL for Google Form submission

    #endregion Fields

    #region Methods

    /// <summary>
    /// Collects user input data and triggers the data submission process.
    /// </summary>
    public void Submit()
    {
        // Get values from the input fields and the selected toggle for color preference
        _name = _inputName.GetComponent<TMP_InputField>().text;
        _country = _inputCountry.GetComponent<TMP_InputField>().text;
        _age = _inputAge.GetComponent<TMP_InputField>().text;
        _color = _colorOptions.GetFirstActiveToggle().transform.GetChild(1).GetComponent<Text>().text;

        // Call the Post coroutine to submit the collected data to the form
        StartCoroutine(Post(_name, _country, _age, _color));

        // Log the collected data for debugging purposes
        Debug.Log("Name: " + _name + " / " + "Country: " + _country + " / " + "Age: " + _age + " / " + "Color: " + _color);
    }

    /// <summary>
    /// Sends the collected data as a POST request to the Google Form.
    /// </summary>
    /// <param name="name">The name of the user.</param>
    /// <param name="country">The country of the user.</param>
    /// <param name="age">The age of the user.</param>
    /// <param name="color">The color preference of the user.</param>
    /// <returns>An IEnumerator for coroutine handling.</returns>
    IEnumerator Post(string name, string country, string age, string color)
    {
        // Log the start of the POST request for debugging purposes
        Debug.Log("Post Started");

        // Create a new form to submit the data
        WWWForm form = new WWWForm();

        // Add form fields and corresponding values to the form
        form.AddField("entry.1076880842", name);  // Name field ID
        form.AddField("entry.5829197", country);  // Country field ID
        form.AddField("entry.1527904835", age);  // Age field ID
        form.AddField("entry.720532905", color); // Color preference field ID

        // Create a POST request to submit the form data to the specified URL
        UnityWebRequest www = UnityWebRequest.Post(_url, form);

        // Wait for the request to complete
        yield return www.SendWebRequest();
    }

    #endregion Methods
}