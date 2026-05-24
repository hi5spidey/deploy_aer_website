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
            Console.WriteLine("[RunFunction] Starting function execution...");
            //var candidate = Path.Combine(AppContext.BaseDirectory, "output"); 
            var candidate = AppContext.BaseDirectory; 
            Console.WriteLine($"[RunFunction] Checking if candidate directory exists: {candidate}");

            bool exists = Directory.Exists(candidate);
            Console.WriteLine($"[RunFunction] Directory existence check result: {exists}");

            string result = exists ? candidate : null; 
            Console.WriteLine($"[RunFunction] Returning path: {(result ?? "null")}");
            return result; 
        } 

        private static int RunBatch(string batchPath) 
        { 
            Console.WriteLine($"[RunBatch] Attempting to run batch file: {batchPath}");

            if (!File.Exists(batchPath)) 
            {
                Console.WriteLine($"[RunBatch] ERROR: Batch file does not exist at path: {batchPath}");
                return -1; 
            }

            Console.WriteLine("[RunBatch] Batch file found. Initializing process...");
            var psi = new ProcessStartInfo("cmd.exe", $"/c \"{batchPath}\"") 
            { 
                CreateNoWindow = true, 
                UseShellExecute = false 
            }; 

            using var p = Process.Start(psi); 
            Console.WriteLine("[RunBatch] Process started. Waiting for execution to complete...");
            p.WaitForExit(); 

            Console.WriteLine($"[RunBatch] Process finished. Exit Code: {p.ExitCode}");
            return p.ExitCode; 
        } 

        public static bool DeployWorkflow() 
        { 
            Console.WriteLine("[DeployWorkflow] Starting workflow deployment process...");

            var outputDir = RunFunction(); 
            if (string.IsNullOrEmpty(outputDir) || !Directory.Exists(outputDir)) 
            { 
                Console.WriteLine($"[DeployWorkflow] FAILURE: Output directory invalid or missing. Value: {(outputDir ?? "null")}");
                return false; 
            } 

            var baseDir = AppContext.BaseDirectory; 
            var unitTestDir = Path.Combine(baseDir, "UNIT_TEST"); 
            Console.WriteLine($"[DeployWorkflow] Target unit test directory: {unitTestDir}");

            Directory.CreateDirectory(unitTestDir); 
            Console.WriteLine("[DeployWorkflow] Unit test directory verified/created.");

            Console.WriteLine($"[DeployWorkflow] Copying HTML files from {outputDir} to {unitTestDir}...");
            int unitTestCopyCount = 0;
            foreach (var file in Directory.EnumerateFiles(outputDir, "*.html", SearchOption.TopDirectoryOnly)) 
            { 
                var dest = Path.Combine(unitTestDir, Path.GetFileName(file)); 
                File.Copy(file, dest, overwrite: true); 
                unitTestCopyCount++;
            } 
            Console.WriteLine($"[DeployWorkflow] Successfully copied {unitTestCopyCount} HTML file(s) to UNIT_TEST folder.");

            string countBatchPath = Path.Combine(baseDir, "count_files.bat");
            var countResult = RunBatch(countBatchPath); 
            if (countResult != 0) 
            {
                Console.WriteLine($"[DeployWorkflow] FAILURE: count_files.bat failed with exit code: {countResult}");
                return false; 
            }

            string diffBatchPath = Path.Combine(baseDir, "difference.bat");
            var diffResult = RunBatch(diffBatchPath); 
            if (diffResult != 0) 
            {
                Console.WriteLine($"[DeployWorkflow] FAILURE: difference.bat failed with exit code: {diffResult}");
                return false; 
            }

            var deployDir = Path.Combine(baseDir, "DEPLOY"); 
            Console.WriteLine($"[DeployWorkflow] Target deployment directory: {deployDir}");

            Directory.CreateDirectory(deployDir); 
            Console.WriteLine("[DeployWorkflow] Deployment directory verified/created.");

            Console.WriteLine($"[DeployWorkflow] Copying HTML files from {unitTestDir} to {deployDir}...");
            int deployCopyCount = 0;
            foreach (var file in Directory.EnumerateFiles(unitTestDir, "*.html", SearchOption.TopDirectoryOnly)) 
            { 
                var dest = Path.Combine(deployDir, Path.GetFileName(file)); 
                File.Copy(file, dest, overwrite: true); 
                deployCopyCount++;
            } 
            Console.WriteLine($"[DeployWorkflow] Successfully copied {deployCopyCount} HTML file(s) to DEPLOY folder.");

            Console.WriteLine("[DeployWorkflow] Workflow completed successfully.");
            return true; 
        } 
    } 
}
