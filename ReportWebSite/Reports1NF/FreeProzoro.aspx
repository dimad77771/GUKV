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

<mini:ProfiledSqlDataSource ID="SqlDataSourceYesNoNone" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select 1, 'Наявна' as name union select 2,'Відсутня' union select 3, 'Не вимагається' order by 1">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceUnknownYesNo" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select 1, 'Немає інформації' as name union select 2,'Так' union select 3, 'Ні' order by 1">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceTypeVlasn" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select 1, 'Державна' as name union select 2,'Комунальна' order by 1">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePerelikType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select 1, 'Перелік першого типу' as name union select 2,'перелік другого типу' union select 3, 'Не визначено' order by 1">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePerelikStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="
select 1, 'Очікує включення в перелік' as name union
select 2, 'Включено в перелік' union
select 3, 'Неактивний' union
select 4, 'Опубліковано оголошення' union
select 5, 'Визначено Орендаря' union
select 6, 'Орендовано' 
order by 1">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceStanReestr" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="
select 1, 'Зареєстровано в державному реєстрі речових прав на нерухомість' as name union
select 2, 'Зареєстровано до першого січня 2013 року відповідно до законодавства, що діяло на момент реєстрації майна' union
select 3, 'Не зареєстровано' union
select 4, 'Реєстрація не вимагається'
order by 1">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceMistoInDom" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="
select 1, 'Надземний' as name union
select 2, 'Цокольний' union
select 3, 'Підвальний' union
select 4, 'Технічний' union
select 5, 'Мансардний' 
order by 1">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceTechnologia" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="
select 1, 'Монолітно-каркасна' as name union
select 2, 'Панель' union
select 3, 'Утеплена панель' union
select 4, 'Цегла' union
select 5, 'Інше' 
order by 1">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataObmezhena" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="
select 1,	'Офісні приміщення, коворкінги. Об’єкти поштового зв’язку та розміщення суб’єктів господарювання, що надають послуги з перевезення та доставки (вручення) поштових відправлень. Редакції засобів масової інформації, видавництва друкованих засобів масової інформації та видавничої продукції. Ломбарди, відділення банків, інших провайдерів фінансових послу' as name union
select 2,	'Громадські об’єднання та благодійні організації' as name union
select 3,	'Бібліотеки. Театри. Кінотеатри, діяльність з кінопоказів' as name union
select 4,	'Центри раннього розвитку дитини. Діяльність у сфері освіти, курси і тренінги' as name union
select 5,	'Тренажерні зали, заклади фізичної культури і спорту, діяльність з організації та проведення занять різними видами спорту' as name union
select 6,	'Заклади охорони здоров’я, клініки, лікарні, приватна медична практика. Аптеки. Ветеринарні лікарні (клініки), лабораторії ветеринарної медицини, ветеринарні аптеки. Медичні лабораторії' as name union
select 7,	'Науково-дослідні установи, наукові парки' as name union
select 8,	'Заклади харчування, кафе, бари, ресторани, які здійснюють продаж товарів підакцизної групи. Торговельні об’єкти, які здійснюють продаж товарів підакцизної групи' as name union
select 9,	'Заклади харчування, їдальні, буфети, кафе, які не здійснюють продаж товарів підакцизної групи. Торговельні об’єкти, які не здійснюють продаж товарів підакцизної групи' as name union
select 10,	'Склади. Камери схову, архіви' as name union
select 11,	'Нічні клуби. Ресторани з нічним режимом роботи (після 22 год). Сауни, лазні. Організація концертів та іншої видовищно-розважальної діяльності. Готелі, хостели, турбази, мотелі, кемпінги, літні будиночки. Комп’ютерні клуби та Інтернет-кафе' as name union
select 12,	'Проведення виставок' as name union
select 13,	'Пункти обміну валюти, банкомати, платіжні термінали. Торговельні автомати. Розміщення технічних засобів і антен операторів телекомунікацій, суб’єктів підприємницької діяльності, які надають послуги зв’язку, послуги доступу до Інтернету, телекомунікації, передання сигналу мовлення. Розміщення зовнішньої реклами на будівлях і спорудах. Продаж книг, газет і журналів' as name union
select 14,	'Майстерні, ательє. Салони краси, перукарні. Надання інших побутових послуг населенню' as name union
select 15,	'Ритуальні послуги. Громадські вбиральні. Збір і сортування вторинної сировини' as name union
select 16,	'Стоянки автомобілів. Розміщення транспортних підприємств з перевезення пасажирів і вантажів. Станції технічного обслуговування автомобілів' as name union
select 17,	'Розміщення суб’єктів підприємницької діяльності, які здійснюють іншу виробничу діяльність' as name union
select 18,	'Інше' as name union
select 19,	'Органи державної влади та органи місцевого самоврядування, інші установи і організації, діяльність яких фінансується за рахунок державного або місцевих бюджетів' as name union
select 20,	'Пенсійний фонд України та його органи' as name union
select 21,	'Державні та комунальні підприємства, установи, організації у сфері культури і мистецтв' as name union
select 22,	'Редакції державних і комунальних періодичних видань, державні видавництва, підприємства книгорозповсюдження, вітчизняні видавництва та підприємства книгорозповсюдження, що забезпечують підготовку, випуск та (або) розповсюдження не менш як 50 відсотків книжкової продукції державною мовою (за винятком видань рекламного та еротичного характеру)' as name union
select 23,	'Громадська приймальня народного депутата України або депутата місцевої ради' as name union
select 24,	'Дипломатичні представництва та консульські установи іноземних держав, представництва міжнародних міжурядових організацій в Україні' as name union
select 25,	'Організація та проведення науково-практичних, культурних, мистецьких, громадських, суспільних та політичних заходів' as name union
select 26,	'Проведення публічних заходів (зборів, дебатів, дискусій) під час та на період виборчої кампанії' as name 
order by 1
">
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourceObmezhen" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select 1, 'Тільки зазначене' as name union select 2,'Окрім зазначеного' union select 3, 'Без обмежень' order by 1">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePriznacheno" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT left(full_name, 150) as name FROM dict_rental_rate ORDER BY 1">
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourceTypeObject" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select 1, 'Нерухоме майно' as name union select 2,'Рухоме майно' union select 3, 'ЦМК' order by 1">
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

