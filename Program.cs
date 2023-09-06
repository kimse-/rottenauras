using RtspClientSharp;
using RtspClientSharp.Rtsp;
using static DirectXDemoConsoleApp.Window;
GameOverlay.TimerService.EnableHighPrecisionTimers();


var serverUri = new Uri("rtmp://localhost/mystream");

var connectionParameters = new ConnectionParameters(serverUri);
var cancellationTokenSource = new CancellationTokenSource();

Task connectTask = ConnectAsync(connectionParameters, cancellationTokenSource.Token);


using (var example = new Example())
{
    example.Run();
}

    static async Task ConnectAsync(ConnectionParameters connectionParameters, CancellationToken token)
    {
        try
        {
            TimeSpan delay = TimeSpan.FromSeconds(5);

            using (var rtspClient = new RtspClient(connectionParameters))
            {
                rtspClient.FrameReceived +=
                    (sender, frame) => Console.WriteLine($"New frame {frame.Timestamp}: {frame.GetType().Name}");

                while (true)
                {
                    Console.WriteLine("Connecting...");

                    try
                    {
                        await rtspClient.ConnectAsync(token);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    catch (RtspClientException e)
                    {
                        Console.WriteLine(e.ToString());
                        await Task.Delay(delay, token);
                        continue;
                    }

                    Console.WriteLine("Connected.");

                    try
                    {
                        await rtspClient.ReceiveAsync(token);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    catch (RtspClientException e)
                    {
                        Console.WriteLine(e.ToString());
                        await Task.Delay(delay, token);
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
    }