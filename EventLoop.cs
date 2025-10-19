using System.Collections.Concurrent;

/// <summary>
/// await 可能なメイン処理を実行するためのイベントループ
/// </summary>
public sealed class EventLoop
{
    /// <summary>
    /// 別スレッドからのイベントを受け取るキュー
    /// </summary>
    private readonly BlockingCollection<Action> _queue = [];

    /// <summary>
    /// イベントを受け取る
    /// </summary>
    /// <param name="action"></param>
    public void Post(Action action) => _queue.Add(action);

    /// <summary>
    /// awaitを伴うメイン関数を実行する
    /// </summary>
    /// <param name="MainFunction"></param>
    public void Run(Func<Task> MainFunction)
    {
        // イベントループを開始
        var thread = StartThread();

        // メイン関数の完了を待つための TaskCompletionSource
        var tcs = new TaskCompletionSource();

        // メイン関数をラップした Action
        Action wrappedMain = async () =>
        {
            try
            {
                // メイン関数を実行
                await MainFunction();

                // 完了を通知
                tcs.SetResult();
            }
            catch (Exception ex)
            {
                // 例外を通知
                tcs.SetException(ex);
            }
        };

        // メイン関数をキューに追加
        _queue.Add(wrappedMain);

        // メイン関数の完了を待つ
        tcs.Task.GetAwaiter().GetResult();

        // キューを閉じる
        _queue.CompleteAdding();

        // スレッドを待つ
        thread.Join();
    }

    /// <summary>
    /// イベントループを開始する
    /// </summary>
    private Thread StartThread()
    {
        var thread = new Thread(Loop);
        thread.Start();
        return thread;

        // イベントループの本体
        void Loop()
        {
            // 自分に継続イベントを投げさせるための SynchronizationContext を設定
            SynchronizationContext.SetSynchronizationContext(new MySyncContext(_queue));

            // イベントループ：キューから取り出して実行
            foreach (var work in _queue.GetConsumingEnumerable())
            {
                work();
            }
        }
    }
}
