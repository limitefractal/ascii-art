using AsciiArt.Entities;
using AsciiArt.Constants;

namespace AsciiArt.Services;

public class DonutService : IDonutService
{
    private char[,] Output { get; set; } = default!;
    private byte[] ConsoleBuffer { get; set; } = default!;
    private double[,] ZBuffer { get; set; } = default!;
    private double K1 { get; set; } = default!;

    public void RenderFrame(ConsoleSize consoleSize, DonutParametersEntity parameters){

        var cosA = Math.Cos(parameters.AngleA);
        var sinA = Math.Sin(parameters.AngleA);
        var cosB = Math.Cos(parameters.AngleB);
        var sinB = Math.Sin(parameters.AngleB);

        for(var theta = 0.0; theta < 2 * Math.PI; theta += DonutConstants.ThetaSpacing) 
        {
            var cosTheta = Math.Cos(theta);
            var sinTheta = Math.Sin(theta);

            for(var phi = 0.0; phi < 2 * Math.PI; phi += DonutConstants.PhiSpacing) 
            {
                var cosPhi = Math.Cos(phi);
                var sinPhi = Math.Sin(phi);

                var circleX = DonutConstants.R2 + DonutConstants.R1 * cosTheta;
                var circleY = DonutConstants.R1 * sinTheta;

                var x = circleX * (cosB * cosPhi + sinA * sinB * sinPhi) - circleY * cosA * sinB;
                var y = circleX * (sinB * cosPhi - sinA * cosB * sinPhi) + circleY * cosA * cosB;
                var ooz = 1.0 / (DonutConstants.K2 + cosA * circleX * sinPhi + circleY * sinA);  

                var L = cosPhi * cosTheta * sinB - cosA * cosTheta * sinPhi - sinA * sinTheta + cosB * (cosA * sinTheta - cosTheta * sinA * sinPhi);

                var xp = (int)(consoleSize.WindowWidth / 2.0 + K1 * ooz * x);
                var yp = (int)(consoleSize.WindowHeight / 2.0 - K1 * ooz * y);

                if(xp >= 0 && xp < consoleSize.WindowWidth && yp >= 0 && yp < consoleSize.WindowHeight && ooz > ZBuffer[xp, yp]) 
                {

                    ZBuffer[xp, yp] = ooz;
                    var luminanceIndex = (int)Math.Max(0, L * 8);
                    Output[xp, yp] = ".,-~:;=!*#$@"[luminanceIndex];
                }
            }
        }
    }
    
    public void ResetConsoleBuffer(ConsoleSize currentConsoleSize)
    {
        currentConsoleSize.WindowWidth = Console.WindowWidth;
        currentConsoleSize.WindowHeight = Console.WindowHeight;

        Output = new char[currentConsoleSize.WindowWidth, currentConsoleSize.WindowHeight]; 
        ZBuffer = new double[currentConsoleSize.WindowWidth, currentConsoleSize.WindowHeight]; 
        ConsoleBuffer = new byte[currentConsoleSize.WindowWidth * currentConsoleSize.WindowHeight];

        K1 = Math.Min(currentConsoleSize.WindowWidth, currentConsoleSize.WindowHeight) * DonutConstants.K2 * 3.0 / (8.0 * (DonutConstants.R1 + DonutConstants.R2));

        for(int y = 0; y < currentConsoleSize.WindowHeight; y++)
            for(int x = 0; x < currentConsoleSize.WindowWidth; x++)
                Output[x, y] = ' ';

    }
    
    public byte[] UpdateConsoleBuffer(ConsoleSize currentConsoleSize)
    {
        Console.SetCursorPosition(0, 0);

        for(var y = 0; y < currentConsoleSize.WindowHeight; y++) 
        {
            for(var x = 0; x < currentConsoleSize.WindowWidth; x++) 
            {
                ConsoleBuffer[x + y * currentConsoleSize.WindowWidth] = (byte)Output[x, y];
                Output[x, y] = ' ';
                ZBuffer[x, y] = 0;
            }
        }

        return ConsoleBuffer;
    }
}