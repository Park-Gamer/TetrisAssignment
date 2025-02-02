using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    public int width = 10;    // The width of the grid 
    public int height = 20;   // The height of the grid 
    public Transform[,] grid; // Array to hold references to the blocks placed in the grid
    private TetrisManager tManager; // Reference to the TetrisManager 

    void Awake() 
    {
        tManager = FindObjectOfType<TetrisManager>(); // Find and assign the first TetrisManager in the scene

        grid = new Transform[width, height + 3]; // Initialize the grid array to hold Transform references
        for (int i = 0; i < width; i++)          // Loop through the width
        {
            for (int j = 0; j < height; j++)     // Loop through the height 
            {
                GameObject cell = new GameObject($"Cell ({i},{j})"); // Create a new GameObject for each cell in the grid
                cell.transform.position = new Vector3(i, j, 0);      // Set the position of the cell in the grid
            }
        }
    }

    public void AddBlockToGrid(Transform block, Vector2Int position) // Method to add a block to a specific position in the grid
    {
        grid[position.x, position.y] = block; // Assign the block to the grid cell at the given position
    }

    public bool IsCellOccupied(Vector2Int position) // Method to check if a specific cell in the grid is occupied
    {
        if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height + 3) // Check if the position is out of bounds
        {
            return true; // If the position is out of bounds, consider it occupied
        }
        return grid[position.x, position.y] != null; // Return true if the cell at the given position is not null 
    }

    public bool IsLineFull(int rowNumber) // Method to check if a specific line is full
    {
        for (int x = 0; x < width; x++)   // Loop through each column in the given row
        {
            if (grid[x, rowNumber] == null) // If any cell in the row is empty
            {
                return false; // Return false, the line is not full
            }
        }
        return true; // If all cells in the row are occupied, return true
    }

    public void ClearLine(int rowNumber) // Method to clear a full line at the given row number
    {
        for (int x = 0; x < width; x++)  // Loop through each column in the row
        {
            Destroy(grid[x, rowNumber].gameObject); // Destroy the block (GameObject) at this position
            grid[x, rowNumber] = null;   // Set the cell in the grid to null, as it's now empty
        }
    }

    public void ClearFullLines() // Method to clear all full lines in the grid
    {
        int linesCleared = 0; // Variable to track the number of lines cleared

        for (int y = 0; y < height; y++) // Loop through each row in the grid
        {
            if (IsLineFull(y)) // If the current row is full
            {
                ClearLine(y);     // Clear the row
                ShiftRowsDown(y); // Shift all rows above the cleared row down by one
                y--;              // Stay on the same row after clearing and shifting 
                linesCleared++;   // Increment the number of lines cleared
            }
        }
        if (linesCleared > 0) // If at least one line was cleared
        {
            tManager.CalculateScore(linesCleared); // Calculate and update the score based on the number of lines cleared
        }
    }

    public void ShiftRowsDown(int clearedRow) // Method to shift all rows above the cleared row down
    {
        for (int y = clearedRow; y < height - 1; y++) // Loop from the cleared row to the second-to-last row
        {
            for (int x = 0; x < width; x++) // Loop through each column in the row
            {
                grid[x, y] = grid[x, y + 1]; // Move the block from the row above into the current row
                if (grid[x, y] != null) // If there's a block in the current position
                {
                    grid[x, y].position += Vector3.down; // Move the block down by one row
                }
                grid[x, y + 1] = null; // Clear the cell above (the block has been moved down)
            }
        }
    }
}
