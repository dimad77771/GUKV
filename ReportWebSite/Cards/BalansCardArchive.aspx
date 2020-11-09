<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BalansCardArchive.aspx.cs" Inherits="Cards_BalansCardArchive"
    MasterPageFile="~/NoMenu.master" Title="Архівний Стан Об'єкту На Балансі" %>

<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" language="javascript">

    // <![CDATA[

    function ShowBalansArchiveCard(archiveId) {

        PopupArchiveStates.Hide();

        var cardUrl = "../Cards/BalansCardArchive.aspx?arid=" + archiveId;

        window.open(cardUrl);
    }

    // ]]>

</script>

    <script type="text/javascript" language="javascript">
        var imageIndex = 0;


        function OnBeginCallback(s, e) {
            //lp.Show();
        }

        function OnCallbackComplete(s, e) {
            //setUrlParam('tab', 5);
            ContentCallback.PerformCallback('refreshphoto:');
        }

        function OnPrevBtnClick(e) {
            PrevImage();
            ASPxClientUtils.PreventEvent(e);
        }

        function OnNextBtnClick(e) {
            NextImage();
            ASPxClientUtils.PreventEvent(e);
        }

        function NextImage() {
            imageIndex++;
            if (imageIndex == GetCount())
                imageIndex = 0;
            cb.PerformCallback(imageIndex);
        }

        function PrevImage() {
            imageIndex--;
            if (imageIndex < 0)
                imageIndex = GetCount() - 1;
            cb.PerformCallback(imageIndex);
        }

        function GetCount() {
            return imageGallery.cpCount;
        }

        function OnDeleteBtnClick(e) {
            DeleteImage();
            lp.Hide();
            ASPxClientUtils.PreventEvent(e);
        }

        function DeleteImage(index) {
            var r = confirm("Ви дійсно хочете видалити фото?");
            if (r == true) {
                cbDelete.PerformCallback(index);
            }
        }

        function EditImage(index, text) {
            textEdit.Text = text;
            editPopup.Show();
            //cbDelete.PerformCallback(index);
        }


        function Uploader_OnUploadStart() {
            btnUpload.SetEnabled(false);
        }


        function setUrlParam(prmName, val) {
            var res = '';
            var d = location.href.split("#")[0].split("?");
            var base = d[0];
            var query = d[1];
            if (query) {
                var params = query.split("&");
                for (var i = 0; i < params.length; i++) {
                    var keyval = params[i].split("=");
                    if (keyval[0] != prmName) {
                        res += params[i] + '&';
                    }
                }
            }
            res += prmName + '=' + val;
            window.location.href = base + '?' + res;
            return false;
        }

        function Uploader_OnFilesUploadComplete(args) {
            UpdateUploadButton();
            //setUrlParam('tab', 5);
            ContentCallback.PerformCallback('refreshphoto:');

        }
        function UpdateUploadButton() {
            btnUpload.SetEnabled(uploader.GetText(0) != "");
        }


    </script>

        <script type="text/javascript">
            function OnInit(s, e) {
                AdjustSize();
            }
            function OnEndCallback(s, e) {
                AdjustSize();
            }
            function OnControlsInitialized(s, e) {
                ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                    AdjustSize();
                });
            }
            function AdjustSize() {
                var height = Math.max(0, document.documentElement.clientHeight) - 260;
                grid.SetHeight(height);
            }
            function ShowPhoto(s, e) {
                if (e.buttonID == 'btnPhoto') {
                    $.cookie('RecordID', s.GetRowKey(e.visibleIndex));
                    ASPxFileManagerPhotoFiles.Refresh();
                    PopupObjectPhotos.Show();
                }
            }
    </script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansCardProperties" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 * FROM view_balans_all WHERE balans_id = @balid">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="balid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansCardPropertiesArch" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 *, NULL AS 'sqr_free_total', NULL AS 'sqr_free_korysna', NULL AS 'sqr_free_mzk', NULL AS 'free_sqr_floors', NULL AS 'free_sqr_purpose'
        FROM view_arch_balans WHERE archive_id = @arid">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="arid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansCardDocs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT bdoc.id AS 'row_id', doc.kind, doc.topic, doc.doc_num, doc.doc_date, bd.sqr_obj, bd.document_id 
        FROM balans_docs bdoc
        LEFT OUTER JOIN building_docs bd ON bd.id = bdoc.building_docs_id
        LEFT OUTER JOIN view_documents doc ON doc.id = bd.document_id
        WHERE bdoc.balans_id = @balid AND NOT (bd.document_id IS NULL) ORDER BY doc.doc_date">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="balid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansCardDocsArch" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT bdoc.archive_id AS 'row_id', doc.kind, doc.topic, doc.doc_num, doc.doc_date, bd.sqr_obj, bd.document_id 
        FROM arch_balans_docs bdoc
        INNER JOIN arch_balans ab ON ab.archive_link_code = bdoc.archive_balans_link_code
        LEFT OUTER JOIN building_docs bd ON bd.id = bdoc.building_docs_id
        LEFT OUTER JOIN view_documents doc ON doc.id = bd.document_id
        WHERE ab.archive_id = @arid AND NOT (bd.document_id IS NULL) ORDER BY doc.doc_date">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="arid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansArchive" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT archive_id, street_full_name, addr_nomer, org_full_name, sqr_total, form_ownership, modify_date, modified_by
        FROM view_arch_balans 
        WHERE balans_id = @balid AND (NOT modified_by IS NULL) AND (NOT modify_date IS NULL) ORDER BY modify_date, archive_id" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="balid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePhoto" runat="server" 
        ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
        SelectCommand="SELECT 
            id, bal_id, 
            file_name, 
            file_ext, 
            user_id, 
            create_date,
            @folder_prefix + CAST(bal_id AS VARCHAR(MAX)) + '/' + CAST(id AS VARCHAR(MAX)) + file_ext as ImageUrl
            FROM balans_photos WHERE bal_id = @bal_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_id" />
        <asp:Parameter DbType="String" DefaultValue="0" Name="folder_prefix" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePhotoArch" runat="server" 
        ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
        SelectCommand="SELECT 
            id, bal_id, 
            file_name, 
            file_ext, 
            user_id, 
            create_date,
            @folder_prefix + CAST(bal_id AS VARCHAR(MAX)) + '/' + CAST(archive_balans_link_code AS VARCHAR(MAX)) + '/' + CAST(id AS VARCHAR(MAX)) + file_ext as ImageUrl
            FROM arch_balans_photos WHERE archive_balans_link_code = (select archive_link_code from arch_balans where archive_id= @archive_id)">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="archive_id" />
        <asp:Parameter DbType="String" DefaultValue="0" Name="folder_prefix" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceTechState" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_tech_state">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceZgodaRenter" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_zgoda_renter ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeSquare" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT [id]
      ,[balans_id]
      ,[total_free_sqr]
      ,[free_sqr_condition_id]
      ,[floor]
      ,[possible_using]
      ,[water]
      ,[heating]
      ,[power]
      ,[gas]
      ,[modify_date]
      ,[modified_by]
      ,[original_id] 
      ,[free_sqr_korysna]
      ,[note]
      ,[is_solution] 
      ,[initiator] 
      ,[zgoda_control_id] 
      ,[zgoda_renter_id] 
    FROM [balans_free_square] WHERE [balans_id] = @balans_id" 
        ProviderName="System.Data.SqlClient">
    <SelectParameters>
        <asp:Parameter Name="balans_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeSquareArch" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select * from arch_balans_free_square where archive_balans_link_code = (select archive_link_code from arch_balans where archive_id = @archive_id)" 
        ProviderName="System.Data.SqlClient">
    <SelectParameters>
        <asp:Parameter Name="archive_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>




    <dx:ASPxCallback ID="cbDelete" ClientInstanceName="cbDelete" runat="server" OnCallback="delete_Callback" >
        <ClientSideEvents CallbackComplete="OnCallbackComplete" BeginCallback="OnBeginCallback" />
    </dx:ASPxCallback>

