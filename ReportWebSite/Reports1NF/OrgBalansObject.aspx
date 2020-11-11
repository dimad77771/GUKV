<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgBalansObject.aspx.cs" Inherits="Reports1NF_OrgBalansObject"
    MasterPageFile="~/NoMenu.master" Title="Картка Об'єкту на Балансі" EnableViewState="true" %>

<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>
<%@ Register src="ReportCommentViewer.ascx" tagname="ReportCommentViewer" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<style type="text/css">
    .SpacingPara
    {
        font-size: 10px;
        margin-top: 4px;
        margin-bottom: 0px;
        padding-top: 0px;
        padding-bottom: 0px;
    }

    .editForm999 .dxgvEditFormTable_DevEx 
    {
         width:1800px !important;
    }

    .dxflGroup_DevEx td.dxflCaptionCellSys
    {
        width: 550px !important;
        min-width: 550px !important;
    }


    .dxflCaptionCell_DevEx
    {
        white-space: normal !important;
    }

    a.dxbButton_DevEx {
        margin:0px !important;
    }

</style>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    var lastFocusedControlId = "";
    var lastFocusedControlTitle = "";

	function ShowBalansArchiveCard(archiveId) {

		PopupArchiveStates.Hide();

		var cardUrl = "../Cards/BalansCardArchive.aspx?arid=" + archiveId;

		window.open(cardUrl);
	}



    function EnableBtiDocumentationControls(s, e) {

//        var condition = ComboObjBTICondition.GetValue();
        var condition = 1;

        // If condition has any value other that 'Documentation exists', do not validate
        if (condition != null && condition != undefined && condition == 1) {
            EditCaseNumber.SetEnabled(true);
            EditObjBtiDate.SetEnabled(true);
        }
        else {
            EditCaseNumber.SetEnabled(false);
            EditObjBtiDate.SetEnabled(false);
        }
    }

    function PerformAllValidations() {

        EditBuildingSqrTotalComment.SetIsValid(true);
        EditBuildingSqrNonHabitComment.SetIsValid(true);
        EditBuildingSqrHabitComment.SetIsValid(true);
        EditObjSqrTotalComment.SetIsValid(true);
        EditObjSqrKorComment.SetIsValid(true);
        EditObjSqrFreeComment.SetIsValid(true);

        var errorsFound = false;
        var activeTabIndex = CardPageControl.GetActiveTabIndex();
        var tabPageCount = CardPageControl.GetTabCount();

        for (var i = 0; i < tabPageCount; i++) {

            CardPageControl.SetActiveTabIndex(i);

            if (!ASPxClientEdit.ValidateEditorsInContainer(CardPageControl.GetMainElement())) {
                errorsFound = true;
                break;
            }
        }

        if (!errorsFound) {
            CardPageControl.SetActiveTabIndex(activeTabIndex);
        }

        return !errorsFound;
    }

    function RemoveAllSpaces(s) {

        var res = "";

        for (var i = 0; i < s.length; i++) {

            if (("" + s[i]) != " ") {

                res = res + s[i];
            }
        }

        return res;
    }

    function IsLiteraPresent(s) {

        var upper = "" + s;
        upper = upper.toUpperCase();

        if (upper.indexOf("ЛІТ") >= 0) {
            return true;
        }

        return false;
    }

    function OnValidateObjectBtiCondition(s, e) {

        var litera = "" + EditBuildingNum2.GetText();
        litera = RemoveAllSpaces(litera);

//        var condition = ComboObjBTICondition.GetValue();
        var condition = 1;

        if (IsLiteraPresent(litera) > 0 && (condition == null || condition == undefined)) {
            e.isValid = false;
            e.errorText = "Необхідно заповнити поле 'Реєстраційний номер об'єкту нерухомого майна'";
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateObjectBtiInvNo(s, e) {

//        var condition = ComboObjBTICondition.GetValue();
          var condition = "1";
        // If condition has any value other that 'Documentation exists', do not validate
        if ("" + condition == "1") {
            var litera = "" + EditBuildingNum2.GetText();
            litera = RemoveAllSpaces(litera);

            var invNo = "" + EditCaseNumber.GetText();
            invNo = RemoveAllSpaces(invNo);

            if (IsLiteraPresent(litera) && invNo.length == 0) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Інвентаризаційний № справи'";
            }
            else {
                e.isValid = true;
            }
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateObjectBtiDate(s, e) {

//        var condition = ComboObjBTICondition.GetValue();
        var condition = "1";

        // If condition has any value other that 'Documentation exists', do not validate
        if ("" + condition == "1") {
            var litera = "" + EditBuildingNum2.GetText();
            litera = RemoveAllSpaces(litera);

            var invDate = EditObjBtiDate.GetValue();

            if (IsLiteraPresent(litera) && (invDate == null || invDate == undefined)) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Дата виготовлення технічного паспорту'";
            }
            else {
                e.isValid = true;
            }
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateBuildingSqrBasement(s, e) {

        if (CheckBasementExists.GetChecked()) {

            var val = EditBuildingSqrBasement.GetValue();

            if (val == null || val == undefined || val <= 0) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Площа підвалу'";
            }
            else {
                e.isValid = true;
            }
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateBuildingSqrLoft(s, e) {

        if (CheckLoftExists.GetChecked()) {

            var val = EditBuildingSqrLoft.GetValue();

            if (val == null || val == undefined || val <= 0) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Площа горища'";
            }
            else {
                e.isValid = true;
            }
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateEditObjFreeSquare(s, e) {

        if (CheckFreeSquareExists.GetChecked()) {

            var total = EditObjSqrTotal.GetValue();
            var val = EditObjFreeSquare.GetValue();

            if (val == null || val == undefined || val <= 0) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Реєстраційний номер об'єкту нерухомого майна'";
            }
/*            else {
                if (total == null || total == undefined) total = 0;
                if (val == null || val == undefined) val = 0;

                if (val > total) {
                    e.isValid = false;
                    e.errorText = 'Корисна площа вільного приміщення не може перевищувати загальну площу на балансі.';
                }
                else {
                    e.isValid = true;
                }
            } */
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateComboFreeSqrTechState(s, e) {

        if (CheckFreeSquareExists.GetChecked()) {

            var val = ComboFreeSqrTechState.GetValue();

            if (val == null || val == undefined) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Технічний стан вільного приміщення'";
            }
            else {
                e.isValid = true;
            }
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateEditObjFreeSqrLocation(s, e) {

        if (CheckFreeSquareExists.GetChecked()) {

            var val = EditObjFreeSqrLocation.GetValue();

            if (val == null || val == undefined) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Розташування вільного приміщення (поверх)'";
            }
            else {
                e.isValid = true;
            }
        }
        else {
            e.isValid = true;
        }
    }
    function EnsureNumeric() {
        var key = window.event.keyCode;
        if (key < 48 || key > 57) 
            window.event.returnValue = false;
    }

    // Object square validation

    var buildingTotalSqrInitial = 0;
    var buildingNonHabitSqrInitial = 0;
    var buildingHabitSqrInitial = 0;

    var objectTotalSqrInitial = 0;
    var objectKorSqrInitial = 0;
	var objectFreeSqrInitial = 0;

	function OnEditMiscAddrTextChanged(e, z) {
		console.log("OnEditMiscAddrTextChanged");
		console.log(e);
		console.log(z);
        <%--<%= EditMiscAddr.ClientID %>.UnselectAll();--%>
	}

	function OnEditMiscAddrValueChanged(e, z) {
		console.log("OnEditMiscAddrValueChanged");
		console.log(e);
		console.log(z);
    }


    // ]]>

</script>

    <script type="text/javascript" language="javascript">
        var imageIndex = 0;


//        function OnBeginCallback(s, e) {
//            //lp.Show();
//        }

//        function OnCallbackComplete(s, e) {
//            //ContentCallback.PerformCallback('refreshphoto2:');
//            ASPxClientUtils.PreventEvent(e);
//            //setUrlParam('tab', 5);
//        }

//        function OnPrevBtnClick(e) {
//            PrevImage();
//            ASPxClientUtils.PreventEvent(e);
//        }

//        function OnNextBtnClick(e) {
//            NextImage();
//            ASPxClientUtils.PreventEvent(e);
//        }

//        function NextImage() {
//            imageIndex++;
//            if (imageIndex == GetCount())
//                imageIndex = 0;
//            cb.PerformCallback(imageIndex);
//        }

//        function PrevImage() {
//            imageIndex--;
//            if (imageIndex < 0)
//                imageIndex = GetCount() - 1;
//            cb.PerformCallback(imageIndex);
//        }

//        function GetCount() {
//            return imageGallery.cpCount;
//        }

//        function OnDeleteBtnClick(e) {
//            DeleteImage();
//            lp.Hide();
//            ASPxClientUtils.PreventEvent(e);
//        }

        function DeleteImage(index) {
            var r = confirm("Ви дійсно хочете видалити фото?");
            if (r == true) {
                ContentCallback.PerformCallback('deleteimage:' + index);
                ASPxCallbackPanelImageGallery.PerformCallback('ASPxCallbackPanelImageGallery:');
                //ASPxClientUtils.PreventEvent(e);
            }
        }

        function EditImage(index) {
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
            ASPxCallbackPanelImageGallery.PerformCallback('refreshphoto:');


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

            if (grid.IsEditing()) {
				var popup = s.GetEditFormTable();
                $(felm__tmp1.mainElement).hide();
                $(felm__tmp2.mainElement).hide();
                $(felm__tmp3.mainElement).hide();
				$(popup).find(".dxflCommandItemSys").css("margin-left", "0px");

                var err_object = $("#MainContent_CPMainPanel_CardPageControl_ASPxRoundPanel1_ASPxGridViewFreeSquare_DXEditingErrorItem");
				console.log("err_object", err_object);
                if (err_object.length > 0) {
                    var err_text = err_object.text();
                    if (err_text == null) err_text = "";
					if (err_text.indexOf("Об'єкт погоджено орендодавцем! Усі зміни ТІЛЬКИ з його дозволу за тел: 202-61-76, 202-61-77, 202-61-96 !") >= 0) {
						var save_image = $(popup).find(".dxflCommandItemSys").find("a").first();
                        save_image.hide();
                    }
				}

				felm__include_in_perelik.ValueChanged.AddHandler(OnEditFormTableItemChange);
                felm__prop_srok_orands.LostFocus.AddHandler(OnEditFormTableItemChange);

                felm__floor.tooltip = "Характеристика об’єкта оренди (будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі (надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів)";
                $(felm__floor.GetInputElement()).attr('title', "Характеристика об’єкта оренди (будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі (надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів)");
                $(popup).find("label").each(function (index) {
                    if ($(this).text() == "Характеристика об’єкта оренди") {
						$(this).attr('title', "Характеристика об’єкта оренди (будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі (надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів)");
					}
				});

                CustomizeEditFormTable();
            } 

            if (editFreeSquareMode) {
                if (!grid.IsEditing()) {
					window.location = "Report1NFFreeSquare.aspx";
                }
			}
        }

		function OnEditFormTableItemChange() {
            CustomizeEditFormTable();
        }

        function CustomizeEditFormTable() {
            console.log("a");
            var include_in_perelik = felm__include_in_perelik.GetValue();
            var showrow1 = false;
			var showrow2 = false;
            if (include_in_perelik == "1") {
                showrow1 = true;
			} else if (include_in_perelik == "2") {
				showrow2 = true;
            }

            var popup = grid.GetEditFormTable();
            var div = $(popup).find('.dxflFormLayout_DevEx');
            var dtable = $(div).children('table');
			var tbody = $(dtable).children('tbody');
            var rows = $(tbody).children('tr');
            var r1 = 9;
            var r2 = r1 + 1;
            if (showrow1) {
				$(rows.get(r1)).show();
            } else {
				$(rows.get(r1)).hide();
            }
			if (showrow2) {
				$(rows.get(r2)).show();
			} else {
				$(rows.get(r2)).hide();
            }


            var prop_srok_orands = felm__prop_srok_orands.GetValue();
            var prop = parseInt(prop_srok_orands, 10);
			var showrow1 = false;
            if (prop > 5) {
                showrow1 = true;
            }
            var r1 = 12;
			if (showrow1) {
				$(rows.get(r1)).show();
			} else {
				$(rows.get(r1)).hide();
			}
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
			//console.log(e.buttonID);
			if (e.buttonID == 'btnPhoto') {
				//console.log("$.cookie", $.cookie);
                $.cookie('RecordID', s.GetRowKey(e.visibleIndex));
                ASPxFileManagerPhotoFiles.Refresh();
                PopupObjectPhotos.Show();
			} else if (e.buttonID == 'btnPdfBuild') {
				grid.GetRowValues(e.visibleIndex, 'id', OnGridPdfBuildGetRowValues);
			} else if (e.buttonID == 'btnFreeCycle') {
				grid.GetRowValues(e.visibleIndex, 'id', OnFreeCycleGetRowValues);
			}
		}

		function OnGridPdfBuildGetRowValues(values) {
			var id = values;
			window.open(
				'BalansFreeSquarePhotosPdf.aspx?rid=' + paramRid + '&bid=' + paramBid + '&id=' + id,
				'_blank',
			);
		}

		function OnFreeCycleGetRowValues(values) {
			var id = values;
			window.open(
				'FreeCycle.aspx?free_square_id=' + id,
				'_blank',
			);
        }

		var paramRid = <%= ParamRid %>;
        var paramBid = <%= ParamBid %>;
        var editFreeSquareMode = <%= EditFreeSquareMode.ToString().ToLower() %>;
	</script>



</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server" EnableViewState="true">

<asp:ScriptManager ID="ScriptManager1" runat="server" />

<mini:ProfiledSqlDataSource ID="SqlDataSourceBuilding" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 b.* FROM reports1nf_balans bal INNER JOIN reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
        WHERE bal.id = @bal_id AND bal.report_id = @rep_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansObject" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 bal.* FROM reports1nf_balans bal WHERE bal.id = @bal_id AND bal.report_id = @rep_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansObjectStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
        SelectCommand="SELECT CASE WHEN submit_date IS NULL OR modify_date IS NULL OR modify_date > submit_date THEN 0 ELSE 1 END AS 'record_status', validation_errors, is_valid FROM reports1nf_balans WHERE id = @bal_id AND report_id = @rep_id" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDistrict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_districts2 ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceStreet" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_streets where (name is not null) and (RTRIM(LTRIM(name)) <> '') order by name">
<%--    SelectCommand="select s.id, s.name as sname, r.name as rname, ISNULL(r.name + ' - ', '') + s.name as name from dict_streets s left join dict_regions r on r.id = s.region_id where (not s.name is null) and (RTRIM(LTRIM(s.name)) <> '')">    --%>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFacade" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_facade">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjKind" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, LEFT(code + '-'+name, 100) as name FROM dict_1nf_object_kind order by code">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_object_type">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceTechState" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_tech_state">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceTechStane" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_tech_stane">
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourcePeriodUsed" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_period_used">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePowerInfo" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_power_info union select null, ''">
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourceUsingPossible" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, left(full_name, 150) as name, rental_rate FROM dict_rental_rate ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceInvestSolution" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_invest_solution union select null, ''">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceHistory" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_history">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDocKind" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_doc_kind ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePurpose" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_balans_purpose ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePurposeGroup" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_balans_purpose_group ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFormOwnership" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_ownership ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOwnershipType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, RTRIM(name) AS 'name' FROM dict_balans_ownership_type ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBTICondition" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_balans_bti ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_balans_obj_status ORDER BY id">
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
            FROM reports1nf_photos WHERE bal_id = @bal_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_id" />
        <asp:Parameter DbType="String" DefaultValue="0" Name="folder_prefix" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceZgodaRenter" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_zgoda_renter ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceIncludeInPerelik" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT '1' id, '1' name, 1 as ordrow union SELECT '2' id, '2' name, 1 as ordrow union select null, '',  2 as ordrow ORDER BY ordrow, name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeObjectType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_free_object_type ORDER BY id">
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



<asp:ObjectDataSource ID="ObjectDataSourceBalansPhoto" runat="server" SelectMethod="SelectFromTempFolder" 
    TypeName="ExtDataEntry.Models.FileAttachment">
    <SelectParameters>
        <asp:Parameter DefaultValue="1NF" Name="scope" Type="String" />
        <asp:Parameter DefaultValue="" Name="recordID" Type="Int32" />
        <asp:Parameter DefaultValue="" Name="tempGuid" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeSquare" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT [id]
      ,[balans_id]
      ,[total_free_sqr]
      ,[free_sqr_korysna]
      ,[free_sqr_condition_id]
	  ,[period_used_id]
      ,[floor]
      ,[possible_using]
	  ,[using_possible_id]
      ,[water]
	  ,[heating]
      ,[power_info_id]
      ,[gas]
      ,[note]
      ,[modify_date]
      ,[modified_by]
      ,[report_id] 
      ,[is_solution] 
	  ,[invest_solution_id] 
      ,[initiator] 
      ,[zgoda_control_id] 
      ,[free_object_type_id] 
      ,[zgoda_renter_id] 
	  ,[is_included] 	
	  ,[komis_protocol] 	
      ,[include_in_perelik]
      ,[zal_balans_vartist]
      ,[perv_balans_vartist]  
      ,[punkt_metod_rozrahunok] 
      ,[prop_srok_orands] 
      ,[nomer_derzh_reestr_neruh] 
      ,[reenum_derzh_reestr_neruh] 
      ,[info_priznach_nouse] 
      ,[info_rahunok_postach] 
      ,[priznach_before] 
      ,[period_nouse] 
      ,[osoba_use_before]
      ,[primitki]
      ,(select Q.form_of_ownership from view_reports1nf Q where Q.report_id = [reports1nf_balans_free_square].report_id) as form_of_ownership 
    FROM [reports1nf_balans_free_square] WHERE [balans_id] = @balans_id and [report_id] = @report_id and ([id] = @free_square_id or @free_square_id = -1)" 
    DeleteCommand="EXEC [delete_reports1nf_balans_free_square] @id" 
    InsertCommand="INSERT INTO [reports1nf_balans_free_square]
    ([balans_id]
      ,[total_free_sqr]
      ,[free_sqr_korysna]
      ,[free_sqr_condition_id]
	  ,[period_used_id]
      ,[floor]
      ,[possible_using]
	  ,[using_possible_id]
      ,[water]
      ,[heating]
      ,[power_info_id]
      ,[gas]
      ,[note]
      ,[modify_date]
      ,[modified_by]
      ,[report_id]
      ,[is_solution] 
	  ,[invest_solution_id]  
      ,[initiator] 
      ,[zgoda_control_id] 
      ,[free_object_type_id] 
      ,[zgoda_renter_id]
	  ,[is_included]
      ,[include_in_perelik]
      ,[zal_balans_vartist]
      ,[perv_balans_vartist]  
      ,[punkt_metod_rozrahunok] 
      ,[prop_srok_orands] 
      ,[nomer_derzh_reestr_neruh] 
      ,[reenum_derzh_reestr_neruh] 
      ,[info_priznach_nouse] 
      ,[info_rahunok_postach]   
      ,[priznach_before]   
      ,[period_nouse]
      ,[osoba_use_before]
      ,[primitki]
    ) 
    VALUES
    (@balans_id
      ,@total_free_sqr
      ,@free_sqr_korysna
      ,@free_sqr_condition_id
	  ,@period_used_id
      ,@floor
      ,@possible_using
	  ,@using_possible_id
      ,@water
      ,@heating
      ,@power_info_id
      ,@gas
      ,@note
      ,@modify_date
      ,@modified_by
      ,@report_id
      ,@is_solution
	  ,@invest_solution_id
      ,@initiator
      ,@zgoda_control_id
      ,@free_object_type_id
      ,@zgoda_renter_id
	  ,@is_included
      ,@include_in_perelik
      ,@zal_balans_vartist
      ,@perv_balans_vartist  
      ,@punkt_metod_rozrahunok 
      ,@prop_srok_orands 
      ,@nomer_derzh_reestr_neruh 
      ,@reenum_derzh_reestr_neruh 
      ,@info_priznach_nouse 
      ,@info_rahunok_postach       
      ,@priznach_before       
      ,@period_nouse
      ,@osoba_use_before
      ,@primitki
    );
SELECT SCOPE_IDENTITY()" 
    UpdateCommand="UPDATE [reports1nf_balans_free_square]
SET
    [balans_id] = @balans_id,
    [total_free_sqr] = @total_free_sqr,
    [free_sqr_korysna] = @free_sqr_korysna,
    [free_sqr_condition_id] = @free_sqr_condition_id,
	[period_used_id] = @period_used_id, 
    [floor] = @floor,
    [possible_using] = @possible_using,
	[using_possible_id] = @using_possible_id,  
    [water] = @water,
    [heating] = @heating,
    [power_info_id] = @power_info_id,
    [gas] = @gas,
    [note] = @note,
    [modify_date] = @modify_date,
    [modified_by] = @modified_by,
    [report_id] = @report_id
      ,[is_solution] = @is_solution
	  ,[invest_solution_id] = @invest_solution_id
      ,[initiator] = @initiator
      ,[zgoda_control_id] = @zgoda_control_id
      ,[free_object_type_id] = @free_object_type_id
      ,[zgoda_renter_id] = @zgoda_renter_id 
	  ,[is_included] = @is_included 
      ,[include_in_perelik] = @include_in_perelik 
        ,[zal_balans_vartist]		  = @zal_balans_vartist
        ,[perv_balans_vartist]  	  = @perv_balans_vartist  
        ,[punkt_metod_rozrahunok] 	  = @punkt_metod_rozrahunok 
        ,[prop_srok_orands] 		  = @prop_srok_orands 
        ,[nomer_derzh_reestr_neruh] 	  = @nomer_derzh_reestr_neruh 
        ,[reenum_derzh_reestr_neruh] 	  = @reenum_derzh_reestr_neruh 
        ,[info_priznach_nouse] 		  = @info_priznach_nouse 
        ,[info_rahunok_postach]  	  = @info_rahunok_postach   
        ,[priznach_before]  	  = @priznach_before   
        ,[period_nouse]  	  = @period_nouse   
        ,[osoba_use_before]  	  = @osoba_use_before   
        ,[primitki]  	  = @primitki     
WHERE id = @id" 
        oninserting="SqlDataSourceFreeSquare_Inserting" 
        onupdating="SqlDataSourceFreeSquare_Updating" ProviderName="System.Data.SqlClient">
    <SelectParameters>
        <asp:Parameter Name="balans_id" />
        <asp:Parameter Name="report_id" />
        <asp:Parameter Name="free_square_id" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="id" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="balans_id" />
        <asp:Parameter Name="total_free_sqr" />
        <asp:Parameter Name="free_sqr_korysna" />
        <asp:Parameter Name="free_sqr_condition_id" />
		<asp:Parameter Name="period_used_id" />
        <asp:Parameter Name="floor" />
        <asp:Parameter Name="possible_using" />
		<asp:Parameter Name="using_possible_id" />
        <asp:Parameter Name="water" />
        <asp:Parameter Name="heating" />
        <asp:Parameter Name="power_info_id" />
        <asp:Parameter Name="gas" />
        <asp:Parameter Name="note" />
        <asp:Parameter Name="modify_date" />
        <asp:Parameter Name="modified_by" />
        <asp:Parameter Name="report_id" />
        <asp:Parameter Name="is_solution" />
		<asp:Parameter Name="invest_solution_id" />
        <asp:Parameter Name="initiator" />
        <asp:Parameter Name="zgoda_control_id" />
        <asp:Parameter Name="free_object_type_id" />
        <asp:Parameter Name="zgoda_renter_id" />
		<asp:Parameter Name="is_included" />		
        <asp:Parameter Name="include_in_perelik" />
        <asp:Parameter Name="zal_balans_vartist" />
        <asp:Parameter Name="perv_balans_vartist" />
        <asp:Parameter Name="punkt_metod_rozrahunok" />
        <asp:Parameter Name="prop_srok_orands" />
        <asp:Parameter Name="nomer_derzh_reestr_neruh" />
        <asp:Parameter Name="reenum_derzh_reestr_neruh" />
        <asp:Parameter Name="info_priznach_nouse" />
        <asp:Parameter Name="info_rahunok_postach" />
        <asp:Parameter Name="priznach_before" />
        <asp:Parameter Name="period_nouse" />
        <asp:Parameter Name="osoba_use_before" />
        <asp:Parameter Name="primitki" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="balans_id" />
        <asp:Parameter Name="total_free_sqr" />
        <asp:Parameter Name="free_sqr_korysna" />
        <asp:Parameter Name="free_sqr_condition_id" />
		<asp:Parameter Name="period_used_id" />
        <asp:Parameter Name="floor" />
        <asp:Parameter Name="possible_using" />
		<asp:Parameter Name="using_possible_id" />
        <asp:Parameter Name="water" />
        <asp:Parameter Name="heating" />
        <asp:Parameter Name="power_info_id" />
        <asp:Parameter Name="gas" />
        <asp:Parameter Name="note" />
        <asp:Parameter Name="modify_date" />
        <asp:Parameter Name="modified_by" />
        <asp:Parameter Name="report_id" />
        <asp:Parameter Name="is_solution" />
		<asp:Parameter Name="invest_solution_id" />
        <asp:Parameter Name="initiator" />
        <asp:Parameter Name="zgoda_control_id" />
        <asp:Parameter Name="free_object_type_id" />
        <asp:Parameter Name="zgoda_renter_id" />
		<asp:Parameter Name="is_included" />	
        <asp:Parameter Name="include_in_perelik" />	
        <asp:Parameter Name="zal_balans_vartist" />
        <asp:Parameter Name="perv_balans_vartist" />
        <asp:Parameter Name="punkt_metod_rozrahunok" />
        <asp:Parameter Name="prop_srok_orands" />
        <asp:Parameter Name="nomer_derzh_reestr_neruh" />
        <asp:Parameter Name="reenum_derzh_reestr_neruh" />
        <asp:Parameter Name="info_priznach_nouse" />
        <asp:Parameter Name="info_rahunok_postach" />
        <asp:Parameter Name="priznach_before" />
        <asp:Parameter Name="period_nouse" />
        <asp:Parameter Name="osoba_use_before" />
        <asp:Parameter Name="primitki" />
        <asp:Parameter Name="id" />
    </UpdateParameters>
</mini:ProfiledSqlDataSource>



<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Cabinet.aspx" Text="Стан"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgInfo.aspx" Text="Загальна Інформація"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansList.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansDeletedList.aspx" Text="Відчужені Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgArendaList.aspx" Text="Договори використання приміщень "></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgRentedList.aspx" Text="Договори Орендування"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/Logout.aspx" Text="Вийти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/ChangePasswordNoMenu.aspx" Text="Пароль"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="http://kmda.iisd.com.ua" Text="Класифікатор майна"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Картка об'єкту на балансі" CssClass="pagetitle"/>
</p>



<dx:ASPxCallbackPanel ID="CPMainPanel" ClientInstanceName="CPMainPanel" 
        runat="server" OnCallback="CPMainPanel_Callback" onunload="CPMainPanel_Unload">
    <PanelCollection>
        <dx:PanelContent ID="Panelcontent2" runat="server">

<asp:FormView runat="server" BorderStyle="None" ID="StatusForm" DataSourceID="SqlDataSourceBalansObjectStatus" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Необхідно надіслати внесені зміни до ДКВ" ClientVisible='<%# 0.Equals(Eval("record_status")) %>' ForeColor="Red" />
        <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Зміни надіслано до ДКВ" ClientVisible='<%# 1.Equals(Eval("record_status")) %>' />
         <asp:Label runat="server" ID="LabeldESTfOLDER"  CssClass="pagetitle"/>
<%--        <asp:Panel ID="Panel1" runat="server" Visible='<%# 0.Equals(Eval("is_valid")) %>'>
            <div style="color:Red;padding:10px">
                <div style="font-weight:bold">Помилки, які потрібно виправити перед надсиланням до ДКВ:</div>
                <div><%# Eval("validation_errors") %></div>
            </div>
        </asp:Panel>--%>

    </ItemTemplate>
</asp:FormView>

<asp:HiddenField EnableViewState="true" ID="UniqieState" runat="server" />

<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" 
                runat="server" ActiveTabIndex="0" >
    <TabPages>

        <dx:TabPage Text="Характеристика будинку" Name="Tab1">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server">

                    <asp:FormView runat="server" BorderStyle="None" ID="AddressForm" DataSourceID="SqlDataSourceBuilding" EnableViewState="False">
                        <ItemTemplate>

                            <dx:ASPxRoundPanel ID="PanelAddress" runat="server" HeaderText="Адреса будинку">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent4" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="990px">
                                            <tr>
                                                <td> <dx:ASPxLabel ID="ASPxLabel40" runat="server" Text="Район" Width="125px"/> </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboAddrDistrict" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDistrict" Value='<%# Eval("addr_distr_new_id") %>' 
                                                        Title="Адреса будинку - Район" />
                                                </td>
                                                <td> <dx:ASPxLabel ID="ASPxLabel41" runat="server" Text="Назва вулиці" Width="125px" /> </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboAddrStreet" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceStreet" Value='<%# Eval("addr_street_id") %>'
                                                        Title="Адреса будинку - Назва вулиці" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td> <dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Номер будинку, літери" Width="125px"/> </td>
                                                <td>
                                                    <table border="0" cellspacing="0" cellpadding="0" width="270px">
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxTextBox ID="EditBuildingNum1" runat="server" Text='<%# EvaluateTrimStr(Eval("addr_nomer1")) %>' Width="70px" 
                                                                    Title="Адреса - Номер будинку" MaxLength="9" />
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="EditBuildingNum2" ClientInstanceName="EditBuildingNum2" runat="server" Text='<%# EvaluateTrimStr(Eval("addr_nomer2")) %>' Width="200px"
                                                                    Title="Адреса - Номер будинку (літери)" MaxLength="18" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td> <dx:ASPxLabel ID="ASPxLabel38" runat="server" Text="Корпус"/> </td>
                                                <td> <dx:ASPxTextBox ID="EditBuildingNum3" runat="server" Text='<%# EvaluateTrimStr(Eval("addr_nomer3")) %>' Width="270px" Title="Адреса - Номер будинку (корпус)" MaxLength="10"/> </td>
                                            </tr>
                                            <tr>
                                                <td> <dx:ASPxLabel ID="ASPxLabel42" runat="server" Text="Додаткова адреса"/> </td>
                                                <td> 
													<dx:ASPxTextBox ID="EditMiscAddr" runat="server" Text='<%# EvaluateTrimStr(Eval("addr_misc")) %>' Width="270px" Title="Адреса будинку - Додаткова адреса" MaxLength="100">
														<ClientSideEvents TextChanged="OnEditMiscAddrTextChanged" ValueChanged="OnEditMiscAddrValueChanged" />
													</dx:ASPxTextBox> 
                                                </td>
                                                <td> <dx:ASPxLabel ID="ASPxLabel43" runat="server" Text="Поштовий індекс"/> </td>
                                                <td> <dx:ASPxTextBox ID="EditZipCode" runat="server" Text='<%# EvaluateTrimStr(Eval("addr_zip_code")) %>' Width="270px" Title="Адреса будинку - Поштовий індекс" MaxLength="10" /> </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelBuildingProps" runat="server" HeaderText="Характеристики будинку">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="990px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Рік будівництва" Width="125px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditBuildYear" runat="server" NumberType="Integer" Value='<%# Eval("construct_year") %>' Width="270px" Title="Рік будівництва будинку" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Рік здачі в експлуатацію" Width="125px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditExplYear" runat="server" NumberType="Integer" Value='<%# Eval("expl_enter_year") %>' Width="270px" Title="Рік здачі будинку в експлуатацію" /></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Фасадний / Не фасадний"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboBuildingFacade" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceFacade" Value='<%# Eval("facade_id") %>'
                                                        Title="Фасадний / Не фасадний будинок" />
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Кількість поверхів загальна"></dx:ASPxLabel></td>
                                                <td><dx:ASPxTextBox ID="EditBuildingNumFloors" runat="server" Text='<%# Eval("num_floors") %>' Width="270px" Title="Загальна кількість поверхів будинку" MaxLength="18" /></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Вид будинку"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboBuildingKind" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceObjKind" Value='<%# Eval("object_kind_id") %>'
                                                        Title="Вид будинку" />
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Тип будинку"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboBuildingType" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceObjType" Value='<%# Eval("object_type_id") %>'
                                                        Title="Тип будинку" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Технічний стан"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboBuildingTechState" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceTechState" Value='<%# Eval("tech_condition_id") %>'
                                                        Title="Технічний стан будинку" />
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Пам'ятка архітектури (історії)"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboBuildingHistory" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceHistory" Value='<%# Eval("history_id") %>'
                                                        Title="Будинок - Пам'ятка архітектури (історії)" />
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelBuildingSquare" runat="server" HeaderText="Площі будинку, що використовуються балансоутримувачем">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="990px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Загальна площа, кв.м" Width="125px"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditBuildingSqrTotal" ClientInstanceName="EditBuildingSqrTotal" runat="server" NumberType="Float" Value='<%# Eval("sqr_total") %>' Width="270px" Title="Загальна площа будинку" ReadOnly ="true">
                                                        <ValidationSettings Display="None" EnableCustomValidation="true" ></ValidationSettings>
                                                        <ClientSideEvents
                                                            Init="function (s,e) { buildingTotalSqrInitial = s.GetValue(); }"
                                                            NumberChanged="function (s,e) {
                                                                var val = s.GetValue();
                                                                EditBuildingSqrTotalComment.SetVisible(val != null && val != undefined && (val - buildingTotalSqrInitial >= 20 || val - buildingTotalSqrInitial <= -20));
                                                                LabelBuildingSqrTotalComment.SetVisible(val != null && val != undefined && (val - buildingTotalSqrInitial >= 20 || val - buildingTotalSqrInitial <= -20));
                                                                e.processOnServer = false; }"
                                                            />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxLabel ID="LabelBuildingSqrTotalComment" ClientInstanceName="LabelBuildingSqrTotalComment" Width="100%" ClientVisible="false"
                                                        runat="server" Text="Будь ласка, вкажіть причину зміни загальної площи будинку, та реквізити підтверджуючого документу"/>

                                                    <dx:ASPxTextBox ID="EditBuildingSqrTotalComment" ClientInstanceName="EditBuildingSqrTotalComment" runat="server" Width="100%"
                                                        Title="Коментар щодо зміни загальної площі будинку" ClientVisible="false">
                                                        <ClientSideEvents Validation="function (s,e) { if (s.GetVisible() && s.GetText().length == 0) { e.isValid = false; e.errorText = 'Необхідно заповнити коментар щодо зміни загальної площі будинку'; } else { e.isValid = true; } }" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel39" runat="server" Text="Площа нежилих приміщень, кв.м" Width="125px"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditBuildingSqrNonHabit" ClientInstanceName="EditBuildingSqrNonHabit" runat="server" NumberType="Float" Value='<%# Eval("sqr_non_habit") %>' Width="270px" Title="Площа нежилих приміщень будинку">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <ClientSideEvents
                                                            Init="function (s,e) { buildingNonHabitSqrInitial = s.GetValue(); }"
                                                            Validation="function (s,e) {
                                                                var total = EditBuildingSqrTotal.GetValue();
                                                                var val = s.GetValue();

                                                                if (total == null || total == undefined) total = 0;
                                                                if (val == null || val == undefined) val = 0;

                                                                if (val > total) { e.isValid = false; e.errorText = 'Площа нежилих приміщень будинку не може перевищувати загальну площу будинку.'; } else { e.isValid = true; }
                                                                }"
                                                            LostFocus="function (s,e) {
                                                                var val = s.GetValue();
                                                              /* 
                                                               EditBuildingSqrNonHabitComment.SetVisible(val != null && val != undefined && (val - buildingNonHabitSqrInitial >= 20 || val - buildingNonHabitSqrInitial <= -20));
                                                                LabelBuildingSqrNonHabitComment.SetVisible(val != null && val != undefined && (val - buildingNonHabitSqrInitial >= 20 || val - buildingNonHabitSqrInitial <= -20));
                                                                e.processOnServer = false; 
                                                              */
                                                                var habit = EditBuildingSqrHabit.GetValue();
                                                                var pidval = EditBuildingSqrBasement.GetValue();
                                                                val = val+habit+pidval;
                                                                EditBuildingSqrTotal.SetValue(val);
                                                                                           }"
                                                            />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Площа житлового фонду, кв.м"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditBuildingSqrHabit" ClientInstanceName="EditBuildingSqrHabit" runat="server" NumberType="Float" Value='<%# Eval("sqr_habit") %>' Width="270px" Title="Площа житлового фонду будинку">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <ClientSideEvents
                                                            Init="function (s,e) { buildingHabitSqrInitial = s.GetValue(); }"
                                                            Validation="function (s,e) {
                                                                var total = EditBuildingSqrTotal.GetValue();
                                                                var val = s.GetValue();

                                                                if (total == null || total == undefined) total = 0;
                                                                if (val == null || val == undefined) val = 0;

                                                                if (val > total) { e.isValid = false; e.errorText = 'Площа житлового фонду будинку не може перевищувати загальну площу будинку.'; } else { e.isValid = true; }
                                                                }"
                                                            LostFocus="function (s,e) {
                                                                var val = s.GetValue();
                                                              /* 
                                                                EditBuildingSqrHabitComment.SetVisible(val != null && val != undefined && (val - buildingHabitSqrInitial >= 20 || val - buildingHabitSqrInitial <= -20));
                                                                LabelBuildingSqrHabitComment.SetVisible(val != null && val != undefined && (val - buildingHabitSqrInitial >= 20 || val - buildingHabitSqrInitial <= -20));
                                                                e.processOnServer = false;
                                                              */
                                                                var nonhabit = EditBuildingSqrNonHabit.GetValue();
                                                                var pidval = EditBuildingSqrBasement.GetValue();
                                                                val = val+nonhabit+pidval;
                                                                EditBuildingSqrTotal.SetValue(val);
                                                                                          }"
                                                            />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxLabel ID="LabelBuildingSqrNonHabitComment" ClientInstanceName="LabelBuildingSqrNonHabitComment" Width="100%" ClientVisible="false"
                                                        runat="server" Text="Будь ласка, вкажіть причину зміни нежитлової площи будинку, та реквізити підтверджуючого документу"/>

                                                    <dx:ASPxTextBox ID="EditBuildingSqrNonHabitComment" ClientInstanceName="EditBuildingSqrNonHabitComment" runat="server" Width="100%"
                                                        Title="Коментар щодо зміни нежитлової площі будинку" ClientVisible="false">
                                                        <ClientSideEvents Validation="function (s,e) { if (s.GetVisible() && s.GetText().length == 0) { e.isValid = false; e.errorText = 'Необхідно заповнити коментар щодо зміни нежитлової площі будинку'; } else { e.isValid = true; } }" />
                                                    </dx:ASPxTextBox>

                                                    <dx:ASPxLabel ID="LabelBuildingSqrHabitComment" ClientInstanceName="LabelBuildingSqrHabitComment" Width="100%" ClientVisible="false"
                                                        runat="server" Text="Будь ласка, вкажіть причину зміни житлової площи будинку, та реквізити підтверджуючого документу"/>

                                                    <dx:ASPxTextBox ID="EditBuildingSqrHabitComment" ClientInstanceName="EditBuildingSqrHabitComment" runat="server" Width="100%"
                                                        Title="Коментар щодо зміни житлової площі будинку" ClientVisible="false">
                                                        <ClientSideEvents Validation="function (s,e) { if (s.GetVisible() && s.GetText().length == 0) { e.isValid = false; e.errorText = 'Необхідно заповнити коментар щодо зміни житлової площі будинку'; } else { e.isValid = true; } }" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel45" runat="server" Text="Площа підвалу висотою 1,9м та вище, кв.м"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditBuildingSqrBasement" ClientInstanceName="EditBuildingSqrBasement" runat="server" NumberType="Float" Value='<%# Eval("sqr_pidval") %>' Width="270px" Title="Площа підвалу будинку">
                                                        <ValidationSettings Display="None"></ValidationSettings>
<%--                                                        <ClientSideEvents Validation="OnValidateBuildingSqrBasement" /> --%>
                                                        <ClientSideEvents
                                                            LostFocus="function (s,e) {
                                                                var val = s.GetValue();
                                                                var habit = EditBuildingSqrHabit.GetValue();
                                                                var nonhabit = EditBuildingSqrNonHabit.GetValue();
                                                                val = val+habit+nonhabit;
                                                                EditBuildingSqrTotal.SetValue(val);
                                                                                          }"
                                                            />

                                                    </dx:ASPxSpinEdit>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Площа горища, кв.м"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditBuildingSqrLoft" ClientInstanceName="EditBuildingSqrLoft" runat="server" NumberType="Float" Value='<%# Eval("sqr_loft") %>' Width="270px" Title="Площа горища будинку">
                                                        <ValidationSettings Display="None"></ValidationSettings>
<%--                                                        <ClientSideEvents Validation="OnValidateBuildingSqrLoft" /> --%>
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
<%--                                            <tr>
                                                <td colspan="2">
                                                    <dx:ASPxCheckBox ID="CheckBasementExists" ClientInstanceName="CheckBasementExists" runat="server" Text="Підвал існує" Value='<%# 1.Equals(Eval("is_basement_exists")) %>' Title="Підвал існує"  ClientVisible ="false">
                                                         <ClientSideEvents CheckedChanged="function (s,e) { EditBuildingSqrBasement.SetEnabled(CheckBasementExists.GetChecked()); }" /> 
                                                    </dx:ASPxCheckBox>
                                                </td>
                                                <td colspan="2">
                                                    <dx:ASPxCheckBox ID="CheckLoftExists" ClientInstanceName="CheckLoftExists" runat="server" Text="Горище існує" Value='<%# 1.Equals(Eval("is_loft_exists")) %>' Title="Горище існує"  ClientVisible ="false">
                                                        <ClientSideEvents CheckedChanged="function (s,e) { EditBuildingSqrLoft.SetEnabled(CheckLoftExists.GetChecked()); }" />  
                                                    </dx:ASPxCheckBox>
                                                </td>
                                            </tr>
--%>                            
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                        </ItemTemplate>
                    </asp:FormView>

                    <p class="SpacingPara"/>

                    <asp:FormView runat="server" BorderStyle="None" ID="FormViewState" DataSourceID="SqlDataSourceBalansObject" EnableViewState="False">
                        <ItemTemplate>
                            <dx:ASPxRoundPanel ID="StatePanel" runat="server" HeaderText="Стан">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="0px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent7" runat="server">
                                        <table border="0" cellspacing="0" cellpadding="0" width="990px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text='<%# EvaluateSignature(Eval("modified_by"), Eval("modify_date")) %>'></dx:ASPxLabel></td>

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

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Характеристика об'єкту на балансі" Name="Tab2">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl5" runat="server">

                    <asp:FormView runat="server" BorderStyle="None" ID="BalansObjForm" 
                        DataSourceID="SqlDataSourceBalansObject" EnableViewState="False" >
                        <ItemTemplate>

                            <dx:ASPxRoundPanel ID="PanelObjProps" runat="server" HeaderText="Характеристики об'єкту">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="990px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Вид об'єкту"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboObjKind" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="250px" 
                                                        IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceObjKind" Value='<%# Eval("object_kind_id") %>'
                                                        Title="Вид об'єкту" >
                                                        <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Характеристики об'єкту: необхідно вибрати вид об'єкту" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Тип об'єкту"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboObjType" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="250px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceObjType" Value='<%# Eval("object_type_id") %>'
                                                        Title="Тип об'єкту" >
                                                        <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Характеристики об'єкту: необхідно вибрати тип об'єкту" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Технічний стан"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboObjTechState" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="250px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceTechState" Value='<%# Eval("tech_condition_id") %>'
                                                        Title="Технічний стан об'єкту">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        </dx:ASPxComboBox>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Інвентарний номер об'єкту"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditObjBtiReestrNo" runat="server" Text='<%# Eval("reestr_no") %>' Width="250px" Title="Інвентарний номер об'єкту" MaxLength="18">
                                                        <ValidationSettings Display="None" EnableCustomValidation="true" ></ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Розташування приміщення (поверх)"></dx:ASPxLabel></td>
                                                <td><dx:ASPxTextBox ID="EditObjFloors" runat="server" Text='<%# Eval("floors") %>' Width="250px" Title="Розташування приміщення (поверх)" MaxLength="100" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel46" runat="server" Text="Поточне використання приміщення"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboObjStatus" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="250px" 
                                                        DataSourceID="SqlDataSourceObjStatus" Value='<%# Eval("obj_status_id") %>' Title="Поточне використання приміщення">
                                                        <ValidationSettings Display="None"  > <RequiredField IsRequired="false" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelObjSquare" runat="server" HeaderText="Площа об'єкту">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent5" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="990px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Площа нежилих приміщень об'єкту, кв.м"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditObjSqrTotal" ClientInstanceName="EditObjSqrTotal" runat="server" NumberType="Float" Value='<%# Eval("sqr_total") %>' Width="100px" Title="Площа нежилих приміщень об'єкту на балансі">
                                                        <ValidationSettings Display="None" > <RequiredField IsRequired="false"  /> </ValidationSettings>
                                                        <ClientSideEvents
                                                            Init="function (s,e) { objectTotalSqrInitial = s.GetValue(); }"
                                                            NumberChanged="function (s,e) {
                                                                var val = s.GetValue();
                                                                EditObjSqrTotalComment.SetVisible(val != null && val != undefined && (val - objectTotalSqrInitial >= 20 || val - objectTotalSqrInitial <= -20));
                                                                LabelObjSqrTotalComment.SetVisible(val != null && val != undefined && (val - objectTotalSqrInitial >= 20 || val - objectTotalSqrInitial <= -20));
                                                                e.processOnServer = false; }"
                                                            />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="- в т.ч. корисна площа об'єкту, кв.м"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditObjSqrKor" ClientInstanceName="EditObjSqrKor" runat="server" NumberType="Float" Value='<%# Eval("sqr_kor") %>' Width="100px" Title="Корисна площа об'єкту на балансі">
                                                        <ValidationSettings Display="Dynamic"></ValidationSettings>
                                                        <ClientSideEvents
                                                            Init="function (s,e) { objectKorSqrInitial = s.GetValue(); }"
                                                            Validation="function (s,e) {
                                                                var total = EditObjSqrTotal.GetValue();
                                                                var val = s.GetValue();

                                                                if (total == null || total == undefined) total = 0;
                                                                if (val == null || val == undefined) val = 0;

                                                                if (val > total) { e.isValid = false; e.errorText = 'Корисна площа не може перевищувати Площу нежилих приміщень об`єкту'; } else { e.isValid = true; 

                                                                EditObjSqrKorComment.SetVisible(val != null && val != undefined && (val - objectKorSqrInitial >= 20 || val - objectKorSqrInitial <= -20));
                                                                LabelObjSqrKorComment.SetVisible(val != null && val != undefined && (val - objectKorSqrInitial >= 20 || val - objectKorSqrInitial <= -20));
                                                                e.processOnServer = false;                                                        
                                                                        }
                                                                }"
                                                            />
