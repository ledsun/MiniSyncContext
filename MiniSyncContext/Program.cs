using MiniSyncContext;

Console.WriteLine("MiniSyncContext デモ - async/await の内部動作を観察");
Console.WriteLine("========================================\n");

// デモ1: 基本的な async/await の動作
Console.WriteLine("【デモ1】基本的な async/await");
Console.WriteLine("----------------------------------------");
var eventLoop1 = new MiniEventLoop();
eventLoop1.Run(async () =>
{
    Console.WriteLine("非同期処理を開始");
    await SimulateAsyncWork("タスク1");
    Console.WriteLine("非同期処理が完了");
});

Console.WriteLine("\n【デモ2】複数の async/await の連鎖");
Console.WriteLine("----------------------------------------");
var eventLoop2 = new MiniEventLoop();
eventLoop2.Run(async () =>
{
    Console.WriteLine("タスク連鎖: 開始");
    await SimulateAsyncWork("ステップ1");
    Console.WriteLine("タスク連鎖: ステップ1完了");
    
    await SimulateAsyncWork("ステップ2");
    Console.WriteLine("タスク連鎖: ステップ2完了");
    
    await SimulateAsyncWork("ステップ3");
    Console.WriteLine("タスク連鎖: すべて完了");
});

Console.WriteLine("\n【デモ3】複数の非同期タスクの順次実行");
Console.WriteLine("----------------------------------------");
var eventLoop3 = new MiniEventLoop();
eventLoop3.Run(async () =>
{
    Console.WriteLine("複数タスク実行開始");
    
    await SimulateAsyncWork("タスクA");
    Console.WriteLine("タスクA完了");
    
    await SimulateAsyncWork("タスクB");
    Console.WriteLine("タスクB完了");
    
    Console.WriteLine("すべてのタスクが完了");
});

Console.WriteLine("\n【デモ4】戻り値を持つ非同期処理");
Console.WriteLine("----------------------------------------");
var eventLoop4 = new MiniEventLoop();
eventLoop4.Run(async () =>
{
    Console.WriteLine("計算開始");
    
    var result1 = await CalculateAsync(10, 20);
    Console.WriteLine($"計算結果1: {result1}");
    
    var result2 = await CalculateAsync(result1, 30);
    Console.WriteLine($"計算結果2: {result2}");
    
    Console.WriteLine("すべての計算が完了");
});

Console.WriteLine("\n【デモ5】SynchronizationContext の動作確認");
Console.WriteLine("----------------------------------------");
var eventLoop5 = new MiniEventLoop();
eventLoop5.Run(async () =>
{
    Console.WriteLine($"現在のSyncContext: {SynchronizationContext.Current?.GetType().Name ?? "null"}");
    
    await SimulateAsyncWork("継続確認");
    
    Console.WriteLine($"await後のSyncContext: {SynchronizationContext.Current?.GetType().Name ?? "null"}");
});

Console.WriteLine("\n========================================");
Console.WriteLine("デモ終了");

// ヘルパーメソッド: 非同期作業をシミュレート
static Task SimulateAsyncWork(string name)
{
    var tcs = new TaskCompletionSource();
    Console.WriteLine($"[{name}] 非同期作業を開始");
    
    // ThreadPoolで非同期作業を実行
    ThreadPool.QueueUserWorkItem(_ =>
    {
        Thread.Sleep(100); // 作業をシミュレート
        Console.WriteLine($"[{name}] バックグラウンド作業完了");
        tcs.SetResult();
    });
    
    return tcs.Task;
}

// ヘルパーメソッド: 戻り値を持つ非同期計算
static Task<int> CalculateAsync(int a, int b)
{
    var tcs = new TaskCompletionSource<int>();
    Console.WriteLine($"[計算] {a} + {b} を実行");
    
    ThreadPool.QueueUserWorkItem(_ =>
    {
        Thread.Sleep(50);
        var result = a + b;
        Console.WriteLine($"[計算] バックグラウンド計算完了: {result}");
        tcs.SetResult(result);
    });
    
    return tcs.Task;
}
