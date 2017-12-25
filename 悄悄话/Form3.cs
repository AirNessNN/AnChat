using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 悄悄话 {
	public partial class 确认密码 : Form {

		private string password = null;

		public 确认密码 (string tex) {
			InitializeComponent();
			this.password = tex;
			Console.WriteLine( password.ToString() );
		}

		private void button1_Click ( object sender, EventArgs e ) {
			if(textBox1.Text!=password){
				MessageBox.Show( "密码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information );
			}else {
				this.DialogResult = DialogResult.OK;
			}
		}
	}
}
