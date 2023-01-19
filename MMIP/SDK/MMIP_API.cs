using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace MMIP.SDK
{
    public class MMIP_API
    {
        static void ExecuteCommand(string command)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine((String.IsNullOrEmpty(output) ? "" : output));
            process.Close();
        }

        public static void TakeOwnership(string path)
        {
            ExecuteCommand($"takeown /f \"{path}\"");
        }

        public static void SystemCopy(string srcDir, string targetDir)
        {
            ExecuteCommand($"xcopy \"{srcDir}\" \"{targetDir}\\\" /o /x /e");
        }

        public static void GrantFullControl(string path)
        {
            try
            {
                // Get the current user's identity
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);

                // Grant full permissions to the file
                var fs = new FileSecurity(path, AccessControlSections.All);
                var accessRule = new FileSystemAccessRule(principal.Identity.Name, FileSystemRights.FullControl, AccessControlType.Allow);
                fs.AddAccessRule(accessRule);
                File.SetAccessControl(path, fs);
                Console.WriteLine("Successfully granted full control to the file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error granting full control to the file: " + ex.Message);
            }
        }
    }
}
