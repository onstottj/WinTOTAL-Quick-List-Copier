namespace WinTOTAL_Quick_List_Copier.data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class QuickListEntry
    {
        [Key]
        public int QLEntryID { get; set; }

        public int QLID { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(25)]
        public string FieldName { get; set; }

        [Column(TypeName = "text")]
        [Required(AllowEmptyStrings = true)]
        public string EntryText { get; set; }

        public virtual QuickList QuickList { get; set; }
    }
}
