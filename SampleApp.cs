internal static class SampleApp
{
    public static async Task Main()
    {
        var subThreadTask = RunSubThreadTask();
        Program.ThreadWriteLine("I'm main");
        Program.ThreadWriteLine($"Ctx={SynchronizationContext.Current?.GetType().Name}");
        Program.ThreadWriteLine("await 500ms");

        // 非同期処理の完了を待つ
        await Task.Delay(500);
        Program.ThreadWriteLine("end await. await sub thread...");

        //　スレッドを使った並列処理の完了を待つ
        await Task.WhenAll(subThreadTask);
        Program.ThreadWriteLine("bye!");
    }

    private static Task RunSubThreadTask()
    {
        // Task.Run はスレッドプールのスレッドで動く
        return Task.Run(() =>
        {
            Program.ThreadWriteLine("I'm sub");
            Thread.Sleep(1000);
            Program.ThreadWriteLine("bye!");
        });
    }
}