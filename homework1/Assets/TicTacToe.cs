using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MonoBehaviour
{
    private const int kRows = 3;
    private const int kCols = 3;
    private const int kWinItems = 3;
    private const int kUnitX = 30;
    private const int kUnitY = 30;

    private enum GameState {NotStarted, Started, End};
    private int[,] board = new int[kRows, kCols];
    private int turn;
    private GameState curGameState;

    // Start is called before the first frame update

    void Start()
    {
        Restart();
    }

    void Restart()
    {
        for (int i = 0; i < kRows; i++)
        {
            for (int j = 0; j < kCols; j++) {
                board[i, j] = 0;
            }
        }
        turn = 1;
        curGameState = GameState.Started;
    }

    // Update is called once per frame
    void Update()
    {

    }

    string StrCellColor(int state) {
        if (state == 1) return "x";
        if (state == -1) return "o";
        return "";
    }

    string StrUser(int user) {
        return StrCellColor(user);
    }

    string StrGameState(GameState state) {
        switch(state) {
            case GameState.NotStarted:
                return "Not Started";
            case GameState.Started:
                return "Playing";
            case GameState.End:
                return StrUser(-turn)+" Wins !";
            default:
                return "";
        }
    }

    bool IsWin(int [] line) {
        if (line[0] == 0) return false;
        for (int i = 1; i < line.Length; i++) {
            if (line[i] != line[0]) return false;
        }
        return true;
    }

    void UpdateGameState() {
        if (curGameState != GameState.Started)
            return;
        int[] line = new int[kWinItems];
        for (int i = 0; i < kRows; i++) {
            for (int j = 0; j < kCols; j++) {
                bool okRowInc = i + kWinItems - 1 < kRows;
                bool okColInc = j + kWinItems - 1 < kCols;
                bool okRowDec = i - kWinItems + 1 >= 0;
                bool okColDec = j - kWinItems + 1 >= 0;

                if (okRowInc) {
                    for (int k = 0; k < kWinItems; k++) {
                        line[k] = board[i+k, j];
                    }
                    if (IsWin(line)) {
                        curGameState = GameState.End;
                    }
                }
                if (okColInc) {
                    for (int k = 0; k < kWinItems; k++) {
                        line[k] = board[i, j+k];
                    }
                    if (IsWin(line)) {
                        curGameState = GameState.End;
                    }
                }
                if (okRowDec && okColInc) {
                    for (int k = 0; k < kWinItems; k++) {
                        line[k] = board[i-k, j+k];
                    }
                    if (IsWin(line)) {
                        curGameState = GameState.End;
                    }
                }
                if (okRowInc && okColInc) {
                    for (int k = 0; k < kWinItems; k++) {
                        line[k] = board[i+k, j+k];
                    }
                    if (IsWin(line)) {
                        curGameState = GameState.End;
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        int buttonUpperY = kCols * kUnitY + 30;
        if (GUI.Button(new Rect (0, buttonUpperY, 50, kUnitY), "Restart"))
        {
            Restart();
        }

        int labelUpperY = buttonUpperY + kUnitY + 10;

        UpdateGameState();

        GUIStyle resLabelStyle = new GUIStyle();
        resLabelStyle.fontSize = 20;
        resLabelStyle.normal.textColor = Color.red;
        GUI.Label(new Rect (0, labelUpperY, 70, kUnitY),
                  StrGameState(curGameState),
                  resLabelStyle);
        buttonUpperY = labelUpperY + kUnitY + 10;
        if (GUI.Button(new Rect (0, buttonUpperY, 50, kUnitY), "Exit")) {
            Application.Quit();
        }
        for (int i = 0; i < kRows; i++)
        {
            for (int j = 0; j < kCols; j++)
            {
                if (GUI.Button(new Rect (i * kUnitX, j * kUnitY,
                                         kUnitX, kUnitY),
                               StrCellColor(board[i, j])))
                {
                    if (board[i, j] == 0 && curGameState != GameState.End) {
                        board[i, j] = turn;
                        turn = -turn;
                    }
                }
            }
        }
    }
}

