internal static class Program
{
    static void Main()
    {
        var eventLoop = new EventLoop();

        // イベントループへ定期的にイベントを追加
        using var cts = PeriodicEvents.StartToPostEvents(eventLoop);

        // イベントループでメイン処理を実行
        eventLoop.Run(SampleApp.Main);

        Console.WriteLine("Done.");
    }

    public static void ThreadWriteLine(string name)
    {
        Console.WriteLine($"[tid:{Environment.CurrentManagedThreadId:00}] {name}");
    }
}
