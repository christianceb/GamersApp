using LiveCharts;
using LiveCharts.Wpf;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;

namespace GamersApp
{
  /// <summary>
  /// Interaction logic for Dashboard.xaml
  /// </summary>
  public partial class Dashboard : MetroWindow
  {
    public bool Modified = false;
    public Companies Companies;
    public string CompaniesFileName = "companies.json";
    private JsonSerializer serializer = new JsonSerializer();

    public Dashboard()
    {
      InitializeComponent();

      Companies = new Companies();

      // Loads the set company file name into the companies object. Unlikely to get false
      if (Companies.Load(CompaniesFileName))
      {
        // Routine for priming the Companies object
        DataContext = Companies;
        SetCards();
        SetStatus();
      } else
      {
        // Should be unlikely to happen but we'll handle it gracefully.
        MessageBox.Show(this, "Unexpected error encountered. Program will exit.", "Error");
        this.Close();
      }
    }

    /// <summary>
    /// Sets values of the chart control cards
    /// </summary>
    private void SetCards()
    {
      CompaniesPi();
      CountryByCompanies();
      CompaniesLetterOccurence();
      CompaniesNumberOccurence();
    }

    /// <summary>
    /// Sets the chart control card value of the graph in "Total Companies". Values are the
    /// first 10 digits in Pi if number of companies in company list is greater than 2 (need at
    /// least 2 plot points for a graph to be visible
    /// </summary>
    private void CompaniesPi()
    {
      if (Companies.Count < 1)
      {
        ChartControlTotalCompanies.SeriesData = new SeriesCollection
        {
          new LineSeries
          {
            Values = new ChartValues<int> { 0, 0 },
            PointGeometry = null
          }
        };
      } else
      {
        ChartControlTotalCompanies.SeriesData = new SeriesCollection
        {
          new LineSeries
          {
            Values = new ChartValues<int> { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3 },
            PointGeometry = null

          }
        };
      }
    }

    /// <summary>
    /// Sets the chart control card value of the graph in "Countries with Companies". If number
    /// of countries is greater than 2, Y axis will correspond to the number of companies in that
    /// country. X axis are the countries that has companies in alphabetical order
    /// </summary>
    private void CountryByCompanies()
    {
      IChartValues ChartValues = new ChartValues<int>();
      List<KeyValuePair<string, int>> CountByCountry = Companies.CountByCountry();

      if (CountByCountry.Count == 0)
      {
        ChartValues.Add(0);
        ChartValues.Add(0);
      } else
      {
        foreach (KeyValuePair<string, int> Group in CountByCountry)
        {
          ChartValues.Add(Group.Value);
        }
      }

      ChartControlCompaniesByCountry.SeriesData = new SeriesCollection
      {
        new LineSeries { Values = ChartValues, PointGeometry = null }
      };
    }

    /// <summary>
    /// Gets the number of occurences of every letter on the company names and sets it as the data in the graph.
    /// </summary>
    private void CompaniesLetterOccurence()
    {
      SortedDictionary<string, int> Characters = new SortedDictionary<string, int>();

      // Populate SortedDictionary with keys first
      for (char Char = 'a'; Char <= 'z'; Char++)
      {
        Characters[Char.ToString()] = 0;
      }

      if ( Companies.List.Count > 2)
      {
        int TotalCharacters = 0;

        // Loop through the list of companies and their names;
        foreach (Company Company in Companies.List)
        {
          foreach (char Char in Company.Name)
          {
            if (Char.IsLetter(Char))
            {
              Characters[Char.ToString().ToLower()]++;
            }
          }
        }

        IChartValues ChartValues = new ChartValues<int>();
        foreach (KeyValuePair<string, int> Pair in Characters)
        {
          TotalCharacters += Pair.Value;
          ChartValues.Add(Pair.Value);
          ChartControlNameCharCount.SeriesData = new SeriesCollection
          {
            new LineSeries { Values = ChartValues, PointGeometry = null }
          };
        }

        ChartControlNameCharCount.CardContent = TotalCharacters.ToString();
      }
    }

