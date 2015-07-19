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
        private Dictionary<int, string> sourceUsers = new Dictionary<int, string>();
        private Dictionary<int, string> destinationUsers = new Dictionary<int, string>();

        public MainWindow()
        {
            this.Resources["InverseBooleanConverter"] = new InverseBooleanConverter();
            InitializeComponent();

            txtSourceConnString.Focus();
        }

        private void lblLoadUsers_Click(object sender, RoutedEventArgs e)
        {
            // Load source users
            var sourceConnStr = txtSourceConnString.Text;
            LoadUserList(sourceConnStr, lbSourceUsers, sourceUsers);

            // Load destination users
            var isSingleServer = chkSingleServer.IsChecked.Value;
            var destConnStr = isSingleServer ? sourceConnStr : txtDestinationConnStr.Text;
            LoadUserList(sourceConnStr, lbDestinationUsers, destinationUsers);
        }

        private async void LoadUserList(string connectionString, ListBox listbox, Dictionary<int, string> userDictionary)
        {
            listbox.Items.Clear();
            listbox.Items.Add("Loading...");

            try
            {
                using (var model = new WintotalModel(connectionString))
                {
                    var users = await (from n in model.QuickListNames
                                       join q in model.QuickLists on n.QLNameID equals q.QLNameID into qlstats
                                       orderby n.Name
                                       select new WintotalUser
                                       {
                                           QLNameID = n.QLNameID,
                                           Name = n.Name + " - " + qlstats.Count() + " list(s)"
                                       }).ToListAsync();

                    listbox.Items.Clear();
                    listbox.SelectedValuePath = "QLNameID";
                    listbox.DisplayMemberPath = "Name";

                    userDictionary.Clear();
                    foreach (var user in users.Distinct())
                    {
                        listbox.Items.Add(user);
                        userDictionary.Add(user.QLNameID, user.Name);
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
            var sourceUser = (WintotalUser)lbSourceUsers.SelectedItem;
            MessageBox.Show(sourceUser != null ? "User " + sourceUser.Name + " with id " + sourceUser.QLNameID : "empty");
        }
    }

    class WintotalUser
    {
        public int QLNameID { get; set; }
        public string Name { get; set; }
    }
}
