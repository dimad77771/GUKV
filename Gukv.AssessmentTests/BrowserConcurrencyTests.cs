using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

// The actual card, scripts and DevExpress assemblies run in a minimal IIS Express host
// against a disposable DB. Authentication/navigation are replaced; save handlers are not.
[TestFixture]
[Category("AssessmentBrowser")]
public class BrowserConcurrencyTests
{
    private readonly SqlConcurrencyTests _database = new SqlConcurrencyTests();
    private Process _server;
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _context;
    private IPage _a;
    private IPage _b;
    private string _url;
    private string _host;
    private readonly System.Collections.Generic.List<string> _browserErrors = new System.Collections.Generic.List<string>();

    [OneTimeSetUp]
    public async Task StartIsolatedWebHost()
    {
        if (Environment.GetEnvironmentVariable("GUKV_ASSESSMENT_BROWSER_TESTS") != "1")
            Assert.Ignore("Set GUKV_ASSESSMENT_BROWSER_TESTS=1 and GUKV_ASSESSMENT_TEST_CONNECTION to run isolated IIS/browser tests.");
        _database.CreateIsolatedDatabase();
        string root = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", ".."));
        _host = Path.Combine(TestContext.CurrentContext.WorkDirectory, "web-host-" + Guid.NewGuid().ToString("N"));
        foreach (string folder in new[] { "Cards", "App_Code", "Bin", "Scripts", "Styles" }) Directory.CreateDirectory(Path.Combine(_host, folder));
        foreach (string file in new[] { "Cards/AssessmentCard.aspx", "Cards/AssessmentCard.aspx.cs", "App_Code/AssessmentMerge.cs",
            "App_Code/AssessmentCardStore.cs", "Scripts/AssessmentConcurrency.js", "Styles/AssessmentConcurrency.css" })
            File.Copy(Path.Combine(root, "ReportWebSite", file), Path.Combine(_host, file));
        foreach (string file in new[] { "DevExpress.Web.v20.1.dll", "DevExpress.Data.v20.1.dll", "DevExpress.Utils.v20.1.dll", "log4net.dll" })
            File.Copy(Path.Combine(root, "ReportWebSite", "Bin", file), Path.Combine(_host, "Bin", file));
        File.WriteAllText(Path.Combine(_host, "NoHeader.master"), @"<%@ Master Language=""C#"" %>
<!DOCTYPE html><html><head runat=""server""><title>Assessment test host</title><asp:ContentPlaceHolder ID=""HeadContent"" runat=""server"" /></head>
<body><form runat=""server""><asp:ContentPlaceHolder ID=""MainContent"" runat=""server"" /></form></body></html>");
        File.WriteAllText(Path.Combine(_host, "App_Code", "TestHost.cs"), @"
using System; using System.Data.SqlClient; using System.Web.UI; using System.Web.UI.WebControls;
namespace MiniProfilerHelpers { public class ProfiledSqlDataSource : SqlDataSource {} }
public static class Utils {
 static Utils() { var appender=new log4net.Appender.FileAppender(); appender.File=System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath,""save-errors.log""); appender.Layout=new log4net.Layout.PatternLayout(""%date %message %exception%n""); appender.ActivateOptions(); log4net.Config.BasicConfigurator.Configure(appender); }
 public static SqlConnection ConnectToDatabase() { var c=new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[""GUKVConnectionString""].ConnectionString); c.Open(); return c; }
 public static long GetNextSequenceValue(string name) { return 100406; }
 public static Control FindControlRecursive(this Control root,string id) { if(root.ID==id)return root; foreach(Control c in root.Controls){var found=c.FindControlRecursive(id);if(found!=null)return found;}return null; }
}
namespace Resources { public static class Strings {
 public static string SignatureUnknownUser {get{return ""?"";}} public static string SignatureUnknownDate {get{return ""?"";}}
 public static string SignatureObjCard {get{return ""Дані внесено користувачем {0}, дата внесення змін: {1}"";}}
} }");
        const string dx = "DevExpress.Web.ASPxHttpHandlerModule, DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a";
        File.WriteAllText(Path.Combine(_host, "Web.config"), "<configuration><connectionStrings><add name=\"GUKVConnectionString\" connectionString=\"" +
            SecurityElement.Escape(_database.TestConnectionString) + "\" /></connectionStrings><system.web><compilation targetFramework=\"4.5.2\" debug=\"true\" />" +
            "<authentication mode=\"Forms\"/><globalization culture=\"uk-UA\" uiCulture=\"uk\" fileEncoding=\"utf-8\"/><customErrors mode=\"Off\"/><httpRuntime targetFramework=\"4.5.2\" /></system.web><system.webServer><validation validateIntegratedModeConfiguration=\"false\"/>" +
            "<modules><add name=\"ASPxHttpHandlerModule\" type=\"" + dx + "\"/></modules><handlers><add name=\"ASPxHttpHandlerModule\" path=\"DX.ashx\" verb=\"GET,POST\" type=\"" + dx + "\"/></handlers></system.webServer></configuration>");
        var listener = new TcpListener(IPAddress.Loopback, 0); listener.Start();
        int port = ((IPEndPoint)listener.LocalEndpoint).Port; listener.Stop();
        _url = "http://localhost:" + port + "/Cards/AssessmentCard.aspx?vid=-100405";
        _server = Process.Start(new ProcessStartInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express", "iisexpress.exe"),
            "/path:\"" + _host + "\" /port:" + port + " /systray:false") {
            UseShellExecute = false, CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true, RedirectStandardError = true
        });
        _server.OutputDataReceived += (s, e) => { }; _server.ErrorDataReceived += (s, e) => { };
        _server.BeginOutputReadLine(); _server.BeginErrorReadLine();
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "chrome", Headless = true });
        Sql(@"
CREATE TABLE organizations(id int,zkpo_code varchar(20),full_name varchar(255));
CREATE TABLE dict_streets(id int,name varchar(255)); CREATE TABLE dict_districts2(id int,name varchar(255));
CREATE TABLE dict_expert(id int,full_name varchar(255));
CREATE TABLE buildings(id int,addr_street_id int,addr_nomer varchar(256),master_building_id int,is_deleted int);
CREATE TABLE dict_expert_obj_type(id int,name varchar(255)); CREATE TABLE dict_expert_valuation_kind(id int,name varchar(255));
CREATE TABLE dict_expert_stan(id int,name varchar(255)); CREATE TABLE dict_expert_rezenz(id int,name varchar(255),is_deleted int);
CREATE TABLE dict_expert_korr(id int,name varchar(255)); CREATE TABLE dict_expert_detail_purpose(id int,name varchar(100));
CREATE TABLE dict_expert_detail_floors(id int,name varchar(100)); CREATE TABLE dict_expert_rezenz_type(id int,name varchar(255));
INSERT dict_expert_korr VALUES(1,'Correspondent One'),(2,'Correspondent Two'),(3,'Correspondent Three');
INSERT dict_expert_rezenz VALUES(1,'Reviewer One',0),(2,'Reviewer Two',0);
INSERT dict_expert_rezenz_type VALUES(1,'Category One'),(2,'Category Two'),(3,'Category Three');
INSERT dict_expert_detail_floors VALUES(1,'1'),(2,'2'); INSERT dict_expert_detail_purpose VALUES(1,'Office');
INSERT dict_streets VALUES(1,'Street One'),(2,'Street Two'); INSERT buildings VALUES(1,1,'3',NULL,0),(2,2,'5',NULL,0);
");
    }

    [OneTimeTearDown]
    public async Task StopHostAndDropTestDatabase()
    {
        if (_browser != null) await _browser.CloseAsync();
        if (_playwright != null) _playwright.Dispose();
        if (_server != null && !_server.HasExited) { _server.Kill(); _server.WaitForExit(10000); }
        if (_server != null) _server.Dispose();
        // The generated config contains only the supplied test connection string; do not retain it.
        if (_host != null && File.Exists(Path.Combine(_host, "Web.config"))) File.Delete(Path.Combine(_host, "Web.config"));
        _database.DropOnlyTheDatabaseCreatedByThisFixture();
    }

    [SetUp]
    public async Task OpenTwoCopies()
    {
        _database.Seed();
        Sql("INSERT expert_input_doc(expert_note_id,doc_date,doc_num,korrespondent_id,rezenz_id) VALUES(-100405,'2026-10-01','001',1,1);" +
            "INSERT expert_output_doc(expert_note_id,doc_date,doc_num,rezenz_type_id) VALUES(-100405,'2026-10-01','001',1);" +
            "INSERT expert_note_detail(expert_note_id,floors,purpose,obj_square,note) VALUES(-100405,'1','Office',250.4,'old detail');" +
            "UPDATE expert_note SET addr_street_id=1,addr_nomer='3'");
        _browserErrors.Clear();
        _context = await _browser.NewContextAsync(new BrowserNewContextOptions { ViewportSize = new ViewportSize { Width = 1440, Height = 1000 } });
        _a = await _context.NewPageAsync(); _b = await _context.NewPageAsync();
        _a.SetDefaultTimeout(60000); _b.SetDefaultTimeout(60000);
        _a.PageError += (s, message) => _browserErrors.Add(message); _b.PageError += (s, message) => _browserErrors.Add(message);
        _a.Dialog += async (s, dialog) => await dialog.AcceptAsync(); _b.Dialog += async (s, dialog) => await dialog.AcceptAsync();
        await _a.GotoAsync(_url); await _b.GotoAsync(_url);
        await _a.WaitForFunctionAsync("typeof AssessmentConcurrency !== 'undefined' && typeof Assessment_obj_square !== 'undefined'");
        await _b.WaitForFunctionAsync("typeof AssessmentConcurrency !== 'undefined' && typeof Assessment_obj_square !== 'undefined'");
    }

    [TearDown]
    public async Task CaptureFailureAndClosePages()
    {
        if (_context == null) return;
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            string name = String.Concat(TestContext.CurrentContext.Test.Name.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
            await _b.ScreenshotAsync(new PageScreenshotOptions { Path = Path.Combine(_host, name + ".png"), FullPage = true });
            File.WriteAllText(Path.Combine(_host, name + ".html"), await _b.ContentAsync());
            TestContext.WriteLine("Browser errors: " + String.Join("; ", _browserErrors));
            TestContext.WriteLine("Window A status: " + await _a.Locator("#assessmentSaveStatus").InnerTextAsync());
            TestContext.WriteLine("Window B status: " + await _b.Locator("#assessmentSaveStatus").InnerTextAsync());
            string log = Path.Combine(_host, "save-errors.log");
            if (File.Exists(log))
                using (var stream = new FileStream(log, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(stream)) TestContext.WriteLine(reader.ReadToEnd());
        }
        await _context.CloseAsync(); _context = null;
        Assert.That(_browserErrors, Is.Empty);
    }

    [Test]
    public async Task ObjectAndNoteEditsInTwoTabsMergeWithoutReloadingInputs()
    {
        await _a.EvaluateAsync("Assessment_obj_square.SetValue(260)"); await SaveMain(_a);
        await _b.EvaluateAsync("CardPageControl.SetActiveTabIndex(4); AssessmentMemo.SetText('second window note')"); await SaveMain(_b);
        Assert.That(await _b.EvaluateAsync<decimal>("Assessment_obj_square.GetValue()"), Is.EqualTo(260m));
        Assert.That(await _b.EvaluateAsync<string>("AssessmentMemo.GetText()"), Is.EqualTo("second window note"));
    }

    [Test]
    public async Task MainConflictCanBeResolvedAndUnrelatedInputIsPreserved()
    {
        await _a.EvaluateAsync("Assessment_obj_square.SetValue(255)"); await SaveMain(_a);
        await _b.EvaluateAsync("Assessment_obj_square.SetValue(260); AssessmentMemo.SetText('unsaved note')"); await SaveMain(_b, true);
        Assert.That(await _b.Locator("#assessmentConflictRows").InnerTextAsync(), Does.Contain("255,00"));
        await _b.ScreenshotAsync(new PageScreenshotOptions { Path = Path.Combine(_host, "conflict-table.png"), FullPage = true });
        await Resolve(_b, "mine");
        Assert.That(await _b.EvaluateAsync<decimal>("Assessment_obj_square.GetValue()"), Is.EqualTo(260m));
        Assert.That(await _b.EvaluateAsync<string>("AssessmentMemo.GetText()"), Is.EqualTo("unsaved note"));
    }

    [TestCase("GridViewAssessmentInputDocs", 1, "korrespondent_id", "2", "3")]
    [TestCase("GridViewAssessmentDetails", 2, "note", "first edit", "second edit")]
    [TestCase("GridViewAssessmentOutputDocs", 3, "rezenz_type_id", "2", "3")]
    public async Task EveryGridDetectsConflictsAndPreservesEditsOnTheNoteTab(string grid, int tab, string field, string first, string second)
    {
        await StartEdit(_a, grid, tab); await StartEdit(_b, grid, tab);
        await _b.EvaluateAsync("AssessmentMemo.SetText('keep this unsaved note')");
        await SetEditor(_a, grid, field, first); await UpdateRow(_a, grid);
        await SetEditor(_b, grid, field, second); await UpdateRow(_b, grid, true);
        await Resolve(_b, "mine"); await WaitGrid(_b, grid);
        Assert.That(await _b.EvaluateAsync<string>("AssessmentMemo.GetText()"), Is.EqualTo("keep this unsaved note"));
        Assert.That(await _b.EvaluateAsync<bool>("name => window[name].IsEditing()", grid), Is.False);
    }

    [Test]
    public async Task IndependentChangesToTheSameInputDocumentMerge()
    {
        const string grid = "GridViewAssessmentInputDocs";
        await StartEdit(_a, grid, 1); await StartEdit(_b, grid, 1);
        await SetEditor(_a, grid, "korrespondent_id", "2"); await UpdateRow(_a, grid);
        await _b.EvaluateAsync("GridViewAssessmentInputDocs.GetEditor('control_date').SetDate(new Date(2026,9,30))");
        await UpdateRow(_b, grid);
        Assert.That(Scalar("SELECT korrespondent_id FROM expert_input_doc"), Is.EqualTo(2));
        Assert.That(Scalar("SELECT control_date FROM expert_input_doc"), Is.EqualTo(new DateTime(2026, 10, 30)));
    }

    [Test]
    public async Task DeletingARowUsesTheVersionActuallyDisplayed()
    {
        const string grid = "GridViewAssessmentDetails";
        await StartEdit(_a, grid, 2);
        await SetEditor(_a, grid, "note", "changed before delete"); await UpdateRow(_a, grid);
        await _b.EvaluateAsync("CardPageControl.SetActiveTabIndex(2); GridViewAssessmentDetails.DeleteRow(0)");
        await WaitGrid(_b, grid);
        Assert.That(await _b.Locator("#assessmentConflictDialog").IsVisibleAsync(), Is.True);
        await Resolve(_b, "current"); await WaitGrid(_b, grid);
        Assert.That(Scalar("SELECT COUNT(*) FROM expert_note_detail"), Is.EqualTo(1));
    }

    [Test]
    public async Task DuplicateNewDocumentsOfferAChoiceAndNeverCreateAnotherRow()
    {
        const string grid = "GridViewAssessmentInputDocs";
        foreach (var page in new[] { _a, _b })
        {
            await page.EvaluateAsync("CardPageControl.SetActiveTabIndex(1); GridViewAssessmentInputDocs.AddNewRow()");
            await WaitGrid(page, grid);
            await page.EvaluateAsync("GridViewAssessmentInputDocs.GetEditor('doc_date').SetDate(new Date(2026,9,26))");
            await SetEditor(page, grid, "doc_num", "NEW");
            await SetEditor(page, grid, "korrespondent_id", page == _a ? "1" : "2");
        }
        await UpdateRow(_a, grid);
        await UpdateRow(_b, grid, true);
        Assert.That(await _b.Locator("#assessmentConflictRows").InnerTextAsync(), Does.Contain("Correspondent One").And.Contain("Correspondent Two"));
        await Resolve(_b, "mine"); await WaitGrid(_b, grid);
        Assert.That(Scalar("SELECT COUNT(*) FROM expert_input_doc WHERE doc_num='NEW'"), Is.EqualTo(1));
        Assert.That(Scalar("SELECT korrespondent_id FROM expert_input_doc WHERE doc_num='NEW'"), Is.EqualTo(2));
    }

    [Test]
    public async Task DateAndNumberAreResolvedTogetherAndCancelKeepsTheEditorValues()
    {
        const string grid = "GridViewAssessmentInputDocs";
        await StartEdit(_a, grid, 1); await StartEdit(_b, grid, 1);
        await _a.EvaluateAsync("GridViewAssessmentInputDocs.GetEditor('doc_date').SetDate(new Date(2026,9,2))");
        await UpdateRow(_a, grid);
        await SetEditor(_b, grid, "doc_num", "002"); await UpdateRow(_b, grid, true);
        Assert.That(await _b.Locator("#assessmentConflictRows").InnerTextAsync(), Does.Contain("Дата документа + Номер документа"));
        await _b.EvaluateAsync("AssessmentConcurrency.close()");
        Assert.That(await _b.EvaluateAsync<string>("GridViewAssessmentInputDocs.GetEditor('doc_num').GetValue()"), Is.EqualTo("002"));
        await UpdateRow(_b, grid, true); await Resolve(_b, "mine"); await WaitGrid(_b, grid);
        Assert.That(Scalar("SELECT doc_date FROM expert_input_doc"), Is.EqualTo(new DateTime(2026, 10, 1)));
        Assert.That(Scalar("SELECT doc_num FROM expert_input_doc"), Is.EqualTo("002"));
    }

    [Test]
    public async Task ANoOpDoesNotChangeTheSignature()
    {
        await SaveMain(_a);
        Assert.That(await _a.Locator("#assessmentSaveStatus").InnerTextAsync(), Is.EqualTo("Немає змін для збереження."));
        Assert.That(Scalar("SELECT modified_by FROM expert_note WHERE id=-100405"), Is.EqualTo("original"));
    }

    [Test]
    public async Task MergingAnotherUsersAddressAlsoRefreshesTheDependentBuildingList()
    {
        await _a.EvaluateAsync("addr_street_id.SetValue(2); AssessmentConcurrency.streetChanged(addr_street_id)");
        await _a.WaitForFunctionAsync("!ComboBuilding.InCallback() && document.getElementById('assessmentBusy').style.display === 'none'");
        await _a.EvaluateAsync("ComboBuilding.SetValue('5')"); await SaveMain(_a);
        await _b.EvaluateAsync("AssessmentMemo.SetText('independent note')"); await SaveMain(_b);
        Assert.That(await _b.EvaluateAsync<int>("addr_street_id.GetValue()"), Is.EqualTo(2));
        Assert.That(await _b.EvaluateAsync<string>("ComboBuilding.GetText()"), Is.EqualTo("5"));
        Assert.That(await _b.EvaluateAsync<bool>("!!ComboBuilding.FindItemByValue('5')"), Is.True);
        Assert.That(await _b.EvaluateAsync<bool>("!!ComboBuilding.FindItemByValue('3')"), Is.False);
    }

    private static async Task SaveMain(IPage page, bool conflict = false)
    {
        await page.EvaluateAsync("document.getElementById('assessmentSaveStatus').textContent=''; AssessmentConcurrency.save()");
        await page.WaitForFunctionAsync("document.getElementById('assessmentSaveStatus').textContent || document.getElementById('assessmentConflictDialog').style.display === 'block'");
        await page.WaitForFunctionAsync("document.getElementById('assessmentBusy').style.display === 'none'");
        Assert.That(await page.Locator("#assessmentConflictDialog").IsVisibleAsync(), Is.EqualTo(conflict));
        Assert.That(await page.Locator("#assessmentSaveStatus").GetAttributeAsync("class"), Does.Not.Contain("assessment-error"));
    }

    private static async Task StartEdit(IPage page, string grid, int tab)
    {
        await page.EvaluateAsync("args => { CardPageControl.SetActiveTabIndex(args.tab); window[args.grid].StartEditRow(0); }", new { grid, tab });
        await WaitGrid(page, grid);
        Assert.That(await page.EvaluateAsync<bool>("name => !!AssessmentEditState.Get(name+':edit')", grid), Is.True);
    }

    private static Task SetEditor(IPage page, string grid, string field, string value)
    {
        return page.EvaluateAsync("args => window[args.grid].GetEditor(args.field).SetValue(args.value)", new { grid, field, value });
    }

    private static async Task UpdateRow(IPage page, string grid, bool conflict = false)
    {
        await page.EvaluateAsync("name => { document.getElementById('assessmentSaveStatus').textContent=''; window[name].UpdateEdit(); }", grid);
        await WaitGrid(page, grid);
        Assert.That(await page.Locator("#assessmentSaveStatus").GetAttributeAsync("class"), Does.Not.Contain("assessment-error"), await page.Locator("#assessmentSaveStatus").InnerTextAsync());
        Assert.That(await page.Locator("#assessmentConflictDialog").IsVisibleAsync(), Is.EqualTo(conflict));
    }

    private static async Task WaitGrid(IPage page, string grid)
    {
        await page.WaitForFunctionAsync("name => !window[name].InCallback() && document.getElementById('assessmentBusy').style.display === 'none'", grid);
    }

    private static async Task Resolve(IPage page, string choice)
    {
        foreach (var select in await page.Locator("#assessmentConflictRows select").AllAsync()) await select.SelectOptionAsync(choice);
        await page.Locator("#assessmentResolveButton").ClickAsync();
        await page.WaitForFunctionAsync("document.getElementById('assessmentConflictDialog').style.display === 'none'");
        Assert.That(await page.Locator("#assessmentSaveStatus").GetAttributeAsync("class"), Does.Not.Contain("assessment-error"));
    }

    private void Sql(string text)
    {
        using (var connection = new SqlConnection(_database.TestConnectionString))
        using (var command = new SqlCommand(text, connection)) { connection.Open(); command.ExecuteNonQuery(); }
    }

    private object Scalar(string text)
    {
        using (var connection = new SqlConnection(_database.TestConnectionString))
        using (var command = new SqlCommand(text, connection)) { connection.Open(); return command.ExecuteScalar(); }
    }
}
