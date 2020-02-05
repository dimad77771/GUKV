using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using GUKV.Conveyancing;
using System.Security.Principal;
using System.Data;

/// <summary>
/// Summary description for RishProjectTableProcessor
/// </summary>
public static class RishProjectTableProcessor
{

    public static void ProcessCommObjects(ObjectsData data, DataSet dataSet, List<DataError> errors, IPrincipal User, bool onlyCheck)
    {
        using (SqlConnection conn = Utils.ConnectToDatabase())
        {
            if (conn == null)
                return;

            GUKV.ImportToolUtils.ObjectFinder buildingFinder = GUKV.ImportToolUtils.ObjectFinder.Instance;
            //GUKV.ImportToolUtils.ObjectFinder buildingFinder = new GUKV.ImportToolUtils.ObjectFinder();
            //buildingFinder.BuildObjectCacheFromSqlServer(conn);

            BalansFinder balansFinder = new BalansFinder(conn);

            using (SqlTransaction tran = conn.BeginTransaction())
            {
                int actDocumentID = onlyCheck ? -1 : BalansTransferUtils.CreateAktNew(conn, tran, 0, 0, data.ActDate, data.ActNumber, 0, 0, data.ProjectDocumentID);

                DB dataset = new DB();

                tran.NewAdapter<DBTableAdapters.bp_rish_project_tableTableAdapter>()
                    .FillByProjectID(dataset.bp_rish_project_table, data.ProjectID);

                Dictionary<string, int> lookupDistricts = tran
                    .NewAdapter<DBTableAdapters.dict_districts2TableAdapter>()
                    .GetData()
                    .Where(x => !x.IsnameNull())
                    .GroupBy(x => x.name.Trim().ToUpper())
                    .ToDictionary(x => x.Key, x => x.First().id);

                Dictionary<string, int> lookupObjKinds = tran
                    .NewAdapter<DBTableAdapters.dict_object_kindTableAdapter>()
                    .GetData()
                    .Where(x => !x.IsnameNull())
                    .GroupBy(x => x.name.Trim().ToUpper())
                    .ToDictionary(x => x.Key, x => x.First().id);

                Dictionary<string, int> lookupObjTypes = tran
                    .NewAdapter<DBTableAdapters.dict_object_typeTableAdapter>()
                    .GetData()
                    .Where(x => !x.IsnameNull())
                    .GroupBy(x => x.name.Trim().ToUpper())
                    .ToDictionary(x => x.Key, x => x.First().id);


                DataRow[] rowsToProcess = (data.SelectedTableRowIDs == null || data.SelectedTableRowIDs.Count() == 0) ?
                    (dataSet == null ? new DataRow[0] : dataSet.Tables[0].Select()) : 
                    (dataset.bp_rish_project_table.Where(x => data.SelectedTableRowIDs.Contains(x.id)).ToArray<DataRow>());

                //foreach (DB.bp_rish_project_tableRow entry in rowsToProcess)
                foreach (DataRow entry in rowsToProcess)
                {
                    int entryid = Convert.ToInt32(entry["id"]);

                    //if (entry.Issqr_totalNull())
                    if (entry.IsNull("sqr_total"))
                    {
                        errors.Add(new DataError()
                        {
                            RowID = entryid,
                            FieldName = "sqr_total",
                            What = "Відсутня інформація щодо площі об'єкту"
                        });
                        continue;
                    }
                    if (entry.IsNull("addr_street_name"))
                    {
                        errors.Add(new DataError()
                        {
                            RowID = entryid,
                            FieldName = "addr_street_name",
                            What = "Відсутня інформація щодо вулиці об'єкту"
                        });
                        continue;
                    }

                    /*
                     * Conveyancing types:
                     * 
                     * case "1": // Прийняти об'єкт на баланс
                     * case "2": // Передати об'єкт з балансу
                     * case "3": // Списати об'єкту з балансу шляхом зносу
                     * case "4": // Списати об'єкту з балансу шляхом приватизації
                     * case "5": // Постановка  новозбудованого об'єкту на баланс
                     */

                    List<int> building_id_list;
                    //int building_id;
                    BalansEntry balans_id;

                    // Note that we ignore the return type until we get into particulars
                    // of processing a specific conveyancing type.
                    FindObject(buildingFinder, balansFinder, lookupDistricts, entry,
                        data.RightRequiresOrgFromID ? data.OrgFromID : data.OrgToID,
                        out building_id_list, out balans_id);

                    if (data.RightRequiresOrgFromID && data.RightRequiresOrgToID)
                    {
                        // Conveyancing type: 1 or 2 (the exact type is irrelevant)

                        // We MUST have the balans object to transfer
                        if (building_id_list.Count == 0)
                        {
                            errors.Add(new DataError()
                            {
                                RowID = entryid,
                                What = "Будинок за вказаною адресою не знайдено",
                            });
                            continue;
                        }
                        //if (building_id_list.Count > 1)
                        //{
                        //    errors.Add(new DataError()
                        //    {
                        //        RowID = entryid,
                        //        What = "Знайдено декілька будинків за вказаною адресою.",
                        //    });
                        //    continue;
                        //}

                        if (balans_id == null)
                        {
                            errors.Add(new DataError()
                            {
                                RowID = entryid,
                                What = "Об'єкт на балансі не знайден",
                            });
                            continue;
                        }

                        //building_id = building_id_list[0];

                        if (!onlyCheck)
                        {
                            var balansTransfer = new BalansTransfer();
                            balansTransfer.transferType = ObjectTransferType.Transfer;
                            balansTransfer.objectId = balans_id.BuildingID;
                            balansTransfer.sqr = entry.IsNull("sqr_total") ? 0 : (decimal)entry["sqr_total"];
                            balansTransfer.organizationFromId = data.OrgFromID;
                            balansTransfer.organizationToId = data.OrgToID;
                            balansTransfer.balansId = balans_id.BalansID;

                            var actObject = new ActObject();
                            actObject.makeChangesIn1NF = true;
                            actObject.objectId = balans_id.BuildingID; //22930; // FAKE!!!
                            actObject.balansTransfers.Add(balansTransfer);

                            var importedAct = new ImportedAct();
                            importedAct.actObjects.Add(actObject);
                            //importedAct.docTitle = "АКТ ПРИЙМАННЯ-ПЕРЕДАЧІ";
                            importedAct.docNum = data.ProjectDocumentNumber; //data.ActNumber;
                            importedAct.docDate = data.ProjectDocumentDate; //data.ActDate;
                            importedAct.docSum = entry.IsNull("initial_cost") ? 0 : (decimal)entry["initial_cost"];
                            importedAct.docFinalSum = entry.IsNull("remaining_cost") ? 0 : (decimal)entry["remaining_cost"];    

                            var rish = new Document();
                            rish.documentKind = 3;
                            rish.ownership_doc_type_id = data.OwnershipDocType;
                            //rish.documentDate = DateTime.Now;
                            rish.documentNumber = data.ActNumber; //data.ProjectDocumentNumber;
                            rish.documentDate = data.ActDate; //data.ProjectDocumentDate;
                            rish.modify_by = User.Identity.Name;
                            rish.ownership_type_id = data.BalansOwnershipTypeID;

                            if (GUKV.Conveyancing.DB.TransferBalansObjects(conn, tran, importedAct, rish, false, 3))
                            {
                                foreach (ActObject act in importedAct.actObjects)
                                {
                                    foreach (BalansTransfer bt in act.balansTransfers)
                                    {
                                        BalansTransferUtils.AddObjectToAkt(conn, tran, balans_id.BalansID, balans_id.BuildingID,
                                            data.ProjectDocumentID, actDocumentID);
                                    }
                                }
                            }
                        }
                    }
                    else if (data.RightRequiresOrgFromID)
                    {
                        // Conveyancing type: 3 or 4 (must determine the exact type)

                        // We MUST have the balans object to transfer
                        if (building_id_list.Count == 0)
                        {
                            errors.Add(new DataError()
                            {
                                RowID = entryid,
                                What = "Будинок за вказаною адресою не знайдено",
                            });
                            continue;
                        }
                        //if (building_id_list.Count > 1)
                        //{
                        //    errors.Add(new DataError()
                        //    {
                        //        RowID = entryid,
                        //        What = "Знайдено декілька будинків за вказаною адресою.",
                        //    });
                        //    continue;
                        //}
                        if (balans_id == null)
                        {
                            errors.Add(new DataError()
                            {
                                RowID = entryid,
                                What = "Об'єкт на балансі не знайден",
                            });
                            continue;
                        }



                        if (!onlyCheck)
                        {
                            int conveyancingType = (data.RightID == 11 ? 3 : 4);

                            var balansTransfer = new BalansTransfer();
                            balansTransfer.transferType = ObjectTransferType.Destroy;
                            balansTransfer.objectId = balans_id.BuildingID;
                            balansTransfer.sqr = entry.IsNull("sqr_total") ? 0 : (decimal)entry["sqr_total"];
                            balansTransfer.organizationFromId = data.OrgFromID;
                            balansTransfer.balansId = balans_id.BalansID;

                            var actObject = new ActObject();
                            actObject.makeChangesIn1NF = true;
                            actObject.objectId = balans_id.BuildingID;// 22930; // FAKE!!!
                            actObject.balansTransfers.Add(balansTransfer);

                            var importedAct = new ImportedAct();
                            importedAct.actObjects.Add(actObject);
                            //importedAct.docTitle = "АКТ ПРИЙМАННЯ-ПЕРЕДАЧІ";
                            importedAct.docNum = data.ActNumber;
                            importedAct.docDate = data.ActDate;
                            importedAct.docSum = entry.IsNull("initial_cost") ? 0 : (decimal)entry["initial_cost"];
                            importedAct.docFinalSum = entry.IsNull("remaining_cost") ? 0 : (decimal)entry["remaining_cost"];    

                            var rish = new Document();
                            rish.documentKind = 3;
                            
                            rish.documentNumber = data.ProjectDocumentNumber;
                            rish.documentDate = data.ProjectDocumentDate;
                            rish.modify_by = User.Identity.Name;
                            rish.ownership_type_id = data.BalansOwnershipTypeID;

                            int vidch_type_id = 0;
                            if (conveyancingType == 3)
                                vidch_type_id = 1;
                            else if (conveyancingType == 4)
                                vidch_type_id = 2;

                            if (GUKV.Conveyancing.DB.TransferBalansObjects(conn, tran, importedAct, rish, false, vidch_type_id))
                            {
                                foreach (ActObject act in importedAct.actObjects)
                                {
                                    foreach (BalansTransfer bt in act.balansTransfers)
                                    {
                                        BalansTransferUtils.AddObjectToAkt(conn, tran, balans_id.BalansID, balans_id.BuildingID, data.ProjectDocumentID, actDocumentID);
                                    }
                                }
                            }

                            if (conveyancingType == 3)
                                BalansTransferUtils.ModifyTechCondition(conn, tran, balans_id.BalansID, 3); // make this balans object 'ЗНЕСЕНИЙ'
                        }
                    }
                    else // Assuming data.RightRequiresOrgToID
                    {
                        // Conveyancing type: 5

                        // We CAN'T expect to have a readily available object
                        // (although if it exists, it is certainly a bad sign
                        // as we might be dealing with a duplicate).

                        // Conversely, we MUST reuse the building_id, if such
                        // a reference was found.
                        int new_building_id = -1;

                        if (building_id_list.Count == 0)
                        {
                            // In this rendition of this functionality, we shall refrain
                            // from creating any dictionary entries. Instead, successful
                            // resolution of dictionary references becomes a requirement.
                            int streetId;
                            if (!buildingFinder.FindStreetId((string)entry["addr_street_name"], out streetId, true))
                            {
                                errors.Add(new DataError()
                                {
                                    RowID = entryid,
                                    FieldName = "addr_street_name",
                                    What = "Немає вулиці в реєстрі",
                                });
                                continue;
                            }
                            if (entry.IsNull("addr_distr") || ((string)entry["addr_distr"]).Trim().Length == 0)
                            {
                                errors.Add(new DataError()
                                {
                                    RowID = entryid,
                                    FieldName = "addr_distr",
                                    What = "Район адреси об'єкта не визначено",
                                });
                                continue;
                            }
                            if (entry.IsNull("obj_kind") || ((string)entry["obj_kind"]).Trim().Length == 0)
                            {
                                errors.Add(new DataError()
                                {
                                    RowID = entryid,
                                    FieldName = "obj_kind",
                                    What = "Вид будинку об'єкта не визначено",
                                });
                                continue;
                            }
                            if (entry.IsNull("obj_type") || ((string)entry["obj_type"]).Trim().Length == 0)
                            {
                                errors.Add(new DataError()
                                {
                                    RowID = entryid,
                                    FieldName = "obj_type",
                                    What = "Тип будинку об'єкта не визначено",
                                });
                                continue;
                            }

                            int districtId;
                            if (!lookupDistricts.TryGetValue(((string)entry["addr_distr"]).Trim().ToUpper(), out districtId))
                            {
                                errors.Add(new DataError()
                                {
                                    RowID = entryid,
                                    FieldName = "addr_distr",
                                    What = "Немає району в реєстрі",
                                });
                                continue;
                            }
                            int objKindId;
                            if (!lookupObjKinds.TryGetValue(((string)entry["obj_kind"]).Trim().ToUpper(), out objKindId))
                            {
                                errors.Add(new DataError()
                                {
                                    RowID = entryid,
                                    FieldName = "obj_kind",
                                    What = "Немає виду будівлі об'єкта в реєстрі",
                                });
                                continue;
                            }
                            int objTypeId;
                            if (!lookupObjTypes.TryGetValue(((string)entry["obj_type"]).Trim().ToUpper(), out objTypeId))
                            {
                                errors.Add(new DataError()
                                {
                                    RowID = entryid,
                                    FieldName = "obj_type",
                                    What = "Немає типу будівлі об'єкта в реєстрі",
                                });
                                continue;
                            }

                            if (!onlyCheck)
                            {

                                string errorMessage;
                                new_building_id = ConveyancingUtils.CreateNew1NFBuilding(
                                    conn,
                                    tran,
                                    streetId,
                                    districtId,
                                    objKindId,
                                    objTypeId,
                                    (entry.IsNull("addr_nomer") ? string.Empty : (string)entry["addr_nomer"]),
                                    string.Empty,
                                    string.Empty,
                                    (entry.IsNull("addr_misc") ? string.Empty : (string)entry["addr_misc"]),
                                    out errorMessage
                                    );
                                if (new_building_id < 0)
                                {
                                    errors.Add(new DataError()
                                    {
                                        RowID = entryid,
                                        What = "Помилка створення запису будівлі (" + errorMessage + ")",
                                    });
                                    continue;
                                }
                            }
                        }
                        else
                            new_building_id = building_id_list[0];

                        if (!onlyCheck)
                        {
                            var balansTransfer = new BalansTransfer();
                            balansTransfer.transferType = ObjectTransferType.Create;
                            balansTransfer.objectId = new_building_id;
                            balansTransfer.sqr = entry.IsNull("sqr_total") ? 0 : (decimal)entry["sqr_total"];
                            balansTransfer.organizationToId = data.OrgToID;

                            var actObject = new ActObject();
                            actObject.makeChangesIn1NF = true;
                            actObject.objectId = new_building_id; // 22930; // FAKE!!!
                            actObject.balansTransfers.Add(balansTransfer);

                            var importedAct = new ImportedAct();
                            importedAct.actObjects.Add(actObject);
                            
                            //importedAct.docTitle = "АКТ ПРИЙМАННЯ-ПЕРЕДАЧІ";
                            importedAct.docNum = data.ProjectDocumentNumber; // data.ActNumber;
                            importedAct.docDate = data.ProjectDocumentDate; //data.ActDate;
                            importedAct.docSum = entry.IsNull("initial_cost") ? 0 : (decimal)entry["initial_cost"];
                            importedAct.docFinalSum = entry.IsNull("remaining_cost") ? 0 : (decimal)entry["remaining_cost"];    

                            var rish = new Document();
                            rish.documentKind = 3;
                            rish.ownership_doc_type_id = data.OwnershipDocType;
                            rish.documentNumber = data.ActNumber; //data.ProjectDocumentNumber;
                            rish.documentDate = data.ActDate; // data.ProjectDocumentDate;
                            rish.modify_by = User.Identity.Name;
                            rish.ownership_type_id = data.BalansOwnershipTypeID;

                            if (GUKV.Conveyancing.DB.TransferBalansObjects(conn, tran, importedAct, rish, false, 0))
                            {
                                foreach (ActObject act in importedAct.actObjects)
                                {
                                    foreach (BalansTransfer bt in act.balansTransfers)
                                    {
                                        BalansTransferUtils.AddObjectToAkt(conn, tran, -1, new_building_id,
                                            data.ProjectDocumentID, actDocumentID);
                                    }
                                }
                            }
                        }
                    }

                    if (!onlyCheck)
                        entry["is_acted_on"] = true;
                }

                if (!onlyCheck)
                {
                    tran.NewAdapter<DBTableAdapters.bp_rish_project_tableTableAdapter>().Update(dataset);

                    tran.Commit();
                }
            }
        }
    }


