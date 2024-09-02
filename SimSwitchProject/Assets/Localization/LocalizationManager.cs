using System.Linq;  
using UnityEngine;  
using UnityEngine.Localization.Settings;
public class LocalizationManager : MonoBehaviour
{
    public void SetLocale(string code)  
    {  
        var localeQuery = (from locale in LocalizationSettings.AvailableLocales.Locales  
        where locale.Identifier.Code == code 
        select locale).FirstOrDefault();  

        if (localeQuery == null)  
        {        
            Debug.LogError($"No locale for {code} found");  
            return;  
        }    

        LocalizationSettings.SelectedLocale = localeQuery;  
    }
}