using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void QuitApplication()
    {
        Debug.Log("Exiting the application...");
        Application.Quit();
    }
}
