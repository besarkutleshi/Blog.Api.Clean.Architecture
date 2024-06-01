using System.Diagnostics;

namespace Blog.IntegrationTests.Infrastructure.Helpers;

public static class DockerComposeService
{
    public static void RunDockerComposeUp()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "docker-compose",
            Arguments = "up --build",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = "C:\\Users\\BesarKutleshi\\Desktop\\Repos\\Blog.Api\\Blog.IntegrationTests"
        };

        using (var process = new Process { StartInfo = processStartInfo })
        {
            process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            //process.WaitForExit();

            //if (process.ExitCode != 0)
            //{
            //    throw new Exception("Docker Compose process failed.");
            //}
        }
    }
    public static void RunDockerComposeDown()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "docker-compose",
            Arguments = "down",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = "C:\\Users\\BesarKutleshi\\Desktop\\Repos\\Blog.Api\\Blog.IntegrationTests"
        };

        using (var process = new Process { StartInfo = processStartInfo })
        {
            process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            //process.WaitForExit();

            //if (process.ExitCode != 0)
            //{
            //    throw new Exception("Docker Compose process failed.");
            //}
        }
    }
}
