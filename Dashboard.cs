namespace deploy_aer_website;

public partial class Dashboard : Form
{
    public Dashboard()
    {
        //Program.AllocConsole();
                // Run the deploy workflow; replace RunFunction implementation in Deployer.cs first.
        Deployer.DeployWorkflow();
        InitializeComponent();
    }
}

