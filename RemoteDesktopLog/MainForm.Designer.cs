namespace RemoteDesktopLog
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton tsbAdd;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripButton tsbBatchDelete;
        private System.Windows.Forms.ToolStripButton tsbMoveUp;
        private System.Windows.Forms.ToolStripButton tsbMoveDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private System.Windows.Forms.ToolStripButton tsbImport;
        private System.Windows.Forms.ToolStripButton tsbConnect;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageMru;
        private System.Windows.Forms.TabPage tabPageOrphan;
        private System.Windows.Forms.DataGridView gridMru;
        private System.Windows.Forms.DataGridView gridOrphan;
        private System.Windows.Forms.Panel panelOrphanActions;
        private System.Windows.Forms.CheckBox chkOrphanDeleteCred;
        private System.Windows.Forms.Button btnOrphanDeleteSelected;
        private System.Windows.Forms.Button btnOrphanDeleteAll;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblCount;
        private System.Windows.Forms.ToolStripStatusLabel lblSpring;
        private System.Windows.Forms.ToolStripStatusLabel lblMstsc;
        private System.Windows.Forms.ContextMenuStrip mnuGrid;
        private System.Windows.Forms.ToolStripMenuItem mnuConnect;
        private System.Windows.Forms.ToolStripMenuItem mnuMoveUp;
        private System.Windows.Forms.ToolStripMenuItem mnuMoveDown;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ToolStripSeparator mnuSep;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.Timer mstscTimer;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbAdd = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbBatchDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveUp = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.tsbImport = new System.Windows.Forms.ToolStripButton();
            this.tsbConnect = new System.Windows.Forms.ToolStripButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageMru = new System.Windows.Forms.TabPage();
            this.tabPageOrphan = new System.Windows.Forms.TabPage();
            this.gridMru = new System.Windows.Forms.DataGridView();
            this.gridOrphan = new System.Windows.Forms.DataGridView();
            this.panelOrphanActions = new System.Windows.Forms.Panel();
            this.chkOrphanDeleteCred = new System.Windows.Forms.CheckBox();
            this.btnOrphanDeleteSelected = new System.Windows.Forms.Button();
            this.btnOrphanDeleteAll = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMstsc = new System.Windows.Forms.ToolStripStatusLabel();
            this.mnuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mstscTimer = new System.Windows.Forms.Timer(this.components);
            this.tabControl.SuspendLayout();
            this.tabPageMru.SuspendLayout();
            this.tabPageOrphan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMru)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridOrphan)).BeginInit();
            this.panelOrphanActions.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.mnuGrid.SuspendLayout();
            this.SuspendLayout();
            //
            // toolStrip
            //
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsbRefresh,
                this.tsbAdd,
                this.tsbDelete,
                this.tsbBatchDelete,
                this.tsbMoveUp,
                this.tsbMoveDown,
                this.toolStripSeparator1,
                this.tsbExport,
                this.tsbImport,
                this.tsbConnect});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(860, 25);
            this.toolStrip.TabIndex = 0;
            //
            // tsbRefresh
            //
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Text = "刷新";
            //
            // tsbAdd
            //
            this.tsbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbAdd.Name = "tsbAdd";
            this.tsbAdd.Text = "添加";
            //
            // tsbDelete
            //
            this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Text = "删除";
            //
            // tsbBatchDelete
            //
            this.tsbBatchDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbBatchDelete.Name = "tsbBatchDelete";
            this.tsbBatchDelete.Text = "批量删除";
            //
            // tsbMoveUp
            //
            this.tsbMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbMoveUp.Name = "tsbMoveUp";
            this.tsbMoveUp.Text = "上移";
            //
            // tsbMoveDown
            //
            this.tsbMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbMoveDown.Name = "tsbMoveDown";
            this.tsbMoveDown.Text = "下移";
            //
            // tsbExport
            //
            this.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Text = "导出";
            //
            // tsbImport
            //
            this.tsbImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbImport.Name = "tsbImport";
            this.tsbImport.Text = "导入";
            //
            // tsbConnect
            //
            this.tsbConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbConnect.Name = "tsbConnect";
            this.tsbConnect.Text = "连接";
            //
            // tabControl
            //
            this.tabControl.Controls.Add(this.tabPageMru);
            this.tabControl.Controls.Add(this.tabPageOrphan);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 25);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(860, 473);
            this.tabControl.TabIndex = 1;
            //
            // tabPageMru
            //
            this.tabPageMru.Controls.Add(this.gridMru);
            this.tabPageMru.Location = new System.Drawing.Point(4, 26);
            this.tabPageMru.Name = "tabPageMru";
            this.tabPageMru.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMru.Size = new System.Drawing.Size(852, 443);
            this.tabPageMru.TabIndex = 0;
            this.tabPageMru.Text = "MRU 历史记录";
            this.tabPageMru.UseVisualStyleBackColor = true;
            //
            // tabPageOrphan
            //
            this.tabPageOrphan.Controls.Add(this.gridOrphan);
            this.tabPageOrphan.Controls.Add(this.panelOrphanActions);
            this.tabPageOrphan.Location = new System.Drawing.Point(4, 26);
            this.tabPageOrphan.Name = "tabPageOrphan";
            this.tabPageOrphan.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOrphan.Size = new System.Drawing.Size(852, 443);
            this.tabPageOrphan.TabIndex = 1;
            this.tabPageOrphan.Text = "Servers 残留清理";
            this.tabPageOrphan.UseVisualStyleBackColor = true;
            //
            // gridMru
            //
            this.gridMru.AllowUserToAddRows = false;
            this.gridMru.AllowUserToDeleteRows = false;
            this.gridMru.AutoGenerateColumns = false;
            this.gridMru.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridMru.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridMru.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.gridMru.ContextMenuStrip = this.mnuGrid;
            this.gridMru.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMru.Location = new System.Drawing.Point(3, 3);
            this.gridMru.Name = "gridMru";
            this.gridMru.ReadOnly = true;
            this.gridMru.RowHeadersVisible = false;
            this.gridMru.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMru.Size = new System.Drawing.Size(846, 437);
            this.gridMru.TabIndex = 0;
            this.gridMru.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colOrder", HeaderText = "序号", DataPropertyName = "Order", FillWeight = 9, SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable, ReadOnly = true },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colServer", HeaderText = "服务器", DataPropertyName = "ServerName", FillWeight = 38, SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable, ReadOnly = true },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colUser", HeaderText = "用户名提示", DataPropertyName = "UsernameHint", FillWeight = 25, SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable, ReadOnly = true },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colCert", HeaderText = "证书指纹", DataPropertyName = "HasCertHash", FillWeight = 13, SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable, ReadOnly = true },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colCred", HeaderText = "已存凭据", DataPropertyName = "HasStoredCredential", FillWeight = 13, SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable, ReadOnly = true }});
            //
            // gridOrphan
            //
            this.gridOrphan.AllowUserToAddRows = false;
            this.gridOrphan.AllowUserToDeleteRows = false;
            this.gridOrphan.AutoGenerateColumns = false;
            this.gridOrphan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridOrphan.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridOrphan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridOrphan.Location = new System.Drawing.Point(3, 3);
            this.gridOrphan.Name = "gridOrphan";
            this.gridOrphan.ReadOnly = true;
            this.gridOrphan.RowHeadersVisible = false;
            this.gridOrphan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridOrphan.Size = new System.Drawing.Size(846, 391);
            this.gridOrphan.TabIndex = 0;
            this.gridOrphan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colOrphanKey", HeaderText = "子键名", DataPropertyName = "KeyName", FillWeight = 40, SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable, ReadOnly = true },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colOrphanUser", HeaderText = "用户名提示", DataPropertyName = "UsernameHint", FillWeight = 28, SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable, ReadOnly = true },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colOrphanCert", HeaderText = "证书指纹", DataPropertyName = "HasCertHash", FillWeight = 14, SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable, ReadOnly = true },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colOrphanCred", HeaderText = "已存凭据", DataPropertyName = "HasStoredCredential", FillWeight = 14, SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable, ReadOnly = true }});
            //
            // panelOrphanActions
            //
            this.panelOrphanActions.Controls.Add(this.chkOrphanDeleteCred);
            this.panelOrphanActions.Controls.Add(this.btnOrphanDeleteSelected);
            this.panelOrphanActions.Controls.Add(this.btnOrphanDeleteAll);
            this.panelOrphanActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOrphanActions.Location = new System.Drawing.Point(3, 394);
            this.panelOrphanActions.Name = "panelOrphanActions";
            this.panelOrphanActions.Size = new System.Drawing.Size(846, 46);
            this.panelOrphanActions.TabIndex = 1;
            //
            // chkOrphanDeleteCred
            //
            this.chkOrphanDeleteCred.AutoSize = true;
            this.chkOrphanDeleteCred.Checked = true;
            this.chkOrphanDeleteCred.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOrphanDeleteCred.Location = new System.Drawing.Point(10, 13);
            this.chkOrphanDeleteCred.Name = "chkOrphanDeleteCred";
            this.chkOrphanDeleteCred.Text = "同时删除已保存的凭据";
            //
            // btnOrphanDeleteSelected
            //
            this.btnOrphanDeleteSelected.Location = new System.Drawing.Point(240, 9);
            this.btnOrphanDeleteSelected.Name = "btnOrphanDeleteSelected";
            this.btnOrphanDeleteSelected.Size = new System.Drawing.Size(100, 28);
            this.btnOrphanDeleteSelected.TabIndex = 1;
            this.btnOrphanDeleteSelected.Text = "删除选中";
            //
            // btnOrphanDeleteAll
            //
            this.btnOrphanDeleteAll.Location = new System.Drawing.Point(350, 9);
            this.btnOrphanDeleteAll.Name = "btnOrphanDeleteAll";
            this.btnOrphanDeleteAll.Size = new System.Drawing.Size(100, 28);
            this.btnOrphanDeleteAll.TabIndex = 2;
            this.btnOrphanDeleteAll.Text = "全部清除";
            //
            // statusStrip
            //
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblCount,
                this.lblSpring,
                this.lblMstsc});
            this.statusStrip.Location = new System.Drawing.Point(0, 498);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(860, 22);
            this.statusStrip.TabIndex = 2;
            //
            // lblCount
            //
            this.lblCount.Name = "lblCount";
            this.lblCount.Text = "就绪";
            //
            // lblSpring
            //
            this.lblSpring.Name = "lblSpring";
            this.lblSpring.Spring = true;
            //
            // lblMstsc
            //
            this.lblMstsc.Name = "lblMstsc";
            this.lblMstsc.Text = "";
            //
            // mnuGrid
            //
            this.mnuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuConnect,
                this.mnuMoveUp,
                this.mnuMoveDown,
                this.mnuDelete,
                this.mnuSep,
                this.mnuCopy});
            this.mnuGrid.Name = "mnuGrid";
            //
            // mnuConnect
            //
            this.mnuConnect.Name = "mnuConnect";
            this.mnuConnect.Text = "连接(C)";
            //
            // mnuMoveUp
            //
            this.mnuMoveUp.Name = "mnuMoveUp";
            this.mnuMoveUp.Text = "上移";
            //
            // mnuMoveDown
            //
            this.mnuMoveDown.Name = "mnuMoveDown";
            this.mnuMoveDown.Text = "下移";
            //
            // mnuDelete
            //
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Text = "删除(D)";
            //
            // mnuCopy
            //
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Text = "复制服务器名";
            //
            // mstscTimer
            //
            this.mstscTimer.Interval = 2000;
            //
            // MainForm
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 520);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.MinimumSize = new System.Drawing.Size(640, 400);
            this.Name = "MainForm";
            this.Text = "远程桌面历史记录管理";
            this.tabControl.ResumeLayout(false);
            this.tabPageMru.ResumeLayout(false);
            this.tabPageOrphan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMru)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridOrphan)).EndInit();
            this.panelOrphanActions.ResumeLayout(false);
            this.panelOrphanActions.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.mnuGrid.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