<%--
                                                            NumberChanged="function (s,e) {
                                                                var val = s.GetValue();
                                                                EditObjSqrKorComment.SetVisible(val != null && val != undefined && (val - objectKorSqrInitial >= 20 || val - objectKorSqrInitial <= -20));
                                                                LabelObjSqrKorComment.SetVisible(val != null && val != undefined && (val - objectKorSqrInitial >= 20 || val - objectKorSqrInitial <= -20));
                                                                e.processOnServer = false; }"

                                                            />
 --%>
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxLabel ID="LabelObjSqrTotalComment" ClientInstanceName="LabelObjSqrTotalComment" Width="100%" ClientVisible="false"
                                                        runat="server" Text="Будь ласка, вкажіть причину зміни площі нежилих приміщень на балансі, та реквізити підтверджуючого документу"/>

                                                    <dx:ASPxTextBox ID="EditObjSqrTotalComment" ClientInstanceName="EditObjSqrTotalComment" runat="server" Width="100%"
                                                        Title="Коментар щодо зміни площі нежилих приміщень на балансі" ClientVisible="false">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <ClientSideEvents Validation="function (s,e) { if (s.GetVisible() && s.GetText().length == 0) { e.isValid = false; e.errorText = 'Необхідно заповнити коментар щодо зміни площі нежилих приміщень на балансі'; } else { e.isValid = true; } }" />
                                                    </dx:ASPxTextBox>

                                                    <dx:ASPxLabel ID="LabelObjSqrKorComment" ClientInstanceName="LabelObjSqrKorComment" Width="100%" ClientVisible="false"
                                                        runat="server" Text="Будь ласка, вкажіть причину зміни корисної площі нежилих приміщень на балансі, та реквізити підтверджуючого документу"/>

                                                    <dx:ASPxTextBox ID="EditObjSqrKorComment" ClientInstanceName="EditObjSqrKorComment" runat="server" Width="100%"
                                                        Title="Коментар щодо зміни корисної площі нежилих приміщень на балансі" ClientVisible="false">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <ClientSideEvents Validation="function (s,e) { if (s.GetVisible() && s.GetText().length == 0) { e.isValid = false; e.errorText = 'Необхідно заповнити коментар щодо зміни корисної площі нежилих приміщень на балансі'; } else { e.isValid = true; } }" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Площа об'єкту для власних потреб, кв.м"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjSqrVlasn" ClientInstanceName="EditObjSqrVlasn" runat="server" NumberType="Float" Value='<%# Eval("sqr_vlas_potreb") %>' Width="100px" Title="Площа для власних потреб" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel32" runat="server" Text="- в т.ч. техніко-інженерних потреб об'єкту, кв.м"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjSqrEng" ClientInstanceName="EditObjSqrEng" runat="server" NumberType="Float" Value='<%# Eval("sqr_engineering") %>' Width="100px" Title="Площа для техніко-інженерних потреб">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
