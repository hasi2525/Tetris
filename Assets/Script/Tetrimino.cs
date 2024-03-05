// -------------------------------------------------------------------------------
// Tetrimino.cs
//
// 作成日: 2023/10/18
// 作成者: 橋本竜汰
// -------------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テトリミノの動作を管理するクラス
/// </summary>
public class Tetrimino : MonoBehaviour {
    private const int PATTERN_X_LENGTH = 4;
    private const int PATTERN_Y_LENGTH = 4;

    private Vector2Int _basePosition = default;
    private int _rollPattern = default;

    // テトリミノのブロックの種類の最小値
    private const int MIN_BLOCK_TYPE_VALUE = 1;
    // テトリミノのブロックの種類の最大値
    private const int MAX_BLOCK_TYPE_VALUE = 7;

    // テトリミノの初期基本位置
    private static readonly Vector2Int _initialBasePosition = new Vector2Int(3, 0);

    /// <summary>
    /// テトリミノのブロックの種類を取得
    /// </summary>
    //BlockTypeプロパティの定義
    public GameManager.BlockTypes BlockType {
        get; private set;
    }

    /// <summary>
    /// テトリミノの回転パターンの数を取得するメソッド
    /// </summary>
    /// <returns>テトリミノの回転パターン</returns>
    private int RollPatternNum {
        get {
            if (BlockType == GameManager.BlockTypes.TetriminoO) {
                return 1;
            } else {
                return 4;
            }
        }
    }

    /// <summary>
    /// 次の回転パターンを取得するメソッド
    /// </summary>
    /// <returns>次の回転パターン</returns>
    private int NextRollPattern {
        get {
            if (_rollPattern + 1 < RollPatternNum) {
                return _rollPattern + 1;
            } else {
                return 0;
            }
        }
    }

    /// <summary>
    /// テトリミノを初期化
    /// </summary>
    /// <param name="blockType">テトリミノのブロックの種類</param>
    /// <remarks>
    /// ブロックの種類がランダムに決定
    /// </remarks>
    public void TetriminoInitialize(GameManager.BlockTypes blockType = GameManager.BlockTypes.None) {
        if (blockType == GameManager.BlockTypes.None) {
            blockType = (GameManager.BlockTypes)Random.Range(MIN_BLOCK_TYPE_VALUE, MAX_BLOCK_TYPE_VALUE + 1);
        }
        //テトリミノが生成される位置
        _basePosition = _initialBasePosition;
        // テトリミノの回転パターンを初期化
        _rollPattern = default;
        // ブロックの種類
        BlockType = blockType;
    }

    /// <summary>
    /// 現在の回転パターンにおけるテトリミノの各ブロックの位置を取得するメソッド
    /// </summary>
    /// <returns>ブロックの位置が格納されたVector2Intの配列</returns>
    public Vector2Int[] GetBlockPositions() {
        return GetBlockPositions(_rollPattern);
    }

    /// <summary>
    /// 指定された回転パターンにおけるテトリミノの各ブロックの位置を計算して取得するメソッド
    /// </summary>
    /// <param name="rollPattern">取得したい回転パターンの番号</param>
    /// <returns>ブロックの位置が格納されたVector2Intの配列</returns>
    Vector2Int[] GetBlockPositions(int rollPattern) {
        Vector2Int[] positions = new Vector2Int[4];
        int[,,] pattern = _typePatterns[BlockType];
        int positionIndex = default;

        for (int y = 0; y < PATTERN_Y_LENGTH; y++) {
            for (int x = 0; x < PATTERN_X_LENGTH; x++) {
                if (pattern[rollPattern, y, x] == 1) {
                    positions[positionIndex].x = _basePosition.x + x;
                    positions[positionIndex].y = _basePosition.y + y;
                    positionIndex++;
                }
            }
        }

        return positions;
    }

    /// <summary>
    /// テトリミノの移動のメソッド
    /// </summary>
    /// <param name="x">横の移動量</param>
    /// <param name="y">縦の移動量</param>
    public void Move(int x, int y) {
        _basePosition.Set(_basePosition.x + x, _basePosition.y + y);
    }

    /// <summary>
    /// テトリミノの次の回転パターンにおける
    /// テトリミノの各ブロックの位置を取得するメソッド
    /// </summary>
    /// <returns>次の回転パターンにおけるテトリミノの各ブロックの位置</returns>
    public Vector2Int[] GetRolledBlockPositions() {
        return GetBlockPositions(NextRollPattern);
    }

