using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState : MonoBehaviour
{
    private TetrisGrid grid; // Declare a reference to the TetrisGrid component
    private void Start() 
    {
        grid = FindAnyObjectByType<TetrisGrid>(); // Find the first object of type TetrisGrid in the scene and assign it to the grid variable

        foreach (Transform block in transform)    // Loop through each block (Transform) that is a child of the current object 
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position); // Convert the block's position to a rounded Vector2Int 
            grid.AddBlockToGrid(block, position); // Add the block to the grid at the calculated position
        }
    }
}