<%--                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel33" runat="server" Text="Кількість договорів оренди"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjNumRentAgreements" runat="server" NumberType="Integer" Value='<%# Eval("num_rent_agr") %>' Width="100px" Title="Кількість договорів оренди" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="Площа надана в оренду, кв.м"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjSqrInRent" runat="server" NumberType="Float" Value='<%# Eval("sqr_in_rent") %>' Width="100px" Title="Площа надана в оренду" /></td>
                                            </tr>
--%>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelObjPurpose" runat="server" HeaderText="Призначення за проектною документацією">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent7" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="990px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Група призначення" width="200px"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboPurposeGroup" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="600px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourcePurposeGroup" Value='<%# Eval("purpose_group_id") %>'
                                                        Title="Група призначення">
                                                        <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Призначення за проектною документацією: необхідно вибрати групу призначення" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="Призначення за класифікацією" width="200px"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboPurpose" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="600px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourcePurpose" Value='<%# Eval("purpose_id") %>'
                                                        Title="Призначення за класифікацією">
                                                        <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Призначення за проектною документацією: необхідно вибрати призначення за класифікацією" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Призначення за уточненням балансоутримувача" width="200px"></dx:ASPxLabel></td>
                                                <td valign="top">
                                                    <dx:ASPxTextBox ID="EditPurposeStr" runat="server" Text='<%# Eval("purpose_str") %>' Width="600px" Title="Призначення за уточненням балансоутримувача" MaxLength="255">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                        </ItemTemplate>
                    </asp:FormView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Правова характеристика об'єкту" Name="Tab3">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server">

                    <asp:FormView runat="server" BorderStyle="None" ID="BalansDocsForm" DataSourceID="SqlDataSourceBalansObject" EnableViewState="False">
                        <ItemTemplate>

                            <dx:ASPxRoundPanel ID="PanelObjRights" runat="server" HeaderText="Права на об'єкт">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent4" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="990px">

                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Форма власності об’єкту"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboObjOwnership" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceFormOwnership" Value='<%# Eval("form_ownership_id") %>'
                                                        Title="Форма власності об’єкту">
                                                        <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Техніко-економічна характеристика об'єкту: необхідно вибрати форму власності об’єкту" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Право користування об’єктом"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboObjRight" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceOwnershipType" Value='<%# Eval("ownership_type_id") %>'
                                                        Title="Право користування об’єктом" />
                                                </td>
                                            </tr>

                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelOwnershipDoc" runat="server" HeaderText="Документ, що встановлює право користування об’єктом">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent3" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="0" width="990px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Тип документу:"></dx:ASPxLabel></td>
                                                <td>&nbsp; &nbsp;</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboOwnershipDocKind" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="350px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDocKind" Value='<%# Eval("ownership_doc_type") %>'
                                                        Title="Тип документу, що встановлює право користування об’єктом" >
                                                        <ValidationSettings Display="None"  > <RequiredField IsRequired="false" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                
                                                <td><dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Номер:"></dx:ASPxLabel></td>
                                                <td>&nbsp; &nbsp;</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditOwnershipDocNum" runat="server" Text='<%# Eval("ownership_doc_num") %>' Width="100px" Title="Номер документу, що встановлює право користування об’єктом" MaxLength="24" >
                                                        <ValidationSettings Display="None"  ></ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>

                                                <td><dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Від:"></dx:ASPxLabel></td>
                                                <td>&nbsp; &nbsp;</td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="EditOwnershipDocDate" runat="server" Value='<%# Eval("ownership_doc_date") %>' Width="100px" Title="Дата документу, що встановлює право користування об’єктом" >
                                                        <ValidationSettings Display="None"  ></ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelBalanspDoc" runat="server" HeaderText="Документ, що підтверджує передачу об’єкту на баланс">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent6" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="0" width="990px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Тип документу:"></dx:ASPxLabel></td>
                                                <td>&nbsp; &nbsp;</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboBalansDocKind" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="350px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDocKind" Value='<%# Eval("balans_doc_type") %>'
                                                        Title="Тип документу, що підтверджує передачу об’єкту на баланс" >
                                                        <ValidationSettings Display="None"  ></ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                
                                                <td><dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="Номер:"></dx:ASPxLabel></td>
                                                <td>&nbsp; &nbsp;</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditBalansDocNum" runat="server" Text='<%# Eval("balans_doc_num") %>' Width="100px" Title="Номер документу, що підтверджує передачу об’єкту на баланс" MaxLength="24" >
                                                        <ValidationSettings Display="None"  ></ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>

                                                <td><dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="Від:"></dx:ASPxLabel></td>
                                                <td>&nbsp; &nbsp;</td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="EditBalansDocDate" runat="server" Value='<%# Eval("balans_doc_date") %>' Width="100px" Title="Дата документу, що підтверджує передачу об’єкту на баланс" >
                                                        <ValidationSettings Display="None"  ></ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>


                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelObjBTI" runat="server" HeaderText="Документація БТІ">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent8" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="990px">

                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Інвентаризаційний № справи"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditCaseNumber" ClientInstanceName="EditCaseNumber" runat="server" 
                                                         Width="240px" Text='<%# EvaluateTrimStr(Eval("obj_bti_code")) %>' Title="Інвентаризаційний № справи" MaxLength="18">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td width="16px">&nbsp;</td>
               
                                                <td><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Дата виготовлення технічного паспорту"></dx:ASPxLabel></td>
                                                <td>&nbsp; </td>
                                                <td align="left">
                                                    <dx:ASPxDateEdit ID="EditObjBtiDate" ClientInstanceName="EditObjBtiDate" runat="server" Value='<%# Eval("date_bti") %>' Width="120px"
                                                        Title="Дата виготовлення технічного паспорту">
                                                        <ValidationSettings Display='None'></ValidationSettings>
                                                      <%--   <ClientSideEvents Validation="OnValidateObjectBtiDate" />  --%>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>

                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelFreeSquare" runat="server" HeaderText="Реєстрація у Державному реєстрі речових прав на нерухоме майно">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent12" runat="server">
                                        <table border="0" cellspacing="0" cellpadding="2" width="990px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Об'єкт нерухомого майна" width="150px"></dx:ASPxLabel></td>
                                                <td colspan="4">
                                                     <dx:ASPxTextBox ID="EditObjFreeSqrLocation" ClientInstanceName="EditObjFreeSqrLocation" runat="server" Text='<%# Eval("free_sqr_location") %>' Width="100%" MaxLength="100"
                                                        Title="Об'єкт нерухомого майна">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Номер запису про право власності"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditFreeSqrCondition" ClientInstanceName="EditFreeSqrCondition" runat="server" Width="250px"  MaxLength="10" 
                                                        Text='<%# Eval("free_sqr_condition_id") %>' 
                                                        Title="Номер запису про право власності">  
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                  <%--       <ClientSideEvents KeyDown="EnsureNumeric"/>    --%>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td width="8px">&nbsp;</td>
                                                <td><dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Реєстраційний номер об'єкту нерухомого майна"> </dx:ASPxLabel> </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditObjBTICondition" ClientInstanceName="EditObjBTICondition" runat="server" Width="250px" MaxLength="13"  
                                                        Text='<%# Eval("bti_id") %>'
                                                        Title="Реєстраційний номер об'єкту нерухомого майна">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                <%--        <ClientSideEvents KeyDown="function(s,e) {EnsureNumeric(s, e);}"/>    --%>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>


                        </ItemTemplate>
                    </asp:FormView>

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
                                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                            <tr>
                                                <td>
                                                

    <dx:ASPxGridView ID="ASPxGridViewFreeSquare" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceFreeSquare" KeyFieldName="id" OnRowValidating="ASPxGridViewFreeSquare_RowValidating" OnStartRowEditing="ASPxGridViewFreeSquare_StartRowEditing"
            ClientInstanceName="grid" oninitnewrow="ASPxGridViewFreeSquare_InitNewRow" >
            <ClientSideEvents CustomButtonClick="ShowPhoto" Init="OnInit" EndCallback="OnEndCallback" />
        <Styles>  
            <EditForm CssClass="editForm999" ></EditForm>  
        </Styles>  

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
            <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowInCustomizationForm="True" CellStyle-Wrap="True" Width="70px" 
                ShowDeleteButton="True" ShowCancelButton="true" ShowUpdateButton="true" ShowClearFilterButton="true" ShowEditButton="true" ShowNewButton="true" >
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnPhoto" Text="Фото"> 
						<Image Url="~/Styles/PhotoIcon.png"> </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="btnPdfBuild" Text="Pdf"> 
						<Image Url="~/Styles/PdfReportIcon.png"> </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="btnFreeCycle" Text="Картка процесу передачі в оренду вільного приміщення"> 
						<Image Url="~/Styles/ReportDocument18.png"> </Image>
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

            <dx:GridViewDataTextColumn FieldName="total_free_sqr" Caption="Загальна площа об’єкта, кв.м." VisibleIndex="3" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="free_sqr_korysna" Caption="Корисна площа об’єкта, кв.м." VisibleIndex="4" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataComboBoxColumn FieldName="free_sqr_condition_id" VisibleIndex="5" 
                Visible="True" Caption="Технічний стан об’єкта">
                <PropertiesComboBox DataSourceID="SqlDataSourceTechStane" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataComboBoxColumn FieldName="period_used_id" VisibleIndex="5" 
                Visible="True" Caption="Період використання">
                <PropertiesComboBox DataSourceID="SqlDataSourcePeriodUsed" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

