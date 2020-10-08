# Reversister
[English](https://translate.google.com/translate?sl=ja&tl=en&u=https://github.com/HoshikawaHikari/Reversister) (by Google Translate)

リバーシ(オセロ)のライブラリ。  
C#製。.Net Core 3  
Unityでも使用可能。  

利用可能なクラス、関数は [Documents](https://github.com/HoshikawaHikari/Reversister#Document) をご覧ください。

## Installation
[Release](https://github.com/HoshikawaHikari/Reversister/releases) ページからDLLとUnityPackageをダウンロード出来ます。  
もしくはソースコードをご利用下さい。

### Unity
向けにUnityPackageを用意しています。  
ImportAssetしてご利用下さい。  
使い方のサンプルも含まれています。

### C# Project
DLLを参照に追加してご利用下さい。  
.Net Core 3 を対象としています。

その他の環境の場合はソースコードをご利用下さい。

## License
MIT

### Document

#### \[namespace] HoshihaLab.Reversister

#### \[class] Reversi
リバーシのゲーム管理を行うメインのクラス。
|Method|Description|
|---|---|
|Reversi()|コンストラクタ。指定のマス数でゲームを初期化する|
|Flip()|指定座標に石を置く|
|Pass()|現在の手番をパスする|
|GetGameState()|現在のゲームステートを取得する|
|CheckEnd()|ゲーム終了かどうかを取得する|
|GetCell()|指定座標のマスを取得する|
|CountCell()|指定タイプのマス数を取得する|
|CountFlip()|指定タイプの石を置ける場所を取得する|
|CountFlipCell|指定座標に置いた場合にめくれる場所を取得する|
|FindFlipCell|指定座標に置いた場合にめくれる数を取得する|
|CanFlip|指定座標に指定タイプの石が置けるかどうかを取得する|
|CanPlay()|指定タイプの石を置く場所があるかどうかを取得する|
|Reset()|ゲーム情報をリセットする|

#### \[class] ReversiAIBase
AIを作成する場合のベースクラス。継承して利用する。
|Method|Description|
|---|---|
|StartGame()|ゲームの初期データを渡す|
|abstract Calc()|継承先でAIが置く場所を返すように実装して下さい|

#### \[class] SimpleReversiAI
ReversiAIBaseを使ったシンプルなAIのサンプル。
|Method|Description|
|---|---|
|override Calc()|一番多くめくれる場所を置く場所とする|

#### \[enum] GameStateType
ゲームステート
|Type|Description|
|---|---|
|BrackTurn|黒の手番|
|WhiteTurn|白の手番|
|BrackWin|黒の勝ちでゲーム終了|
|WhiteWin|白の勝ちでゲーム終了|
|Draw|引き分けでゲーム終了|

#### \[enum] CellType
マスのタイプ
|Type|Description|
|---|---|
|None|何も置かれていない|
|White|白の石が置かれている|
|Black|黒の石が置かれている|
