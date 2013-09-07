namespace NewLt
{
    partial class FormFileter
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gridFilter = new DevExpress.XtraGrid.GridControl();
            this.bandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.progressFilter = new DevExpress.XtraEditors.ProgressBarControl();
            this.gridFiltOut = new DevExpress.XtraGrid.GridControl();
            this.gridFiltViewOut = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandedGridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn5 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressFilter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFiltOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFiltViewOut)).BeginInit();
            this.SuspendLayout();
            // 
            // gridFilter
            // 
            this.gridFilter.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridFilter.Location = new System.Drawing.Point(8, 10);
            this.gridFilter.MainView = this.bandedGridView1;
            this.gridFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridFilter.Name = "gridFilter";
            this.gridFilter.Size = new System.Drawing.Size(396, 277);
            this.gridFilter.TabIndex = 2;
            this.gridFilter.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandedGridView1});
            // 
            // bandedGridView1
            // 
            this.bandedGridView1.Appearance.BandPanel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.bandedGridView1.Appearance.BandPanel.Options.UseFont = true;
            this.bandedGridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.bandedGridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.bandedGridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.bandedGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.bandedGridView1.Appearance.Row.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.bandedGridView1.Appearance.Row.Options.UseFont = true;
            this.bandedGridView1.Appearance.Row.Options.UseTextOptions = true;
            this.bandedGridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.bandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.bandedGridView1.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.bandedGridView1.GridControl = this.gridFilter;
            this.bandedGridView1.Name = "bandedGridView1";
            this.bandedGridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.bandedGridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.bandedGridView1.OptionsCustomization.AllowBandMoving = false;
            this.bandedGridView1.OptionsCustomization.AllowBandResizing = false;
            this.bandedGridView1.OptionsCustomization.AllowColumnMoving = false;
            this.bandedGridView1.OptionsCustomization.AllowColumnResizing = false;
            this.bandedGridView1.OptionsCustomization.AllowFilter = false;
            this.bandedGridView1.OptionsCustomization.AllowGroup = false;
            this.bandedGridView1.OptionsCustomization.AllowQuickHideColumns = false;
            this.bandedGridView1.OptionsCustomization.AllowSort = false;
            this.bandedGridView1.OptionsView.ShowGroupPanel = false;
            this.bandedGridView1.OptionsView.ShowIndicator = false;
            // 
            // gridBand1
            // 
            this.gridBand1.AppearanceHeader.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.gridBand1.AppearanceHeader.Options.UseFont = true;
            this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridBand1.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridBand1.Caption = "    过滤选项";
            this.gridBand1.Columns.Add(this.gridColumn1);
            this.gridBand1.Columns.Add(this.gridColumn2);
            this.gridBand1.Columns.Add(this.gridColumn3);
            this.gridBand1.Columns.Add(this.gridColumn4);
            this.gridBand1.Columns.Add(this.gridColumn5);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.RowCount = 2;
            this.gridBand1.Width = 394;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "名称";
            this.gridColumn1.FieldName = "TYPE";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowMove = false;
            this.gridColumn1.OptionsColumn.AllowShowHide = false;
            this.gridColumn1.OptionsColumn.AllowSize = false;
            this.gridColumn1.OptionsColumn.FixedWidth = true;
            this.gridColumn1.Visible = true;
            this.gridColumn1.Width = 50;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "最小值";
            this.gridColumn2.FieldName = "MIN";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.FixedWidth = true;
            this.gridColumn2.Visible = true;
            this.gridColumn2.Width = 50;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "最大值";
            this.gridColumn3.FieldName = "MAX";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.FixedWidth = true;
            this.gridColumn3.Visible = true;
            this.gridColumn3.Width = 50;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "包含";
            this.gridColumn4.FieldName = "INC";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.Width = 114;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "排除";
            this.gridColumn5.FieldName = "DEC";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.Width = 130;
            // 
            // progressFilter
            // 
            this.progressFilter.Location = new System.Drawing.Point(8, 265);
            this.progressFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressFilter.Name = "progressFilter";
            this.progressFilter.Size = new System.Drawing.Size(396, 22);
            this.progressFilter.TabIndex = 3;
            // 
            // gridFiltOut
            // 
            this.gridFiltOut.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            gridLevelNode1.RelationName = "Level1";
            this.gridFiltOut.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridFiltOut.Location = new System.Drawing.Point(410, 10);
            this.gridFiltOut.MainView = this.gridFiltViewOut;
            this.gridFiltOut.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridFiltOut.Name = "gridFiltOut";
            this.gridFiltOut.Size = new System.Drawing.Size(396, 277);
            this.gridFiltOut.TabIndex = 4;
            this.gridFiltOut.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridFiltViewOut});
            // 
            // gridFiltViewOut
            // 
            this.gridFiltViewOut.Appearance.BandPanel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.gridFiltViewOut.Appearance.BandPanel.Options.UseFont = true;
            this.gridFiltViewOut.Appearance.HeaderPanel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.gridFiltViewOut.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridFiltViewOut.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridFiltViewOut.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridFiltViewOut.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridFiltViewOut.Appearance.Row.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.gridFiltViewOut.Appearance.Row.Options.UseFont = true;
            this.gridFiltViewOut.Appearance.Row.Options.UseTextOptions = true;
            this.gridFiltViewOut.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridFiltViewOut.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridFiltViewOut.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand2});
            this.gridFiltViewOut.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.bandedGridColumn1,
            this.bandedGridColumn2,
            this.bandedGridColumn3,
            this.bandedGridColumn4,
            this.bandedGridColumn5});
            this.gridFiltViewOut.GridControl = this.gridFiltOut;
            this.gridFiltViewOut.Name = "gridFiltViewOut";
            this.gridFiltViewOut.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridFiltViewOut.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridFiltViewOut.OptionsCustomization.AllowBandMoving = false;
            this.gridFiltViewOut.OptionsCustomization.AllowBandResizing = false;
            this.gridFiltViewOut.OptionsCustomization.AllowColumnMoving = false;
            this.gridFiltViewOut.OptionsCustomization.AllowColumnResizing = false;
            this.gridFiltViewOut.OptionsCustomization.AllowFilter = false;
            this.gridFiltViewOut.OptionsCustomization.AllowGroup = false;
            this.gridFiltViewOut.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridFiltViewOut.OptionsCustomization.AllowSort = false;
            this.gridFiltViewOut.OptionsView.ShowGroupPanel = false;
            this.gridFiltViewOut.OptionsView.ShowIndicator = false;
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.gridBand2.AppearanceHeader.Options.UseFont = true;
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridBand2.Caption = "    过滤结果";
            this.gridBand2.Columns.Add(this.bandedGridColumn1);
            this.gridBand2.Columns.Add(this.bandedGridColumn2);
            this.gridBand2.Columns.Add(this.bandedGridColumn3);
            this.gridBand2.Columns.Add(this.bandedGridColumn4);
            this.gridBand2.Columns.Add(this.bandedGridColumn5);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.RowCount = 2;
            this.gridBand2.Width = 394;
            // 
            // bandedGridColumn1
            // 
            this.bandedGridColumn1.Caption = "号码";
            this.bandedGridColumn1.FieldName = "NUM";
            this.bandedGridColumn1.Name = "bandedGridColumn1";
            this.bandedGridColumn1.OptionsColumn.AllowMove = false;
            this.bandedGridColumn1.OptionsColumn.AllowShowHide = false;
            this.bandedGridColumn1.OptionsColumn.AllowSize = false;
            this.bandedGridColumn1.OptionsColumn.FixedWidth = true;
            this.bandedGridColumn1.OptionsColumn.ReadOnly = true;
            this.bandedGridColumn1.Visible = true;
            this.bandedGridColumn1.Width = 165;
            // 
            // bandedGridColumn2
            // 
            this.bandedGridColumn2.Caption = "和值";
            this.bandedGridColumn2.FieldName = "SUM";
            this.bandedGridColumn2.Name = "bandedGridColumn2";
            this.bandedGridColumn2.Visible = true;
            this.bandedGridColumn2.Width = 56;
            // 
            // bandedGridColumn3
            // 
            this.bandedGridColumn3.Caption = "AC";
            this.bandedGridColumn3.FieldName = "AC";
            this.bandedGridColumn3.Name = "bandedGridColumn3";
            this.bandedGridColumn3.Visible = true;
            this.bandedGridColumn3.Width = 56;
            // 
            // bandedGridColumn4
            // 
            this.bandedGridColumn4.Caption = "散度";
            this.bandedGridColumn4.FieldName = "SD";
            this.bandedGridColumn4.Name = "bandedGridColumn4";
            this.bandedGridColumn4.Visible = true;
            this.bandedGridColumn4.Width = 57;
            // 
            // bandedGridColumn5
            // 
            this.bandedGridColumn5.Caption = "偶数";
            this.bandedGridColumn5.FieldName = "ODD";
            this.bandedGridColumn5.Name = "bandedGridColumn5";
            this.bandedGridColumn5.Visible = true;
            this.bandedGridColumn5.Width = 60;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Location = new System.Drawing.Point(278, 21);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(116, 30);
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "全量过虑";
            this.simpleButton2.Click += new System.EventHandler(this.btAllFilter_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(561, 21);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(98, 30);
            this.simpleButton1.TabIndex = 5;
            this.simpleButton1.Text = "换一批";
            this.simpleButton1.Click += new System.EventHandler(this.btNextFilterOut_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.simpleButton4.Appearance.Options.UseFont = true;
            this.simpleButton4.Location = new System.Drawing.Point(667, 21);
            this.simpleButton4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(128, 30);
            this.simpleButton4.TabIndex = 5;
            this.simpleButton4.Text = "添加到历史列表";
            this.simpleButton4.Click += new System.EventHandler(this.btAddFilterOutToHis_Click);
            // 
            // FormFileter
            // 
            this.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 299);
            this.Controls.Add(this.simpleButton4);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.gridFiltOut);
            this.Controls.Add(this.progressFilter);
            this.Controls.Add(this.gridFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.LookAndFeel.SkinName = "Office 2010 Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFileter";
            this.ShowIcon = false;
            this.Text = "号码过滤";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.gridFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressFilter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFiltOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFiltViewOut)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridFilter;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn4;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn5;
        private DevExpress.XtraEditors.ProgressBarControl progressFilter;
        private DevExpress.XtraGrid.GridControl gridFiltOut;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gridFiltViewOut;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn4;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn5;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
    }
}