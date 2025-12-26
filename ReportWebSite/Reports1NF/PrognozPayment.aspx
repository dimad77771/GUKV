<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrognozPayment.aspx.cs" Inherits="Reports1NF_Report1NFPrivatisatSquare"
	MasterPageFile="~/NoHeader.master" Title="Прогноз впливу нормативно-управлінських рішень на рівень надходження орендної плати" %>

<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Export" TagPrefix="dx" %>
<%@ Register Src="../UserControls/FieldChooser.ascx" TagName="FieldChooser" TagPrefix="uc3" %>
<%@ Register Src="../UserControls/FieldFixxer.ascx" TagName="FieldFixxer" TagPrefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">

	<style>
		.command-column-class {
			white-space: normal !important;
		}

		a.dxbButton_DevEx {
			margin: 0px !important;
		}
	</style>

	<script type="text/javascript" src="../Scripts/PageScript.js"></script>

	<script type="text/javascript" language="javascript">

        // <![CDATA[

        window.onresize = function () { AdjustGridSizes(); };

        function AdjustGridSizes() {
            var hh = 120
            InflationGridView.SetHeight(hh);
            PrivatisatGridView.SetHeight(window.innerHeight - 140 - hh);
        }

        function GridViewFreeSquareInit(s, e) {

            PrivatisatGridView.PerformCallback("init:");
        }

        function GridViewFreeSquareEndCallback(s, e) {

            AdjustGridSizes();
        }

        function ShowFieldChooserPopupControl(s, e) {

            PrimaryGridView = PrivatisatGridView;
            PopupFieldChooser.Show();
        }


        function ShowPhoto(s, e) {
            if (e.buttonID == 'btnPdfBuild') {
                PrivatisatGridView.GetRowValues(e.visibleIndex, 'id', OnGridPdfBuildGetRowValues);
            } else if (e.buttonID == 'btnJpegBuild') {
                PrivatisatGridView.GetRowValues(e.visibleIndex, 'id', OnGridJpegBuildGetRowValues);
            } else if (e.buttonID == 'bnt_current_stage_pdf') {
                $.cookie('RecordID', s.GetRowKey(e.visibleIndex));
                ASPxFileManagerPhotoFiles.Refresh();
                PopupObjectPhotos.Show();
            } else if (e.buttonID == 'btnMapShow') {
                PrivatisatGridView.GetRowValues(e.visibleIndex, 'id', OnMapShowGetRowValues);
            } else if (e.buttonID == 'btnFreeCycle') {
                PrivatisatGridView.GetRowValues(e.visibleIndex, 'id', OnFreeCycleGetRowValues);
            } else if (e.buttonID == 'btnOrgBalansObject') {
                PrivatisatGridView.GetRowValues(e.visibleIndex, 'id;balans_id;report_id', OnClickOrgBalansObject);
            } else if (e.buttonID == 'btnCopyFullDescription2') {
                var cols = "include_in_perelik;zal_balans_vartist;perv_balans_vartist;free_object_type_name;prop_srok_orands;punkt_metod_rozrahunok;invest_solution;";
                cols += "zgoda_control;district;street_name;addr_nomer;total_free_sqr;free_sql_usefull;";
                cols += "floor;condition;water;heating;gas;power_text;history;zgoda_renter;nomer_derzh_reestr_neruh;reenum_derzh_reestr_neruh;info_priznach_nouse;info_rahunok_postach;priznach_before;period_nouse;osoba_use_before"
                PrivatisatGridView.GetRowValues(e.visibleIndex, cols, OnCopyFullDescription2);
            } else if (e.buttonID == 'btnCopyFullDescription') {
                var cols = "id";
                PrivatisatGridView.GetRowValues(e.visibleIndex, cols, OnCopyFullDescription);
            }
        }

        function onZkpoCodeChanged(s, e) {
            //console.log("s", s);
            //console.log("e", e);
            //grid.GetEditor("org_name").PerformCallback(s.GetValue());
            var editor = PrivatisatGridView.GetEditor("org_info_id");
            var table = $(editor.inputElement).parents("#MainContent_PrivatisatGridView_DXEditingRow");
            var child = table.find("td.dxgv").eq(4).children("span");
            console.log("editor", editor);
            console.log("table", table);
            console.log("child", child);
            var text = editor.GetText();
            text = text.substring(text.indexOf("-") + 2);
            console.log("text", text);
            child.text(text);
        }

        function OnCopyFullDescription(values) {
            var headers = [
                "Включено до переліку № - ",
                "Залишкова балансова вартість – ",
                "Первісна балансова вартість - ",
                "Тип об’єкта - ",
                "Пропонований строк оренди (у роках) – ",
                "Пункт Методики розрахунку орендної плати (якщо об’єкт пропонується для включення до Переліку другого типу) - ",
                "Наявність рішень про проведення інвестиційного конкурсу або про включення об’єкта до переліку майна, що підлягає приватизації - ",

                "Погодження органу управління балансоутримувача – ",
                "Район – ",
                "Назва Вулиці - ",
                "Номер Будинку - ",
                "Загальна площа об’єкта - ",
                "Корисна площа об’єкта – ",
                "Характеристика об’єкта оренди(будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі(надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів) – ",
                "Технічний стан – ",
                "Водопостачання – ",
                "Теплопостачання – ",
                "Газопостачання – ",
                "Електропостачання – ",
                "Пам’ятка культурної спадщини - ",
                "Погодження органу охорони культурної спадщини - ",
                "Номер запису про право власності у Реєстрація у Державному реєстрі речових прав на нерухоме майно – ",
                "Реєстраційний номер об'єкту нерухомого майна у Реєстрація у Державному реєстрі речових прав на нерухоме майно – ",
                "Інформація про цільове призначення об’єкта оренди – ",
                "Інформація про наявність окремих особових рахунків на об'єкт оренди, відкритих постачальниками комунальних послуг - ",
                "Цільове призначення об’єкта, за яким об’єкт використовувався перед тим, як він став вакантним – ",
                "Період часу, протягом якого об’єкт не використовується – ",
                "Інформацію про особу, яка використовувала об’єкт перед тим, як він став вакантним – ",
            ];

            console.log("values", values);

            var txt = "";
            //for (var i = 0; i < headers.length; i++) {
            //	var vv = values[i];
            //	if (vv === null) {
            //		vv = "";
            //	} else if (vv === true) {
            //		vv = "так";
            //	} else if (vv === false) {
            //		vv = "ні";
            //	}

            //	txt += (i == 0 ? "" : "\n") + headers[i] + vv;
            //}

            //var id = values[values.length - 1];
            var id = values;
            txt += "Фото - https://dkv.kyivcity.gov.ua/Reports1NF/BalansPrivatisatPhotosPdf.aspx?id=" + id + '&jpeg=1';

            //console.log("txt", txt);

            $("#inpit-for-copy-clipboard").val(txt);
            $("#inpit-for-copy-clipboard").select();
            document.execCommand("copy");
            return;

            navigator.clipboard.writeText(txt).then(function () {
                alert("Опис скопійовано в буфер обміну");
            }, function () {
                alert("Не можу записати буфер обміну");
            });
        }

        function OnCopyFullDescription2(values) {
            var headers = [
                "Включено до переліку № - ",
                "Залишкова балансова вартість – ",
                "Первісна балансова вартість - ",
                "Тип об’єкта - ",
                "Пропонований строк оренди (у роках) – ",
                "Пункт Методики розрахунку орендної плати (якщо об’єкт пропонується для включення до Переліку другого типу) - ",
                "Наявність рішень про проведення інвестиційного конкурсу або про включення об’єкта до переліку майна, що підлягає приватизації - ",

                "Погодження органу управління балансоутримувача – ",
                "Район – ",
                "Назва Вулиці - ",
                "Номер Будинку - ",
                "Загальна площа об’єкта - ",
                "Корисна площа об’єкта – ",
                "Характеристика об’єкта оренди(будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі(надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів) – ",
                "Технічний стан – ",
                "Водопостачання – ",
                "Теплопостачання – ",
                "Газопостачання – ",
                "Електропостачання – ",
                "Пам’ятка культурної спадщини - ",
                "Погодження органу охорони культурної спадщини - ",
                "Номер запису про право власності у Реєстрація у Державному реєстрі речових прав на нерухоме майно – ",
                "Реєстраційний номер об'єкту нерухомого майна у Реєстрація у Державному реєстрі речових прав на нерухоме майно – ",
                "Інформація про цільове призначення об’єкта оренди – ",
                "Інформація про наявність окремих особових рахунків на об'єкт оренди, відкритих постачальниками комунальних послуг - ",
                "Цільове призначення об’єкта, за яким об’єкт використовувався перед тим, як він став вакантним – ",
                "Період часу, протягом якого об’єкт не використовується – ",
                "Інформацію про особу, яка використовувала об’єкт перед тим, як він став вакантним – ",
            ];

            console.log("values", values);

            var txt = "";
            for (var i = 0; i < headers.length; i++) {
                var vv = values[i];
                if (vv === null) {
                    vv = "";
                } else if (vv === true) {
                    vv = "так";
                } else if (vv === false) {
                    vv = "ні";
                }

                txt += (i == 0 ? "" : "\n") + headers[i] + vv;
            }

            //console.log("txt", txt);

            $("#inpit-for-copy-clipboard").val(txt);
            $("#inpit-for-copy-clipboard").select();
            document.execCommand("copy");
            return;

            navigator.clipboard.writeText(txt).then(function () {
                alert("Опис скопійовано в буфер обміну");
            }, function () {
                alert("Не можу записати буфер обміну");
            });
        }


        function OnMapShowGetRowValues(values) {
            var id = values;
            window.open(
                'Report1NFPrivatisatMap.aspx?fs_id=' + id,
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

        function OnClickOrgBalansObject(values) {
            window.location = 'OrgBalansObject.aspx?rid=' + values[2] + '&bid=' + values[1] + '&edit_free_square_id=' + values[0];
        }


        function OnGridPdfBuildGetRowValues(values) {
            console.log(values);
            var id = values;
            window.open(
                'BalansPrivatisatPhotosPdf.aspx?id=' + id,
                '_blank',
            );
        }

        function OnGridJpegBuildGetRowValues(values) {
            console.log(values);
            var id = values;
            window.open(
                'BalansPrivatisatPhotosPdf.aspx?id=' + id + '&jpeg=1',
                '_blank',
            );
        }


        function OnDropDown(comboBox) {
            //SetDropDownWidth(comboBox, "368px");
            _aspxMakeScollableArea(comboBox);
        }

        function SetDropDownWidth(comboBox, width) {
            var listBox = comboBox.GetListBoxControl();
            var scrollDiv = listBox.GetScrollDivElement();
            //scrollDiv.style.overflowX = "auto";
            //scrollDiv.style.width = width;

            scrollDiv.style.overflowY = "scroll";
            console.log("scrollDiv.style.overflowY", scrollDiv.style.overflowY);

            var popupControl = comboBox.GetPopupControl();
            //popupControl.SetSize("0", "0");
        }


        function _aspxMakeScollableArea(comboBox) {
            var listBox = comboBox.GetListBoxControl();
            if (_aspxIsExists(listBox)) {
                var lsScrollableDiv = listBox.GetScrollDivElement();
                if (_aspxIsExists(lsScrollableDiv)) {
                    var browserWidth = (_aspxGetDocumentClientWidth() - 10) + 'px';
                    //alert(browserWidth);
                    _aspxSetAttribute(lsScrollableDiv.style, "width", browserWidth);
                    _aspxSetAttribute(lsScrollableDiv.style, "overflow-x", "scroll");
                    _aspxSetAttribute(lsScrollableDiv.style, "overflow-y", "scroll");
                }
            }
        }

        // ]]>

    </script>

	<script type="text/javascript">
		function showOverlay() {
			document.getElementById("blockingOverlay").style.display = "block";
		}

		function hideOverlay() {
			document.getElementById("blockingOverlay").style.display = "none";
		}

		function handleError(msg) {
			hideOverlay();
			alert("Сталася помилка:\n" + msg);
		}
	</script>



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictStanRecieve" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKV2016ConnectionString %>" 
    SelectCommand="SELECT 1 as id, '1 кв.' as name union
SELECT 2 as id, '2 кв.' as name union
SELECT 3 as id, '3 кв.' as name union
SELECT 4 as id, '4 кв.' as name	">
</mini:ProfiledSqlDataSource>


	<mini:ProfiledSqlDataSource ID="SqlDataSourceInflation" runat="server"
		ConnectionString="<%$ ConnectionStrings:GUKV2016ConnectionString %>"
		SelectCommand="SELECT [id], [prognoz_inflation_this], [prognoz_inflation_next], prognoz_inflation_1,prognoz_inflation_2,prognoz_inflation_3,prognoz_inflation_4,rozrah_persion FROM [current_inflation]"
		UpdateCommand="UPDATE [current_inflation] SET 
			[prognoz_inflation_1] = @prognoz_inflation_1,
			[prognoz_inflation_2] = @prognoz_inflation_2,
			[prognoz_inflation_3] = @prognoz_inflation_3,
			[prognoz_inflation_4] = @prognoz_inflation_4
		">
	</mini:ProfiledSqlDataSource>

	<mini:ProfiledSqlDataSource ID="SqlDataSourcePrivatisat" runat="server"
		ConnectionString="<%$ ConnectionStrings:GUKV2016ConnectionString %>"
		SelectCommand="

SELECT 
		isnull((select Q.contribution_rate from reports1nf_org_info Q where Q.report_id = rep.report_id),0) as contribution_rate,
		(select Q.contribution_rate from reports1nf_org_info_new_contribution_rate Q where Q.report_id = rep.report_id) new_contribution_rate,
        isnull(ddd.name, 'Невідомо') as 'dict_rent_occupation_name',
        (SELECT Q.stan_recieve_name FROM dict_stan_recieve Q where Q.stan_recieve_id = rep.stan_recieve_id) stan_recieve_name,
		rep.*,
		ar.NumOfSubmAgr, 
		ar.NumOfAgr, 
		obj.NumOfSubmObj, 
		obj.NumOfObj,
        (SELECT MAX(sdt) FROM (VALUES
        (rep.bal_max_submit_date),
        (rep.bal_del_max_submit_date),
        (rep.arenda_max_submit_date),
        (rep.arenda_rented_max_submit_date),
        (rep.org_max_submit_date)) AS AllMaxSubmitDates(sdt)) AS 'max_submit_date',
        CASE WHEN rep.is_reviewed = 0 THEN N'НI' ELSE N'ТАК' END AS 'review_performed'
        
        --,ReportDates.*
        
        ,ObjectAndRentTotals1.*
        ,ObjectAndRentTotals2.*
        ,ObjectAndRentTotals3.*
        ,ObjectAndRentTotals4.*
		,OrganizationProperties.*
		,RentPaymentProperties1.*
		,RentPaymentProperties2.*

        ,isnull(PAY_RECV_ZVIT,0) + isnull(PAY_RECV_OTHER,0) - isnull(PAY_RETURN_ALL_OREND_PAYED,0) as PAY_RECV_ZVIT_new
        ,corporav_prava
        ,case when exists (select 1 from reports1nf_balans_dinfo_attachfiles Q join reports1nf_balans Q2 on Q.free_square_id = 500000 * Q2.report_id + Q2.id where Q2.report_id = rep.report_id) then 1 else 0 end as has_reports1nf_balans_akt_attachfiles
        ,case when exists (select 1 from reports1nf_balans_rish_attachfiles Q join reports1nf_balans Q2 on Q.free_square_id = 500000 * Q2.report_id + Q2.id where Q2.report_id = rep.report_id) then 1 else 0 end as has_reports1nf_balans_rish_attachfiles
        ,case when exists (select 1 from reports1nf_balans_bti_attachfiles Q join reports1nf_balans Q2 on Q.free_square_id = 500000 * Q2.report_id + Q2.id where Q2.report_id = rep.report_id) then 1 else 0 end as has_reports1nf_balans_bti_attachfiles
        ,case when exists (select 1 from reports1nf_balans_dinfo_attachfiles Q join reports1nf_balans Q2 on Q.free_square_id = 500000 * Q2.report_id + Q2.id where Q2.report_id = rep.report_id) then 1 else 0 end as has_reports1nf_balans_dinfo_attachfiles
        
        FROM view_reports1nf rep
        LEFT JOIN (SELECT sum(CASE WHEN (r1a.submit_date IS NULL OR r1a.modify_date IS NULL OR r1a.modify_date > r1a.submit_date) THEN 0 ELSE 1 END) as NumOfSubmAgr, 
			Count(r1a.ID) AS NumOfAgr, report_id
        FROM reports1nf_arenda r1a LEFT JOIN arenda a
        ON r1a.id = a.id WHERE a.is_deleted IS NULL OR a.is_deleted = 0
        GROUP BY report_id) ar on rep.report_id = ar.report_id
        
        LEFT JOIN (SELECT sum(CASE WHEN (b.submit_date IS NULL OR b.modify_date IS NULL OR b.modify_date > b.submit_date) THEN 0 ELSE 1 END) AS NumOfSubmObj,
        Count(ID) AS NumOfObj,
        report_id
        FROM reports1nf_balans b
        WHERE is_deleted IS NULL OR is_deleted = 0
        GROUP BY report_id) obj on rep.report_id = obj.report_id
        
  --      LEFT JOIN (
		--SELECT 
	 --       (SELECT MAX(org.submit_date) FROM reports1nf_org_info org WHERE org.report_id = r.id) AS 'org_date',
	 --       (SELECT MAX(bal.submit_date) FROM reports1nf_balans bal WHERE bal.report_id = r.id AND (bal.is_deleted IS NULL OR bal.is_deleted = 0)) AS 'bal_date',
	 --       (SELECT MAX(bd.submit_date) FROM reports1nf_balans_deleted bd WHERE bd.report_id = r.id) AS 'bal_deleted_date',
	 --       (SELECT MAX(ar.submit_date) FROM reports1nf_arenda ar WHERE ar.report_id = r.id AND (ar.is_deleted IS NULL OR ar.is_deleted = 0)) AS 'arenda_date',
	 --       (SELECT MAX(arr.submit_date) FROM reports1nf_arenda_rented arr WHERE arr.report_id = r.id AND (arr.is_deleted IS NULL OR arr.is_deleted = 0)) AS 'arenda_rented_date',
	 --       id as report_id
  --      FROM reports1nf r GROUP BY r.id) ReportDates on ReportDates.report_id = rep.report_id
        
        LEFT JOIN (
        SELECT
             SUM(bal.sqr_total) AS 'SQR_TOTAL_BAL'
            ,SUM(bal.sqr_kor) AS 'SQR_KOR'
			,SUM(bal.sqr_vlas_potreb) AS 'SQR_VLAS_POTREB'
	        ,SUM(case when bal.sqr_vlas_potreb > 0 then 1 else 0 end) AS 'SQR_VLAS_POTREB_COUNT'
--            ,SUM(CASE WHEN bal.is_free_sqr = 1 THEN bal.free_sqr_useful ELSE 0 END) AS 'SQR_FREE'
--            ,SUM(bal.free_sqr_useful) AS 'SQR_FREE'
              ,sum(fs.sqr_free) AS 'SQR_FREE'
			  ,sum(fs.sqr_free_count) AS 'SQR_FREE_COUNT'
            ,COUNT(*) AS 'NUM_BALANS'
            ,bal.report_id
        FROM reports1nf_balans bal
        outer apply (select sum(fs.total_free_sqr) as sqr_free, count(*) as sqr_free_count from reports1nf_balans_free_square fs where fs.balans_id = bal.id and fs.report_id = bal.report_id and fs.is_included = 1 and fs.total_free_sqr > 0) fs  
        WHERE (bal.is_deleted IS NULL OR bal.is_deleted = 0)
        GROUP BY bal.report_id) ObjectAndRentTotals1 ON ObjectAndRentTotals1.report_id = rep.report_id
        
        LEFT JOIN (
        SELECT
            SUM(bal.sqr_total) AS 'SQR_VIDCH',
            COUNT(*) AS 'NUM_VIDCH',
            bal.report_id
            FROM reports1nf_balans_deleted bal
            WHERE 
            bal.sqr_total > 0 and
            (year(bal.vidch_doc_date) = @period_year or bal.vidch_doc_date is null)
            GROUP BY bal.report_id) ObjectAndRentTotals2 ON ObjectAndRentTotals2.report_id = rep.report_id
            
         LEFT JOIN (
         SELECT R.report_id, SUM(R.row_count) AS 'NUM_GIVEN', SUM(R.SQR_GIVEN) AS 'SQR_GIVEN', COUNT(distinct org_renter_id) AS 'NUM_RENTER'
--        SELECT R.report_id, SUM(R.row_count) AS 'NUM_GIVEN', SUM(R.SQR_GIVEN) AS 'SQR_GIVEN'
--         SELECT R.report_id, SUM(NUM_GIVEN)  as 'NUM_GIVEN', SUM(R.SQR_GIVEN) AS 'SQR_GIVEN'
         FROM (
         	SELECT 
		SUM(ar.rent_square) AS 'SQR_GIVEN'
           		,COUNT(ar.id) AS 'NUM_GIVEN'
           		,1 AS 'row_count'
            		,ar.report_id
		,ar.org_renter_id
		FROM reports1nf_arenda ar
		WHERE (ar.is_deleted IS NULL OR ar.is_deleted = 0)
			AND NOT EXISTS(SELECT id FROM arenda a WHERE a.id = ar.id AND ISNULL(a.is_deleted, 0) = 1)
			AND ar.agreement_state = 1
		GROUP BY ar.report_id,ar.id /*ar.agreement_num,ar.agreement_date,ar.rent_start_date,ar.rent_finish_date*/,ar.org_renter_id) R GROUP BY R.report_id /*, R.row_count*/) ObjectAndRentTotals3 ON ObjectAndRentTotals3.report_id = rep.report_id
--		GROUP BY ar.report_id ,ar.agreement_num /*,ar.agreement_date,ar.rent_start_date,ar.rent_finish_date,ar.org_giver_id*/) R GROUP BY R.report_id /*, R.row_count*/) ObjectAndRentTotals3 ON ObjectAndRentTotals3.report_id = rep.report_id
			
         LEFT JOIN (
	SELECT
             SUM(ar.rent_square) AS 'SQR_RENTED'
            ,COUNT(*) AS 'NUM_RENTED',
            ar.report_id
			FROM reports1nf_arenda_rented ar
			WHERE (ar.is_deleted IS NULL OR ar.is_deleted = 0)
			GROUP BY ar.report_id) ObjectAndRentTotals4 ON ObjectAndRentTotals4.report_id = rep.report_id
			
		LEFT JOIN (
		SELECT
            [org].[full_name] AS 'SUBMITTER_FULL_NAME' -- 0
           ,[org].[short_name] AS 'SUBMITTER_SHORT_NAME' -- 1
           ,[org].[zkpo_code] AS 'SUBMITTER_ZKPO'-- 2
           ,[dict_org_industry].[name] AS 'ORG_INDUSTRY' -- 3
           ,[dict_org_occupation].[name] AS 'ORG_OCCUPATION' -- 4
           ,[dict_org_status].[name] AS 'ORG_STATUS' -- 5
           ,[dict_org_form_gosp].[name] AS 'ORG_FIN_FORM' -- 6
           ,[dict_org_ownership].[name] AS 'ORG_OWNERSHIP' -- 7
           ,[dict_org_gosp_struct].[name] AS 'gosp_struct' -- 8
           ,[dict_org_vedomstvo].[name] AS 'ORG_VEDOMSTVO' -- 9
           ,[dict_org_form].[name] AS 'ORG_GOSP_FORM' -- 10
           --,[dict_org_gosp_struct_type].[name] AS 'gosp_struct_type' -- 11
           --,[dict_org_sfera_upr].[name] AS 'sfera_upr' -- 12
           ,[dict_org_old_organ].[name] AS 'ORG_GOSP_UPR' -- 13
           ,[director_fio] AS 'BOSS_FIO' -- 14
           ,[director_phone] AS 'BOSS_TEL' -- 15
           ,[director_email] AS 'BOSS_EMAIL'-- 16
           ,[director_title] AS 'BOSS_POSADA'-- 17
           ,[kved_code] AS 'ORG_KVED' -- 18
           --,jr_street.name AS 'addr_street' -- 19
           --,org.addr_nomer -- 20
           --,org.addr_misc -- 21
           ,ph_street.name AS 'physAddrStreet' -- 22
           ,org.phys_addr_nomer AS 'physAddrNumber' -- 23
           ,org.phys_addr_misc AS 'physAddrMisc'-- 24
           ,CASE WHEN org.contribution_rate IS NULL THEN 0 ELSE ORG.contribution_rate END AS 'ORG_CONTRIB_RATE' -- 25
           ,org.buhgalter_fio AS 'USER_FIO' -- 26
           ,org.buhgalter_phone AS 'USER_TEL' -- 27
           ,org.buhgalter_email AS 'USER_EMAIL' -- 28
           ,org.budget_narah_50_uah AS 'PAY_50_NARAH' -- 29

           --,org.budget_zvit_50_uah AS 'PAY_50_PAYED'-- 30
           --,dbo.[get_kazna_total](org.zkpo_code,null,null) AS 'PAY_50_PAYED'-- 30 
           ,kazna.pay_sum AS 'PAY_50_PAYED'

           ,org.budget_prev_50_uah AS 'PAY_50_DEBT'-- 31
           ,org.budget_debt_30_50_uah AS 'PAY_50_DEBT_OLD' -- 32
           ,org.payment_budget_special AS 'PAY_SPECIAL'-- 33
           --,org.konkurs_payments -- 34
           --,org.unknown_payments -- 35
           --,CASE WHEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(org.budget_zvit_50_uah, 0)) > 0 THEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(org.budget_zvit_50_uah, 0)) ELSE 0 END AS 'PAY_50_DEBT_CUR'
           ,CASE WHEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(kazna.pay_sum, 0) + ISNULL(org.budget_prev_50_uah, 0)) - ISNULL(org.unknown_payments,0) < 0 THEN 0 Else (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(kazna.pay_sum, 0) + ISNULL(org.budget_prev_50_uah, 0)) - ISNULL(org.unknown_payments,0) END AS 'PAY_50_DEBT_CUR'
           ,org.konkurs_payments AS 'PAY_RECV_OTHER'
           ,[org].[report_id]
           ,[dict_otdel_gukv].name as 'otdel_gukv'
           ,[org].[prim_balanc]
           ,org.unknown_payments AS 'PAY_UNKNOWN_PAYMENTS'

            ,org.planuvania_1
            ,org.planuvania_2
            ,org.planuvania_3
            ,org.planuvania_4
            ,org.planuvania_5
            ,org.corporav_prava

            --,[dbo].[get_conveyancingRequests_count]([org].[report_id]) AS conveyancingRequests_count
            ,isnull(view_conveyancingRequests_count.cnt,0) AS conveyancingRequests_count

        FROM
            reports1nf_org_info org
            LEFT OUTER JOIN view_conveyancingRequests_count on view_conveyancingRequests_count.report_id = [org].[report_id]
            LEFT OUTER JOIN kazna_total_info(null, null) kazna on kazna.ident_bal_zkpo = org.zkpo_code
            LEFT OUTER JOIN dict_otdel_gukv ON org.otdel_gukv_id = dict_otdel_gukv.id
            LEFT OUTER JOIN dict_org_industry ON org.industry_id = dict_org_industry.id
            LEFT OUTER JOIN dict_org_occupation ON org.occupation_id = dict_org_occupation.id
            LEFT OUTER JOIN dict_org_status ON org.status_id = dict_org_status.id
            LEFT OUTER JOIN dict_org_form_gosp ON org.form_gosp_id = dict_org_form_gosp.id
            LEFT OUTER JOIN dict_org_ownership ON org.form_ownership_id = dict_org_ownership.id
            LEFT OUTER JOIN dict_org_gosp_struct ON org.gosp_struct_id = dict_org_gosp_struct.id
            LEFT OUTER JOIN dict_org_gosp_struct_type ON org.gosp_struct_type_id = dict_org_gosp_struct_type.id
            LEFT OUTER JOIN dict_org_vedomstvo ON org.vedomstvo_id = dict_org_vedomstvo.id
            LEFT OUTER JOIN dict_org_form ON org.form_id = dict_org_form.id
            LEFT OUTER JOIN dict_org_sfera_upr ON org.sfera_upr_id = dict_org_sfera_upr.id
            LEFT OUTER JOIN dict_org_old_organ ON org.old_organ_id = dict_org_old_organ.id
            LEFT OUTER JOIN dict_streets jr_street ON org.addr_street_id = jr_street.id
            LEFT OUTER JOIN dict_streets ph_street ON org.phys_addr_street_id = ph_street.id) OrganizationProperties ON OrganizationProperties.report_id = rep.report_id
            
		LEFT JOIN (SELECT report_id,MAX(rent_period_id) AS 'max_rent_period_id' FROM reports1nf_arenda_payments group by report_id) mrp on mrp.report_id = rep.report_id
            		
		LEFT JOIN (
		SELECT
                 --SUM(pay.sqr_total_rent) as 'sqr_total_rent' -- 0
                --,SUM(pay.sqr_payed_by_percent) as 'sqr_payed_by_percent' -- 1
                --,SUM(pay.sqr_payed_by_1uah) as 'sqr_payed_by_1uah' -- 2
                --,SUM(pay.sqr_payed_hourly) as 'sqr_payed_hourly' -- 3
                isnull(SUM(pay.payment_narah),0) - isnull(SUM(pay.znyato_nadmirno_narah),0) as 'PAY_NARAH_ZVIT' -- 4
                ,SUM(pay.last_year_saldo) as 'PAY_PEREPLATA' -- 5
                ,SUM(pay.zabezdepoz_prishlo) as 'PAY_ZABEZDEPOZ_PRISHLO' -- 5
                ,SUM(pay.payment_received) as 'PAY_RECV_ZVIT' -- 6
                ,SUM(pay.payment_nar_zvit) as 'PAY_RECV_NARAH' -- 7
                --,SUM(pay.payment_budget_special) as 'payment_budget_special' -- 8
                ,SUM(pay.debt_total) as 'PAY_DEBT_TOTAL' -- 9
                ,SUM(pay.debt_zvit) as 'PAY_DEBT_ZVIT' -- 10
                ,SUM(pay.debt_3_month) as 'PAY_debt_3_month' -- 11
                ,SUM(pay.debt_12_month) as 'PAY_debt_12_month' -- 12
                ,SUM(pay.debt_3_years) as 'PAY_debt_3_years' -- 13
                ,SUM(pay.debt_over_3_years) as 'PAY_debt_over_3_years' -- 14
                ,SUM(pay.debt_v_mezhah_vitrat) as 'PAY_DEBT_V_MEZH' -- 15
                ,SUM(pay.debt_spysano) as 'debt_spysano' -- 16
                --,SUM(budget_narah_50_uah) as 'budget_narah_50_uah' -- 17
                --,SUM(budget_zvit_50_uah) as 'budget_zvit_50_uah' -- 18
                --,SUM(budget_prev_50_uah) as 'budget_prev_50_uah' -- 19
                --,SUM(budget_debt_50_uah) as 'budget_debt_50_uah' -- 20
                --,SUM(budget_debt_30_50_uah) as 'budget_debt_30_50_uah' -- 21
                ,SUM(pay.old_debts_payed) as 'PAY_LAST_PER' -- 22
                ,SUM( isnull(pay.payment_narah,0) - isnull(pay.znyato_nadmirno_narah,0) ) as 'PAY_NARAH_ZVIT_NORMAL' -- 23
                ,SUM(pay.znyato_nadmirno_narah) as 'PAY_ZNYATO_NADMIRNO_NARAH' -- 24
                ,SUM(pay.zabezdepoz_saldo) as 'PAY_AVANCE_SALDO' -- 25
                ,SUM(pay.avance_plat) as 'PAY_AVANCE_PLAT' -- 26
                ,SUM(pay.total_pereplata) as 'PAY_PEREPLATA_ALL' -- 27
                ,SUM(pay.avance_debt) as 'PAY_AVANCE_DEBT' -- 28
                ,SUM(pay.avance_paymentnar) as 'PAY_AVANCE_PAYMENTNAR' -- 28
                ,SUM(pay.return_orend_payed) as 'PAY_RETURN_OREND_PAYED' -- 28
                ,SUM(pay.znyato_from_avance) as 'PAY_ZNYATO_FROM_AVANCE' -- 28
                ,SUM(pay.return_all_orend_payed) as 'PAY_RETURN_ALL_OREND_PAYED' -- 28
				,report_id
				,rent_period_id
            FROM reports1nf_arenda_payments pay
            WHERE NOT EXISTS(SELECT id FROM arenda a WHERE a.id = pay.arenda_id AND ISNULL(a.is_deleted, 0) = 1)  and pay.arenda_id > 0
            GROUP BY pay.report_id,pay.rent_period_id) RentPaymentProperties1 ON RentPaymentProperties1.report_id = rep.report_id and RentPaymentProperties1.rent_period_id = mrp.max_rent_period_id
            
        LEFT JOIN (
        SELECT
            SUM(pay.cmk_sqr_rented) as 'PAY_CMK_SQR'
            ,SUM(pay.cmk_payment_narah) as 'PAY_CMK_NARAH'
            ,SUM(pay.cmk_payment_to_budget) as 'PAY_CMK_BUDGET'
            ,SUM(pay.cmk_rent_debt) as 'PAY_CMK_DEBT'
            ,pay.report_id
        FROM reports1nf_arenda_rented pay
		WHERE pay.is_cmk > 0 and ISNULL(pay.is_deleted, 0) = 0
        GROUP BY pay.report_id) RentPaymentProperties2 ON RentPaymentProperties2.report_id = rep.report_id

		LEFT OUTER JOIN (
select obp.org_id,occ.name from org_by_period obp
join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
join dict_rent_occupation occ on occ.id = obp.org_occupation_id
		) DDD ON DDD.org_id = rep.organization_id


        WHERE 
            (@p_rda_district_id = 0 OR (rep.org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND rep.org_district_id = @p_rda_district_id))
                AND
            (@p_misto_id = 0 OR rep.old_organ_id = @p_misto_id)
				AND
			( (@smode = 0) OR (@smode = 1 and obj.NumOfObj > 0) OR (@smode = 2 and isnull(obj.NumOfObj,0) <= 0) )
                AND
            ( (@p_show_neziznacheni = 1) OR (@p_show_neziznacheni = 0 AND (isnull(ddd.name, 'Невідомо') <> 'Невизначені')) ) 
                AND
            ( (@p_show_neviznacheni = 1) OR (@p_show_neviznacheni = 0 AND (isnull(ddd.name, '') not in ('Невизначені','АРХИВНІ(ПРИПИНЕНІ)'))) ) 
                AND
			(NUM_GIVEN > 0)
				AND
			(otdel_gukv = 'ДІЮЧЕ')
				AND
            (8888 = 8888)
                AND
    case when rep.zkpo_code in 
