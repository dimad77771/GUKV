using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.ImportToolUtils
{
    /// <summary>
    /// A placeholder for all functions that create field mappings between
    /// Firebird database tables and SQL Server tables
    /// </summary>
    public static class FieldMappings
    {
        public static void Create1NFObjectFieldMapping(Dictionary<string, string> fields, bool testOnly, bool enable1NFNewFieldsImport)
        {
            fields.Clear();

            fields.Add("OBJECT_KOD", "id");
            fields.Add("ULNAME", "addr_street_name");
            fields.Add("ULKOD", "addr_street_id");
            fields.Add("ULNAME2", "addr_street_name2");
            fields.Add("ULKOD2", "addr_street_id2");

            fields.Add("FULL_ULNAME", "street_full_name");
            fields.Add("DISTR", "addr_distr_old_id");
            fields.Add("NEWDISTR", "addr_distr_new_id");
            fields.Add("NOMER1", "addr_nomer1");
            fields.Add("NOMER2", "addr_nomer2");

            fields.Add("NOMER3", "addr_nomer3");
            fields.Add("ADRDOP", "addr_misc");
            fields.Add("KORPUS", "addr_korpus_flag");
            fields.Add("ADDRESS_NO_KORPUS", "addr_korpus");

            fields.Add("TEXSTAN", "tech_condition_id");
            fields.Add("DT_BEG", "date_begin");
            fields.Add("DT_END", "date_end");
            fields.Add("PNUM", "num_floors");
            fields.Add("BUDYEAR", "construct_year");

            fields.Add("STAN_YEAR", "condition_year");
            fields.Add("REALSTAN", "is_condition_valid");
            fields.Add("KODBTI", "bti_code");
            fields.Add("HISTORY", "history_id");
            fields.Add("TYPOBJ", "object_type_id");

            fields.Add("VIDOBJ", "object_kind_id");
            fields.Add("IKOD", "addr_zip_code");
            fields.Add("ZEMLIA", "is_land");
            fields.Add("DT", "modify_date");
            fields.Add("EIS_MODIFIED_BY", "modified_by");

            fields.Add("KADASTR", "kadastr_code");
            fields.Add("BALANS_VARTIST", "cost_balans");
            fields.Add("SZAG", "sqr_total");
            fields.Add("SPIDV", "sqr_pidval");
            fields.Add("V_MK", "sqr_mk");

            fields.Add("V_DK", "sqr_dk");
            fields.Add("V_RK", "sqr_rk");
            fields.Add("V_IN", "sqr_other");
            fields.Add("V_AR", "sqr_rented");
            fields.Add("V_OB", "sqr_zagal");

            fields.Add("V_PR", "sqr_for_rent");
            fields.Add("V_JIL", "sqr_habit");
            fields.Add("V_NEJ", "sqr_non_habit");
            fields.Add("DODAT_VID", "additional_info");
            fields.Add("DELETED", "is_deleted");

            fields.Add("DTDELETE", "del_date");
            fields.Add("KOD_OATUU", "oatuu_id");
            fields.Add("UPDPR", "updpr");
            fields.Add("ARCH_KOD", "arch_id");
            fields.Add("IN_ARCH", "arch_flag");

            fields.Add("OZNAKA_BUD", "facade_id");
            fields.Add("NOMER_INT", "nomer_int");

            if (!testOnly)
            {
                fields.Add("CHARACTERISTIC", "characteristics");
            }

            if (enable1NFNewFieldsImport)
            {
                fields.Add("EIS_EXPL_ENTER_YEAR", "expl_enter_year");
                fields.Add("EIS_BASEMENT_EXISTS", "is_basement_exists");
                fields.Add("EIS_LOFT_EXISTS", "is_loft_exists");
                fields.Add("EIS_LOFT_SQUARE", "sqr_loft");
            }
        }

        public static void Create1NFOrgFieldMapping(Dictionary<string, string> fields, bool enable1NFNewFieldsImport)
        {
            fields.Clear();

            fields.Add("KOD_OBJ", "id");
            fields.Add("LAST_SOST", "last_state");
            fields.Add("KOD_VID_DIAL", "occupation_id");
            fields.Add("KOD_STATUS", "status_id");
            fields.Add("KOD_FORM_GOSP", "form_gosp_id");

            // fields.Add("KOD_VID_PREDPR", "kind_id"); - duplicates "form_id"

            // Writing KOD_VID_PREDPR into a temporary column. After migration this column
            // is merged with the KOD_ORG_FORM column (form_id)
            fields.Add("KOD_VID_PREDPR", "temp_org_kind_id");

            fields.Add("KOD_FORM_VLASN", "form_ownership_id");
            fields.Add("KOD_GOSP_STRUKT", "gosp_struct_id");
            fields.Add("KOD_ORGAN", "organ_id");
            fields.Add("KOD_GALUZ", "industry_id");

            fields.Add("NOMER_OBJ", "nomer_obj");
            fields.Add("KOD_ZKPO", "zkpo_code");
            fields.Add("KOD_RAYON", "addr_distr_old_id");
            fields.Add("KOD_RAYON2", "addr_distr_new_id");
            fields.Add("NAME_UL", "addr_street_name");

            fields.Add("NOMER_DOMA", "addr_nomer");
            fields.Add("NOMER_DOMA2", "addr_nomer2");
            fields.Add("NOMER_KORPUS", "addr_korpus");
            fields.Add("POST_INDEX", "addr_zip_code");
            fields.Add("ADDITIONAL_ADRESS", "addr_misc");

            fields.Add("FIO_BOSS", "director_fio");
            fields.Add("TEL_BOSS", "director_phone");
            fields.Add("FIO_BOSS_R_VIDM", "director_fio_kogo");
            fields.Add("POST_BOSS", "director_title");
            fields.Add("POST_BOSS_R_VIDM", "director_title_kogo");

            fields.Add("DOC_BOSS", "director_doc");
            fields.Add("DOC_BOSS_R_VIDM", "director_doc_kogo");
            fields.Add("FIO_BUH", "buhgalter_fio");
            fields.Add("TEL_BUH", "buhgalter_phone");
            fields.Add("KOLVO_BUD", "num_buildings");

            fields.Add("FULL_NAME_OBJ", "full_name");
            fields.Add("SHORT_NAME_OBJ", "short_name");
            fields.Add("KOD_ADDITION_PRIZNAK", "priznak_id");
            fields.Add("KOD_PRIKMET", "title_form_id");
            fields.Add("KOD_PRIZNAK_1", "form_1nf_id");

            fields.Add("KOD_VIDOM_NAL", "vedomstvo_id");
            fields.Add("KOD_IMEN", "title_id");
            fields.Add("KOD_ORG_FORM", "form_id");
            fields.Add("KOD_VID_GOSP_STR", "gosp_struct_type_id");
            fields.Add("FIND_NAME_OBJ", "search_name");

            fields.Add("NAME_DAVAL_VIDM", "name_komu");
            fields.Add("TELEFAX", "fax");
            fields.Add("REESORG", "registration_auth");
            fields.Add("REESNO", "registration_num");
            fields.Add("REESDT", "registration_date");

            fields.Add("NO_SVIDOT", "registration_svidot");
            fields.Add("LYEAR", "l_year");
            fields.Add("DTLYEAR", "date_l_year");
            fields.Add("PL_BALANS", "sqr_on_balance");
            fields.Add("PL_VIROB_PRIZN", "sqr_manufact");

            fields.Add("PL_NOT_VIROB_PRIZN", "sqr_non_manufact");
            fields.Add("PL_ARENDA_VILNA", "sqr_free_for_rent");
            fields.Add("PL_ZAGAL", "sqr_total");
            fields.Add("PL_ARENDUETSYA", "sqr_rented");
            fields.Add("PL_PRIVAT", "sqr_privat");

            fields.Add("PL_NADANA_V_A", "sqr_given_for_rent");
            fields.Add("PL_ZNYATA_Z_BALANSU", "sqr_znyata_z_balansu");
            fields.Add("PL_PRODAJ", "sqr_prodaj");
            fields.Add("PL_SPISANI_ZNESENI", "sqr_spisani_zneseni");
            fields.Add("PL_PEREDANA_INSHUM", "sqr_peredana");

            fields.Add("KOLVO_OBJ", "num_objects");
            fields.Add("KOD_KVED", "kved_code");
            fields.Add("DT_STAT_DOVIDKA", "date_stat_spravka");

            fields.Add("KOATUU", "koatuu");
            fields.Add("EIS_MODIFIED_BY", "modified_by");
            fields.Add("DT", "modify_date");
            fields.Add("KOD_VID_AKCIA", "share_type_id");
            fields.Add("PERSENT_AKCIA", "share");

            fields.Add("NAME_BANK", "bank_name");
            fields.Add("MFO", "bank_mfo");
            fields.Add("DELETED", "is_deleted");
            fields.Add("DTDELETE", "del_date");
            fields.Add("VIDDIL", "otdel_gukv_id");

            fields.Add("ARCH_KOD", "arch_id");
            fields.Add("IN_ARCH", "arch_flag");
            fields.Add("LIKVID", "is_liquidated");
            fields.Add("LIKVID_DATE", "liquidation_date");
            fields.Add("PIDP_RDA", "pidp_rda");

            fields.Add("IS_AREND", "is_arend");
            fields.Add("DT_BEG_STAN", "beg_state_date");
            fields.Add("DT_END_STAN", "end_state_date");

            // The fields below were added to 1NF for compatibility with EIS
            if (enable1NFNewFieldsImport)
            {
                fields.Add("EIS_ADDR_STREET_ID", "addr_street_id");
                fields.Add("EIS_DIRECTOR_EMAIL", "director_email");
                fields.Add("EIS_BUH_EMAIL", "buhgalter_email");
                fields.Add("EIS_PHYS_ADDR_STREET_ID", "phys_addr_street_id");
                fields.Add("EIS_PHYS_ADDR_DISTRICT_ID", "phys_addr_district_id");
                fields.Add("EIS_PHYS_ADDR_NOMER", "phys_addr_nomer");
                fields.Add("EIS_PHYS_ADDR_ZIP_CODE", "phys_addr_zip_code");
                fields.Add("EIS_PHYS_ADDR_MISC", "phys_addr_misc");
                fields.Add("EIS_CONTRIB_RATE", "contribution_rate");

                fields.Add("EIS_BUDGET_NARAH_50_UAH", "budget_narah_50_uah");
                fields.Add("EIS_BUDGET_ZVIT_50_UAH", "budget_zvit_50_uah");
                fields.Add("EIS_BUDGET_PREV_50_UAH", "budget_prev_50_uah");
                fields.Add("EIS_BUDGET_DEBT_30_50_UAH", "budget_debt_30_50_uah");
                fields.Add("EIS_IS_SPECIAL_ORG", "is_special_organization");
                fields.Add("EIS_PAYMENT_BUDGET_SPECIAL", "payment_budget_special");

                fields.Add("EIS_KONKURS_PAYMENTS", "konkurs_payments");
                fields.Add("EIS_UNKNOWN_PAYMENTS", "unknown_payments");
                fields.Add("EIS_UNKNOWN_PAYMENT_NOTE", "unknown_payment_note");
            }
        }

        public static void Create1NFBalansFieldMapping(Dictionary<string, string> fields, bool testOnly, bool enable1NFNewFieldsImport)
        {
            fields.Clear();

            fields.Add("ID", "id");
            fields.Add("LYEAR", "year_balans");
            fields.Add("OBJECT", "building_id");
            fields.Add("ORG", "organization_id");
            fields.Add("SQR_ZAG", "sqr_total");

            fields.Add("SQR_PIDV", "sqr_pidval");
            fields.Add("SQR_VLAS", "sqr_vlas_potreb");
            fields.Add("SQR_FREE", "sqr_free");
            fields.Add("V_SQR_ORENDA", "sqr_in_rent");
            fields.Add("SQR_PRIV", "sqr_privatizov");

            fields.Add("SQR_VLASN_N", "sqr_not_for_rent");
            fields.Add("SQR_GURTOJ", "sqr_gurtoj");
            fields.Add("SQR_NEJ", "sqr_non_habit");
            fields.Add("SQR_KOR", "sqr_kor");
            fields.Add("BALANS_VARTIST", "cost_balans");

            fields.Add("SPRAV_VARTIST", "cost_fair");
            fields.Add("VARTIST_EXP", "cost_expert_1m");
            fields.Add("VARTIST_EXP_V", "cost_expert_total");
            fields.Add("OREND_NARAH", "cost_rent_narah");
            fields.Add("OREND_SPLACH", "cost_rent_payed");

            fields.Add("BORG", "cost_debt");
            fields.Add("ZAL_VART", "cost_zalishkova");
            fields.Add("VARTIST_RINK", "cost_rinkova");
            fields.Add("SPR_VART_1KVM", "cost_fair_1m");
            fields.Add("PPL_NUM", "num_people");

            fields.Add("V_DOG_ORENDA", "num_rent_agr");
            fields.Add("PRIV_COUNT", "num_privat_apt");
            fields.Add("O26", "o26_id");
            fields.Add("OBTI", "bti_id");
            fields.Add("RISHEN", "approval_by");

            fields.Add("RNOMER", "approval_num");
            fields.Add("RDATA", "approval_date");
            fields.Add("FORM_VLASN", "form_ownership_id");
            fields.Add("KINDOBJ", "object_kind_id");
            fields.Add("TYPEOBJ", "object_type_id");

            fields.Add("HISTORY", "history_id");
            fields.Add("TEXSTAN", "tech_condition_id");
            fields.Add("GRPURP", "purpose_group_id");
            fields.Add("PURPOSE", "purpose_id");
            fields.Add("PURP_STR", "purpose_str");

            fields.Add("FLOATS", "floors");
            fields.Add("PRIZNAK1NF", "priznak_1nf");
            fields.Add("PRAVO", "ownership_type_id");
            fields.Add("OBJ_ULNAME", "obj_street_name");

            fields.Add("OBJ_KODUL", "obj_street_id");
            fields.Add("ULNAME2", "obj_street_name2");
            fields.Add("ULKOD2", "obj_street_id2");
            fields.Add("OBJ_NOMER1", "obj_nomer1");
            fields.Add("OBJ_NOMER2", "obj_nomer2");

            fields.Add("OBJ_NOMER3", "obj_nomer3");
            fields.Add("OBJ_KODBTI", "obj_bti_code");
            fields.Add("OBJ_ADRDOP", "obj_addr_misc");
            fields.Add("OBJ_ULADRDOP", "obj_street_misc");

            fields.Add("EIS_MODIFIED_BY", "modified_by");
            fields.Add("DT", "modify_date");
            fields.Add("FORM_GIV", "form_giver_id");
            fields.Add("ORG_EXPL", "org_maintain_id");
            fields.Add("UPD_SOURCE", "update_src_id");

            fields.Add("DELETED", "is_deleted");
            fields.Add("DTDELETE", "del_date");
            fields.Add("ZNOS", "znos");
            fields.Add("ZNOS_DATE", "znos_date");
            fields.Add("DTEXP", "date_expert");

            fields.Add("REESNO", "reestr_no");
            fields.Add("DTSPRAV", "fair_cost_date");
            fields.Add("VIDDIL", "otdel_gukv_id");
            fields.Add("DATE_BTI", "date_bti");
            fields.Add("ARCH_KOD", "arch_id");

            fields.Add("IN_ARCH", "arch_flag");
            fields.Add("RINK_VART_DT", "date_cost_rinkova");

			fields.Add("GEODATA_MAP_POINTS", "geodata_map_opoints");

            fields.Add("ZNIZHINO_FLAG_", "znizhino_flag");
            fields.Add("ZNIZHINO_PERCENT_", "znizhino_percent");
            fields.Add("ZNIZHINO_STANOM_", "znizhino_stanom");

            fields.Add("ZNIZHINO_SHKODA_", "znizhino_shkoda");
            fields.Add("ZNIZHINO_ZVITAKT_", "znizhino_zvitakt");
            fields.Add("ZNIZHINO_PRIMITKA_", "znizhino_primitka");

            if (!testOnly)
            {
                fields.Add("MEMO", "memo");
                fields.Add("PRYMITKA", "note");
            }

            if (enable1NFNewFieldsImport)
            {
                fields.Add("EIS_FREE_SQR_EXISTS", "is_free_sqr");
                fields.Add("EIS_FREE_SQR_USEFUL", "free_sqr_useful");
                fields.Add("EIS_FREE_SQR_CONDITION", "free_sqr_condition_id");
                fields.Add("EIS_FREE_SQR_LOCATION", "free_sqr_location");

                fields.Add("EIS_OWNERSHIP_DOC_TYPE", "ownership_doc_type");
                fields.Add("EIS_OWNERSHIP_DOC_NUM", "ownership_doc_num");
                fields.Add("EIS_OWNERSHIP_DOC_DATE", "ownership_doc_date");

                fields.Add("EIS_BALANS_DOC_TYPE", "balans_doc_type");
                fields.Add("EIS_BALANS_DOC_NUM", "balans_doc_num");
                fields.Add("EIS_BALANS_DOC_DATE", "balans_doc_date");

                fields.Add("EIS_VIDCH_DOC_TYPE", "vidch_doc_type");
                fields.Add("EIS_VIDCH_DOC_NUM", "vidch_doc_num");
                fields.Add("EIS_VIDCH_DOC_DATE", "vidch_doc_date");

                fields.Add("EIS_VIDCH_TYPE_ID", "vidch_type_id");
                fields.Add("EIS_VIDCH_ORG_ID", "vidch_org_id");
                fields.Add("EIS_VIDCH_COST", "vidch_cost");

                fields.Add("EIS_SQR_ENGINEERING", "sqr_engineering");
                fields.Add("EIS_OBJ_STATUS_ID", "obj_status_id");
            }
        }

        public static void Create1NFArendaFieldMapping(Dictionary<string, string> fields, bool testOnly, bool enable1NFNewFieldsImport)
        {
            fields.Clear();

            fields.Add("ID", "id");
            fields.Add("OBJECT_KOD", "building_id");
            fields.Add("BALANS_KOD", "org_balans_id");
            fields.Add("ORG_KOD", "org_renter_id");
            fields.Add("ORGIV", "org_giver_id");

            fields.Add("BALID", "balans_id");
            fields.Add("LYEAR", "rent_year");
            fields.Add("OBJKIND", "object_kind_id");
            fields.Add("GRPURPOSE", "purpose_group_id");
            fields.Add("PURPOSE", "purpose_id");

            fields.Add("PURP_STR", "purpose_str");
            fields.Add("NAME", "name");
            fields.Add("PRIVAT", "is_privat");
            fields.Add("UPDT_SOURCE", "update_src_id");
            fields.Add("DOGKIND", "agreement_kind_id");

            fields.Add("DOGDATE", "agreement_date");
            fields.Add("DOGNUM", "agreement_num");
            fields.Add("DOGSTR", "agreement_str");
            fields.Add("POVERH", "floor_number");
            fields.Add("PPL_CNT", "num_people");

            fields.Add("PLATA_NARAH", "cost_narah");
            fields.Add("PLATA_SPLACH", "cost_payed");
            fields.Add("PLATA_BORG", "cost_debt");
            fields.Add("PLATA_DOGOVOR", "cost_agreement");
            fields.Add("VARTIST_EXP_V", "cost_expert_1m");

            fields.Add("VARTIST_EXP", "cost_expert_total");
            fields.Add("PIDSTAVA_STR", "pidstava");
            fields.Add("PIDSTAVA_DATE", "pidstava_date");
            fields.Add("PIDSTAVA_NUM", "pidstava_num");
            fields.Add("PIDSTAVA_FACT", "pidstava_fact");

            fields.Add("PIDSTAVA_STR2", "pidstava2");
            fields.Add("PIDSTAVA_NUM2", "pidstava_num2");
            fields.Add("PIDSTAVA_DATE2", "pidstava_date2");
            fields.Add("START_ORENDA", "rent_start_date");
            fields.Add("FINISH_ORENDA", "rent_finish_date");

            fields.Add("FINISH_ORENDA_FACT", "rent_actual_finish_date");
            fields.Add("AR_STAVKA", "rent_rate");
            fields.Add("AR_STAVKA_UAH", "rent_rate_uah");
            fields.Add("SQUARE", "rent_square");
            fields.Add("PRIZNAK_1NF", "priznak_1nf");

            fields.Add("SROK_BORG", "debt_timespan");
            fields.Add("ORDER_NO", "order_num");
            fields.Add("ORDER_DATE", "order_date");
            fields.Add("ORDER_NO2", "order_no2");

            fields.Add("RISHEN", "rishennya_id");
            fields.Add("NOACTIVE", "is_inactive");
            fields.Add("NOACTIVE_DT", "inactive_date");
            fields.Add("DELETED", "is_deleted");
            fields.Add("DTDELETE", "del_date");

            fields.Add("DTEXP", "date_expert");
            fields.Add("SUBAR", "is_subarenda");
            fields.Add("WAYPRIVAT", "privat_kind_id");
            fields.Add("KILK_PRIM", "num_primirnikiv");
            fields.Add("DATE_EXP", "date_expl_enter");

            fields.Add("NUM_AKT", "num_akt");
            fields.Add("DATE_AKT", "date_akt");
            fields.Add("NUM_BTI", "num_bti");
            fields.Add("DATE_BTI", "date_bti");
            fields.Add("SER_SVID", "svidotstvo_serial");

            fields.Add("NUM_SVID", "svidotstvo_num");
            fields.Add("DATE_SVID", "svidotstvo_date");
            fields.Add("KOD_VIDPLATA", "payment_type_id");
            fields.Add("ARCH_KOD", "arch_id");
            fields.Add("EIS_MODIFIED_BY", "modified_by");
            fields.Add("DT", "modify_date");

            if (!testOnly)
            {
                fields.Add("DESCRIPTION", "note");
            }

            if (enable1NFNewFieldsImport)
            {
                fields.Add("EIS_AGREEMENT_STATE", "agreement_state");
                fields.Add("EIS_IS_INSURED", "is_insured");
                fields.Add("EIS_INSURANCE_START", "insurance_start");
                fields.Add("EIS_INSURANCE_END", "insurance_end");
                fields.Add("EIS_INSURANCE_SUM", "insurance_sum");
                fields.Add("EIS_IS_LOAN_AGREEMENT", "is_loan_agreement");
            }
        }

        public static void Create1NFArendaNotesFieldMapping(Dictionary<string, string> fields, bool enable1NFNewFieldsImport)
        {
            fields.Clear();

            fields.Add("ARENDA", "arenda_id");
            fields.Add("GRPURP", "purpose_group_id");
            fields.Add("PURP", "purpose_id");
            fields.Add("PURP_STR", "purpose_str");
            fields.Add("SQUARE", "rent_square");

            fields.Add("DT", "modify_date");
            fields.Add("ISP", "modified_by");
            fields.Add("DESCRIPT", "note");

            /* fields.Add("AR_STAVKA", "cost_narah");
            fields.Add("NARAH", "cost_agreement");
            fields.Add("PRICE", "rent_rate"); */
            fields.Add("AR_STAVKA", "rent_rate");
            fields.Add("NARAH", "cost_agreement");
            fields.Add("PRICE", "rent_rate_uah");

            fields.Add("DELETED", "is_deleted");
            fields.Add("DTDELETE", "del_date");
            fields.Add("VARTIST_EXP", "cost_expert_total");
            fields.Add("DTEXP", "date_expert");
            fields.Add("KOD_VIDPLATA", "payment_type_id");

            if (enable1NFNewFieldsImport)
            {
                fields.Add("EIS_INVENT_NO", "invent_no");
                fields.Add("EIS_PRIM_STATUS", "note_status_id");
            }
        }

        public static Dictionary<string, string> ReverseMapping(Dictionary<string, string> fields)
        {
            Dictionary<string, string> reverseMapping = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> pair in fields)
            {
                reverseMapping.Add(pair.Value, pair.Key);
            }

            return reverseMapping;
        }

        public static Dictionary<int, int> CreateObjectTypeMappingNJFto1NF(int objTypeInBuilt1NF, int objTypeInBlock1NF, int objTypeOther1NF)
        {
            Dictionary<int, int> mapping = new Dictionary<int, int>();

            mapping.Add(1, objTypeInBuilt1NF);
            mapping.Add(2, 4);
            mapping.Add(3, 1);
            mapping.Add(4, 9);
            mapping.Add(5, 2);
            mapping.Add(6, 5);
            mapping.Add(7, objTypeInBlock1NF);
            mapping.Add(8, 3);
            mapping.Add(9, 2);
            mapping.Add(10, 1);
            mapping.Add(11, objTypeOther1NF);

            return mapping;
        }
    }
}
