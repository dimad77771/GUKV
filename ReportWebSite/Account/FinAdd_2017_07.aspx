<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinAdd.aspx.cs" Inherits="Account_FinAdd" MasterPageFile="~/NoMenu.master" Title="Інформація" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        h3
        {
        	font-size:20.0pt;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<%--        <h2>
        Увага
    </h2>


<p style="width: 600px;">       
        Шановні балансоутримувачі,
        повідомляємо Вам, що новий метод подання звіту щодо використання комунального майна, у майбутньому буде поширений також на фінансову звітність. Про терміни впровадження зовнішньої подачі фінансової звітності буде повідомлено пізніше.
        Пропозиції та зауваження чекаємо на адресу: 
    </p>--%>



    <div style="padding:20px">

 
    <p class="MsoNormal">
        <span lang="UK" style="font-size:24.0pt;color:rgb(46, 108, 184);mso-bidi-font-size:11.0pt;
line-height:100%;font-family:&quot;Calibri&quot;,&quot;serif&quot;"><b>Шановний користувачу!</b><o:p></o:p></span>
    </p>
    <p class="MsoNormal">
        <span lang="UK" style="font-size:14.0pt;color:black;mso-bidi-font-size:11.0pt;
line-height:150%;font-family:&quot;Calibri&quot;,&quot;serif&quot;">З 07.03.2017 року введено НЕЗМІННУ у подальшому адресу доступу до ЄІС ДКВ: <a href="http://eis.gukv.gov.ua/gukv/">eis.gukv.gov.ua/gukv/</a>
<br>При цьому ім'я та пароль користувача не змінюються.
<o:p></o:p></span></p>
    <h3>
        <span lang="UK" style="font-family:Calibri;color:red;mso-fareast-theme-font:major-latin;
mso-bidi-font-family:Cambria;mso-bidi-theme-font:major-latin">
        <span style="mso-list:Ignore">
           <b>Зверніть увагу на основні принципи функціонування системи дистанційної подачі звітності ЄІС ДКВ з першого півріччя 2017 року: </b>
            </span>
            <br>
     </h3>
    <p class="MsoNormal">
        <span lang="UK" style="font-size:13.0pt;mso-bidi-font-size:11.0pt;
line-height:100%;font-family:&quot;Calibri&quot;,&quot;serif&quot;"><i>
•	Балансоутримувачі комунального майна зобов’язані актуалізовувати загальні відомості про організацію та інформацію щодо об’єктів, які знаходяться у них на балансі, договорів оренди та надходження орендної плати щоквартально (до 20 квітня, до 20 липня, до 20 жовтня, до 20 січня) і підтверджувати це шляхом надання до ДКВ електронної копії Звіту з використання комунального майна, завіреного посадовими особами балансоутримувача, у форматі PDF. Звіт формується у системі ЄІС ДКВ шляхом натискання кнопки «Надіслати» розділу «Загальна інформація» та кнопки «Сформувати звіт» розділу «Стан» відповідно. Електронна копія Звіту надсилається на адресу технічної підтримки ЄІС ДКВ (seic@gukv.gov.ua) із заголовком повідомлення у форматі: код ЕДРПОУ # коротка назва. Після перевірки звіту користувач отримує на електронну пошту повідомлення «Прийнято» або зауваження до Звіту, після усунення яких слід повторити процедуру надання Звіту до ДКВ.<br><br>
•	Інші комунальні підприємства, установи, організації зобов’язані актуалізовувати і підтверджувати загальні відомості про організацію та  інформацію щодо об’єктів, які вони орендують або використовують на інших підставах, і щоквартально (до 20 квітня, до 20 липня, до 20 жовтня, до 20 січня) надсилати до ДКВ через систему дистанційної подачі звітності шляхом натискання кнопки «Надіслати» розділу «Загальна інформація».<br><br>
•	Для зарахування подання звітності в ЄІС ДКВ у розділі «Стан» повинна стояти позначка «Надісланий», а дата останнього надсилання має бути між 1 та 20 числом місяця, наступного за звітним кварталом.<br><br>
•	Надання звітності за формою «Звіт за 3 місяців 2017 року», яку розміщено на сайті Департаменту за адресою: <a href="http://www.gukv.gov.ua/звіти/">http://www.gukv.gov.ua/звіти/</a>, скасовується.<br><br>
•	Відповідальність за достовірність інформації несе балансоутримувач.<br></i>
            <o:p></o:p></span>
    </p>
 

  </div>

    <p>
        <dx:ASPxHyperLink ID="LinkToCabinet" runat="server" Text="Продовжити" NavigateUrl="~/Default.aspx" Font-Size="14.00pt"/>
    </p>
</asp:Content>
  