<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssessmentCard.aspx.cs" Inherits="Cards_AssessmentCard"
    MasterPageFile="~/NoHeader.master" Title="Картка Об'єкту Оцінки" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentProperties" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT * FROM expert_note WHERE id = @vid" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="vid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentInputDoc" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT indoc.id AS 'doc_id', indoc.doc_date, indoc.doc_num, indoc.control_date, korr.name AS 'korr', indoc.rezenz_name
        FROM expert_input_doc indoc LEFT OUTER JOIN dict_expert_korr korr ON korr.id = indoc.korrespondent_id
        WHERE indoc.expert_note_id = @vid AND indoc.is_deleted <> 1" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="vid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentOutputDoc" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT outdoc.id AS 'doc_id', outdoc.doc_date, outdoc.doc_num, rt.name AS 'rezenz_type'
        FROM expert_output_doc outdoc LEFT OUTER JOIN dict_expert_rezenz_type rt ON rt.id = outdoc.rezenz_type_id
        WHERE outdoc.expert_note_id = @vid AND outdoc.is_deleted <> 1" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="vid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentDetails" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT d.id AS 'detail_id', d.obj_square, d.cost_1_usd, d.valuation_date, d.floors, d.purpose, d.note 
        FROM expert_note_detail d WHERE d.expert_note_id = @vid AND d.is_deleted <> 1" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="vid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrganizations" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, CONCAT(zkpo_code, ' - ', full_name) name from organizations order by case when zkpo_code <> '' then 1 else 2 end, 2">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceStreets" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id,name from [dict_streets] order by name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictExpertObjType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_expert_obj_type order by 2">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictExpertValuationKind" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_expert_valuation_kind order by 1">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictExpertRezenz" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_expert_rezenz order by 2">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictExpert" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, full_name as name from dict_expert where full_name <> '' order by 2">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDistrict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_districts2 WHERE id < 900 ORDER BY name">
</mini:ProfiledSqlDataSource>


<table border="0" cellspacing="0" cellpadding="0">
<tr>
<td>

<dx:ASPxCallbackPanel ID="CPMainPanel" ClientInstanceName="CPMainPanel" 
        runat="server" OnCallback="CPMainPanel_Callback" onunload="CPMainPanel_Unload">
<PanelCollection>
<dx:PanelContent ID="Panelcontent2" runat="server">
<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server" ActiveTabIndex="0">
    <TabPages>

        <dx:TabPage Text="Відомості про об'єкт" Name="AssessmentCardProperties">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">

