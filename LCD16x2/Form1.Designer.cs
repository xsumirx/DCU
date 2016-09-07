namespace LCD16x2
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
            this.textBoxLCD = new System.Windows.Forms.TextBox();
            this.printButton = new System.Windows.Forms.Button();
            this.checkBoxLcdClear = new System.Windows.Forms.CheckBox();
            this.lcdClearButton = new System.Windows.Forms.Button();
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem_about = new System.Windows.Forms.MenuItem();
            this.menuItem_exit = new System.Windows.Forms.MenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.collection_AutoMode = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // textBoxLCD
            // 
            this.textBoxLCD.Location = new System.Drawing.Point(15, 42);
            this.textBoxLCD.Name = "textBoxLCD";
            this.textBoxLCD.Size = new System.Drawing.Size(120, 23);
            this.textBoxLCD.TabIndex = 0;
            this.textBoxLCD.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxLCD_KeyDown);
            // 
            // printButton
            // 
            this.printButton.Location = new System.Drawing.Point(48, 71);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(72, 20);
            this.printButton.TabIndex = 1;
            this.printButton.Text = "Print";
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // checkBoxLcdClear
            // 
            this.checkBoxLcdClear.Location = new System.Drawing.Point(250, 71);
            this.checkBoxLcdClear.Name = "checkBoxLcdClear";
            this.checkBoxLcdClear.Size = new System.Drawing.Size(79, 20);
            this.checkBoxLcdClear.TabIndex = 2;
            this.checkBoxLcdClear.Text = "Clear";
            // 
            // lcdClearButton
            // 
            this.lcdClearButton.Location = new System.Drawing.Point(126, 71);
            this.lcdClearButton.Name = "lcdClearButton";
            this.lcdClearButton.Size = new System.Drawing.Size(72, 20);
            this.lcdClearButton.TabIndex = 3;
            this.lcdClearButton.Text = "Clear";
            this.lcdClearButton.Click += new System.EventHandler(this.lcdClearButton_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(this.menuItem1);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuItem_about);
            this.menuItem1.MenuItems.Add(this.menuItem_exit);
            this.menuItem1.Text = "File";
            // 
            // menuItem_about
            // 
            this.menuItem_about.Text = "about";
            this.menuItem_about.Click += new System.EventHandler(this.menuItem_about_Click);
            // 
            // menuItem_exit
            // 
            this.menuItem_exit.Text = "exit";
            this.menuItem_exit.Click += new System.EventHandler(this.menuItem_exit_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(3, 97);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(350, 77);
            this.textBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(153, 42);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(176, 23);
            this.textBox2.TabIndex = 5;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(3, 190);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(350, 23);
            this.textBox3.TabIndex = 6;
            this.textBox3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox3_KeyDown);
            // 
            // collection_AutoMode
            // 
            this.collection_AutoMode.Interval = 2000;
            this.collection_AutoMode.Tick += new System.EventHandler(this.collection_AutoMode_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(365, 226);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lcdClearButton);
            this.Controls.Add(this.checkBoxLcdClear);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.textBoxLCD);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "DPU";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxLCD;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.CheckBox checkBoxLcdClear;
        private System.Windows.Forms.Button lcdClearButton;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem_about;
        private System.Windows.Forms.MenuItem menuItem_exit;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Timer collection_AutoMode;

    }
}