<%-- 
            <dx:GridViewDataTextColumn FieldName="floor" Caption="Місце розташування вільного приміщення (поверх)" VisibleIndex="5">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="possible_using" Caption="Можливе використання вільного приміщення" VisibleIndex="6">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>
--%>
            <dx:GridViewBandColumn Caption="Комунальне забезпечення">
                <Columns>

                    <dx:GridViewDataCheckColumn FieldName="water" Caption="Водо-постача-ння" VisibleIndex="7">
                        <HeaderStyle Wrap="True" />
						<EditFormSettings Caption="Водопостачання" />
                    </dx:GridViewDataCheckColumn>

                    <dx:GridViewDataCheckColumn FieldName="heating" Caption="Тепло-постача-ння" VisibleIndex="8">
                        <HeaderStyle Wrap="True" />
						<EditFormSettings Caption="Теплопостачання" />
                    </dx:GridViewDataCheckColumn>

                    <dx:GridViewDataComboBoxColumn FieldName="power_info_id" Width="120px" VisibleIndex="9" 
                        Visible="True" Caption="Потужність електромережі">
                        <HeaderStyle Wrap="True" />
                        <PropertiesComboBox DataSourceID="SqlDataSourcePowerInfo" ValueField="id" TextField="name" ValueType="System.Int32" />
                    </dx:GridViewDataComboBoxColumn>


                    <dx:GridViewDataCheckColumn FieldName="gas" Caption="Газо-постача-ння" VisibleIndex="10">
						<EditFormSettings Caption="Газопостачання" />
                        <HeaderStyle Wrap="True" />
                    </dx:GridViewDataCheckColumn>

                </Columns>
            </dx:GridViewBandColumn>

            <dx:GridViewDataDateColumn FieldName="modify_date" Caption="Дата редагування" VisibleIndex="12" ReadOnly="true">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataTextColumn FieldName="modified_by" Caption="Користувач" VisibleIndex="13" ReadOnly="true">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="floor" Caption="Характеристика об’єкта оренди" ToolTip="Характеристика об’єкта оренди (будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі (надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів)" VisibleIndex="15">
                <HeaderStyle Wrap="True" />
                
            </dx:GridViewDataTextColumn>

            <%--<dx:GridViewDataTextColumn FieldName="possible_using" Caption="Можливе використання вільного приміщення" VisibleIndex="16" Width ="100px">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>--%>

