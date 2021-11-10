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
    SelectCommand="SELECT * FROM expert_input_doc WHERE expert_note_id = @vid"
	DeleteCommand="DELETE FROM expert_input_doc where id = @id"
	UpdateCommand="UPDATE expert_input_doc SET
		[doc_date] = @doc_date,
		[doc_num] = @doc_num,
		[korrespondent_id] = @korrespondent_id,
		[control_date] = @control_date,
		[rezenz_name] = @rezenz_name, 
		[rezenz_id] = @rezenz_id,
		[modify_date] = @modify_date,
		[modified_by] = @modified_by
		WHERE id = @id" 	
	InsertCommand="INSERT INTO expert_input_doc
		(expert_note_id
		,doc_date
		,doc_num
		,korrespondent_id
		,control_date
		,rezenz_name
		,rezenz_id
		,modify_date
		,modified_by)
		values(@expert_note_id
		,@doc_date
		,@doc_num
		,@korrespondent_id
		,@control_date
		,@rezenz_name
		,@rezenz_id
		,@modify_date
		,@modified_by)"
	onupdating="SqlDataSourceFreeSquare_Updating"
	oninserting="SqlDataSourceFreeSquare_Inserting" 
	>
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="vid" />
    </SelectParameters>
	<UpdateParameters>
		<asp:Parameter Name="doc_date" />
		<asp:Parameter Name="doc_num" />
		<asp:Parameter Name="korrespondent_id" />
		<asp:Parameter Name="control_date" />
		<asp:Parameter Name="rezenz_name" />
		<asp:Parameter Name="rezenz_id" />
		<asp:Parameter Name="modify_date" />
        <asp:Parameter Name="modified_by" />
	</UpdateParameters>
	<InsertParameters>
		<asp:Parameter Name="expert_note_id" />
		<asp:Parameter Name="doc_date" />
		<asp:Parameter Name="doc_num" />
		<asp:Parameter Name="korrespondent_id" />
		<asp:Parameter Name="control_date" />
		<asp:Parameter Name="rezenz_name" />
		<asp:Parameter Name="rezenz_id" />
		<asp:Parameter Name="modify_date" />
        <asp:Parameter Name="modified_by" />
	</InsertParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentOutputDoc" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT * FROM expert_output_doc WHERE expert_note_id = @vid" 
	DeleteCommand="DELETE FROM expert_output_doc where id = @id"
	UpdateCommand="UPDATE expert_output_doc SET
		[doc_date] = @doc_date,
		[doc_num] = @doc_num,
		[rezenz_type_id] = @rezenz_type_id,
	    [rezenz_date] = @rezenz_date,
		[modify_date] = @modify_date,
		[modified_by] = @modified_by
		WHERE id = @id" 	
	InsertCommand="INSERT INTO expert_output_doc
		(expert_note_id
		,doc_date
		,doc_num
		,rezenz_type_id
	    ,rezenz_date
		,modify_date
		,modified_by)
		values(@expert_note_id
		,@doc_date
		,@doc_num
		,@rezenz_type_id
	    ,@rezenz_date
		,@modify_date
		,@modified_by)"
	onupdating="SqlDataSourceAssessmentOutputDoc_Updating"
	oninserting="SqlDataSourceAssessmentOutputDoc_Inserting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="vid" />
    </SelectParameters>
	<UpdateParameters>
		<asp:Parameter Name="doc_date" />
		<asp:Parameter Name="doc_num" />
		<asp:Parameter Name="rezenz_type_id" />
		<asp:Parameter Name="rezenz_date" />
		<asp:Parameter Name="modify_date" />
        <asp:Parameter Name="modified_by" />
	</UpdateParameters>
	<InsertParameters>
		<asp:Parameter Name="expert_note_id" />
		<asp:Parameter Name="doc_date" />
		<asp:Parameter Name="doc_num" />
		<asp:Parameter Name="rezenz_type_id" />
		<asp:Parameter Name="rezenz_date" />
		<asp:Parameter Name="modify_date" />
        <asp:Parameter Name="modified_by" />
	</InsertParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentDetails" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT * FROM expert_note_detail d WHERE d.expert_note_id = @vid"
	DeleteCommand="DELETE FROM expert_note_detail where id = @id"
	UpdateCommand="UPDATE expert_note_detail SET
		[floors] = @floors,
		[obj_square] = @obj_square,
		[purpose] = @purpose,
		[cost_1_usd] = @cost_1_usd,
		[valuation_date] = @valuation_date,
		[note] = @note,
		[modify_date] = @modify_date,
		[modified_by] = @modified_by
		WHERE id = @id" 	
	InsertCommand="INSERT INTO expert_note_detail
		(expert_note_id
		,floors
		,obj_square
		,purpose
		,cost_1_usd
		,valuation_date
		,note
		,modify_date
		,modified_by)
		values(@expert_note_id
		,@floors
		,@obj_square
		,@purpose
		,@cost_1_usd
		,@valuation_date
		,@note
		,@modify_date
		,@modified_by)"
	onupdating="SqlDataSourceAssessmentDetails_Updating"
	oninserting="SqlDataSourceAssessmentDetails_Inserting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="vid2" />
    </SelectParameters>
	<UpdateParameters>
		<asp:Parameter Name="floors" />
		<asp:Parameter Name="obj_square" />
		<asp:Parameter Name="purpose" />
		<asp:Parameter Name="cost_1_usd" />
		<asp:Parameter Name="valuation_date" />
		<asp:Parameter Name="note" />
		<asp:Parameter Name="modify_date" />
        <asp:Parameter Name="modified_by" />
	</UpdateParameters>
	<InsertParameters>
		<asp:Parameter Name="expert_note_id" />
		<asp:Parameter Name="floors" />
		<asp:Parameter Name="obj_square" />
		<asp:Parameter Name="purpose" />
		<asp:Parameter Name="cost_1_usd" />
		<asp:Parameter Name="valuation_date" />
		<asp:Parameter Name="note" />
		<asp:Parameter Name="modify_date" />
        <asp:Parameter Name="modified_by" />
	</InsertParameters>
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

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictExpertStan" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_expert_stan order by 1">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictExpertRezenz" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_expert_rezenz order by 2">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictExpertKorr" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_expert_korr order by 2">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictExpertPurpose" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_expert_detail_purpose order by 2">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictExpertFloors" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_expert_detail_floors order by 2">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictExpertRezenzType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_expert_rezenz_type order by 2">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictExpert" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, full_name as name from dict_expert where full_name <> '' order by 2">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDistrict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_districts2 WHERE id < 900 ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictBuildings" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, LTRIM(RTRIM(addr_nomer)) AS 'nomer' from buildings where 
        (is_deleted IS NULL OR is_deleted = 0) AND
        (master_building_id IS NULL) AND
        addr_street_id = @street_id AND
        (RTRIM(LTRIM(addr_nomer)) <> '') ORDER BY RTRIM(LTRIM(addr_nomer))"
    OnSelecting="SqlDataSourceDictBuildings_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="street_id" />
    </SelectParameters>
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
								<dx:ASPxComboBox ID="addr_street_id" ClientInstanceName="addr_street_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px"
                                IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceStreets" Value='<%# Eval("addr_street_id") %>'
                                Title="Назва Вулиці"
								DropDownStyle="DropDown"
								EnableCallbackMode="True"
								CallbackPageSize="100" >
									<ClientSideEvents SelectedIndexChanged="function(s, e) { ComboBuilding.PerformCallback(addr_street_id.GetValue().toString()); }" />
								</dx:ASPxComboBox>
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Номер Будинку"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
								<dx:ASPxComboBox runat="server" ID="ComboBuilding" ClientInstanceName="ComboBuilding"
									Value='<%# Bind("addr_nomer") %>'
                                    DataSourceID="SqlDataSourceDictBuildings" DropDownStyle="DropDownList" TextField="nomer"
                                    ValueField="nomer" Width="290px" IncrementalFilteringMode="StartsWith"
                                    EnableSynchronization="False" OnCallback="ComboBuilding_Callback">
                               </dx:ASPxComboBox>
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
								<dx:ASPxComboBox ID="expert_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="700px"
									IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceDictExpert" Value='<%# Eval("expert_id") %>'
									Title="СОД"
									DropDownStyle="DropDown" />
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <%--<tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Рецензент"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5">
								<dx:ASPxComboBox ID="rezenz_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="700px"
									IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceDictExpertRezenz" Value='<%# Eval("rezenz_id") %>'
									Title="Рецензент"
									DropDownStyle="DropDown" />
                            </td>
                        </tr>--%>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Вид рецензії"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
								<dx:ASPxComboBox ID="valuation_kind_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px"
									IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceDictExpertValuationKind" Value='<%# Eval("valuation_kind_id") %>'
									Title="Вид рецензії"
									DropDownStyle="DropDown" />
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Стан рецензії"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
								<dx:ASPxComboBox ID="stan_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px"
									IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceDictExpertStan" Value='<%# Eval("stan_id") %>'
									Title="Стан рецензії"
									DropDownStyle="DropDown" />
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
						<tr>
							<td width="100px">
								<dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Площа Об'єкту"></dx:ASPxLabel>
							</td>
							<td width="8px">&nbsp;</td>
							<td>
								<dx:ASPxSpinEdit ID="obj_square" runat="server" NumberType="Float" Value='<%# Eval("obj_square") %>' Width="290px"
									Title="Площа Об'єкту" SpinButtons-ShowIncrementButtons="false" />
							</td>
							<td width="8px">&nbsp;</td>
							<%--<td width="100px">
								<dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Вартість Об'єкту без ПДВ (грн.)"></dx:ASPxLabel>
							</td>
							<td width="8px">&nbsp;</td>
							<td>
								<dx:ASPxSpinEdit ID="cost_prim" runat="server" NumberType="Float" Value='<%# Eval("cost_prim") %>' Width="290px"
									Title="Вартість Об'єкту (грн.)" SpinButtons-ShowIncrementButtons="false" />
							</td>--%>
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
                                <dx:ASPxCheckBox ID="is_archived" runat="server" Checked='<%# (1.Equals(Eval("is_archived"))) ? true : false %>' Text="Переміщено в Архів" />
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
						<tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Вартість Об'єкту без ПДВ (грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
								<dx:ASPxSpinEdit ID="cost_prim" runat="server" NumberType="Float" Value='<%# Eval("cost_prim") %>' Width="290px"
									Title="Вартість Об'єкту (грн.)" SpinButtons-ShowIncrementButtons="false" />
                            </td>
                            <td width="8px">&nbsp;</td>
                            <%--<td width="100px"><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Дата Затвердження"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
								<dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" Value='<%# Eval("final_date") %>' Width="290px" />
                            </td>--%>
                        </tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Архівний Номер"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
								<dx:ASPxTextBox ID="arch_num" runat="server" Text='<%# Eval("arch_num") %>' Width="290px" />

                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Дата Затвердження"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
								<dx:ASPxDateEdit ID="final_date" runat="server" Value='<%# Eval("final_date") %>' Width="290px" />
                            </td>
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
						KeyFieldName="id"
						Width="1240px">

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
						</SettingsCommandButton>

						<GroupSummary>
							<dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
						</GroupSummary>

						<Columns>
							<dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowInCustomizationForm="True" CellStyle-Wrap="True" Width="80px" CellStyle-CssClass="command-column-class"
								ShowDeleteButton="True" ShowCancelButton="true" ShowUpdateButton="true" ShowClearFilterButton="true" ShowEditButton="true" ShowNewButton="true">
								<CellStyle Wrap="False"></CellStyle>
							</dx:GridViewCommandColumn>

							<dx:GridViewDataDateColumn FieldName="doc_date" Caption="Дата Документа" Width="100px"></dx:GridViewDataDateColumn>
							<dx:GridViewDataTextColumn FieldName="doc_num" Caption="Номер Документа" Width="120px"></dx:GridViewDataTextColumn>

							<dx:GridViewDataComboBoxColumn Caption="Кореспондент" Width="250px" FieldName="korrespondent_id"  >
								<PropertiesComboBox DataSourceID="SqlDataSourceDictExpertKorr" ValueField="id" TextField="name" ValueType="System.Int32" />
							</dx:GridViewDataComboBoxColumn>

							<dx:GridViewDataDateColumn FieldName="control_date" Caption="Контрольна Дата" Width="100px"></dx:GridViewDataDateColumn>

							<%--<dx:GridViewDataTextColumn FieldName="rezenz_name" Caption="Рецензент" Width="250px"></dx:GridViewDataTextColumn>--%>
							<dx:GridViewDataComboBoxColumn Caption="Рецензент" Width="250px" FieldName="rezenz_id"  >
								<PropertiesComboBox DataSourceID="SqlDataSourceDictExpertRezenz" ValueField="id" TextField="name" ValueType="System.Int32" />
							</dx:GridViewDataComboBoxColumn>


						</Columns>

						<SettingsBehavior ConfirmDelete="True" />
						<SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
						<SettingsPager PageSize="15">
						</SettingsPager>
						<Settings
							VerticalScrollBarMode="Hidden"
							VerticalScrollBarStyle="Standard"
							HorizontalScrollBarMode="Visible"
							ShowFooter="True" />
						<SettingsCookies CookiesID="GUKV.AssessmentCard.Input" Version="A2" Enabled="False" />
						<Styles Header-Wrap="True">
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
                        KeyFieldName="id"
                        Width="1240px" >

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
						</SettingsCommandButton>

                        <GroupSummary>
                            <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
                        </GroupSummary>

                        <Columns>
							<dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowInCustomizationForm="True" CellStyle-Wrap="True" Width="80px" CellStyle-CssClass="command-column-class"
								ShowDeleteButton="True" ShowCancelButton="true" ShowUpdateButton="true" ShowClearFilterButton="true" ShowEditButton="true" ShowNewButton="true">
								<CellStyle Wrap="False"></CellStyle>
							</dx:GridViewCommandColumn>
                            
							<%--<dx:GridViewDataTextColumn FieldName="floors" Caption="Поверх" Width="80px" ></dx:GridViewDataTextColumn>--%>

							<dx:GridViewDataComboBoxColumn Caption="Поверх" Width="80px" FieldName="floors" PropertiesComboBox-DropDownStyle="DropDown" >
								<PropertiesComboBox DataSourceID="SqlDataSourceDictExpertFloors" ValueField="name" TextField="name" ValueType="System.String" />
							</dx:GridViewDataComboBoxColumn>

                            <dx:GridViewDataSpinEditColumn FieldName="obj_square" Caption="Площа" Width="80px" PropertiesSpinEdit-SpinButtons-ShowIncrementButtons="false"></dx:GridViewDataSpinEditColumn>
                            
							<dx:GridViewDataComboBoxColumn Caption="НЕВ" Width="200px" FieldName="purpose" PropertiesComboBox-DropDownStyle="DropDown" >
								<PropertiesComboBox DataSourceID="SqlDataSourceDictExpertPurpose" ValueField="name" TextField="name" ValueType="System.String" />
							</dx:GridViewDataComboBoxColumn>


                            <dx:GridViewDataSpinEditColumn FieldName="cost_1_usd" Caption="Вартість 1 кв.м. без ПДВ, $" Width="80px" PropertiesSpinEdit-SpinButtons-ShowIncrementButtons="false"></dx:GridViewDataSpinEditColumn>
                            <dx:GridViewDataDateColumn FieldName="valuation_date" Caption="Дата Оцінки" Width="80px"></dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="note" Caption="Примітка" Width="130px"></dx:GridViewDataTextColumn>
                        </Columns>

                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="obj_square" SummaryType="Sum" DisplayFormat="{0}" />
                            <%--<dx:ASPxSummaryItem FieldName="cost_1_usd" SummaryType="Sum" DisplayFormat="{0}" />--%>
                        </TotalSummary>

						<SettingsBehavior ConfirmDelete="True" />
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
                        KeyFieldName="id"
                        Width="1240px" >

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
						</SettingsCommandButton>

                        <GroupSummary>
                            <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
                        </GroupSummary>

                        <Columns>
							<dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowInCustomizationForm="True" CellStyle-Wrap="True" Width="80px" CellStyle-CssClass="command-column-class"
								ShowDeleteButton="True" ShowCancelButton="true" ShowUpdateButton="true" ShowClearFilterButton="true" ShowEditButton="true" ShowNewButton="true">
								<CellStyle Wrap="False"></CellStyle>
							</dx:GridViewCommandColumn>
                            <dx:GridViewDataDateColumn FieldName="doc_date" Caption="Дата Документа" Width="80px"></dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="doc_num" Caption="Номер Документа" Width="160px"></dx:GridViewDataTextColumn>
							<dx:GridViewDataComboBoxColumn Caption="Категорія рецензії" Width="280px" FieldName="rezenz_type_id"  >
								<PropertiesComboBox DataSourceID="SqlDataSourceDictExpertRezenzType" ValueField="id" TextField="name" ValueType="System.Int32" />
							</dx:GridViewDataComboBoxColumn>
							<dx:GridViewDataDateColumn FieldName="rezenz_date" Caption="Дата рецензування" Width="80px"></dx:GridViewDataDateColumn>

                        </Columns>

						<SettingsBehavior ConfirmDelete="True" />
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

						<p class="SpacingPara"/>
					<dx:ASPxButton ID="ButtonSave" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
						<ClientSideEvents Click="function (s,e) 
						{ 
							CPMainPanel.PerformCallback('save:'); 
						}" />
					</dx:ASPxButton>
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
