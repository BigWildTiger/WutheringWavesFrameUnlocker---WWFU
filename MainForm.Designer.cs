
using Microsoft.Win32;
using System.Reflection;
using System.Windows.Forms;

namespace WutheringWavesFrameUnlocker___WWFU
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			textBox1 = new TextBox();
			openFileDialog1 = new OpenFileDialog();
			button1 = new Button();
			button2 = new Button();
			radioButton1 = new RadioButton();
			radioButton2 = new RadioButton();
			radioButton3 = new RadioButton();
			radioButton4 = new RadioButton();
			groupBox1 = new GroupBox();
			radioButton5 = new RadioButton();
			textBox2 = new TextBox();
			timer1 = new System.Windows.Forms.Timer(components);
			button3 = new Button();
			button4 = new Button();
			label1 = new Label();
			label2 = new Label();
			groupBox1.SuspendLayout();
			SuspendLayout();
			// 
			// textBox1
			// 
			textBox1.Location = new Point(12, 12);
			textBox1.Multiline = true;
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(566, 93);
			textBox1.TabIndex = 1;
			// 
			// openFileDialog1
			// 
			openFileDialog1.FileName = "openFileDialog1";
			// 
			// button1
			// 
			button1.Location = new Point(328, 216);
			button1.Name = "button1";
			button1.Size = new Size(132, 43);
			button1.TabIndex = 2;
			button1.Text = "명조 실행파일 선택";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// button2
			// 
			button2.Enabled = false;
			button2.Location = new Point(466, 216);
			button2.Name = "button2";
			button2.Size = new Size(112, 43);
			button2.TabIndex = 3;
			button2.Text = "주사율 설정 저장";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// radioButton1
			// 
			radioButton1.AutoSize = true;
			radioButton1.Location = new Point(28, 32);
			radioButton1.Name = "radioButton1";
			radioButton1.Size = new Size(54, 19);
			radioButton1.TabIndex = 4;
			radioButton1.TabStop = true;
			radioButton1.Tag = "30";
			radioButton1.Text = "30Hz";
			radioButton1.UseVisualStyleBackColor = true;
			radioButton1.CheckedChanged += radioButton1_CheckedChanged;
			// 
			// radioButton2
			// 
			radioButton2.AutoSize = true;
			radioButton2.Location = new Point(122, 32);
			radioButton2.Name = "radioButton2";
			radioButton2.Size = new Size(54, 19);
			radioButton2.TabIndex = 5;
			radioButton2.TabStop = true;
			radioButton2.Tag = "60";
			radioButton2.Text = "60Hz";
			radioButton2.UseVisualStyleBackColor = true;
			radioButton2.CheckedChanged += radioButton2_CheckedChanged;
			// 
			// radioButton3
			// 
			radioButton3.AutoSize = true;
			radioButton3.Location = new Point(216, 32);
			radioButton3.Name = "radioButton3";
			radioButton3.Size = new Size(61, 19);
			radioButton3.TabIndex = 6;
			radioButton3.TabStop = true;
			radioButton3.Tag = "100";
			radioButton3.Text = "100Hz";
			radioButton3.UseVisualStyleBackColor = true;
			radioButton3.CheckedChanged += radioButton3_CheckedChanged;
			// 
			// radioButton4
			// 
			radioButton4.AutoSize = true;
			radioButton4.Location = new Point(317, 32);
			radioButton4.Name = "radioButton4";
			radioButton4.Size = new Size(61, 19);
			radioButton4.TabIndex = 7;
			radioButton4.TabStop = true;
			radioButton4.Tag = "120";
			radioButton4.Text = "120Hz";
			radioButton4.UseVisualStyleBackColor = true;
			radioButton4.CheckedChanged += radioButton4_CheckedChanged;
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(radioButton1);
			groupBox1.Controls.Add(radioButton5);
			groupBox1.Controls.Add(radioButton4);
			groupBox1.Controls.Add(radioButton2);
			groupBox1.Controls.Add(radioButton3);
			groupBox1.Location = new Point(12, 116);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(566, 76);
			groupBox1.TabIndex = 8;
			groupBox1.TabStop = false;
			groupBox1.Text = "주사율 설정";
			// 
			// radioButton5
			// 
			radioButton5.AutoSize = true;
			radioButton5.Location = new Point(418, 32);
			radioButton5.Name = "radioButton5";
			radioButton5.Size = new Size(61, 19);
			radioButton5.TabIndex = 8;
			radioButton5.TabStop = true;
			radioButton5.Tag = "144";
			radioButton5.Text = "144Hz";
			radioButton5.UseVisualStyleBackColor = true;
			radioButton5.CheckedChanged += radioButton5_CheckedChanged;
			// 
			// textBox2
			// 
			textBox2.Location = new Point(12, 302);
			textBox2.Multiline = true;
			textBox2.Name = "textBox2";
			textBox2.ScrollBars = ScrollBars.Both;
			textBox2.Size = new Size(566, 123);
			textBox2.TabIndex = 9;
			// 
			// timer1
			// 
			timer1.Enabled = true;
			timer1.Interval = 50;
			timer1.Tick += timer1_Tick;
			// 
			// button3
			// 
			button3.Location = new Point(12, 268);
			button3.Name = "button3";
			button3.Size = new Size(535, 23);
			button3.TabIndex = 10;
			button3.Text = "Log..";
			button3.UseVisualStyleBackColor = true;
			button3.Click += button3_Click;
			// 
			// button4
			// 
			button4.Location = new Point(551, 268);
			button4.Name = "button4";
			button4.Size = new Size(27, 23);
			button4.TabIndex = 11;
			button4.Text = "c";
			button4.UseVisualStyleBackColor = true;
			button4.Click += button4_Click;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(429, 198);
			label1.Name = "label1";
			label1.Size = new Size(122, 15);
			label1.TabIndex = 12;
			label1.Text = "현재 저장된 주사율 : ";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(40, 230);
			label2.Name = "label2";
			label2.Size = new Size(184, 15);
			label2.TabIndex = 13;
			label2.Text = "프레임 설정 프로그램 by Cityhall";
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(590, 437);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(button4);
			Controls.Add(button3);
			Controls.Add(textBox2);
			Controls.Add(groupBox1);
			Controls.Add(button2);
			Controls.Add(button1);
			Controls.Add(textBox1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			Name = "MainForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "WutheringWavesFrameUnlocker - WWFU";
			FormClosing += MainForm_FormClosing;
			FormClosed += MainForm_FormClosed;
			Load += MainForm_Load;
			Shown += MainForm_Shown;
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private TextBox textBox1;
		private OpenFileDialog openFileDialog1;
		private Button button1;
		private Button button2;
		private RadioButton radioButton1;
		private RadioButton radioButton2;
		private RadioButton radioButton3;
		private RadioButton radioButton4;
		private GroupBox groupBox1;
		private RadioButton radioButton5;
		private TextBox textBox2;
		private System.Windows.Forms.Timer timer1;
		private Button button3;
		private Button button4;
		private Label label1;
		private Label label2;
	}
}
