//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class List : MonoBehaviour
//{
//    int completedRowCount = 0; // �폜���ꂽ�s�����J�E���g

//    for (int y = FieldYLength - 1; y >= 0; y--)
//    {
//        bool rowIsCompleted = true; // �s�������Ă��邩�ǂ���

//        for (int x = 0; x<FieldXLength; x++)
//        {
//            if (_fieldBlocks[y, x] == BlockType.None)
//            {
//                rowIsCompleted = false; // �u���b�N���Ȃ��ꍇ�A�s�͑����Ă��Ȃ�
//                break;
//            }
//        }

//        if (rowIsCompleted)
//{
//    // �������s���폜
//    DeleteRow(y);
//    completedRowCount++;

//    // ��̍s������ɂ��炷
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
//    // �X�R�A���v�Z���A���Z
//    int scoreToAdd = CalculateScore(completedRowCount);
//    AddScore(scoreToAdd);

//    // �X�R�A��\������Ȃǂ̓K�؂ȏ������s��
//    UpdateScoreDisplay();
//}
//}

//// �s���폜
//private void DeleteRow(int row)
//{
//    for (int x = 0; x < FieldXLength; x++)
//    {
//        _fieldBlocks[row, x] = BlockType.None;
//    }
//}

//// �X�R�A���v�Z
//private int CalculateScore(int completedRowCount)
//{
//    int baseScore = 100; // 1�s�폜���̊�{���_
//    int bonusScore = 50; // �{�[�i�X���_
//    return (baseScore + bonusScore * (completedRowCount - 1));
//}

//// �X�R�A�����Z
//private void AddScore(int scoreToAdd)
//{
//    // �X�R�A�����Z���鏈���������ɒǉ�
//}

//// �X�R�A��\��
//private void UpdateScoreDisplay()
//{
//    // �X�R�A�\�����X�V���鏈���������ɒǉ�
//}
//}
//}
//}