(
SELECT
	distinct
    org.zkpo_code
FROM reports1nf_accounts acc
INNER JOIN aspnet_Users usr ON usr.UserId = acc.UserId
INNER JOIN aspnet_Membership mem ON mem.UserId = acc.UserId
LEFT OUTER JOIN organizations org ON org.id = acc.organization_id
LEFT OUTER JOIN dict_districts2 rda ON rda.id = rda_district_id
LEFT OUTER JOIN dict_org_old_organ misto ON misto.id = misto_district_id
) then 1 else 0 end = 1

order by case when exists (select 1 from reports1nf_org_info_new_contribution_rate Q where Q.report_id = rep.report_id) then 1 else 2 end, zkpo_code

	"
		OnSelecting="SqlDataSourceReports_Selecting"
		DeleteCommand="DELETE FROM [valsndogovor] WHERE id = @id"
		UpdateCommand="
	DELETE from [reports1nf_org_info_new_contribution_rate] WHERE report_id = @report_id;
	IF @new_contribution_rate IS NOT NULL
		INSERT INTO [reports1nf_org_info_new_contribution_rate] VALUES(@report_id, @new_contribution_rate);
	"
		OnUpdating="SqlDataSourcePrivatisat_Updating"
		InsertCommand="INSERT INTO [valsndogovor]
    ([nomer_zapis]
    ,[priyom_date]
    ,[nazva_vlasn]
    ,[address]
    ,[plosha]
    ,[result]
    ,[modify_date2]
    ,[modified_by2]
    ) 
    VALUES
    (@nomer_zapis
    ,@priyom_date
    ,@nazva_vlasn
    ,@address
    ,@plosha
    ,@result
    ,@modify_date2
    ,@modified_by2
    );
