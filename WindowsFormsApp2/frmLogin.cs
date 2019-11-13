using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace WindowsFormsApp2
{
	public partial class frmLogin : Form
	{
		SqlConnection ctx = new SqlConnection("data source=DESKTOP-LU5VIM4\\MSSQLSERVER1;initial catalog=DBHashingSalting;integrated security=True");
		public frmLogin()
		{
			InitializeComponent();
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			SHA1 sha = new SHA1CryptoServiceProvider();
			string sifrelenecekveri = txtPassword.Text;
			string sifrelenmisveri = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(sifrelenecekveri)));
			listBox1.Items.Add(sifrelenecekveri + " --> " + sifrelenmisveri);

			if (ctx.State == ConnectionState.Open)
			{
				ctx.Close();
			}
			else
			{
				ctx.Open();

				SqlCommand cmd = new SqlCommand("select * from TBLLogin where username=@p1 and password=@p2", ctx);
				cmd.Parameters.AddWithValue("@p1", txtUsername.Text);
				cmd.Parameters.AddWithValue("@p2", sifrelenmisveri);
				SqlDataReader dr = cmd.ExecuteReader();

				if (dr.Read())
				{
					MessageBox.Show("Giriş Başarılı!");
					frmMain frm = new frmMain();
					frm.username = txtUsername.Text;
					frm.Show();
					this.Hide();
				}
				else
				{
					MessageBox.Show("Giriş bilgileri hatalı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}

				ctx.Close();
			}
		}

		private void btnRegister_Click(object sender, EventArgs e)
		{
			frmRegister frm = new frmRegister();
			frm.Show();
			this.Hide();
		}
	}
}
