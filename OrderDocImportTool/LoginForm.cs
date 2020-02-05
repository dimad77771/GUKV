using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace GUKV.DataMigration
{
    public partial class LoginForm : Form
    {
        public Preferences preferences = null;

        public LoginForm()
        {
            InitializeComponent();

            // Fill the drop-down of users
            foreach (KeyValuePair<string, string> usr in DB.users)
            {
                comboUsers.Items.Add(usr.Value.Trim());
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (comboUsers.SelectedItem is string)
            {
                // Find the user name by the display name
                string userDisplayName = comboUsers.SelectedItem as string;
                string userName = "";
                string userPassword = editPassword.Text;

                foreach (KeyValuePair<string, string> usr in DB.users)
                {
                    if (usr.Value.Trim() == userDisplayName)
                    {
                        userName = usr.Key;
                        break;
                    }
                }

                if (userName.Length > 0)
                {
                    // Try to connect to NJF database with those credentials
                    Cursor.Current = Cursors.WaitCursor;

                    bool connectedOk = false;

                    FbConnection connection = new FbConnection(preferences.GetNJFConnectionString(userName, userPassword));

                    try
                    {
                        connection.Open();
                        connection.Close();

                        connectedOk = true;
                    }
                    catch (Exception)
                    {
                    }

                    Cursor.Current = Cursors.Default;

                    if (connectedOk)
                    {
                        DB.UserName = userName;
                        DB.UserPassword = userPassword;

                        DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("І'мя користувача або пароль не є вірними.",
                            "Помилка",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Не знайдено користувача з таким іменем.",
                        "Помилка",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, виберіть і'мя користувача та введіть пароль.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void editPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(this, new EventArgs());
            }
        }
    }
}
