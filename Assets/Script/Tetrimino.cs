// -------------------------------------------------------------------------------
// Tetrimino.cs
//
// �쐬��: 2023/10/18
// �쐬��: ���{����
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�g���~�m�̓�����Ǘ�����N���X
/// </summary>
public class Tetrimino : MonoBehaviour 
{
    // �e�g���~�m��]�p�^�[���̒萔
    private const int PATTERN_X_LENGTH = 4;
    private const int PATTERN_Y_LENGTH = 4;

    // �e�g���~�m�̊�{�ʒu�Ɖ�]�p�^�[���̊Ǘ�
    private Vector2Int _basePosition = default;
    private int _rollpattern = default;

    // BlockType�v���p�e�B�̒�`
    public GameManager.BlockTypes BlockType {
        get; private set;
    }
    /// <summary>
    /// �e�g���~�m�̉�]�p�^�[���̐����擾���郁�\�b�h
    /// </summary>
    /// <remarks>
    /// �e�g���~�m�̎�ނ�TetriminoO�̏ꍇ�͉�]�p�^�[����1�̂�
    /// ����ȊO�̃e�g���~�m�͒ʏ�4�̉�]�p�^�[��������
    /// </remarks>
    private int RollPatternNum {
        get 
        {
            return BlockType == GameManager.BlockTypes.TetriminoO ? 1 : 4;
        }
    }
    /// <summary>
    /// ���̉�]�p�^�[�����擾���郁�\�b�h
    /// </summary>
    private int NextRollPattern {
        get 
        {
            return _rollpattern + 1 < RollPatternNum ? _rollpattern + 1 : 0;
        }
    }
    /// <summary>
    /// �e�g���~�m�����������郁�\�b�h
    /// </summary>
    /// <param name="blockType"></param>
    //�e�g���~�m�̎�ށA��{�ʒu�A��]�p�^�[��������������
    public void TetriminoInitialize(GameManager.BlockTypes blockType = GameManager.BlockTypes.None) {
        if (blockType == GameManager.BlockTypes.None) {
            //�����_���Ƀe�g���~�m������
            blockType = (GameManager.BlockTypes)Random.Range(1, 8);
        }
        //�e�g���~�m�����������ʒu
        _basePosition = new Vector2Int(3, 0);
        _rollpattern = default;
        BlockType = blockType;
    }
    /// <summary>
    /// ���݂̉�]�p�^�[���ɂ�����e�g���~�m�̊e�u���b�N�̈ʒu���擾���郁�\�b�h
    /// </summary>
    /// <returns>�u���b�N�̈ʒu���i�[���ꂽVector2Int�̔z��</returns>
    public Vector2Int[] GetBlockPositions() {
        return GetBlockPositions(_rollpattern);
    }
    /// <summary>
    /// �w�肳�ꂽ��]�p�^�[���ɂ�����e�g���~�m�̊e�u���b�N�̈ʒu���v�Z���Ď擾���郁�\�b�h
    /// </summary>
    /// <param name="rollPattern">�擾��������]�p�^�[���̔ԍ�</param>
    /// <returns>�u���b�N�̈ʒu���i�[���ꂽVector2Int�̔z��</returns>
    Vector2Int[] GetBlockPositions(int rollPattern)
        {
        Vector2Int[] positions = new Vector2Int[4];
        // �e�g���~�m�̃p�^�[�����擾
        int[,,] pattern = _typePatterns[BlockType];
        // �u���b�N�̈ʒu���i�[����z��̃C���f�b�N�X
        int positionIndex = default;
        for (int y = 0; y < PATTERN_Y_LENGTH; y++)
            {
            for (int x = 0; x < PATTERN_X_LENGTH; x++)
                {
                // �p�^�[����1�̏ꏊ���u���b�N�����݂���ʒu
                if (pattern[rollPattern, y, x] == 1)
                {
                    // �e�u���b�N�̈ʒu���v�Z���Ĕz��Ɋi�[
                    positions[positionIndex] = new Vector2Int(_basePosition.x + x, _basePosition.y + y);
                    positionIndex++;
                }
            }
        }
        // �v�Z�����u���b�N�̈ʒu��Ԃ�
        return positions;
    }
    /// <summary>
    /// �e�g���~�m�̈ړ��̃��\�b�h
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Move(int x, int y) {
        //�e�g���~�m�̈ʒu���ړ�
        _basePosition.Set(_basePosition.x + x, _basePosition.y + y);
    }
    /// <summary>
    /// �e�g���~�m�̎��̉�]�p�^�[���ɂ�����
    /// �e�g���~�m�̊e�u���b�N�̈ʒu���擾���郁�\�b�h
    /// </summary>
    /// <returns>���̉�]�p�^�[���ɂ�����e�g���~�m�̊e�u���b�N�̈ʒu</returns>
    public Vector2Int[] GetRolledBlockPositions() {
        return GetBlockPositions(NextRollPattern);
    }
    /// <summary>
    /// �e�g���~�m�̉�]���\�b�h
    /// </summary>
    public void Roll() {
        // �e�g���~�m����]�����A���̉�]�p�^�[����ݒ�
        _rollpattern = NextRollPattern;
    }
    // �e�g���~�m�̊e���]�p�^�[�����`
    static readonly Dictionary<GameManager.BlockTypes, int[,,]> _typePatterns = new()
    {
        // �e�e�g���~�m�̉�]�p�^�[�����`
        // GameManager.BlockTypes �̓e�g���~�m�̎�ނ�\���񋓌^�ŁA���ꂼ��̃e�g���~�m�̌`��1��0�ŕ\��
        // 1�̓u���b�N�����݂��邱�Ƃ�����0�̓u���b�N�����݂��Ȃ����Ƃ�\��
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