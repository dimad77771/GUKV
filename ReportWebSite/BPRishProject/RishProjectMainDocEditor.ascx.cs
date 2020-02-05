using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.ASPxUploadControl;
using System.IO;

public partial class RishProjectMainDocEditor : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RishProjectMainDocItem mainDoc = Page.GetEditedTreeItem<RishProjectMainDocItem>();
        if (mainDoc == null)
            return;

        if (!this.IsInitialized)
        {
            EditDocSubject.Text = mainDoc.subject;

            ComboRishContact.DataBind();
            ComboRishContact.SelectedIndex = ComboRishContact.Items.IndexOfValue(mainDoc.projectContactId);

            MemoDocIntro.Html = mainDoc.introText.Length > 0 ? mainDoc.introText : "<p style=\"font-family: Times New Roman; font-size: 14pt;\">&nbsp;</p>";
            MemoDocOutro.Html = mainDoc.outroText.Length > 0 ? mainDoc.outroText : "<p style=\"font-family: Times New Roman; font-size: 14pt;\">&nbsp;</p>";

            EnableContolsDependentOnFileUpload(mainDoc.useExternalDocument);

            if (!string.IsNullOrEmpty(mainDoc.externalDocumentName))
            {
                LinkFileName.Text = mainDoc.externalDocumentName;
                LinkFileName.NavigateUrl = ResolveClientUrl(
                    string.Format("~/Documents/ExternalDocument.aspx?id={0}&name={1}",
                        Path.GetFileName(mainDoc.externalDocumentUniqueFileName), mainDoc.externalDocumentName
                        )
                    );
            }

            this.IsInitialized = true;
        }

        Utils.HideUnnecessaryHtmlEditorButtons(MemoDocIntro, true);
        Utils.HideUnnecessaryHtmlEditorButtons(MemoDocOutro, true);
    }

    protected void CPMainDocProperties_Callback(object sender, CallbackEventArgsBase e)
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
        LabelIntro.ClientEnabled = !externalFileEnabled;
        MemoDocIntro.ClientEnabled = !externalFileEnabled;
        LabelOutro.ClientEnabled = !externalFileEnabled;
        MemoDocOutro.ClientEnabled = !externalFileEnabled;

        UploadFile.Enabled = externalFileEnabled;

        RadioButtonDoc.Checked = !externalFileEnabled;
        RadioButtonFile.Checked = externalFileEnabled;
    }

    public void SaveChanges()
    {
        RishProjectMainDocItem mainDoc = Page.GetEditedTreeItem<RishProjectMainDocItem>();
        if (mainDoc == null)
            return;

        mainDoc.subject = EditDocSubject.Text;

        if (ComboRishContact.Value is int)
            mainDoc.projectContactId = (int)ComboRishContact.Value;

        mainDoc.useExternalDocument = !RadioButtonDoc.Checked;
        if (mainDoc.useExternalDocument)
        {
            // Only when the temporary file name is initialized we can be certain
            // that a new file was uploaded. Only then we should touch the external_document
            // reference of the main document (which would cause .Modified status on the
            // affected rows).
            if (!string.IsNullOrEmpty(TempFileName.Value))
            {
                mainDoc.externalDocumentName = OrigFileName.Value;
                mainDoc.externalDocumentUniqueFileName =
                    Path.Combine(ExternalDocument.GetExternalDocumentStorePath(true), TempFileName.Value);
            }
        }
        else
        {
            mainDoc.introText = MemoDocIntro.Html;
            mainDoc.outroText = MemoDocOutro.Html;
        }
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