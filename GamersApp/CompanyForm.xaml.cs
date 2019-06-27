using ISO3166;
using System;
using System.Windows;

namespace GamersApp
{
  /// <summary>
  /// Interaction logic for CompanyForm.xaml
  /// </summary>
  public partial class CompanyForm : Window
  {
    public string Verb;

    public CompanyForm(String title, String verb)
    {
      InitializeComponent();

      Verb = verb;

      // Set Window Labels
      this.Title = title;
      lblOperationTitle.Content = title;
      btnOperation.Content = verb;

      // Set Countries Combo Box Values
      Country[] countries = ISO3166.Country.List;
      cboCountry.ItemsSource = countries;
      cboCountry.DisplayMemberPath = "Name";
    }


    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void BtnOperation_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}