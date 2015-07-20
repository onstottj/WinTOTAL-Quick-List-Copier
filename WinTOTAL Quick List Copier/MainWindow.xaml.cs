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
            LoadUserList(GetDestinationConnectionString(), lbDestinationUsers, destinationUsers);
        }

        private string GetDestinationConnectionString()
        {
            var isSingleServer = chkSingleServer.IsChecked.Value;
            return isSingleServer ? txtSourceConnString.Text : txtDestinationConnStr.Text;
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
                ShowErrorMessage("An error occurred loading data: " + e.Message);
            }
        }

        private void lblCopyQuickLists_Click(object sender, RoutedEventArgs e)
        {
            var sourceUser = (WintotalUser)lbSourceUsers.SelectedItem;
            var destinationUser = (WintotalUser)lbDestinationUsers.SelectedItem;
            if (sourceUser == null)
            {
                ShowErrorMessage("Please select a user to copy quick lists from.");
            }
            else if (destinationUser == null)
            {
                ShowErrorMessage("Please select a user to copy quick lists to.");
            }
            else
            {
                DeleteQuickListsOfUser(destinationUser.QLNameID, GetDestinationConnectionString());

                CopyQuickLists(sourceUser.QLNameID, destinationUser.QLNameID);
            }
        }

        private void DeleteQuickListsOfUser(int qlNameID, string connectionString)
        {
            try
            {
                using (var model = new WintotalModel(connectionString))
                {
                    var quickLists = from q in model.QuickLists
                                     where q.QLNameID == qlNameID
                                     select q;
                    foreach (var quickList in quickLists.ToList())
                    {
                        model.QuickLists.Remove(quickList);
                    }

                    model.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ShowErrorMessage("An error occurred while attempting to delete the destination user's quick lists: " + e.Message);
            }
        }

        private void CopyQuickLists(int sourceQlNameID, int destQlNameID)
        {
            // The technique used to copy the quick lists came from http://stackoverflow.com/a/18114082/132374
            try
            {
                using (var sourceModel = new WintotalModel(txtSourceConnString.Text))
                {
                    using (var destinationModel = new WintotalModel(GetDestinationConnectionString()))
                    {
                        var quickLists = (from q in sourceModel.QuickLists
                                          where q.QLNameID == sourceQlNameID
                                          select q).AsNoTracking();
                        foreach (var quickList in quickLists.ToList())
                        {
                            // Assign the quick list to the destination user
                            quickList.QLNameID = destQlNameID;

                            // The PK value will be recreated
                            quickList.QLID = 0;

                            foreach (var entry in quickList.QuickListEntries)
                            {
                                // The PK value will be recreated
                                entry.QLEntryID = 0;
                            }

                            destinationModel.QuickLists.Add(quickList);
                        }

                        destinationModel.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                ShowErrorMessage("An error occurred while copying the quick lists: " + e.Message);
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error");
        }
    }

    class WintotalUser
    {
        public int QLNameID { get; set; }
        public string Name { get; set; }
    }
}
