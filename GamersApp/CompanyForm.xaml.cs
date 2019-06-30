using ISO3166;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GamersApp
{
  /// <summary>
  /// Interaction logic for CompanyForm.xaml
  /// </summary>
  public partial class CompanyForm : MetroWindow
  {
    private CompanyList CompanyList;
    public string Verb;
    List<Control> Fields;
    bool Valid = true;
    static List<Country> CountryList;

    public CompanyForm( String title, String verb )
    {
      InitializeComponent();

      Verb = verb;

      // Collate fields for possible iteration later.
      Fields = new List<Control>( new Control[] {
        txtCompanyName,
        txtContactPerson,
        txtPhone,
        txtEmail,
        txtWebsite,
        txtAddress,
        txtCity,
        txtState,
        txtPostCode,
        cboCountry
      });

      // Set Window Labels
      this.Title = title;
      lblOperationTitle.Content = title;
      btnOperation.Content = verb;

      // Set Countries Combo Box Values
      Country[] countries = ISO3166.Country.List;
      cboCountry.ItemsSource = countries;
      cboCountry.DisplayMemberPath = "Name";

      // Form is editable by default. However, we don't want it to be editable when you're deleting or viewing a company.
      if ( verb == "Delete" || verb == "View")
      {

        foreach (Control Field in Fields)
        {
          Field.IsEnabled = false;
        }

        // Hide "required fields" helper message;
        lblRequired.Visibility = Visibility.Hidden;

        // More subroutines to do when the verb is just viewing a company.
        if ( verb == "View" )
        {
          this.Title = "Company Profile - " + title;
          btnOperation.Visibility = Visibility.Hidden;
          btnCancel.Content = "Close";
          btnCancel.Margin = new Thickness(0, 0, 0, 0);
        }
      }
    }

    /// <summary>
    /// Finds the most recent ID used in the company list.
    /// </summary>
    /// <returns>The ID of the most recent company created. If the list is empty, returns 0</returns>
    private int FindId()
    {
      int tail = CompanyList.Dashboard.Companies.Count;
      int id = 0;

      if (tail == 0)
      {
        id = 1;
      }
      else
      {
        tail--;
        id = CompanyList.Dashboard.Companies.List[tail].Id + 1;
      }

      return id;
    }

    /// <summary>
    /// Iterative way to validate fields especially on required fields.
    /// </summary>
    /// <returns>True if the form is valid. Otherwise false.</returns>
    private bool ValidateFields()
    {
      Valid = true;
      List<string> RequiredFields = new List<string>(new string[] {
        "txtCompanyName",
        "txtEmail",
        "txtAddress",
        "txtCity",
        "txtState",
        "txtPostCode",
        "cboCountry"
      });

      foreach (Control Field in Fields)
      {
        // Matches the current field name if it exists in the RequiredFields list.
        if (RequiredFields.Contains(Field.Name))
        {
          if ( Field is TextBox )
          {
            // Reset previous modification to style to field if it failed validation.
            Field.ClearValue(TextBox.BorderBrushProperty);

            TextBox CastedField = ((TextBox)Field);
            if (CastedField.Text == "")
            {
              // Sets border of field to red to indicate input is not appropriate.
              Field.BorderBrush = Brushes.Red;
              Valid = false;
            }
          } else if (Field is ComboBox)
          {
            // Edge case on handling combobox validation.
            ComboBox CastedField = ((ComboBox)Field);
            if (CastedField.Name == "cboCountry")
            {
              /**
               * Reset previous modification to style to field if it failed validation. Note
               * that we are modifying the border element on top of the combobox and not the
               * actual combobox due to differences in operating systems where this application
               * will be run.
               */
              bdrCountry.BorderBrush = Brushes.Transparent;

              if (CastedField.Text == "")
              {
                Valid = false;
              }
              bdrCountry.BorderBrush = Brushes.Red;
            }
          }
        }
      }

      return Valid;
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void BtnOperation_Click(object sender, RoutedEventArgs e)
    {
      CompanyList = (CompanyList) Owner;

      if ( Verb == "Add" )
      {
        if ( ValidateFields() )
        {
          // Form validated as correct and will be saved as a new company.
          CompanyList.Dashboard.Companies.List.Add(new Company()
          {
            Id = FindId(),
            Name = txtCompanyName.Text,
            ContactPerson = txtContactPerson.Text,
            Phone = txtPhone.Text,
            Email = txtEmail.Text,
            Website = txtWebsite.Text,
            Address = txtAddress.Text,
            City = txtCity.Text,
            State = txtState.Text,
            PostCode = txtPostCode.Text,
            Country = cboCountry.Text
          });
          CompanyList.Dashboard.Modified = true;
        }
      }
      else if ( Verb == "Edit"  )
      {
        if ( ValidateFields() )
        {
          // Form validated as correct and will save changes made to the company
          Company Company = (Company)this.DataContext;
          Company.Name = txtCompanyName.Text;
          Company.ContactPerson = txtContactPerson.Text;
          Company.Phone = txtPhone.Text;
          Company.Email = txtEmail.Text;
          Company.Website = txtWebsite.Text;
          Company.Address = txtAddress.Text;
          Company.City = txtCity.Text;
          Company.State = txtState.Text;
          Company.PostCode = txtPostCode.Text;
          Company.Country = cboCountry.Text;

          CompanyList.Dashboard.Modified = true;
        }
      }
      else if ( Verb == "Delete" )
      {
        // Ensure that user is serious about deleting the company.
        MessageBoxResult MessageBoxResult = MessageBox.Show(
          "Are you sure you want to delete this Company?",
          "Confirm",
          MessageBoxButton.YesNo,
          MessageBoxImage.Warning
        );

        // User is serious.
        if ( MessageBoxResult == MessageBoxResult.Yes )
        {
          Company Company = (Company)this.DataContext;
          CompanyList.Dashboard.Companies.List.Remove(Company);

          CompanyList.Dashboard.Modified = true;
        }
      }

      // Refresh companies list as the company list window itemsource for the listview is not bound.
      if ( Valid )
      {
        CompanyList.RefreshCompaniesList();
        this.Close();
      }
    }

    private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
    {
      // Manually set country of this company because binding is a mess with this.
      Company Company = (Company)DataContext;

      if (DataContext != null)
      {
        if (CountryList == null)
        {
          CountryList = ISO3166.Country.List.ToList();
        }

        cboCountry.SelectedValue = CountryList.Find(CountryA => Company.Country == CountryA.Name);
      }
    }
  }
}