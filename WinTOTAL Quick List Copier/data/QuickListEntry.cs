//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WinTOTAL_Quick_List_Copier.data
{
    using System;
    using System.Collections.Generic;
    
    public partial class QuickListEntry
    {
        public int QLEntryID { get; set; }
        public int QLID { get; set; }
        public string FieldName { get; set; }
        public string EntryText { get; set; }
    
        public virtual QuickList QuickList { get; set; }
    }
}