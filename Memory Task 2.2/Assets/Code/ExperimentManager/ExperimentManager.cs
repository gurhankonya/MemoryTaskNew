﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using UnityEditor;

public class ExperimentManager : MonoBehaviour
{
    [Header("Experiment Components: ")]
    public SequenceGenerator sequenceGenerator;
    public TextManager textManager;

    [Header("Block variables: ")]
    public int numberOfBlocks;
    public int numberOfTasks;

    private int _currentBlockNumber;
    private int _currentTaskNumber;
    private int _sequenceLength = 5;

    // Manage time:
    private float _stimuliStartTime;
    private float _responseTime;

    private bool _isBlockRunning;
    // We use _isPressable to block our UI while we wait. In this way the user cannot press any buttons while
    // they should be memorizing the digits
    private bool _isPressable;

    private List<int> _participantSequence;
    
    // time during which we freeze the UI while the participant memorizes the digits
    public int sleepieTime;

    // Data and Headers for the csv-file we write
    private List<List<string>> _data;
    private List<string> _csvHeaders;


    /// <summary>
    /// Initialize our private variables
    /// Public variables are initialized in Unity
    /// </summary>
    private void Start()
    {
        _currentTaskNumber = 0;
        _currentBlockNumber = 0;
        // the first sequences have 5 items, will be increased in each new block
        sequenceGenerator.SetSequenceLength(_sequenceLength);

        _isBlockRunning = false;
        _isPressable = true;

        // TODO CSV stuff
        _data = new List<List<string>>();
        _csvHeaders = new List<string>();
        _csvHeaders.Add("Block Number");
        _csvHeaders.Add("Task Number");
        _csvHeaders.Add("Sequence Length");
        _csvHeaders.Add("Shown Sequence");
        _csvHeaders.Add("Participant Sequence");
        _csvHeaders.Add("Answered Correctly");
        _csvHeaders.Add("Reaction Time");

    }

    
    /// <summary>
    /// Reacts to the participant being either right or wrong.
    /// Saves the collected data in a CSV file
    /// starts the next task after the output of the current task was analyzed by calling StartNextTask()
    /// </summary>
    /// <param name="participantWasRight"> Boolean that tells us whether the participant's sequence corresponds to the
    /// correct sequence generated by the sequenceGenerator object</param>
    private void ParticipantAnswered(bool participantWasRight)
    {
        Debug.Log(string.Format("Participant was: {0}", participantWasRight));
        _responseTime = Time.realtimeSinceStartup;

        List<string> taskData = new List<string>();

        taskData.Add(_currentBlockNumber.ToString());
        taskData.Add(_currentTaskNumber.ToString());
        taskData.Add(_sequenceLength.ToString());
        taskData.Add(sequenceGenerator.GetSequenceString());
        taskData.Add(getParticipantSequenceString());
        taskData.Add(participantWasRight.ToString());
        taskData.Add((_responseTime - _stimuliStartTime).ToString());
        _data.Add(taskData);

        StartNextTask();
    }
    
