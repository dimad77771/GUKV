namespace GrantAccessTool
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxServerName = new System.Windows.Forms.TextBox();
            this.textBoxDatabaseName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxUserToGrant = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonGrant = new System.Windows.Forms.Button();
            this.textBoxSQL = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxServerName
            // 
            this.textBoxServerName.Location = new System.Drawing.Point(11, 28);
            this.textBoxServerName.Name = "textBoxServerName";
            this.textBoxServerName.Size = new System.Drawing.Size(334, 23);
            this.textBoxServerName.TabIndex = 0;
            this.textBoxServerName.Text = "gukv";
            // 
            // textBoxDatabaseName
            // 
            this.textBoxDatabaseName.Location = new System.Drawing.Point(365, 28);
            this.textBoxDatabaseName.Name = "textBoxDatabaseName";
            this.textBoxDatabaseName.Size = new System.Drawing.Size(334, 23);
            this.textBoxDatabaseName.TabIndex = 1;
            this.textBoxDatabaseName.Text = "d:\\database\\njf\\newnjf.gdb";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(362, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Database Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(362, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "User Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "User Name";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(365, 82);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(334, 23);
            this.textBoxPassword.TabIndex = 5;
            this.textBoxPassword.Text = "masterkey";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(11, 82);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(334, 23);
            this.textBoxUserName.TabIndex = 4;
            this.textBoxUserName.Text = "SYSDBA";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(145, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Grant Full Access To";
            // 
            // textBoxUserToGrant
            // 
            this.textBoxUserToGrant.Location = new System.Drawing.Point(11, 140);
            this.textBoxUserToGrant.Name = "textBoxUserToGrant";
            this.textBoxUserToGrant.Size = new System.Drawing.Size(688, 23);
            this.textBoxUserToGrant.TabIndex = 9;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(583, 335);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(117, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonGrant
            // 
            this.buttonGrant.Location = new System.Drawing.Point(460, 335);
            this.buttonGrant.Name = "buttonGrant";
            this.buttonGrant.Size = new System.Drawing.Size(117, 23);
            this.buttonGrant.TabIndex = 11;
            this.buttonGrant.Text = "Grant Access";
            this.buttonGrant.UseVisualStyleBackColor = true;
            this.buttonGrant.Click += new System.EventHandler(this.buttonGrant_Click);
            // 
            // textBoxSQL
            // 
            this.textBoxSQL.Location = new System.Drawing.Point(12, 204);
            this.textBoxSQL.Multiline = true;
            this.textBoxSQL.Name = "textBoxSQL";
            this.textBoxSQL.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSQL.Size = new System.Drawing.Size(688, 115);
            this.textBoxSQL.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 185);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "System SQL";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 370);
            this.Controls.Add(this.textBoxSQL);
            this.Controls.Add(this.buttonGrant);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxUserToGrant);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxDatabaseName);
            this.Controls.Add(this.textBoxServerName);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Rights Updater for NJF Database";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxServerName;
        private System.Windows.Forms.TextBox textBoxDatabaseName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxUserToGrant;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonGrant;
        private System.Windows.Forms.TextBox textBoxSQL;
        private System.Windows.Forms.Label label6;
    }
}

