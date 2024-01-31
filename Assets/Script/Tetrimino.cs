// -------------------------------------------------------------------------------
// Tetrimino.cs
//
// 作成日: 2023/10/18
// 作成者: 橋本竜汰
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テトリミノの動作を管理するクラス
/// </summary>
public class Tetrimino : MonoBehaviour 
{
    // テトリミノ回転パターンの定数
    private const int PATTERN_X_LENGTH = 4;
    private const int PATTERN_Y_LENGTH = 4;

    // テトリミノの基本位置と回転パターンの管理
    private Vector2Int _basePosition = default;
    private int _rollpattern = default;

    // BlockTypeプロパティの定義
    public GameManager.BlockTypes BlockType {
        get; private set;
    }
    /// <summary>
    /// テトリミノの回転パターンの数を取得するメソッド
    /// </summary>
    /// <remarks>
    /// テトリミノの種類がTetriminoOの場合は回転パターンが1つのみ
    /// それ以外のテトリミノは通常4つの回転パターンを持つ
    /// </remarks>
    private int RollPatternNum {
        get 
        {
            return BlockType == GameManager.BlockTypes.TetriminoO ? 1 : 4;
        }
    }
    /// <summary>
    /// 次の回転パターンを取得するメソッド
    /// </summary>
    private int NextRollPattern {
        get 
        {
            return _rollpattern + 1 < RollPatternNum ? _rollpattern + 1 : 0;
        }
    }
    /// <summary>
    /// テトリミノを初期化するメソッド
    /// </summary>
    /// <param name="blockType"></param>
    //テトリミノの種類、基本位置、回転パターンを初期化する
    public void TetriminoInitialize(GameManager.BlockTypes blockType = GameManager.BlockTypes.None) {
        if (blockType == GameManager.BlockTypes.None) {
            //ランダムにテトリミノを決定
            blockType = (GameManager.BlockTypes)Random.Range(1, 8);
        }
        //テトリミノが生成される位置
        _basePosition = new Vector2Int(3, 0);
        _rollpattern = default;
        BlockType = blockType;
    }
    /// <summary>
    /// 現在の回転パターンにおけるテトリミノの各ブロックの位置を取得するメソッド
    /// </summary>
    /// <returns>ブロックの位置が格納されたVector2Intの配列</returns>
    public Vector2Int[] GetBlockPositions() {
        return GetBlockPositions(_rollpattern);
    }
    /// <summary>
    /// 指定された回転パターンにおけるテトリミノの各ブロックの位置を計算して取得するメソッド
    /// </summary>
    /// <param name="rollPattern">取得したい回転パターンの番号</param>
    /// <returns>ブロックの位置が格納されたVector2Intの配列</returns>
    Vector2Int[] GetBlockPositions(int rollPattern)
        {
        Vector2Int[] positions = new Vector2Int[4];
        // テトリミノのパターンを取得
        int[,,] pattern = _typePatterns[BlockType];
        // ブロックの位置を格納する配列のインデックス
        int positionIndex = default;
        for (int y = 0; y < PATTERN_Y_LENGTH; y++)
            {
            for (int x = 0; x < PATTERN_X_LENGTH; x++)
                {
                // パターンが1の場所がブロックが存在する位置
                if (pattern[rollPattern, y, x] == 1)
                {
                    // 各ブロックの位置を計算して配列に格納
                    positions[positionIndex] = new Vector2Int(_basePosition.x + x, _basePosition.y + y);
                    positionIndex++;
                }
            }
        }
        // 計算したブロックの位置を返す
        return positions;
    }
    /// <summary>
    /// テトリミノの移動のメソッド
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Move(int x, int y) {
        //テトリミノの位置を移動
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
        // テトリミノを回転させ、次の回転パターンを設定
        _rollpattern = NextRollPattern;
    }
    // テトリミノの各種回転パターンを定義
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