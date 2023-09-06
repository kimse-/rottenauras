using static DirectXDemoConsoleApp.Window;
GameOverlay.TimerService.EnableHighPrecisionTimers();

using (var example = new Example())
{
    example.Run();
}