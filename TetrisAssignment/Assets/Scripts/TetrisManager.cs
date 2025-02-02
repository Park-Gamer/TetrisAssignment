using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TetrisManager : MonoBehaviour
{
    private TetrisGrid grid;                    // Reference to the TetrisGrid to interact with the game grid
    [SerializeField] GameObject gameOverText;   // Reference to the GameObject that displays the game over message
    [SerializeField] TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component that displays the score
    public int score;                           // Variable to keep track of the player's score

    [SerializeField] TetrisSpawner spawner;     // Reference to the TetrisSpawner that spawns new tetrominoes

    void Start() 
    {
        grid = FindAnyObjectByType<TetrisGrid>(); // Find and assign the first TetrisGrid component in the scene
    }

    void Update() 
    {
        CheckGameOver(); // Call the CheckGameOver method to see if the game is over
        scoreText.text = "Score: " + score; // Update the score display with the current score
    }

    public void CalculateScore(int linesCleared) // Method to calculate the score based on the number of lines cleared
    {
        switch (linesCleared) // Check how many lines were cleared
        {
            case 1:
                score += 100; // If 1 line was cleared, add 100 points to the score
                break;
            case 2:
                score += 300; // If 2 lines were cleared, add 300 points to the score
                break;
            case 3:
                score += 500; // If 3 lines were cleared, add 500 points to the score
                break;
            case 4:
                score += 800; // If 4 lines were cleared, add 800 points to the score
                break;
        }
    }

    public void CheckGameOver() // Method to check if the game is over
    {
        for (int i = 0; i < grid.width; i++) // Loop through each column at the top row of the grid
        {
            if (grid.IsCellOccupied(new Vector2Int(i, grid.height - 1))) // Check if the cell at the top row is occupied
            {
                gameOverText.SetActive(true);        // Show the game over text
                spawner.gameObject.SetActive(false); // Deactivate the spawner to stop spawning new tetrominoes
                Invoke("ReloadScene", 5);            // Call the ReloadScene method after a 5-second delay to reload the scene
            }
        }
    }

    public void ReloadScene() // Method to reload the scene
    {
        SceneManager.LoadScene("LevelSelect"); // Load the "LevelSelect" scene 
    }
}
