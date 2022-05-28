using AsciiArt.Entities;
using AsciiArt.Constants;

namespace AsciiArt.Services;

public class CoreService : ICoreService
{
    private readonly IDonutService _donutService;

    public CoreService(IDonutService donutService)
    {
        _donutService = donutService;
    }

    public ConsoleSize GetConsoleSize()
    {
        return new ConsoleSize{WindowWidth = Console.WindowWidth, WindowHeight = Console.WindowHeight};
    }

    public void ExecuteDonut()
    {
        var standardOutput = Console.OpenStandardOutput();
        var currentConsoleSize = new ConsoleSize();
        var parameters = new DonutParametersEntity();

        while (!Console.KeyAvailable)
        {

            if (currentConsoleSize != GetConsoleSize())
            {
                _donutService.ResetConsoleBuffer(currentConsoleSize);
            }

            parameters.AngleA += DonutConstants.AngleAIncrement;
            parameters.AngleB += DonutConstants.AngleBIncrement;

            _donutService.RenderFrame(currentConsoleSize, parameters);
            var consoleBuffer = _donutService.UpdateConsoleBuffer(currentConsoleSize);

            standardOutput.Write(consoleBuffer, 0, consoleBuffer.Length - 1);

            Thread.Sleep(20);
        }

        standardOutput.Close();
    }
    
}