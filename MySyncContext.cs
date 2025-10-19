
using System.Collections.Concurrent;

/// <summary>
/// Postだけ持つ簡易 SynchronizationContext
/// </summary>
public sealed class MySyncContext : SynchronizationContext
{
    /// <summary>
    /// イベントループにActionを送るためのキュー
    /// </summary>
    private readonly BlockingCollection<Action> _queue;

    public MySyncContext(BlockingCollection<Action> queue) => _queue = queue;

    public override void Post(SendOrPostCallback d, object? state)
    {
        // メイン処理内で await された後の継続処理がここに来る
        // イベントループのキューに追加して、イベントループで実行させる
        _queue.Add(() => d(state));
    }
}
