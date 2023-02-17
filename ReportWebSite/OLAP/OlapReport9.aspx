<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OlapReport9.aspx.cs" Inherits="Assessment_AssessmentObjects" MasterPageFile="~/NoHeader.master" Title="Оцінка Об'єктів"%>

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
<dx:WebChartControl ID="WebChartControl_2022" runat="server" Height="500px"
        Width="1500px" ClientInstanceName="chart"
        CrosshairEnabled="False" ToolTipEnabled="true" RenderFormat="Svg">

        <Titles>
            <dx:ChartTitle Text="Заборгованість по орендній платі (10 найбільших боржників) за 2022 рік" Font="Tahoma, 14pt"></dx:ChartTitle>
        </Titles>
		
        <DiagramSerializable>
            <dx:XYDiagram Rotated="True">
                <AxisX VisibleInPanesSerializable="-1">
                </AxisX>
                <AxisY Title-Text="млн." Title-Visibility="True" VisibleInPanesSerializable="-1">
                    <Label TextPattern="{V:0}"/>
                    <GridLines MinorVisible="True"></GridLines>
                </AxisY>
                <defaultpane>
                    <stackedbartotallabel textpattern="всього {TV:0.0}" visible="true">
                    </stackedbartotallabel>
                </defaultpane>
            </dx:XYDiagram>
        </DiagramSerializable>

		<Legend AlignmentHorizontal="Right"  />

    </dx:WebChartControl>

		</td>

	</tr>

	<tr>
		<td>
<dx:WebChartControl ID="WebChartControl_2021" runat="server" Height="500px"
        Width="1500px" ClientInstanceName="chart"
        CrosshairEnabled="False" ToolTipEnabled="true" RenderFormat="Svg">

        <Titles>
            <dx:ChartTitle Text="Заборгованість по орендній платі (10 найбільших боржників) за 2021 рік" Font="Tahoma, 14pt"></dx:ChartTitle>
        </Titles>
		
        <DiagramSerializable>
            <dx:XYDiagram Rotated="True">
                <AxisX VisibleInPanesSerializable="-1">
                </AxisX>
                <AxisY Title-Text="млн." Title-Visibility="True" VisibleInPanesSerializable="-1">
                    <Label TextPattern="{V:0}"/>
                    <GridLines MinorVisible="True"></GridLines>
                </AxisY>
                <defaultpane>
                    <stackedbartotallabel textpattern="всього {TV:0.0}" visible="true">
                    </stackedbartotallabel>
                </defaultpane>
            </dx:XYDiagram>
        </DiagramSerializable>

		<Legend AlignmentHorizontal="Right"  />

    </dx:WebChartControl>

		</td>

	</tr>

	<tr>
		<td>
<dx:WebChartControl ID="WebChartControl_2020" runat="server" Height="500px"
        Width="1500px" ClientInstanceName="chart"
        CrosshairEnabled="False" ToolTipEnabled="true" RenderFormat="Svg">

        <Titles>
            <dx:ChartTitle Text="Заборгованість по орендній платі (10 найбільших боржників) за 2020 рік" Font="Tahoma, 14pt"></dx:ChartTitle>
        </Titles>
		
        <DiagramSerializable>
            <dx:XYDiagram Rotated="True">
                <AxisX VisibleInPanesSerializable="-1">
                </AxisX>
                <AxisY Title-Text="млн." Title-Visibility="True" VisibleInPanesSerializable="-1">
                    <Label TextPattern="{V:0}"/>
                    <GridLines MinorVisible="True"></GridLines>
                </AxisY>
                <defaultpane>
                    <stackedbartotallabel textpattern="всього {TV:0.0}" visible="true">
                    </stackedbartotallabel>
                </defaultpane>
            </dx:XYDiagram>
        </DiagramSerializable>

		<Legend AlignmentHorizontal="Right"  />

    </dx:WebChartControl>

		</td>

	</tr>

	<tr>
		<td>
<dx:WebChartControl ID="WebChartControl_2019" runat="server" Height="500px"
        Width="1500px" ClientInstanceName="chart"
        CrosshairEnabled="False" ToolTipEnabled="true" RenderFormat="Svg">

        <Titles>
            <dx:ChartTitle Text="Заборгованість по орендній платі (10 найбільших боржників) за 2019 рік" Font="Tahoma, 14pt"></dx:ChartTitle>
        </Titles>
		
        <DiagramSerializable>
            <dx:XYDiagram Rotated="True">
                <AxisX VisibleInPanesSerializable="-1">
                </AxisX>
                <AxisY Title-Text="млн." Title-Visibility="True" VisibleInPanesSerializable="-1">
                    <Label TextPattern="{V:0}"/>
                    <GridLines MinorVisible="True"></GridLines>
                </AxisY>
                <defaultpane>
                    <stackedbartotallabel textpattern="всього {TV:0.0}" visible="true">
                    </stackedbartotallabel>
                </defaultpane>
            </dx:XYDiagram>
        </DiagramSerializable>

		<Legend AlignmentHorizontal="Right" />

    </dx:WebChartControl>

		</td>

	</tr>

	<tr>
		<td>
<dx:WebChartControl ID="WebChartControl_2018" runat="server" Height="500px"
        Width="1500px" ClientInstanceName="chart"
        CrosshairEnabled="False" ToolTipEnabled="true" RenderFormat="Svg">

        <Titles>
            <dx:ChartTitle Text="Заборгованість по орендній платі (10 найбільших боржників) за 2018 рік" Font="Tahoma, 14pt"></dx:ChartTitle>
        </Titles>
		
        <DiagramSerializable>
            <dx:XYDiagram Rotated="True">
                <AxisX VisibleInPanesSerializable="-1">
                </AxisX>
                <AxisY Title-Text="млн." Title-Visibility="True" VisibleInPanesSerializable="-1">
                    <Label TextPattern="{V:0}"/>
                    <GridLines MinorVisible="True"></GridLines>
                </AxisY>
                <defaultpane>
                    <stackedbartotallabel textpattern="всього {TV:0.0}" visible="true">
                    </stackedbartotallabel>
                </defaultpane>
            </dx:XYDiagram>
        </DiagramSerializable>

		<Legend AlignmentHorizontal="Right" />

    </dx:WebChartControl>

		</td>

	</tr>

	<tr>
		<td>
<dx:WebChartControl ID="WebChartControl_2017" runat="server" Height="500px"
        Width="1500px" ClientInstanceName="chart"
        CrosshairEnabled="False" ToolTipEnabled="true" RenderFormat="Svg">

        <Titles>
            <dx:ChartTitle Text="Заборгованість по орендній платі (10 найбільших боржників) за 2017 рік" Font="Tahoma, 14pt"></dx:ChartTitle>
        </Titles>
		
        <DiagramSerializable>
            <dx:XYDiagram Rotated="True">
                <AxisX VisibleInPanesSerializable="-1">
                </AxisX>
                <AxisY Title-Text="млн." Title-Visibility="True" VisibleInPanesSerializable="-1">
                    <Label TextPattern="{V:0}"/>
                    <GridLines MinorVisible="True"></GridLines>
                </AxisY>
                <defaultpane>
                    <stackedbartotallabel textpattern="всього {TV:0.0}" visible="true">
                    </stackedbartotallabel>
                </defaultpane>
            </dx:XYDiagram>
        </DiagramSerializable>

		<Legend AlignmentHorizontal="Right" />

    </dx:WebChartControl>

		</td>

	</tr>

</table>


</center>



</asp:Content>
