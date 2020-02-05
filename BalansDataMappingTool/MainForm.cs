using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;

namespace GUKV.BalansDataMappingTool
{
    public partial class MainForm : Form
    {
        #region Member variables

        /// <summary>
        /// Preferences
        /// </summary>
        Preferences preferences = new Preferences();

        /// <summary>
        /// Connection to the SQL Server
        /// </summary>
        private SqlConnection connectionSqlClient = null;

        /// <summary>
        /// Connection to the 1NF database in Firebird
        /// </summary>
        private FbConnection connection1NF = null;

        /// <summary>
        /// Connection to the Balans database in Firebird
        /// </summary>
        private FbConnection connectionBalans = null;

        /// <summary>
        /// Full list of organizations from 1 NF database
        /// </summary>
        private List<object> organizations1NF = new List<object>();

        /// <summary>
        /// The first 1000 organizations from 1 NF database - used instead of the full list
        /// when no filtering is aplied
        /// </summary>
        private List<object> organizations1NFFirst1000 = new List<object>();

        /// <summary>
        /// Full list of organizations from Balans database
        /// </summary>
        private List<object> organizationsBalans = new List<object>();

        /// <summary>
        /// A generic mapping between organizations, produced by comparing their ZKPO code
        /// </summary>
        private Dictionary<int, int> mappingBalansOrgTo1NFByZKPO = new Dictionary<int, int>();

        /// <summary>
        /// Existing mapping between 1NF organizations and Balans organizations
        /// </summary>
        private Dictionary<int, int> mappingBalansOrgTo1NF = new Dictionary<int, int>();

        /// <summary>
        /// A cache to lookup 1NF organization by its ZKPO code
        /// </summary>
        private Dictionary<long, int> mappingZkpo1NF = new Dictionary<long, int>();

        /// <summary>
        /// Indicates that there are some unsaved changes
        /// </summary>
        private bool changesPending = false;

        #endregion (Member variables)

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            SaveUserMappingToSqlServer();

            MessageBox.Show("Дані збережено", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            changesPending = false;
        }

        private void checkBoxShowOnlyUnmapped_CheckedChanged(object sender, EventArgs e)
        {
            FillBalansOrgList();
        }

        private void buttonApplyFilterBalans_Click(object sender, EventArgs e)
        {
            FillBalansOrgList();
        }

        private void buttonApplyFilter1NF_Click(object sender, EventArgs e)
        {
            Fill1NFOrgList();
        }

        private void checkBoxUseOrgSmartFilter_CheckedChanged(object sender, EventArgs e)
        {
            textBoxFilter1NF.Enabled = !checkBoxUseOrgSmartFilter.Checked;
            buttonApplyFilter1NF.Enabled = !checkBoxUseOrgSmartFilter.Checked;

            Fill1NFOrgList();
        }

        private void listBoxBalansOrganizations_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Write the name of selected Balans organization to the text box above the list
            if (listBoxBalansOrganizations.SelectedIndex >= 0)
            {
                OrganizationListBoxItem selectedItem = listBoxBalansOrganizations.Items[
                    listBoxBalansOrganizations.SelectedIndex] as OrganizationListBoxItem;

                textBoxSelectedOrgBalans.Text = selectedItem.organizationName;

                // If there is a mapping for this organization, display this mapping
                if (mappingBalansOrgTo1NF.ContainsKey(selectedItem.organizationId))
                {
                    int orgId1NF = mappingBalansOrgTo1NF[selectedItem.organizationId];

                    // Find this organization in the total array of 1NF organizations
                    for (int i = 0; i < organizations1NF.Count; i++)
                    {
                        OrganizationListBoxItem item1NF = organizations1NF[i] as OrganizationListBoxItem;

                        if (item1NF.organizationId == orgId1NF)
                        {
                            textBoxSelectedOrg1NF.Text = item1NF.organizationName;
                            break;
                        }
                    }
                }
                else
                {
                    textBoxSelectedOrg1NF.Text = "";
                }

                // Display 1NF organizations that could be a potential match
                // If Auto-filter is not enabled, the list would not change
                if (checkBoxUseOrgSmartFilter.Checked)
                {
                    Fill1NFOrgList();
                }
            }
            else
            {
                textBoxSelectedOrgBalans.Text = "";
                textBoxSelectedOrg1NF.Text = "";

                listBox1NFOrganizations.Items.Clear();
            }

