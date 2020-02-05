using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace dedupe
{
    [Table("buildings")]
    class Building
    {        
        [Key]
        public int Id { get; set; }

        public int? Master_Building_Id { get; set; }
        
        public string Addr_Street_Name { get; set; }

        public string Addr_Nomer { get; set; }      
        
    }
}
