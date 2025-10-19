namespace MiniSyncContext;

/// <summary>
/// カスタムイベントループ
/// SynchronizationContext に投稿された作業を順次処理する
/// </summary>
public class MiniEventLoop
{
    private readonly MiniSynchronizationContext _syncContext;
    private bool _isRunning;

    public MiniEventLoop()
    {
        _syncContext = new MiniSynchronizationContext();
    }

    public MiniSynchronizationContext SyncContext => _syncContext;

    /// <summary>
    /// イベントループを開始して、すべての作業が完了するまで実行
    /// </summary>
    public void Run(Action action)
    {
        // 現在のスレッドに SynchronizationContext を設定
        SynchronizationContext.SetSynchronizationContext(_syncContext);
        
        Console.WriteLine("=== イベントループ開始 ===");
        _isRunning = true;

        // 最初のアクションを実行
        action();

        // キューに作業がある限り実行を続ける
        while (_isRunning && _syncContext.HasWork())
        {
            _syncContext.ExecuteNext();
        }

        Console.WriteLine("=== イベントループ終了 ===");
        
        // SynchronizationContext をクリア
        SynchronizationContext.SetSynchronizationContext(null);
    }

    /// <summary>
    /// 非同期アクションでイベントループを開始
    /// </summary>
    public void Run(Func<Task> asyncAction)
    {
        // 現在のスレッドに SynchronizationContext を設定
        SynchronizationContext.SetSynchronizationContext(_syncContext);
        
        Console.WriteLine("=== イベントループ開始 ===");
        _isRunning = true;

        // 最初の非同期アクションを開始
        var task = asyncAction();

        // タスクが完了していない、またはキューに作業がある限り実行を続ける
        while (_isRunning && (!task.IsCompleted || _syncContext.HasWork()))
        {
            if (_syncContext.HasWork())
            {
                _syncContext.ExecuteNext();
            }
            else if (!task.IsCompleted)
            {
                // キューが空でタスクが未完了の場合は少し待つ
                Thread.Sleep(10);
            }
        }

        Console.WriteLine("=== イベントループ終了 ===");
        
        // SynchronizationContext をクリア
        SynchronizationContext.SetSynchronizationContext(null);
    }

    /// <summary>
    /// イベントループを停止
    /// </summary>
    public void Stop()
    {
        _isRunning = false;
        Console.WriteLine("[Stop] イベントループ停止要求");
    }
}
