namespace NewLt
{
    partial class MainForm
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
            this.sc_h_1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.sc_h_v2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.grid_history = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.sc_h_1)).BeginInit();
            this.sc_h_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sc_h_v2)).BeginInit();
            this.sc_h_v2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_history)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // sc_h_1
            // 
            this.sc_h_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc_h_1.Location = new System.Drawing.Point(0, 0);
            this.sc_h_1.Name = "sc_h_1";
            this.sc_h_1.Panel1.Controls.Add(this.sc_h_v2);
            this.sc_h_1.Panel1.Text = "Panel1";
            this.sc_h_1.Panel2.Text = "Panel2";
            this.sc_h_1.Size = new System.Drawing.Size(1002, 595);
            this.sc_h_1.SplitterPosition = 742;
            this.sc_h_1.TabIndex = 0;
            this.sc_h_1.Text = "splitContainerControl1";
            // 
            // sc_h_v2
            // 
            this.sc_h_v2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc_h_v2.Horizontal = false;
            this.sc_h_v2.Location = new System.Drawing.Point(0, 0);
            this.sc_h_v2.Name = "sc_h_v2";
            this.sc_h_v2.Panel1.Controls.Add(this.grid_history);
            this.sc_h_v2.Panel1.Text = "Panel1";
            this.sc_h_v2.Panel2.Text = "Panel2";
            this.sc_h_v2.Size = new System.Drawing.Size(742, 595);
            this.sc_h_v2.SplitterPosition = 328;
            this.sc_h_v2.TabIndex = 0;
            this.sc_h_v2.Text = "splitContainerControl1";
            // 
            // grid_history
            // 
            this.grid_history.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_history.Location = new System.Drawing.Point(0, 0);
            this.grid_history.MainView = this.gridView1;
            this.grid_history.Name = "grid_history";
            this.grid_history.Size = new System.Drawing.Size(742, 328);
            this.grid_history.TabIndex = 0;
            this.grid_history.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.grid_history;
            this.gridView1.Name = "gridView1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 595);
            this.Controls.Add(this.sc_h_1);
            this.LookAndFeel.SkinName = "Office 2010 Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.sc_h_1)).EndInit();
            this.sc_h_1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sc_h_v2)).EndInit();
            this.sc_h_v2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_history)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl sc_h_1;
        private DevExpress.XtraEditors.SplitContainerControl sc_h_v2;
        private DevExpress.XtraGrid.GridControl grid_history;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;


    }
}