<asp:FormView runat="server" BorderStyle="None" ID="ObjDetails" DataSourceID="SqlDataSourceAssessmentProperties" EnableViewState="False">
    <ItemTemplate>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="Адреса Будинку">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
						<tr>
							<td width="100px">
								<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Район"></dx:ASPxLabel>
							</td>
							<td width="8px">&nbsp;</td>
							<td>
								<%--<dx:ASPxTextBox ID="addr_district_id" runat="server" Text='<%# Eval("addr_district_id") %>' Width="290px" />--%>
								<dx:ASPxComboBox ID="addr_district_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px"
									IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceDistrict" Value='<%# Eval("addr_district_id") %>'
									Title="Район"
									DropDownStyle="DropDown" />
							</td>
							<td width="8px">&nbsp;</td>
							<td width="100px">
								<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Вид Об'єкту"></dx:ASPxLabel>
							</td>
							<td width="8px">&nbsp;</td>
							<td>
								<dx:ASPxComboBox ID="expert_obj_type_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px"
									IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceDictExpertObjType" Value='<%# Eval("expert_obj_type_id") %>'
									Title="Вид Об'єкту"
									DropDownStyle="DropDown" />
							</td>
						</tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Назва Вулиці"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
								<dx:ASPxComboBox ID="addr_street_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px"
                                IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceStreets" Value='<%# Eval("addr_street_id") %>'
                                Title="Назва Вулиці"
								DropDownStyle="DropDown"
								EnableCallbackMode="True"
								CallbackPageSize="100" />
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Номер Будинку"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
								<dx:ASPxTextBox ID="ASPxTextBox22" runat="server" Text='<%# Eval("addr_nomer") %>' Width="290px" />
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <br/>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Організації">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
					<table border="0" cellspacing="0" cellpadding="0" width="810px">
						<tr>
							<td width="100px">
								<dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Балансоутримувач"></dx:ASPxLabel>
							</td>
							<td width="8px">&nbsp;</td>
							<td colspan="5">
								<dx:ASPxComboBox ID="org_balans_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="700px"
									IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceOrganizations" Value='<%# Eval("org_balans_id") %>'
									Title="Балансоутримувач"
									DropDownStyle="DropDown"
									EnableCallbackMode="True"
									CallbackPageSize="100" />
							</td>
						</tr>
						<tr>
							<td colspan="7" height="4px" />
						</tr>
						<tr>
							<td width="100px">
								<dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Орендар"></dx:ASPxLabel>
							</td>
							<td width="8px">&nbsp;</td>
							<td colspan="5">
								<dx:ASPxComboBox ID="org_renter_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="700px"
									IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceOrganizations" Value='<%# Eval("org_renter_id") %>'
									Title="Орендар"
									DropDownStyle="DropDown"
									EnableCallbackMode="True"
									CallbackPageSize="100" />
							</td>
						</tr>
					</table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <br/>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" HeaderText="Оцінка">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="СОД"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5">
								<%--<dx:ASPxTextBox ID="ASPxTextBox5" runat="server" Text='<%# Eval("expert_id") %>' Width="700px" />--%>
								<dx:ASPxComboBox ID="expert_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="700px"
									IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceDictExpert" Value='<%# Eval("expert_id") %>'
									Title="Рецензент"
									DropDownStyle="DropDown" />
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Рецензент"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5">
								<dx:ASPxComboBox ID="rezenz_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="700px"
									IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceDictExpertRezenz" Value='<%# Eval("rezenz_id") %>'
									Title="Рецензент"
									DropDownStyle="DropDown" />
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Вид оцінки"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
								<dx:ASPxComboBox ID="valuation_kind_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px"
									IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceDictExpertValuationKind" Value='<%# Eval("valuation_kind_id") %>'
									Title="Вид оцінки"
									DropDownStyle="DropDown" />
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Дата Проведення Оцінки"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
								<dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" Value='<%# Eval("valuation_date") %>' Width="290px" />
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Площа Об'єкту"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox8" runat="server" Text='<%# Eval("obj_square") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Вартість Об'єкту (грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox9" runat="server" Text='<%# Eval("cost_prim") %>' Width="290px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <br/>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Архівний Стан">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td colspan="7" align="left">
                                <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" Checked='<%# (1.Equals(Eval("is_archived"))) ? true : false %>' Text="Переміщено в Архів" />
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Архівний Номер"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox4" runat="server" Text='<%# Eval("arch_num") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Дата Затвердження"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" Value='<%# Eval("final_date") %>' Width="290px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

		<p class="SpacingPara"/>

		<table border="0" cellspacing="0" cellpadding="0">
			<tr>
				<td>
					<dx:ASPxButton ID="ButtonSave" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
						<ClientSideEvents Click="function (s,e) 
						{ 
							CPMainPanel.PerformCallback('save:'); 
						}" />
					</dx:ASPxButton>
				</td>
			</tr>
		</table>

    </ItemTemplate>
