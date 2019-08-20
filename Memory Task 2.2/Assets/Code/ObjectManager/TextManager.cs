using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class to manage our Text
/// </summary>
public class TextManager : MonoBehaviour
{
    /// <summary>
    /// The label that is to be changed
    /// </summary>
    public TextMeshProUGUI label;
    
    /// <summary>
    /// Method that changes the text of our label.
    /// Can be called by any other class e.g. experiment manager
    /// </summary>
    /// <param name="message"> the text that will be written in the label </param>
    public void ShowText(string message)
    {
        // eventually we could put the font size here as well (relevant for cubes)
        label.text = message;
    }
    
}
