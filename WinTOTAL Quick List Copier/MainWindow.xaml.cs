using System;
using System.Collections.Generic;
using System.Data.Entity;
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

            txtSourceConnString.Focus();
        }

        private void lblLoadUsers_Click(object sender, RoutedEventArgs e)
        {
            var sourceConnStr = txtSourceConnString.Text;
            LoadUserList(sourceConnStr, lbSourceUsers);

            var isSingleServer= chkSingleServer.IsChecked.Value;
            var destConnStr = isSingleServer ? sourceConnStr : txtDestinationConnStr.Text;
            LoadUserList(sourceConnStr, lbDestinationUsers);
        }

        private async void LoadUserList(string connectionString, ListBox listbox)
        {
            listbox.Items.Clear();
            listbox.Items.Add("Loading...");

            try
            {
                using (var model = new WintotalModel(connectionString))
                {
                    var names = await (from n in model.QuickListNames
                                       join q in model.QuickLists on n.QLNameID equals q.QLNameID into qlstats
                                       orderby n.Name
                                       select n.Name + " - " + qlstats.Count() + " list(s)").ToListAsync();
                    listbox.Items.Clear();
                    foreach (var name in names.Distinct())
                    {
                        listbox.Items.Add(name);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred loading data: " + e.Message, "Error");
            }
        }

        private void lblCopyQuickLists_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