    public static bool ProcessCreateAct(List<DataError> Errors, int ProjectID, IPrincipal User, string actNumber, DateTime actDate, DataSet dataSet, int[] selectedTableRowIDs, int rightID, int orgFromID, int orgToID, int objectType, bool onlyCheck)
    {
        bool res = true;

        Errors.Clear();

        if ((ProjectID <= 0) && (!onlyCheck))
        {
            Errors.Add(new DataError() { What = "Неактуальні дані. Перезавантажте сторінку." });
            res = false;
        }

        // Round up items entered by the end user in this form

        if ((actNumber.Length == 0) && (!onlyCheck))
        {
            Errors.Add(new DataError() { What = "Вкажіть номер акту" });
            res = false;
        }


        if ((actDate == default(DateTime)) && (!onlyCheck))
        {
            Errors.Add(new DataError() { What = "Вкажіть дату акту" });
            res = false;
        }


        if ((selectedTableRowIDs.Length == 0) && (!onlyCheck))
        {
            Errors.Add(new DataError() { What = "Оберіть об'єкти що належать до акту" });
            res = false;
        }

        // Determine the rest of the parameters by interrogating the bp_rish_project(*)
        // elements.
        DB dataset = new DB();


        if (rightID <= 0)
        {
            Errors.Add(new DataError() { What = "Відсутнє право у додатку до розпорядчого документу" });
            res = false;
        }

        (new DBTableAdapters.dict_obj_rightsTableAdapter()).FillByID(dataset.dict_obj_rights, rightID);
        if (dataset.dict_obj_rights.Count != 1)
        {
            Errors.Add(new DataError() { What = "Невідомий код права у додатку до розпорядчого документу: " + rightID });
            res = false;
        }
        bool requireOrgFromID = !dataset.dict_obj_rights[0].Isenable_org_fromNull() && dataset.dict_obj_rights[0].enable_org_from;
        bool requireOrgToID = !dataset.dict_obj_rights[0].Isenable_org_toNull() && dataset.dict_obj_rights[0].enable_org_to;

        int balans_ownership_type = dataset.dict_obj_rights[0].balans_ownership_type_mapping_id;

        if (requireOrgFromID && orgFromID <= 0)
        {
            Errors.Add(new DataError() { What = "Обране право потребує наявності організацій від якої передається власність" });
            res = false;
        }
        if (requireOrgToID && orgToID <= 0)
        {
            Errors.Add(new DataError() { What = "Обране право потребує наявності організації до якої передається власність" });
            res = false;
        }

        (new DBTableAdapters.bp_rish_project_infoTableAdapter())
            .FillByProjectID(dataset.bp_rish_project_info, ProjectID);
        if (dataset.bp_rish_project_info.Count != 1)
        {
            Errors.Add(new DataError() { What = "Помилка даних (bp_rish_project_info)" });
            res = false;
        }
        if (dataset.bp_rish_project_info[0].Isdocument_idNull() && (!onlyCheck))
        {
            Errors.Add(new DataError() { What = "Помилка даних (document_id)" });
            res = false;
        }
        if (dataset.bp_rish_project_info[0].Isdocument_numNull() && (!onlyCheck))
        {
            Errors.Add(new DataError() { What = "Помилка даних (document_num)" });
            res = false;
        }
        if (dataset.bp_rish_project_info[0].Isdocument_dateNull() && (!onlyCheck))
        {
            Errors.Add(new DataError() { What = "Помилка даних (document_date)" });
            res = false;
        }

        (new DBTableAdapters.dict_rish_project_typeTableAdapter()).FillByID(dataset.dict_rish_project_type, dataset.bp_rish_project_info[0].project_type_id);

        ObjectsData data = new ObjectsData()
        {
            ProjectID = ProjectID,
            ActNumber = actNumber,
            ActDate = actDate,
            SelectedTableRowIDs = selectedTableRowIDs,
            BalansOwnershipTypeID = balans_ownership_type, //rightID,
            RightID = rightID,
            RightRequiresOrgFromID = requireOrgFromID,
            RightRequiresOrgToID = requireOrgToID,
            OrgFromID = orgFromID,
            OrgToID = orgToID,
            ObjectType = objectType,
            ProjectDocumentID = onlyCheck ? -1 : dataset.bp_rish_project_info[0].document_id,
            ProjectDocumentNumber = onlyCheck ? string.Empty : dataset.bp_rish_project_info[0].document_num,
            ProjectDocumentDate = onlyCheck ? DateTime.MinValue : dataset.bp_rish_project_info[0].document_date,
            OwnershipDocType = onlyCheck ? -1 : dataset.dict_rish_project_type[0].doc_kind_id
        };

        switch (objectType)
        {
            case 0: // Об’єкти нерухомості нежитлового фонду
                ProcessCommObjects(data, dataSet, Errors, User, onlyCheck);
                break;
            case 1: // Інші об’єкти комунальної власності
                ProcessOtherObjects(data, onlyCheck);
                break;
        }

        if ((!onlyCheck) && (Errors.Count == 0))
        {
            // If the appendix is fully executed, let's assign it a proper status
            using (SqlConnection conn = Utils.ConnectToDatabase())
            {
                bool fullyExecuted;

                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 600;
                    command.CommandText =
                        @"select count(*)
                        from bp_rish_project_item
                        inner join bp_rish_project_item parent_item on parent_item.id = bp_rish_project_item.parent_item_id
                        where bp_rish_project_item.project_id = @projectID
	                        and bp_rish_project_item.is_table = 1
	                        and exists (select * from bp_rish_project_table 
		                        where bp_rish_project_table.bp_rish_project_item_id = bp_rish_project_item.id
			                        and isnull(is_acted_on, 0) = 0)";

                    command.Parameters.AddWithValue("@projectID", ProjectID);

                    fullyExecuted = ((int)command.ExecuteScalar() == 0);
                }

                if (fullyExecuted)
                {
                    DB.dict_rish_project_stateRow executedState =
                        (new DBTableAdapters.dict_rish_project_stateTableAdapter()).GetData()
                            .FirstOrDefault(x => ((RishProjectState)x.flags & RishProjectState.Final) == RishProjectState.Final);
                    if (executedState != null)
                    {
                        (new DBTableAdapters.bp_rish_project_stateTableAdapter()).Insert(
                            ProjectID,
                            executedState.id,
                            DateTime.Now,
                            (Guid)System.Web.Security.Membership.GetUser().ProviderUserKey,
                            null,
                            null,
                            null,
                            null
                            );
                    }
                }
            }

            return true;
        }