<%--            <dx:GridViewDataComboBoxColumn FieldName="using_possible_id" VisibleIndex="16" Width = "320px" Visible="True" Caption="Можливе використання вільного приміщення">
				<HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceUsingPossible" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>--%>
            <dx:GridViewDataComboBoxColumn FieldName="possible_using" VisibleIndex="16" Width = "320px" Visible="True" Caption="Можливе використання вільного приміщення" >
				<HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceUsingPossible" DropDownStyle="DropDown" AllowMouseWheel="true" ValueField="name" TextField="name" ValueType="System.String" EnableSynchronization="False" />
            </dx:GridViewDataComboBoxColumn>


            <%--<dx:GridViewDataCheckColumn FieldName="is_solution" Caption="Наявність рішень про проведення інвестиційного конкурсу або про включення об’єкта до переліку майна, що підлягає приватизації" VisibleIndex="18" Width ="100px">
                 <HeaderStyle Wrap="True" />
				 <EditFormSettings Caption="Наявність рішень про проведення інвестиційного конкурсу або про включення об’єкта до переліку майна, що підлягає приватизації" />
  				<EditFormCaptionStyle Wrap="True"/>
             </dx:GridViewDataCheckColumn>--%>

            <dx:GridViewDataComboBoxColumn FieldName="invest_solution_id" VisibleIndex="18" Width = "100px" Visible="True" Caption="Наявність рішень про проведення інвестиційного конкурсу або про включення об’єкта до переліку майна, що підлягає приватизації">
				<HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceInvestSolution" ValueField="id" TextField="name" ValueType="System.Int32" />
				<EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataTextColumn FieldName="initiator" Caption="Ініціатор оренди (текстова інформація, якщо балансоутримувач - пусто)" VisibleIndex="19" Width ="100px">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataComboBoxColumn FieldName="zgoda_control_id" VisibleIndex="20" Width ="80px"
                Visible="True" Caption="Погодження органу управління балансоутримувача">
                <HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceZgodaRenter" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataComboBoxColumn FieldName="zgoda_renter_id" VisibleIndex="21" Width ="80px"
                Visible="True" Caption="Погодження органу охорони культурної спадщини">
                <HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceZgodaRenter" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            <%--<dx:GridViewDataTextColumn FieldName="note" Caption="Примітка" VisibleIndex="22" Width ="100px" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataComboBoxColumn FieldName="free_object_type_id" VisibleIndex="22" Width ="100px"
                Visible="True" Caption="Тип об’єкта">
                <HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceFreeObjectType" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>


            <dx:GridViewDataCheckColumn FieldName="is_included" Caption="Включено до переліку вільних приміщень" VisibleIndex="30">
                <HeaderStyle Wrap="True" />
				<EditFormSettings Caption="Включено до переліку вільних приміщень" />
            </dx:GridViewDataCheckColumn>

            <dx:GridViewDataTextColumn FieldName="komis_protocol" Caption="Погодження орендодавця" VisibleIndex="32" Width ="80px" >
                <HeaderStyle Wrap="True" />
				<EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="id" Caption="Реєстра-ційний №" VisibleIndex="40" Width ="50px"  >
				<CellStyle HorizontalAlign="Center" />
                <HeaderStyle Wrap="True" />
				<EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>

		    <dx:GridViewDataComboBoxColumn FieldName="include_in_perelik" VisibleIndex="50" Width = "50px" Visible="True" Caption="Включено до переліку №">
			    <HeaderStyle Wrap="True" />
			    <PropertiesComboBox DataSourceID="SqlDataSourceIncludeInPerelik" ValueField="id" TextField="name" ValueType="System.String" />
                <EditFormSettings ColumnSpan="1" />
		    </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataTextColumn Name="tmp1" Caption="" VisibleIndex="55" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="zal_balans_vartist" Caption="Залишкова балансова вартість, грн." VisibleIndex="110" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="perv_balans_vartist" Caption="Первісна балансова вартість, грн." VisibleIndex="120" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="punkt_metod_rozrahunok" Caption="Посилання на пункт Методики розрахунку орендної плати, яким встановлена орендна ставка для запропонованого цільового призначення" VisibleIndex="130" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Name="tmp2" Caption="" VisibleIndex="135" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="prop_srok_orands" Caption="Пропонований строк оренди (у роках)" VisibleIndex="140" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Name="tmp3" Caption="" VisibleIndex="145" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="nomer_derzh_reestr_neruh" Caption="Номер запису про право власності у Реєстрація у Державному реєстрі речових прав на нерухоме майно" VisibleIndex="150" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="reenum_derzh_reestr_neruh" Caption="Реєстраційний номер об'єкту нерухомого майна у Реєстрація у Державному реєстрі речових прав на нерухоме майно" VisibleIndex="155" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="info_priznach_nouse" Caption="Інформація про цільове призначення об’єкта оренди у випадках неможливості використання об’єкта за будь-яким цільовим призначенням, якщо об’єкт розташований у приміщеннях, які мають відповідне соціально-економічне призначення" VisibleIndex="160" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="info_rahunok_postach" Caption="Інформація про наявність окремих особових рахунків на об'єкт оренди, відкритих постачальниками комунальних послуг, або інформація про порядок участі орендаря у компенсації балансоутримувачу витрат на оплату комунальних послуг, якщо об'єкт оренди не має окремих особових рахунків, відкритих для нього відповідними постачальниками комунальних послуг" VisibleIndex="165" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="priznach_before" Caption="Цільове призначення об’єкта, за яким об’єкт використовувався перед тим, як він став вакантним" VisibleIndex="205" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="period_nouse" Caption="Період часу, протягом якого об’єкт не використовується" VisibleIndex="215" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="osoba_use_before" Caption="Інформацію про особу, яка використовувала об’єкт перед тим, як він став вакантним (якщо такою особою був балансоутримувач, проставляється позначка “об’єкт використовувався балансоутримувачем”)" VisibleIndex="225" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="primitki" Caption="Примітки" VisibleIndex="230" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="form_of_ownership" Caption="-" VisibleIndex="10000" Width ="80px" Visible="false" >
                <HeaderStyle Wrap="True" />
				<EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>




        </Columns>
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsPager PageSize="10" />
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
                    DeleteMethod="Delete" InsertMethod="Insert" 
                    OnInserting="ObjectDataSourcePhotoFiles_Inserting" 
                    SelectMethod="Select" 
                    TypeName="ExtDataEntry.Models.FileAttachment">
                    <DeleteParameters>
                        <asp:Parameter DefaultValue="1NF" Name="scope" Type="String" />
                        <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
                        <asp:Parameter Name="id" Type="String" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter DefaultValue="1NF" Name="scope" Type="String" />
                        <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
                        <asp:Parameter Name="Name" Type="String" />
                        <asp:Parameter Name="Image" Type="Object" />
                    </InsertParameters>
                    <SelectParameters>
                        <asp:Parameter DefaultValue="1NF" Name="scope" Type="String" />
                        <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>

                <dx:ASPxFileManager ID="ASPxFileManagerPhotoFiles" runat="server" 
                    ClientInstanceName="ASPxFileManagerPhotoFiles" DataSourceID="ObjectDataSourcePhotoFiles">
                    <Settings RootFolder="~\" ThumbnailFolder="~\Thumb\" />
                    <SettingsFileList>
                        <ThumbnailsViewSettings ThumbnailSize="180px" />
                    </SettingsFileList>
                    <SettingsEditing AllowDelete="True" />
                    <SettingsFolders Visible="False" />
                    <SettingsToolbar ShowDownloadButton="True" ShowPath="False" />
                    <SettingsUpload UseAdvancedUploadMode="True">
                        <AdvancedModeSettings EnableMultiSelect="True" />
                    </SettingsUpload>

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

        <dx:TabPage Text="Вартісна оцінка" Name="Tab5">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3" runat="server">

                    <asp:FormView runat="server" BorderStyle="None" ID="BalansCostForm" DataSourceID="SqlDataSourceBalansObject" EnableViewState="False">
                        <ItemTemplate>

                            <dx:ASPxRoundPanel ID="PanelObjCost" runat="server" HeaderText="Вартісні показники">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent5" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="0" width="990px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Первісна (переоцінена) вартість, тис. грн." width="200px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjCostBalans" runat="server" NumberType="Float" Value='<%# Eval("cost_balans") %>' Width="190px" Title="Первісна (переоцінена) вартість" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="Залишкова вартість, тис. грн." width="200px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjCostZalishkova" runat="server" NumberType="Float" Value='<%# Eval("cost_zalishkova") %>' Width="190px" Title="Залишкова вартість" /></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Знос, тис. грн." width="200px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjZnos" runat="server" NumberType="Float" Value='<%# Eval("znos") %>' Width="190px" Title="Знос" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel32" runat="server" Text="станом на" width="200px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxDateEdit ID="EditObjZnosDate" runat="server" Value='<%# Eval("znos_date") %>' Width="190px" Title="Знос станом на" /></td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelExpertCost" runat="server" HeaderText="Ринкова вартість">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent9" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="0" width="990px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Ринкова вартість, тис. грн." width="200px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditCostExpertTotal" runat="server" NumberType="Float" Value='<%# Eval("cost_expert_total") %>' Width="190px" Title="Ринкова вартість" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="станом на" width="200px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxDateEdit ID="EditDateExpert" runat="server" Value='<%# Eval("date_expert") %>' Width="190px" Title="Ринкова вартість станом на" /></td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                        </ItemTemplate>
                    </asp:FormView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Фото / плани" Name="Tab6">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl4" runat="server">

                    <dx:ASPxRoundPanel ID="PanelPhoto" runat="server" HeaderText="Фото / плани" EnableViewState="true">
                        <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent5" runat="server">


                                <table border="0" cellspacing="0" cellpadding="0" width="990px">
                                <tr>
                                <td>

                                <dx:ASPxCallbackPanel ID="ASPxCallbackPanelImageGallery" EnableViewState="true"
                                    ClientInstanceName="ASPxCallbackPanelImageGallery" runat="server" 
                                    OnCallback="ASPxCallbackPanelImageGallery_Callback">

                                    <SettingsLoadingPanel Enabled="false"/>
                            
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent11" runat="server" SupportsDisabledAttribute="True">
                                        
                                    <dx:ASPxImageGallery ID="imageGalleryDemo" runat="server" DataSourceID="ObjectDataSourceBalansPhoto"
                                        EnableViewState="false" 
                                        AlwaysShowPager="false" 
                                        PagerAlign="Center"
                                        ThumbnailHeight="190" ThumbnailWidth="190"
                                        SettingsFullscreenViewer-ShowCloseButton="true" 
                                        OnDataBound="imageGalleryDemo_DataBound" >

