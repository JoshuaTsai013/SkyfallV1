using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
public class LocaleSelector : MonoBehaviour
{
    private bool _hasActive = false;

    //localeID: 0 - zh-Hant, 1 - en
    public void ChangeLocale(int localeID)
    {
        if (_hasActive == true)
            return;
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int _localeID)
    {
        _hasActive = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        _hasActive = false;
    }
}