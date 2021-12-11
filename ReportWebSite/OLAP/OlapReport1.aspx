<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OlapReport1.aspx.cs" Inherits="Assessment_AssessmentObjects" MasterPageFile="~/NoHeader.master" Title="Оцінка Об'єктів"%>

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
<dxcharts:WebChartControl ID="WebChartControl1" runat="server" Height="400px"
        Width="800px" 
        ClientInstanceName="chart" 
        ToolTipEnabled="False" CrosshairEnabled="True" RenderFormat="Svg">
        <Legend Name="Default Legend"></Legend>
        <SeriesSerializable>
            <dxcharts:Series Name="Район" LegendTextPattern="{A}" >
                <Points>
<%--                    <dxcharts:SeriesPoint Values="17.0752" ArgumentSerializable="Russia" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="9.98467" ArgumentSerializable="Canada" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="9.63142" ArgumentSerializable="USA" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="9.59696" ArgumentSerializable="China" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="8.511965" ArgumentSerializable="Brazil" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="7.68685" ArgumentSerializable="Australia" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="3.28759" ArgumentSerializable="India" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="81.2" ArgumentSerializable="Others" ></dxcharts:SeriesPoint>--%>
                </Points>
                <ViewSerializable>
                    <dxcharts:PieSeriesView Rotation="90" RuntimeExploding="True">
                        <titles>
                            <dxcharts:SeriesTitle Dock="Bottom" Text="Всього: {TV:#,##0} грн." />
                        </titles>
                    </dxcharts:PieSeriesView>
                </ViewSerializable>
                <LabelSerializable>
                    <dxcharts:PieSeriesLabel Position="Radial" ColumnIndent="20" TextColor="Black" BackColor="Transparent" Font="Tahoma, 6pt, style=Bold" TextPattern="{TV:#,##0} грн. ({VP:P1})">
                        <Border Visibility="False"></Border>
                    </dxcharts:PieSeriesLabel>
                </LabelSerializable>
            </dxcharts:Series>
        </SeriesSerializable>
        <ClientSideEvents
            ObjectSelected="function(s, e) {
                                var hitInPie = e.hitInfo.inSeries &amp;&amp; !e.hitInfo.inLegend;
                                if(hitInPie) {
                                    var itemCount = cbExplodedPoints.GetItemCount();
                                    cbExplodedPoints.SetSelectedIndex(itemCount - 1);
                                }
                                e.processOnServer = hitInPie;
                            }"
            ObjectHotTracked="function(s, e) {
                                var hitInPie = e.hitInfo.inSeries &amp;&amp; !e.hitInfo.inLegend;
                                s.SetCursor(hitInPie ? 'pointer' : 'default');
                            }" />
        <BorderOptions Visibility="False" />
        <Titles>
            <dxcharts:ChartTitle Text="2020 р."></dxcharts:ChartTitle>
            <%--<dxcharts:ChartTitle Dock="Bottom" Alignment="Far" Text="From www.nationmaster.com" Font="Tahoma, 6pt" TextColor="Gray"></dxcharts:ChartTitle>--%>
        </Titles>
        <DiagramSerializable>
            <dxcharts:SimpleDiagram></dxcharts:SimpleDiagram>
        </DiagramSerializable>
    </dxcharts:WebChartControl>

		</td>

		<td>
	<dxcharts:WebChartControl ID="WebChartControl2" runat="server" Height="400px"
        Width="800px" 
        ClientInstanceName="chart" 
        ToolTipEnabled="False" CrosshairEnabled="True" RenderFormat="Svg">
        <Legend Name="Default Legend"></Legend>
        <SeriesSerializable>
            <dxcharts:Series Name="Район" LegendTextPattern="{A}" >
                <Points>
