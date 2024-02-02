using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //ゲームフィールドのサイズ
    const int FIELD_Y_LENGTH = 22;
    const int FIELD_X_LENGTH = 10;
    //次のテトリミノが表示されるゲームフィールドのサイズ
    const int NEXT_FIELD_X_LENGTH = 4;
    const int NEXT_FIELD_Y_LENGTH = 10;

    private int _score = default;
    private int _nscore = default;
    [SerializeField]
    private Text _scoreText;
    //メインのゲームフィールド
    [SerializeField]
    private GameObject _field;

    //次のテトリミノが表示されるフィールド
    [SerializeField]
    private GameObject _nextField;

    //ゲームオーバーテキスト
    [SerializeField]
    private GameObject _gameOverText;

    // ブロックのプレハブ
    [SerializeField]
    private SpriteRenderer _blockPrefab;

    // 効果音再生用のオーディオソース
    [SerializeField]
    private AudioSource _seAudioSource;

    // テトリミノ回転時の効果音
    [SerializeField]
    private AudioClip _rotateSe;　

    // テトリミノ移動時の効果音
    [SerializeField]
    private AudioClip _moveSe;

    // 行の消去時の効果音
    [SerializeField]
    private AudioClip _deleteSe;

    // メインゲームフィールド上のブロックを格納する配列
    private SpriteRenderer[,] _blockObjects;　

    //次のテトリミノ表示フィールド上のブロックを格納する配列
    private SpriteRenderer[,] _nextBlockObjects;

    // 現在のテトリミノ
    Tetrimino _tetrimino;  

    // 次のテトリミノ
    Tetrimino _nextTetrimino;

    // 次のテトリミノ
    Tetrimino _nextTetrimino1;

    // テトリミノの落下間隔
    private float _fallInterval = 0.3f;　

    // 落ちてるテトリミノの落下時間
    private DateTime _lastFallTime;　

    //落ちたテトリミノのコントロール時間
    private DateTime _lastControlTime;　

    //フィールド上のブロック配置を保持する二次元配列
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
    /// ゲーム全体のステータス
    /// </summary>
    public enum GameState
    {
        None,
        Playing,
        Result,

    }
    private void Start() {
        //テトリミノ取得
        GameObject ttetrimino = GameObject.Find("Tetrimino1");
        _tetrimino = ttetrimino.GetComponent<Tetrimino>();

        //次のテトリミノ生成
        _nextTetrimino = Instantiate(_tetrimino); 
        _nextTetrimino.TetriminoInitialize();
        _nextTetrimino1 = Instantiate(_tetrimino);
        _nextTetrimino1.TetriminoInitialize();
        //次のテトリミノを生成
        InitialzeBlockObjects();

        //ゲームの初期化
        GameInitialize();

    }
    private void InitialzeBlockObjects()
    {

        //ゲームフィールドのブロックオブジェクトを生成
        _blockObjects = new SpriteRenderer[FIELD_Y_LENGTH, FIELD_X_LENGTH];
        for (int y = 0; y < FIELD_Y_LENGTH; y++)
        {
            for (int x = 0; x < FIELD_X_LENGTH; x++)
            {
                SpriteRenderer block = Instantiate(_blockPrefab, _field.transform);
                //ブロックの位置設定
                block.transform.localPosition = new Vector3(x, y, 0);
                //ブロックの回転初期化
                block.transform.localRotation = Quaternion.identity;
                //ブロックのスケール初期化
                block.transform.localScale = Vector3.one;
                //ブロックの色を黒に設定
                block.color = Color.black;
                // _blockObjects 配列に生成したブロックを格納
                _blockObjects[y, x] = block;
            }
        }
            //次のゲームフィールドのブロックオブジェクト生成
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
    /// ゲームの初期化
    /// </summary>
    private void GameInitialize()
    {
        //ゲームオーバーテキストを非表示
        _gameOverText.SetActive(false);
        //テトリミノの初期化
        _tetrimino.TetriminoInitialize();
        _nextTetrimino.TetriminoInitialize();
        _nextTetrimino1.TetriminoInitialize();
        //時間の変数の初期化
        _lastFallTime = DateTime.UtcNow;
        _lastControlTime = DateTime.UtcNow;
        //ゲーム状態をプレイ中に設定
        State = GameState.Playing;
        //フィールド上のブロックオブジェクトを初期化
        for (int y = 0; y < FIELD_Y_LENGTH; y++)
        {
            for (int x = 0; x < FIELD_X_LENGTH; x++)
            {
                //フィールドにブロックオブジェクトを置く
                _fieldBlocks[y, x] = BlockTypes.None;
            }
        }
    }
    private void Update()
    {
        _scoreText.text = "SCORE: " + _score.ToString();
        switch (State)
        {
            //UpdatePlayが呼び出される
            case GameState.Playing:
                UpdatePlay();
                break;
            //UpdateResultが呼び出される
            case GameState.Result:
                UpdateResult();
                break;
        }
    }

    //ゲーム終了の処理
    private void UpdateResult()
    {
        //ゲームオーバーした後にスペースキーを押したら最初から
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //ゲームの初期化
            GameInitialize();
            State = GameState.Playing;
        }
    }
    //ゲームプレイの処理
    private void UpdatePlay()
    {
        _fallInterval = 0.3f;
        // ControlTetriminoを呼び出す
        bool controlled = ControlTetrimino();
        //現在の時間を取得
        DateTime now = DateTime.UtcNow;
        //現在の時間と最後の落下時間の差を計算して自動落下時間と比較//自動的に落下する時間になっていて
        if ((now - _lastFallTime).TotalSeconds < _fallInterval)
        {
            //操作がおこなわれていなかったら
            if (!controlled) {
                return;
            }
        }
        else
        {
            // 一定の時間間隔が経過した場合、テトリミノを下方向に移動させる
            _lastFallTime = now;

            if (!TryMoveTetrimino(0, 1))
            {
                //　テトリミノを固定
                Vector2Int[] positions = _tetrimino.GetBlockPositions();
                foreach (Vector2Int position in positions)
                {
                    _fieldBlocks[position.y, position.x] = _tetrimino.BlockType;
                }
                //行が消えたら音がなる
                if (DeleteLines())
                {
                    _seAudioSource.PlayOneShot(_deleteSe);
                }
                //テトリミノ生成
                _tetrimino.TetriminoInitialize(_nextTetrimino.BlockType);
                _nextTetrimino.TetriminoInitialize();
                _nextTetrimino.TetriminoInitialize(_nextTetrimino1.BlockType);
                _nextTetrimino1.TetriminoInitialize();
                //ゲームオーバー判定
                if (!CanMoveTetrimino(0, 0))
                {
                    _gameOverText.SetActive(true);
                    State = GameState.Result;
                }
            }
        }
        Draw();
    }

    //行が消えたかどうかの判定
    private bool DeleteLines() {
        //行を消すかの判定
        bool deleted = false;
        //揃った行を消去
        for (int y = FIELD_Y_LENGTH - 1; y >= 0;)
        {
            //その行に空白(BlockTypes.None)があるか
            bool hasBlank = false;
            for (int x = 0; x < FIELD_X_LENGTH; x++)
            {
                //その行に空白があったらtrue
                if (_fieldBlocks[y, x] == BlockTypes.None)
                {
                    hasBlank = true;
                    break;
                }
            }
            //現在の行に空白（BlockTypes.None)かじゃないか示す
            if (hasBlank)
            {
                y--;
                continue;
            }
            //行が消えた
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
            //フィールドの下から上に行をチェック
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

    //テトリミノのコントロール
    private bool ControlTetrimino()
    {
        //コントロールの感覚
        DateTime now = DateTime.UtcNow;
        if ((now - _lastControlTime).TotalSeconds < 0.1f) {
            return false;
        }
        //左矢印キーがおされた場合
        if (Input.GetKey(KeyCode.LeftArrow)) {
            if (TryMoveTetrimino(-1, 0)) {
                _seAudioSource.PlayOneShot(_moveSe);
                _lastControlTime = now;
                return true;
            }

        }
        //右矢印キーがおされた場合
        else if (Input.GetKey(KeyCode.RightArrow)) {
            if (TryMoveTetrimino(1, 0)) {
                _seAudioSource.PlayOneShot(_moveSe);
                _lastControlTime = now;
                return true;
            }
        }
        //下矢印キーがおされた場合
        else if (Input.GetKey(KeyCode.DownArrow)) {
            if (TryMoveTetrimino(0, 1)) {
                _seAudioSource.PlayOneShot(_moveSe);
                _lastControlTime = now;
                return true;
            }
        }
        //スペースキーがおされた場合　テトリミノの回転
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
    // 二次元配列を使用して、テトリミノの移動が可能かどうかを判定
    private bool TryMoveTetrimino(int moveX, int moveY)
    {
        if (CanMoveTetrimino(moveX, moveY))
        {
            _tetrimino.Move(moveX, moveY);
            return true;
        }
        return false;
    }
    //テトリミノが移動可能かチェック
    private bool CanMoveTetrimino(int moveX, int moveY)
    {
        //テトリミノの各ブロックの位置を取得
        Vector2Int[] blockpositions = _tetrimino.GetBlockPositions();
        //フィールドを超えてない,他のブロックと衝突してないかチェック
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
            // 移動先に他のブロックが存在しないかチェック
            if (_fieldBlocks[y, x] != BlockTypes.None) {
                return false;
            }
        }
        // テトリミノを指定位置に移動できる場合は true を返す
        return true;
    }

    // テトリミノを回転させ、回転に成功した場合 true を返すメソッド
    private bool TryRollTetrimino()
    {
        if(CanRollTetrimino())
        {
            _tetrimino.Roll();
            return true;
        }
        return false;
    }
    // テトリミノを回転できるかどうかをチェックするメソッド
    private bool CanRollTetrimino()
    {
        //回転した後のブロックの位置を配列にいれる
        Vector2Int[] blockPositions = _tetrimino.GetRolledBlockPositions();
        foreach (Vector2Int blockPosition in blockPositions)
        {
            int x = blockPosition.x;
            int y = blockPosition.y;
            // フィールドの境界外かどうかをチェック
            if (x < 0 || x >= FIELD_X_LENGTH) {
                return false;
            }

            if (y < 0 || y >= FIELD_Y_LENGTH) {
                return false;
            }
            // 回転後の位置に他のブロックが存在しないかチェック
            if (_fieldBlocks[y, x] != BlockTypes.None) {
                return false;
            }
        }
        // テトリミノを回転できる場合は true を返す
        return true;
    }
    /// <summary>
    /// オブジェクトの描画
    /// </summary>
    private void Draw()
    {
        //フィールド描画
        for (int y = 0; y < FIELD_Y_LENGTH; y++)
        {
            for (int x = 0; x < FIELD_X_LENGTH; x++)
            {
                SpriteRenderer blockObj = _blockObjects[y, x];
                BlockTypes blockType = _fieldBlocks[y, x];
                blockObj.color = GetBlockColor(blockType);
            }
        }
        //ミノ描画
        {
            Vector2Int[] positions = _tetrimino.GetBlockPositions();
            foreach (Vector2Int position in positions)
            {
                SpriteRenderer tetriminoBlock = _blockObjects[position.y, position.x];
                tetriminoBlock.color = GetBlockColor(_tetrimino.BlockType);
            }
        }
        //nextフィールド描画
        for (int y = 0; y < NEXT_FIELD_Y_LENGTH; y++)
        {
            for (int x = 0; x < NEXT_FIELD_X_LENGTH; x++)
            {
                _nextBlockObjects[y, x].color = GetBlockColor(BlockTypes.None);
            }
        }
        //nextミノ描画
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
    //テトリミノの色を取得
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