<table border="0" cellspacing="0" cellpadding="0">

<tr>
<td>
<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" 
        runat="server" ActiveTabIndex="0">
    <TabPages>
        <dx:TabPage Text="Архівний Стан Об'єкту На Балансі" Name="BalansCardProperties">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the first tab BEGIN --%>

<asp:FormView runat="server" BorderStyle="None" ID="BalansDetails" DataSourceID="SqlDataSourceBalansCardProperties" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Адреса Будинку">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="Район"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditOrgId" runat="server" ReadOnly="True" Text='<%# Eval("district") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Назва Вулиці"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox34" runat="server" ReadOnly="True" Text='<%# Eval("street_full_name") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Номер Будинку"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox33" runat="server" ReadOnly="True" Text='<%# Eval("addr_nomer") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Додаткова Адреса"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox35" runat="server" ReadOnly="True" Text='<%# Eval("addr_misc") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Кількість Поверхів"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox1" runat="server" ReadOnly="True" Text='<%# Eval("num_floors") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Розташування (Поверх)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ReadOnly="True" Text='<%# Eval("floors") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
         <dx:ASPxRoundPanel ID="ASPxRoundPanel9" runat="server" HeaderText="Реєстрація права власності">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent11" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                       <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Інвентаризаційний номер справи БТІ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ReadOnly="True" Text='<%# Eval("obj_bti_code") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Номер запису про право власності"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ReadOnly="True" Text='<%# Eval("privacynote") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Дата виготовлення технічного паспорту в БТІ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit4" runat="server" ReadOnly="True" Value='<%# Eval("date_bti") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Реєстраційний № об'єкту в Держреєстрі"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox6" runat="server" ReadOnly="True" Text='<%# Eval("bti_condition") %>' Width="290px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Балансоутримувач">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Балансоутримувач"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox10" runat="server" ReadOnly="True" Text='<%# Eval("org_full_name") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Право"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox12" runat="server" ReadOnly="True" Text='<%# Eval("ownership_type") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Форма Власності"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox13" runat="server" ReadOnly="True" Text='<%# Eval("org_ownership") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Утримуюча Організація"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox11" runat="server" ReadOnly="True" Text='<%# Eval("org_maintainer_full_name") %>' Width="700px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Додаткові Відомості">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Вид Обєкта"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox7" runat="server" ReadOnly="True" Text='<%# Eval("object_kind") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Тип Обєкта"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox8" runat="server" ReadOnly="True" Text='<%# Eval("object_type") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Група Призначення"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox14" runat="server" ReadOnly="True" Text='<%# Eval("purpose_group") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Призначення"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox15" runat="server" ReadOnly="True" Text='<%# Eval("purpose") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Технічний Стан"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox16" runat="server" ReadOnly="True" Text='<%# Eval("condition") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Форма Власності"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox17" runat="server" ReadOnly="True" Text='<%# Eval("form_ownership") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Історична Цінність"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox18" runat="server" ReadOnly="True" Text='<%# Eval("history") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
 <%--pgv                           <td width="100px"><dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Відділ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox19" runat="server" ReadOnly="True" Text='<%# Eval("otdel_gukv") %>' Width="290px" /></td>        --%>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Назва Об'єкту"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox5" runat="server" ReadOnly="True" Text='<%# Eval("balans_obj_name") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Примітка"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox9" runat="server" ReadOnly="True" Text='<%# Eval("note") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td colspan="7">
                                <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" ReadOnly="True" Checked='<%# (1.Equals(Eval("priznak_1nf"))) ? true : false %>' Text="Введено по 1НФ" />
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" HeaderText="Стан">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel33" runat="server" Text='<%# EvaluateSignature(Eval("modified_by"), Eval("modify_date")) %>'></dx:ASPxLabel></td>
                            <td align="right">
                                <dx:ASPxPopupControl ID="PopupArchiveStates" runat="server" 
                                    HeaderText="Архівні стани" 
                                    ClientInstanceName="PopupArchiveStates" 
                                    PopupElementID="CardPageControl"
                                    PopupAction="None"
                                    PopupHorizontalAlign="Center"
                                    PopupVerticalAlign="Middle"
                                    PopupAnimationType="Slide">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            
                                            <dx:ASPxGridView ID="GridViewBalansArchiveStates" ClientInstanceName="GridViewBalansArchiveStates" runat="server"
                                                AutoGenerateColumns="False" DataSourceID="SqlDataSourceBalansArchive" KeyFieldName="archive_id" Width="780px">

                                                <Columns>
                                                    <dx:GridViewDataTextColumn FieldName="archive_id" VisibleIndex="0" Caption="Картка Архівного Стану">
                                                        <DataItemTemplate>
                                                            <%# "<center><a href=\"javascript:ShowBalansArchiveCard(" + Eval("archive_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
                                                        </DataItemTemplate>
                                                        <Settings ShowInFilterControl="False"/>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="street_full_name" VisibleIndex="1" Caption="Назва Вулиці" Width="120px"/>
                                                    <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="2" Caption="Номер Будинку"/>
                                                    <dx:GridViewDataTextColumn FieldName="org_full_name" VisibleIndex="3" Caption="Балансоутримувач" Width="180px"/>
                                                    <dx:GridViewDataTextColumn FieldName="sqr_total" VisibleIndex="4" Caption="Площа На Балансі (кв.м.)"/>
                                                    <dx:GridViewDataTextColumn FieldName="form_ownership" VisibleIndex="5" Caption="Форма Власності"/>
                                                    <dx:GridViewDataDateColumn FieldName="modify_date" VisibleIndex="6" Caption="Коли Внесені Зміни"/>
                                                    <dx:GridViewDataTextColumn FieldName="modified_by" VisibleIndex="7" Caption="Ким Внесені Зміни"/>
                                                </Columns>

                                                <SettingsBehavior ColumnResizeMode="Control" EnableCustomizationWindow="False" />
                                                <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
                                                <SettingsPager PageSize="10" />
                                                <Styles Header-Wrap="True" />
                                                <SettingsCookies CookiesID="GUKV.BalansCard.ArchiveStates" Enabled="False" Version="A2_1" />

                                                <ClientSideEvents EndCallback="function (s,e) { GridViewBalansArchiveStates.SetHeight(500); }"/>
                                            </dx:ASPxGridView>

                                        </dx:PopupControlContentControl>
                                    </ContentCollection>

                                    <ClientSideEvents PopUp="function (s,e) { GridViewBalansArchiveStates.SetHeight(500); }"/>
                                </dx:ASPxPopupControl>

                                <dx:ASPxButton ID="ASPxButtonArchive" ClientInstanceName="ASPxButtonArchive" runat="server" Text="Архівні Стани" AutoPostBack="false"
                                    ClientVisible='<%# IsHistoryButtonVisible() %>' >
                                    <ClientSideEvents Click="function (s,e) { PopupArchiveStates.Show(); }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </ItemTemplate>