<table style="width:1510px">
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
		<td align="center">
			<dx:ASPxButton ID="ButtonExcel" runat="server" Text="Excel" OnClick="ButtonPrint_Click">
				
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

        <dx:ASPxRoundPanel ID="PanelIdent" runat="server" ShowHeader="false">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent8" runat="server">
                    <table border="0" cellspacing="0" cellpadding="2" width="1510px">
						<colgroup>
							<col style="width:300px">
							<col style="width:25%">
							<col style="width:300px">
							<col style="width:25%">
						</colgroup>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реєстраціний номер (Реєстраційний №)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v300" ClientInstanceName="v300" runat="server" Text='<%# Eval("v300") %>' Width="350px" Title="Реєстраціний номер (Реєстраційний №)" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="PanelAddress" runat="server" HeaderText="1. Ідентифікація орендодавця">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server">
                    <table border="0" cellspacing="0" cellpadding="2" width="1510px">
						<colgroup>
							<col style="width:300px">
							<col style="width:25%">
							<col style="width:300px">
							<col style="width:25%">
						</colgroup>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ідентифікатор організації"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v46" ClientInstanceName="v46" runat="server" Text='<%# Eval("v46") %>' Width="350px" Title="Ідентифікатор організації" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Повна юридична назва організації"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v47" ClientInstanceName="v47" runat="server" Text='<%# Eval("v47") %>' Width="350px" Title="Повна юридична назва організації" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інформація щодо підтвердження повноважень"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v201" ClientInstanceName="v201" runat="server" Text='<%# Eval("v201") %>' Width="350px" Title="Інформація щодо підтвердження повноважень" /></td>

                            <td align="right"><dx:ASPxLabel runat="server" Text="Країна"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v202" ClientInstanceName="v202" runat="server" Text='<%# Eval("v202") %>' Width="350px" Title="Країна" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="область"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v48" ClientInstanceName="v48" runat="server" Text='<%# Eval("v48") %>' Width="350px" Title="область" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Населений пункт"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v49" ClientInstanceName="v49" runat="server" Text='<%# Eval("v49") %>' Width="350px" Title="Населений пункт" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Вулиця, будинок, квартира"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v50" ClientInstanceName="v50" runat="server" Text='<%# Eval("v50") %>' Width="350px" Title="Вулиця, будинок, квартира" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Індекс"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v51" ClientInstanceName="v51" runat="server" Text='<%# Eval("v51") %>' Width="350px" Title="Індекс" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Призвище"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v52" ClientInstanceName="v52" runat="server" Text='<%# Eval("v52") %>' Width="350px" Title="Призвище" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ім'я"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v53" ClientInstanceName="v53" runat="server" Text='<%# Eval("v53") %>' Width="350px" Title="Ім'я" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="По батькові"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v54" ClientInstanceName="v54" runat="server" Text='<%# Eval("v54") %>' Width="350px" Title="По батькові" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="E-mail для повідомлень"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v55" ClientInstanceName="v55" runat="server" Text='<%# Eval("v55") %>' Width="350px" Title="E-mail для повідомлень" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Телефон"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v56" ClientInstanceName="v56" runat="server" Text='<%# Eval("v56") %>' Width="350px" Title="Телефон" /></td>
						</tr>

                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="2. Ідентифікація балансоутримувача">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <table border="0" cellspacing="0" cellpadding="2" width="1510px">
						<colgroup>
							<col style="width:300px">
							<col style="width:25%">
							<col style="width:300px">
							<col style="width:25%">
						</colgroup>

                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Погодження балансоутримувача"></dx:ASPxLabel></td>
							<td><dx:ASPxCheckBox ID="v5" runat="server" Checked='<%# ("Так".Equals(Eval("v5"))) ? true : false %>' ToolTip="Погодження балансоутримувача" /></td>
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
							<td><dx:ASPxCheckBox ID="v9" runat="server" Checked='<%# ("Так".Equals(Eval("v9"))) ? true : false %>' ToolTip="Інформація про згоду на здійснення поточного та/або капітального ремонту орендованого майна" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити підтверджуючого документу про згоду на здійснення ремонту (за наявності)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v10" ClientInstanceName="v10" runat="server" Text='<%# Eval("v10") %>' Width="350px" Title="Реквізити підтверджуючого документу про згоду на здійснення ремонту (за наявності)" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Погодження органу управління "></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v15" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DataSourceID="SqlDataSourceYesNoNone" Value='<%# Eval("v15") %>' Title="Погодження органу управління" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити підтверджуючого документу про погодження органу управління (за наявності)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v16" ClientInstanceName="v16" runat="server" Text='<%# Eval("v16") %>' Width="350px" Title="Реквізити підтверджуючого документу про погодження органу управління (за наявності)" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Погодження органу охорони культурної спадщини"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v17" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DataSourceID="SqlDataSourceYesNoNone" Value='<%# Eval("v17") %>' Title="Погодження органу охорони культурної спадщини" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити підтверджуючого документу про погодження органу охорони культ. спадищин (за наявності)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v18" ClientInstanceName="v18" runat="server" Text='<%# Eval("v18") %>' Width="350px" Title="Реквізити підтверджуючого документу про погодження органу охорони культ. спадищин (за наявності)" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ідентифікатор організації"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v57" ClientInstanceName="v57" runat="server" Text='<%# Eval("v57") %>' Width="350px" Title="Ідентифікатор організації" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Повна юридична назва організації"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v58" ClientInstanceName="v58" runat="server" Text='<%# Eval("v58") %>' Width="350px" Title="Повна юридична назва організації" /></td>
                        </tr>
						<tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Країна"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v203" ClientInstanceName="v203" runat="server" Text='<%# Eval("v203") %>' Width="350px" Title="Країна" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інформація щодо підтвердження повноважень"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v204" ClientInstanceName="v204" runat="server" Text='<%# Eval("v204") %>' Width="350px" Title="Інформація щодо підтвердження повноважень" /></td>
						</tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="область"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v59" ClientInstanceName="v59" runat="server" Text='<%# Eval("v59") %>' Width="350px" Title="область" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Населений пункт"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v60" ClientInstanceName="v60" runat="server" Text='<%# Eval("v60") %>' Width="350px" Title="Населений пункт" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Вулиця, будинок, квартира"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v61" ClientInstanceName="v61" runat="server" Text='<%# Eval("v61") %>' Width="350px" Title="Вулиця, будинок, квартира" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Індекс"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v62" ClientInstanceName="v62" runat="server" Text='<%# Eval("v62") %>' Width="350px" Title="Індекс" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Призвище"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v63" ClientInstanceName="v63" runat="server" Text='<%# Eval("v63") %>' Width="350px" Title="Призвище" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ім'я"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v64" ClientInstanceName="v64" runat="server" Text='<%# Eval("v64") %>' Width="350px" Title="Ім'я" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="По батькові"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v65" ClientInstanceName="v65" runat="server" Text='<%# Eval("v65") %>' Width="350px" Title="По батькові" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="E-mail для повідомлень"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v66" ClientInstanceName="v66" runat="server" Text='<%# Eval("v66") %>' Width="350px" Title="E-mail для повідомлень" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Телефон"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v67" ClientInstanceName="v67" runat="server" Text='<%# Eval("v67") %>' Width="350px" Title="Телефон" /></td>
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
                    <table border="0" cellspacing="0" cellpadding="2" width="1510px">
						<colgroup>
							<col style="width:300px">
							<col style="width:25%">
							<col style="width:300px">
							<col style="width:25%">
						</colgroup>

                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ідентифікатор організації"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v68" ClientInstanceName="v68" runat="server" Text='<%# Eval("v68") %>' Width="350px" Title="Ідентифікатор організації" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Повна юридична назва організації"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v69" ClientInstanceName="v69" runat="server" Text='<%# Eval("v69") %>' Width="350px" Title="Повна юридична назва організації" /></td>
                        </tr>
						<tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інформація щодо підтвердження повноважень"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v205" ClientInstanceName="v205" runat="server" Text='<%# Eval("v205") %>' Width="350px" Title="Інформація щодо підтвердження повноважень" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Країна"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v206" ClientInstanceName="v206" runat="server" Text='<%# Eval("v206") %>' Width="350px" Title="Країна" /></td>
						</tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Область"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v70" ClientInstanceName="v70" runat="server" Text='<%# Eval("v70") %>' Width="350px" Title="Область" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Населений пункт"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v71" ClientInstanceName="v71" runat="server" Text='<%# Eval("v71") %>' Width="350px" Title="Населений пункт" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Вулиця, будинок, квартира"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v72" ClientInstanceName="v72" runat="server" Text='<%# Eval("v72") %>' Width="350px" Title="Вулиця, будинок, квартира" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Індекс"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v73" ClientInstanceName="v73" runat="server" Text='<%# Eval("v73") %>' Width="350px" Title="Індекс" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Призвище"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v74" ClientInstanceName="v74" runat="server" Text='<%# Eval("v74") %>' Width="350px" Title="Призвище" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ім'я"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v75" ClientInstanceName="v75" runat="server" Text='<%# Eval("v75") %>' Width="350px" Title="Ім'я" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="По батькові"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v76" ClientInstanceName="v76" runat="server" Text='<%# Eval("v76") %>' Width="350px" Title="По батькові" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="E-mail для повідомлень"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v77" ClientInstanceName="v77" runat="server" Text='<%# Eval("v77") %>' Width="350px" Title="E-mail для повідомлень" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Телефон"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v78" ClientInstanceName="v78" runat="server" Text='<%# Eval("v78") %>' Width="350px" Title="Телефон" /></td>
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
                    <table border="0" cellspacing="0" cellpadding="2" width="1510px">
						<colgroup>
							<col style="width:300px">
							<col style="width:25%">
							<col style="width:300px">
							<col style="width:25%">
						</colgroup>


                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ідентифікатор організації"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v79" ClientInstanceName="v79" runat="server" Text='<%# Eval("v79") %>' Width="350px" Title="Ідентифікатор організації" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Повна юридична назва організації"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v80" ClientInstanceName="v80" runat="server" Text='<%# Eval("v80") %>' Width="350px" Title="Повна юридична назва організації" /></td>
                        </tr>

                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інформація щодо підтвердження повноважень"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v207" ClientInstanceName="v207" runat="server" Text='<%# Eval("v207") %>' Width="350px" Title="Інформація щодо підтвердження повноважень" /></td>
							<td align="right"><dx:ASPxLabel runat="server" Text="Країна"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v208" ClientInstanceName="v208" runat="server" Text='<%# Eval("v208") %>' Width="350px" Title="Країна" /></td>
                        </tr>

                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="область"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v81" ClientInstanceName="v81" runat="server" Text='<%# Eval("v81") %>' Width="350px" Title="область" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Населений пункт"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v82" ClientInstanceName="v82" runat="server" Text='<%# Eval("v82") %>' Width="350px" Title="Населений пункт" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Вулиця, будинок, квартира"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v83" ClientInstanceName="v83" runat="server" Text='<%# Eval("v83") %>' Width="350px" Title="Вулиця, будинок, квартира" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Індекс"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v84" ClientInstanceName="v84" runat="server" Text='<%# Eval("v84") %>' Width="350px" Title="Індекс" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Призвище"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v85" ClientInstanceName="v85" runat="server" Text='<%# Eval("v85") %>' Width="350px" Title="Призвище" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ім'я"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v86" ClientInstanceName="v86" runat="server" Text='<%# Eval("v86") %>' Width="350px" Title="Ім'я" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="По батькові"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v87" ClientInstanceName="v87" runat="server" Text='<%# Eval("v87") %>' Width="350px" Title="По батькові" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="E-mail для повідомлень"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v88" ClientInstanceName="v88" runat="server" Text='<%# Eval("v88") %>' Width="350px" Title="E-mail для повідомлень" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Телефон"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v89" ClientInstanceName="v89" runat="server" Text='<%# Eval("v89") %>' Width="350px" Title="Телефон" /></td>
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
                    <table border="0" cellspacing="0" cellpadding="2" width="1510px">
						<colgroup>
							<col style="width:300px">
							<col style="width:25%">
							<col style="width:300px">
							<col style="width:25%">
						</colgroup>

                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Тип об’єкта"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v209" Value='<%# Eval("v209") %>' Title="Тип об’єкта" DataSourceID="SqlDataSourceTypeObject" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Назва об`єкту реєстра"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v1" ClientInstanceName="v1" runat="server" Text='<%# Eval("v1") %>' Width="350px" Title="Назва об`єкту реєстра" /></td>
                        </tr>
                        <%--<tr>
							<td align="right"><dx:ASPxLabel runat="server" Text="Країна об`єкту реєстра"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v210" ClientInstanceName="v210" runat="server" Text='<%# Eval("v210") %>' Width="350px" Title="Країна об`єкту реєстра" /></td>
							<td align="right"><dx:ASPxLabel runat="server" Text="Область об`єкту реєстра"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v211" ClientInstanceName="v211" runat="server" Text='<%# Eval("v211") %>' Width="350px" Title="Область об`єкту реєстра" /></td>
                        </tr>--%>
                        <tr>
                            <%--<td align="right"><dx:ASPxLabel runat="server" Text="Місто об`єкту реєстра"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v212" ClientInstanceName="v212" runat="server" Text='<%# Eval("v212") %>' Width="350px" Title="Місто об`єкту реєстра" /></td>--%>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Будинок та вулиця об`єкту реєстра"></dx:ASPxLabel></td>
                            <td ><dx:ASPxTextBox ID="v213" ClientInstanceName="v213" runat="server" Text='<%# Eval("v213") %>' Width="350px" Title="Будинок та вулиця об`єкту реєстра" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Тип власності"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v2" Value='<%# Eval("v2") %>' DataSourceID="SqlDataSourceTypeVlasn" Title="Тип власності" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Тип переліку, до якого віднесено об'єкт"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v3" Value='<%# Eval("v3") %>' Title="Тип переліку, до якого віднесено об'єкт" DataSourceID="SqlDataSourcePerelikType" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Статус об'єкта в переліку"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v4" Value='<%# Eval("v4") %>' Title="Статус об'єкта в переліку" DataSourceID="SqlDataSourcePerelikStatus" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Рішення про передачу об'єкта на приватизацію"></dx:ASPxLabel></td>
							<td><dx:ASPxCheckBox ID="v11" runat="server" Checked='<%# ("Так".Equals(Eval("v11"))) ? true : false %>' ToolTip="Рішення про передачу об'єкта на приватизацію" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити підтверджуючого документу про передачу об'єкта на приватизацію (за наявності)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v12" ClientInstanceName="v12" runat="server" Text='<%# Eval("v12") %>' Width="350px" Title="Реквізити підтверджуючого документу про передачу об'єкта на приватизацію (за наявності)" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Рішення про проведення інвестиційного конкурсу"></dx:ASPxLabel></td>
							<td><dx:ASPxCheckBox ID="v13" runat="server" Checked='<%# ("Так".Equals(Eval("v13"))) ? true : false %>' ToolTip="Рішення про проведення інвестиційного конкурсу" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити підтверджуючого документу про проведення інвестиційного конкурсу (за наявності)"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v14" ClientInstanceName="v14" runat="server" Text='<%# Eval("v14") %>' Width="350px" Title="Реквізити підтверджуючого документу про проведення інвестиційного конкурсу (за наявності)" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Стан пам'ятки культурної спадщини"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v214" ClientInstanceName="v214" runat="server" Text='<%# Eval("v214") %>' Width="350px" Title="Стан пам'ятки культурної спадщини" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Стан державної реєстрації об'єкта"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v19" Value='<%# Eval("v19") %>' Title="Стан державної реєстрації об'єкта" DataSourceID="SqlDataSourceStanReestr" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити державної реєстрації об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v20" ClientInstanceName="v20" runat="server" Text='<%# Eval("v20") %>' Width="350px" Title="Реквізити державної реєстрації об'єкта" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Первісна балансова вартість, грн"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v34" ClientInstanceName="v34" runat="server" Text='<%# Eval("v34") %>' Width="350px" Title="Первісна балансова вартість, грн" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Дані зі звіту Баланс"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v215" ClientInstanceName="v215" runat="server" Text='<%# Eval("v215") %>' Width="350px" Title="Дані зі звіту Баланс" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Залишкова балансова вартість, грн"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v35" ClientInstanceName="v35" runat="server" Text='<%# Eval("v35") %>' Width="350px" Title="Залишкова балансова вартість, грн" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ринкова вартість, грн"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v36" ClientInstanceName="v36" runat="server" Text='<%# Eval("v36") %>' Width="350px" Title="Ринкова вартість, грн" NumberType="Float" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Дата визначення ринкової вартості"></dx:ASPxLabel></td>
                            <td><dx:ASPxDateEdit ID="v37" ClientInstanceName="v37" runat="server" Value='<%# Eval("v37") %>' Width="350px" Title="Дата визначення ринкової вартості" /></td>
							<td align="right"><dx:ASPxLabel runat="server" Text="Інформація про оцінювача та необхідність компенсації оцінки орендарем"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v216" ClientInstanceName="v216" runat="server" Text='<%# Eval("v216") %>' Width="350px" Title="Інформація про оцінювача та необхідність компенсації оцінки орендарем" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Вартість оцінки, грн"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v217" ClientInstanceName="v217" runat="server" Text='<%# Eval("v217") %>' Width="350px" Title="Вартість оцінки, грн" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Сума компенсації, грн"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v218" ClientInstanceName="v218" runat="server" Text='<%# Eval("v218") %>' Width="350px" Title="Сума компенсації, грн" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Рішення про затвердження переліку об’єктів, або про включення нового об’єкта до переліку"></dx:ASPxLabel></td>
							<td><dx:ASPxCheckBox ID="v45" runat="server" Checked='<%# ("Так".Equals(Eval("v45"))) ? true : false %>' ToolTip="Рішення про затвердження переліку об’єктів, або про включення нового об’єкта до переліку" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Реквізити рішення про затвердження переліку об’єктів, або про включення нового об’єкта до переліку"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v219" ClientInstanceName="v219" runat="server" Text='<%# Eval("v219") %>' Width="350px" Title="Реквізити рішення про затвердження переліку об’єктів, або про включення нового об’єкта до переліку" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Назва об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v90" ClientInstanceName="v90" runat="server" Text='<%# Eval("v90") %>' Width="350px" Title="Назва об'єкта" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Опис об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v91" ClientInstanceName="v91" runat="server" Text='<%# Eval("v91") %>' Width="350px" Title="Опис об'єкта" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Основний класифікатор об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v92" ClientInstanceName="v92" runat="server" Text='<%# Eval("v92") %>' Width="350px" Title="Основний класифікатор об'єкта" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Країна"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v220" ClientInstanceName="v220" runat="server" Text='<%# Eval("v220") %>' Width="350px" Title="Країна" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="область"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v93" ClientInstanceName="v93" runat="server" Text='<%# Eval("v93") %>' Width="350px" Title="область" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Населений пункт"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v94" ClientInstanceName="v94" runat="server" Text='<%# Eval("v94") %>' Width="350px" Title="Населений пункт" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Вулиця, будинок, квартира"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v95" ClientInstanceName="v95" runat="server" Text='<%# Eval("v95") %>' Width="350px" Title="Вулиця, будинок, квартира" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Індекс"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v96" ClientInstanceName="v96" runat="server" Text='<%# Eval("v96") %>' Width="350px" Title="Індекс" /></td>
                        </tr>
                        <tr>
							<td align="right"><dx:ASPxLabel runat="server" Text="Широта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v221" ClientInstanceName="v221" runat="server" Text='<%# Eval("v221") %>' Width="350px" Title="Широта" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Довгота"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v222" ClientInstanceName="v222" runat="server" Text='<%# Eval("v222") %>' Width="350px" Title="Довгота" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Рік будівництва"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v98" ClientInstanceName="v98" runat="server" Text='<%# Eval("v98") %>' Width="350px" Title="Рік будівництва" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Місце розташування об’єкта в будівлі"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v99" Value='<%# Eval("v99") %>' Title="Місце розташування об’єкта в будівлі" DataSourceID="SqlDataSourceMistoInDom" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Номер поверху або поверхів"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v100" ClientInstanceName="v100" runat="server" Text='<%# Eval("v100") %>' Width="350px" Title="Номер поверху або поверхів" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Загальна площа об'єкта в будівлі, кв.м."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v102" ClientInstanceName="v102" runat="server" Text='<%# Eval("v102") %>' Width="350px" Title="Загальна площа об'єкта в будівлі, кв.м." NumberType="Float" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Корисна площа об'єкта в будівлі, кв.м."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v103" ClientInstanceName="v103" runat="server" Text='<%# Eval("v103") %>' Width="350px" Title="Корисна площа об'єкта в будівлі, кв.м." NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Технічний стан об'єкта оренди"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v107" ClientInstanceName="v107" runat="server" Text='<%# Eval("v107") %>' Width="350px" Title="Технічний стан об'єкта оренди" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Чи приєднаний об'єкт оренди до електромережі"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v108" Value='<%# Eval("v108") %>' Title="Чи приєднаний об'єкт оренди до електромережі" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Водозабезпечення присутнє"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v109" Value='<%# Eval("v109") %>' Title="Водозабезпечення присутнє" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Централізоване опалення присутнє"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v110" Value='<%# Eval("v110") %>' Title="Централізоване опалення присутнє" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інформація про наявність окремих особових рахунків на об'єкт оренди, відкритих постачальниками комунальних послуг, або інформація про порядок участі орендаря у компенсації балансоутримувачу витрат на оплату комунальних послуг"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v126" ClientInstanceName="v126" runat="server" Text='<%# Eval("v126") %>' Width="350px" Title="Інформація про наявність окремих особових рахунків на об'єкт оренди, відкритих постачальниками комунальних послуг, або інформація про порядок участі орендаря у компенсації балансоутримувачу витрат на оплату комунальних послуг" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інформація щодо компенсації балансоутримувачу сплати земельного податку за користування земельною ділянкою, на якій розташований об'єкт оренди"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v127" ClientInstanceName="v127" runat="server" Text='<%# Eval("v127") %>' Width="350px" Title="Інформація щодо компенсації балансоутримувачу сплати земельного податку за користування земельною ділянкою, на якій розташований об'єкт оренди" /></td>
							<td align="right"><dx:ASPxLabel runat="server" Text="Технологія будівництва"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v97" Value='<%# Eval("v97") %>' Title="Технологія будівництва" DataSourceID="SqlDataSourceTechnologia" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Загальна площа будівлі, кв.м."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v101" ClientInstanceName="v101" runat="server" Text='<%# Eval("v101") %>' Width="350px" Title="Загальна площа будівлі, кв.м." NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Житлова площа, кв. м"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v104" ClientInstanceName="v104" runat="server" Text='<%# Eval("v104") %>' Width="350px" Title="Житлова площа, кв. м" NumberType="Float" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Площа кухні, кв. м"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v105" ClientInstanceName="v105" runat="server" Text='<%# Eval("v105") %>' Width="350px" Title="Площа кухні, кв. м" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Площа ділянки, кв. м"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v106" ClientInstanceName="v106" runat="server" Text='<%# Eval("v106") %>' Width="350px" Title="Площа ділянки, кв. м" NumberType="Float" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Вентиляція присутня"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v111" Value='<%# Eval("v111") %>' Title="Вентиляція присутня" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Телебачення присутнє"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v112" Value='<%# Eval("v112") %>' Title="Телебачення присутнє" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Охоронна сигналізація присутня"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v113" Value='<%# Eval("v113") %>' Title="Охоронна сигналізація присутня" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Каналізація присутня"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v114" Value='<%# Eval("v114") %>' Title="Каналізація присутня" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Автономне опалення присутнє"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v115" Value='<%# Eval("v115") %>' Title="Автономне опалення присутнє" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Кондиціонування присутнє"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v116" Value='<%# Eval("v116") %>' Title="Кондиціонування присутнє" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інтернет присутній"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v117" Value='<%# Eval("v117") %>' Title="Інтернет присутній" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Пожежна сигналізація присутня"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v118" Value='<%# Eval("v118") %>' Title="Пожежна сигналізація присутня" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Газифікація присутня"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v119" Value='<%# Eval("v119") %>' Title="Газифікація присутня" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Лічильник опалення присутній"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v120" Value='<%# Eval("v120") %>' Title="Лічильник опалення присутній" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Телефонізація присутня"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v121" Value='<%# Eval("v121") %>' Title="Телефонізація присутня" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Ліфт присутній"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v122" Value='<%# Eval("v122") %>' Title="Ліфт присутній" DataSourceID="SqlDataSourceUnknownYesNo" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                        </tr>
                        <tr>
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
                    <table border="0" cellspacing="0" cellpadding="2" width="1510px">
						<colgroup>
							<col style="width:300px">
							<col style="width:25%">
							<col style="width:300px">
							<col style="width:25%">
						</colgroup>


                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Тривалість оренди"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v21" ClientInstanceName="v21" runat="server" Text='<%# Eval("v21") %>' Width="350px" Title="Тривалість оренди" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Тривалість оренди місяців"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v22" ClientInstanceName="v22" runat="server" Text='<%# Eval("v22") %>' Width="350px" Title="Тривалість оренди місяців" NumberType="Float" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Тривалість оренди днів"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v23" ClientInstanceName="v23" runat="server" Text='<%# Eval("v23") %>' Width="350px" Title="Тривалість оренди днів" NumberType="Float" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Спосіб обмеження цільового призначення об'єкта"></dx:ASPxLabel></td>
							<td><dx:ASPxComboBox ID="v24" Value='<%# Eval("v24") %>' Title="Спосіб обмеження цільового призначення об'єкта" DataSourceID="SqlDataSourceObmezhen" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Графікова оренда: кількість годин"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v25" ClientInstanceName="v25" runat="server" Text='<%# Eval("v25") %>' Width="350px" Title="Графікова оренда: кількість годин" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Кількість годин передбачається на"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v26" ClientInstanceName="v26" runat="server" Text='<%# Eval("v26") %>' Width="350px" Title="Кількість годин передбачається на" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Графікова оренда: кількість днів"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v27" ClientInstanceName="v27" runat="server" Text='<%# Eval("v27") %>' Width="350px" Title="Графікова оренда: кількість днів" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Кількість дні передбачається на"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v28" ClientInstanceName="v28" runat="server" Text='<%# Eval("v28") %>' Width="350px" Title="Кількість дні передбачається на" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Інший графік використання"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v29" ClientInstanceName="v29" runat="server" Text='<%# Eval("v29") %>' Width="350px" Title="Інший графік використання" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Цільове призначення об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxComboBox ID="v30" Value='<%# Eval("v30") %>' Title="Цільове призначення об'єкта" DataSourceID="SqlDataSourcePriznacheno" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Обмеження щодо використання майна (заборонені цільові призначення)"></dx:ASPxLabel></td>
                            <td><dx:ASPxComboBox ID="v31" Value='<%# Eval("v31") %>' Title="Обмеження щодо використання майна (заборонені цільові призначення)" DataSourceID="SqlDataObmezhena" runat="server" ValueType="System.String" TextField="name" ValueField="name" Width="350px" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" AllowNull="true" ItemStyle-Wrap="True" DropDownWidth="1100px" /></td>
                            <%--<td><dx:ASPxTextBox ID="v31" ClientInstanceName="v31" runat="server" Text='<%# Eval("v31") %>' Width="350px" Title="Обмеження щодо використання майна (заборонені цільові призначення)" /></td>--%>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Опис обмежень цільового призначення об'єкта"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v223" ClientInstanceName="v223" runat="server" Text='<%# Eval("v223") %>' Width="350px" Title="Опис обмежень цільового призначення об'єкта" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Додаткові умови оренди майна"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v32" ClientInstanceName="v32" runat="server" Text='<%# Eval("v32") %>' Width="350px" Title="Додаткові умови оренди майна" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Можливість суборенди"></dx:ASPxLabel></td>
							<td><dx:ASPxCheckBox ID="v33" runat="server" Checked='<%# ("Так".Equals(Eval("v33"))) ? true : false %>' ToolTip="Можливість суборенди" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Посилання на пункт Методики розрахунку орендної плати, яким встановлена орендна ставка для запропонованого цільового призначення"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="v41" ClientInstanceName="v41" runat="server" Text='<%# Eval("v41") %>' Width="350px" Title="Посилання на пункт Методики розрахунку орендної плати, яким встановлена орендна ставка для запропонованого цільового призначення" /></td>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Фактичне значення орендної ставки, грн"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v42" ClientInstanceName="v42" runat="server" Text='<%# Eval("v42") %>' Width="350px" Title="Фактичне значення орендної ставки, грн" NumberType="Float" /></td>
                        </tr>
                        <tr>
                            <td align="right"><dx:ASPxLabel runat="server" Text="Значення орендної ставки у відсотках"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="v43" ClientInstanceName="v43" runat="server" Text='<%# Eval("v43") %>' Width="350px" Title="Значення орендної ставки у відсотках" NumberType="Float" /></td>
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
