using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInputField;

    public static string playerName;

    private void Start()
    {
        // Optionally, you can add a listener to handle input change in real-time
        playerNameInputField.onValueChanged.AddListener(OnNameChanged);
    }

    private void OnDestroy()
    {
        // Clean up the listener when the object is destroyed
        playerNameInputField.onValueChanged.RemoveListener(OnNameChanged);
    }

    // This method is called when the input field value changes
    private void OnNameChanged(string newName)
    {
        playerName = newName;
        Debug.Log("Player Name: " + playerName);
    }

    // This method can be called when you want to retrieve the player name
    public void SavePlayerName()
    {
        playerName = playerNameInputField.text;
        Debug.Log("Player Name Saved: " + playerName);

        // You can now use playerName to save or process further
    }
}

