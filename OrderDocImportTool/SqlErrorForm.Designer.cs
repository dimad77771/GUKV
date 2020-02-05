namespace GUKV.DataMigration
{
    partial class SqlErrorForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSqlStatement = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxErrorMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(420, 345);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "SQL Statement";
            // 
            // textBoxSqlStatement
            // 
            this.textBoxSqlStatement.Location = new System.Drawing.Point(12, 25);
            this.textBoxSqlStatement.Multiline = true;
            this.textBoxSqlStatement.Name = "textBoxSqlStatement";
            this.textBoxSqlStatement.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSqlStatement.Size = new System.Drawing.Size(483, 167);
            this.textBoxSqlStatement.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Error Message";
            // 
            // textBoxErrorMessage
            // 
            this.textBoxErrorMessage.Location = new System.Drawing.Point(12, 217);
            this.textBoxErrorMessage.Multiline = true;
            this.textBoxErrorMessage.Name = "textBoxErrorMessage";
            this.textBoxErrorMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxErrorMessage.Size = new System.Drawing.Size(483, 116);
            this.textBoxErrorMessage.TabIndex = 4;
            // 
            // SqlErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 380);
            this.Controls.Add(this.textBoxErrorMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSqlStatement);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SqlErrorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Помилка під час завантаження даних";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSqlStatement;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxErrorMessage;
    }
}