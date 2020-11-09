using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web;
using log4net;
using System.Data.SqlClient;
using GUKV.Conveyancing;
using System.Data;
using DevExpress.Web;
using System.Security.Principal;

public partial class BPRishProject_NewAct : System.Web.UI.Page
{
    private static readonly ILog LOGGER = LogManager.GetLogger(typeof(BPRishProject_NewAct));

    protected void Page_Load(object sender, EventArgs e)
    {
        GetPageUniqueKey();

        string projectIdStr = Request.QueryString["projid"];

        if (projectIdStr != null && projectIdStr.Length > 0)
        {
            ProjectID = int.Parse(projectIdStr.Trim());
        }

        if (ProjectID > 0)
        {
            if (!IsPostBack && !IsCallback)
            {
                DB dataset = new DB();
                (new DBTableAdapters.bp_rish_projectTableAdapter())
                    .FillByID(dataset.bp_rish_project, ProjectID);
                (new DBTableAdapters.bp_rish_project_infoTableAdapter())
                    .FillByProjectID(dataset.bp_rish_project_info, ProjectID);
                (new DBTableAdapters.dict_rish_project_typeTableAdapter())
                    .Fill(dataset.dict_rish_project_type);

                if (dataset.bp_rish_project.Count > 0 && dataset.bp_rish_project_info.Count > 0)
                {
                    LabelRishNumber.Text = dataset.bp_rish_project_info[0].document_num;
                    LabelRishDate.Text = dataset.bp_rish_project_info[0].document_date.ToString("d");
                    LabelRishType.Text = dataset.dict_rish_project_type.FindByid(dataset.bp_rish_project_info[0].project_type_id).name;
                    MemoRishName.Text = dataset.bp_rish_project[0].name;
                }
            }
        }
    }

    #region Session management

    protected int ProjectID
    {
        set
        {
            Session[GetPageUniqueKey() + "_ProjectID"] = value;
        }

        get
        {
            object val = Session[GetPageUniqueKey() + "_ProjectID"];

            if (val is int)
            {
                return (int)val;
            }

            return -1;
        }
    }

    protected string GetPageUniqueKey()
    {
        object key = ViewState["PageUniqueKey"];

        if (key is string)
        {
            return (string)key;
        }

        // Generate unique key
        Guid guid = Guid.NewGuid();

        string str = guid.ToString();

        ViewState["PageUniqueKey"] = str;

        return str;
    }

    #endregion (Session management)



    protected List<DataError> Errors
    {
        get
        {
            List<DataError> value = Session[GetPageUniqueKey() + "_Errors"] as List<DataError>;
            if (value == null)
                Session[GetPageUniqueKey() + "_Errors"] = (value = new List<DataError>());
            return value;
        }
        set
        {
            Session[GetPageUniqueKey() + "_Errors"] = value;
        }
    }

    protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        if (ProjectID > 0)
            e.Command.Parameters["@projectID"].Value = ProjectID;
    }

    protected void CPAct_Callback(object sender, CallbackEventArgsBase e)
    {
        if (ComboBoxAppendix.SelectedItem == null
            || !(ComboBoxAppendix.SelectedItem.GetValue("right_name") is string))
        {
            LabelActRight.Text = "немає даних";
        }
        else
        {
            LabelActRight.Text = (string)ComboBoxAppendix.SelectedItem.GetValue("right_name");
            switch ((int)ComboBoxAppendix.SelectedItem.GetValue("table_type"))
            {
                case 0: // Об’єкти нерухомості нежитлового фонду
                    GridViewObjects.Columns["addr_street_name"].Visible = true;
                    GridViewObjects.Columns["addr_nomer"].Visible = true;
                    GridViewObjects.Columns["addr_misc"].Visible = true;
                    GridViewObjects.Columns["addr_distr"].Visible = true;
                    GridViewObjects.Columns["year_built"].Visible = true;
                    GridViewObjects.Columns["sqr_total"].Visible = true;
                    GridViewObjects.Columns["obj_kind"].Visible = true;
                    GridViewObjects.Columns["obj_type"].Visible = true;

                    GridViewObjects.Columns["address"].Visible = false;
                    GridViewObjects.Columns["location"].Visible = false;
                    GridViewObjects.Columns["commissioned_date"].Visible = false;
                    break;
                case 1: // Інші об’єкти комунальної власності
                    GridViewObjects.Columns["addr_street_name"].Visible = false;
                    GridViewObjects.Columns["addr_nomer"].Visible = false;
                    GridViewObjects.Columns["addr_misc"].Visible = false;
                    GridViewObjects.Columns["addr_distr"].Visible = false;
                    GridViewObjects.Columns["year_built"].Visible = false;
                    GridViewObjects.Columns["sqr_total"].Visible = false;
                    GridViewObjects.Columns["obj_kind"].Visible = false;
                    GridViewObjects.Columns["obj_type"].Visible = false;

                    GridViewObjects.Columns["address"].Visible = true;
                    GridViewObjects.Columns["location"].Visible = true;
                    GridViewObjects.Columns["commissioned_date"].Visible = true;
                    break;
            }
        }

        GridViewObjects.DataBind();
    }

    protected void SqlDataSource2_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        if (ComboBoxAppendix.SelectedItem != null)
            e.Command.Parameters["@itemID"].Value = ComboBoxAppendix.SelectedItem.Value;
    }

    private bool ProcessCreateAct()
    {
        string actNumber = TextBoxActNumber.Text.Trim();
        DateTime actDate = DateEditActDate.Date;
        int[] selectedTableRowIDs = GridViewObjects.GetSelectedFieldValues("id").Select(x => Convert.ToInt32(x)).ToArray();
        int rightID = ConvertTo<int>(ComboBoxAppendix.SelectedItem.GetValue("right_id"));
        int orgFromID = ConvertTo<int>(ComboBoxAppendix.SelectedItem.GetValue("org_from_id"));
        int orgToID = ConvertTo<int>(ComboBoxAppendix.SelectedItem.GetValue("org_to_id"));
        int objectType = (int)ComboBoxAppendix.SelectedItem.GetValue("table_type");

        return RishProjectTableProcessor.ProcessCreateAct(Errors, ProjectID, User, actNumber, actDate, null, selectedTableRowIDs, rightID, orgFromID, orgToID, objectType, false);
    }

    protected void ButtonCreateAct_Click(object sender, EventArgs e)
    {
        if (ProcessCreateAct())
        {
            Response.Redirect("~/BPRishProject/RishProjectForm.aspx?projid=" + ProjectID);
        }
        else
        {
            PanelServerErrors.ClientVisible = true;
            LabelServerErrors.Text = string.Join("\n", Errors.Where(x => x.RowID == 0).Select(x => x.What));
        }
    }



    //private void ProcessCommObjects(ObjectsData data)
    //{
    //    using (SqlConnection conn = Utils.ConnectToDatabase())
    //    {
    //        if (conn == null)
    //            return;

    //        GUKV.ImportToolUtils.ObjectFinder buildingFinder = new GUKV.ImportToolUtils.ObjectFinder();
    //        buildingFinder.BuildObjectCacheFromSqlServer(conn);

    //        BalansFinder balansFinder = new BalansFinder(conn);

    //        using (SqlTransaction tran = conn.BeginTransaction())
    //        {
    //            int actDocumentID = BalansTransferUtils.CreateAktNew(conn, tran, 0, 0, data.ActDate, data.ActNumber,
    //                0, 0, data.ProjectDocumentID);

    //            DB dataset = new DB();

    //            tran.NewAdapter<DBTableAdapters.bp_rish_project_tableTableAdapter>()
    //                .FillByProjectID(dataset.bp_rish_project_table, data.ProjectID);

    //            Dictionary<string, int> lookupDistricts = tran
    //                .NewAdapter<DBTableAdapters.dict_districts2TableAdapter>()
    //                .GetData()
    //                .Where(x => !x.IsnameNull())
    //                .GroupBy(x => x.name.Trim().ToUpper())
    //                .ToDictionary(x => x.Key, x => x.First().id);
    //            Dictionary<string, int> lookupObjKinds = tran
    //                .NewAdapter<DBTableAdapters.dict_object_kindTableAdapter>()
    //                .GetData()
    //                .Where(x => !x.IsnameNull())
    //                .GroupBy(x => x.name.Trim().ToUpper())
    //                .ToDictionary(x => x.Key, x => x.First().id);
    //            Dictionary<string, int> lookupObjTypes = tran
    //                .NewAdapter<DBTableAdapters.dict_object_typeTableAdapter>()
    //                .GetData()
    //                .Where(x => !x.IsnameNull())
    //                .GroupBy(x => x.name.Trim().ToUpper())
    //                .ToDictionary(x => x.Key, x => x.First().id);

    //            foreach (var entry in
    //                dataset.bp_rish_project_table.Where(x => data.SelectedTableRowIDs.Contains(x.id)))
    //            {
    //                if (entry.Issqr_totalNull())
    //                {
    //                    Errors.Add(new DataError()
    //                    {
    //                        RowID = entry.id,
    //                        FieldName = "sqr_total",
    //                        What = "Відсутня інформація щодо площі об'єкту"
    //                    });
    //                    continue;
    //                }
    //                if (entry.Isaddr_street_nameNull())
    //                {
    //                    Errors.Add(new DataError()
    //                    {
    //                        RowID = entry.id,
    //                        FieldName = "addr_street_name",
    //                        What = "Відсутня інформація щодо вулиці об'єкту"
    //                    });
    //                    continue;
    //                }

    //                /*
    //                 * Conveyancing types:
    //                 * 
    //                 * case "1": // Прийняти об'єкт на баланс
    //                 * case "2": // Передати об'єкт з балансу
    //                 * case "3": // Списати об'єкту з балансу шляхом зносу
    //                 * case "4": // Списати об'єкту з балансу шляхом приватизації
    //                 * case "5": // Постановка  новозбудованого об'єкту на баланс
    //                 */

    //                int building_id, balans_id;

    //                // Note that we ignore the return type until we get into particulars
    //                // of processing a specific conveyancing type.
    //                RishProjectTableProcessor.FindObject(buildingFinder, balansFinder, lookupDistricts, entry, 
    //                    data.RightRequiresOrgFromID ? data.OrgFromID : data.OrgToID,
    //                    out building_id, out balans_id);

    //                if (data.RightRequiresOrgFromID && data.RightRequiresOrgToID)
    //                {
    //                    // Conveyancing type: 1 or 2 (the exact type is irrelevant)

    //                    // We MUST have the balans object to transfer
    //                    if (building_id <= 0)
    //                    {
    //                        Errors.Add(new DataError()
    //                        {
    //                            RowID = entry.id,
    //                            What = "Будинок за вказаною адресою не знайдено",
    //                        });
    //                        continue;
    //                    }
    //                    if (balans_id <= 0)
    //                    {
    //                        Errors.Add(new DataError()
    //                        {
    //                            RowID = entry.id,
    //                            What = "Об'єкт на балансі не знайден",
    //                        });
    //                        continue;
    //                    }

    //                    var balansTransfer = new BalansTransfer();
    //                    balansTransfer.transferType = ObjectTransferType.Transfer;
    //                    balansTransfer.objectId = building_id;
    //                    balansTransfer.sqr = entry.sqr_total;
    //                    balansTransfer.organizationFromId = data.OrgFromID;
    //                    balansTransfer.organizationToId = data.OrgToID;
    //                    balansTransfer.balansId = balans_id;

    //                    var actObject = new ActObject();
    //                    actObject.makeChangesIn1NF = true;
    //                    actObject.objectId = 22930; // FAKE!!!
    //                    actObject.balansTransfers.Add(balansTransfer);

    //                    var importedAct = new ImportedAct();
    //                    importedAct.actObjects.Add(actObject);

    //                    var rish = new Document();
    //                    rish.documentDate = DateTime.Now;
    //                    rish.documentNumber = data.ProjectDocumentNumber;
    //                    rish.documentDate = data.ProjectDocumentDate;
    //                    rish.modify_by = User.Identity.Name;
    //                    rish.ownership_type_id = data.RightID;

    //                    if (GUKV.Conveyancing.DB.TransferBalansObjects(conn, tran, importedAct, rish, false, 3))
    //                    {
    //                        foreach (ActObject act in importedAct.actObjects)
    //                        {
    //                            foreach (BalansTransfer bt in act.balansTransfers)
    //                            {
    //                                BalansTransferUtils.AddObjectToAkt(conn, tran, balans_id, building_id, 
    //                                    data.ProjectDocumentID, actDocumentID);
    //                            }
    //                        }
    //                    }
    //                }
    //                else if (data.RightRequiresOrgFromID)
    //                {
    //                    // Conveyancing type: 3 or 4 (must determine the exact type)

    //                    // We MUST have the balans object to transfer
    //                    if (building_id <= 0)
    //                    {
    //                        Errors.Add(new DataError()
    //                        {
    //                            RowID = entry.id,
    //                            What = "Будинок за вказаною адресою не знайдено",
    //                        });
    //                        continue;
    //                    }
    //                    if (balans_id <= 0)
    //                    {
    //                        Errors.Add(new DataError()
    //                        {
    //                            RowID = entry.id,
    //                            What = "Об'єкт на балансі не знайден",
    //                        });
    //                        continue;
    //                    }

    //                    int conveyancingType = (data.RightID == 11 ? 3 : 4);

    //                    var balansTransfer = new BalansTransfer();
    //                    balansTransfer.transferType = ObjectTransferType.Destroy;
    //                    balansTransfer.objectId = building_id;
    //                    balansTransfer.sqr = entry.sqr_total;
    //                    balansTransfer.organizationFromId = data.OrgFromID;
    //                    balansTransfer.balansId = balans_id;

    //                    var actObject = new ActObject();
    //                    actObject.makeChangesIn1NF = true;
    //                    actObject.objectId = 22930; // FAKE!!!
    //                    actObject.balansTransfers.Add(balansTransfer);

    //                    var importedAct = new ImportedAct();
    //                    importedAct.actObjects.Add(actObject);

    //                    var rish = new Document();
    //                    rish.documentDate = DateTime.Now;
    //                    rish.documentNumber = data.ProjectDocumentNumber;
    //                    rish.documentDate = data.ProjectDocumentDate;
    //                    rish.modify_by = User.Identity.Name;
    //                    rish.ownership_type_id = data.RightID;

    //                    int vidch_type_id = 0;
    //                    if (conveyancingType == 3)
    //                        vidch_type_id = 2;
    //                    else if (conveyancingType == 4)
    //                        vidch_type_id = 1;

    //                    if (GUKV.Conveyancing.DB.TransferBalansObjects(conn, tran, importedAct, rish, false, vidch_type_id))
    //                    {
    //                        foreach (ActObject act in importedAct.actObjects)
    //                        {
    //                            foreach (BalansTransfer bt in act.balansTransfers)
    //                            {
    //                                BalansTransferUtils.AddObjectToAkt(conn, tran, balans_id, building_id,
    //                                    data.ProjectDocumentID, actDocumentID);
    //                            }
    //                        }
    //                    }

    //                    if (conveyancingType == 3)
    //                        BalansTransferUtils.ModifyTechCondition(conn, tran, balans_id, 3); // make this balans object 'ЗНЕСЕНИЙ'
    //                }
    //                else // Assuming data.RightRequiresOrgToID
    //                {
    //                    // Conveyancing type: 5

    //                    // We CAN'T expect to have a readily available object
    //                    // (although if it exists, it is certainly a bad sign
    //                    // as we might be dealing with a duplicate).
                        
    //                    // Conversely, we MUST reuse the building_id, if such
    //                    // a reference was found.
    //                    if (building_id < 0)
    //                    {
    //                        // In this rendition of this functionality, we shall refrain
    //                        // from creating any dictionary entries. Instead, successful
    //                        // resolution of dictionary references becomes a requirement.
    //                        int streetId;
    //                        if (!buildingFinder.FindStreetId(entry.addr_street_name, out streetId, true))
    //                        {
    //                            Errors.Add(new DataError()
    //                            {
    //                                RowID = entry.id,
    //                                FieldName = "addr_street_name",
    //                                What = "Немає вулиці в реєстрі",
    //                            });
    //                            continue;
    //                        }
    //                        if (entry.Isaddr_distrNull() || entry.addr_distr.Trim().Length == 0)
    //                        {
    //                            Errors.Add(new DataError()
    //                            {
    //                                RowID = entry.id,
    //                                FieldName = "addr_distr",
    //                                What = "Район адреси об'єкта не визначено",
    //                            });
    //                            continue;
    //                        }
    //                        if (entry.Isobj_kindNull() || entry.obj_kind.Trim().Length == 0)
    //                        {
    //                            Errors.Add(new DataError()
    //                            {
    //                                RowID = entry.id,
    //                                FieldName = "obj_kind",
    //                                What = "Вид будинку об'єкта не визначено",
    //                            });
    //                            continue;
    //                        }
    //                        if (entry.Isobj_typeNull() || entry.obj_type.Trim().Length == 0)
    //                        {
    //                            Errors.Add(new DataError()
    //                            {
    //                                RowID = entry.id,
    //                                FieldName = "obj_type",
    //                                What = "Тип будинку об'єкта не визначено",
    //                            });
    //                            continue;
    //                        }

    //                        int districtId;
    //                        if (!lookupDistricts.TryGetValue(entry.addr_distr.Trim().ToUpper(), out districtId))
    //                        {
    //                            Errors.Add(new DataError()
    //                            {
    //                                RowID = entry.id,
    //                                FieldName = "addr_distr",
    //                                What = "Немає району в реєстрі",
    //                            });
    //                            continue;
    //                        }
    //                        int objKindId;
    //                        if (!lookupObjKinds.TryGetValue(entry.obj_kind.Trim().ToUpper(), out objKindId))
    //                        {
    //                            Errors.Add(new DataError()
    //                            {
    //                                RowID = entry.id,
    //                                FieldName = "obj_kind",
    //                                What = "Немає виду будівлі об'єкта в реєстрі",
    //                            });
    //                            continue;
    //                        }
    //                        int objTypeId;
    //                        if (!lookupObjTypes.TryGetValue(entry.obj_type.Trim().ToUpper(), out objTypeId))
    //                        {
    //                            Errors.Add(new DataError()
    //                            {
    //                                RowID = entry.id,
    //                                FieldName = "obj_type",
    //                                What = "Немає типу будівлі об'єкта в реєстрі",
    //                            });
    //                            continue;
    //                        }

    //                        string errorMessage;
    //                        building_id = ConveyancingUtils.CreateNew1NFBuilding(
    //                            conn,
    //                            tran,
    //                            streetId,
    //                            districtId,
    //                            objKindId,
    //                            objTypeId,
    //                            (entry.Isaddr_nomerNull() ? string.Empty : entry.addr_nomer),
    //                            string.Empty,
    //                            string.Empty,
    //                            (entry.Isaddr_miscNull() ? string.Empty : entry.addr_misc),
    //                            out errorMessage
    //                            );
    //                        if (building_id < 0)
    //                        {
    //                            Errors.Add(new DataError()
    //                            {
    //                                RowID = entry.id,
    //                                What = "Помилка створення запису будівлі (" + errorMessage + ")",
    //                            });
    //                            continue;
    //                        }
    //                    }

    //                    var balansTransfer = new BalansTransfer();
    //                    balansTransfer.transferType = ObjectTransferType.Create;
    //                    balansTransfer.objectId = building_id;
    //                    balansTransfer.sqr = entry.sqr_total;
    //                    balansTransfer.organizationToId = data.OrgToID;

    //                    var actObject = new ActObject();
    //                    actObject.makeChangesIn1NF = true;
    //                    actObject.objectId = 22930; // FAKE!!!
    //                    actObject.balansTransfers.Add(balansTransfer);

    //                    var importedAct = new ImportedAct();
    //                    importedAct.actObjects.Add(actObject);

    //                    var rish = new Document();
    //                    rish.documentDate = DateTime.Now;
    //                    rish.documentNumber = data.ProjectDocumentNumber;
    //                    rish.documentDate = data.ProjectDocumentDate;
    //                    rish.modify_by = User.Identity.Name;
    //                    rish.ownership_type_id = data.RightID;

    //                    if (GUKV.Conveyancing.DB.TransferBalansObjects(conn, tran, importedAct, rish, false, 0))
    //                    {
    //                        foreach (ActObject act in importedAct.actObjects)
    //                        {
    //                            foreach (BalansTransfer bt in act.balansTransfers)
    //                            {
    //                                BalansTransferUtils.AddObjectToAkt(conn, tran, balans_id, building_id,
    //                                    data.ProjectDocumentID, actDocumentID);
    //                            }
    //                        }
    //                    }
    //                }

    //                entry.is_acted_on = true;
    //            }

    //            tran.NewAdapter<DBTableAdapters.bp_rish_project_tableTableAdapter>()
    //                .Update(dataset);

    //            tran.Commit();
    //        }
    //    }
    //}

    private static T ConvertTo<T>(object value)
    {
        if (value == null || value is DBNull)
            return default(T);
        return (T)Convert.ChangeType(value, typeof(T));
    }
    
    protected void GridViewObjects_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        DataError error = Errors.FirstOrDefault(x => x.RowID == (int)e.KeyValue);
        if (error == null)
            return;

        e.Cell.ToolTip = error.What;

        if (error.FieldName == null || error.FieldName == e.DataColumn.FieldName)
        {
            e.Cell.BackColor = System.Drawing.Color.Red;
            e.Cell.ForeColor = System.Drawing.Color.White;
        }
        else if (error.FieldName != null)
        {
            e.Cell.BackColor = System.Drawing.Color.LightYellow;
        }
    }
}
