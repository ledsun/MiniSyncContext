internal static class PeriodicEvents
{
    public static CancellationTokenSource StartToPostEvents(EventLoop ui)
    {
        // 「擬似イベント」をUIキューへ投げる
        // UIスレッドがawaitしてる間に仕事ができることを確かめます。
        var cts = new CancellationTokenSource();
        _ = Task.Run(async () =>
        {
            Program.ThreadWriteLine("PeriodicEvents started.");

            var val1 = 1;
            var val2 = 1;
            while (!cts.IsCancellationRequested)
            {
                ui.Post(() =>
                {
                    var tmp = val1 + val2;
                    Program.ThreadWriteLine($"fibonacci {tmp}");

                    val1 = val2;
                    val2 = tmp;
                });
                await Task.Delay(200);
            }
        });
        return cts;
    }
}