    /// <summary>
    /// Check given input from our user and continue the program accordingly
    /// TODO feedback if answer was right/wrong, add the possible answers
    /// </summary>
    private void Update()
    {
        // we only want to update our view while _isPressable is set to true and we are not waiting
        if (_isPressable)
        {
            if (_isBlockRunning)
            {
                // TODO change in VR

                if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
                {
                    _participantSequence.Add(0);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    _participantSequence.Add(1);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                {
                    _participantSequence.Add(2);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    _participantSequence.Add(3);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                {
                    _participantSequence.Add(4);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
                {
                    _participantSequence.Add(5);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
                {
                    _participantSequence.Add(6);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
                {
                    _participantSequence.Add(7);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
                {
                    _participantSequence.Add(8);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
                {
                    _participantSequence.Add(9);
                }
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    foreach (int i in _participantSequence)
                    {
                        Debug.Log("The participantSequence was:" + i);
                    }

                    bool participantWasCorrect = CompareSequences();
                    ParticipantAnswered(participantWasCorrect);

                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartNextBlock();
                }
            }
        }
    }

    /// <summary>
    /// Method to compare whether the sequence we have generated and the sequence the participant gave us are the same
    /// </summary>
    /// <returns> Bool isCorrect is true when both sequences are equal and false when there are one or more mistakes
    /// </returns>
    private bool CompareSequences()
    {
        List<int> correctSequence = sequenceGenerator.GetSequence();
        bool isCorrect = true;

        // if the length of both sequences is not equal, the method automatically returns false
        Debug.LogFormat("The correct sequence is {0}, the participantSequence is {1} long", correctSequence.Count, _participantSequence.Count);
        if (correctSequence.Count != _participantSequence.Count)
        {
            Debug.Log("Your Sequence is not as long as the given sequence!");
            return false;
        }
        
        for (int i = 0; i < correctSequence.Count; i++)
        {
            if (correctSequence[i] != _participantSequence[i])
            {
                isCorrect = false;
            }
        }

        return isCorrect;
    }

    /// <summary>
    /// Method to get the next task. While we have not surpassed the trial limit we set, we move on to the next trial.
    /// When we have surpassed the limit, we start a new block by calling the method StartNextBlock().
    /// After each task begins, the program should wait for a few seconds (the amount of time can be set in unity) after
    /// which the sequence will disappear
    /// </summary>
    private async void StartNextTask()
    {
        // Debug.Log("Current Task number is: " + _currentTaskNumber);
        
        // initiate the participantSequence here instead of in Start() since there needs to be a new one every time our
        // current task changes
        _participantSequence = new List<int>();
        
        // if our tasks are not yet complete, we create a new sequence and continue
        if (_currentTaskNumber < numberOfTasks)
        {
            sequenceGenerator.GenerateSequence();
            Debug.LogFormat("String: {0}",sequenceGenerator.GetSequenceString());
            ShowMessage(sequenceGenerator.GetSequenceString());

            // right before we start to wait, we set _isPressable to false so our view will not be updated for this time
            _isPressable = false;
            // wait for x seconds (saved in variable at the beginning of our code) where UI freezes so the participant
            // cannot do anything except for memorizing the digits
            await Task.Delay(sleepieTime * 1000);
            _isPressable = true;

            // this will vanish the sequence after the wait is over (after sleepieTime has passed)
            ShowMessage("");

            _stimuliStartTime = Time.realtimeSinceStartup;
            _currentTaskNumber++;
        }
        else
        {
            //
            if (_currentBlockNumber < numberOfBlocks)
            {
                // TODO change all press SPACE etc instances to something that can be done in VR!
                ShowMessage("End of Block. Press SPACE to continue");
            }
            else
            {
                // TODO can't we just directly print the end message here instead of putting it in StartNextBlock()?
                StartNextBlock();
            }

            _isBlockRunning = false;

        }
    }

    /// <summary>
    /// Start a new block while we have not surpassed the block limit we set. If we have surpassed our block limit,
    /// the experiment will end
    /// </summary>
    private void StartNextBlock()
    {
        // Debug.Log("Current Block number is: " + _currentBlockNumber);
        if (_currentBlockNumber < numberOfBlocks)
        {
            // Reset our _currentTaskNumber to 0 by calling the method ResetTaskNumber()
            ResetTaskNumber();
            _isBlockRunning = true;
            _currentBlockNumber++;
            sequenceGenerator.SetSequenceLength(_sequenceLength++);
            StartNextTask();
            
        }
        else
        {
            ShowMessage("THE END... Now please add your credit card number... for a... prize?");
            // Generating and saving a csv file with all the collected data
            List<string> csvLines = CSVTools.GenerateCSV(_data, _csvHeaders);
            foreach (string line in csvLines)
            {
                Debug.Log(line);
            }

            CSVTools.SaveCSV(csvLines, Application.dataPath + "/Data/" + GUID.Generate());
        }
    }

    /// <summary>
    /// Method for adding a custom text message wherever we need it
    /// </summary>
    /// <param name="message"> the text message we want to show</param>
    private void ShowMessage(string message)
    {
        textManager.ShowText(message);
    }

    /// <summary>
    /// Resets our current Task Number so that we can increment it once again in a new block
    /// </summary>
    private void ResetTaskNumber()
    {
        _currentTaskNumber = 0;
    }

    private string getParticipantSequenceString()
    {
        string seq = "";
        foreach(int digit in _participantSequence)
        {
            seq += digit.ToString();
        }
        return seq;
    }
    
}
