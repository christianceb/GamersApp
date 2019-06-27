using System.Windows;

namespace GamersApp
{
  /// <summary>
  /// Interaction logic for Dashboard.xaml
  /// </summary>
  public partial class Dashboard : Window
  {
    public Dashboard()
    {
      InitializeComponent();
    }

    private void MitCompanies_Click(object sender, RoutedEventArgs e)
    {
      // Invokes the window to show the companies list.
      CompanyList companyList = new CompanyList();
      companyList.Owner = this;
      companyList.ShowDialog();
    }

    private void MitQuit_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}