
namespace ConsumerApp
{
    partial class Consumer
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
            listBox1 = new System.Windows.Forms.ListBox();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 17;
            listBox1.Location = new System.Drawing.Point(8, 8);
            listBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(336, 191);
            listBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(353, 8);
            button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(141, 24);
            button1.TabIndex = 1;
            button1.Text = "开启 MQ 监听";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(353, 54);
            button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(141, 24);
            button2.TabIndex = 2;
            button2.Text = "关闭 MQ 监听";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Button2_Click;
            // 
            // Consumer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(505, 209);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(listBox1);
            Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            Name = "Consumer";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "消费者客户端";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;

        /// <summary>
        /// 开启 MQ 监听
        /// </summary>
        private System.Windows.Forms.Button button1;

        /// <summary>
        /// 关闭 MQ 监听
        /// </summary>
        private System.Windows.Forms.Button button2;
    }
}

