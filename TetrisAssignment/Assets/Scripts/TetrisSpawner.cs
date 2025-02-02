using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs; // Array to hold references to different tetromino prefab GameObjects
    private TetrisGrid grid;              // Reference to the TetrisGrid where tetrominoes will be placed
    private GameObject nextPiece;         // Reference to the next tetromino piece to spawn

    void Start() 
    {
        grid = FindObjectOfType<TetrisGrid>(); // Find and assign the first TetrisGrid component in the scene
        if (grid == null) // If no TetrisGrid component is found
        {
            return;       // Exit the Start method if grid is not found 
        }
        SpawnPiece();     // Call the SpawnPiece method to spawn the first piece when the game starts
    }

    public void SpawnPiece() // Method to spawn the next tetromino piece
    {
        Vector3 spawnPosition = new Vector3(Mathf.Floor(grid.width / 2f), grid.height, 0); // Calculate the spawn position in the center of the grid width, at the top of the grid height

        if (nextPiece != null) // If there is already a next piece 
        {
            nextPiece.SetActive(true); // Activate the next piece 
            nextPiece.transform.position = spawnPosition; // Set its position to the calculated spawn position
        }
        else // If there is no next piece yet
        {
            nextPiece = InstantiateRandomPiece();         // Instantiate a new random piece using InstantiateRandomPiece method
            nextPiece.transform.position = spawnPosition; // Set its position to the calculated spawn position
        }
        nextPiece = InstantiateRandomPiece(); // Instantiate a new random piece for the next spawn
        nextPiece.SetActive(false);           // Deactivate the new piece so it doesn't appear immediately
    }

    public GameObject InstantiateRandomPiece() // Method to instantiate a random tetromino piece from the prefab array
    {
        int index = Random.Range(0, tetrominoPrefabs.Length); // Generate a random index to pick a random tetromino prefab from the array
        return Instantiate(tetrominoPrefabs[index]);          // Instantiate and return the selected random tetromino prefab
    }
}
