#if DXCommon
public static class AssemblyInfo {
#else
namespace DevExpress.Internal {
	internal static class AssemblyInfo {
#endif
    public const string AssemblyCopyright = "Copyright (c) 2000-2012 Developer Express Inc.";
    public const string AssemblyCompany = "Developer Express Inc.";

    public const int VersionId = 121;
    public const string VersionShort = "12.1";
#if !SILVERLIGHT
    public const string Version = VersionShort + ".0.0";
    public const string XpfPrefix = "WPF";
#else
    public const string Version = VersionShort + ".0.5"; //SL
    public const string XpfPrefix = "Silverlight";
#endif
    public const string FileVersion = Version;
    public const string MarketingVersion = "v2012 vol 1";
    public const string VirtDirSuffix = "_v12_1";

    public const string SatelliteContractVersion = VersionShort + ".0.0";
    public const string VSuffixWithoutSeparator = "v" + VersionShort;
    public const string VSuffix = "." + VSuffixWithoutSeparator;
    public const string VSuffixDesign = VSuffix + ".Design";
    public const string VSuffixExport = VSuffix + ".Export";
    public const string VSuffixLinq = VSuffix + ".Linq";
    public const string SRAssemblyAgScheduler = "DevExpress.Xpf.Scheduler" + VSuffix;
    public const string SRAssemblyAssemblyLoader = "DevExpress.Xpf.AssemblyLoader" + VSuffix;
    public const string SRAssemblyXpfRichEdit = "DevExpress.Xpf.RichEdit" + VSuffix;
    public const string SRAssemblyXpfScheduler = "DevExpress.Xpf.Scheduler" + VSuffix;
    public const string SRAssemblyScheduleriCalendarExchange = "DevExpress.XtraScheduler" + VSuffix + ".iCalendarExchange";
    public const string SRAssemblyXpfRichEditExtensions = "DevExpress.Xpf.RichEdit" + VSuffix + ".Extensions";
    public const string SRAssemblyXpfPrintingService = "DevExpress.Xpf.Printing" + VSuffix + ".Service";
    public const string SRAssemblyXpfPrinting = "DevExpress.Xpf.Printing" + VSuffix;
    public const string SRAssemblyXpfPrintingCore = "DevExpress.Xpf.Printing" + VSuffix + ".Core";
    public const string SRAssemblyXpfReportDesigner = "DevExpress.Xpf.ReportDesigner" + VSuffix;
    public const string SRAssemblyXpfCore = "DevExpress.Xpf.Core" + VSuffix;
    public const string SRAssemblyXpfCoreExtensions = "DevExpress.Xpf.Core" + VSuffix + ".Extensions";
    public const string SRAssemblyXpfDemoBase = "DevExpress.Xpf.DemoBase" + VSuffix;
    public const string SRAssemblyXpfGrid = "DevExpress.Xpf.Grid" + VSuffix;
    public const string SRAssemblyXpfGridCore = "DevExpress.Xpf.Grid" + VSuffix + ".Core";
    public const string SRAssemblyXpfDocking = "DevExpress.Xpf.Docking" + VSuffix;
    public const string SRAssemblyXpfLayoutCore = "DevExpress.Xpf.Layout" + VSuffix + ".Core";
    public const string SRAssemblyDXCharts = "DevExpress.Xpf.Charts" + VSuffix;
    public const string SRAssemblyDXGauges = "DevExpress.Xpf.Gauges" + VSuffix;
    public const string SRAssemblyDXMap = "DevExpress.Xpf.Map" + VSuffix;

    public const string SRAssemblyData = "DevExpress.Data" + VSuffix;
    public const string SRAssemblyDemoDataCore = "DevExpress.DemoData" + VSuffix + ".Core";
    public const string SRAssemblyPrintingCore = "DevExpress.Printing" + VSuffix + ".Core";
    public const string SRAssemblyRichEditCore = "DevExpress.RichEdit" + VSuffix + ".Core";
    public const string SRAssemblyReports = "DevExpress.XtraReports" + VSuffix;
    public const string SRAssemblyPrintingDesign = "DevExpress.XtraPrinting" + VSuffixDesign;

