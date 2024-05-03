namespace ZTest01
{
	partial class ZTest
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
			this.btnTestMix = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnTestMix
			// 
			this.btnTestMix.Location = new System.Drawing.Point(50, 42);
			this.btnTestMix.Name = "btnTestMix";
			this.btnTestMix.Size = new System.Drawing.Size(180, 55);
			this.btnTestMix.TabIndex = 0;
			this.btnTestMix.Text = "TestMix";
			this.btnTestMix.UseVisualStyleBackColor = true;
			this.btnTestMix.Click += new System.EventHandler(this.btnTestMix_Click);
			// 
			// ZTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(626, 344);
			this.Controls.Add(this.btnTestMix);
			this.Name = "ZTest";
			this.Text = "ZTest";
			this.Load += new System.EventHandler(this.ZTest_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnTestMix;
	}
}

