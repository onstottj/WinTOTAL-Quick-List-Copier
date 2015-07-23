using System;
using System.Data.Entity;
using System.Linq;
using WinTOTAL_Quick_List_Copier.data;
using WinTOTAL_Quick_List_Copier.ui;

namespace WinTOTAL_Quick_List_Copier.wintotal
{
    public static class WintotalUtilities
    {

        public static void CopyQuickLists(int sourceQlNameID, int destQlNameID, string sourceConnStr, string destConnStr)
        {
            // The technique used to copy the quick lists came from http://stackoverflow.com/a/18114082/132374
            try
            {
                using (var sourceModel = new WintotalModel(sourceConnStr))
                {
                    using (var destinationModel = new WintotalModel(destConnStr))
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
                UiUtilities.ShowErrorMessage("An error occurred while copying the quick lists: " + e.Message);
            }
        }

        public static void DeleteQuickListsOfUser(int qlNameID, string connectionString)
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
                UiUtilities.ShowErrorMessage("An error occurred while attempting to delete the destination "
                    + "user's quick lists: " + e.Message);
            }
        }

    }
}
