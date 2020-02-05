using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DevExpress.Web.ASPxEditors;
using System.Data;

namespace GUKV
{
    /// <summary>
    /// Summary description for ValidatorBase
    /// </summary>
    public class ValidatorBase
    {
        protected System.Web.UI.Page page;
        public bool IsValid = true;
        public IList<string> ValidationErrorMessages = new List<string>();

        protected List<string> reqEditors = new List<string>();
        protected List<string> reqFields = new List<string>();
        protected List<string> errorText = new List<string>();

        public ValidatorBase()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void AddControl(string ctlID, string validationGroup, string field, string error)
        {
            reqEditors.Add(ctlID);
            reqFields.Add(field);
            errorText.Add(error);

            if (page != null)
            {
                //ASPxTextEdit ctl = (ASPxTextEdit)controls[ctlID.ToLower()];
                ASPxTextEdit ctl = (ASPxTextEdit)page.FindControlRecursive(ctlID);
//pgv
 	if (ctl != null)
		{
	               ctl.Validation += new EventHandler<ValidationEventArgs>(OnValidation);
 	               ctl.ValidationSettings.ValidationGroup = validationGroup;
 	               //ctl.ValidationSettings.Display = Display.None;
 	               ctl.ValidationSettings.ErrorText = error;
  		 }
            }
        }

        public void AddField(string field, string error)
        {
            reqFields.Add(field);
            errorText.Add(error);
        }

        protected void OnValidation(object sender, ValidationEventArgs e)
        {
            ASPxTextEdit edit = sender as ASPxTextEdit;
            if (reqEditors.Contains(edit.ID))
            {
                if (edit.Text.Trim() == "" || edit.Text == "<НЕ ЗАДАНО>")
                {
                    e.IsValid = false;
                    IsValid = false;
                    ValidationErrorMessages.Add(e.ErrorText);
                }
            }
        }

        public bool ValidateUI()
        {
            IsValid = true;
            ValidationErrorMessages.Clear();

            foreach (string ctlID in reqEditors)
            {
                //ASPxTextEdit ctl = (ASPxTextEdit)controls[reqfield.ToLower()];
                ASPxTextEdit ctl = (ASPxTextEdit)page.FindControlRecursive(ctlID);
//pgv 
               	if (ctl != null)
		{
		 ctl.Validate();
           		 }
            }

            return IsValid;
        }

        public bool ValidateDB(SqlConnection connection, string tableName, string keyFilter, bool updateDB = false)
        {
            IsValid = true;
            ValidationErrorMessages.Clear();

            SqlTransaction transaction = null;
            if (updateDB)
                transaction = connection.BeginTransaction();

            string query = "select " + string.Join(", ", reqFields) + " from " + tableName + " where " + keyFilter;
            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        for (int i = 0; i < reqFields.Count; i++)
                        {
                            if (reader.IsDBNull(i))
                            {
                                IsValid = false;
                                ValidationErrorMessages.Add(errorText[i]);
                            }
                        }
                    }

                    reader.Close();
                }
            }

            if (updateDB)
            {
                query = "update " + tableName + " set is_valid = @isValid, validation_errors = @errMsgs where " + keyFilter;
                using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("isValid", IsValid);
                    cmd.Parameters.AddWithValue("errMsgs", (!IsValid && ValidationErrorMessages.Count > 0) ? string.Join("<br/>", ValidationErrorMessages) : "");
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }

            return IsValid;
        }

        public DataTable FormatErrorDataSource()
        {
            DataTable res = new DataTable();
            res.Columns.Add("error_message");
            foreach (string msg in ValidationErrorMessages)
            {
                DataRow r = res.NewRow();
                r["error_message"] = msg;
                res.Rows.Add(r);
            }
            return res;
        }

    }
}