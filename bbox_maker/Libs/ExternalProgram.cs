using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bbox_maker.Lib
{
    public static class ExternalProgram
    {
        /// <summary>
        /// Executes an external program. 
        /// No console window is shown. It's up to the caller to provide a callback to invoke when a line of console output is read and display it in the app.        
        /// </summary>
        /// <param name="program">Program to execute.</param>
        /// <param name="args">Arguments to provide to the program being executed.</param>
        /// <param name="workingDir">Working directory of the process.</param>
        /// <param name="inputLines">These lines are written to the console input with a delay of 1 sec between each write.</param>
        /// <param name="outputLineCallback">Callback when a line of console output is read.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string Run(
            string program,
            string args,
            string workingDir,
            List<string> inputLines,
            Action<string> outputLineCallback)
        {
            var startInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = args,
                FileName = program,
                WorkingDirectory = workingDir,
            };

            var process = Process.Start(startInfo);

            try
            {
                if (process == null)
                    throw new Exception("Could not start process");

                if(inputLines != null)
                {
                    using (var sw = process.StandardInput)
                    {
                        if (sw.BaseStream.CanWrite)
                        {
                            foreach (var command in inputLines)
                            {
                                Thread.Sleep(1000);
                                sw.WriteLine(command);
                                sw.Flush();
                            }
                        }
                        sw.Close();
                    }
                }

                var sb = new StringBuilder();
                while (!process.HasExited)
                {
                    var oline = process.StandardOutput.ReadLine();
                    if (oline != null)
                    {
                        sb.Append(oline);
                        outputLineCallback?.Invoke(oline);
                    }
                }

                return sb.ToString();
            }
            finally
            {
                process?.Dispose();
            }
        }

        public static void RunInWindow(
            string program,
            string args,
            string workingDir)
        {
            var startInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = false,
                RedirectStandardInput = false,
                RedirectStandardError = false,
                UseShellExecute = true,
                CreateNoWindow = false,
                Arguments = args,
                FileName = program,
                WorkingDirectory = workingDir,
            };

            var process = Process.Start(startInfo);

            try
            {
                if (process == null)
                    throw new Exception("Could not start process");

                process.WaitForExit();
            }
            finally
            {
                process?.Dispose();
            }
        }

    }

}
