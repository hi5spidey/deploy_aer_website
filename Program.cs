namespace deploy_aer_website;
using System.Runtime.InteropServices;
static class Program
{
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AllocConsole();
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
[STAThread]
    static void Main()
    {
        //AllocConsole();
        Deployer.DeployWorkflow();

        //ApplicationConfiguration.Initialize();

        //Application.Run(new Dashboard());
    }
}