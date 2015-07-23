using System.Windows;

namespace WinTOTAL_Quick_List_Copier.ui
{
    public static class UiUtilities
    {
        public static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error");
        }
    }
}
