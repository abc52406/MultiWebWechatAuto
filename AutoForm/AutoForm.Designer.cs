namespace AutoForm
{
    partial class AutoForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelWait = new System.Windows.Forms.Label();
            this.labelQR = new System.Windows.Forms.Label();
            this.labelLogin = new System.Windows.Forms.Label();
            this.labelCheck = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelWait
            // 
            this.labelWait.AutoSize = true;
            this.labelWait.Location = new System.Drawing.Point(59, 28);
            this.labelWait.Name = "labelWait";
            this.labelWait.Size = new System.Drawing.Size(65, 12);
            this.labelWait.TabIndex = 0;
            this.labelWait.Text = "等待中粉丝";
            // 
            // labelQR
            // 
            this.labelQR.AutoSize = true;
            this.labelQR.Location = new System.Drawing.Point(59, 53);
            this.labelQR.Name = "labelQR";
            this.labelQR.Size = new System.Drawing.Size(65, 12);
            this.labelQR.TabIndex = 1;
            this.labelQR.Text = "待扫码粉丝";
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.Location = new System.Drawing.Point(59, 77);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(65, 12);
            this.labelLogin.TabIndex = 2;
            this.labelLogin.Text = "待登录粉丝";
            // 
            // labelCheck
            // 
            this.labelCheck.AutoSize = true;
            this.labelCheck.Location = new System.Drawing.Point(59, 102);
            this.labelCheck.Name = "labelCheck";
            this.labelCheck.Size = new System.Drawing.Size(65, 12);
            this.labelCheck.TabIndex = 3;
            this.labelCheck.Text = "检测中粉丝";
            // 
            // AutoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 145);
            this.Controls.Add(this.labelCheck);
            this.Controls.Add(this.labelLogin);
            this.Controls.Add(this.labelQR);
            this.Controls.Add(this.labelWait);
            this.Name = "AutoForm";
            this.Text = "微信网页轮询工具";
            this.Load += new System.EventHandler(this.AutoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWait;
        private System.Windows.Forms.Label labelQR;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.Label labelCheck;
    }
}

