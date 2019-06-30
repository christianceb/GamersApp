using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GamersApp
{
  public class Companies
  {
    public List<Company> List;
    public int Count { get => GetCount(); }
    public int CountriesCount { get => GetCountriesCountWithCompanies(); }
    public string LoadStatus;
    public string LoadStatusShort;
    public string FileURI;
    private string FileName;
    private JsonSerializer serializer = new JsonSerializer();

    /// <summary>
    /// Load the file that contains the JSON of companies. If file does not exist, creates an empty JSON file.
    /// </summary>
    /// <param name="FileName">The file name of the file to load. If file does not exist, the filename will be used to create the file.</param>
    /// <returns>True if the file was successfully (created) loaded and parsed. Unlikely to return false</returns>
    public bool Load(string FileName)
    {
      bool Loaded = false;
      bool Created = false;
      this.FileName = FileName;

      // Create File if it doesn't exist from an empty list
      if (!File.Exists(FileName))
      {
        using (StreamWriter file = File.CreateText(FileName))
        {
          List = new List<Company>();
          serializer.Serialize(file, List);
          Created = true;
        }
      }

      // Load file
      using (StreamReader file = File.OpenText(FileName))
      {
        List = (List<Company>)serializer.Deserialize(file, typeof(List<Company>));
        Loaded = true;

        FileInfo TheFile = new FileInfo(FileName);
        FileURI = TheFile.FullName;
      }

      // Prime status messages
      if (Created == false && Loaded)
      {
        LoadStatusShort = "NORMAL_LOAD";
        LoadStatus = "Loaded at " + FileURI;
      }
      else if (Created && Loaded)
      {
        LoadStatusShort = "CREATE_LOAD";
        LoadStatus = "Created and Loaded at " + FileURI;
      }
      else
      {
        LoadStatusShort = "ERROR_LOAD";
        LoadStatus = "Error Loading " + FileName;
      }

      return Loaded;
    }

    /// <summary>
    /// Saves the companies in the list into the file set in the class.
    /// </summary>
    public void Save()
    {
      using (StreamWriter file = File.CreateText(FileName))
      {
        serializer.Serialize(file, List);
      }

      // Set status message
      LoadStatusShort = "SAVED";
      LoadStatus = "Saved at " + FileURI;
    }

    /// <summary>
    /// Get number of companies on this instance of this class.
    /// </summary>
    /// <returns></returns>
    private int GetCount()
    {
      int ListCount = 0;

      if (List != null)
      {
        ListCount = List.Count;
      }

      return ListCount;
    }

    /// <summary>
    /// Returns a list of countries and the number of companies it has based on the list of companies on this class. The countries are sorted in an alphabetical order.
    /// </summary>
    /// <returns>A list of key/value pairs of the countries and their corresponding number of companies</returns>
    public List<KeyValuePair<string, int>> CountByCountry()
    {
      List<KeyValuePair<string, int>> ByCountryFinal = new List<KeyValuePair<string, int>>();
      Dictionary<string, int> ByCountry = new Dictionary<string, int>();

      var query = List.GroupBy(Company => Company.Country);
      foreach (var item in query)
      {
        ByCountry.Add(item.Key, item.Count());
      }

      ByCountryFinal = ByCountry.ToList();

      ByCountryFinal.Sort(
        delegate ( KeyValuePair<string, int> PairA, KeyValuePair<string, int> PairB) {
          return PairA.Key.CompareTo(PairB.Key);
        }
      );

      return ByCountryFinal;
    }

    /// <summary>
    /// Gets the number of countries with companies in them based on the list of companies on this instance.
    /// </summary>
    /// <returns>The number of countries containing at least one company</returns>
    public int GetCountriesCountWithCompanies()
    {
      int CountriesCount = 0;

      Dictionary<string, int> ByCountry = new Dictionary<string, int>();
      var query = List.GroupBy(Company => Company.Country);

      CountriesCount = query.Count();

      return CountriesCount;
    }
  }
}