using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Il2CppDump解析器
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(@" 3个参数 [arm-linux-androideabi-addr2line.exe] [符号文件] [dumpText文件]");
                String exeName = args[0];
                String symbPath = args[1];
                String dumpTextPath = args[2];
                String dumpText = File.ReadAllText(dumpTextPath);
                StringBuilder sb = new StringBuilder();
                //.*? pc (.*?) (/data.*)
                foreach (Match match in Regex.Matches(dumpText, @"(.*?) pc (.*?) (/data.*)"))
                {
                    String bugId = match.Groups[2].Value;
                    Process p = new Process();
                    // Redirect the output stream of the child process.
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = exeName;
                    p.StartInfo.Arguments = "   -f -C -e  " + symbPath + "  " + bugId;
                    p.Start();
                    // Do not wait for the child process to exit before
                    // reading to the end of its redirected stream.
                    // p.WaitForExit();
                    // Read the output stream first and then wait.
                    string output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    Console.WriteLine(output);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("解析完毕");
            Console.ReadKey();
        }
    }
}
