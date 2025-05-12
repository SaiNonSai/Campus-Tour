using UnityEngine;

public class TimetableLinkOpener : MonoBehaviour
{
    private string timetableURL;

    public void SetTimetableURL(string url)
    {
        timetableURL = url;

        if (string.IsNullOrEmpty(url))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void OpenTimetable()
    {
        if (!string.IsNullOrEmpty(timetableURL))
        {
            Application.OpenURL("https://" + timetableURL);
        }
    }
}
