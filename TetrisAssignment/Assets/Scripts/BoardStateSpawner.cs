using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStateSpawner : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs; // Array holding references to different tetromino prefabs 
    private TetrisGrid grid;              // Reference to the TetrisGrid 
    private GameObject nextPiece;         // Reference to the next tetromino piece to spawn
    private int currentIndex = 0;         // Track the current index for the next piece

    void Start() 
    {
        grid = FindObjectOfType<TetrisGrid>(); // Find the TetrisGrid component in the scene
        if (grid == null)                      // If no TetrisGrid is found
        {
            return;                            // Exit early if no grid is found
        }
        SpawnPiece();                          // Call SpawnPiece to spawn the first tetromino piece
    }

    public void SpawnPiece() // Method responsible for spawning a new tetromino piece
    {
        Vector3 spawnPosition = new Vector3(Mathf.Floor(grid.width / 2f), grid.height, 0); // Calculate the spawn position in the middle of the grid width and at the top of the grid height

        if (nextPiece != null)         // If there is a next piece already
        {
            nextPiece.SetActive(true); // Activate the next piece (if it was deactivated previously)
            nextPiece.transform.position = spawnPosition; // Set its position to the calculated spawn position
        }
        else // If there is no next piece yet
        {
            nextPiece = InstantiateNextPiece();           // Instantiate a new piece using the InstantiateNextPiece method
            nextPiece.transform.position = spawnPosition; // Set its position to the calculated spawn position
        }

        // Prepare the next piece for the next spawn
        nextPiece = InstantiateNextPiece(); // Instantiate the next piece for future use
        nextPiece.SetActive(false);         // Deactivate it so it doesn't appear immediately
    }

    public GameObject InstantiateNextPiece() // Method that instantiates a new tetromino piece based on the current index
    {
        if (currentIndex >= tetrominoPrefabs.Length) // If the index exceeds the array length 
        {
            currentIndex = 0;  // Reset the index to 0, so we start from the beginning of the array
        }

        GameObject piece = Instantiate(tetrominoPrefabs[currentIndex]); // Instantiate the tetromino prefab at the current index
        currentIndex++;  // Move to the next piece in the array for the next spawn
        return piece;    // Return the instantiated piece
    }
}
