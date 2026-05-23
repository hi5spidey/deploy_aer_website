namespace deploy_aer_website;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
[STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Run the deploy workflow; replace RunFunction implementation in Deployer.cs first.
        Deployer.DeployWorkflow();

        Application.Run(new Dashboard());
    }
}