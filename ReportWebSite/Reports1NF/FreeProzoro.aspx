<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreeProzoro.aspx.cs" Inherits="Reports1NF_FreeProzoro"
    MasterPageFile="~/NoHeader.master" Title="Картка вільного приміщення для Prozorro" %>

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
    </style>

    <script type="text/javascript" language="javascript">

		// <![CDATA[

		var lastFocusedControlId = "";
		var lastFocusedControlTitle = "";

		var isSaveError = false;

		// ]]>

		function copyAll() {
			var clipobj = { FreeProzoroClipboard: true };
			elements = getAllValueElements();
			//console.log("a", window["v46"].GetValue());
			//console.log("elements", elements);
			elements.forEach(element => {
				clipobj[element.globalName] = element.GetValue();
			});
			var json = JSON.stringify(clipobj);
			//console.log("json", json);

			//$("#inpit-for-copy-clipboard").val(json);
			//$("#inpit-for-copy-clipboard").select();
			//document.execCommand("copy");

			CallbackCopyPaste.PerformCallback(json);
		}

		function pasteAll(s) {
			//CallbackCopyPaste.PerformCallback("paste");
			//$("#inpit-for-copy-clipboard").select();
			//document.execCommand("paste");
			//let json = $("#inpit-for-copy-clipboard").val();
			//console.log("json", json);

			var clipobj = JSON.parse(s.cp_clipborddata);
			if (clipobj.FreeProzoroClipboard) {
				elements = getAllValueElements();
				elements.forEach(element => {
					let val = clipobj[element.globalName];
					if (element.isASPxClientDateEdit) {
						let dat = null;
						if (val != null) {
							dat = new Date(val);
						}
						//console.log(element.globalName);
						//console.log(val);
						//console.log(dat);
						element.SetValue(dat);
					} else {
						element.SetValue(val);
					}
				});
			}
		}

		

		function getAllValueElements() {
			let elements = [];
			for (let i = 0; i <= 400; i++) {
				var element = window["v" + i];
				if (element != null) {
					elements.push(element);
				}
			}
			return elements;
		}

	</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceMain" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT a.* FROM reports1nf_balans_free_prozoro a WHERE a.id = @id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<%--<textarea rows="2" cols="2" id="inpit-for-copy-clipboard" style="display:none2;width:1px;height:1px;position:absolute;top:1px;right:1px;z-index:-1" ></textarea>--%>
<%--<textarea rows="2" cols="2" id="inpit-for-copy-clipboard" style="width:500px;height:30px;position:absolute;top:1px;right:1px;z-index:100" ></textarea>--%>

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
    </Items>
</dx:ASPxMenu>

<table style="width:1210px">
	<tr>
		<td>
			<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
				<asp:Label runat="server" ID="ASPxLabel19" Text="Картка вільного приміщення для Prozorro" CssClass="pagetitle"/>
			</p>
		</td>
		<td align="center">
			<dx:ASPxButton ID="ASPxButton2" runat="server" Text="Копіювати" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { copyAll() }" />
            </dx:ASPxButton>
			<dx:ASPxButton ID="ASPxButton3" runat="server" Text="Вставити" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CallbackCopyPaste.PerformCallback('paste') }" />
            </dx:ASPxButton>
		</td>
		<td align="right">
			<dx:ASPxButton ID="ButtonSave" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('save:'); }" />
            </dx:ASPxButton>
		</td>
	</tr>
</table>

<dx:ASPxCallbackPanel ID="CallbackCopyPaste" ClientInstanceName="CallbackCopyPaste" runat="server" OnCallback="CallbackCopyPaste_Callback" 
	ClientSideEvents-EndCallback="function (s,e) { pasteAll(s) }"/>

<dx:ASPxCallbackPanel ID="CPMainPanel" ClientInstanceName="CPMainPanel" runat="server" OnCallback="CPMainPanel_Callback" 
	ClientSideEvents-BeginCallback="function (s,e) { isSaveError = false }"
	ClientSideEvents-CallbackError="function (s,e) { isSaveError = true }"
	ClientSideEvents-EndCallback="function (s,e) { if (!isSaveError) { document.location.reload(); window.close() } }">	
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent2" runat="server">


