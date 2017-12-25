using System;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace 悄悄话 {
	public partial class Form1 : Form {

		//字段
		private Content content = new Content();



		public Form1 () {
			InitializeComponent();
            this.AllowDrop = true;
		}

		private void btnAbout_Click ( object sender, EventArgs e ) {
			Form form = new Form2();
			form.ShowDialog();
		}

		private void btnClear_Click ( object sender, EventArgs e ) {
			page1.Text = "";
		}
		//翻译的动作事件
		private void btnTranslate_Click ( object sender, EventArgs e ) {

			string dir = null;
			
			//加密
			if (page1.Text == "") {
				MessageBox.Show( "请输入想要翻译的文字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				return;
			} else {
				FolderBrowserDialog fd = new FolderBrowserDialog();
				if (fd.ShowDialog() == DialogResult.OK) {
					dir = fd.SelectedPath;
					if (name.Text == "") {
						dir += "\\pillowTalk.ans";
					} else {
						dir += "\\" + name.Text + ".ans";
					}
				}

				if (checkBox.CheckState == CheckState.Checked) {
					if (passwordTextBox.Text == "") {
						MessageBox.Show( "请输入配对秘钥", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
						return;
					}
					if (passwordTextBox.Text.Length > 6) {
						MessageBox.Show( "秘钥不得大于6位数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
						return;
					}
					content.Index = page1.Text;
					content.Password = passwordTextBox.Text;
					serialize( content ,dir);

				} else {
					content.Index = page1.Text;
					serialize( content, dir );
				}
			}
			
		}

		//加密文字
		private void serialize (Content content,string dir) {
			//序列化对象
			Stream fSteam = new FileStream( dir, FileMode.Create, FileAccess.ReadWrite );
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize( fSteam, content );
			fSteam.Close();
			MessageBox.Show( "加密完成，文件目录"+dir, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information );
		}

		
		private void checkBox_CheckedChanged ( object sender, EventArgs e ) {
			if(checkBox.CheckState==CheckState.Checked) {
				passwordTextBox.Enabled = true;
			}else {
				passwordTextBox.Enabled = false;
			}
		}

		private void btnRead_Click ( object sender, EventArgs e ) {

			page1.Clear();

			string dir=null;
			OpenFileDialog od = new OpenFileDialog();
			od.Multiselect = false;
			od.Title = "请选择文件";
			if (od.ShowDialog() == DialogResult.OK) {
				dir = od.FileName;
				Stream fSteam = new FileStream( dir, FileMode.Open, FileAccess.Read );
				BinaryFormatter bf = new BinaryFormatter();
				try {
					Content con = (Content)bf.Deserialize( fSteam );
					
					if(!con.isEmptyPassword) {
						确认密码 f3 = new 确认密码(con.Password);
						f3.ShowDialog();
						if(f3.DialogResult==DialogResult.OK) {
							page1.Text = con.Index;
						}
					}else {
						MessageBox.Show( "读取完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information );
						page1.Text = con.Index;
					}
				}catch(System.Runtime.Serialization.SerializationException) {
					MessageBox.Show( "反序列化失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error );
				}
			}
		}

        private void Form1_DragDrop(object sender, DragEventArgs e) {
            page1.Clear();
            string dir = null;
            string[] dirs = (string[])e.Data.GetData(DataFormats.FileDrop);
            dir = dirs[0];
            if (Path.GetExtension(dir) == ".ans") {
                Stream fSteam = new FileStream(dir, FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                try {
                    Content con = (Content)bf.Deserialize(fSteam);

                    if (!con.isEmptyPassword) {
                        确认密码 f3 = new 确认密码(con.Password);
                        f3.ShowDialog();
                        if (f3.DialogResult == DialogResult.OK) {
                            page1.Text = con.Index;
                        }
                    } else {
                        MessageBox.Show("读取完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        page1.Text = con.Index;
                    }
                } catch (System.Runtime.Serialization.SerializationException) {
                    MessageBox.Show("反序列化失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
            }
        }
    }

    //加密类
    [Serializable]
	public class Content {

	//字段
		private Boolean PasswordFlag = true;

		private String password;

		private String content;

		public String Password {

			get {
				if(PasswordFlag) {
					return null;
				}else {
					return password;
				}
			}
			set {
				password = value;
				PasswordFlag = false;
			}
		}

		public String Index {

			get {
				if (  content== null) {
					return null;
				} else {
					return content;
				}
			}

			set {
				content = value;
			}
		}

		public Boolean isEmptyPassword {
			get {
				return PasswordFlag;
			}
		}
	}
}