<%--                    <dxcharts:SeriesPoint Values="17.0752" ArgumentSerializable="Russia" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="9.98467" ArgumentSerializable="Canada" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="9.63142" ArgumentSerializable="USA" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="9.59696" ArgumentSerializable="China" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="8.511965" ArgumentSerializable="Brazil" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="7.68685" ArgumentSerializable="Australia" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="3.28759" ArgumentSerializable="India" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="81.2" ArgumentSerializable="Others" ></dxcharts:SeriesPoint>--%>
                </Points>
                <ViewSerializable>
                    <dxcharts:PieSeriesView Rotation="90" RuntimeExploding="True">
                        <titles>
                            <dxcharts:SeriesTitle Dock="Bottom" Text="Всього: {TV:#,##0} грн." />
                        </titles>
                    </dxcharts:PieSeriesView>
                </ViewSerializable>
                <LabelSerializable>
                    <dxcharts:PieSeriesLabel Position="Radial" ColumnIndent="20" TextColor="Black" BackColor="Transparent" Font="Tahoma, 6pt, style=Bold" TextPattern="{TV:#,##0} грн. ({VP:P1})">
                        <Border Visibility="False"></Border>
                    </dxcharts:PieSeriesLabel>
                </LabelSerializable>
            </dxcharts:Series>
        </SeriesSerializable>
        <ClientSideEvents
            ObjectSelected="function(s, e) {
                                var hitInPie = e.hitInfo.inSeries &amp;&amp; !e.hitInfo.inLegend;
                                if(hitInPie) {
                                    var itemCount = cbExplodedPoints.GetItemCount();
                                    cbExplodedPoints.SetSelectedIndex(itemCount - 1);
                                }
                                e.processOnServer = hitInPie;
                            }"
            ObjectHotTracked="function(s, e) {
                                var hitInPie = e.hitInfo.inSeries &amp;&amp; !e.hitInfo.inLegend;
                                s.SetCursor(hitInPie ? 'pointer' : 'default');
                            }" />
        <BorderOptions Visibility="False" />
        <Titles>
            <dxcharts:ChartTitle Text="2019 р."></dxcharts:ChartTitle>
            <%--<dxcharts:ChartTitle Dock="Bottom" Alignment="Far" Text="From www.nationmaster.com" Font="Tahoma, 6pt" TextColor="Gray"></dxcharts:ChartTitle>--%>
        </Titles>
        <DiagramSerializable>
            <dxcharts:SimpleDiagram></dxcharts:SimpleDiagram>
        </DiagramSerializable>
    </dxcharts:WebChartControl>

		</td>
	</tr>

	<tr>
		<td>
<dxcharts:WebChartControl ID="WebChartControl3" runat="server" Height="400px"
        Width="800px" 
        ClientInstanceName="chart" 
        ToolTipEnabled="False" CrosshairEnabled="True" RenderFormat="Svg">
        <Legend Name="Default Legend"></Legend>
        <SeriesSerializable>
            <dxcharts:Series Name="Район" LegendTextPattern="{A}" >
                <Points>
<%--                    <dxcharts:SeriesPoint Values="17.0752" ArgumentSerializable="Russia" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="9.98467" ArgumentSerializable="Canada" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="9.63142" ArgumentSerializable="USA" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="9.59696" ArgumentSerializable="China" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="8.511965" ArgumentSerializable="Brazil" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="7.68685" ArgumentSerializable="Australia" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="3.28759" ArgumentSerializable="India" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="81.2" ArgumentSerializable="Others" ></dxcharts:SeriesPoint>--%>
                </Points>
                <ViewSerializable>
                    <dxcharts:PieSeriesView Rotation="90" RuntimeExploding="True">
                        <titles>
                            <dxcharts:SeriesTitle Dock="Bottom" Text="Всього: {TV:#,##0} грн." />
                        </titles>
                    </dxcharts:PieSeriesView>
                </ViewSerializable>
                <LabelSerializable>
                    <dxcharts:PieSeriesLabel Position="Radial" ColumnIndent="20" TextColor="Black" BackColor="Transparent" Font="Tahoma, 6pt, style=Bold" TextPattern="{TV:#,##0} грн. ({VP:P1})">
                        <Border Visibility="False"></Border>
                    </dxcharts:PieSeriesLabel>
                </LabelSerializable>
            </dxcharts:Series>
        </SeriesSerializable>
        <ClientSideEvents
            ObjectSelected="function(s, e) {
                                var hitInPie = e.hitInfo.inSeries &amp;&amp; !e.hitInfo.inLegend;
                                if(hitInPie) {
                                    var itemCount = cbExplodedPoints.GetItemCount();
                                    cbExplodedPoints.SetSelectedIndex(itemCount - 1);
                                }
                                e.processOnServer = hitInPie;
                            }"
            ObjectHotTracked="function(s, e) {
                                var hitInPie = e.hitInfo.inSeries &amp;&amp; !e.hitInfo.inLegend;
                                s.SetCursor(hitInPie ? 'pointer' : 'default');
                            }" />
        <BorderOptions Visibility="False" />
        <Titles>
            <dxcharts:ChartTitle Text="2018 р."></dxcharts:ChartTitle>
            <%--<dxcharts:ChartTitle Dock="Bottom" Alignment="Far" Text="From www.nationmaster.com" Font="Tahoma, 6pt" TextColor="Gray"></dxcharts:ChartTitle>--%>
        </Titles>
        <DiagramSerializable>
            <dxcharts:SimpleDiagram></dxcharts:SimpleDiagram>
        </DiagramSerializable>
    </dxcharts:WebChartControl>

		</td>

		<td>
	<dxcharts:WebChartControl ID="WebChartControl4" runat="server" Height="400px"
        Width="800px" 
        ClientInstanceName="chart" 
        ToolTipEnabled="False" CrosshairEnabled="True" RenderFormat="Svg">
        <Legend Name="Default Legend"></Legend>
        <SeriesSerializable>
            <dxcharts:Series Name="Район" LegendTextPattern="{A}" >
                <Points>
