<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OlapReport10.aspx.cs" Inherits="Assessment_AssessmentObjects" MasterPageFile="~/NoHeader.master" Title="Оцінка Об'єктів"%>

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
    SelectCommand="SELECT v2 - v5 - v8 as v101, * FROM OlapReportTotalYear">
</mini:ProfiledSqlDataSource>

<center>


	
<table>
	<tr>
		<td>
<dx:WebChartControl ID="WebChartControl1" runat="server" Height="400px"
        Width="700px" ClientInstanceName="chart"
        CrosshairEnabled="False" ToolTipEnabled="true" RenderFormat="Svg">

        <Titles>
            <dx:ChartTitle Text="Кількість об'єктів на балансі" Font="Tahoma, 14pt"></dx:ChartTitle>
        </Titles>
        <DiagramSerializable>
            <dx:XYDiagram>
                <AxisX VisibleInPanesSerializable="-1"></AxisX>
                <AxisY Title-Text="Percents" VisibleInPanesSerializable="-1">
                    <VisualRange AutoSideMargins="False" SideMarginsValue="0"></VisualRange>
                    <WholeRange Auto="False" AutoSideMargins="False" SideMarginsValue="0" ></WholeRange>
                    <Label TextPattern="{V:P0}"/>
                </AxisY>
                <defaultpane>
                    <stackedbartotallabel textpattern="Total
{TV}K">
                    </stackedbartotallabel>
                </defaultpane>
            </dx:XYDiagram>
        </DiagramSerializable>
    </dx:WebChartControl>

		</td>

	</tr>
</table>


</center>



</asp:Content>