<%--    pgv                             <SettingsFolder ImageCacheFolder="~\Thumb\" ImageSourceFolder="~\ImgContent\tmp\" />      --%>         
                                      <SettingsFolder ImageCacheFolder="~\Thumb\"  /> 
                                        
                                        <ItemTextTemplate>

                                            <div class="item">
                                                <%--<div class="item_email" style="text-align:center"><%# Container.EvalDataItem("file_name")%></div><br />--%>
                                                <div class="item_email" style="text-align:center">
                                                <%--<a style="color:White;cursor:pointer" onclick="javascript:EditImage(<%# Container.ItemIndex %>);" title="Редагувати">Редагувати</a>&nbsp;&nbsp;--%>
                                                    <a style="color:White;cursor:pointer" onclick="javascript:DeleteImage(<%# Container.ItemIndex %>);" title="Видалити">Видалити</a>

                                                    <%--<a style="color:White;cursor:pointer" onclick="function(s, e) { imageGalleryDemo.PerformCallback('deleteimage:<%# Container.ItemIndex %>'); }" title="Видалити">Видалити</a>--%>
                                                    
                                                </div>
                                            </div>

                                        </ItemTextTemplate>
                                        <SettingsTableLayout RowsPerPage="2" ColumnCount="5" />

                                        <SettingsTableLayout ColumnCount="5" RowsPerPage="2"></SettingsTableLayout>

                                        <PagerSettings Position="TopAndBottom">
                                            <PageSizeItemSettings Visible="False" />
                                            <PageSizeItemSettings Visible="False"></PageSizeItemSettings>
                                        </PagerSettings>

                                    </dx:ASPxImageGallery> 

                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                                </td>
                                </tr>
                                <tr>
                                <td>





