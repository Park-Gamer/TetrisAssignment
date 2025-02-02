using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void SelectLevelTetris()
    {
        SceneManager.LoadScene("Tetris"); // Loads the scene to play tetris
    }
    public void SelectLevelBoardstate()
    {
        SceneManager.LoadScene("BoardState"); // Loads the scene of board state
    }
}
