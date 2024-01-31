//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class List : MonoBehaviour
//{
//    int completedRowCount = 0; // 削除された行数をカウント

//    for (int y = FieldYLength - 1; y >= 0; y--)
//    {
//        bool rowIsCompleted = true; // 行が揃っているかどうか

//        for (int x = 0; x<FieldXLength; x++)
//        {
//            if (_fieldBlocks[y, x] == BlockType.None)
//            {
//                rowIsCompleted = false; // ブロックがない場合、行は揃っていない
//                break;
//            }
//        }

//        if (rowIsCompleted)
//{
//    // 揃った行を削除
//    DeleteRow(y);
//    completedRowCount++;

//    // 上の行を一つ下にずらす
//    for (int aboveY = y - 1; aboveY >= 0; aboveY--)
//    {
//        for (int x = 0; x < FieldXLength; x++)
//        {
//            _fieldBlocks[aboveY + 1, x] = _fieldBlocks[aboveY, x];
//        }
//    }
//}
//    }

//    if (completedRowCount > 0)
//{
//    // スコアを計算し、加算
//    int scoreToAdd = CalculateScore(completedRowCount);
//    AddScore(scoreToAdd);

//    // スコアを表示するなどの適切な処理を行う
//    UpdateScoreDisplay();
//}
//}

//// 行を削除
//private void DeleteRow(int row)
//{
//    for (int x = 0; x < FieldXLength; x++)
//    {
//        _fieldBlocks[row, x] = BlockType.None;
//    }
//}

//// スコアを計算
//private int CalculateScore(int completedRowCount)
//{
//    int baseScore = 100; // 1行削除時の基本得点
//    int bonusScore = 50; // ボーナス得点
//    return (baseScore + bonusScore * (completedRowCount - 1));
//}

//// スコアを加算
//private void AddScore(int scoreToAdd)
//{
//    // スコアを加算する処理をここに追加
//}

//// スコアを表示
//private void UpdateScoreDisplay()
//{
//    // スコア表示を更新する処理をここに追加
//}
//}
//}
//}
