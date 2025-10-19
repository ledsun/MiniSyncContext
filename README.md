# MiniSyncContext
C# の async/await の裏側を「イベントループ」と「SynchronizationContext」を自作して観察するデモプロジェクト。

「await の内部で何が起きているか？」を理解するための学習用実装です。

## 動作環境

- .net 9
- C# 12

## 実行方法

```bash
git clone https://github.com/ledsun/MiniSyncContext.git
cd MiniSyncContext
dotnet run
```

## 結果例

```
[tid:05] PeriodicEvents started.
[tid:11] I'm main
[tid:10] I'm sub
[tid:11] Ctx=MySyncContext
[tid:11] await 500ms
[tid:11] fibonacci 2
[tid:11] fibonacci 3
[tid:11] fibonacci 5
[tid:11] end await. await sub thread...
[tid:11] fibonacci 8
[tid:11] fibonacci 13
[tid:10] bye!
[tid:11] bye!
Done.
```