<%--                                <asp:UpdatePanel ID="updPanel" EnableViewState="true" runat="server" ChildrenAsTriggers="true">
                                    <ContentTemplate>--%>

                                <dx:ASPxCallbackPanel ID="ContentCallback" runat="server" EnableViewState="true"
                                    ClientInstanceName="ContentCallback" OnCallback="ContentCallback_Callback">
                                    
                                    <PanelCollection>
                                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                            
<%--                                            <asp:HiddenField ID="TempFolderIDField" runat="server" 
                                                OnValueChanged="TempFolderIDField_ValueChanged" />      --%>

                                            <dx:ASPxUploadControl ID="uplImage" runat="server" ShowUploadButton="false" 
                                                FileUploadMode="OnPageLoad"
                                                ClientInstanceName="uploader" NullText="..." 
                                                OnFileUploadComplete="ASPxUploadPhotoControl_FileUploadComplete" 
                                                ShowProgressPanel="True" Size="35" UploadMode="Auto">
                                                <ValidationSettings AllowedFileExtensions=".jpg, .jpeg, .jpe, .gif, .png, .bmp, .pdf" 
                                                    MaxFileSize="20480000">

                                                </ValidationSettings>
                                                <ClientSideEvents FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" 
                                                    FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }" 
                                                    TextChanged="function(s, e) { UpdateUploadButton(); }" />
                                                <AdvancedModeSettings EnableMultiSelect="True" />
                                            </dx:ASPxUploadControl>

                                            <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Visible ="true"
                                                ClientInstanceName="btnUpload" Text="Завантажити" 
                                            onclick="btnUpload_Click">
                                                <ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
                                            </dx:ASPxButton>

                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>