        Errors.Add(new DataError()
        {
            What = string.Format(
                "{0} запис(ів) оброблено з помилками",
                Errors.Where(x => x.RowID != 0).Select(x => x.RowID).Distinct().Count()
                )
        });
        return false;
    }

    public static void ProcessOtherObjects(ObjectsData data, bool onlyCheck)
    {
        using (SqlConnection conn = Utils.ConnectToDatabase())
        {
            if (conn == null)
                return;

            using (SqlTransaction tran = conn.BeginTransaction())
            {
                int actDocumentID = onlyCheck ? -1 : BalansTransferUtils.CreateAktNew(conn, tran, 0, 0, data.ActDate, data.ActNumber, 0, 0, data.ProjectDocumentID);

                DB dataset = new DB();

                tran.NewAdapter<DBTableAdapters.bp_rish_project_tableTableAdapter>()
                    .FillByProjectID(dataset.bp_rish_project_table, data.ProjectID);

                string[] allInvNumbers = dataset.bp_rish_project_table
                    .Where(x => data.SelectedTableRowIDs.Contains(x.id))
                    .Where(x => !x.Isinv_numberNull())
                    .Select(x => x.inv_number)
                    .Distinct()
                    .ToArray();
                if (allInvNumbers.Length > 0)
                {
                    if (data.RightRequiresOrgToID)
                    {
                        tran.NewAdapter<DBTableAdapters.balans_otherTableAdapter>()
                            .FillByOrgIDandInvNumbers(dataset.balans_other, data.OrgToID, string.Join(",", allInvNumbers));
                    }
                    if (data.RightRequiresOrgFromID)
                    {
                        if (!data.RightRequiresOrgToID || data.OrgFromID != data.OrgToID)
                        {
                            tran.NewAdapter<DBTableAdapters.balans_otherTableAdapter>(a => a.ClearBeforeFill = false)
                                .FillByOrgIDandInvNumbers(dataset.balans_other, data.OrgFromID, string.Join(",", allInvNumbers));
                        }
                    }
                }

                foreach (var entry in
                    dataset.bp_rish_project_table.Where(x => data.SelectedTableRowIDs.Contains(x.id)))
                {
                    DB.balans_otherRow balansOtherRow = null;

                    if (!entry.Isinv_numberNull())
                    {
                        string invNumber = entry.inv_number;

                        if (data.RightRequiresOrgFromID)
                        {
                            // Attempt to identify the object using orgFromID and invNumber
                            balansOtherRow = dataset.balans_other.FirstOrDefault(
                                x => x.org_id == data.OrgFromID
                                    && !x.Isinv_numberNull()
                                    && x.inv_number.Equals(invNumber, StringComparison.OrdinalIgnoreCase)
                                );
                        }
                        if (balansOtherRow == null && data.RightRequiresOrgToID)
                        {
                            // Attempt to identify the object using orgToID and invNumber
                            balansOtherRow = dataset.balans_other.FirstOrDefault(
                                x => x.org_id == data.OrgToID
                                    && !x.Isinv_numberNull()
                                    && x.inv_number.Equals(invNumber, StringComparison.OrdinalIgnoreCase)
                                );
                        }

                        if (balansOtherRow != null)
                        {
                            // An object exists, we should examine its fields and update accordingly
                            if (data.RightRequiresOrgToID)
                            {
                                // The object appears to be assigned to OrgToID
                                balansOtherRow.org_id = data.OrgToID;
                            }
                            else // presumably, data.RightRequiresOrgFromID
                            {
                                // The object appears to be decommissioned
                                balansOtherRow.org_id = data.OrgFromID;
                                balansOtherRow.decommissioned_date = data.ActDate;
                            }
                            balansOtherRow.document_date = data.ProjectDocumentDate;
                            balansOtherRow.document_number = data.ProjectDocumentNumber;
                        }
                    }

                    if (balansOtherRow == null)
                    {
                        balansOtherRow = dataset.balans_other.Addbalans_otherRow(
                            data.GetSignificantOrgID(),
                            entry.IsnameNull() ? null : entry.name,
                            entry.IsaddressNull() ? null : entry.address,
                            entry.Isinv_numberNull() ? null : entry.inv_number,
                            entry.Isinitial_costNull() ? 0 : entry.initial_cost,
                            entry.Isremaining_costNull() ? 0 : entry.remaining_cost,
                            entry.IslocationNull() ? null : entry.location,
                            entry.Iscommissioned_dateNull() ? DateTime.Now : entry.commissioned_date,
                            data.ActDate,
                            data.ProjectDocumentDate,
                            data.ActDate,
                            data.ProjectDocumentNumber
                            );
                        if (entry.Isinitial_costNull())
                            balansOtherRow.Setinitial_costNull();
                        if (entry.Isremaining_costNull())
                            balansOtherRow.Setremaining_costNull();
                        if (data.RightRequiresOrgToID)
                            balansOtherRow.Setdecommissioned_dateNull();
                        if (entry.Iscommissioned_dateNull())
                            balansOtherRow.Setcommissioned_dateNull();
                    }

                    // Associate the object with the Act
                    dataset.balans_other_docs.Addbalans_other_docsRow(balansOtherRow, actDocumentID);

                    if (!onlyCheck)
                        entry.is_acted_on = true;
                }

                if (!onlyCheck)
                {
                    tran.NewAdapter<DBTableAdapters.balans_otherTableAdapter>().Update(dataset);
                    tran.NewAdapter<DBTableAdapters.balans_other_docsTableAdapter>().Update(dataset);
                    tran.NewAdapter<DBTableAdapters.bp_rish_project_tableTableAdapter>().Update(dataset);

                    tran.Commit();
                }
            }
        }
    }




    public static bool FindObject(GUKV.ImportToolUtils.ObjectFinder buildingFinder, BalansFinder balansFinder,
        Dictionary<string, int> lookupDistricts, DataRow inputRow, int orgID,
        out List<int> building_id_list, out BalansEntry balans_id)
    {
        building_id_list = new List<int>();
        balans_id = null;

        int districtId = 0;
        if (!inputRow.IsNull("addr_distr"))
        {
            string district = ((string)inputRow["addr_distr"]).Trim().ToUpper();

            lookupDistricts.TryGetValue(district, out districtId);
        }

        bool isAddressSimple, areSimilarAddressesExist;

        // Find the matching building

        building_id_list = buildingFinder.FindObject(
            (inputRow.IsNull("addr_street_name") ? string.Empty : ((string)inputRow["addr_street_name"]).Trim().ToUpper()),
            string.Empty,
            (inputRow.IsNull("addr_nomer") ? string.Empty : ((string)inputRow["addr_nomer"]).Trim().ToUpper()),
            string.Empty,
            string.Empty,
            (inputRow.IsNull("addr_misc") ? string.Empty : ((string)inputRow["addr_misc"]).Trim().ToUpper()),
            (districtId <= 0 ? null : (object)districtId),
            (decimal)inputRow["sqr_total"],
            out isAddressSimple,
            out areSimilarAddressesExist
            );

        if (building_id_list.Count == 0)
            return false;

        balans_id = balansFinder.FindBalansObject(
            building_id_list,
            orgID,
            (decimal)inputRow["sqr_total"],
            (inputRow.IsNull("inv_number") ? null : (string)inputRow["inv_number"]));

        if (balans_id == null)
            return false;

        return true;
    }

}