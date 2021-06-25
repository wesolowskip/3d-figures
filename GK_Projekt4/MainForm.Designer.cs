namespace GK_Projekt4
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.openFileBtn = new System.Windows.Forms.Button();
            this.saveFileBtn = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.perspectiveCorrectionCheckbox = new System.Windows.Forms.CheckBox();
            this.backfaceCullingCheckbox = new System.Windows.Forms.CheckBox();
            this.zBufferingCheckbox = new System.Windows.Forms.CheckBox();
            this.lightingModelCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.offAnimationRadio = new System.Windows.Forms.RadioButton();
            this.oneFpsAnimationRadio = new System.Windows.Forms.RadioButton();
            this.normalAnimationRadio = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lightsListBox = new System.Windows.Forms.ListBox();
            this.lightsMenuStrip = new System.Windows.Forms.MenuStrip();
            this.addLightMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteLightMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.figuresListBox = new System.Windows.Forms.ListBox();
            this.figuresMenuStrip = new System.Windows.Forms.MenuStrip();
            this.addFigureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCuboidMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSphereMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCylinderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addConeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFigureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.camerasListBox = new System.Windows.Forms.ListBox();
            this.camerasMenuStrip = new System.Windows.Forms.MenuStrip();
            this.addCameraMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCameraMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.fpsTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.lightsMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.figuresMenuStrip.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.camerasMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(984, 581);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.pictureBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(345, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(636, 575);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(636, 575);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox6, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.groupBox7, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.groupBox8, 0, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(214, 575);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.propertyGrid);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 83);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(208, 228);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Edycja obiektu";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 156);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.MaximumSize = new System.Drawing.Size(180, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 65);
            this.label1.TabIndex = 1;
            this.label1.Text = "Uwaga! Przy dużym obciążeniu PropertyGrid może nie działać, dlatego wtedy należy " +
    "najpierw włączyć tryb 1FPS lub wyłączyć animację";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Location = new System.Drawing.Point(3, 16);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(202, 137);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.openFileBtn);
            this.groupBox2.Controls.Add(this.saveFileBtn);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(208, 74);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Projekt";
            // 
            // openFileBtn
            // 
            this.openFileBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.openFileBtn.Location = new System.Drawing.Point(17, 45);
            this.openFileBtn.Name = "openFileBtn";
            this.openFileBtn.Size = new System.Drawing.Size(176, 23);
            this.openFileBtn.TabIndex = 1;
            this.openFileBtn.Text = "Wczytaj scenę z pliku";
            this.openFileBtn.UseVisualStyleBackColor = true;
            this.openFileBtn.Click += new System.EventHandler(this.openFileBtn_Click);
            // 
            // saveFileBtn
            // 
            this.saveFileBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.saveFileBtn.Location = new System.Drawing.Point(17, 19);
            this.saveFileBtn.Name = "saveFileBtn";
            this.saveFileBtn.Size = new System.Drawing.Size(176, 23);
            this.saveFileBtn.TabIndex = 0;
            this.saveFileBtn.Text = "Zapisz scenę do pliku";
            this.saveFileBtn.UseVisualStyleBackColor = true;
            this.saveFileBtn.Click += new System.EventHandler(this.saveFileBtn_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.perspectiveCorrectionCheckbox);
            this.groupBox6.Controls.Add(this.backfaceCullingCheckbox);
            this.groupBox6.Controls.Add(this.zBufferingCheckbox);
            this.groupBox6.Controls.Add(this.lightingModelCheckbox);
            this.groupBox6.Location = new System.Drawing.Point(3, 317);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 114);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Dodatkowe opcje";
            // 
            // perspectiveCorrectionCheckbox
            // 
            this.perspectiveCorrectionCheckbox.AutoSize = true;
            this.perspectiveCorrectionCheckbox.Location = new System.Drawing.Point(6, 87);
            this.perspectiveCorrectionCheckbox.Name = "perspectiveCorrectionCheckbox";
            this.perspectiveCorrectionCheckbox.Size = new System.Drawing.Size(130, 17);
            this.perspectiveCorrectionCheckbox.TabIndex = 3;
            this.perspectiveCorrectionCheckbox.Text = "Korekcja perspektywy";
            this.perspectiveCorrectionCheckbox.UseVisualStyleBackColor = true;
            this.perspectiveCorrectionCheckbox.CheckedChanged += new System.EventHandler(this.perspectiveCorrectionCheckbox_CheckedChanged);
            // 
            // backfaceCullingCheckbox
            // 
            this.backfaceCullingCheckbox.AutoSize = true;
            this.backfaceCullingCheckbox.Location = new System.Drawing.Point(6, 19);
            this.backfaceCullingCheckbox.Name = "backfaceCullingCheckbox";
            this.backfaceCullingCheckbox.Size = new System.Drawing.Size(138, 17);
            this.backfaceCullingCheckbox.TabIndex = 2;
            this.backfaceCullingCheckbox.Text = "Obcinanie ścian tylnych";
            this.backfaceCullingCheckbox.UseVisualStyleBackColor = true;
            this.backfaceCullingCheckbox.CheckedChanged += new System.EventHandler(this.backfaceCullingCheckbox_CheckedChanged);
            // 
            // zBufferingCheckbox
            // 
            this.zBufferingCheckbox.AutoSize = true;
            this.zBufferingCheckbox.Location = new System.Drawing.Point(6, 42);
            this.zBufferingCheckbox.Name = "zBufferingCheckbox";
            this.zBufferingCheckbox.Size = new System.Drawing.Size(112, 17);
            this.zBufferingCheckbox.TabIndex = 1;
            this.zBufferingCheckbox.Text = "Buforowanie głębi";
            this.zBufferingCheckbox.UseVisualStyleBackColor = true;
            this.zBufferingCheckbox.CheckedChanged += new System.EventHandler(this.zBufferingCheckbox_CheckedChanged);
            // 
            // lightingModelCheckbox
            // 
            this.lightingModelCheckbox.AutoSize = true;
            this.lightingModelCheckbox.Location = new System.Drawing.Point(6, 64);
            this.lightingModelCheckbox.Name = "lightingModelCheckbox";
            this.lightingModelCheckbox.Size = new System.Drawing.Size(80, 17);
            this.lightingModelCheckbox.TabIndex = 0;
            this.lightingModelCheckbox.Text = "Oświetlenie";
            this.lightingModelCheckbox.UseVisualStyleBackColor = true;
            this.lightingModelCheckbox.CheckedChanged += new System.EventHandler(this.lightingModelCheckbox_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.fpsLabel);
            this.groupBox7.Location = new System.Drawing.Point(3, 437);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(200, 58);
            this.groupBox7.TabIndex = 4;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "FPS";
            // 
            // fpsLabel
            // 
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Font = new System.Drawing.Font("Arial Black", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fpsLabel.ForeColor = System.Drawing.Color.Red;
            this.fpsLabel.Location = new System.Drawing.Point(12, 16);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(0, 28);
            this.fpsLabel.TabIndex = 0;
            this.fpsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.offAnimationRadio);
            this.groupBox8.Controls.Add(this.oneFpsAnimationRadio);
            this.groupBox8.Controls.Add(this.normalAnimationRadio);
            this.groupBox8.Location = new System.Drawing.Point(3, 501);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(200, 71);
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Animacja";
            // 
            // offAnimationRadio
            // 
            this.offAnimationRadio.AutoSize = true;
            this.offAnimationRadio.Location = new System.Drawing.Point(6, 42);
            this.offAnimationRadio.Name = "offAnimationRadio";
            this.offAnimationRadio.Size = new System.Drawing.Size(123, 17);
            this.offAnimationRadio.TabIndex = 2;
            this.offAnimationRadio.Text = "Animacja wyłączona";
            this.offAnimationRadio.UseVisualStyleBackColor = true;
            this.offAnimationRadio.CheckedChanged += new System.EventHandler(this.offAnimationRadio_CheckedChanged);
            // 
            // oneFpsAnimationRadio
            // 
            this.oneFpsAnimationRadio.AutoSize = true;
            this.oneFpsAnimationRadio.Location = new System.Drawing.Point(105, 20);
            this.oneFpsAnimationRadio.Name = "oneFpsAnimationRadio";
            this.oneFpsAnimationRadio.Size = new System.Drawing.Size(75, 17);
            this.oneFpsAnimationRadio.TabIndex = 1;
            this.oneFpsAnimationRadio.TabStop = true;
            this.oneFpsAnimationRadio.Text = "Tryb 1FPS";
            this.oneFpsAnimationRadio.UseVisualStyleBackColor = true;
            this.oneFpsAnimationRadio.CheckedChanged += new System.EventHandler(this.oneFpsAnimationRadio_CheckedChanged);
            // 
            // normalAnimationRadio
            // 
            this.normalAnimationRadio.AutoSize = true;
            this.normalAnimationRadio.Checked = true;
            this.normalAnimationRadio.Location = new System.Drawing.Point(6, 20);
            this.normalAnimationRadio.Name = "normalAnimationRadio";
            this.normalAnimationRadio.Size = new System.Drawing.Size(91, 17);
            this.normalAnimationRadio.TabIndex = 0;
            this.normalAnimationRadio.TabStop = true;
            this.normalAnimationRadio.Text = "Tryb normalny";
            this.normalAnimationRadio.UseVisualStyleBackColor = true;
            this.normalAnimationRadio.CheckedChanged += new System.EventHandler(this.normalAnimationRadio_CheckedChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox4, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(223, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(116, 575);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lightsListBox);
            this.groupBox4.Controls.Add(this.lightsMenuStrip);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 385);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(110, 187);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Światła";
            // 
            // lightsListBox
            // 
            this.lightsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightsListBox.FormattingEnabled = true;
            this.lightsListBox.Location = new System.Drawing.Point(3, 40);
            this.lightsListBox.Name = "lightsListBox";
            this.lightsListBox.Size = new System.Drawing.Size(104, 144);
            this.lightsListBox.TabIndex = 2;
            this.lightsListBox.SelectedIndexChanged += new System.EventHandler(this.lightsListBox_SelectedIndexChanged);
            // 
            // lightsMenuStrip
            // 
            this.lightsMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLightMenuItem,
            this.deleteLightMenuItem});
            this.lightsMenuStrip.Location = new System.Drawing.Point(3, 16);
            this.lightsMenuStrip.Name = "lightsMenuStrip";
            this.lightsMenuStrip.Size = new System.Drawing.Size(104, 24);
            this.lightsMenuStrip.TabIndex = 1;
            this.lightsMenuStrip.Text = "menuStrip2";
            // 
            // addLightMenuItem
            // 
            this.addLightMenuItem.Name = "addLightMenuItem";
            this.addLightMenuItem.Size = new System.Drawing.Size(50, 20);
            this.addLightMenuItem.Text = "Dodaj";
            this.addLightMenuItem.Click += new System.EventHandler(this.addLightMenuItem_Click);
            // 
            // deleteLightMenuItem
            // 
            this.deleteLightMenuItem.Name = "deleteLightMenuItem";
            this.deleteLightMenuItem.Size = new System.Drawing.Size(46, 20);
            this.deleteLightMenuItem.Text = "Usuń";
            this.deleteLightMenuItem.Click += new System.EventHandler(this.deleteLightMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.figuresListBox);
            this.groupBox1.Controls.Add(this.figuresMenuStrip);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(110, 185);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Figury";
            // 
            // figuresListBox
            // 
            this.figuresListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.figuresListBox.FormattingEnabled = true;
            this.figuresListBox.Location = new System.Drawing.Point(3, 40);
            this.figuresListBox.Name = "figuresListBox";
            this.figuresListBox.Size = new System.Drawing.Size(104, 142);
            this.figuresListBox.TabIndex = 1;
            this.figuresListBox.SelectedIndexChanged += new System.EventHandler(this.figuresListBox_SelectedIndexChanged);
            // 
            // figuresMenuStrip
            // 
            this.figuresMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFigureMenuItem,
            this.deleteFigureMenuItem});
            this.figuresMenuStrip.Location = new System.Drawing.Point(3, 16);
            this.figuresMenuStrip.Name = "figuresMenuStrip";
            this.figuresMenuStrip.Size = new System.Drawing.Size(104, 24);
            this.figuresMenuStrip.TabIndex = 0;
            this.figuresMenuStrip.Text = "menuStrip1";
            // 
            // addFigureMenuItem
            // 
            this.addFigureMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCuboidMenuItem,
            this.addSphereMenuItem,
            this.addCylinderMenuItem,
            this.addConeMenuItem});
            this.addFigureMenuItem.Name = "addFigureMenuItem";
            this.addFigureMenuItem.Size = new System.Drawing.Size(50, 20);
            this.addFigureMenuItem.Text = "Dodaj";
            // 
            // addCuboidMenuItem
            // 
            this.addCuboidMenuItem.Name = "addCuboidMenuItem";
            this.addCuboidMenuItem.Size = new System.Drawing.Size(165, 22);
            this.addCuboidMenuItem.Text = "Prostopadłościan";
            this.addCuboidMenuItem.Click += new System.EventHandler(this.addCuboidMenuItem_Click);
            // 
            // addSphereMenuItem
            // 
            this.addSphereMenuItem.Name = "addSphereMenuItem";
            this.addSphereMenuItem.Size = new System.Drawing.Size(165, 22);
            this.addSphereMenuItem.Text = "Sfera";
            this.addSphereMenuItem.Click += new System.EventHandler(this.addSphereMenuItem_Click);
            // 
            // addCylinderMenuItem
            // 
            this.addCylinderMenuItem.Name = "addCylinderMenuItem";
            this.addCylinderMenuItem.Size = new System.Drawing.Size(165, 22);
            this.addCylinderMenuItem.Text = "Walec";
            this.addCylinderMenuItem.Click += new System.EventHandler(this.addCylinderMenuItem_Click);
            // 
            // addConeMenuItem
            // 
            this.addConeMenuItem.Name = "addConeMenuItem";
            this.addConeMenuItem.Size = new System.Drawing.Size(165, 22);
            this.addConeMenuItem.Text = "Stożek";
            this.addConeMenuItem.Click += new System.EventHandler(this.addConeMenuItem_Click);
            // 
            // deleteFigureMenuItem
            // 
            this.deleteFigureMenuItem.Name = "deleteFigureMenuItem";
            this.deleteFigureMenuItem.Size = new System.Drawing.Size(46, 20);
            this.deleteFigureMenuItem.Text = "Usuń";
            this.deleteFigureMenuItem.Click += new System.EventHandler(this.deleteFigureMenuItem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.camerasListBox);
            this.groupBox3.Controls.Add(this.camerasMenuStrip);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 194);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(110, 185);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Kamery";
            // 
            // camerasListBox
            // 
            this.camerasListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camerasListBox.FormattingEnabled = true;
            this.camerasListBox.Location = new System.Drawing.Point(3, 40);
            this.camerasListBox.Name = "camerasListBox";
            this.camerasListBox.Size = new System.Drawing.Size(104, 142);
            this.camerasListBox.TabIndex = 2;
            this.camerasListBox.SelectedIndexChanged += new System.EventHandler(this.camerasListBox_SelectedIndexChanged);
            // 
            // camerasMenuStrip
            // 
            this.camerasMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCameraMenuItem,
            this.deleteCameraMenuItem});
            this.camerasMenuStrip.Location = new System.Drawing.Point(3, 16);
            this.camerasMenuStrip.Name = "camerasMenuStrip";
            this.camerasMenuStrip.Size = new System.Drawing.Size(104, 24);
            this.camerasMenuStrip.TabIndex = 1;
            this.camerasMenuStrip.Text = "menuStrip1";
            // 
            // addCameraMenuItem
            // 
            this.addCameraMenuItem.Name = "addCameraMenuItem";
            this.addCameraMenuItem.Size = new System.Drawing.Size(50, 20);
            this.addCameraMenuItem.Text = "Dodaj";
            this.addCameraMenuItem.Click += new System.EventHandler(this.addCameraMenuItem_Click);
            // 
            // deleteCameraMenuItem
            // 
            this.deleteCameraMenuItem.Name = "deleteCameraMenuItem";
            this.deleteCameraMenuItem.Size = new System.Drawing.Size(46, 20);
            this.deleteCameraMenuItem.Text = "Usuń";
            this.deleteCameraMenuItem.Click += new System.EventHandler(this.deleteCameraMenuItem_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 40;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Title = "Zapisz plik ze sceną:";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Title = "Otwórz plik ze sceną:";
            // 
            // fpsTimer
            // 
            this.fpsTimer.Enabled = true;
            this.fpsTimer.Interval = 1000;
            this.fpsTimer.Tick += new System.EventHandler(this.fpsTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 581);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MainMenuStrip = this.figuresMenuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GK Projekt 4";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.lightsMenuStrip.ResumeLayout(false);
            this.lightsMenuStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.figuresMenuStrip.ResumeLayout(false);
            this.figuresMenuStrip.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.camerasMenuStrip.ResumeLayout(false);
            this.camerasMenuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button openFileBtn;
        private System.Windows.Forms.Button saveFileBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.MenuStrip figuresMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addFigureMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addCuboidMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSphereMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addCylinderMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addConeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFigureMenuItem;
        private System.Windows.Forms.ListBox figuresListBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.MenuStrip camerasMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addCameraMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteCameraMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox lightsListBox;
        private System.Windows.Forms.MenuStrip lightsMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addLightMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteLightMenuItem;
        private System.Windows.Forms.ListBox camerasListBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox perspectiveCorrectionCheckbox;
        private System.Windows.Forms.CheckBox backfaceCullingCheckbox;
        private System.Windows.Forms.CheckBox zBufferingCheckbox;
        private System.Windows.Forms.CheckBox lightingModelCheckbox;
        private System.Windows.Forms.Timer fpsTimer;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton offAnimationRadio;
        private System.Windows.Forms.RadioButton oneFpsAnimationRadio;
        private System.Windows.Forms.RadioButton normalAnimationRadio;
    }
}