    public const string SRDocumentationLink = "http://documentation.devexpress.com/";
    public const string InstallationRegistryKeyBase = "SOFTWARE\\DevExpress\\DXperience\\";
    public const string InstallationRegistryKey = InstallationRegistryKeyBase + VSuffixWithoutSeparator;
    public const string InstallationRegistryRootPathValueName = "RootDirectory";
    public const string SRAssemblyXpfPrefix = "DevExpress.Xpf";
    public const string ThemePrefixWithoutSeparator = "Themes";
    public const string ThemePrefix = "." + ThemePrefixWithoutSeparator + ".";
#if !SILVERLIGHT
    public const string
        SRAssemblyMVC = "DevExpress.Web.Mvc" + VSuffix,
        SRAssemblyExpressAppWeb = "DevExpress.ExpressApp.Web" + VSuffix,
        SRAssemblyASPxThemes = "DevExpress.Web.ASPxThemes" + VSuffix,
        SRAssemblyASPxGridView = "DevExpress.Web.ASPxGridView" + VSuffix,
        SRAssemblyASPxGridViewExport = "DevExpress.Web.ASPxGridView" + VSuffixExport,
        SRAssemblyASPxPivotGrid = "DevExpress.Web.ASPxPivotGrid" + VSuffix,
        SRAssemblyASPxPivotGridExport = "DevExpress.Web.ASPxPivotGrid" + VSuffixExport,
        SRAssemblyBonusSkins = "DevExpress.BonusSkins" + VSuffix,
        //SRAssemblyOfficeSkins = "DevExpress.OfficeSkins" + VSuffix,
        SRAssemblyDesign = "DevExpress.Design" + VSuffix,
        SRAssemblyDataLinq = "DevExpress.Data" + VSuffixLinq,
        SRAssemblyUtils = "DevExpress.Utils" + VSuffix,
        SRAssemblyParser = "DevExpress.Parser" + VSuffix,
        SRAssemblyPrinting = "DevExpress.XtraPrinting" + VSuffix,
        SRAssemblyEditors = "DevExpress.XtraEditors" + VSuffix,
        SRAssemblyEditorsDesign = "DevExpress.XtraEditors" + VSuffixDesign,
        SRAssemblyNavBar = "DevExpress.XtraNavBar" + VSuffix,
        SRAssemblyNavBarDesign = "DevExpress.XtraNavBar" + VSuffixDesign,
        SRAssemblyBars = "DevExpress.XtraBars" + VSuffix,
        SRAssemblyBarsDesign = "DevExpress.XtraBars" + VSuffixDesign,
        SRAssemblyGrid = "DevExpress.XtraGrid" + VSuffix,
        SRAssemblyGaugesCore = "DevExpress.XtraGauges" + VSuffix + ".Core",
        SRAssemblyGaugesPresets = "DevExpress.XtraGauges" + VSuffix + ".Presets",
        SRAssemblyGaugesWin = "DevExpress.XtraGauges" + VSuffix + ".Win",
        SRAssemblyASPxGauges = "DevExpress.Web.ASPxGauges" + VSuffix,
        SRAssemblyGaugesDesignWin = "DevExpress.XtraGauges" + VSuffixDesign + ".Win",
        SRAssemblyGridDesign = "DevExpress.XtraGrid" + VSuffixDesign,
        SRAssemblyPivotGrid = "DevExpress.XtraPivotGrid" + VSuffix,
        SRAssemblyPivotGridCore = "DevExpress.PivotGrid" + VSuffix + ".Core",
        SRAssemblyPivotGridDesign = "DevExpress.XtraPivotGrid" + VSuffixDesign,
        SRAssemblyTreeList = "DevExpress.XtraTreeList" + VSuffix,
        SRAssemblyTreeListDesign = "DevExpress.XtraTreeList" + VSuffixDesign,
        SRAssemblyVertGrid = "DevExpress.XtraVerticalGrid" + VSuffix,
        SRAssemblyVertGridDesign = "DevExpress.XtraVerticalGrid" + VSuffixDesign,
        SRAssemblyReportsService = "DevExpress.XtraReports" + VSuffix + ".Service",
        SRAssemblyReportsDesign = "DevExpress.XtraReports" + VSuffixDesign,
        SRAssemblyReportsImport = "DevExpress.XtraReports" + VSuffix + ".Import",
        SRAssemblyReportsWeb = "DevExpress.XtraReports" + VSuffix + ".Web",
        SRAssemblyReportsDesigner = "DevExpress.Reports" + VSuffix + ".Designer",
        SRAssemblyReportsExtensions = "DevExpress.XtraReports" + VSuffix + ".Extensions",
        SRAssemblyReportsDesignDelphi8 = "DevExpress.XtraReports" + VSuffix + ".Design.Delphi8",
        SRAssemblyReportsDesignDelphi9 = "DevExpress.XtraReports" + VSuffix + ".Design.Delphi9",
        SRAssemblyReportsDesignDelphi10 = "DevExpress.XtraReports" + VSuffix + ".Design.Delphi10",
        SRAssemblyReportServerDesigner = "DevExpress.ReportServer" + VSuffix + ".Designer.Core",
        SRAssemblyRichEdit = "DevExpress.XtraRichEdit" + VSuffix,
        SRAssemblyRichEditDesign = "DevExpress.XtraRichEdit" + VSuffixDesign,
        SRAssemblyRichEditExtensions = "DevExpress.XtraRichEdit" + VSuffix + ".Extensions",
        SRAssemblyRichEditPrinting = "DevExpress.XtraRichEdit" + VSuffix + ".Printing",
        SRAssemblyScheduler = "DevExpress.XtraScheduler" + VSuffix,
        SRAssemblySchedulerCore = "DevExpress.XtraScheduler" + VSuffix + ".Core",
        SRAssemblySchedulerDesign = "DevExpress.XtraScheduler" + VSuffixDesign,
        SRAssemblySchedulerWeb = "DevExpress.Web.ASPxScheduler" + VSuffix,
        SRAssemblySchedulerWebDesign = "DevExpress.Web.ASPxScheduler" + VSuffixDesign,
        SRAssemblySchedulerOutlookExchange = "DevExpress.XtraScheduler" + VSuffix + ".OutlookExchange",
        SRAssemblySchedulerVCalendarExchange = "DevExpress.XtraScheduler" + VSuffix + ".VCalendarExchange",
        SRAssemblySchedulerExtensions = "DevExpress.XtraScheduler" + VSuffix + ".Extensions",
        SRAssemblySchedulerReporting = "DevExpress.XtraScheduler" + VSuffix + ".Reporting",
        SRAssemblySchedulerReportingExtensions = "DevExpress.XtraScheduler" + VSuffix + ".Reporting.Extensions",
        SRAssemblyChartsCore = "DevExpress.Charts" + VSuffix + ".Core",
        SRAssemblyCharts = "DevExpress.XtraCharts" + VSuffix,
        SRAssemblyChartsExtensions = "DevExpress.XtraCharts" + VSuffix + ".Extensions",
        SRAssemblyChartsDesign = "DevExpress.XtraCharts" + VSuffixDesign,
        SRAssemblyChartsWebDesign = "DevExpress.XtraCharts" + VSuffix + ".Web.Design",
        SRAssemblyChartsUI = "DevExpress.XtraCharts" + VSuffix + ".UI",
        SRAssemblyChartsWeb = "DevExpress.XtraCharts" + VSuffix + ".Web",
        SRAssemblyWizard = "DevExpress.XtraWizard" + VSuffix,
        SRAssemblyWizardDesign = "DevExpress.XtraWizard" + VSuffixDesign,
        SRAssemblyXpo = "DevExpress.Xpo" + VSuffix,
        SRAssemblyXpoDesign = "DevExpress.Xpo" + VSuffixDesign,        
        SRAssemblyLayoutControl = "DevExpress.XtraLayout" + VSuffix,
        SRAssemblyLayoutControlDesign = "DevExpress.XtraLayout" + VSuffixDesign,
        SRAssemblySpellCheckerCore = "DevExpress.SpellChecker" + VSuffix + ".Core",
        SRAssemblySpellChecker = "DevExpress.XtraSpellChecker" + VSuffix,
        SRAssemblySpellCheckerDesign = "DevExpress.XtraSpellChecker" + VSuffixDesign,
        SRAssemblySpellCheckerWeb = "DevExpress.Web.ASPxSpellChecker" + VSuffix,
        SRAssemblyWeb = "DevExpress.Web" + VSuffix,
        SRAssemblyWebLinq = "DevExpress.Web" + VSuffixLinq,
        SRAssemblyHtmlEditorWeb = "DevExpress.Web.ASPxHtmlEditor" + VSuffix,
        SRAssemblyEditorsWeb = "DevExpress.Web.ASPxEditors" + VSuffix,
        SRAssemblyTreeListWeb = "DevExpress.Web.ASPxTreeList" + VSuffix,
        SRAssemblyTreeListWebExport = "DevExpress.Web.ASPxTreeList" + VSuffixExport,
        SRAssemblyDXPivotGrid = "DevExpress.Xpf.PivotGrid" + VSuffix,
        SRAssemblyDXThemeEditorUIWithoutVSuffix = "DevExpress.Xpf.ThemeEditor",
        SRAssemblyDXThemeEditorUI = SRAssemblyDXThemeEditorUIWithoutVSuffix + VSuffix,
        SRAssemblyUtilsUI = "DevExpress.Utils" + VSuffix + ".UI",
        SRAssemblyDashboardCore = "DevExpress.Dashboard" + VSuffix + ".Core",
        SRAssemblyDashboard = "DevExpress.DashboardWin" + VSuffix,
        SRAssemblyDashboardDesign = "DevExpress.DashboardWin" + VSuffixDesign;
#endif
    public const string
        DXTabNameComponents = "Components",
        DXTabNameNavigationAndLayout = "Navigation & Layout",
        DXTabNameReporting = "Reporting",
        DXTabNameReportControls = "Report Controls",
        DXTabNameData = "Data",
        DXTabNameVisualization = "Visualization",
        DXTabNameScheduling = "Scheduling",
        DXTabNameSchedulerReporting = "Scheduler Reporting",
        DXTabNameRichEdit = "Rich Text Editor",
        DXTabNameCommon = "Common Controls",
        DXTabNameLayoutControl = "Layout Control",
        DXTabPrefix = "DX." + VersionShort + ": ",
        DXTabComponents = DXTabPrefix + DXTabNameComponents,
        DXTabNavigation = DXTabPrefix + DXTabNameNavigationAndLayout,
        DXTabReporting = DXTabPrefix + DXTabNameReporting,
        DXTabReportControls = DXTabPrefix + DXTabNameReportControls,
        DXTabData = DXTabPrefix + DXTabNameData,
        DXTabVisualization = DXTabPrefix + DXTabNameVisualization,
        DXTabScheduling = DXTabPrefix + DXTabNameScheduling,
        DXTabSchedulerReporting = DXTabPrefix + DXTabNameSchedulerReporting,
        DXTabRichEdit = DXTabPrefix + DXTabNameRichEdit,
        DXTabCommon = DXTabPrefix + DXTabNameCommon,
        DXTabNameXPOProfiler = "XPO " + VersionShort + " Profiler";

    public const string
        DXTabWpfNavigation = "DX." + VersionShort + "." + XpfPrefix + ": Navigation & Layout",
        DXTabWpfReporting = "DX." + VersionShort + "." + XpfPrefix + ": Reporting",
        DXTabWpfData = "DX." + VersionShort + "." + XpfPrefix + ": Data",
        DXTabWpfVisualization = "DX." + VersionShort + "." + XpfPrefix + ": Visualization",
        DXTabWpfCommon = "DX." + VersionShort + "." + XpfPrefix + ": Common Controls",
        DXTabWpfScheduling = "DX." + VersionShort + "." + XpfPrefix + ": Scheduling",
        DXTabWpfRichEdit = "DX." + VersionShort + "." + XpfPrefix + ": Rich Text Editor";
}
#if !DXCommon
}
#endif