<asp:FormView runat="server" BorderStyle="None" ID="AddressForm" DataSourceID="SqlDataSourceMain" EnableViewState="False">
    <ItemTemplate>

        <dx:ASPxRoundPanel ID="PanelAddress" runat="server" HeaderText="1. Ідентифікація орендодавця">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server">
                    <table border="0" cellspacing="0" cellpadding="2" width="1210px">

                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Код ЄДРПОУ Орендодавця"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v46" ClientInstanceName="v46" runat="server" Text='<%# Eval("v46") %>' Width="350px" Title="Код ЄДРПОУ Орендодавця" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Повна юридична назва орендодавця"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v47" ClientInstanceName="v47" runat="server" Text='<%# Eval("v47") %>' Width="350px" Title="Повна юридична назва орендодавця" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Область орендодавця"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v48" ClientInstanceName="v48" runat="server" Text='<%# Eval("v48") %>' Width="350px" Title="Область орендодавця" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Місто орендодавця"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v49" ClientInstanceName="v49" runat="server" Text='<%# Eval("v49") %>' Width="350px" Title="Місто орендодавця" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Будинок та вулиця орендодавця"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v50" ClientInstanceName="v50" runat="server" Text='<%# Eval("v50") %>' Width="350px" Title="Будинок та вулиця орендодавця" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Індекс орендодавця"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v51" ClientInstanceName="v51" runat="server" Text='<%# Eval("v51") %>' Width="350px" Title="Індекс орендодавця" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Прізвище представника орендодавця"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v52" ClientInstanceName="v52" runat="server" Text='<%# Eval("v52") %>' Width="350px" Title="Прізвище представника орендодавця" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ім'я представника орендодавця"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v53" ClientInstanceName="v53" runat="server" Text='<%# Eval("v53") %>' Width="350px" Title="Ім'я представника орендодавця" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="По батькові представника орендодавця"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v54" ClientInstanceName="v54" runat="server" Text='<%# Eval("v54") %>' Width="350px" Title="По батькові представника орендодавця" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="E-mail  контактної особи орендодавця"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v55" ClientInstanceName="v55" runat="server" Text='<%# Eval("v55") %>' Width="350px" Title="E-mail  контактної особи орендодавця" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Телефон орендодавця "></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v56" ClientInstanceName="v56" runat="server" Text='<%# Eval("v56") %>' Width="350px" Title="Телефон орендодавця " /></td>
                        <tr>

                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="2. Ідентифікація балансоутримувача">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <table border="0" cellspacing="0" cellpadding="2" width="1210px">

                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Погодження балансоутримувача"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v5" ClientInstanceName="v5" runat="server" Text='<%# Eval("v5") %>' Width="350px" Title="Погодження балансоутримувача" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити підтверджуючого документу про погодження балансоутримувача (за наявності)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v6" ClientInstanceName="v6" runat="server" Text='<%# Eval("v6") %>' Width="350px" Title="Реквізити підтверджуючого документу про погодження балансоутримувача (за наявності)" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Погодження орендодавця"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v7" ClientInstanceName="v7" runat="server" Text='<%# Eval("v7") %>' Width="350px" Title="Погодження орендодавця" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити підтверджуючого документу про погодження орендодавця (за наявності)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v8" ClientInstanceName="v8" runat="server" Text='<%# Eval("v8") %>' Width="350px" Title="Реквізити підтверджуючого документу про погодження орендодавця (за наявності)" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інформація про згоду на здійснення поточного та/або капітального ремонту орендованого майна"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v9" ClientInstanceName="v9" runat="server" Text='<%# Eval("v9") %>' Width="350px" Title="Інформація про згоду на здійснення поточного та/або капітального ремонту орендованого майна" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити підтверджуючого документу про згоду на здійснення ремонту (за наявності)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v10" ClientInstanceName="v10" runat="server" Text='<%# Eval("v10") %>' Width="350px" Title="Реквізити підтверджуючого документу про згоду на здійснення ремонту (за наявності)" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Погодження органу управління "></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v15" ClientInstanceName="v15" runat="server" Text='<%# Eval("v15") %>' Width="350px" Title="Погодження органу управління " /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити підтверджуючого документу про погодження органу управління (за наявності)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v16" ClientInstanceName="v16" runat="server" Text='<%# Eval("v16") %>' Width="350px" Title="Реквізити підтверджуючого документу про погодження органу управління (за наявності)" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Погодження органу управління ( органу охорони культ. спадищин)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v17" ClientInstanceName="v17" runat="server" Text='<%# Eval("v17") %>' Width="350px" Title="Погодження органу управління ( органу охорони культ. спадищин)" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити підтверджуючого документу про погодження органу охорони культ. спадищин (за наявності)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v18" ClientInstanceName="v18" runat="server" Text='<%# Eval("v18") %>' Width="350px" Title="Реквізити підтверджуючого документу про погодження органу охорони культ. спадищин (за наявності)" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Код ЄДРПОУ Балансоутримувача"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v57" ClientInstanceName="v57" runat="server" Text='<%# Eval("v57") %>' Width="350px" Title="Код ЄДРПОУ Балансоутримувача" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Повна юридична назва Балансоутримувача"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v58" ClientInstanceName="v58" runat="server" Text='<%# Eval("v58") %>' Width="350px" Title="Повна юридична назва Балансоутримувача" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Область Балансоутримувача"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v59" ClientInstanceName="v59" runat="server" Text='<%# Eval("v59") %>' Width="350px" Title="Область Балансоутримувача" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Місто Балансоутримувача"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v60" ClientInstanceName="v60" runat="server" Text='<%# Eval("v60") %>' Width="350px" Title="Місто Балансоутримувача" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Будинок та вулиця Балансоутримувача"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v61" ClientInstanceName="v61" runat="server" Text='<%# Eval("v61") %>' Width="350px" Title="Будинок та вулиця Балансоутримувача" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Індекс Балансоутримувача"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v62" ClientInstanceName="v62" runat="server" Text='<%# Eval("v62") %>' Width="350px" Title="Індекс Балансоутримувача" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Прізвище представника Балансоутримувача"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v63" ClientInstanceName="v63" runat="server" Text='<%# Eval("v63") %>' Width="350px" Title="Прізвище представника Балансоутримувача" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ім'я представника Балансоутримувача"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v64" ClientInstanceName="v64" runat="server" Text='<%# Eval("v64") %>' Width="350px" Title="Ім'я представника Балансоутримувача" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="По батькові представника Балансоутримувача"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v65" ClientInstanceName="v65" runat="server" Text='<%# Eval("v65") %>' Width="350px" Title="По батькові представника Балансоутримувача" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="E-mail  контактної особи Балансоутримувача"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v66" ClientInstanceName="v66" runat="server" Text='<%# Eval("v66") %>' Width="350px" Title="E-mail  контактної особи Балансоутримувача" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Телефон Балансоутримувача"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v67" ClientInstanceName="v67" runat="server" Text='<%# Eval("v67") %>' Width="350px" Title="Телефон Балансоутримувача" /></td>
                        </tr>


                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="3. Ідентифікація органу управління">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent3" runat="server">
                    <table border="0" cellspacing="0" cellpadding="2" width="1210px">

                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Код ЄДРПОУ Органу управління"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v68" ClientInstanceName="v68" runat="server" Text='<%# Eval("v68") %>' Width="350px" Title="Код ЄДРПОУ Органу управління" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Повна юридична назва Органу управління"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v69" ClientInstanceName="v69" runat="server" Text='<%# Eval("v69") %>' Width="350px" Title="Повна юридична назва Органу управління" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Область Органу управління"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v70" ClientInstanceName="v70" runat="server" Text='<%# Eval("v70") %>' Width="350px" Title="Область Органу управління" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Місто Органу управління"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v71" ClientInstanceName="v71" runat="server" Text='<%# Eval("v71") %>' Width="350px" Title="Місто Органу управління" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Будинок та вулиця Органу управління"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v72" ClientInstanceName="v72" runat="server" Text='<%# Eval("v72") %>' Width="350px" Title="Будинок та вулиця Органу управління" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Індекс Органу управління"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v73" ClientInstanceName="v73" runat="server" Text='<%# Eval("v73") %>' Width="350px" Title="Індекс Органу управління" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Прізвище представника Органу управління"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v74" ClientInstanceName="v74" runat="server" Text='<%# Eval("v74") %>' Width="350px" Title="Прізвище представника Органу управління" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ім'я представника Органу управління"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v75" ClientInstanceName="v75" runat="server" Text='<%# Eval("v75") %>' Width="350px" Title="Ім'я представника Органу управління" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="По батькові представника Органу управління"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v76" ClientInstanceName="v76" runat="server" Text='<%# Eval("v76") %>' Width="350px" Title="По батькові представника Органу управління" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="E-mail  контактної особи Органу управління"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v77" ClientInstanceName="v77" runat="server" Text='<%# Eval("v77") %>' Width="350px" Title="E-mail  контактної особи Органу управління" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Телефон Органу управління"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v78" ClientInstanceName="v78" runat="server" Text='<%# Eval("v78") %>' Width="350px" Title="Телефон Органу управління" /></td>
                        </tr> 


                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="4. Ідентифікація чинного орендаря">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent5" runat="server">
                    <table border="0" cellspacing="0" cellpadding="2" width="1210px">

                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Код ЄДРПОУ/РНОКПП Чинного орендаря"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v79" ClientInstanceName="v79" runat="server" Text='<%# Eval("v79") %>' Width="350px" Title="Код ЄДРПОУ/РНОКПП Чинного орендаря" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Повна юридична назва Чинного орендаря"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v80" ClientInstanceName="v80" runat="server" Text='<%# Eval("v80") %>' Width="350px" Title="Повна юридична назва Чинного орендаря" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Область Чинного орендаря"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v81" ClientInstanceName="v81" runat="server" Text='<%# Eval("v81") %>' Width="350px" Title="Область Чинного орендаря" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Місто Чинного орендаря"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v82" ClientInstanceName="v82" runat="server" Text='<%# Eval("v82") %>' Width="350px" Title="Місто Чинного орендаря" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Будинок та вулиця Чинного орендаря"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v83" ClientInstanceName="v83" runat="server" Text='<%# Eval("v83") %>' Width="350px" Title="Будинок та вулиця Чинного орендаря" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Індекс Чинного орендаря"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v84" ClientInstanceName="v84" runat="server" Text='<%# Eval("v84") %>' Width="350px" Title="Індекс Чинного орендаря" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Прізвище представника Чинного орендаря"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v85" ClientInstanceName="v85" runat="server" Text='<%# Eval("v85") %>' Width="350px" Title="Прізвище представника Чинного орендаря" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ім'я представника Чинного орендаря"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v86" ClientInstanceName="v86" runat="server" Text='<%# Eval("v86") %>' Width="350px" Title="Ім'я представника Чинного орендаря" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="По батькові представника Чинного орендаря"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v87" ClientInstanceName="v87" runat="server" Text='<%# Eval("v87") %>' Width="350px" Title="По батькові представника Чинного орендаря" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="E-mail  контактної особи Чинного орендаря"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v88" ClientInstanceName="v88" runat="server" Text='<%# Eval("v88") %>' Width="350px" Title="E-mail  контактної особи Чинного орендаря" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Телефон Чинного орендаря"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v89" ClientInstanceName="v89" runat="server" Text='<%# Eval("v89") %>' Width="350px" Title="Телефон Чинного орендаря" /></td>
                        </tr>


                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" HeaderText="5. Опис обєкту">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server">
                    <table border="0" cellspacing="0" cellpadding="2" width="1210px">

                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Назва об`єкту реєстра"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v1" ClientInstanceName="v1" runat="server" Text='<%# Eval("v1") %>' Width="350px" Title="Назва об`єкту реєстра" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Тип власності"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v2" ClientInstanceName="v2" runat="server" Text='<%# Eval("v2") %>' Width="350px" Title="Тип власності" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Тип переліку до якого віднесено об'єкт"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v3" ClientInstanceName="v3" runat="server" Text='<%# Eval("v3") %>' Width="350px" Title="Тип переліку до якого віднесено об'єкт" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Статус об'єкту в Переліку"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v4" ClientInstanceName="v4" runat="server" Text='<%# Eval("v4") %>' Width="350px" Title="Статус об'єкту в Переліку" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Рішення про передачу об'єкта на приватизацію"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v11" ClientInstanceName="v11" runat="server" Text='<%# Eval("v11") %>' Width="350px" Title="Рішення про передачу об'єкта на приватизацію" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити підтверджуючого документу про передачу об'єкта на приватизацію (за наявності)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v12" ClientInstanceName="v12" runat="server" Text='<%# Eval("v12") %>' Width="350px" Title="Реквізити підтверджуючого документу про передачу об'єкта на приватизацію (за наявності)" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Рішення про проведення інвестиційного конкурсу"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v13" ClientInstanceName="v13" runat="server" Text='<%# Eval("v13") %>' Width="350px" Title="Рішення про проведення інвестиційного конкурсу" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити підтверджуючого документу про проведення інвестиційного конкурсу (за наявності)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v14" ClientInstanceName="v14" runat="server" Text='<%# Eval("v14") %>' Width="350px" Title="Реквізити підтверджуючого документу про проведення інвестиційного конкурсу (за наявності)" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Стан державної реєстрації об'єкту"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v19" ClientInstanceName="v19" runat="server" Text='<%# Eval("v19") %>' Width="350px" Title="Стан державної реєстрації об'єкту" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити державної реєстрації об'єкту"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v20" ClientInstanceName="v20" runat="server" Text='<%# Eval("v20") %>' Width="350px" Title="Реквізити державної реєстрації об'єкту" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Первісна балансова вартість, грн"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v34" ClientInstanceName="v34" runat="server" Text='<%# Eval("v34") %>' Width="350px" Title="Первісна балансова вартість, грн" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Залишкова балансова вартість, грн"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v35" ClientInstanceName="v35" runat="server" Text='<%# Eval("v35") %>' Width="350px" Title="Залишкова балансова вартість, грн" NumberType="Float" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ринкова вартість, грн"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v36" ClientInstanceName="v36" runat="server" Text='<%# Eval("v36") %>' Width="350px" Title="Ринкова вартість, грн" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Дата визначення ринкової вартості (01/01/2021)"></dx:ASPxLabel></td>
                            <td><dx:ASPxDateEdit ID="v37" ClientInstanceName="v37" runat="server" Value='<%# Eval("v37") %>' Width="350px" Title="Дата визначення ринкової вартості (01/01/2021)" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Рішення про затвердження переліку об’єктів, або про включення нового об’єкта до переліку"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v45" ClientInstanceName="v45" runat="server" Text='<%# Eval("v45") %>' Width="350px" Title="Рішення про затвердження переліку об’єктів, або про включення нового об’єкта до переліку" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Назва об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v90" ClientInstanceName="v90" runat="server" Text='<%# Eval("v90") %>' Width="350px" Title="Назва об'єкта" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Опис об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v91" ClientInstanceName="v91" runat="server" Text='<%# Eval("v91") %>' Width="350px" Title="Опис об'єкта" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Основний класифікатор об'єкта:"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v92" ClientInstanceName="v92" runat="server" Text='<%# Eval("v92") %>' Width="350px" Title="Основний класифікатор об'єкта:" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Область Розташування об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v93" ClientInstanceName="v93" runat="server" Text='<%# Eval("v93") %>' Width="350px" Title="Область Розташування об'єкта" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Місто розташування об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v94" ClientInstanceName="v94" runat="server" Text='<%# Eval("v94") %>' Width="350px" Title="Місто розташування об'єкта" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Вулиця та будинок розташування об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v95" ClientInstanceName="v95" runat="server" Text='<%# Eval("v95") %>' Width="350px" Title="Вулиця та будинок розташування об'єкта" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Індекс розташування об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v96" ClientInstanceName="v96" runat="server" Text='<%# Eval("v96") %>' Width="350px" Title="Індекс розташування об'єкта" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Рік будівництва"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v98" ClientInstanceName="v98" runat="server" Text='<%# Eval("v98") %>' Width="350px" Title="Рік будівництва" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Місце розташування об’єкта в будівлі"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v99" ClientInstanceName="v99" runat="server" Text='<%# Eval("v99") %>' Width="350px" Title="Місце розташування об’єкта в будівлі" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Номер поверху або поверхів"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v100" ClientInstanceName="v100" runat="server" Text='<%# Eval("v100") %>' Width="350px" Title="Номер поверху або поверхів" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Загальна площа об'єкту оренди, кв.м."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v102" ClientInstanceName="v102" runat="server" Text='<%# Eval("v102") %>' Width="350px" Title="Загальна площа об'єкту оренди, кв.м." NumberType="Float" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Корисна площа об'єкту оренди, кв.м."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v103" ClientInstanceName="v103" runat="server" Text='<%# Eval("v103") %>' Width="350px" Title="Корисна площа об'єкту оренди, кв.м." NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Технічний стан об'єкта оренди"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v107" ClientInstanceName="v107" runat="server" Text='<%# Eval("v107") %>' Width="350px" Title="Технічний стан об'єкта оренди" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Чи приєднаний об'єкт оренди до електромережі"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v108" ClientInstanceName="v108" runat="server" Text='<%# Eval("v108") %>' Width="350px" Title="Чи приєднаний об'єкт оренди до електромережі" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Водозабезпечення присутнє"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v109" ClientInstanceName="v109" runat="server" Text='<%# Eval("v109") %>' Width="350px" Title="Водозабезпечення присутнє" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Централізоване опалення присутнє"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v110" ClientInstanceName="v110" runat="server" Text='<%# Eval("v110") %>' Width="350px" Title="Централізоване опалення присутнє" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інформація про наявність окремих особових рахунків на об'єкт оренди, відкритих постачальниками комунальних послуг, або інформація про порядок участі орендаря у компенсації балансоутримувачу витрат на оплату комунальних послуг"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v126" ClientInstanceName="v126" runat="server" Text='<%# Eval("v126") %>' Width="350px" Title="Інформація про наявність окремих особових рахунків на об'єкт оренди, відкритих постачальниками комунальних послуг, або інформація про порядок участі орендаря у компенсації балансоутримувачу витрат на оплату комунальних послуг" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інформація щодо компенсації балансоутримувачу сплати земельного податку за користування земельною ділянкою, на якій розташований об'єкт оренди"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v127" ClientInstanceName="v127" runat="server" Text='<%# Eval("v127") %>' Width="350px" Title="Інформація щодо компенсації балансоутримувачу сплати земельного податку за користування земельною ділянкою, на якій розташований об'єкт оренди" /></td>
                        </tr>


                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Технологія будівництва"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v97" ClientInstanceName="v97" runat="server" Text='<%# Eval("v97") %>' Width="350px" Title="Технологія будівництва" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Загальна площа будівлі, до складу якої входить об'єкт оренди, кв.м."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v101" ClientInstanceName="v101" runat="server" Text='<%# Eval("v101") %>' Width="350px" Title="Загальна площа будівлі, до складу якої входить об'єкт оренди, кв.м." NumberType="Float" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Житлова площа, кв. м."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v104" ClientInstanceName="v104" runat="server" Text='<%# Eval("v104") %>' Width="350px" Title="Житлова площа, кв. м." NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Площа кухні, кв. м"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v105" ClientInstanceName="v105" runat="server" Text='<%# Eval("v105") %>' Width="350px" Title="Площа кухні, кв. м" NumberType="Float" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Площа ділянки, кв. м"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v106" ClientInstanceName="v106" runat="server" Text='<%# Eval("v106") %>' Width="350px" Title="Площа ділянки, кв. м" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Вентиляція присутня"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v111" ClientInstanceName="v111" runat="server" Text='<%# Eval("v111") %>' Width="350px" Title="Вентиляція присутня" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Телебачення присутнє"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v112" ClientInstanceName="v112" runat="server" Text='<%# Eval("v112") %>' Width="350px" Title="Телебачення присутнє" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Охоронна сигналізація присутня"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v113" ClientInstanceName="v113" runat="server" Text='<%# Eval("v113") %>' Width="350px" Title="Охоронна сигналізація присутня" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Каналізація присутня"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v114" ClientInstanceName="v114" runat="server" Text='<%# Eval("v114") %>' Width="350px" Title="Каналізація присутня" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Автономне опалення присутнє"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v115" ClientInstanceName="v115" runat="server" Text='<%# Eval("v115") %>' Width="350px" Title="Автономне опалення присутнє" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Кондиціонування присутнє"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v116" ClientInstanceName="v116" runat="server" Text='<%# Eval("v116") %>' Width="350px" Title="Кондиціонування присутнє" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інтернет присутній"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v117" ClientInstanceName="v117" runat="server" Text='<%# Eval("v117") %>' Width="350px" Title="Інтернет присутній" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Пожежна сигналізація присутня"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v118" ClientInstanceName="v118" runat="server" Text='<%# Eval("v118") %>' Width="350px" Title="Пожежна сигналізація присутня" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Газифікація присутня"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v119" ClientInstanceName="v119" runat="server" Text='<%# Eval("v119") %>' Width="350px" Title="Газифікація присутня" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Лічильник опалення присутній"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v120" ClientInstanceName="v120" runat="server" Text='<%# Eval("v120") %>' Width="350px" Title="Лічильник опалення присутній" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Телефонізація присутня"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v121" ClientInstanceName="v121" runat="server" Text='<%# Eval("v121") %>' Width="350px" Title="Телефонізація присутня" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ліфт присутній"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v122" ClientInstanceName="v122" runat="server" Text='<%# Eval("v122") %>' Width="350px" Title="Ліфт присутній" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Додаткова інформація щодо комунікацій, що є в об'єкті"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v125" ClientInstanceName="v125" runat="server" Text='<%# Eval("v125") %>' Width="350px" Title="Додаткова інформація щодо комунікацій, що є в об'єкті" /></td>
                        </tr>


                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" HeaderText="6. Умови оренди">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent7" runat="server">
                    <table border="0" cellspacing="0" cellpadding="2" width="1210px">

                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Тривалість оренди років"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v21" ClientInstanceName="v21" runat="server" Text='<%# Eval("v21") %>' Width="350px" Title="Тривалість оренди років" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Тривалість оренди місяців"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v22" ClientInstanceName="v22" runat="server" Text='<%# Eval("v22") %>' Width="350px" Title="Тривалість оренди місяців" NumberType="Float" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Тривалість оренди днів"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v23" ClientInstanceName="v23" runat="server" Text='<%# Eval("v23") %>' Width="350px" Title="Тривалість оренди днів" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Спосіб обмеження цільового призначення об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v24" ClientInstanceName="v24" runat="server" Text='<%# Eval("v24") %>' Width="350px" Title="Спосіб обмеження цільового призначення об'єкта" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Графікова оренда: кількість годин"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v25" ClientInstanceName="v25" runat="server" Text='<%# Eval("v25") %>' Width="350px" Title="Графікова оренда: кількість годин" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Кількість годин передбачається на:"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v26" ClientInstanceName="v26" runat="server" Text='<%# Eval("v26") %>' Width="350px" Title="Кількість годин передбачається на:" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Графікова оренда: кількість днів"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v27" ClientInstanceName="v27" runat="server" Text='<%# Eval("v27") %>' Width="350px" Title="Графікова оренда: кількість днів" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Кількість дні передбачається на:"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v28" ClientInstanceName="v28" runat="server" Text='<%# Eval("v28") %>' Width="350px" Title="Кількість дні передбачається на:" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інший графік використання"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v29" ClientInstanceName="v29" runat="server" Text='<%# Eval("v29") %>' Width="350px" Title="Інший графік використання" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Цільове призначення об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v30" ClientInstanceName="v30" runat="server" Text='<%# Eval("v30") %>' Width="350px" Title="Цільове призначення об'єкта" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Опис обмежень цільового призначення об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v31" ClientInstanceName="v31" runat="server" Text='<%# Eval("v31") %>' Width="350px" Title="Опис обмежень цільового призначення об'єкта" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Додаткові умови оренди майна"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v32" ClientInstanceName="v32" runat="server" Text='<%# Eval("v32") %>' Width="350px" Title="Додаткові умови оренди майна" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Можливість суборенди"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v33" ClientInstanceName="v33" runat="server" Text='<%# Eval("v33") %>' Width="350px" Title="Можливість суборенди" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Посилання на пункт Методики розрахунку орендної плати, яким встановлена орендна ставка для запропонованого цільового призначення"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v41" ClientInstanceName="v41" runat="server" Text='<%# Eval("v41") %>' Width="350px" Title="Посилання на пункт Методики розрахунку орендної плати, яким встановлена орендна ставка для запропонованого цільового призначення" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Фактичне значення орендної ставки, грн"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v42" ClientInstanceName="v42" runat="server" Text='<%# Eval("v42") %>' Width="350px" Title="Фактичне значення орендної ставки, грн" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Значення орендної ставки у відсотках"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v43" ClientInstanceName="v43" runat="server" Text='<%# Eval("v43") %>' Width="350px" Title="Значення орендної ставки у відсотках" NumberType="Float" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інші відомомсті щодо визначення орендної ставки"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v44" ClientInstanceName="v44" runat="server" Text='<%# Eval("v44") %>' Width="350px" Title="Інші відомомсті щодо визначення орендної ставки" /></td>
                        </tr>


                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <p class="SpacingPara"/>

    </ItemTemplate>
</asp:FormView>





        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>


<p class="SpacingPara"/>


<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('save:'); }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
    </tr>
</table>



</asp:Content>
