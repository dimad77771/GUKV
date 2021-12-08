<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OlapReportTotalYear.aspx.cs" Inherits="Assessment_AssessmentObjects" MasterPageFile="~/NoHeader.master" Title="Оцінка Об'єктів"%>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>
<%@ Register src="../UserControls/SaveReportCtrl.ascx" tagname="SaveReportCtrl" tagprefix="uc1" %>
<%@ Register src="../UserControls/AddressPicker.ascx" tagname="AddressPicker" tagprefix="uc2" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register src="../UserControls/FieldFixxer.ascx" tagname="FieldFixxer" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    window.onresize = function () { AdjustGridSizes(); };

    function ShowFieldFixxerPopupControl(s, e) { PopupFieldFixxer.Show(); }
    
    function AdjustGridSizes() {

		pivotGrid.SetHeight(window.innerHeight - 185);
    }

    function GridViewAssessmentObjectsInit(s, e) {

        pivotGrid.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewAssessmentObjectsEndCallback(s, e) {

        AdjustGridSizes();
    }
</script>

<mini:ProfiledSqlDataSource ID="SqlDataOlap" runat="server" EnableCaching="false"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM OlapReportTotalYear">
</mini:ProfiledSqlDataSource>

<center>


	<dx:ASPxPivotGrid ID="pivotGrid" runat="server" ClientInstanceName="pivotGrid" DataSourceID="SqlDataOlap"
			OptionsData-DataProcessingEngine="Optimized" OptionsFilter-FilterPanelMode="Filter"	 
			Width="100%" Height="600px">
			<Fields>
				<dx:PivotGridField Area="RowArea" AreaIndex="0" FieldName="sfera" ID="IDsfera"
					Caption="Сфера діяльності" />
				<dx:PivotGridField Area="RowArea" AreaIndex="1" FieldName="nam" ID="IDnam"
					Caption="Назва Організації" />
				
<%--				<dx:PivotGridField Area="RowArea" AreaIndex="1" FieldName="zkpo" ID="IDzkpo"
					Caption="ЄДРПОУ" />--%>
				<dx:PivotGridField Area="ColumnArea" AreaIndex="0" FieldName="yy" ID="IDyy"
					Caption="Рік" />
				<dx:PivotGridField Area="DataArea" AreaIndex="0" FieldName="v2" ID="IDv2" CellFormat-FormatType="Numeric" CellFormat-FormatString="0"
					Caption="Загальна площа, що знаходиться на балансі, кв.м." />
				<dx:PivotGridField Area="DataArea" AreaIndex="0" FieldName="v3" ID="IDv3" CellFormat-FormatType="Numeric" CellFormat-FormatString="0"
					Caption="Корисна площа, що знаходиться на балансі, кв.м." />
			</Fields>
			<OptionsPager RowsPerPage="50000" ColumnsPerPage="15" Visible="False" />
			<OptionsView VerticalScrollBarMode="Auto" ShowFilterHeaders="false"
				ShowFilterSeparatorBar="True" HorizontalScrollBarMode="Auto" VerticalScrollingMode="Virtual" HorizontalScrollingMode="Virtual" 
				ShowColumnGrandTotals="True"
				ShowRowGrandTotals="True"
				/>
			<%--<Filter CriteriaString="[yy] = 2017"  />--%>
	</dx:ASPxPivotGrid>


</center>



</asp:Content>
