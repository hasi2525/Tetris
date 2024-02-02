// -------------------------------------------------------------------------------
// Tetrimino.cs
//
// �쐬��: 2023/10/18
// �쐬��: ���{����
// -------------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�g���~�m�̓�����Ǘ�����N���X
/// </summary>
public class Tetrimino : MonoBehaviour {
    private const int PATTERN_X_LENGTH = 4;
    private const int PATTERN_Y_LENGTH = 4;

    private Vector2Int _basePosition = default;
    private int _rollPattern = default;

    // �e�g���~�m�̃u���b�N�̎�ނ̍ŏ��l
    private const int MIN_BLOCK_TYPE_VALUE = 1;
    // �e�g���~�m�̃u���b�N�̎�ނ̍ő�l
    private const int MAX_BLOCK_TYPE_VALUE = 7;

    // �e�g���~�m�̏�����{�ʒu
    private static readonly Vector2Int _initialBasePosition = new Vector2Int(3, 0);

    /// <summary>
    /// �e�g���~�m�̃u���b�N�̎�ނ��擾
    /// </summary>
    //BlockType�v���p�e�B�̒�`
    public GameManager.BlockTypes BlockType {
        get; private set;
    }

    /// <summary>
    /// �e�g���~�m�̉�]�p�^�[���̐����擾���郁�\�b�h
    /// </summary>
    /// <returns>�e�g���~�m�̉�]�p�^�[��</returns>
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
    /// ���̉�]�p�^�[�����擾���郁�\�b�h
    /// </summary>
    /// <returns>���̉�]�p�^�[��</returns>
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
    /// �e�g���~�m��������
    /// </summary>
    /// <param name="blockType">�e�g���~�m�̃u���b�N�̎��</param>
    /// <remarks>
    /// �u���b�N�̎�ނ������_���Ɍ���
    /// </remarks>
    public void TetriminoInitialize(GameManager.BlockTypes blockType = GameManager.BlockTypes.None) {
        if (blockType == GameManager.BlockTypes.None) {
            blockType = (GameManager.BlockTypes)Random.Range(MIN_BLOCK_TYPE_VALUE, MAX_BLOCK_TYPE_VALUE + 1);
        }
        //�e�g���~�m�����������ʒu
        _basePosition = _initialBasePosition;
        // �e�g���~�m�̉�]�p�^�[����������
        _rollPattern = default;
        // �u���b�N�̎��
        BlockType = blockType;
    }

    /// <summary>
    /// ���݂̉�]�p�^�[���ɂ�����e�g���~�m�̊e�u���b�N�̈ʒu���擾���郁�\�b�h
    /// </summary>
    /// <returns>�u���b�N�̈ʒu���i�[���ꂽVector2Int�̔z��</returns>
    public Vector2Int[] GetBlockPositions() {
        return GetBlockPositions(_rollPattern);
    }

    /// <summary>
    /// �w�肳�ꂽ��]�p�^�[���ɂ�����e�g���~�m�̊e�u���b�N�̈ʒu���v�Z���Ď擾���郁�\�b�h
    /// </summary>
    /// <param name="rollPattern">�擾��������]�p�^�[���̔ԍ�</param>
    /// <returns>�u���b�N�̈ʒu���i�[���ꂽVector2Int�̔z��</returns>
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
    /// �e�g���~�m�̈ړ��̃��\�b�h
    /// </summary>
    /// <param name="x">���̈ړ���</param>
    /// <param name="y">�c�̈ړ���</param>
    public void Move(int x, int y) {
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
        _rollPattern = NextRollPattern;
    }

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
