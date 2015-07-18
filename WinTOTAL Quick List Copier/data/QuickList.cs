namespace WinTOTAL_Quick_List_Copier.data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuickList")]
    public partial class QuickList
    {
        public QuickList()
        {
            QuickListEntries = new HashSet<QuickListEntry>();
        }

        [Key]
        public int QLID { get; set; }

        public int QLNameID { get; set; }

        [Required]
        [StringLength(25)]
        public string FieldName { get; set; }

        [Required]
        [StringLength(20)]
        public string FormType { get; set; }

        public int Number { get; set; }

        [Required]
        [StringLength(20)]
        public string ShortName { get; set; }

        public DateTime DateEntered { get; set; }

        public bool ShowInAllView { get; set; }

        public virtual QuickListName QuickListName { get; set; }

        public virtual ICollection<QuickListEntry> QuickListEntries { get; set; }
    }
}
