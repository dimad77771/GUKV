using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace dedupe
{
    [Table("dict_streets")]
    class Street
    {        
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Name_Output { get; set; }
        
        public string Kind { get; set; }
    }
}
