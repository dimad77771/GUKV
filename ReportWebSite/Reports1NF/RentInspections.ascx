<%@ Control Language="C#" CodeFile="RentInspections.ascx.cs" Inherits="RentInspections" %>
<%@ Register Assembly="DevExpress.Web.v20.1" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:SqlDataSource ID="Representatives" runat="server" ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT DISTINCT name FROM dbo.dict_lessor_representatives WHERE name IS NOT NULL ORDER BY name" />
<asp:SqlDataSource ID="Inspections" runat="server" ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    OnSelecting="SelectInspection" OnInserting="WriteInspection" OnUpdating="WriteInspection" OnDeleting="WriteInspection"
    SelectCommand="SELECT * FROM dbo.reports1nf_arenda_inspections WHERE report_id=@rid AND arenda_id=@aid ORDER BY act_date DESC, id DESC"
    InsertCommand="INSERT INTO dbo.reports1nf_arenda_inspections
        (report_id, arenda_id, order_num, order_date, lessor_rep, holder_rep, tenant_rep, holder_name, holder_street, holder_house, area, agreement_num, agreement_date, property_use, condition, sublease, area_matches, rent_payment, insurance, accessibility, other_info, documents, violations, warning, recorded_by, receipt, sent_date, sent_address, sender, act_num, act_date)
        VALUES (@rid, @aid, @order_num, @order_date, @lessor_rep, @holder_rep, @tenant_rep, @holder_name, @holder_street, @holder_house, @area, @agreement_num, @agreement_date, @property_use, @condition, @sublease, @area_matches, @rent_payment, @insurance, @accessibility, @other_info, @documents, @violations, @warning, @recorded_by, @receipt, @sent_date, @sent_address, @sender, @act_num, @act_date)"
    UpdateCommand="UPDATE dbo.reports1nf_arenda_inspections SET
        order_num=@order_num,
        order_date=@order_date,
        lessor_rep=@lessor_rep,
        holder_rep=@holder_rep,
        tenant_rep=@tenant_rep,
        holder_name=@holder_name,
        holder_street=@holder_street,
        holder_house=@holder_house,
        area=@area,
        agreement_num=@agreement_num,
        agreement_date=@agreement_date,
        property_use=@property_use,
        condition=@condition,
        sublease=@sublease,
        area_matches=@area_matches,
        rent_payment=@rent_payment,
        insurance=@insurance,
        accessibility=@accessibility,
        other_info=@other_info,
        documents=@documents,
        violations=@violations,
        warning=@warning,
        recorded_by=@recorded_by,
        receipt=@receipt,
        sent_date=@sent_date,
        sent_address=@sent_address,
        sender=@sender,
        act_num=@act_num,
        act_date=@act_date
        WHERE id=@id AND report_id=@rid AND arenda_id=@aid"
    DeleteCommand="DELETE FROM dbo.reports1nf_arenda_inspections WHERE id=@id AND report_id=@rid AND arenda_id=@aid">
    <InsertParameters>
        <asp:Parameter Name="order_num" DbType="AnsiString" />
        <asp:Parameter Name="order_date" DbType="Date" />
        <asp:Parameter Name="lessor_rep" DbType="AnsiString" />
        <asp:Parameter Name="holder_rep" DbType="AnsiString" />
        <asp:Parameter Name="tenant_rep" DbType="AnsiString" />
        <asp:Parameter Name="holder_name" DbType="AnsiString" />
        <asp:Parameter Name="holder_street" DbType="AnsiString" />
        <asp:Parameter Name="holder_house" DbType="AnsiString" />
        <asp:Parameter Name="area" DbType="Decimal" />
        <asp:Parameter Name="agreement_num" DbType="AnsiString" />
        <asp:Parameter Name="agreement_date" DbType="AnsiString" />
        <asp:Parameter Name="property_use" DbType="AnsiString" />
        <asp:Parameter Name="condition" DbType="AnsiString" />
        <asp:Parameter Name="sublease" DbType="AnsiString" />
        <asp:Parameter Name="area_matches" DbType="AnsiString" />
        <asp:Parameter Name="rent_payment" DbType="AnsiString" />
        <asp:Parameter Name="insurance" DbType="AnsiString" />
        <asp:Parameter Name="accessibility" DbType="AnsiString" />
        <asp:Parameter Name="other_info" DbType="AnsiString" />
        <asp:Parameter Name="documents" DbType="AnsiString" />
        <asp:Parameter Name="violations" DbType="AnsiString" />
        <asp:Parameter Name="warning" DbType="AnsiString" />
        <asp:Parameter Name="recorded_by" DbType="AnsiString" />
        <asp:Parameter Name="receipt" DbType="AnsiString" />
        <asp:Parameter Name="sent_date" DbType="Date" />
        <asp:Parameter Name="sent_address" DbType="AnsiString" />
        <asp:Parameter Name="sender" DbType="AnsiString" />
        <asp:Parameter Name="act_num" DbType="AnsiString" />
        <asp:Parameter Name="act_date" DbType="Date" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="id" Type="Int32" />
        <asp:Parameter Name="order_num" DbType="AnsiString" />
        <asp:Parameter Name="order_date" DbType="Date" />
        <asp:Parameter Name="lessor_rep" DbType="AnsiString" />
        <asp:Parameter Name="holder_rep" DbType="AnsiString" />
        <asp:Parameter Name="tenant_rep" DbType="AnsiString" />
        <asp:Parameter Name="holder_name" DbType="AnsiString" />
        <asp:Parameter Name="holder_street" DbType="AnsiString" />
        <asp:Parameter Name="holder_house" DbType="AnsiString" />
        <asp:Parameter Name="area" DbType="Decimal" />
        <asp:Parameter Name="agreement_num" DbType="AnsiString" />
        <asp:Parameter Name="agreement_date" DbType="AnsiString" />
        <asp:Parameter Name="property_use" DbType="AnsiString" />
        <asp:Parameter Name="condition" DbType="AnsiString" />
        <asp:Parameter Name="sublease" DbType="AnsiString" />
        <asp:Parameter Name="area_matches" DbType="AnsiString" />
        <asp:Parameter Name="rent_payment" DbType="AnsiString" />
        <asp:Parameter Name="insurance" DbType="AnsiString" />
        <asp:Parameter Name="accessibility" DbType="AnsiString" />
        <asp:Parameter Name="other_info" DbType="AnsiString" />
        <asp:Parameter Name="documents" DbType="AnsiString" />
        <asp:Parameter Name="violations" DbType="AnsiString" />
        <asp:Parameter Name="warning" DbType="AnsiString" />
        <asp:Parameter Name="recorded_by" DbType="AnsiString" />
        <asp:Parameter Name="receipt" DbType="AnsiString" />
        <asp:Parameter Name="sent_date" DbType="Date" />
        <asp:Parameter Name="sent_address" DbType="AnsiString" />
        <asp:Parameter Name="sender" DbType="AnsiString" />
        <asp:Parameter Name="act_num" DbType="AnsiString" />
        <asp:Parameter Name="act_date" DbType="Date" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="id" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>
