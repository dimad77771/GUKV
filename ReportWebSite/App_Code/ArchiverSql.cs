using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using ExtDataEntry.Models;

/// <summary>
/// Summary description for ArchiverSql
/// </summary>
public static class ArchiverSql
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionSql"></param>
    /// <param name="arendaId"></param>
    /// <param name="transactionSql"></param>
    /// <returns></returns>
    public static int CreateArendaArchiveRecord(SqlConnection connectionSql, int arendaId, string user, SqlTransaction transactionSql)
    {
        int new_archive_link_code = 0;
        using (SqlCommand cmdNewLinkCode = new SqlCommand("select MAX(archive_link_code) + 1 from arch_arenda", connectionSql, transactionSql))
        {
            using (SqlDataReader r = cmdNewLinkCode.ExecuteReader())
            {
                if (r.Read())
                    new_archive_link_code = (int)r.GetValue(0);
                r.Close();
            }
        }

        string sql = @"insert into arch_arenda (
            id,building_id,org_balans_id,org_renter_id,org_giver_id,balans_id,rent_year,object_kind_id,purpose_group_id,purpose_id,purpose_str,name,is_privat,update_src_id,
            agreement_kind_id,agreement_date,agreement_num,agreement_str,floor_number,num_people,cost_narah,cost_payed,cost_debt,cost_agreement,cost_expert_1m,cost_expert_total,
            pidstava,pidstava_date,pidstava_num,pidstava_fact,pidstava2,pidstava_num2,pidstava_date2,pidstava_display,rent_start_date,rent_finish_date,rent_actual_finish_date,
            rent_rate,rent_rate_uah,rent_square,priznak_1nf,debt_timespan,order_num,order_date,order_no2,rishennya_id,is_inactive,inactive_date,is_deleted,del_date,date_expert,
            is_subarenda,privat_kind_id,num_primirnikiv,date_expl_enter,num_akt,date_akt,num_bti,date_bti,svidotstvo_serial,svidotstvo_num,svidotstvo_date,payment_type_id,
            arch_id,modified_by,modify_date,note,agreement_state,is_insured,insurance_start,insurance_end,insurance_sum,is_loan_agreement,
            base_month,method_calc_id,
            
            rent_period_id,report_id
            
            ,archive_create_date,archive_create_user,archive_link_code)
        select 
            id,building_id,org_balans_id,org_renter_id,org_giver_id,balans_id,rent_year,object_kind_id,purpose_group_id,purpose_id,purpose_str,name,is_privat,update_src_id,
            agreement_kind_id,agreement_date,agreement_num,agreement_str,floor_number,num_people,cost_narah,cost_payed,cost_debt,cost_agreement,cost_expert_1m,cost_expert_total,
            pidstava,pidstava_date,pidstava_num,pidstava_fact,pidstava2,pidstava_num2,pidstava_date2,pidstava_display,rent_start_date,rent_finish_date,rent_actual_finish_date,
            rent_rate,rent_rate_uah,rent_square,priznak_1nf,debt_timespan,order_num,order_date,order_no2,rishennya_id,is_inactive,inactive_date,is_deleted,del_date,date_expert,
            is_subarenda,privat_kind_id,num_primirnikiv,date_expl_enter,num_akt,date_akt,num_bti,date_bti,svidotstvo_serial,svidotstvo_num,svidotstvo_date,payment_type_id,
            arch_id,modified_by,modify_date,note,agreement_state,is_insured,insurance_start,insurance_end,insurance_sum,is_loan_agreement,
            base_month,method_calc_id,
            
            (select top 1 p.rent_period_id from reports1nf_arenda_payments p join reports1nf_arenda a on a.id = p.arenda_id where a.id = @arendaId order by p.modify_date desc) as rent_period_id
            , (select report_id from reports1nf_arenda where id = @arendaId) as report_id

            ,@archive_create_date,@archive_create_user,@archive_link_code
        from arenda
        where id = @arendaId";

        using (SqlCommand cmdArchArenda = new SqlCommand(sql, connectionSql, transactionSql))
        {
            cmdArchArenda.Parameters.AddWithValue("archive_create_date", DateTime.Now.Date);
            cmdArchArenda.Parameters.AddWithValue("archive_create_user", user);
            cmdArchArenda.Parameters.AddWithValue("archive_link_code", new_archive_link_code);
            cmdArchArenda.Parameters.AddWithValue("arendaId", arendaId);

            cmdArchArenda.ExecuteNonQuery();
        }

        sql = @"insert into arch_link_arenda_2_decisions (
            arenda_id,rishen_id,ord,modified_by,modify_date,doc_num,doc_date,doc_dodatok,doc_punkt,purpose_str,rent_square,decision_id,doc_raspor_id,pidstava,
            archive_arenda_link_code)
            select 
            arenda_id,rishen_id,ord,modified_by,modify_date,doc_num,doc_date,doc_dodatok,doc_punkt,purpose_str,rent_square,decision_id,doc_raspor_id,pidstava,
            @archive_arenda_link_code
            from link_arenda_2_decisions
            where arenda_id = @arendaId";

        using (SqlCommand cmdArchArenda2Decisions = new SqlCommand(sql, connectionSql, transactionSql))
        {
            cmdArchArenda2Decisions.Parameters.AddWithValue("archive_arenda_link_code", new_archive_link_code);
            cmdArchArenda2Decisions.Parameters.AddWithValue("arendaId", arendaId);

            cmdArchArenda2Decisions.ExecuteNonQuery();
        }

        sql = @"insert into arch_arenda_notes (
            arenda_id,purpose_group_id,purpose_id,purpose_str,rent_square,modify_date,modified_by,note,rent_rate,rent_rate_uah,cost_narah,cost_agreement,is_deleted,del_date,cost_expert_total,date_expert,payment_type_id,invent_no,note_status_id,zapezh_deposit,
            archive_arenda_link_code)
            select 
            arenda_id,purpose_group_id,purpose_id,purpose_str,rent_square,modify_date,modified_by,note,rent_rate,rent_rate_uah,cost_narah,cost_agreement,is_deleted,del_date,cost_expert_total,date_expert,payment_type_id,invent_no,note_status_id,zapezh_deposit,
            @archive_arenda_link_code
            from arenda_notes
            where arenda_id = @arendaId";

        using (SqlCommand cmdArchArendaNotes = new SqlCommand(sql, connectionSql, transactionSql))
        {
            cmdArchArendaNotes.Parameters.AddWithValue("archive_arenda_link_code", new_archive_link_code);
            cmdArchArendaNotes.Parameters.AddWithValue("arendaId", arendaId);

            cmdArchArendaNotes.ExecuteNonQuery();
        }
        return new_archive_link_code;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionSql"></param>
    /// <param name="balansId"></param>
    /// <param name="transactionSql"></param>
    /// <returns></returns>
    public static void CreateBalansArchiveRecord(SqlConnection connectionSql, int balansId, string user, SqlTransaction transactionSql)
    {
        int new_archive_link_code = 0;
        using (SqlCommand cmdNewLinkCode = new SqlCommand("select MAX(archive_link_code) + 1 from arch_balans", connectionSql, transactionSql))
        {
            using (SqlDataReader r = cmdNewLinkCode.ExecuteReader())
            {
                if (r.Read())
                    new_archive_link_code = (int)r.GetValue(0);
                r.Close();
            }
        }

        string sql = @"insert into arch_balans (
            id,year_balans,building_id,organization_id,sqr_total,sqr_pidval,sqr_vlas_potreb,sqr_free,sqr_in_rent,sqr_privatizov,sqr_not_for_rent,sqr_gurtoj,sqr_non_habit,sqr_kor,cost_balans,
            cost_fair,cost_expert_1m,cost_expert_total,cost_rent_narah,cost_rent_payed,cost_debt,cost_zalishkova,cost_rinkova,cost_fair_1m,num_people,num_rent_agr,num_privat_apt,o26_id,
            bti_id,approval_by,approval_num,approval_date,form_ownership_id,object_kind_id,object_type_id,history_id,tech_condition_id,purpose_group_id,purpose_id,purpose_str,floors,
            priznak_1nf,ownership_type_id,obj_street_name,obj_street_id,obj_street_name2,obj_street_id2,obj_nomer1,obj_nomer2,obj_nomer3,obj_nomer,obj_bti_code,obj_addr_misc,obj_street_misc,
            modified_by,modify_date,form_giver_id,org_maintain_id,update_src_id,is_deleted,del_date,znos,znos_date,date_expert,reestr_no,fair_cost_date,otdel_gukv_id,date_bti,arch_id,
            arch_flag,date_cost_rinkova,memo,note,is_free_sqr,free_sqr_useful,free_sqr_condition_id,free_sqr_location,ownership_doc_type,ownership_doc_num,ownership_doc_date,balans_doc_type,
            balans_doc_num,balans_doc_date,vidch_doc_type,vidch_doc_num,vidch_doc_date,vidch_type_id,vidch_org_id,vidch_cost,sqr_engineering,obj_status_id,geodata_map_opoints,
            znizhino_flag,znizhino_percent,znizhino_stanom,znizhino_shkoda,znizhino_zvitakt,znizhino_primitka,
            
            rent_period_id,report_id
            
            ,archive_create_date,archive_create_user,archive_link_code)
        select 
            id,year_balans,building_id,organization_id,sqr_total,sqr_pidval,sqr_vlas_potreb,sqr_free,sqr_in_rent,sqr_privatizov,sqr_not_for_rent,sqr_gurtoj,sqr_non_habit,sqr_kor,cost_balans,
            cost_fair,cost_expert_1m,cost_expert_total,cost_rent_narah,cost_rent_payed,cost_debt,cost_zalishkova,cost_rinkova,cost_fair_1m,num_people,num_rent_agr,num_privat_apt,o26_id,
            bti_id,approval_by,approval_num,approval_date,form_ownership_id,object_kind_id,object_type_id,history_id,tech_condition_id,purpose_group_id,purpose_id,purpose_str,floors,
            priznak_1nf,ownership_type_id,obj_street_name,obj_street_id,obj_street_name2,obj_street_id2,obj_nomer1,obj_nomer2,obj_nomer3,obj_nomer,obj_bti_code,obj_addr_misc,obj_street_misc,
            modified_by,modify_date,form_giver_id,org_maintain_id,update_src_id,is_deleted,del_date,znos,znos_date,date_expert,reestr_no,fair_cost_date,otdel_gukv_id,date_bti,arch_id,
            arch_flag,date_cost_rinkova,memo,note,is_free_sqr,free_sqr_useful,free_sqr_condition_id,free_sqr_location,ownership_doc_type,ownership_doc_num,ownership_doc_date,balans_doc_type,
            balans_doc_num,balans_doc_date,vidch_doc_type,vidch_doc_num,vidch_doc_date,vidch_type_id,vidch_org_id,vidch_cost,sqr_engineering,obj_status_id,geodata_map_opoints,
            znizhino_flag,znizhino_percent,znizhino_stanom,znizhino_shkoda,znizhino_zvitakt,znizhino_primitka,
            
            isnull((select top 1 p.rent_period_id from reports1nf_arenda_payments p join reports1nf_arenda a on a.id = p.arenda_id where a.balans_id = @balansId and isnull(a.is_deleted, 0) = 0 order by p.modify_date desc), (select id from dict_rent_period per where per.is_active = 1)) as rent_period_id
            , (select report_id from reports1nf_balans where id = @balansId) as report_id

            ,@archive_create_date,@archive_create_user,@archive_link_code
        from balans
        where id = @balansId";

        using (SqlCommand cmdArchBalans = new SqlCommand(sql, connectionSql, transactionSql))
        {
            cmdArchBalans.Parameters.AddWithValue("archive_create_date", DateTime.Now.Date);
            cmdArchBalans.Parameters.AddWithValue("archive_create_user", user);
            cmdArchBalans.Parameters.AddWithValue("archive_link_code", new_archive_link_code);
            cmdArchBalans.Parameters.AddWithValue("balansId", balansId);

            cmdArchBalans.ExecuteNonQuery();
        }

        sql = @"insert into arch_balans_docs (balans_id,building_id,link_kind,building_docs_id,sort_field,archive_balans_link_code)
                select balans_id,building_id,link_kind,building_docs_id,sort_field,@archive_balans_link_code
                from balans_docs
                where balans_id = @balansId";

        using (SqlCommand cmdArchBalansDoc = new SqlCommand(sql, connectionSql, transactionSql))
        {
            cmdArchBalansDoc.Parameters.AddWithValue("balansId", balansId);
            cmdArchBalansDoc.Parameters.AddWithValue("archive_balans_link_code", new_archive_link_code);

            cmdArchBalansDoc.ExecuteNonQuery();
        }


        // Free Square
        sql = @"insert into arch_balans_free_square ([id],[balans_id],[total_free_sqr],[free_sqr_korysna],[free_sqr_condition_id],[floor],[possible_using],[water],[heating],[power],[gas],[note],[modify_date],[modified_by],[original_id],[archive_balans_link_code],is_solution,[initiator],zgoda_control_id,zgoda_renter_id)
                select [id],[balans_id],[total_free_sqr],[free_sqr_korysna],[free_sqr_condition_id],[floor],[possible_using],[water],[heating],[power],[gas],[note],[modify_date],[modified_by],[original_id],
                @archive_balans_link_code,is_solution,[initiator],zgoda_control_id,zgoda_renter_id
                from balans_free_square
                where balans_id = @balansId";

        using (SqlCommand cmdArchBalansFreeSquare = new SqlCommand(sql, connectionSql, transactionSql))
        {
            cmdArchBalansFreeSquare.Parameters.AddWithValue("balansId", balansId);
            cmdArchBalansFreeSquare.Parameters.AddWithValue("archive_balans_link_code", new_archive_link_code);
            cmdArchBalansFreeSquare.ExecuteNonQuery();
        }

        string photoRootPath = LLLLhotorowUtils.ImgFreeSquareRootFolder;
        string photo1NFPath = Path.Combine(photoRootPath, "1NF");
        string photoBalansPath = Path.Combine(photoRootPath, "Balans");
        
        sql = @"SELECT [id],[free_square_id],[file_name],[file_ext],[modify_date],[modified_by],[original_id]
                FROM [balans_free_square_photos]
                WHERE [free_square_id] in (select id from balans_free_square where balans_id = @balans_id)";
        using (SqlCommand cmdArchBalansFreeSquarePhotoFiles = new SqlCommand(sql, connectionSql, transactionSql))
        {
            cmdArchBalansFreeSquarePhotoFiles.Parameters.AddWithValue("balans_id", balansId);
            using (SqlDataReader r = cmdArchBalansFreeSquarePhotoFiles.ExecuteReader())
            {
                while (r.Read())
                {
                    int id = r.IsDBNull(0) ? -1 : r.GetInt32(0);
                    int free_square_id = r.IsDBNull(1) ? -1 : r.GetInt32(1);
                    string file_name = r.IsDBNull(2) ? "" : r.GetString(2);
                    string file_ext = r.IsDBNull(3) ? "" : r.GetString(3);

                    string photoArchBalansPath = Path.Combine(photoRootPath, "Arch", free_square_id.ToString());
                    string sourcePath = Path.Combine(photoBalansPath, free_square_id.ToString(), file_name + file_ext);
                    string destPath = Path.Combine(photoArchBalansPath, file_name + file_ext);
                    LLLLhotorowUtils.Delete(destPath, connectionSql, transactionSql);
                    LLLLhotorowUtils.Copy(sourcePath, destPath, connectionSql, transactionSql);
                }
                r.Close();
            }
        }

        sql = @"insert into [arch_balans_free_square_photos] ([id],[free_square_id],[file_name],[file_ext],[modify_date],[modified_by],[original_id],[archive_balans_link_code])
            select [id],[free_square_id],[file_name],[file_ext],[modify_date],[modified_by],[original_id],@archive_balans_link_code from [balans_free_square_photos] where [free_square_id] in (select id from balans_free_square where balans_id = @balans_id)";
        using (SqlCommand cmdArchBalansFreeSquarePhotos = new SqlCommand(sql, connectionSql, transactionSql))
        {
            cmdArchBalansFreeSquarePhotos.Parameters.AddWithValue("balans_id", balansId);
            cmdArchBalansFreeSquarePhotos.Parameters.AddWithValue("archive_balans_link_code", new_archive_link_code);
            cmdArchBalansFreeSquarePhotos.ExecuteNonQuery();
        }


        // Object Photos
        string balansPhotoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
        string balansPhoto1NFPath = Path.Combine(balansPhotoRootPath, "1NF");
        string balansPhotoBalansPath = Path.Combine(balansPhotoRootPath, "Balans");
        string balansPhotoArchPath = Path.Combine(balansPhotoRootPath, "Arch");

        sql = "select [id],[bal_id],[file_name],[file_ext],[user_id],[create_date] from [balans_photos] where [bal_id] = @balans_id";
        using (SqlCommand cmdPhotoSelect = new SqlCommand(sql, connectionSql, transactionSql))
        {
            cmdPhotoSelect.Parameters.AddWithValue("balans_id", balansId);
            using (SqlDataReader r = cmdPhotoSelect.ExecuteReader())
            {
                while (r.Read())
                {
                    int id = (int)r.GetValue(0);
                    string file_name = r.IsDBNull(2) ? "" : r.GetString(2);
                    string file_ext = r.IsDBNull(3) ? "" : r.GetString(3);
                    string sourceFile = Path.Combine(balansPhotoBalansPath, balansId.ToString(), id.ToString() + file_ext);
                    string destPath = Path.Combine(balansPhotoArchPath, balansId.ToString(), new_archive_link_code.ToString());
                    string destFile = Path.Combine(destPath, id.ToString() + file_ext);
                    LLLLhotorowUtils.Delete(destFile, connectionSql, transactionSql);
                    LLLLhotorowUtils.Copy(sourceFile, destFile, connectionSql, transactionSql);
                }
                r.Close();
            }
        }

        sql = @"insert into [arch_balans_photos] ([id],[bal_id],[file_name],[file_ext],[user_id],[create_date],[archive_balans_link_code])
            select [id],[bal_id],[file_name],[file_ext],[user_id],[create_date],@archive_balans_link_code from [balans_photos] where [bal_id] = @balans_id";
        using (SqlCommand cmdArchBalansPhotos = new SqlCommand(sql, connectionSql, transactionSql))
        {
            cmdArchBalansPhotos.Parameters.AddWithValue("balans_id", balansId);
            cmdArchBalansPhotos.Parameters.AddWithValue("archive_balans_link_code", new_archive_link_code);
            cmdArchBalansPhotos.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionSql"></param>
    /// <param name="objectId"></param>
    /// <param name="transactionSql"></param>
    /// <returns></returns>
    public static void CreateObjectArchiveRecord(SqlConnection connectionSql, int objectId, string user, SqlTransaction transactionSql)
    {
        // Format the SQL statement
        string sql = @"insert INTO arch_buildings (
                id,master_building_id,addr_street_name,addr_street_id,addr_street_name2,addr_street_id2,street_full_name,addr_distr_old_id,addr_distr_new_id,
                addr_nomer1,addr_nomer2,addr_nomer3,addr_nomer,addr_misc,addr_korpus_flag,addr_korpus,addr_zip_code,addr_address,tech_condition_id,date_begin,date_end,num_floors,construct_year,
                condition_year,is_condition_valid,bti_code,history_id,object_type_id,object_kind_id,is_land,modify_date,modified_by,kadastr_code,cost_balans,sqr_total,sqr_pidval,sqr_mk,sqr_dk,
                sqr_rk,sqr_other,sqr_rented,sqr_zagal,sqr_for_rent,sqr_habit,sqr_non_habit,additional_info,is_deleted,del_date,oatuu_id,updpr,arch_id,arch_flag,facade_id,nomer_int,characteristics,
                expl_enter_year,is_basement_exists,is_loft_exists,sqr_loft,
                archive_create_date,archive_create_user)
            SELECT
                id,master_building_id,addr_street_name,addr_street_id,addr_street_name2,addr_street_id2,street_full_name,addr_distr_old_id,addr_distr_new_id,
                addr_nomer1,addr_nomer2,addr_nomer3,addr_nomer,addr_misc,addr_korpus_flag,addr_korpus,addr_zip_code,addr_address,tech_condition_id,date_begin,date_end,num_floors,construct_year,
                condition_year,is_condition_valid,bti_code,history_id,object_type_id,object_kind_id,is_land,modify_date,modified_by,kadastr_code,cost_balans,sqr_total,sqr_pidval,sqr_mk,sqr_dk,
                sqr_rk,sqr_other,sqr_rented,sqr_zagal,sqr_for_rent,sqr_habit,sqr_non_habit,additional_info,is_deleted,del_date,oatuu_id,updpr,arch_id,arch_flag,facade_id,nomer_int,characteristics,
                expl_enter_year,is_basement_exists,is_loft_exists,sqr_loft,
                @archive_create_date,@archive_create_user
            FROM buildings
            WHERE id = @objid";

        using (SqlCommand cmd = new SqlCommand(sql, connectionSql, transactionSql))
        {
            cmd.Parameters.AddWithValue("archive_create_date", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("archive_create_user", user);
            cmd.Parameters.AddWithValue("objid", objectId);

            cmd.ExecuteNonQuery();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionSql"></param>
    /// <param name="organizationId"></param>
    /// <param name="transactionSql"></param>
    /// <returns></returns>
    public static void CreateOrganizationArchiveRecord(SqlConnection connectionSql, int organizationId, string user, SqlTransaction transactionSql)
    {
        // Format the SQL statement
        string sql = @"insert INTO arch_organizations (
                id,master_org_id,last_state,occupation_id,status_id,form_gosp_id,form_ownership_id,gosp_struct_id,organ_id,industry_id,nomer_obj,zkpo_code,addr_distr_old_id,addr_distr_new_id,
                addr_street_name,addr_street_id,addr_nomer,addr_nomer2,addr_korpus,addr_zip_code,addr_misc,director_fio,director_phone,director_fio_kogo,prozoro_title,director_title,director_title_kogo,
                director_doc,director_doc_kogo,director_email,buhgalter_fio,buhgalter_phone,buhgalter_email,num_buildings,full_name,short_name,priznak_id,title_form_id,form_1nf_id,vedomstvo_id,
                title_id,form_id,gosp_struct_type_id,search_name,name_komu,fax,registration_auth,registration_num,registration_date,registration_svidot,l_year,date_l_year,sqr_on_balance,
                sqr_manufact,sqr_non_manufact,sqr_free_for_rent,sqr_total,sqr_rented,sqr_privat,sqr_given_for_rent,sqr_znyata_z_balansu,sqr_prodaj,sqr_spisani_zneseni,sqr_peredana,num_objects,
                kved_code,date_stat_spravka,koatuu,modified_by,modify_date,share_type_id,share,bank_name,bank_mfo,is_deleted,del_date,otdel_gukv_id,arch_id,arch_flag,is_liquidated,liquidation_date,
                pidp_rda,is_arend,beg_state_date,end_state_date,mayno_id,contact_email,contact_posada_id,nadhodjennya_id,vibuttya_id,privat_status_id,cur_state_id,sfera_upr_id,plan_zone_id,
                registr_org_id,nadhodjennya_date,vibuttya_date,chastka,registration_rish,registration_dov_date,registration_corp,strok_start_date,strok_end_date,stat_fond,size_plus,addr_zip_code_3,
                old_industry_id,old_occupation_id,old_organ_id,form_vlasn_vibuttya_id,addr_city,addr_flat_num,povnovajennia,povnov_osoba_fio,povnov_passp_seria,povnov_passp_num,povnov_passp_auth,
                povnov_passp_date,director_passp_seria,director_passp_num,director_passp_auth,director_passp_date,registration_svid_date,origin_db,budg_payments_rate,is_under_closing,
                phys_addr_street_id,phys_addr_district_id,phys_addr_nomer,phys_addr_zip_code,phys_addr_misc,contribution_rate,budget_narah_50_uah,budget_zvit_50_uah,budget_prev_50_uah,
                budget_debt_30_50_uah,is_special_organization,payment_budget_special,konkurs_payments,unknown_payments,unknown_payment_note,
                archive_create_date,archive_create_user)
            SELECT
                id,master_org_id,last_state,occupation_id,status_id,form_gosp_id,form_ownership_id,gosp_struct_id,organ_id,industry_id,nomer_obj,zkpo_code,addr_distr_old_id,addr_distr_new_id,
                addr_street_name,addr_street_id,addr_nomer,addr_nomer2,addr_korpus,addr_zip_code,addr_misc,director_fio,director_phone,director_fio_kogo,prozoro_title,director_title,director_title_kogo,
                director_doc,director_doc_kogo,director_email,buhgalter_fio,buhgalter_phone,buhgalter_email,num_buildings,full_name,short_name,priznak_id,title_form_id,form_1nf_id,vedomstvo_id,
                title_id,form_id,gosp_struct_type_id,search_name,name_komu,fax,registration_auth,registration_num,registration_date,registration_svidot,l_year,date_l_year,sqr_on_balance,
                sqr_manufact,sqr_non_manufact,sqr_free_for_rent,sqr_total,sqr_rented,sqr_privat,sqr_given_for_rent,sqr_znyata_z_balansu,sqr_prodaj,sqr_spisani_zneseni,sqr_peredana,num_objects,
                kved_code,date_stat_spravka,koatuu,modified_by,modify_date,share_type_id,share,bank_name,bank_mfo,is_deleted,del_date,otdel_gukv_id,arch_id,arch_flag,is_liquidated,liquidation_date,
                pidp_rda,is_arend,beg_state_date,end_state_date,mayno_id,contact_email,contact_posada_id,nadhodjennya_id,vibuttya_id,privat_status_id,cur_state_id,sfera_upr_id,plan_zone_id,
                registr_org_id,nadhodjennya_date,vibuttya_date,chastka,registration_rish,registration_dov_date,registration_corp,strok_start_date,strok_end_date,stat_fond,size_plus,addr_zip_code_3,
                old_industry_id,old_occupation_id,old_organ_id,form_vlasn_vibuttya_id,addr_city,addr_flat_num,povnovajennia,povnov_osoba_fio,povnov_passp_seria,povnov_passp_num,povnov_passp_auth,
                povnov_passp_date,director_passp_seria,director_passp_num,director_passp_auth,director_passp_date,registration_svid_date,origin_db,budg_payments_rate,is_under_closing,
                phys_addr_street_id,phys_addr_district_id,phys_addr_nomer,phys_addr_zip_code,phys_addr_misc,contribution_rate,budget_narah_50_uah,budget_zvit_50_uah,budget_prev_50_uah,
                budget_debt_30_50_uah,is_special_organization,payment_budget_special,konkurs_payments,unknown_payments,unknown_payment_note,
                @archive_create_date,@archive_create_user
            FROM organizations
            WHERE id = @objid";
        using (SqlCommand cmd = new SqlCommand(sql, connectionSql, transactionSql))
        {
            cmd.Parameters.AddWithValue("archive_create_date", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("archive_create_user", user);
            cmd.Parameters.AddWithValue("objid", organizationId);

            cmd.ExecuteNonQuery();
        }


    }


}