    /// <summary>
    /// Similar to CompaniesLetterOccurence, this one counts the numbers instead.
    /// </summary>
    private void CompaniesNumberOccurence()
    {
      SortedDictionary<string, int> Characters = new SortedDictionary<string, int>();

      // Populate SortedDictionary with keys first
      for (int i = 0; i <= 9; i++)
      {
        Characters[i.ToString()] = 0;
      }

      if (Companies.List.Count > 2)
      {
        int TotalCharacters = 0;

        // Loop through the list of companies and their names;
        foreach (Company Company in Companies.List)
        {
          foreach (char Char in Company.Name)
          {
            if (Char.IsDigit(Char))
            {
              Characters[Char.ToString().ToLower()]++;
            }
          }
        }

        IChartValues ChartValues = new ChartValues<int>();
        foreach (KeyValuePair<string, int> Pair in Characters)
        {
          TotalCharacters += Pair.Value;
          ChartValues.Add(Pair.Value);
          ChartControlNumberCharCount.SeriesData = new SeriesCollection
          {
            new LineSeries { Values = ChartValues, PointGeometry = null }
          };
        }

        ChartControlNumberCharCount.CardContent = TotalCharacters.ToString();
      }
    }

    /// <summary>
    /// Sets the status indicators on the bottom right of the window based on the status of
    /// the lock keys.
    /// </summary>
    private void CheckAndSetLockStatus()
    {
      if (System.Windows.Input.Keyboard.IsKeyToggled(System.Windows.Input.Key.NumLock))
      {
        TbkNum.ClearValue(TextBox.ForegroundProperty);
      }
      else
      {
        TbkNum.Foreground = new SolidColorBrush(Colors.Gray);
      }

      if (System.Windows.Input.Keyboard.IsKeyToggled(System.Windows.Input.Key.CapsLock))
      {
        TbkCaps.ClearValue(TextBox.ForegroundProperty);
      }
      else
      {
        TbkCaps.Foreground = new SolidColorBrush(Colors.Gray);
      }

      if (System.Windows.Input.Keyboard.IsKeyToggled(System.Windows.Input.Key.Scroll))
      {
        TbkScroll.ClearValue(TextBox.ForegroundProperty);
      }
      else
      {
        TbkScroll.Foreground = new SolidColorBrush(Colors.Gray);
      }
    }

    /// <summary>
    /// Sets status on the bottom left of the window based off the status in the Companies
    /// class. Statuses mostly pertain to the file operation statuses of the companies
    /// file used.
    /// </summary>
    private void SetStatus() => TbkStatus.Text = Companies.LoadStatus;

    private void MitSave_Checked(object sender, RoutedEventArgs e)
    {
      Companies.Save();
      SetStatus();
    }

    private void MitCompanies_Click(object sender, RoutedEventArgs e)
    {
      // Invokes the window to show the companies list.
      CompanyList companyList = new CompanyList();
      companyList.DataContext = Companies;
      companyList.Owner = this;
      companyList.ShowDialog();
    }

    private void MitAbout_Click(object sender, RoutedEventArgs e)
    {
      // Invokes the window to show the about window.
      About about = new About();
      about.Owner = this;
      about.ShowDialog();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      // Check if there have been any changes in the company list or the companies.
      if (Modified)
      {
        // Prompt user to save the unsaved changes
        MessageBoxResult MessageBoxResult = MessageBox.Show(
          "Do you want to save your unsaved changes before exiting?",
          "Warning",
          MessageBoxButton.YesNoCancel,
          MessageBoxImage.Warning
        );

        // Save the changes
        if (MessageBoxResult == MessageBoxResult.Yes)
        {
          Companies.Save();
          SetStatus();
        }
        else if (MessageBoxResult == MessageBoxResult.Cancel)
        {
          // Cancel exit sequence of this application.
          e.Cancel = true;
        }
      }
    }

    /// <summary>
    /// When application is brought to the foreground, modify lock key indicators if needed
    /// </summary>
    private void MetroWindow_Activated(object sender, System.EventArgs e) => CheckAndSetLockStatus();

    /// <summary>
    /// On keyup in windoow, modify lock key indicators if needed;
    /// </summary>
    private void MetroWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) => CheckAndSetLockStatus();

    private void MitQuit_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}