            // Highlight mapping, if it is found
            if (textBoxSelectedOrgBalans.Text.Length > 0 &&
                textBoxSelectedOrg1NF.Text.Length > 0)
            {
                textBoxSelectedOrgBalans.ForeColor = Color.Blue;
                textBoxSelectedOrg1NF.ForeColor = Color.Blue;

                buttonDeassociate.Enabled = true;
            }
            else
            {
                textBoxSelectedOrgBalans.ForeColor = Color.Black;
                textBoxSelectedOrg1NF.ForeColor = Color.Black;

                buttonDeassociate.Enabled = false;
            }
        }

        private void listBox1NFOrganizations_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonAssociate.Enabled = true;
        }

        private void buttonAssociate_Click(object sender, EventArgs e)
        {
            if (listBoxBalansOrganizations.SelectedIndex >= 0 &&
                listBox1NFOrganizations.SelectedIndex >= 0)
            {
                OrganizationListBoxItem itemBalans = listBoxBalansOrganizations.Items[
                    listBoxBalansOrganizations.SelectedIndex] as OrganizationListBoxItem;

                OrganizationListBoxItem item1NF = listBox1NFOrganizations.Items[
                    listBox1NFOrganizations.SelectedIndex] as OrganizationListBoxItem;

                mappingBalansOrgTo1NF[itemBalans.organizationId] = item1NF.organizationId;

                // Display the new mapping
                textBoxSelectedOrg1NF.Text = item1NF.organizationName;

                // Highlight the new mapping
                textBoxSelectedOrgBalans.ForeColor = Color.Blue;
                textBoxSelectedOrg1NF.ForeColor = Color.Blue;

                // Now we have some unsaved changes
                changesPending = true;

                // Allow user to de-associate
                buttonDeassociate.Enabled = true;
            }
        }

        private void buttonDeassociate_Click(object sender, EventArgs e)
        {
            if (listBoxBalansOrganizations.SelectedIndex >= 0)
            {
                OrganizationListBoxItem itemBalans = listBoxBalansOrganizations.Items[
                    listBoxBalansOrganizations.SelectedIndex] as OrganizationListBoxItem;

                mappingBalansOrgTo1NF.Remove(itemBalans.organizationId);

                // Display the new mapping
                textBoxSelectedOrg1NF.Text = "";

                // Highlight the new mapping
                textBoxSelectedOrgBalans.ForeColor = Color.Black;
                textBoxSelectedOrg1NF.ForeColor = Color.Black;

                // Now we have some unsaved changes
                changesPending = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Read the preferences from INI file and connect to all databases
            if (preferences.ReadPreferencesFromFile())
            {
                if (Connect())
                {
                    Build1NFMappingZkpoToId();
                    ReadExistingMapping();

                    FillBalansOrgList();
                    Fill1NFOrgList();
                }
                else
                {
                    Application.Exit();
                }
            }
            else
            {
                Application.Exit();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changesPending)
            {
                if (MessageBox.Show("Зберегти зроблені зміни?", "Вихід з програми",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SaveUserMappingToSqlServer();
                }
            }

            Disconnect();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            int spacingBetweenPanels = 11;
            int panelIndent = 3;

            // Resize the panels in horizontal dimension
            Size pageSize = tabPageOrganizations.Size;

            int panelWidth = (pageSize.Width - (2 * panelIndent + spacingBetweenPanels)) / 2;

            panelLeft.Size = new Size(panelWidth, panelLeft.Size.Height);
            panelRight.Size = new Size(panelWidth, panelRight.Size.Height);
        }

        #region Working with organizations

        private void Build1NFMappingZkpoToId()
        {
            string querySelect = "SELECT KOD_OBJ, KOD_ZKPO FROM SORG_1NF WHERE (NOT KOD_ZKPO IS NULL)";

            try
            {
                using (FbCommand commandSelect = new FbCommand(querySelect, connection1NF))
                {
                    using (FbDataReader reader = commandSelect.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Get the organization data
                            object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                            object dataZkpo = reader.IsDBNull(1) ? null : reader.GetValue(1);

                            if (dataId is int && dataZkpo is string)
                            {
                                // Try to find organization in 1NF by ZKPO code
                                string zkpo = ((string)dataZkpo).Trim();

                                if (zkpo.Length > 0)
                                {
                                    long zkpoAsNumber = 0;

                                    if (long.TryParse(zkpo, out zkpoAsNumber))
                                    {
                                        mappingZkpo1NF[zkpoAsNumber] = (int)dataId;
                                    }
                                }
                            }
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(querySelect, ex.Message);
            }
        }

        private void ReadExistingMapping()
        {
            /////////////////////////////////////////////////////////////////////////////
            // 1) Read the mapping defined by ZKPO code
            /////////////////////////////////////////////////////////////////////////////

            // Get the required columns from the SORG table (Balans database)
            string querySelect = "SELECT KOD_OBJ, KOD_ZKPO FROM SORG";

            try
            {
                using (FbCommand commandSelect = new FbCommand(querySelect, connectionBalans))
                {
                    using (FbDataReader reader = commandSelect.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Get the organization data
                            object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                            object dataZkpo = reader.IsDBNull(1) ? null : reader.GetValue(1);

                            if (dataId is int && dataZkpo is string)
                            {
                                // Try to find organization in 1NF by ZKPO code
                                string zkpo = ((string)dataZkpo).Trim();

                                if (zkpo.Length > 0)
                                {
                                    long zkpoAsNumber = 0;

                                    if (long.TryParse(zkpo, out zkpoAsNumber))
                                    {
                                        if (mappingZkpo1NF.ContainsKey(zkpoAsNumber))
                                        {
                                            // Mapping by ZKPO code exists in 1 NF
                                            int orgIdIn1NF = mappingZkpo1NF[zkpoAsNumber];

                                            mappingBalansOrgTo1NF[(int)dataId] = orgIdIn1NF;
                                            mappingBalansOrgTo1NFByZKPO[(int)dataId] = orgIdIn1NF;
                                        }
                                    }
                                }
                            }
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(querySelect, ex.Message);
            }

            /////////////////////////////////////////////////////////////////////////////
            // 2) Read the user-defined mapping
            /////////////////////////////////////////////////////////////////////////////

            // Read the 'mapping_org_privat_1nf' table from SQL Server
            querySelect = "SELECT org_id_balans, org_id_1nf FROM mapping_org_balans_1nf";

            try
            {
                using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
                {
                    using (SqlDataReader reader = commandSelect.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Get the mapping entry
                            object dataOrgIdBalans = reader.IsDBNull(0) ? null : reader.GetValue(0);
                            object dataOrgId1nf = reader.IsDBNull(1) ? null : reader.GetValue(1);

                            if (dataOrgIdBalans is int && dataOrgId1nf is int)
                            {
                                mappingBalansOrgTo1NF[(int)dataOrgIdBalans] = (int)dataOrgId1nf;
                            }
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(querySelect, ex.Message);
            }
        }

        private void FillBalansOrgList()
        {
            if (organizationsBalans.Count == 0)
            {
                ReadBalansOrganizations();
            }

            listBoxBalansOrganizations.Items.Clear();

            // First, filter the unmapped organizations, if requested
            List<object> unmappedOrganizations = null;

            if (checkBoxShowOnlyUnmapped.Checked)
            {
                unmappedOrganizations = new List<object>();
                unmappedOrganizations.Capacity = 5000;

                foreach (object item in organizationsBalans)
                {
                    OrganizationListBoxItem orgItem = item as OrganizationListBoxItem;

                    // Add organization only if there is no mapping for it
                    if (!mappingBalansOrgTo1NF.ContainsKey(orgItem.organizationId))
                    {
                        unmappedOrganizations.Add(orgItem);
                    }
                }
            }
            else
            {
                // Add all organizations
                unmappedOrganizations = organizationsBalans;
            }

            // Now filter by the user-entered string, if requested
            string filter = textBoxFilterBalans.Text.Trim().ToUpper();

            if (filter.Length > 0)
            {
                // Add only organizations that contain the filter value
                for (int i = 0; i < unmappedOrganizations.Count; i++)
                {
                    OrganizationListBoxItem itemBalans = unmappedOrganizations[i] as OrganizationListBoxItem;

                    if (itemBalans.organizationName.ToUpper().Contains(filter))
                    {
                        listBoxBalansOrganizations.Items.Add(itemBalans);
                    }
                }
            }
            else
            {
                // Add all organizations
                listBoxBalansOrganizations.Items.AddRange(unmappedOrganizations.ToArray());
            }
        }

        private void Fill1NFOrgList()
        {
            if (organizations1NF.Count == 0)
            {
                Read1NFOrganizations();
            }

            if (checkBoxUseOrgSmartFilter.Checked)
            {
                ApplySmartFilter();
            }
            else
            {
                listBox1NFOrganizations.Items.Clear();

                string filter = textBoxFilter1NF.Text.Trim().ToUpper();

                if (filter.Length > 0)
                {
                    // Add only organizations that contain the filter value
                    for (int i = 0; i < organizations1NF.Count; i++)
                    {
                        OrganizationListBoxItem item1NF = organizations1NF[i] as OrganizationListBoxItem;

                        if (item1NF.organizationName.ToUpper().Contains(filter))
                        {
                            listBox1NFOrganizations.Items.Add(item1NF);
                        }
                    }
                }
                else
                {
                    // Add first 1000 organizations - for optimization
                    listBox1NFOrganizations.Items.AddRange(organizations1NFFirst1000.ToArray());
                }
            }

            // Right now no 1 NF organization is selected; disable the 'Associate' button
            buttonAssociate.Enabled = false;
        }

        private void ApplySmartFilter()
        {
            listBox1NFOrganizations.Items.Clear();

            string orgNameBalans = textBoxSelectedOrgBalans.Text.Trim().ToUpper();

            if (orgNameBalans.Length > 0)
            {
                // Split the name into separate words
                char[] separators = new char[9];

                separators[0] = ' ';
                separators[1] = '\t';
                separators[2] = '\"';
                separators[3] = '\'';
                separators[4] = '(';
                separators[5] = ')';
                separators[6] = '.';
                separators[7] = ',';
                separators[8] = '-';

                string[] nameWords = orgNameBalans.Split(separators);

                List<string> keywords = FilterKeywords(nameWords);

                if (keywords.Count > 0)
                {
                    int curBestIndex = 1; // No need to add organization names without any match

                    for (int i = 0; i < organizations1NF.Count; i++)
                    {
                        OrganizationListBoxItem item1NF = organizations1NF[i] as OrganizationListBoxItem;

                        string orgName1NF = item1NF.organizationName.ToUpper();

                        int orgIndex = GetOrganizationSmartIndex(orgName1NF, keywords);

                        if (orgIndex >= curBestIndex)
                        {
                            if (orgIndex > curBestIndex)
                            {
                                curBestIndex = orgIndex;

                                // There is a better match; remove all prevous findings
                                listBox1NFOrganizations.Items.Clear();
                            }

                            listBox1NFOrganizations.Items.Add(item1NF);
                        }
                    }
                }
            }
        }

        private int GetOrganizationSmartIndex(string orgName, List<string> keywords)
        {
            int keywordsFound = 0;

            foreach (string keyword in keywords)
            {
                if (orgName.Contains(keyword))
                {
                    keywordsFound++;
                }
            }

            return keywordsFound;
        }

        private List<string> FilterKeywords(string[] nameWords)
        {
            List<string> keywords = new List<string>();

            foreach (string item in nameWords)
            {
                string word = item.Trim();

                // Skip words that are too short
                if (word.Length > 3)
                {
                    // Skip the organization froms and words that are too common
                    if (/*
                        word != "ФІРМА" &&
                        word != "ДЕРЖАВНЕ" &&
                        word != "УПРАВЛІННЯ" &&
                        word != "ПАРТІЯ" &&
                        word != "СПІЛКА" &&
                        word != "КОМУНАЛЬНЕ" &&
                        word != "ПІДПРИЄМСТВО" &&
                        word != "ОРГАНІЗАЦІЯ" &&
                        word != "ПРИВАТНИЙ" &&
                        word != "ПІДПРИЄМЕЦЬ" &&
                        word != "КОМІТЕТ" &&
                        word != "УНІВЕРМАГ" &&
                        word != "УНІВЕРСАМ" &&
                        word != "САЛОН" &&
                        word != "БУДИНОК" &&
                        */

                        word != "ЗАКРИТЕ" &&
                        word != "ВІДКРИТЕ" &&
                        word != "АКЦІОНЕРНЕ" &&
                        word != "ТОВАРИСТВО" &&

                        word != "КИЇВСЬКА" &&
                        word != "КИЇВСЬКИЙ" &&
                        word != "КИЇВСЬКОГО" &&

                        word != "МІСЬКИЙ" &&
                        word != "МІСЬКА" &&
                        word != "МІСЬКОГО" &&
                        
                        word != "РАЙОННИЙ" &&
                        word != "РАЙОННА" &&
                        word != "РАЙОННОГО" &&

                        word != "ДЕРЖАВНИЙ" &&
                        word != "ДЕРЖАВНА" &&
                        word != "ДЕРЖАВНОГО")
                    {
                        keywords.Add(word);
                    }
                }
            }

            return keywords;
        }

        private void ReadBalansOrganizations()
        {
            // A primitive optimization
            organizationsBalans.Capacity = 5000;

            // Get all organizations from the SORG table (Balans database)
            string querySelect = "SELECT KOD_OBJ, FULL_NAME_OBJ FROM SORG";

            try
            {
                using (FbCommand commandSelect = new FbCommand(querySelect, connectionBalans))
                {
                    using (FbDataReader reader = commandSelect.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                            object dataName = reader.IsDBNull(1) ? null : reader.GetValue(1);

                            if (dataId is int && dataName is string)
                            {
                                string orgName = ((string)dataName).Trim();

                                if (orgName.Length > 0)
                                {
                                    organizationsBalans.Add(new OrganizationListBoxItem((int)dataId, orgName));
                                }
                            }
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(querySelect, ex.Message);
            }
        }

        private void Read1NFOrganizations()
        {
            // A primitive optimization
            organizations1NF.Capacity = 45000;
            organizations1NFFirst1000.Capacity = 1000;

            // Get all organizations from the SORG_1NF table (1NF database)
            string querySelect = "SELECT KOD_OBJ, FULL_NAME_OBJ FROM SORG_1NF";

            try
            {
                using (FbCommand commandSelect = new FbCommand(querySelect, connection1NF))
                {
                    using (FbDataReader reader = commandSelect.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                            object dataName = reader.IsDBNull(1) ? null : reader.GetValue(1);

                            if (dataId is int && dataName is string)
                            {
                                string orgName = ((string)dataName).Trim();

                                if (orgName.Length > 0)
                                {
                                    organizations1NF.Add(new OrganizationListBoxItem((int)dataId, orgName));
                                }
                            }
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(querySelect, ex.Message);
            }

            // Copy the first 1000 organizations to the 'optimized' array
            if (organizations1NF.Count <= 1000)
            {
                organizations1NFFirst1000.AddRange(organizations1NF);
            }
            else
            {
                for (int i = 0; i < 1000; i++)
                {
                    organizations1NFFirst1000.Add(organizations1NF[i]);
                }
            }
        }

        private void SaveUserMappingToSqlServer()
        {
            /////////////////////////////////////////////////////////////////////////////
            // 1) Erase all previous entries from the mapping table
            /////////////////////////////////////////////////////////////////////////////

            string queryErase = "DELETE FROM mapping_org_balans_1nf";

            try
            {
                using (SqlCommand commandErase = new SqlCommand(queryErase, connectionSqlClient))
                {
                    commandErase.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(queryErase, ex.Message);
            }

            /////////////////////////////////////////////////////////////////////////////
            // 2) Save all entries to the mapping table
            /////////////////////////////////////////////////////////////////////////////

            foreach (KeyValuePair<int, int> entry in mappingBalansOrgTo1NF)
            {
                int orgIdBalans = entry.Key;
                int orgId1NF = entry.Value;

                // We save only user-defined mappings; if this mapping is
                // generated by the ZKPO code, we do not save it
                bool mappingByZkpo = false;

                if (mappingBalansOrgTo1NFByZKPO.ContainsKey(orgIdBalans))
                {
                    if (mappingBalansOrgTo1NFByZKPO[orgIdBalans] == orgId1NF)
                    {
                        mappingByZkpo = true;
                    }
                }

                if (!mappingByZkpo)
                {
                    // INSERT this mapping entry into the table
                    string queryInsert = "INSERT INTO " + preferences.databaseNameSqlServer +
                        ".dbo.mapping_org_balans_1nf (org_id_balans, org_id_1nf) VALUES (" +
                        orgIdBalans.ToString() + ", " + orgId1NF.ToString() + ")";

                    try
                    {
                        using (SqlCommand commandInsert = new SqlCommand(queryInsert, connectionSqlClient))
                        {
                            commandInsert.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowSqlErrorMessageDlg(queryInsert, ex.Message);
                    }
                }
            }
        }

        #endregion (Working with organizations)

        #region Connection to databases

        /// <summary>
        /// Connects to SQL Server and all the Firebird databases
        /// </summary>
        /// <returns>'True' if connection to all databases was successful</returns>
        public bool Connect()
        {
            string connStringSqlServer = preferences.GetSqlServerConnectionString();
            string connString1NF = preferences.Get1NFConnectionString();
            string connStringBalans = preferences.GetBalansConnectionString();

            // Connect to SQL Server
            connectionSqlClient = new SqlConnection(connStringSqlServer);

            try
            {
                connectionSqlClient.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Cannot conect to SQL Server. Message: " + ex.Message + " Connection string: " + connStringSqlServer,
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            // Connect to 1NF database
            connection1NF = new FbConnection(connString1NF);

            try
            {
                connection1NF.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Cannot conect to database 1NF. Message: " + ex.Message + " Connection string: " + connString1NF,
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            // Connect to Balans database
            connectionBalans = new FbConnection(connStringBalans);

            try
            {
                connectionBalans.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Cannot conect to database Balans. Message: " + ex.Message + " Connection string: " + connStringBalans,
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Disconnects from all the databases
        /// </summary>
        public void Disconnect()
        {
            if (connectionSqlClient != null && connectionSqlClient.State == ConnectionState.Open)
            {
                connectionSqlClient.Close();
                connectionSqlClient = null;
            }

            if (connection1NF != null && connection1NF.State == ConnectionState.Open)
            {
                connection1NF.Close();
                connection1NF = null;
            }

            if (connectionBalans != null && connectionBalans.State == ConnectionState.Open)
            {
                connectionBalans.Close();
                connectionBalans = null;
            }
        }

        #endregion (Connection to databases)

        #region Debugging

        /// <summary>
        /// Displays an error message window for an invalid SQL statement
        /// </summary>
        /// <param name="sqlStatement">The SQL statement that caused an error</param>
        /// <param name="errorMessage">The error message</param>
        private void ShowSqlErrorMessageDlg(string sqlStatement, string errorMessage)
        {
            // Display the error dialog
            using (SqlErrorForm errorForm = new SqlErrorForm())
            {
                errorForm.SqlStatement = sqlStatement;
                errorForm.ErrorMessage = errorMessage;

                errorForm.ShowDialog();
            }
        }

        #endregion (Debugging)
    }
}