    /// <summary>
    /// テトリミノの回転メソッド
    /// </summary>
    public void Roll() {
        _rollPattern = NextRollPattern;
    }

    static readonly Dictionary<GameManager.BlockTypes, int[,,]> _typePatterns = new()
    {
        // 各テトリミノの回転パターンを定義
        // GameManager.BlockTypes はテトリミノの種類を表す列挙型で、それぞれのテトリミノの形を1と0で表す
        // 1はブロックが存在することを示し0はブロックが存在しないことを表す
        {
            GameManager.BlockTypes.TetriminoI,
            new int[,,]
            {
                {
                    {0, 0, 0, 0 },
                    {1, 1, 1, 1 },
                    {0, 0, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 0, 1, 0 },
                    {0, 0, 1, 0 },
                    {0, 0, 1, 0 },
                    {0, 0, 1, 0 },
                },
                {
                    {0, 0, 0, 0 },
                    {0, 0, 0, 0 },
                    {1, 1, 1, 1 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 1, 0, 0 },
                    {0, 1, 0, 0 },
                    {0, 1, 0, 0 },
                    {0, 1, 0, 0 },
                },
            }
        },
        {
            GameManager.BlockTypes.TetriminoO,
            new int[,,]
            {
                {
                    {0, 1, 1, 0 },
                    {0, 1, 1, 0 },
                    {0, 0, 0, 0 },
                    {0, 0, 0, 0 },
                },
            }
        },
        {
            GameManager.BlockTypes.TetriminoS,
            new int[,,]
            {
                {
                    {1, 0, 0, 0 },
                    {1, 1, 0, 0 },
                    {0, 1, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 0, 0, 0 },
                    {0, 1, 1, 0 },
                    {1, 1, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {1, 0, 0, 0 },
                    {1, 1, 0, 0 },
                    {0, 1, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 0, 0, 0 },
                    {0, 1, 1, 0 },
                    {1, 1, 0, 0 },
                    {0, 0, 0, 0 },
                },
            }
        },
        {
            GameManager.BlockTypes.TetriminoZ,
            new int[,,]
            {
                {
                    {1, 1, 0, 0 },
                    {0, 1, 1, 0 },
                    {0, 0, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 0, 1, 0 },
                    {0, 1, 1, 0 },
                    {0, 1, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 0, 0, 0 },
                    {1, 1, 0, 0 },
                    {0, 1, 1, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 1, 0, 0 },
                    {1, 1, 0, 0 },
                    {1, 0, 0, 0 },
                    {0, 0, 0, 0 },
                },
            }
        },
        {
            GameManager.BlockTypes.TetriminoJ,
            new int[,,]
            {
                {
                    {1, 0, 0, 0 },
                    {1, 1, 1, 0 },
                    {0, 0, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 1, 1, 0 },
                    {0, 1, 0, 0 },
                    {0, 1, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 0, 0, 0 },
                    {1, 1, 1, 0 },
                    {0, 0, 1, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 1, 0, 0 },
                    {0, 1, 0, 0 },
                    {1, 1, 0, 0 },
                    {0, 0, 0, 0 },
                },
            }
        },
        {
            GameManager.BlockTypes.TetriminoL,
            new int[,,]
            {
                {
                    {0, 0, 1, 0 },
                    {1, 1, 1, 0 },
                    {0, 0, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 1, 0, 0 },
                    {0, 1, 0, 0 },
                    {0, 1, 1, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 0, 0, 0 },
                    {1, 1, 1, 0 },
                    {1, 0, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {1, 1, 0, 0 },
                    {0, 1, 0, 0 },
                    {0, 1, 0, 0 },
                    {0, 0, 0, 0 },
                },
            }
        },
        {
            GameManager.BlockTypes.TetriminoT,
            new int[,,]
            {
                {
                    {0, 1, 0, 0 },
                    {1, 1, 1, 0 },
                    {0, 0, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 1, 0, 0 },
                    {0, 1, 1, 0 },
                    {0, 1, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 0, 0, 0 },
                    {1, 1, 1, 0 },
                    {0, 1, 0, 0 },
                    {0, 0, 0, 0 },
                },
                {
                    {0, 1, 0, 0 },
                    {1, 1, 0, 0 },
                    {0, 1, 0, 0 },
                    {0, 0, 0, 0 },
                },
            }
        },
    };
}