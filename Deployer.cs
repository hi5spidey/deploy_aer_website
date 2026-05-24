using System;
using System.Diagnostics;
using System.IO;

namespace deploy_aer_website
{
    public static class Deployer
    {
        // Replace RunFunction() with your actual function that returns the folder path containing generated HTML files on success.
        private static string RunFunction()
        {
            //var candidate = Path.Combine(AppContext.BaseDirectory, "output");
            var candidate = AppContext.BaseDirectory;
            return Directory.Exists(candidate) ? candidate : null;
        }

        private static int RunBatch(string batchPath)
        {
            if (!File.Exists(batchPath)) return -1;
            var psi = new ProcessStartInfo("cmd.exe", $"/c \"{batchPath}\"")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            using var p = Process.Start(psi);
            p.WaitForExit();
            return p.ExitCode;
        }

        public static bool DeployWorkflow()
        {
            var outputDir = RunFunction();
            if (string.IsNullOrEmpty(outputDir) || !Directory.Exists(outputDir))
            {
                return false;
            }

            var baseDir = AppContext.BaseDirectory;
            var unitTestDir = Path.Combine(baseDir, "UNIT_TEST");
            Directory.CreateDirectory(unitTestDir);

            foreach (var file in Directory.EnumerateFiles(outputDir, "*.html", SearchOption.TopDirectoryOnly))
            {
                var dest = Path.Combine(unitTestDir, Path.GetFileName(file));
                File.Copy(file, dest, overwrite: true);
            }

            var countResult = RunBatch(Path.Combine(baseDir, "count_files.bat"));
            if (countResult != 0) return false;

            var diffResult = RunBatch(Path.Combine(baseDir, "difference.bat"));
            if (diffResult != 0) return false;

            var deployDir = Path.Combine(baseDir, "DEPLOY");
            Directory.CreateDirectory(deployDir);
            foreach (var file in Directory.EnumerateFiles(unitTestDir, "*.html", SearchOption.TopDirectoryOnly))
            {
                var dest = Path.Combine(deployDir, Path.GetFileName(file));
                File.Copy(file, dest, overwrite: true);
            }
            return true;
        }
    }
}