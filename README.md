# MiniSyncContext

C# の async/await の内部動作を、自作イベントループと SynchronizationContext で観察するデモ。

## 概要

このプロジェクトは、.NET 9 C# のコンソールアプリケーションで、async/await の内部動作を理解するための教育的なデモです。カスタム SynchronizationContext とイベントループを実装し、非同期処理の継続（continuation）がどのように管理されるかを可視化します。

## 主要なコンポーネント

### MiniSynchronizationContext
- カスタム SynchronizationContext の実装
- async/await の継続処理をキューイング
- Post/Send メソッドで非同期作業を管理
- キューサイズと実行状況をコンソールに出力

### MiniEventLoop
- カスタムイベントループの実装
- SynchronizationContext をスレッドに設定
- キューされた作業を順次実行
- イベントループの開始/終了を管理

### Program.cs
5つのデモシナリオを実装：
1. 基本的な async/await の動作
2. 複数の async/await の連鎖
3. 複数の非同期タスクの順次実行
4. 戻り値を持つ非同期処理
5. SynchronizationContext の動作確認

## 実行方法

```bash
cd MiniSyncContext
dotnet run
```

## 動作環境

- .NET 9.0
- テストコードなし
- 外部依存ライブラリなし

## 出力例

アプリケーションを実行すると、各デモで以下のような情報が表示されます：
- イベントループの開始/終了
- キューへの作業追加
- キューサイズ
- 作業の実行状況
- SynchronizationContext の状態

これにより、async/await がどのように継続を管理し、イベントループで処理されるかを観察できます。
