using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinTOTAL_Quick_List_Copier.data
{
    public static class ConnectionTools
    {
        /// <summary>
        /// Customized version from http://stackoverflow.com/a/20254520/132374
        /// </summary>
        public static void ChangeDatabase(
            this DbContext source,
            string connectionString)
        {
            source.Database.Connection.ConnectionString = connectionString;
        }
    }
}