</asp:FormView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Вхідна Інформація" Name="AssessmentCardInputDocs">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">

                    <dx:ASPxGridView ID="GridViewAssessmentInputDocs" runat="server" 
                        ClientInstanceName="GridViewAssessmentInputDocs"
                        AutoGenerateColumns="False"
                        DataSourceID="SqlDataSourceAssessmentInputDoc" 
                        KeyFieldName="doc_id"
                        Width="840px" >

                        <GroupSummary>
                            <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
                        </GroupSummary>

                        <Columns>
                            <dx:GridViewDataDateColumn FieldName="doc_date" VisibleIndex="0" Caption="Дата Документа"></dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="doc_num" VisibleIndex="1" Caption="Номер Документа"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="korr" VisibleIndex="2" Caption="Кореспондент"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="control_date" VisibleIndex="3" Caption="Контрольна Дата"></dx:GridViewDataDateColumn>
                            <dx:GridViewDataDateColumn FieldName="rezenz_name" VisibleIndex="4" Caption="Рецензент"></dx:GridViewDataDateColumn>
                        </Columns>

                        <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
                        <SettingsPager PageSize="15">
                        </SettingsPager>
                        <Settings
                            VerticalScrollBarMode="Hidden"
                            VerticalScrollBarStyle="Standard"
                            HorizontalScrollBarMode="Visible"
                            ShowFooter="True" />
                        <SettingsCookies CookiesID="GUKV.AssessmentCard.Input" Version="A2" Enabled="False" />
                        <Styles Header-Wrap="True" >
                            <Header Wrap="True"></Header>
                        </Styles>
                    </dx:ASPxGridView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Характеристика Об'єкта" Name="AssessmentCardDetails">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">

                    <dx:ASPxGridView ID="GridViewAssessmentDetails" runat="server" 
                        ClientInstanceName="GridViewAssessmentDetails"
                        AutoGenerateColumns="False"
                        DataSourceID="SqlDataSourceAssessmentDetails" 
                        KeyFieldName="detail_id"
                        Width="840px" >

                        <GroupSummary>
                            <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
                        </GroupSummary>

                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="floors" VisibleIndex="0" Caption="Поверх"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="obj_square" VisibleIndex="1" Caption="Площа"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="purpose" VisibleIndex="2" Caption="НЕВ"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="cost_1_usd" VisibleIndex="3" Caption="Вартість 1 кв.м., $" Visible="true"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="valuation_date" VisibleIndex="4" Caption="Дата Оцінки"></dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="note" VisibleIndex="5" Caption="Примітка"></dx:GridViewDataTextColumn>
                        </Columns>

                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="obj_square" SummaryType="Sum" DisplayFormat="{0}" />
                            <dx:ASPxSummaryItem FieldName="cost_1_usd" SummaryType="Sum" DisplayFormat="{0}" />
                        </TotalSummary>

                        <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
                        <SettingsPager PageSize="15">
                        </SettingsPager>
                        <Settings
                            VerticalScrollBarMode="Hidden"
                            VerticalScrollBarStyle="Standard"
                            HorizontalScrollBarMode="Visible"
                            ShowFooter="True" />
                        <SettingsCookies CookiesID="GUKV.AssessmentCard.Output" Version="A2_1" Enabled="False" />
                        <Styles Header-Wrap="True" >
                            <Header Wrap="True"></Header>
                        </Styles>
                    </dx:ASPxGridView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Вихідна Інформація" Name="AssessmentCardOutputDocs">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">

                    <dx:ASPxGridView ID="GridViewAssessmentOutputDocs" runat="server" 
                        ClientInstanceName="GridViewAssessmentOutputDocs"
                        AutoGenerateColumns="False"
                        DataSourceID="SqlDataSourceAssessmentOutputDoc" 
                        KeyFieldName="doc_id"
                        Width="840px" >

                        <GroupSummary>
                            <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
                        </GroupSummary>

                        <Columns>
                            <dx:GridViewDataDateColumn FieldName="doc_date" VisibleIndex="0" Caption="Дата Документа"></dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="doc_num" VisibleIndex="1" Caption="Номер Документа"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rezenz_type" VisibleIndex="2" Caption="Вид Рецензії"></dx:GridViewDataTextColumn>
                        </Columns>

                        <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
                        <SettingsPager PageSize="15">
                        </SettingsPager>
                        <Settings
                            VerticalScrollBarMode="Hidden"
                            VerticalScrollBarStyle="Standard"
                            HorizontalScrollBarMode="Visible"
                            ShowFooter="True" />
                        <SettingsCookies CookiesID="GUKV.AssessmentCard.Output" Version="A2" Enabled="False" />
                        <Styles Header-Wrap="True" >
                            <Header Wrap="True"></Header>
                        </Styles>
                    </dx:ASPxGridView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Примітка" Name="AssessmentCardNote">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">

                    <asp:FormView runat="server" BorderStyle="None" ID="FormView1" DataSourceID="SqlDataSourceAssessmentProperties" EnableViewState="False">
                        <ItemTemplate>
                            <dx:ASPxMemo ID="AssessmentMemo" runat="server" ClientInstanceName="AssessmentMemo" Width="840px" Text='<%# Eval("note_text") %>' Height="200px" ></dx:ASPxMemo>
                        </ItemTemplate>
                    </asp:FormView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

    </TabPages>
</dx:ASPxPageControl>
</dx:PanelContent>
</PanelCollection>
</dx:ASPxCallbackPanel>

</td>
</tr>
</table>

</asp:Content>