SELECT SCOPE_IDENTITY()"
		OnInserting="SqlDataSourcePrivatisat_Inserting">
		<SelectParameters>
			<asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
			<asp:Parameter DbType="Int32" DefaultValue="0" Name="period_year" />
			<asp:Parameter DbType="Int32" DefaultValue="0" Name="p_misto_id" />
			<asp:Parameter DbType="Int32" DefaultValue="0" Name="smode" />
			<asp:Parameter DbType="Int32" DefaultValue="0" Name="p_show_neziznacheni" />
			<asp:Parameter DbType="Int32" DefaultValue="0" Name="p_show_neviznacheni" />
		</SelectParameters>
	</mini:ProfiledSqlDataSource>

	<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgInfo" runat="server"
		ConnectionString="<%$ ConnectionStrings:GUKV2016ConnectionString %>"
		SelectCommand="select report_id, zkpo_code, zkpo_code + ' - ' + isnull(short_name,'') as nam from reports1nf_org_info order by zkpo_code">
	</mini:ProfiledSqlDataSource>


	<mini:ProfiledSqlDataSource ID="SqlDataSourceDistrict" runat="server"
		ConnectionString="<%$ ConnectionStrings:GUKV2016ConnectionString %>"
		SelectCommand="SELECT id, name FROM dict_1nf_districts2 where id < 400 ORDER BY name">
	</mini:ProfiledSqlDataSource>


	<mini:ProfiledSqlDataSource ID="SqlDataSourceStreet" runat="server"
		ConnectionString="<%$ ConnectionStrings:GUKV2016ConnectionString %>"
		SelectCommand="select id, name from dict_streets where (name is not null) and (RTRIM(LTRIM(name)) <> '') order by name">
	</mini:ProfiledSqlDataSource>


	<mini:ProfiledSqlDataSource ID="SqlDataSourceFreecycleStepDict" runat="server"
		ConnectionString="<%$ ConnectionStrings:GUKV2016ConnectionString %>"
		SelectCommand="SELECT step_id, step_name, step_cod, step_ord, istitle FROM freecycle_step_dict union select null, '<пусто>', '00', -1, 0 ORDER BY step_ord">
	</mini:ProfiledSqlDataSource>

	<mini:ProfiledSqlDataSource ID="SqlDataSourceUsingPossible" runat="server"
		ConnectionString="<%$ ConnectionStrings:GUKV2016ConnectionString %>"
		SelectCommand="SELECT id, left(full_name, 150) as name, rental_rate, 1 as ordrow FROM dict_rental_rate union select null, '<пусто>', null, 2 as ordrow ORDER BY ordrow, name">
	</mini:ProfiledSqlDataSource>

	<mini:ProfiledSqlDataSource ID="SqlDataSourceIncludeInPerelik" runat="server"
		ConnectionString="<%$ ConnectionStrings:GUKV2016ConnectionString %>"
		SelectCommand="SELECT '1' id, '1' name, 1 as ordrow union SELECT '2' id, '2' name, 1 as ordrow union select null, '',  2 as ordrow ORDER BY ordrow, name">
	</mini:ProfiledSqlDataSource>

	<textarea rows="2" cols="2" id="inpit-for-copy-clipboard" style="display: none2; width: 1px; height: 1px; position: absolute; top: 1px; right: 1px; z-index: -1"></textarea>

	<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="false">
		<Items>
			<dx:MenuItem NavigateUrl="../Reports1NF/Report1NFList.aspx" Text="Звіти Балансоутримувачів"></dx:MenuItem>
			<dx:MenuItem NavigateUrl="../Reports1NF/Report1NFAccounts.aspx" Text="Облікові Записи"></dx:MenuItem>
			<dx:MenuItem NavigateUrl="../Reports1NF/Report1NFNotifications.aspx" Text="Налаштування Повідомлень"></dx:MenuItem>
			<dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem>
			<dx:MenuItem NavigateUrl="../Reports1NF/Report1NFFreeSquare.aspx" Text="Перелік вільних приміщень"></dx:MenuItem>
			<dx:MenuItem NavigateUrl="../Reports1NF/Report1NFPrivatisatSquare.aspx" Text="Об'єкти приватизації"></dx:MenuItem>
			<dx:MenuItem NavigateUrl="../Reports1NF/Report1NFDogContinue.aspx" Text="Продовження договорів"></dx:MenuItem>
		</Items>
	</dx:ASPxMenu>

	<dx:ASPxMenu ID="SectionMenuForRDARole" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="false">
		<Items>
			<dx:MenuItem NavigateUrl="../Reports1NF/Report1NFList.aspx" Text="Звіти Балансоутримувачів"></dx:MenuItem>
			<dx:MenuItem NavigateUrl="../Reports1NF/Report1NFFreeSquare.aspx" Text="Перелік вільних приміщень"></dx:MenuItem>
			<dx:MenuItem NavigateUrl="../Reports1NF/Report1NFPrivatisatSquare.aspx" Text="Об'єкти приватизації"></dx:MenuItem>
			<dx:MenuItem NavigateUrl="../Reports1NF/Report1NFDogContinue.aspx" Text="Продовження договорів"></dx:MenuItem>
		</Items>
	</dx:ASPxMenu>

	<table border="0" cellspacing="4" cellpadding="0" width="100%">
		<tr>
			<td style="width: 100%;">
				<asp:Label ID="LabelReportTitle1" runat="server" Text="Прогноз впливу нормативно-управлінських рішень на рівень надходження орендної плати" CssClass="reporttitle"></asp:Label>
			</td>
			<td>
				<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False"
					Text="Додаткові Колонки" Width="148px" Visible="false">
					<ClientSideEvents Click="ShowFieldChooserPopupControl" />
				</dx:ASPxButton>
			</td>
			<td>
				<dx:ASPxCheckBox ID="CheckBoxInflation" runat="server" Checked='False' Text="Використовувати індекс інфляції"
					Width="230px" ClientInstanceName="CheckBoxInflation" Visible="false">
				</dx:ASPxCheckBox>
			</td>
			<td>
				<dx:ASPxCheckBox ID="CheckBoxDictRentalRate" runat="server" Checked='False' Text="Використовувати нову ставку за використання"
					Width="320px" ClientInstanceName="CheckBoxDictRentalRate" Visible="false">
				</dx:ASPxCheckBox>
			</td>
			<td valign="bottom">
				<div style="display:flex">
					<div style="white-space:nowrap; vertical-align:bottom; color: black; margin-right:3px; align-content:center">
						Нова ставка відрахувань до бюджету (%)
					</div>
					<dx:ASPxSpinEdit ID="EditChange" ClientInstanceName="EditCollectionDebtZvit" runat="server" NumberType="Integer" Width="100px" >

					</dx:ASPxSpinEdit>
					<div style="width:5px"></div>
					<dx:ASPxButton ID="ASPxButton_change" runat="server" AutoPostBack="False" Text="Змінити" OnClick="ASPxButton_change_Click">
						<%--<ClientSideEvents Click="function(s, e) { alert(1) }" />--%>
					</dx:ASPxButton>

					<div style="width:30px"></div>
				</div>
			</td>
			<td>
				<dx:ASPxCallback ID="CallbackRecalculate" runat="server" OnCallback="ASPxButton_Recalculate_Click" ClientInstanceName="cbRecalc">
					<ClientSideEvents 
						CallbackComplete="function(s, e) {
							hideOverlay();
						}"
						CallbackError="function(s, e) {
							hideOverlay();
							//console.log('error', e.message);
							e.handled = true;
							alert('Помилка: ' + e.message);
						}"
					/>
				</dx:ASPxCallback>

				<dx:ASPxButton ID="ASPxButton_Recalculate" runat="server" AutoPostBack="False" Text="Перерахувати"
							Width="148px"
							ClientSideEvents-Click="function(s, e) {
								showOverlay();
								cbRecalc.PerformCallback();
							}" />
			</td>
			<td>
				<div style="width:30px"></div>
			</td>
			<td>
				<dx:ASPxPopupControl
					ID="ASPxPopupControl_FreeSquare_SaveAs" runat="server"
					HeaderText="Збереження у Файлі"
					ClientInstanceName="ASPxPopupControl_FreeSquare_SaveAs"
					PopupElementID="ASPxButton_FreeSquare_SaveAs">
					<ContentCollection>
						<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
							<dx:ASPxButton ID="ASPxButton_FreeSquare_ExportXLS" runat="server"
								Text="XLS - Microsoft Excel&reg;"
								OnClick="ASPxButton_FreeSquare_ExportXLS_Click" Width="180px">
							</dx:ASPxButton>
							<br />
							<dx:ASPxButton ID="ASPxButton_FreeSquare_ExportPDF" runat="server"
								Text="PDF - Adobe Acrobat&reg;"
								OnClick="ASPxButton_FreeSquare_ExportPDF_Click" Width="180px">
							</dx:ASPxButton>
							<br />
							<dx:ASPxButton ID="ASPxButton_FreeSquare_ExportCSV" runat="server"
								Text="CSV - значення, розділені комами"
								OnClick="ASPxButton_FreeSquare_ExportCSV_Click" Width="180px">
							</dx:ASPxButton>
						</dx:PopupControlContentControl>
					</ContentCollection>
				</dx:ASPxPopupControl>

				<dx:ASPxButton ID="ASPxButton_Report" runat="server" AutoPostBack="False" Text="Звіт"
					Width="148px" OnClick="ASPxButton_Report_Click">
				</dx:ASPxButton>

				<dx:ASPxButton ID="ASPxButton_FreeSquare_SaveAs" runat="server" AutoPostBack="False" Visible="false"
					Text="Зберегти у Файлі" Width="148px">
				</dx:ASPxButton>

				<dx:ASPxPopupControl ID="ASPxPopupControlFreeSquare" runat="server" AllowDragging="True"
					ClientInstanceName="PopupObjectPhotos" EnableClientSideAPI="True"
					HeaderText="Документ" Modal="True"
					PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"
					PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px">
					<ContentCollection>
						<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">

							<asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles" runat="server"
								DeleteMethod="Delete" InsertMethod="Insert"
								OnInserting="ObjectDataSourcePhotoFiles_Inserting"
								SelectMethod="Select"
								TypeName="ExtDataEntry.Models.FileAttachment">
								<DeleteParameters>
									<asp:Parameter DefaultValue="privatisat_documents" Name="scope" Type="String" />
									<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
									<asp:Parameter Name="id" Type="String" />
								</DeleteParameters>
								<InsertParameters>
									<asp:Parameter DefaultValue="privatisat_documents" Name="scope" Type="String" />
									<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
									<asp:Parameter Name="Name" Type="String" />
									<asp:Parameter Name="Image" Type="Object" />
								</InsertParameters>
								<SelectParameters>
									<asp:Parameter DefaultValue="privatisat_documents" Name="scope" Type="String" />
									<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
								</SelectParameters>
							</asp:ObjectDataSource>

							<dx:ASPxFileManager ID="ASPxFileManagerPhotoFiles" runat="server"
								ClientInstanceName="ASPxFileManagerPhotoFiles" DataSourceID="ObjectDataSourcePhotoFiles">
								<Settings RootFolder="~\" ThumbnailFolder="~\Thumb\" />
								<SettingsFileList>
									<ThumbnailsViewSettings ThumbnailSize="180px" />
								</SettingsFileList>
								<SettingsEditing AllowDelete="True" AllowDownload="true" />
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

	<dx:ASPxGridViewExporter ID="GridViewFreeSquareExporter" runat="server"
		FileName="Приватизація" GridViewID="PrivatisatGridView" PaperKind="A4"
		BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
		<Styles>
			<Default Font-Names="Calibri,Verdana,Sans Serif">
			</Default>
			<AlternatingRowCell BackColor="#E0E0E0">
			</AlternatingRowCell>
		</Styles>
	</dx:ASPxGridViewExporter>

	<dx:ASPxGridView ID="ASPxGridInflation" runat="server" AutoGenerateColumns="False"
		DataSourceID="SqlDataSourceInflation" KeyFieldName="id" Width="100%"
		ClientInstanceName="InflationGridView">

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
		<SettingsEditing Mode="Inline" />

		<Columns>
			<dx:GridViewCommandColumn Width="70px" ButtonType="Image" CellStyle-Wrap="True" FixedStyle="Left" CellStyle-CssClass="command-column-class"
				ShowDeleteButton="false" ShowCancelButton="true" ShowUpdateButton="true" ShowEditButton="true" ShowNewButton="false">
				<CellStyle Wrap="False"></CellStyle>
			</dx:GridViewCommandColumn>

			<dx:GridViewDataSpinEditColumn FieldName="prognoz_inflation_this" Caption="Прогноз індексу інфляції на 4 квартал 2024 року, %" Width="250px" Visible="false" />
			<dx:GridViewDataSpinEditColumn FieldName="prognoz_inflation_next" Caption="Прогноз індексу інфляції на 2025 рік, %" Width="250px" Visible="false" />

			<dx:GridViewDataSpinEditColumn FieldName="prognoz_inflation_1" Caption="Прогноз індексу інфляції на 2025 рік, %" Width="250px" />
			<dx:GridViewDataSpinEditColumn FieldName="prognoz_inflation_2" Caption="Прогноз індексу інфляції на 2026 рік, %" Width="250px" />
			<dx:GridViewDataSpinEditColumn FieldName="prognoz_inflation_3" Caption="Прогноз індексу інфляції на 2027 рік, %" Width="250px" />
			<dx:GridViewDataSpinEditColumn FieldName="prognoz_inflation_4" Caption="Прогноз індексу інфляції на 2028 рік, %" Width="250px" />

			<dx:GridViewDataComboBoxColumn FieldName="rozrah_persion" Caption="Розрахунковий період" Width="300px" Visible="false">
				<PropertiesComboBox 
					DataSourceID="SqlDataSourceDictStanRecieve"
					DropDownStyle="DropDownList"
					DropDownWidth="300px"
					TextField="name"  
					ValueField="id">
				</PropertiesComboBox>  
			</dx:GridViewDataComboBoxColumn>

