namespace WindowsFormsApp
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
			this.textBoxForName = new System.Windows.Forms.TextBox();
			this.writeHelloButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBoxForName
			// 
			this.textBoxForName.Location = new System.Drawing.Point(89, 47);
			this.textBoxForName.Name = "textBoxForName";
			this.textBoxForName.Size = new System.Drawing.Size(100, 20);
			this.textBoxForName.TabIndex = 0;
			// 
			// writeHelloButton
			// 
			this.writeHelloButton.Location = new System.Drawing.Point(100, 73);
			this.writeHelloButton.Name = "writeHelloButton";
			this.writeHelloButton.Size = new System.Drawing.Size(75, 23);
			this.writeHelloButton.TabIndex = 1;
			this.writeHelloButton.Text = "Hello";
			this.writeHelloButton.UseVisualStyleBackColor = true;
			this.writeHelloButton.Click += new System.EventHandler(this.writeHelloButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.writeHelloButton);
			this.Controls.Add(this.textBoxForName);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxForName;
		private System.Windows.Forms.Button writeHelloButton;
	}
}

