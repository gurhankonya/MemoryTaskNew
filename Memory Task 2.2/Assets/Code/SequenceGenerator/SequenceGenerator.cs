using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
///  Class to manage our sequences
/// </summary>
public class SequenceGenerator : MonoBehaviour
{
    public TextMeshProUGUI sequenceLabel;
    private List<int> _currentSequence;
    
    // TODO change the currentLength and keep increasing it maybe after each block?
    private int _currentLength;

    /// <summary>
    /// Generates a new Sequence of numbers by using the Random functionality with a range form 0 to 9
    /// </summary>
    public void GenerateSequence()
    {
        
        // initialize the List here because we want to be able to create an entirely new List
        _currentSequence = new List<int>();
        
        // fill in the sequence list with random values
        for (int i = 0; i < _currentLength; i++)
        {
            _currentSequence.Add(Mathf.FloorToInt(Random.Range(0, 9)));
        }
    }

    /// <summary>
    /// Goes through our Sequence List structure and transforms its content into a string
    /// </summary>
    /// <returns>A string of the content of our sequence i.e. 1 4 2 9</returns>
    public string GetSequenceString()
    {
        string sequenceText = "";
        
        foreach (int currentNumber in _currentSequence)
        {
            sequenceText += currentNumber;
        }

        return sequenceText;
    }

    /// <summary>
    /// Getter Method so that other classes will be able to access the Sequence we made
    /// </summary>
    /// <returns> The current sequence this class has generated in GenerateSequence()</returns>
    public List<int> GetSequence()
    {
        return _currentSequence;
    }

    public void SetSequenceLength(int length)
    {
        this._currentLength = length;
    }


    
}
