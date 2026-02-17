namespace TFSData
{
    partial class CaiaphasForm
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
            if(disposing && (components != null))
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
            this.lblHeader = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.lblTFSCodeReviewQuery = new System.Windows.Forms.Label();
            this.txtCodeReviewQuery = new System.Windows.Forms.TextBox();
            this.btnChangeSet = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTFSCodeReviewResponseQuery = new System.Windows.Forms.Label();
            this.txtCodeReviewResponseQuery = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            //
            // lblHeader
            //
            this.lblHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(12, 9);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(909, 60);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Team Foundation Server (TFS) Data Extraction Tool";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // txtUrl
            //
            this.txtUrl.Location = new System.Drawing.Point(12, 118);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(909, 20);
            this.txtUrl.TabIndex = 1;
            //
            // lblUrl
            //
            this.lblUrl.AutoSize = true;
            this.lblUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUrl.Location = new System.Drawing.Point(8, 95);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(216, 29);
            this.lblUrl.TabIndex = 2;
            this.lblUrl.Text = "Enter the TFS Url";
            //
            // lblTFSCodeReviewQuery
            //
            this.lblTFSCodeReviewQuery.AutoSize = true;
            this.lblTFSCodeReviewQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTFSCodeReviewQuery.Location = new System.Drawing.Point(8, 146);
            this.lblTFSCodeReviewQuery.Name = "lblTFSCodeReviewQuery";
            this.lblTFSCodeReviewQuery.Size = new System.Drawing.Size(303, 29);
            this.lblTFSCodeReviewQuery.TabIndex = 8;
            this.lblTFSCodeReviewQuery.Text = "TFS Code Review Query";
            //
            // txtCodeReviewQuery
            //
            this.txtCodeReviewQuery.Location = new System.Drawing.Point(12, 169);
            this.txtCodeReviewQuery.Multiline = true;
            this.txtCodeReviewQuery.Name = "txtCodeReviewQuery";
            this.txtCodeReviewQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCodeReviewQuery.Size = new System.Drawing.Size(909, 164);
            this.txtCodeReviewQuery.TabIndex = 7;
            //
            // btnChangeSet
            //
            this.btnChangeSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeSet.Location = new System.Drawing.Point(12, 541);
            this.btnChangeSet.Name = "btnChangeSet";
            this.btnChangeSet.Size = new System.Drawing.Size(253, 35);
            this.btnChangeSet.TabIndex = 9;
            this.btnChangeSet.Text = "Execute";
            this.btnChangeSet.UseVisualStyleBackColor = true;
            //
            // btnClose
            //
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(668, 541);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(253, 35);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            //
            // lblTFSCodeReviewResponseQuery
            //
            this.lblTFSCodeReviewResponseQuery.AutoSize = true;
            this.lblTFSCodeReviewResponseQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTFSCodeReviewResponseQuery.Location = new System.Drawing.Point(8, 348);
            this.lblTFSCodeReviewResponseQuery.Name = "lblTFSCodeReviewResponseQuery";
            this.lblTFSCodeReviewResponseQuery.Size = new System.Drawing.Size(428, 29);
            this.lblTFSCodeReviewResponseQuery.TabIndex = 16;
            this.lblTFSCodeReviewResponseQuery.Text = "TFS Code Review Response Query";
            //
            // txtCodeReviewResponseQuery
            //
            this.txtCodeReviewResponseQuery.Location = new System.Drawing.Point(12, 371);
            this.txtCodeReviewResponseQuery.Multiline = true;
            this.txtCodeReviewResponseQuery.Name = "txtCodeReviewResponseQuery";
            this.txtCodeReviewResponseQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCodeReviewResponseQuery.Size = new System.Drawing.Size(909, 164);
            this.txtCodeReviewResponseQuery.TabIndex = 15;
            //
            // CaiaphasForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 584);
            this.Controls.Add(this.lblTFSCodeReviewResponseQuery);
            this.Controls.Add(this.txtCodeReviewResponseQuery);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnChangeSet);
            this.Controls.Add(this.lblTFSCodeReviewQuery);
            this.Controls.Add(this.txtCodeReviewQuery);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.lblHeader);
            this.Name = "CaiaphasForm";
            this.Text = "CaiaphasForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.Label lblTFSCodeReviewQuery;
        private System.Windows.Forms.TextBox txtCodeReviewQuery;
        private System.Windows.Forms.Button btnChangeSet;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTFSCodeReviewResponseQuery;
        private System.Windows.Forms.TextBox txtCodeReviewResponseQuery;
    }
}