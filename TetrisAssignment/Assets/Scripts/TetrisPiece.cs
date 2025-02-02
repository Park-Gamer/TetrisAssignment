using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisPiece : MonoBehaviour
{
    private TetrisGrid grid;         // Reference to the TetrisGrid to interact with the game grid
    private float dropInterval = 1f; // The interval at which the piece should automatically drop
    private float dropTimer;         // Timer to keep track of when the next automatic drop should occur

    void Start() 
    {
        grid = FindObjectOfType<TetrisGrid>(); // Find and assign the first TetrisGrid component in the scene
        dropTimer = dropInterval;              // Set the drop timer to the initial drop interval
    }

    void Update() 
    {
        HandleAutomaticDrop(); // Call the method to handle automatic piece dropping

        // Check if the player presses the left arrow key and move the piece left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector3.left);
        }

        // Check if the player presses the right arrow key and move the piece right
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector3.right);
        }

        // Check if the player presses the down arrow key and move the piece down
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector3.down);
        }

        // Check if the player presses the up arrow key and rotate the piece
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotatePiece();
        }
    }

    public void Move(Vector3 direction) // Method to move the piece in a given direction
    {
        transform.position += direction; // Move the piece in the specified direction

        if (!IsValidPosition()) // If the new position is not valid 
        {
            transform.position -= direction; // Undo the move 

            if (direction == Vector3.down) // If the piece was moved down
            {
                LockPiece(); // Lock the piece in place on the grid 
            }
        }
    }

    private bool IsValidPosition() // Method to check if the current position of the piece is valid
    {
        foreach (Transform block in transform) // Loop through each block that makes up the tetromino piece
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position); // Round the block's position to a grid-aligned position

            if (grid.IsCellOccupied(position)) // If the cell where the block is located is already occupied
            {
                return false; // The position is invalid
            }
        }
        return true; // The position is valid if none of the blocks are colliding with others
    }

    private void RotatePiece() // Method to rotate the piece 90 degrees
    {
        Vector3 originalPosition = transform.position; // Save the original position of the piece
        Quaternion originalRotation = transform.rotation; // Save the original rotation of the piece

        transform.Rotate(0, 0, 90); // Rotate the piece 90 degrees clockwise

        if (!IsValidPosition()) // If the new rotated position is invalid
        {
            if (!TryWallKick(originalPosition, originalRotation)) // Try to apply wall kicks 
            {
                transform.position = originalPosition; // If wall kicks fail, revert to the original position
                transform.rotation = originalRotation; // Revert to the original rotation
            }
        }
    }

    private void HandleAutomaticDrop() // Method to handle the automatic downward movement of the piece
    {
        dropTimer -= Time.deltaTime; // Decrease the drop timer by the time passed since the last frame

        if (dropTimer < 0) // If the timer reaches zero 
        {
            Move(Vector3.down); // Move the piece down by one unit
            dropTimer = dropInterval; // Reset the drop timer to the initial drop interval
        }
    }

    public void LockPiece() // Method to lock the piece in place and add it to the grid
    {
        foreach (Transform block in transform) // Loop through each block of the piece
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position); // Get the position of the block, rounded to grid coordinates
            grid.AddBlockToGrid(block, position); // Add the block to the grid at the specified position
        }

        grid.ClearFullLines(); // Check and clear any full lines in the grid

        // Spawn a new piece if the TetrisSpawner exists
        if (FindObjectOfType<TetrisSpawner>() != null)
        {
            FindObjectOfType<TetrisSpawner>().SpawnPiece();
        }
        // If no TetrisSpawner exists, check for the BoardStateSpawner and spawn a piece if found
        else if (FindAnyObjectByType<BoardStateSpawner>())
        {
            FindObjectOfType<BoardStateSpawner>().SpawnPiece();
        }

        // If a BoardStateManager is found, increase the move count
        if (FindAnyObjectByType<BoardStateManager>() != null)
        {
            FindAnyObjectByType<BoardStateManager>().IncreaseMoveCount();
        }

        Destroy(this); // Destroy the current script after the piece has been locked (it's no longer needed)
    }

    private bool TryWallKick(Vector3 originalPosition, Quaternion originalRotation) // Method to try and perform wall kicks when rotating the piece
    {
        Vector2Int[] wallKickOffsets = new Vector2Int[] // Array of possible offsets to try for wall kicks (adjustments to fit the piece)
        {
        new Vector2Int(1, 0),  // Move Right
        new Vector2Int(-1, 0), // Move Left
        new Vector2Int(0, -1), // Move Down
        new Vector2Int(1, -1), // Move Diagonally right-down
        new Vector2Int(-1, -1),// Move Diagonally left-down

        new Vector2Int(2, 0),  // Move Right by 2
        new Vector2Int(-2, 0), // Move Left
        new Vector2Int(0, -2), // Move Down
        new Vector2Int(2, -1), // Move Diagonally right-down
        new Vector2Int(-2, -1),// Move Diagonally left-down
        new Vector2Int(2, -2),
        new Vector2Int(-2, -2),

        new Vector2Int(3, 0),  // Move Right by 3
        new Vector2Int(-3, 0), // Move Left
        new Vector2Int(0, -3), // Move Down
        new Vector2Int(3, -1), // Move Diagonally right-down
        new Vector2Int(3, -2),
        new Vector2Int(-3, -2), // Move Diagonally left-down
        new Vector2Int(3, -3),
        new Vector2Int(-3, -3),
        };

        foreach (Vector2Int offset in wallKickOffsets) // Loop through each wall kick offset
        {
            transform.position += (Vector3Int)offset; // Try moving the piece by the offset

            if (IsValidPosition()) // If the new position is valid after applying the offset
            {
                return true; // Successful wall kick
            }

            transform.position -= (Vector3Int)offset; // Undo the offset if the position is still invalid
        }

        return false; // Return false if no wall kick was successful
    }
}