<%--
	<ClientSideEvents DropDown="function(s, e) {
						OnDropDown(s);
					}" />--%>


		</Columns>


		<SettingsBehavior ConfirmDelete="True" />
		<SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
		<SettingsPager AlwaysShowPager="true" PageSize="25"></SettingsPager>
		<SettingsPopup>
			<HeaderFilter Width="200" Height="300" />
		</SettingsPopup>
		<Settings
			ShowFilterRow="False"
			ShowFilterRowMenu="False"
			ShowGroupPanel="False"
			ShowFilterBar="Hidden"
			ShowHeaderFilterButton="False"
			ShowTitlePanel="false"
			HorizontalScrollBarMode="Auto"
			ShowFooter="false"
			VerticalScrollBarMode="Auto"
			VerticalScrollBarStyle="Standard" />
		<Styles Header-Wrap="True">
			<Header Wrap="True"></Header>
		</Styles>

	</dx:ASPxGridView>

	<dx:ASPxGridView ID="PrivatisatGridView" runat="server" AutoGenerateColumns="False"
		DataSourceID="SqlDataSourcePrivatisat" KeyFieldName="report_id" Width="100%"
		ClientInstanceName="PrivatisatGridView">
		<ClientSideEvents CustomButtonClick="ShowPhoto" />

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
			<dx:GridViewCommandColumn Width="70px" ButtonType="Image" CellStyle-Wrap="True" FixedStyle="Left" CellStyle-CssClass="command-column-class"
				ShowDeleteButton="false" ShowCancelButton="true" ShowUpdateButton="true" ShowEditButton="true" ShowNewButton="false">
				<%--<CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="btnPdfBuild" Text="Pdf"> 
					<Image Url="~/Styles/PdfReportIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnJpegBuild" Text="Jpeg" Visibility="Invisible"> 
					<Image Url="~/Styles/PhotoIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnMapShow" Text="Показати на мапі"> 
					<Image Url="~/Styles/MapShowIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
				<dx:GridViewCommandColumnCustomButton ID="btnCopyFullDescription" Text="Опис об'єкта до буфера обміну"> 
					<Image Url="~/Styles/CopyIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>--%>
				<CellStyle Wrap="False"></CellStyle>
			</dx:GridViewCommandColumn>


			<dx:GridViewDataTextColumn FieldName="full_name" Caption="Назва Організації" Width="200px" ReadOnly="true">
				<EditItemTemplate>
					<dx:ASPxLabel runat="server" Text='<%# Eval("full_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
				</EditItemTemplate>
			</dx:GridViewDataTextColumn>

			<dx:GridViewDataTextColumn FieldName="zkpo_code" Caption="Код ЄДРПОУ" Width="90px" ReadOnly="true">
				<EditItemTemplate>
					<dx:ASPxLabel runat="server" Text='<%# Eval("zkpo_code") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
				</EditItemTemplate>
			</dx:GridViewDataTextColumn>


			<dx:GridViewDataSpinEditColumn FieldName="contribution_rate" Caption="Ставка відрахувань до бюджету (%)" Width="150px" Visible="true">
				<EditItemTemplate>
					<dx:ASPxLabel runat="server" Text='<%# Eval("contribution_rate") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
				</EditItemTemplate>
			</dx:GridViewDataSpinEditColumn>

			<dx:GridViewDataSpinEditColumn FieldName="new_contribution_rate" Caption="Нова ставка відрахувань до бюджету (%)" Width="150px">
			</dx:GridViewDataSpinEditColumn>

			<dx:GridViewDataTextColumn FieldName="dict_rent_occupation_name" ReadOnly="True" ShowInCustomizationForm="True" Caption="Сфера діяльності">
				<EditItemTemplate>
					<dx:ASPxLabel runat="server" Text='<%# Eval("dict_rent_occupation_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
				</EditItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="industry" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="12" Visible="true" Caption="Галузь">
				<EditItemTemplate>
					<dx:ASPxLabel runat="server" Text='<%# Eval("industry") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
				</EditItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="occupation" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="13" Visible="true" Caption="Вид Діяльності" Width="120px">
				<EditItemTemplate>
					<dx:ASPxLabel runat="server" Text='<%# Eval("occupation") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
				</EditItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="form_gosp" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="16" Visible="true" Caption="Форма фінансування">
				<EditItemTemplate>
					<dx:ASPxLabel runat="server" Text='<%# Eval("form_gosp") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
				</EditItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="form_of_ownership" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="17" Visible="true" Caption="Форма Власності">
				<EditItemTemplate>
					<dx:ASPxLabel runat="server" Text='<%# Eval("form_of_ownership") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
				</EditItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="org_form" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="20" Visible="true" Caption="Орг.-правова форма госп.">
				<EditItemTemplate>
					<dx:ASPxLabel runat="server" Text='<%# Eval("org_form") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
				</EditItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="old_organ" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="21" Visible="true" Caption="Орган госп. упр.">
				<EditItemTemplate>
					<dx:ASPxLabel runat="server" Text='<%# Eval("old_organ") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
				</EditItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="kved_code" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="32" Visible="true" Caption="КВЕД"></dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="otdel_gukv" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="34" Visible="true" Caption="Стан юр.особи"></dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="NumOfObj" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="41" Caption="Кількість об'єктів на балансі">
				<EditItemTemplate>
					<dx:ASPxLabel runat="server" Text='<%# Eval("NumOfObj") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
				</EditItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="NUM_GIVEN" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="52" Caption="Кількість договорів оренди">
				<EditItemTemplate>
					<dx:ASPxLabel runat="server" Text='<%# Eval("NUM_GIVEN") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
				</EditItemTemplate>
			</dx:GridViewDataTextColumn>

		</Columns>

		<%--    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="total_free_sqr" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="free_sql_usefull" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>--%>

		<SettingsBehavior ConfirmDelete="True" />
		<SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
		<SettingsPager AlwaysShowPager="true" PageSize="25"></SettingsPager>
		<SettingsPopup>
			<HeaderFilter Width="200" Height="300" />
		</SettingsPopup>
		<Settings
			ShowFilterRow="True"
			ShowFilterRowMenu="True"
			ShowGroupPanel="False"
			ShowFilterBar="Visible"
			ShowHeaderFilterButton="True"
			HorizontalScrollBarMode="Auto"
			ShowFooter="false"
			VerticalScrollBarMode="Auto"
			VerticalScrollBarStyle="Standard" />
		<SettingsCookies CookiesID="GUKV.Reports1NF.PrognozPayment" Version="A4_006" Enabled="true" />
		<Styles Header-Wrap="True">
			<Header Wrap="True"></Header>
		</Styles>

		<ClientSideEvents Init="GridViewFreeSquareInit" EndCallback="GridViewFreeSquareEndCallback" />
	</dx:ASPxGridView>

	<dx:ASPxPopupControl ID="PopupFieldChooser" runat="server"
		HeaderText="Додаткові Колонки"
		ClientInstanceName="PopupFieldChooser"
		PopupElementID="PrimaryGridView"
		PopupAction="None"
		PopupHorizontalAlign="Center"
		PopupVerticalAlign="Middle"
		PopupAnimationType="Slide">
		<ContentCollection>
			<dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
				<uc3:FieldChooser ID="FieldChooser1" runat="server" />
			</dx:PopupControlContentControl>
		</ContentCollection>
		<ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
	</dx:ASPxPopupControl>

	<div id="blockingOverlay" style="display:none;position:fixed;top:0;left:0;width:100%;height:100%;z-index:9999;background-color:rgba(255,255,255,0.8);text-align:center;padding-top:200px;font-size:20px;">
		Йде обробка... Зачекайте, будь ласка
	</div>

</asp:Content>