<%--                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>

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

<asp:FormView ID="errorForm" runat="server" OnDataBound="errorForm_DataBound">
    <ItemTemplate>
        <div style="padding:10px;">
        <div style="color:Red;font-weight:bold">Наступні поля мають бути заповнені перед надсиланням до ДКВ:</div>
        <ul>
            <asp:Repeater ID="errorList" runat="server">
                <ItemTemplate>
                    <li style="color:Red"><%# Eval("error_message") %> </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
        </div>
    </ItemTemplate>
</asp:FormView>


        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<p class="SpacingPara"/>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonSave" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) 
                { 
                    var objSqrTotal = EditObjSqrTotal.GetValue();
                    if (objSqrTotal == -1)  //pgv
                    {
                        if (!confirm('Увага! Об&rsquo;єкт буде видалено, якщо площа на балансі дорівнює -1. Ви дійсно хочете видалити цей об&rsquo;єкт?'))
                        {
                            //pgv EditObjSqrTotal.SetValue(objectTotalSqrInitial);
                        }
                    }
                    CPMainPanel.PerformCallback('save:'); 
                }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonSend" runat="server" Text="Надіслати" AutoPostBack="false" ValidationGroup="MainGroup" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('send:'); }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonClear" runat="server" Text="Відмінити зміни" CausesValidation="false" AutoPostBack="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('clear:'); ASPxCallbackPanelImageGallery.PerformCallback('refreshphoto:'); }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonComments" runat="server" Text="Коментарі" CausesValidation="false" AutoPostBack="false" />

            <dx:ASPxPopupControl ID="PopupComments" runat="server" 
                HeaderText="Коментарі" 
                ClientInstanceName="PopupComments" 
                PopupElementID="ButtonComments" >
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                        
                        <dx:ASPxCallbackPanel ID="CPCommentViewerPanel" ClientInstanceName="CPCommentViewerPanel" runat="server" OnCallback="CPCommentViewerPanel_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent6" runat="server">

                                    <uc1:ReportCommentViewer ID="ReportCommentViewer1" runat="server"/>

                                </dx:panelcontent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents
                    PopUp="function (s,e) { CPCommentViewerPanel.PerformCallback('' + lastFocusedControlId + ';' + lastFocusedControlTitle); }"
                    CloseUp="function (s,e) { CPMainPanel.PerformCallback('clear:'); }" />
            </dx:ASPxPopupControl>
        </td>
    </tr>
</table>



</asp:Content>

