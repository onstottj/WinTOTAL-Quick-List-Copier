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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.Resources["InverseBooleanConverter"] = new InverseBooleanConverter();
            InitializeComponent();
        }

        private void lnkRefreshSource_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentData(txtSourceConnString.Text);
        }

        private void lnkRefreshDestination_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentData(txtDestinationConnStr.Text);
        }

        private void UpdateCurrentData(string connectionString)
        {
            lbQuicklistUsers.Items.Clear();

            try
            {
                var finalConnectionString = @"metadata=res://*/data.WintotalSqlModel.csdl
                |res://*/data.WintotalSqlModel.ssdl
                |res://*/data.WintotalSqlModel.msl;
                provider=System.Data.SqlClient;
                provider connection string=" + "\"" + connectionString + "\"";

                using (var entities = new WinTOTAL_Quick_List_Copier.data.Entities())
                {
                    entities.ChangeDatabase(connectionString);
                    var names = from n in entities.QuickListNames
                                orderby n.Name
                                select n.Name;
                    foreach (var name in names.Distinct())
                    {
                        lbQuicklistUsers.Items.Add(name);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred loading data: " + e.Message);
            }
        }
    }
}
