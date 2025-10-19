namespace MiniSyncContext;

/// <summary>
/// カスタム SynchronizationContext の実装
/// async/await の継続処理をキューイングして、カスタムイベントループで実行する
/// </summary>
public class MiniSynchronizationContext : SynchronizationContext
{
    private readonly Queue<(SendOrPostCallback callback, object? state)> _workItems = new();
    private readonly object _lock = new();

    /// <summary>
    /// 非同期操作をキューに追加する
    /// </summary>
    public override void Post(SendOrPostCallback callback, object? state)
    {
        lock (_lock)
        {
            _workItems.Enqueue((callback, state));
            Console.WriteLine($"[Post] 作業をキューに追加しました。キューサイズ: {_workItems.Count}");
        }
    }

    /// <summary>
    /// 同期的に実行する（このデモでは非同期と同じ動作）
    /// </summary>
    public override void Send(SendOrPostCallback callback, object? state)
    {
        Console.WriteLine("[Send] 同期実行が要求されました");
        callback(state);
    }

    /// <summary>
    /// キューに作業があるかチェック
    /// </summary>
    public bool HasWork()
    {
        lock (_lock)
        {
            return _workItems.Count > 0;
        }
    }

    /// <summary>
    /// キューから1つの作業を取得して実行
    /// </summary>
    public void ExecuteNext()
    {
        (SendOrPostCallback callback, object? state) workItem;
        
        lock (_lock)
        {
            if (_workItems.Count == 0)
            {
                return;
            }
            workItem = _workItems.Dequeue();
        }

        Console.WriteLine($"[ExecuteNext] 作業を実行します。残りキューサイズ: {_workItems.Count}");
        workItem.callback(workItem.state);
    }

    /// <summary>
    /// すべてのキューされた作業を実行
    /// </summary>
    public void RunAll()
    {
        Console.WriteLine("[RunAll] すべての作業を実行開始");
        while (HasWork())
        {
            ExecuteNext();
        }
        Console.WriteLine("[RunAll] すべての作業を実行完了");
    }
}