</asp:FormView>

<%-- Content of the first tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Інформація щодо площ" Name="BalansCardSqr">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the second tab BEGIN --%>

<asp:FormView runat="server" BorderStyle="None" ID="BalansDetailsPage2" DataSourceID="SqlDataSourceBalansCardProperties" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Інформація щодо площ">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="Площа Нежилих Приміщень На Балансі (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditOrgId" runat="server" ReadOnly="True" Text='<%# Eval("sqr_total") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Корисна Площа Нежилих Приміщень На Балансі (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox34" runat="server" ReadOnly="True" Text='<%# Eval("sqr_kor") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Площа Власних Потреб (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox33" runat="server" ReadOnly="True" Text='<%# Eval("sqr_vlas_potreb") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Площа Підвалів (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox35" runat="server" ReadOnly="True" Text='<%# Eval("sqr_pidval") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td colspan="5"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Площа Власних Потреб, Яка Не Може Бути Надана В Оренду (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ReadOnly="True" Text='<%# Eval("sqr_not_for_rent") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="24px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Площа Надана В Оренду (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ReadOnly="True" Text='<%# Eval("sqr_in_rent") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Площа Приватизована (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ReadOnly="True" Text='<%# Eval("sqr_privatizov") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Кількість Договорів Оренди"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox20" runat="server" ReadOnly="True" Text='<%# Eval("num_rent_agr") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Кількість Приватизованих Приміщень"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox6" runat="server" ReadOnly="True" Text='<%# Eval("num_privat_apt") %>' Width="140px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel8" runat="server" HeaderText="Вільні Приміщення">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent8" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel40" runat="server" Text="Вільні Приміщення: Загальна Площа (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox32" runat="server" ReadOnly="True" Text='<%# Eval("sqr_free_total") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel41" runat="server" Text="Вільні Приміщення: Корисна Площа (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox36" runat="server" ReadOnly="True" Text='<%# Eval("sqr_free_korysna") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
 <%--                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel42" runat="server" Text="Вільні Приміщення МЗК (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox37" runat="server" ReadOnly="True" Text='<%# Eval("sqr_free_mzk") %>' Width="140px" /></td>             --%>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel43" runat="server" Text="Місце Розташування Вільного Приміщення (поверх)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox38" runat="server" ReadOnly="True" Text='<%# Eval("free_sqr_floors") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel38" runat="server" Text="Можливе Використання Вільного Приміщення"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox30" runat="server" ReadOnly="True" Text='<%# Eval("free_sqr_purpose") %>' Width="549px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </ItemTemplate>
</asp:FormView>

<%-- Content of the second tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Вільні приміщення" Name="Tab4">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3a" runat="server">
<%--                    <asp:FormView runat="server" BorderStyle="None" ID="FreeSquareForm" DataSourceID="SqlDataSourceFreeSquare" EnableViewState="False">
                        <ItemTemplate>--%>
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Вільні приміщення">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent10" runat="server">
                                        <table border="0" cellspacing="0" cellpadding="0" width="990px">
                                            <tr>
                                                <td>
                                                

    <dx:ASPxGridView ID="ASPxGridViewFreeSquare" runat="server" AutoGenerateColumns="False" 
        KeyFieldName="id" Width="100%" ClientInstanceName="grid">
            <ClientSideEvents CustomButtonClick="ShowPhoto" Init="OnInit" EndCallback="OnEndCallback" />

	    <SettingsCommandButton>
		    <EditButton>
			    <Image Url="~/Styles/EditIcon.png" />
		    </EditButton>
		    <CancelButton>
			    <Image Url="~/Styles/CancelIcon.png" />
		    </CancelButton>
		    <UpdateButton>
			    <Image Url="~/Styles/SaveIcon.png" />
		    </UpdateButton>
		    <DeleteButton>
			    <Image Url="~/Styles/DeleteIcon.png" />
		    </DeleteButton>
		    <NewButton>
			    <Image Url="~/Styles/AddIcon.png" />
		    </NewButton>
		    <ClearFilterButton Text="Очистити" RenderMode="Link" />
	    </SettingsCommandButton>

        <Columns>
            <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" 
                ShowInCustomizationForm="True" CellStyle-Wrap="False" ShowClearFilterButton="true">

                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnPhoto" Text="Фото">
                        <Image Url="~/Styles/PhotoIcon.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>

                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn FieldName="id" Caption="ID" VisibleIndex="1"  ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="balans_id" Caption="balans_id" VisibleIndex="2" ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="report_id" Caption="report_id" VisibleIndex="14" ReadOnly="true" Visible="false" >
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="total_free_sqr" Caption="Загальна площа об’єкта" VisibleIndex="3" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="free_sqr_korysna" Caption="Корисна площа об’єкта" VisibleIndex="4" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataComboBoxColumn FieldName="free_sqr_condition_id" VisibleIndex="5" 
                Visible="True" Caption="Стан">
                <PropertiesComboBox DataSourceID="SqlDataSourceTechState" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

<%--             <dx:GridViewDataTextColumn FieldName="floor" Caption="Місце розташування вільного приміщення (поверх)" VisibleIndex="6">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="possible_using" Caption="Можливе використання вільного приміщення" VisibleIndex="7">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>           --%>

            <dx:GridViewBandColumn Caption="Комунальне забезпечення">
                <Columns>

                    <dx:GridViewDataCheckColumn FieldName="water" Caption="Водопостачання" VisibleIndex="7">
                        <HeaderStyle Wrap="True" />
                    </dx:GridViewDataCheckColumn>

                    <dx:GridViewDataCheckColumn FieldName="heating" Caption="Теплопостачання" VisibleIndex="8">
                        <HeaderStyle Wrap="True" />
                    </dx:GridViewDataCheckColumn>

                    <dx:GridViewDataCheckColumn FieldName="power" Caption="Електропостачання" VisibleIndex="9">
                        <HeaderStyle Wrap="True" />
                    </dx:GridViewDataCheckColumn>

                    <dx:GridViewDataCheckColumn FieldName="gas" Caption="Газопостачання" VisibleIndex="10">
                        <HeaderStyle Wrap="True" />
                    </dx:GridViewDataCheckColumn>

                </Columns>
            </dx:GridViewBandColumn>

            <dx:GridViewDataDateColumn FieldName="modify_date" Caption="Дата редагування" VisibleIndex="11" ReadOnly="true">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataTextColumn FieldName="modified_by" Caption="Користувач" VisibleIndex="12" ReadOnly="true">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="floor" Caption="Місце розташування вільного приміщення (поверх)" VisibleIndex="13">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="possible_using" Caption="Можливе використання вільного приміщення" VisibleIndex="14" Width ="100px">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataCheckColumn FieldName="is_solution" Caption="Наявність рішень про проведення інвестиційного конкурсу або про включення об’єкта до переліку майна, що підлягає приватизації" VisibleIndex="15" Width ="100px">
                 <HeaderStyle Wrap="True" />
             </dx:GridViewDataCheckColumn>

            <dx:GridViewDataTextColumn FieldName="initiator" Caption="Ініціатор оренди (текстова інформація, якщо балансоутримувач - пусто)" VisibleIndex="16" Width ="100px">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataComboBoxColumn FieldName="zgoda_control_id" VisibleIndex="17" Width ="80px"
                Visible="True" Caption="Погодження органу управління (освіта, охорона здор., культ.спадщина)">
                <HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceZgodaRenter" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataComboBoxColumn FieldName="zgoda_renter_id" VisibleIndex="18" Width ="80px"
                Visible="True" Caption="Погодження орендодавця">
                <HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceZgodaRenter" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataTextColumn FieldName="note" Caption="Примітка" VisibleIndex="19" Width ="100px">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

        </Columns>
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsPager PageSize="5" />
        <SettingsEditing NewItemRowPosition="Bottom" />
        <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" VerticalScrollableHeight="0" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard"/>
    </dx:ASPxGridView>

    <dx:ASPxPopupControl ID="ASPxPopupControlFreeSquare" runat="server" AllowDragging="True" 
        ClientInstanceName="PopupObjectPhotos" EnableClientSideAPI="True" 
        HeaderText="Фотографії об'єкту з вільним приміщенням" Modal="True" 
        PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
        PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">

                <asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles" runat="server" 
                    SelectMethod="Select" 
                    TypeName="ExtDataEntry.Models.FileAttachment">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="Balans" Name="scope" Type="String" />
                        <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>

                <asp:ObjectDataSource ID="ObjectDataSourcePhotoFilesArch" runat="server" 
                    SelectMethod="Select" 
                    TypeName="ExtDataEntry.Models.FileAttachment">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="Arch" Name="scope" Type="String" />
                        <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>

                <dx:ASPxFileManager ID="ASPxFileManagerPhotoFiles" runat="server" 
                    ClientInstanceName="ASPxFileManagerPhotoFiles" DataSourceID="ObjectDataSourcePhotoFiles">
                    <Settings RootFolder="~\" ThumbnailFolder="~\Thumb\" />
                    <SettingsFileList>
                        <ThumbnailsViewSettings ThumbnailSize="180px" />
                    </SettingsFileList>
                    <SettingsEditing AllowDelete="False" />
                    <SettingsFolders Visible="False" />
                    <SettingsToolbar ShowDownloadButton="True" ShowPath="False" />
                    <SettingsUpload Enabled="false" />

                    <SettingsDataSource FileBinaryContentFieldName="Image" 
                        IsFolderFieldName="IsFolder" KeyFieldName="ID" 
                        LastWriteTimeFieldName="LastModified" NameFieldName="Name" 
                        ParentKeyFieldName="ParentID" />
                </dx:ASPxFileManager>

                <br />

                <dx:ASPxButton ID="ASPxButtonClose" runat="server" AutoPostBack="False" Text="Закрити" HorizontalAlign="Center">
                    <ClientSideEvents Click="function(s, e) { PopupObjectPhotos.Hide(); }" />
                </dx:ASPxButton>

            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>


                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
<%--                        </ItemTemplate>
                    </asp:FormView>--%>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Вартісні показники" Name="BalansCardCosts">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the third tab BEGIN --%>

<asp:FormView runat="server" BorderStyle="None" ID="BalansDetailsPage3" DataSourceID="SqlDataSourceBalansCardProperties" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Вартість Об'єкту">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="Балансова Вартість (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditOrgId" runat="server" ReadOnly="True" Text='<%# Eval("cost_balans") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Залишкова Вартість (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox34" runat="server" ReadOnly="True" Text='<%# Eval("cost_zalishkova") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Знос"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox33" runat="server" ReadOnly="True" Text='<%# Eval("znos") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Станом на"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" ReadOnly="True" Value='<%# Eval("znos_date") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Ринкова Вартість Приміщення (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox1" runat="server" ReadOnly="True" Text='<%# Eval("cost_rinkova") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Станом на"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" ReadOnly="True" Value='<%# Eval("date_cost_rinkova") %>' Width="140px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="Експертна Оцінка">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Вартість Приміщення (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox21" runat="server" ReadOnly="True" Text='<%# Eval("cost_expert_total") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Вартість 1 кв.м."></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox22" runat="server" ReadOnly="True" Text='<%# Eval("cost_expert_1m") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Дата Експертної Оцінки"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit3" runat="server" ReadOnly="True" Value='<%# Eval("date_expert") %>' Width="140px" /></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" HeaderText="Справедлива Вартість">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="Вартість Приміщення (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox23" runat="server" ReadOnly="True" Text='<%# Eval("cost_fair") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Вартість 1 кв.м."></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox24" runat="server" ReadOnly="True" Text='<%# Eval("cost_fair_1m") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel26" runat="server" Text=""></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit5" runat="server" ReadOnly="True" Value='<%# Eval("fair_cost_date") %>' Width="140px" /></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel7" runat="server" HeaderText="Орендна Плата">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent7" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Орендна Плата Нарахована (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox25" runat="server" ReadOnly="True" Text='<%# Eval("cost_rent_narah") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Орендна Плата Сплачена (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox26" runat="server" ReadOnly="True" Text='<%# Eval("cost_rent_payed") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="Заборгованість (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit6" runat="server" ReadOnly="True" Value='<%# Eval("cost_debt") %>' Width="140px" /></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </ItemTemplate>
</asp:FormView>

<%-- Content of the third tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Документи" Name="BalansCardDocs">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the fourth tab BEGIN --%>

<dx:ASPxGridView ID="GridViewBalansCardDocs" runat="server" 
    ClientInstanceName="GridViewBalansCardDocs"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceBalansCardDocs" 
    KeyFieldName="row_id"
    Width="840px" >

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn FieldName="kind" VisibleIndex="0" Caption="Вид Документу" Width="120px" />
        <dx:GridViewDataTextColumn FieldName="topic" VisibleIndex="1" Caption="Назва Документу" Width="350px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("document_id") + ")\">" + Eval("topic") + "</a>"%>
            </DataItemTemplate> 
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="doc_num" VisibleIndex="2" Caption="Номер Документу">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("document_id") + ")\">" + Eval("doc_num") + "</a>"%>
            </DataItemTemplate> 
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="doc_date" VisibleIndex="3" Caption="Дата Документу"/>
        <dx:GridViewDataTextColumn FieldName="sqr_obj" VisibleIndex="4" Caption="Площа Об'єкту За Документом" />
    </Columns>

    <SettingsBehavior EnableCustomizationWindow="False" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" AllowFocusedRow="false" />
    <SettingsPager PageSize="15">
    </SettingsPager>
    <Settings
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard"
        HorizontalScrollBarMode="Visible"
        ShowFooter="True" />
    <SettingsCookies CookiesID="GUKV.BalansCardArchive.Docs" Version="A2" Enabled="False" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

<%-- Content of the fourth tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Фото / плани" Name="Tab5">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl5" runat="server">



                            <dx:ASPxRoundPanel ID="PanelPhoto" runat="server" HeaderText="Фото / плани">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent5" runat="server">


                        <dx:ASPxCallbackPanel ID="ContentCallback" ClientInstanceName="ContentCallback" 
                            runat="server" OnCallback="ContentCallback_Callback" >
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent9" runat="server" SupportsDisabledAttribute="True">


        <dx:ASPxPopupControl ID="editPopup" ClientInstanceName="editPopup" runat="server" Modal="true"  PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"                       ShowCloseButton="false" ShowHeader="false" EnableTheming="false">
            <HeaderStyle HorizontalAlign="Center" />
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <asp:HiddenField ID="editTextField" runat="server" />
                    <dx:ASPxTextBox ID="textEdit" runat="server" AutoPostBack="true" Text="aaa"></dx:ASPxTextBox>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>


                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>

                                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                    <tr>
                                    <td>



        <dx:ASPxImageGallery ID="imageGalleryDemo" runat="server" 
            EnableViewState="false" 
            AlwaysShowPager="false" 
            PagerAlign="Center"
            ThumbnailHeight="195" ThumbnailWidth="195"
            SettingsFullscreenViewer-ShowCloseButton="true" >
            <ItemTextTemplate>



                <div class="item">
                    <%--<div class="item_email" style="text-align:center"><%# Container.EvalDataItem("file_name")%></div><br />--%>
                    <div class="item_email" style="text-align:center">
                        <%--<a style="color:White;cursor:pointer" onclick="javascript:EditImage(<%# Container.ItemIndex, Container.EvalDataItem("file_name") %>);" title="Редагувати">Редагувати</a>&nbsp;&nbsp;--%>
                        <a style="color:White;cursor:pointer;" onclick="javascript:DeleteImage(<%# Container.ItemIndex %>);" title="Видалити" >Видалити</a>
                    </div>
                </div>



            </ItemTextTemplate>
            <SettingsTableLayout RowsPerPage="2" />
            <PagerSettings Position="TopAndBottom">
                <PageSizeItemSettings Visible="False" />
            </PagerSettings>
        </dx:ASPxImageGallery> 

        <asp:Panel ID="uploadPanel" runat="server">

        <dx:ASPxUploadControl ID="uplImage" runat="server" ClientInstanceName="uploader" ShowProgressPanel="True"
            NullText="..." Size="35" 
            OnFileUploadComplete="ASPxUploadPhotoControl_FileUploadComplete">
            <ClientSideEvents 
                FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }"
                FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }"
                TextChanged="function(s, e) { UpdateUploadButton(); }">
            </ClientSideEvents>
            <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".jpg,.jpeg,.jpe,.gif">
            </ValidationSettings>
            <AdvancedModeSettings EnableMultiSelect="true" />
        </dx:ASPxUploadControl>               

        <dx:ASPxButton ID="btnUpload" ClientInstanceName="btnUpload" runat="server" Text="Завантажити" AutoPostBack="false">
        <ClientSideEvents Click="function(s, e)
        {
            uploader.Upload();
        }" />
        </dx:ASPxButton>

        </asp:Panel>
                                    </td>
                                    </tr>
                                    </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                                </dx:ASPxRoundPanel>


                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

    </TabPages>
</dx:ASPxPageControl>
</td>
</tr>

</table>
</asp:Content>
