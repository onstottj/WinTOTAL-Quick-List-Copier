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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinTOTAL_Quick_List_Copier.data;
using WinTOTAL_Quick_List_Copier.ui;

namespace WinTOTAL_Quick_List_Copier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.Resources["InverseBooleanConverter"] = new InverseBooleanConverter();
            InitializeComponent();
        }

        private void lnkRefreshSource_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            //var connectionString = "data source=dellosaurus;initial catalog=WinTOTAL_OLD;persist security info=True;user id=jon;password=***********;MultipleActiveResultSets=True;App=EntityFramework";
            UpdateCurrentData(txtSourceConnString.Text);
            e.Handled = true;
        }

        private void lnkRefreshDestination_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            UpdateCurrentData(txtDestinationConnStr.Text);
            e.Handled = true;
        }

        private void UpdateCurrentData(string connectionString)
        {
            var finalConnectionString = @"metadata=res://*/data.WintotalSqlModel.csdl
                |res://*/data.WintotalSqlModel.ssdl
                |res://*/data.WintotalSqlModel.msl;
                provider=System.Data.SqlClient;
                provider connection string=" + "\"" + connectionString + "\"";
            
            using (var entities = new WinTOTAL_Quick_List_Copier.data.Entities() )
            {
                entities.ChangeDatabase(connectionString);
            }
        }
    }
}
