using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace dedupe
{
    [Table("buildings")]
    class BuildingFull
    {
        [Key]
        public int Id { get; set; }
        public int? addr_street_id { get; set; }
        public int? Master_Building_Id { get; set; }
        public string Addr_Street_Name { get; set; }
        public string Addr_Nomer { get; set; }
        public string addr_misc { get; set; }
        public int? addr_korpus_flag { get; set; }
        public string addr_korpus { get; set; }
        public string addr_zip_code { get; set; }
        public int? tech_condition_id { get; set; }
        public DateTime? date_begin { get; set; }
        public DateTime? date_end { get; set; }
        public string num_floors { get; set; }
        public int? construct_year { get; set; }
        public int? condition_year { get; set; }
        public int? is_condition_valid { get; set; }
        public string bti_code { get; set; }
        public int? history_id { get; set; }
        public int? object_type_id { get; set; }
        public int? is_land { get; set; }
        public DateTime? modify_date { get; set; }
        public string modified_by { get; set; }
        public string kadastr_code { get; set; }
        public decimal? cost_balans { get; set; }
        public decimal? sqr_total { get; set; }
        public decimal? sqr_pidval { get; set; }
        public decimal? sqr_mk { get; set; }
        public decimal? sqr_dk { get; set; }
        public decimal? sqr_rk { get; set; }
        public decimal? sqr_other { get; set; }
        public decimal? sqr_rented { get; set; }
        public decimal? sqr_zagal { get; set; }
        public decimal? sqr_for_rent { get; set; }
        public decimal? sqr_habit { get; set; }
        public decimal? sqr_non_habit { get; set; }
        public string additional_info { get; set; }
        public int? is_deleted { get; set; }
        public DateTime? del_date { get; set; }
        public string oatuu_id { get; set; }
        public int? updpr { get; set; }
        public int? arch_id { get; set; }
        public int? arch_flag { get; set; }
        public int? facade_id { get; set; }
        public int? nomer_int { get; set; }
        public string characteristics { get; set; }
        public int? expl_enter_year { get; set; }
        public int? is_basement_exists { get; set; }
        public int? is_loft_exists { get; set; }
        public decimal? sqr_loft { get; set; }
        [NotMapped]
        public string normalizedAddress { get; set; }
    }
}