<dx:ASPxGridView ID="Grid" runat="server" DataSourceID="Inspections" KeyFieldName="id" AutoGenerateColumns="False" Width="100%" OnInitNewRow="InitInspection">
    <ClientSideEvents CustomButtonClick="function(s, e) {
        if (e.buttonID === 'InspectionAct') {
            e.processOnServer = false;
            window.location.href = s.cpActUrl + encodeURIComponent(s.GetRowKey(e.visibleIndex));
        }
    }" />
    <Templates>
        <EditForm>
            <dx:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" />
            <div style="text-align:left">
                <dx:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormUpdateButton" />
                <dx:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormCancelButton" />
            </div>
        </EditForm>
    </Templates>
    <SettingsEditing Mode="EditForm" EditFormColumnCount="2" />
    <SettingsText EmptyDataRow="Перевірок немає" />
    <SettingsCommandButton>
        <NewButton Text="Додати" RenderMode="Image"><Image Url="~/Styles/AddIcon.png" AlternateText="Додати" /></NewButton>
        <EditButton Text="Редагувати" RenderMode="Image"><Image Url="~/Styles/EditIcon.png" AlternateText="Редагувати" /></EditButton>
        <DeleteButton Text="Видалити" RenderMode="Image"><Image Url="~/Styles/DeleteIcon.png" AlternateText="Видалити" /></DeleteButton>
        <UpdateButton Text="Зберегти" RenderMode="Image"><Image Url="~/Styles/SaveIcon.png" AlternateText="Зберегти" /></UpdateButton>
        <CancelButton Text="Скасувати" RenderMode="Image"><Image Url="~/Styles/CancelIcon.png" AlternateText="Скасувати" /></CancelButton>
    </SettingsCommandButton>
    <Columns>
        <dx:GridViewCommandColumn Name="Actions" ButtonType="Image" CellStyle-Wrap="False" ShowNewButton="True" ShowEditButton="True" ShowDeleteButton="True">
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="InspectionAct" Text="Сформувати акт">
                    <Image Url="~/Styles/ReportDocument18.png" AlternateText="Сформувати акт" />
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
        </dx:GridViewCommandColumn>
        <dx:GridViewDataTextColumn FieldName="order_num" Caption="Номер наказу на проведення перевірки" Visible="True">
            <EditFormSettings Visible="True" VisibleIndex="0" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="order_date" Caption="Дата наказу на проведення перевірки" Visible="True">
            <EditFormSettings Visible="True" VisibleIndex="1" ColumnSpan="1" />
            <PropertiesDateEdit DisplayFormatString="dd.MM.yyyy" EditFormat="Custom" EditFormatString="dd.MM.yyyy" />
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataComboBoxColumn FieldName="lessor_rep" Caption="Представник орендодавця" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="2" ColumnSpan="1" />
            <PropertiesComboBox DataSourceID="Representatives" TextField="name" ValueField="name" ValueType="System.String" DropDownStyle="DropDown" />
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataTextColumn FieldName="holder_rep" Caption="Представник балансоутримувача" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="3" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="tenant_rep" Caption="Представник орендаря" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="4" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="holder_name" Caption="Назва балансоутримувача" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="5" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="holder_street" Caption="Адреса балансоутримувача (вулиця)" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="6" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="holder_house" Caption="Адреса балансоутримувача (номер будинку)" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="7" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="area" Caption="Площа приміщення, що використовується, кв.м" Visible="True">
            <EditFormSettings Visible="True" VisibleIndex="8" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_num" Caption="Номер договору" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="9" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_date" Caption="Дата укладання договору" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="10" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataMemoColumn FieldName="property_use" Caption="Використання майна (цільове призначення / фактичне)" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="11" ColumnSpan="1" />
        </dx:GridViewDataMemoColumn>
        <dx:GridViewDataComboBoxColumn FieldName="condition" Caption="Технічний стан об’єкта оренди" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="12" ColumnSpan="1" />
            <PropertiesComboBox ValueType="System.String"><Items>
                <dx:ListEditItem Text="Задовільний" Value="Задовільний" />
                <dx:ListEditItem Text="Незадовільний" Value="Незадовільний" />
            </Items></PropertiesComboBox>
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataComboBoxColumn FieldName="sublease" Caption="Наявність суборенди" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="13" ColumnSpan="1" />
            <PropertiesComboBox ValueType="System.String"><Items>
                <dx:ListEditItem Text="Так" Value="Так" />
                <dx:ListEditItem Text="Ні" Value="Ні" />
            </Items></PropertiesComboBox>
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataComboBoxColumn FieldName="area_matches" Caption="Відповідність займаної площі акту приймання-передавання" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="14" ColumnSpan="1" />
            <PropertiesComboBox ValueType="System.String"><Items>
                <dx:ListEditItem Text="Так" Value="Так" />
                <dx:ListEditItem Text="Ні" Value="Ні" />
            </Items></PropertiesComboBox>
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataComboBoxColumn FieldName="rent_payment" Caption="Надходження плати за оренду до балансоутримувача / бюджету міста Києва" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="15" ColumnSpan="1" />
            <PropertiesComboBox ValueType="System.String"><Items>
                <dx:ListEditItem Text="Заборгованість відсутня" Value="Заборгованість відсутня" />
                <dx:ListEditItem Text="Заборгованість наявна" Value="Заборгованість наявна" />
            </Items></PropertiesComboBox>
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataTextColumn FieldName="insurance" Caption="Наявність договору страхування" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="16" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataComboBoxColumn FieldName="accessibility" Caption="Пристосування об’єкта до потреб осіб з інвалідністю відповідно до ДБН В.2.2-40:2018" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="17" ColumnSpan="1" />
            <PropertiesComboBox ValueType="System.String"><Items>
                <dx:ListEditItem Text="Пристосовано" Value="Пристосовано" />
                <dx:ListEditItem Text="Не пристосовано" Value="Не пристосовано" />
            </Items></PropertiesComboBox>
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataMemoColumn FieldName="other_info" Caption="Інше (зазначити)" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="18" ColumnSpan="1" />
        </dx:GridViewDataMemoColumn>
        <dx:GridViewDataMemoColumn FieldName="documents" Caption="Надані під час контролю документи" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="19" ColumnSpan="1" />
        </dx:GridViewDataMemoColumn>
        <dx:GridViewDataComboBoxColumn FieldName="violations" Caption="Порушення щодо використання орендованого майна" Visible="True">
            <EditFormSettings Visible="True" VisibleIndex="20" ColumnSpan="1" />
            <PropertiesComboBox ValueType="System.String"><Items>
                <dx:ListEditItem Text="Виявлено" Value="Виявлено" />
                <dx:ListEditItem Text="Не виявлено" Value="Не виявлено" />
            </Items></PropertiesComboBox>
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataMemoColumn FieldName="warning" Caption="Попередження щодо усунення порушень" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="21" ColumnSpan="1" />
        </dx:GridViewDataMemoColumn>
        <dx:GridViewDataTextColumn FieldName="recorded_by" Caption="Фото (відео) фіксацію здійснював" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="22" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataMemoColumn FieldName="receipt" Caption="Примірник Акта отримав Орендар / від отримання відмовився" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="23" ColumnSpan="1" />
        </dx:GridViewDataMemoColumn>
        <dx:GridViewDataDateColumn FieldName="sent_date" Caption="Дата надсилання примірника Акта орендарю" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="24" ColumnSpan="1" />
            <PropertiesDateEdit DisplayFormatString="dd.MM.yyyy" EditFormat="Custom" EditFormatString="dd.MM.yyyy" />
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="sent_address" Caption="Реквізити адреси відправлення" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="25" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sender" Caption="Відправник (ПІБ, посада)" Visible="False">
            <EditFormSettings Visible="True" VisibleIndex="26" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="act_num" Caption="Номер акта перевірки" Visible="True">
            <EditFormSettings Visible="True" VisibleIndex="27" ColumnSpan="1" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="act_date" Caption="Дата акта перевірки" Visible="True">
            <EditFormSettings Visible="True" VisibleIndex="28" ColumnSpan="1" />
            <PropertiesDateEdit DisplayFormatString="dd.MM.yyyy" EditFormat="Custom" EditFormatString="dd.MM.yyyy" />
        </dx:GridViewDataDateColumn>
    </Columns>
</dx:ASPxGridView>
