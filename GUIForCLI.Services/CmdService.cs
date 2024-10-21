
using GUIForCLI.Api.Entities;
using System.Diagnostics;
using System.Text;

namespace GUIForCLI.Services
{
    public class CmdService : ICmdService
    {
        public async Task<(bool,string)> CustomTask(CLIRequest request)
        {
            string command = "cmd.exe";
            string arguments = "/c " + request.Command;

            Process process = new();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            StringBuilder outputBuilder = new();
            StringBuilder errorBuilder = new();

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    outputBuilder.AppendLine(e.Data);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    errorBuilder.AppendLine(e.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            int exitCode = process.ExitCode;
            string output = outputBuilder.ToString();
            string error = errorBuilder.ToString();

            if (exitCode != 0)
            {
                return (false,"Error: " + error);
            }

            return (true, output);
        }
    }
}
