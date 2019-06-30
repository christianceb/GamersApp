using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Data;

namespace GamersApp
{
  /// <summary>
  /// Interaction logic for CompanyList.xaml
  /// </summary>
  public partial class CompanyList : MetroWindow
  {
    public Dashboard Dashboard;

    public CompanyList()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Sets the search filter on how to match the input to the company list. Function used
    /// programmatically in the search filter and not used anywhere in the class manually.
    /// </summary>
    /// <param name="item">The company in a generic object</param>
    /// <returns>A positive integer if it found a match</returns>
    private bool CompanyFilter(object item)
    {
      if (String.IsNullOrEmpty(txtSearch.Text))
      {
        return true;
      }
      else
      {
        return ((item as Company).Name.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
      }
    }

    /// <summary>
    /// Refreshes the list view of the company list. Useful in the event of making changes in
    /// the companies in the list, or even the list.
    /// </summary>
    public void RefreshCompaniesList()
    {
      lvwCompanies.ItemsSource = null;
      lvwCompanies.ItemsSource = Dashboard.Companies.List;
    }

    /// <summary>
    /// Attempts to open the the URI provided.
    /// </summary>
    /// <param name="URI">The URI to open. Anticipated URIs are URLs, Phone numbers and Email Addresses</param>
    private void OpenResource(string URI)
    {
      try
      {
        System.Diagnostics.Process.Start(URI);
      }
      catch (System.ComponentModel.Win32Exception noBrowser)
      {
        if (noBrowser.ErrorCode == -2147467259)
        {
          MessageBox.Show(noBrowser.Message);
        }
      }
      catch (System.Exception other)
      {
        MessageBox.Show(other.Message);
      }
    }

    private void BtnExit_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void BtnNew_Click(object sender, RoutedEventArgs e)
    {
      // Invokes a new company form
      CompanyForm CompanyForm = new CompanyForm("Add Company", "Add");
      CompanyForm.Owner = this;
      CompanyForm.DataContext = null;
      CompanyForm.ShowDialog();
    }


    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
      // Invokes an editable company form with the selected company in the list view as context.
      if (lvwCompanies.SelectedItem != null)
      {
        CompanyForm CompanyForm = new CompanyForm("Edit Company", "Edit");
        CompanyForm.DataContext = (Company)lvwCompanies.SelectedItem;
        CompanyForm.Owner = this;
        CompanyForm.ShowDialog();
      }
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
      // Invokes a company form with the selected company in the list view as context, that is to be deleted if confirmed.
      if (lvwCompanies.SelectedItem != null)
      {
        CompanyForm CompanyForm = new CompanyForm("Delete Company - Are you sure?", "Delete");
        CompanyForm.DataContext = (Company)lvwCompanies.SelectedItem;
        CompanyForm.Owner = this;
        CompanyForm.ShowDialog();
      }
    }

    private void BtnView_Click(object sender, RoutedEventArgs e)
    {
      // Invokes a company form with the fields disabled for viewing purposes only.
      if (lvwCompanies.SelectedItem != null)
      {
        Company Company = (Company)lvwCompanies.SelectedItem;

        CompanyForm CompanyForm = new CompanyForm(Company.Name, "View");
        CompanyForm.DataContext = Company;
        CompanyForm.Owner = this;
        CompanyForm.ShowDialog();
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      // Set ItemsSource of the listview on this window.
      Dashboard = (Dashboard)this.Owner;
      lvwCompanies.ItemsSource = Dashboard.Companies.List;

      // Instantiate search filter
      CollectionView CollectionViewX = (CollectionView)CollectionViewSource.GetDefaultView(lvwCompanies.ItemsSource);
      CollectionViewX.Filter = CompanyFilter;
    }

    private void BtnWebsite_Click(object sender, RoutedEventArgs e)
    {
      // Opens the company website on a browser if it's set.
      System.Windows.Controls.Button Button = sender as System.Windows.Controls.Button;
      Company Company = Button.DataContext as Company;

      if ( Company.Website != "" )
      {
        OpenResource(Company.Website);
      }
    }

    private void BtnPhone_Click(object sender, RoutedEventArgs e)
    {
      // Opens the company phone number in an application (presumably a dialer) if it's set
      System.Windows.Controls.Button Button = sender as System.Windows.Controls.Button;
      Company Company = Button.DataContext as Company;

      OpenResource("tel:" + Company.Phone);
    }

    private void BtnEmail_Click(object sender, RoutedEventArgs e)
    {
      // Opens an email client with the To: set to the company email if its set
      System.Windows.Controls.Button Button = sender as System.Windows.Controls.Button;
      Company Company = Button.DataContext as Company;

      OpenResource("mailto:" + Company.Email);
    }

    private void TxtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
      // Refreshes the list view company list in the event of changes in the company search field
      CollectionViewSource.GetDefaultView( lvwCompanies.ItemsSource ).Refresh();
    }

    private void BtnReset_Click(object sender, RoutedEventArgs e)
    {
      txtSearch.Text = "";
    }
  }
}