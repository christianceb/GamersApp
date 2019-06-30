using LiveCharts;
using System.Windows;
using System.Windows.Controls;

namespace GamersApp
{
  /// <summary>
  /// Interaction logic for ChartControl.xaml
  /// </summary>
  public partial class ChartControl : UserControl
  {
    public ChartControl()
    {
      InitializeComponent();
      CardGrid.DataContext = this;
    }

    public string CardTitle
    {
      set { SetValue(CardTitleText, value); }
    }

    public static readonly DependencyProperty CardTitleText = DependencyProperty.Register(
      "CardTitle",
      typeof(string),
      typeof(ChartControl),
      new PropertyMetadata(null)
    );

    public string CardContent
    {
      set { SetValue(CardContentText, value); }
    }

    public static readonly DependencyProperty CardContentText = DependencyProperty.Register(
      "CardContent",
      typeof(string),
      typeof(ChartControl),
      new PropertyMetadata(null)
    );

    public SeriesCollection SeriesData
    {
      get { return null; }
      set { SetValue(SeriesDataProperty, value); }
    }

    public static readonly DependencyProperty SeriesDataProperty = DependencyProperty.Register(
      "SeriesData",
      typeof(SeriesCollection),
      typeof(ChartControl),
      new PropertyMetadata(new SeriesCollection())
    );
  }
}
