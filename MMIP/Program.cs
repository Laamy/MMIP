using MMIP.SDK;
using System;
using System.IO;

namespace MMIP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string mcManifest =  @"C:\Program Files\WindowsApps\Microsoft.MinecraftUWP_1.19.5101.0_x64__8wekyb3d8bbwe\AppxManifest.xml";

            MMIP_API.TakeOwnership(mcManifest);
            MMIP_API.GrantFullControl(mcManifest);

            MMIP_API.SystemCopy(mcManifest, new FileInfo("AppxManifest (1).xml").FullName);

            File.Delete(mcManifest); // delete original

            MMIP_API.SystemCopy(new FileInfo("AppxManifest (1).xml").FullName, mcManifest);
        }
    }
}
