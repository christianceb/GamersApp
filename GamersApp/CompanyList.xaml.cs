using System.Windows;

namespace GamersApp
{
  /// <summary>
  /// Interaction logic for CompanyList.xaml
  /// </summary>
  public partial class CompanyList : Window
  {
    public Dashboard Dashboard;

    public CompanyList()
    {
      InitializeComponent();
    }

    private void BtnExit_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void BtnNew_Click(object sender, RoutedEventArgs e)
    {
      CompanyForm CompanyForm = new CompanyForm("Add Company", "Add");
      CompanyForm.Owner = this;
      CompanyForm.DataContext = null;
      CompanyForm.ShowDialog();
    }


    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
      CompanyForm CompanyForm = new CompanyForm("Edit Company", "Edit");
      CompanyForm.Owner = this;
      CompanyForm.ShowDialog();
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
      CompanyForm CompanyForm = new CompanyForm("Delete Company - Are you sure?", "Delete");
      CompanyForm.Owner = this;
      CompanyForm.ShowDialog();
    }
  }
}