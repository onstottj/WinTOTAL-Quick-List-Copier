using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WinTOTAL_Quick_List_Copier.data;
using WinTOTAL_Quick_List_Copier.ui;
using WinTOTAL_Quick_List_Copier.wintotal;

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
            // Load source users
            var sourceConnStr = txtSourceConnString.Text;
            LoadUserList(sourceConnStr, lbSourceUsers);

            // Load destination users
            LoadUserList(GetDestinationConnectionString(), lbDestinationUsers);
        }

        private string GetDestinationConnectionString()
        {
            var isSingleServer = chkSingleServer.IsChecked.Value;
            return isSingleServer ? txtSourceConnString.Text : txtDestinationConnStr.Text;
        }

        private async void LoadUserList(string connectionString, ListBox listbox)
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

                    foreach (var user in users.Distinct())
                    {
                        listbox.Items.Add(user);
                    }
                }
            }
            catch (Exception e)
            {
                UiUtilities.ShowErrorMessage("An error occurred loading data: " + e.Message);
            }
        }

        private void lblCopyQuickLists_Click(object sender, RoutedEventArgs e)
        {
            var sourceUser = (WintotalUser)lbSourceUsers.SelectedItem;
            var destinationUser = (WintotalUser)lbDestinationUsers.SelectedItem;
            if (sourceUser == null)
            {
                UiUtilities.ShowErrorMessage("Please select a user to copy quick lists from.");
            }
            else if (destinationUser == null)
            {
                UiUtilities.ShowErrorMessage("Please select a user to copy quick lists to.");
            }
            else
            {
                var sourceConnStr = txtSourceConnString.Text;
                var destConnStr = GetDestinationConnectionString();
                WintotalUtilities.CopyQuickLists(sourceUser.QLNameID, destinationUser.QLNameID, sourceConnStr, destConnStr);
            }
        }

        private void lblLoadUsersForDeletion_Click(object sender, RoutedEventArgs e)
        {
            LoadUserList(GetDestinationConnectionString(), lbDestinationUsersForDeletion);
        }

        private void lblDeleteQuickLists_Click(object sender, RoutedEventArgs e)
        {
            var destinationUser = (WintotalUser)lbDestinationUsersForDeletion.SelectedItem;
            if (destinationUser == null)
            {
                UiUtilities.ShowErrorMessage("Please select a user whose quick lists should be deleted.");
            }
            else
            {
                var destConnStr = GetDestinationConnectionString();
                WintotalUtilities.DeleteQuickListsOfUser(destinationUser.QLNameID, destConnStr);
            }
        }
    }

    class WintotalUser
    {
        public int QLNameID { get; set; }
        public string Name { get; set; }
    }
}
