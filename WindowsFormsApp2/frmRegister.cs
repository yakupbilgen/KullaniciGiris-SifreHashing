using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace WindowsFormsApp2
{
	public partial class frmRegister : Form
	{
		SqlConnection ctx = new SqlConnection("data source=DESKTOP-LU5VIM4\\MSSQLSERVER1;initial catalog=DBHashingSalting;integrated security=True");

		public frmRegister()
		{
			InitializeComponent();
		}

		private void frmRegister_Load(object sender, EventArgs e)
		{
			txtPassword.PasswordChar = '*';
		}

		private void cbPasswordEnable_CheckedChanged(object sender, EventArgs e)
		{
			if (cbPasswordEnable.Checked == true)
			{
				txtPassword.PasswordChar = '\0';
			}
			else
			{
				txtPassword.PasswordChar = '*';
			}
		}

		private void btnRegister_Click(object sender, EventArgs e)
		{
			try
			{
				if (ctx.State == ConnectionState.Open)
				{
					ctx.Close();
				}
				else
				{
					ctx.Open();

					SHA1 sha = new SHA1CryptoServiceProvider();
					string sifrelenecekveri = txtPassword.Text;
					string sifrelenmisveri = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(sifrelenecekveri)));

					SqlCommand cmd = new SqlCommand("INSERT INTO TBLLogin (username,password) VALUES (@username,@password)", ctx);
					cmd.Parameters.AddWithValue("@username", txtUsername.Text);
					cmd.Parameters.AddWithValue("@password", sifrelenmisveri);
					cmd.ExecuteNonQuery();
					this.Hide();
					frmLogin frm = new frmLogin();
					frm.Show();
					ctx.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
