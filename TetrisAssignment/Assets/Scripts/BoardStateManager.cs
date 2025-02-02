using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardStateManager : MonoBehaviour
{
    private int moveCount = 0;                  // Variable to track the number of moves made by the player
    [SerializeField] GameObject gameOverText;   // Reference to the GameObject displaying the game over text
    [SerializeField] GameObject gameWinText;    // Reference to the GameObject displaying the game win text
    [SerializeField] BoardStateSpawner spawner; // Reference to the object responsible for spawning in the board state
    [SerializeField] TetrisManager manager;     // Reference to the TetrisManager to access game-related data like score

    private void Update() 
    {
        if (moveCount == 7) // If the player has made 7 moves without solving the board state
        {
            gameOverText.SetActive(true);        // Activate the game over text 
            spawner.gameObject.SetActive(false); // Deactivate the spawner to stop further spawning
            Invoke("ReloadScene", 5);            // Invoke the ReloadScene method after a 5-second delay
        }
        else if (manager.score == 1400) // If the player's score reaches 1400 (indicating a win)
        {
            gameWinText.SetActive(true);         // Activate the game win text to show it to the player
            spawner.gameObject.SetActive(false); // Deactivate the spawner to stop further spawning
            Invoke("ReloadScene", 5);            // Invoke the ReloadScene method after a 5-second delay
        }
    }

    public void IncreaseMoveCount() 
    {
        moveCount++; // Increment the move count by 1
    }

    public void ReloadScene() 
    {
        SceneManager.LoadScene("LevelSelect"); // Load the "LevelSelect" scene
    }
}
