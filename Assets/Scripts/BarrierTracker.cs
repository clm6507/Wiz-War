using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BarrierTracker : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject doorPrefab;
    public GameObject noBarrierPrefab;

    char[][] board1Vertical = {
            new char[] {'n', 'w', 'w', 'n', 'w'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'w', 'n', 'w', 'n', 'n'},
            new char[] {'n', 'w', 'n', 'w', 'n'},
        };
    char[][] board1Horizontal = {
            new char[] {'n', 'n', 'n', 'n'},
            new char[] {'w', 'n', 'w', 'n'},
            new char[] {'d', 'w', 'n', 'w'},
            new char[] {'n', 'w', 'n', 'w'},
            new char[] {'n', 'n', 'n', 'n'},
        };

    char[][] board2Vertical = {
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'}
        };
    char[][] board2Horizontal = {
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'}
        };

    char[][] board3Vertical = {
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'}
        };
    char[][] board3Horizontal = {
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'}
        };

    char[][] board4Vertical = {
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'}
        };
    char[][] board4Horizontal = {
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'}
        };

    char[][] board5Vertical = {
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'}
        };
    char[][] board5Horizontal = {
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'}
        };

    char[][] board6Vertical = {
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'}
        };
    char[][] board6Horizontal = {
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'},
            new char[] {'n', 'n', 'n', 'n', 'n'}
        };

    public char[][][] getBarrierRepresentation(int boardnum)
    {
        char[][][] board = new char[2][][];
        if (boardnum == 1)
        {
            board[0] = board1Vertical;
            board[1] = board1Horizontal;
        }
        else if (boardnum == 2)
        {
            board[0] = board2Vertical;
            board[1] = board2Horizontal;
        }
        else if (boardnum == 3)
        {
            board[0] = board3Vertical;
            board[1] = board3Horizontal;
        }
        else if (boardnum == 4)
        {
            board[0] = board4Vertical;
            board[1] = board4Horizontal;
        }
        else if (boardnum == 5)
        {
            board[0] = board5Vertical;
            board[1] = board5Horizontal;
        }
        else
        {
            board[0] = board6Vertical;
            board[1] = board6Horizontal;
        }

        return board;
    }
}