<%--                    <dxcharts:SeriesPoint Values="17.0752" ArgumentSerializable="Russia" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="9.98467" ArgumentSerializable="Canada" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="9.63142" ArgumentSerializable="USA" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="9.59696" ArgumentSerializable="China" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="8.511965" ArgumentSerializable="Brazil" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="7.68685" ArgumentSerializable="Australia" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="3.28759" ArgumentSerializable="India" ></dxcharts:SeriesPoint>
                    <dxcharts:SeriesPoint Values="81.2" ArgumentSerializable="Others" ></dxcharts:SeriesPoint>--%>
                </Points>
                <ViewSerializable>
                    <dxcharts:PieSeriesView Rotation="90" RuntimeExploding="True">
                        <titles>
                            <dxcharts:SeriesTitle Dock="Bottom" Text="Всього: {TV:#,##0} грн." />
                        </titles>
                    </dxcharts:PieSeriesView>
                </ViewSerializable>
                <LabelSerializable>
                    <dxcharts:PieSeriesLabel Position="Radial" ColumnIndent="20" TextColor="Black" BackColor="Transparent" Font="Tahoma, 6pt, style=Bold" TextPattern="{TV:#,##0} грн. ({VP:P1})">
                        <Border Visibility="False"></Border>
                    </dxcharts:PieSeriesLabel>
                </LabelSerializable>
            </dxcharts:Series>
        </SeriesSerializable>
        <ClientSideEvents
            ObjectSelected="function(s, e) {
                                var hitInPie = e.hitInfo.inSeries &amp;&amp; !e.hitInfo.inLegend;
                                if(hitInPie) {
                                    var itemCount = cbExplodedPoints.GetItemCount();
                                    cbExplodedPoints.SetSelectedIndex(itemCount - 1);
                                }
                                e.processOnServer = hitInPie;
                            }"
            ObjectHotTracked="function(s, e) {
                                var hitInPie = e.hitInfo.inSeries &amp;&amp; !e.hitInfo.inLegend;
                                s.SetCursor(hitInPie ? 'pointer' : 'default');
                            }" />
        <BorderOptions Visibility="False" />
        <Titles>
            <dxcharts:ChartTitle Text="2017 р."></dxcharts:ChartTitle>
            <%--<dxcharts:ChartTitle Dock="Bottom" Alignment="Far" Text="From www.nationmaster.com" Font="Tahoma, 6pt" TextColor="Gray"></dxcharts:ChartTitle>--%>
        </Titles>
        <DiagramSerializable>
            <dxcharts:SimpleDiagram></dxcharts:SimpleDiagram>
        </DiagramSerializable>
    </dxcharts:WebChartControl>

		</td>
	</tr>
</table>


</center>



</asp:Content>
