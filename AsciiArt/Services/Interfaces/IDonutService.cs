using AsciiArt.Entities;

namespace AsciiArt.Services;
public interface IDonutService
{
    void RenderFrame(ConsoleSize consoleSize, DonutParametersEntity parameters);
    void ResetConsoleBuffer(ConsoleSize currentConsoleSize);
    byte[] UpdateConsoleBuffer(ConsoleSize currentConsoleSize);

}