using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //�Q�[���t�B�[���h�̃T�C�Y
    const int FIELD_Y_LENGTH = 22;
    const int FIELD_X_LENGTH = 10;
    //���̃e�g���~�m���\�������Q�[���t�B�[���h�̃T�C�Y
    const int NEXT_FIELD_X_LENGTH = 4;
    const int NEXT_FIELD_Y_LENGTH = 10;

    private int _score = default;
    private int _nscore = default;
    [SerializeField]
    private Text _scoreText;
    //���C���̃Q�[���t�B�[���h
    [SerializeField]
    private GameObject _field;

    //���̃e�g���~�m���\�������t�B�[���h
    [SerializeField]
    private GameObject _nextField;

    //�Q�[���I�[�o�[�e�L�X�g
    [SerializeField]
    private GameObject _gameOverText;

    // �u���b�N�̃v���n�u
    [SerializeField]
    private SpriteRenderer _blockPrefab;

    // ���ʉ��Đ��p�̃I�[�f�B�I�\�[�X
    [SerializeField]
    private AudioSource _seAudioSource;

    // �e�g���~�m��]���̌��ʉ�
    [SerializeField]
    private AudioClip _rotateSe;�@

    // �e�g���~�m�ړ����̌��ʉ�
    [SerializeField]
    private AudioClip _moveSe;

    // �s�̏������̌��ʉ�
    [SerializeField]
    private AudioClip _deleteSe;

    // ���C���Q�[���t�B�[���h��̃u���b�N���i�[����z��
    private SpriteRenderer[,] _blockObjects;�@

    //���̃e�g���~�m�\���t�B�[���h��̃u���b�N���i�[����z��
    private SpriteRenderer[,] _nextBlockObjects;

    // ���݂̃e�g���~�m
    Tetrimino _tetrimino;  

    // ���̃e�g���~�m
    Tetrimino _nextTetrimino;

    // ���̃e�g���~�m
    Tetrimino _nextTetrimino1;

    // �e�g���~�m�̗����Ԋu
    private float _fallInterval = 0.3f;�@

    // �����Ă�e�g���~�m�̗�������
    private DateTime _lastFallTime;�@

    //�������e�g���~�m�̃R���g���[������
    private DateTime _lastControlTime;�@

    //�t�B�[���h��̃u���b�N�z�u��ێ�����񎟌��z��
    private BlockTypes[,] _fieldBlocks;

    public enum BlockTypes
    {
        None = 0,
        TetriminoI = 1,
        TetriminoO = 2,
        TetriminoS = 3,
        TetriminoZ = 4,
        TetriminoJ = 5,
        TetriminoL = 6,
        TetriminoT = 7,
    }

    private GameState State { get; set; } = GameState.None;

    /// <summary>
    /// �Q�[���S�̂̃X�e�[�^�X
    /// </summary>
    public enum GameState
    {
        None,
        Playing,
        Result,

    }
    private void Start() {
        //�e�g���~�m�擾
        GameObject ttetrimino = GameObject.Find("Tetrimino1");
        _tetrimino = ttetrimino.GetComponent<Tetrimino>();

        //���̃e�g���~�m����
        _nextTetrimino = Instantiate(_tetrimino); 
        _nextTetrimino.TetriminoInitialize();
        _nextTetrimino1 = Instantiate(_tetrimino);
        _nextTetrimino1.TetriminoInitialize();
        //���̃e�g���~�m�𐶐�
        InitialzeBlockObjects();

        //�Q�[���̏�����
        GameInitialize();

    }
    private void InitialzeBlockObjects()
    {

        //�Q�[���t�B�[���h�̃u���b�N�I�u�W�F�N�g�𐶐�
        _blockObjects = new SpriteRenderer[FIELD_Y_LENGTH, FIELD_X_LENGTH];
        for (int y = 0; y < FIELD_Y_LENGTH; y++)
        {
            for (int x = 0; x < FIELD_X_LENGTH; x++)
            {
                SpriteRenderer block = Instantiate(_blockPrefab, _field.transform);
                //�u���b�N�̈ʒu�ݒ�
                block.transform.localPosition = new Vector3(x, y, 0);
                //�u���b�N�̉�]������
                block.transform.localRotation = Quaternion.identity;
                //�u���b�N�̃X�P�[��������
                block.transform.localScale = Vector3.one;
                //�u���b�N�̐F�����ɐݒ�
                block.color = Color.black;
                // _blockObjects �z��ɐ��������u���b�N���i�[
                _blockObjects[y, x] = block;
            }
        }
            //���̃Q�[���t�B�[���h�̃u���b�N�I�u�W�F�N�g����
            _nextBlockObjects = new SpriteRenderer[NEXT_FIELD_Y_LENGTH, NEXT_FIELD_X_LENGTH];
        for (int y = 0; y < NEXT_FIELD_Y_LENGTH; y++)
        {
            for (int x = 0; x < NEXT_FIELD_X_LENGTH; x++)
            {
                SpriteRenderer block = Instantiate(_blockPrefab, _nextField.transform);
                block.transform.localPosition = new Vector3(x, y, 0);
                block.transform.localRotation = Quaternion.identity;
                block.transform.localScale = Vector3.one;
                block.color = Color.black;
                _nextBlockObjects[y, x] = block;
            }
        }

        _fieldBlocks = new BlockTypes[FIELD_Y_LENGTH, FIELD_X_LENGTH];
    }
    /// <summary>
    /// �Q�[���̏�����
    /// </summary>
    private void GameInitialize()
    {
        //�Q�[���I�[�o�[�e�L�X�g���\��
        _gameOverText.SetActive(false);
        //�e�g���~�m�̏�����
        _tetrimino.TetriminoInitialize();
        _nextTetrimino.TetriminoInitialize();
        _nextTetrimino1.TetriminoInitialize();
        //���Ԃ̕ϐ��̏�����
        _lastFallTime = DateTime.UtcNow;
        _lastControlTime = DateTime.UtcNow;
        //�Q�[����Ԃ��v���C���ɐݒ�
        State = GameState.Playing;
        //�t�B�[���h��̃u���b�N�I�u�W�F�N�g��������
        for (int y = 0; y < FIELD_Y_LENGTH; y++)
        {
            for (int x = 0; x < FIELD_X_LENGTH; x++)
            {
                //�t�B�[���h�Ƀu���b�N�I�u�W�F�N�g��u��
                _fieldBlocks[y, x] = BlockTypes.None;
            }
        }
    }
    private void Update()
    {
        _scoreText.text = "SCORE: " + _score.ToString();
        switch (State)
        {
            //UpdatePlay���Ăяo�����
            case GameState.Playing:
                UpdatePlay();
                break;
            //UpdateResult���Ăяo�����
            case GameState.Result:
                UpdateResult();
                break;
        }
    }

    //�Q�[���I���̏���
    private void UpdateResult()
    {
        //�Q�[���I�[�o�[������ɃX�y�[�X�L�[����������ŏ�����
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //�Q�[���̏�����
            GameInitialize();
            State = GameState.Playing;
        }
    }
    //�Q�[���v���C�̏���
    private void UpdatePlay()
    {
        _fallInterval = 0.3f;
        // ControlTetrimino���Ăяo��
        bool controlled = ControlTetrimino();
        //���݂̎��Ԃ��擾
        DateTime now = DateTime.UtcNow;
        //���݂̎��ԂƍŌ�̗������Ԃ̍����v�Z���Ď����������ԂƔ�r//�����I�ɗ������鎞�ԂɂȂ��Ă���
        if ((now - _lastFallTime).TotalSeconds < _fallInterval)
        {
            //���삪�����Ȃ��Ă��Ȃ�������
            if (!controlled) {
                return;
            }
        }
        else
        {
            // ���̎��ԊԊu���o�߂����ꍇ�A�e�g���~�m���������Ɉړ�������
            _lastFallTime = now;

            if (!TryMoveTetrimino(0, 1))
            {
                //�@�e�g���~�m���Œ�
                Vector2Int[] positions = _tetrimino.GetBlockPositions();
                foreach (Vector2Int position in positions)
                {
                    _fieldBlocks[position.y, position.x] = _tetrimino.BlockType;
                }
                //�s���������特���Ȃ�
                if (DeleteLines())
                {
                    _seAudioSource.PlayOneShot(_deleteSe);
                }
                //�e�g���~�m����
                _tetrimino.TetriminoInitialize(_nextTetrimino.BlockType);
                _nextTetrimino.TetriminoInitialize();
                _nextTetrimino.TetriminoInitialize(_nextTetrimino1.BlockType);
                _nextTetrimino1.TetriminoInitialize();
                //�Q�[���I�[�o�[����
                if (!CanMoveTetrimino(0, 0))
                {
                    _gameOverText.SetActive(true);
                    State = GameState.Result;
                }
            }
        }
        Draw();
    }

    //�s�����������ǂ����̔���
    private bool DeleteLines() {
        //�s���������̔���
        bool deleted = false;
        //�������s������
        for (int y = FIELD_Y_LENGTH - 1; y >= 0;)
        {
            //���̍s�ɋ�(BlockTypes.None)�����邩
            bool hasBlank = false;
            for (int x = 0; x < FIELD_X_LENGTH; x++)
            {
                //���̍s�ɋ󔒂���������true
                if (_fieldBlocks[y, x] == BlockTypes.None)
                {
                    hasBlank = true;
                    break;
                }
            }
            //���݂̍s�ɋ󔒁iBlockTypes.None)������Ȃ�������
            if (hasBlank)
            {
                y--;
                continue;
            }
            //�s��������
            deleted = true;
            //int intTest = Convert.ToInt32(deleted);
            //print(intTest);
            if (deleted == true) {
               _nscore++;
            }
            if (_nscore == 1) {
                _score =+ 400;
            }
            if (_nscore == 2) {
                _score = +800;
            }
            if (_nscore == 4) {
                _score = +1600;
            }
            //�t�B�[���h�̉������ɍs���`�F�b�N
            for (int downY = y; downY >= 0; downY--)
            {
                for (int x = 0; x < FIELD_X_LENGTH; x++)
                {
                    _fieldBlocks[downY, x] = downY == 0 ? BlockTypes.None : _fieldBlocks[downY - 1,x];
                }
            }
        }
        return deleted;
    }

    //�e�g���~�m�̃R���g���[��
    private bool ControlTetrimino()
    {
        //�R���g���[���̊��o
        DateTime now = DateTime.UtcNow;
        if ((now - _lastControlTime).TotalSeconds < 0.1f) {
            return false;
        }
        //�����L�[�������ꂽ�ꍇ
        if (Input.GetKey(KeyCode.LeftArrow)) {
            if (TryMoveTetrimino(-1, 0)) {
                _seAudioSource.PlayOneShot(_moveSe);
                _lastControlTime = now;
                return true;
            }

        }
        //�E���L�[�������ꂽ�ꍇ
        else if (Input.GetKey(KeyCode.RightArrow)) {
            if (TryMoveTetrimino(1, 0)) {
                _seAudioSource.PlayOneShot(_moveSe);
                _lastControlTime = now;
                return true;
            }
        }
        //�����L�[�������ꂽ�ꍇ
        else if (Input.GetKey(KeyCode.DownArrow)) {
            if (TryMoveTetrimino(0, 1)) {
                _seAudioSource.PlayOneShot(_moveSe);
                _lastControlTime = now;
                return true;
            }
        }
        //�X�y�[�X�L�[�������ꂽ�ꍇ�@�e�g���~�m�̉�]
        else if (Input.GetKeyDown(KeyCode.Space)) {
            if (TryRollTetrimino()) {
                _seAudioSource.PlayOneShot(_rotateSe);
                _lastControlTime = now;
                return true;
            }
        } else if (Input.GetKey(KeyCode.H)) {
            _fallInterval = 0.0001f;
            return true;
        }
        return false;
    }
    // �񎟌��z����g�p���āA�e�g���~�m�̈ړ����\���ǂ����𔻒�
    private bool TryMoveTetrimino(int moveX, int moveY)
    {
        if (CanMoveTetrimino(moveX, moveY))
        {
            _tetrimino.Move(moveX, moveY);
            return true;
        }
        return false;
    }
    //�e�g���~�m���ړ��\���`�F�b�N
    private bool CanMoveTetrimino(int moveX, int moveY)
    {
        //�e�g���~�m�̊e�u���b�N�̈ʒu���擾
        Vector2Int[] blockpositions = _tetrimino.GetBlockPositions();
        //�t�B�[���h�𒴂��ĂȂ�,���̃u���b�N�ƏՓ˂��ĂȂ����`�F�b�N
        foreach (Vector2Int blockPositions in blockpositions)
        {
            int x = blockPositions.x + moveX;
            int y = blockPositions.y + moveY;
            if (x < 0 || x >= FIELD_X_LENGTH) {
                return false;
            }

            if (y < 0 || y >= FIELD_Y_LENGTH) {
                return false;
            }
            // �ړ���ɑ��̃u���b�N�����݂��Ȃ����`�F�b�N
            if (_fieldBlocks[y, x] != BlockTypes.None) {
                return false;
            }
        }
        // �e�g���~�m���w��ʒu�Ɉړ��ł���ꍇ�� true ��Ԃ�
        return true;
    }

    // �e�g���~�m����]�����A��]�ɐ��������ꍇ true ��Ԃ����\�b�h
    private bool TryRollTetrimino()
    {
        if(CanRollTetrimino())
        {
            _tetrimino.Roll();
            return true;
        }
        return false;
    }
    // �e�g���~�m����]�ł��邩�ǂ������`�F�b�N���郁�\�b�h
    private bool CanRollTetrimino()
    {
        //��]������̃u���b�N�̈ʒu��z��ɂ����
        Vector2Int[] blockPositions = _tetrimino.GetRolledBlockPositions();
        foreach (Vector2Int blockPosition in blockPositions)
        {
            int x = blockPosition.x;
            int y = blockPosition.y;
            // �t�B�[���h�̋��E�O���ǂ������`�F�b�N
            if (x < 0 || x >= FIELD_X_LENGTH) {
                return false;
            }

            if (y < 0 || y >= FIELD_Y_LENGTH) {
                return false;
            }
            // ��]��̈ʒu�ɑ��̃u���b�N�����݂��Ȃ����`�F�b�N
            if (_fieldBlocks[y, x] != BlockTypes.None) {
                return false;
            }
        }
        // �e�g���~�m����]�ł���ꍇ�� true ��Ԃ�
        return true;
    }
    /// <summary>
    /// �I�u�W�F�N�g�̕`��
    /// </summary>
    private void Draw()
    {
        //�t�B�[���h�`��
        for (int y = 0; y < FIELD_Y_LENGTH; y++)
        {
            for (int x = 0; x < FIELD_X_LENGTH; x++)
            {
                SpriteRenderer blockObj = _blockObjects[y, x];
                BlockTypes blockType = _fieldBlocks[y, x];
                blockObj.color = GetBlockColor(blockType);
            }
        }
        //�~�m�`��
        {
            Vector2Int[] positions = _tetrimino.GetBlockPositions();
            foreach (Vector2Int position in positions)
            {
                SpriteRenderer tetriminoBlock = _blockObjects[position.y, position.x];
                tetriminoBlock.color = GetBlockColor(_tetrimino.BlockType);
            }
        }
        //next�t�B�[���h�`��
        for (int y = 0; y < NEXT_FIELD_Y_LENGTH; y++)
        {
            for (int x = 0; x < NEXT_FIELD_X_LENGTH; x++)
            {
                _nextBlockObjects[y, x].color = GetBlockColor(BlockTypes.None);
            }
        }
        //next�~�m�`��
        {
            {
                Vector2Int[] positions = _nextTetrimino.GetBlockPositions();
                foreach (Vector2Int position in positions) {
                    SpriteRenderer tetriminoBlock = _nextBlockObjects[position.y + 1, position.x -3];
                    tetriminoBlock.color = GetBlockColor(_nextTetrimino.BlockType);
                }
                Vector2Int[] positions1 = _nextTetrimino1.GetBlockPositions();
                foreach (Vector2Int position in positions1) {
                    SpriteRenderer tetriminoBlock1 = _nextBlockObjects[position.y + 5, position.x - 3];
                    tetriminoBlock1.color = GetBlockColor(_nextTetrimino1.BlockType);
                }
            }
        
        }
    }
    //�e�g���~�m�̐F���擾
    private Color GetBlockColor(BlockTypes blockType)
    {
        switch(blockType)
        {
            case BlockTypes.None:
                return Color.black;
            case BlockTypes.TetriminoI:
                return Color.cyan;
            case BlockTypes.TetriminoO:
                return Color.yellow;
            case BlockTypes.TetriminoS:
                return Color.green;
            case BlockTypes.TetriminoZ:
                return Color.red;
            case BlockTypes.TetriminoJ:
                return Color.blue;
            case BlockTypes.TetriminoL:
                return new Color(0f, 0.5f, 0f);
            case BlockTypes.TetriminoT:
                return Color.magenta;
            default:
                return Color.white;
        }
    }
}
