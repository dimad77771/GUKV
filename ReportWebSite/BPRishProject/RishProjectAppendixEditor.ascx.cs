using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using System.IO;
using DevExpress.Web;

public partial class RishProjectAppendixEditor : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RishProjectAppendixItem appendix = Page.GetEditedTreeItem<RishProjectAppendixItem>();
        if (appendix == null)
            return;

        if (!this.IsInitialized)
        {
            EditAppendixName.Text = appendix.name;

            MemoAppendixIntro.Html = appendix.introText.Length > 0 ? appendix.introText : "<p style=\"font-family: Times New Roman; font-size: 14pt;\">&nbsp;</p>";
            MemoAppendixOutro.Html = appendix.outroText.Length > 0 ? appendix.outroText : "<p style=\"font-family: Times New Roman; font-size: 14pt;\">&nbsp;</p>";

            EnableContolsDependentOnFileUpload(appendix.useExternalDocument);

            if (!string.IsNullOrEmpty(appendix.externalDocumentName))
            {
                LinkFileName.Text = appendix.externalDocumentName;
                LinkFileName.NavigateUrl = ResolveClientUrl(
                    string.Format("~/Documents/ExternalDocument.aspx?id={0}&name={1}",
                        Path.GetFileName(appendix.externalDocumentUniqueFileName), appendix.externalDocumentName
                        )
                    );
            }

            this.IsInitialized = true;
        }

        Utils.HideUnnecessaryHtmlEditorButtons(MemoAppendixIntro, true);
        Utils.HideUnnecessaryHtmlEditorButtons(MemoAppendixOutro, true);
    }

    public void SaveChanges()
    {
        RishProjectAppendixItem appendix = Page.GetEditedTreeItem<RishProjectAppendixItem>();
        if (appendix == null)
            return;

        appendix.name = EditAppendixName.Text;

        appendix.useExternalDocument = !RadioButtonDoc.Checked;
        if (appendix.useExternalDocument)
        {
            // Only when the temporary file name is initialized we can be certain
            // that a new file was uploaded. Only then we should touch the external_document
            // reference of the main document (which would cause .Modified status on the
            // affected rows).
            if (!string.IsNullOrEmpty(TempFileName.Value))
            {
                appendix.externalDocumentName = OrigFileName.Value;
                appendix.externalDocumentUniqueFileName =
                    Path.Combine(ExternalDocument.GetExternalDocumentStorePath(true), TempFileName.Value);
            }
        }
        else
        {
            appendix.introText = MemoAppendixIntro.Html;
            appendix.outroText = MemoAppendixOutro.Html;
        }
    }

    protected void CPAppendixProperties_Callback(object sender, CallbackEventArgsBase e)
    {
        bool enableDoc = e.Parameter.StartsWith("enabledoc:");
        bool enableAttach = e.Parameter.StartsWith("enableattach:");

        if (enableDoc || enableAttach)
        {
            EnableContolsDependentOnFileUpload(enableAttach);
        }
    }

    protected void EnableContolsDependentOnFileUpload(bool externalFileEnabled)
    {
        LabelAppendixName.ClientEnabled = !externalFileEnabled;
        EditAppendixName.ClientEnabled = !externalFileEnabled;
        LabelIntro.ClientEnabled = !externalFileEnabled;
        MemoAppendixIntro.ClientEnabled = !externalFileEnabled;
        LabelOutro.ClientEnabled = !externalFileEnabled;
        MemoAppendixOutro.ClientEnabled = !externalFileEnabled;

        UploadFile.Enabled = externalFileEnabled;

        RadioButtonDoc.Checked = !externalFileEnabled;
        RadioButtonFile.Checked = externalFileEnabled;
    }

    private bool IsInitialized
    {
        get { return Page.GetEditorFormInitialized(); }
        set { Page.SetEditorFormInitialized(value); }
    }

    protected void UploadFile_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        string attachmentFileName = ExternalDocument.Copy(e.UploadedFile.FileContent, e.UploadedFile.FileName, true);
        e.CallbackData =
            (new
            {
                OriginalFileName = Path.GetFileName(e.UploadedFile.FileName),

                // Passing the full path back to the client side is not such a good idea. Upon save,
                // we can always assume that the file was uploaded into the temporary storage and
                // rebuild its path correspondingly.
                TempFileName = Path.GetFileName(attachmentFileName),

                ViewDocumentUrl = ResolveClientUrl(
                    string.Format("~/Documents/ExternalDocument.aspx?id={0}&name={1}",
                        Path.GetFileName(attachmentFileName), Path.GetFileName(e.UploadedFile.FileName))
                    ),
            }).ToJSON();
    }
}