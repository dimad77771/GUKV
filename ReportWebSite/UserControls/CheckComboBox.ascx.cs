using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses;

public class CheckComboBoxClientSideEvents : ClientSideEvents
{
    public string SelectionChanged { get; set; }
}

[ToolboxItem(true)]
[ParseChildren(true)]
public partial class UserControls_CheckComboBox : System.Web.UI.UserControl
{
    public UserControls_CheckComboBox()
    {
        this.ClientSideEvents = new CheckComboBoxClientSideEvents();
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        Page.PreLoad += OnPreLoad;
    }

    protected void OnPreLoad(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            Value = string.Join(",", ASPxListBox1.SelectedValues
                .Cast<object>()
                .Where(x => x != null)
                .Select(x => x.ToString())
                );
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Width != default(Unit))
            ASPxDropDownEdit1.Width = this.Width;
        if (this.ReadOnly)
            ASPxDropDownEdit1.ClientEnabled = false;
        if (this.DropDownHeight != default(Unit))
            ASPxListBox1.Height = this.DropDownHeight;

        ASPxButton1.ClientSideEvents.Click =
            string.Format("function (s, e) {{ {0}.HideDropDown(); }}", ASPxDropDownEdit1.ClientID);

        ASPxListBox1.TextField = this.TextField;
        ASPxListBox1.ValueField = this.ValueField;
        ASPxListBox1.DataSourceID = this.DataSourceID;

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(this.Value))
            {
                ASPxListBox1.DataBindItems();

                foreach (string value in this.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    ListEditItem item = ASPxListBox1.Items.FindByValue(value);
                    if (item != null)
                        item.Selected = true;
                }

                ASPxDropDownEdit1.Text =
                    string.Join(ValueSeparator, ASPxListBox1.SelectedItems.Cast<ListEditItem>().Select(x => x.Text));
            }
        }
    }

    public ASPxListBox ASPxListBox1 { get { return (ASPxListBox)ASPxDropDownEdit1.FindControl("ASPxListBox1"); } }
    protected ASPxButton ASPxButton1 { get { return (ASPxButton)ASPxDropDownEdit1.FindControl("ASPxButton1"); } }

    [Description("Sets the control to 'read-only' state, where it displays the currently selected value, but doesn't allow to change it")]
    public bool ReadOnly { get; set; }

    [Description("The width of the element")]
    public Unit Width { get; set; }

    [Description("The height of the drop-down portion of the control (the selection list)")]
    public Unit DropDownHeight { get; set; }

    [Description("The ID of the DataSource used to populate the drop-down list, and resolve the list of chosen Values")]
    public string DataSourceID { get; set; }

    [Description("The field from the DataSource that contains the text to be presented to the end user")]
    public string TextField { get; set; }

    [Description("The field from the DataSource that contains the value used for storage")]
    public string ValueField { get; set; }

    [Description("Comma-separated list of values correponding to the ValueField")]
    public string Value { get; set; }

    private string _valueSeparator;
    [DefaultValue(";")]
    [Description("The character to be used to separate multiple entries in the Value property")]
    public string ValueSeparator { get { return string.IsNullOrEmpty(_valueSeparator) ? ";" : _valueSeparator.Substring(0, 1); } set { _valueSeparator = value; } }

    [Description("The ID of the control to be used on the client side with the control API")]
    public string ClientInstanceName { get; set; }

    [MergableProperty(false)]
    [AutoFormatDisable]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public CheckComboBoxClientSideEvents ClientSideEvents { get; set; }

    protected void ASPxListBox1_Callback(object sender, CallbackEventArgsBase e)
    {
    }
}
