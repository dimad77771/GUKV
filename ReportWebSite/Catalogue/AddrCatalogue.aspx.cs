using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.Web;
using GUKV;

public partial class Catalogue_AddrCatalogue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();

        bool userIsReportManager = Roles.IsUserInRole(Utils.ReportManagerRole);

        ButtonShowFoldersPopup1.Visible = userIsReportManager;

        // Check if this is first loading of this page
        object uniqueKey = ViewState["PageUniqueKey"];
        bool isFirstLoading = (uniqueKey == null);

        // Generate a unique key for this instance of the page
        GetPageUniqueKey();

        // If user-defined report is displayed, switch to the proper page of the Page control
        if (isFirstLoading)
        {
            string reportTitle = "";
            string preFilter = "";
            var fixedColumns = string.Empty;

            if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridIDCatalogue_Buildings)
            {
                LabelReportTitle1.Text = reportTitle;
                Utils.RestoreFixedColumns(PrimaryGridView, fixedColumns);
            }
            else
            {
                // Restore grid fixed columns
                Utils.RestoreFixedColumns(PrimaryGridView);
            }
        }

        // A filter restored from ViewState may predate on-the-fly address grouping.
        NormalizeCurrentAddressNumberFilter();

        // Bind data to the grid dynamically
        this.ProcessGridDataFetch(ViewState, PrimaryGridView);

        // Enable advanced header filter for all grid columns
        Utils.AdjustColumnsVisibleInFilter(PrimaryGridView);
    }

    protected void ASPxButton_AllBuildings_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewAllBuildingsExporter, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_AllBuildings_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(GridViewAllBuildingsExporter, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_AllBuildings_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(GridViewAllBuildingsExporter, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void GridViewAllBuildings_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;

        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 35);

        if (!ApplyAddressPickerFilter(param))
        {
            Utils.ProcessDataGridSaveLayoutCallback(
                param, PrimaryGridView, Utils.GridIDCatalogue_Buildings, "");
        }
    }

    protected void GridViewAllBuildings_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewAllBuildings_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        if (e.Kind == GridViewAutoFilterEventKind.CreateCriteria &&
            e.Column != null && e.Column.FieldName == "addr_nomer" &&
            TryRewriteAddressNumberPredicate(e.Criteria))
        {
            return;
        }

        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void GridViewAllBuildings_CustomColumnSort(object sender,
        DevExpress.Web.CustomColumnSortEventArgs e)
    {
        Utils.ProcessGridSortByBuildingNumber(e);
    }

    private bool ApplyAddressPickerFilter(string callbackParameter)
    {
        const string prefix = "building:";

        if (!callbackParameter.StartsWith(prefix, StringComparison.Ordinal))
        {
            return false;
        }

        string streetAndBuilding = callbackParameter.Substring(prefix.Length);
        int separator = streetAndBuilding.IndexOf('$');

        if (separator > 0 && separator < streetAndBuilding.Length - 1)
        {
            string street = EscapeFilterValue(streetAndBuilding.Substring(0, separator));
            string building = streetAndBuilding.Substring(separator + 1);
            string normalized = CatalogueAddressDataSource.NormalizeNumberForSearch(building);

            if (normalized.Length > 0)
            {
                PrimaryGridView.FilterExpression =
                    "StartsWith([street_full_name], '" + street +
                    "') And StartsWith([addr_nomer_normalized], '" +
                    EscapeFilterValue(normalized) + "')";
            }
            else
            {
                PrimaryGridView.FilterExpression =
                    "StartsWith([street_full_name], '" + street +
                    "') And StartsWith([addr_nomer], '" +
                    EscapeFilterValue(building) + "')";
            }
        }

        return true;
    }

    protected override void OnPreRender(EventArgs e)
    {
        // Filter Builder and a saved client layout can install a complete expression
        // after Page_Load and therefore bypass ProcessColumnAutoFilter.
        if (NormalizeCurrentAddressNumberFilter())
        {
            PrimaryGridView.DataBind();
        }

        base.OnPreRender(e);
    }

    private bool NormalizeCurrentAddressNumberFilter()
    {
        string expression = PrimaryGridView.FilterExpression;
        if (string.IsNullOrWhiteSpace(expression))
        {
            return false;
        }

        CriteriaOperator criteria;
        try
        {
            criteria = CriteriaOperator.Parse(expression);
        }
        catch
        {
            // Preserve the legacy behavior for an unknown/custom expression format.
            return false;
        }

        if (!NormalizeAddressNumberPredicates(criteria))
        {
            return false;
        }

        PrimaryGridView.FilterExpression = criteria.ToString();
        return true;
    }

    private static bool NormalizeAddressNumberPredicates(CriteriaOperator criteria)
    {
        if (ReferenceEquals(criteria, null))
        {
            return false;
        }

        GroupOperator group = criteria as GroupOperator;
        if (!ReferenceEquals(group, null))
        {
            bool changed = false;
            foreach (CriteriaOperator operand in group.Operands)
            {
                changed = NormalizeAddressNumberPredicates(operand) || changed;
            }

            return changed;
        }

        UnaryOperator unary = criteria as UnaryOperator;
        if (!ReferenceEquals(unary, null))
        {
            return NormalizeAddressNumberPredicates(unary.Operand);
        }

        return TryRewriteAddressNumberPredicate(criteria);
    }

    private static bool TryRewriteAddressNumberPredicate(CriteriaOperator criteria)
    {
        FunctionOperator function = criteria as FunctionOperator;
        if (!ReferenceEquals(function, null))
        {
            if (function.OperatorType != FunctionOperatorType.StartsWith &&
                function.OperatorType != FunctionOperatorType.EndsWith &&
                function.OperatorType != FunctionOperatorType.Contains)
            {
                return false;
            }

            if (function.Operands.Count != 2)
            {
                return false;
            }

            return TryRewriteSimplePredicate(
                function.Operands[0] as OperandProperty,
                new[] { function.Operands[1] as OperandValue },
                false);
        }

        BinaryOperator binary = criteria as BinaryOperator;
        if (!ReferenceEquals(binary, null))
        {
            if (binary.OperatorType != BinaryOperatorType.Equal &&
                binary.OperatorType != BinaryOperatorType.NotEqual &&
                binary.OperatorType != BinaryOperatorType.Like)
            {
                return false;
            }

            OperandProperty property = binary.LeftOperand as OperandProperty;
            OperandValue value = binary.RightOperand as OperandValue;

            // Equal/NotEqual can be written with the operands reversed. Like cannot.
            if (ReferenceEquals(property, null) &&
                binary.OperatorType != BinaryOperatorType.Like)
            {
                property = binary.RightOperand as OperandProperty;
                value = binary.LeftOperand as OperandValue;
            }

            return TryRewriteSimplePredicate(
                property,
                new[] { value },
                binary.OperatorType == BinaryOperatorType.Like);
        }

        InOperator inOperator = criteria as InOperator;
        if (!ReferenceEquals(inOperator, null))
        {
            return TryRewriteSimplePredicate(
                inOperator.LeftOperand as OperandProperty,
                inOperator.Operands.Cast<CriteriaOperator>()
                    .Select(operand => operand as OperandValue)
                    .ToArray(),
                false);
        }

        // Range comparisons and complex/custom functions preserve their legacy
        // semantics; canonical string ordering is not equivalent to address ordering.
        return false;
    }

    private static bool TryRewriteSimplePredicate(OperandProperty property,
        IEnumerable<OperandValue> values, bool isLikePattern)
    {
        if (ReferenceEquals(property, null) ||
            property.PropertyName != "addr_nomer" || values == null)
        {
            return false;
        }

        List<OperandValue> valueList = values.ToList();
        if (valueList.Count == 0 ||
            valueList.Any(value => ReferenceEquals(value, null)))
        {
            return false;
        }

        List<string> normalizedValues = new List<string>();
        foreach (OperandValue value in valueList)
        {
            string text = value.Value as string;
            if (text == null)
            {
                return false;
            }

            string normalized = isLikePattern
                ? NormalizeLikePattern(text)
                : CatalogueAddressDataSource.NormalizeNumberForSearch(text);

            if (string.IsNullOrEmpty(normalized))
            {
                return false;
            }

            normalizedValues.Add(normalized);
        }

        property.PropertyName = "addr_nomer_normalized";
        for (int index = 0; index < valueList.Count; index++)
        {
            valueList[index].Value = normalizedValues[index];
        }

        return true;
    }

    private static string NormalizeLikePattern(string pattern)
    {
        if (string.IsNullOrEmpty(pattern) || pattern.IndexOf('[') >= 0 ||
            pattern.IndexOf(']') >= 0 || pattern.IndexOf('\\') >= 0)
        {
            return null;
        }

        StringBuilder result = new StringBuilder();
        StringBuilder literal = new StringBuilder();
        bool hasNormalizedLiteral = false;

        foreach (char character in pattern)
        {
            if (character == '%' || character == '_' ||
                character == '*' || character == '?')
            {
                bool hadLiteral = literal.Length > 0;
                if (!AppendNormalizedLiteral(result, literal))
                {
                    return null;
                }

                hasNormalizedLiteral |= hadLiteral;

                result.Append(character);
            }
            else
            {
                literal.Append(character);
            }
        }

        bool hadTrailingLiteral = literal.Length > 0;
        if (!AppendNormalizedLiteral(result, literal))
        {
            return null;
        }

        hasNormalizedLiteral |= hadTrailingLiteral;
        return hasNormalizedLiteral ? result.ToString() : null;
    }

    private static bool AppendNormalizedLiteral(StringBuilder result, StringBuilder literal)
    {
        if (literal.Length == 0)
        {
            return true;
        }

        string normalized = CatalogueAddressDataSource.NormalizeNumberForSearch(literal.ToString());
        if (normalized.Length == 0)
        {
            return false;
        }

        result.Append(normalized);
        literal.Clear();
        return true;
    }

    private static string EscapeFilterValue(string value)
    {
        return (value ?? string.Empty).Replace("'", "''");
    }

    protected void SaveFixxedColumns_Click(object sender, EventArgs e)
    {

    }

    protected string GetPageUniqueKey()
    {
        object key = ViewState["PageUniqueKey"];

        if (key is string)
        {
            return (string)key;
        }

        // Generate unique key
        Guid guid = Guid.NewGuid();

        string str = guid.ToString();

        ViewState["PageUniqueKey"] = str;

        return str;
    }
}
