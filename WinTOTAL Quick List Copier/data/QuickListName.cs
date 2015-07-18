namespace WinTOTAL_Quick_List_Copier.data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class QuickListName
    {
        public QuickListName()
        {
            QuickLists = new HashSet<QuickList>();
        }

        [Key]
        public int QLNameID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<QuickList> QuickLists { get; set; }
    }
}
