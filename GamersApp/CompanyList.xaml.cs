using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GamersApp
{
    /// <summary>
    /// Interaction logic for CompanyList.xaml
    /// </summary>
    public partial class CompanyList : Window
    {
        public CompanyList()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Dashboard dashboardWindow = new Dashboard();
            this.Close();
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            CompanyForm companyForm = new CompanyForm();
            companyForm.Owner = this;
            companyForm.Show();
        }
    }
}
