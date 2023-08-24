using ReaLTaiizor.Colors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ReaLTaiizor.Forms;
using ReaLTaiizor.Controls;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Projeto4
{
    public partial class Cadastro : MaterialForm
    {
        bool isAlteracao = false;
        string cs = @"server=localhost;uid=root;pwd=;database=academico";
        public Cadastro()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidarFormulario())
            {
                Salvar();
                materialTabControl1.SelectedIndex = 1;
            }
        }

        private void Salvar()
        {
            var con = new MySqlConnection(cs);
            con.Open();
            if (!isAlteracao)
            {

                var sql = "INSERT INTO aluno(matricula, data_nasc, nome, endereco, bairro, cidade, estado, senha) VALUES (@matricula, @data_nasc, @nome, @endereco, @bairro, @cidade, @estado, @senha)";
                var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@matricula", txtMatricula.Text);
                DateTime.TryParse(txtData.Text, out var dataNascimento);
                cmd.Parameters.AddWithValue("@data_nasc", dataNascimento);
                cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@endereco", txtEndereco.Text);
                cmd.Parameters.AddWithValue("@bairro", txtBairro.Text);
                cmd.Parameters.AddWithValue("@cidade", txtCidade.Text);
                cmd.Parameters.AddWithValue("@estado", cmbEstado.Text);
                cmd.Parameters.AddWithValue("@senha", txtSenha.Text);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

            }
            else
            {

            }
            LimpaCampos();
        }
        private bool ValidarFormulario()
        {
            if (string.IsNullOrEmpty(txtMatricula.Text))
            {
                MessageBox.Show("Matricula é obrigatória", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatricula.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtNome.Text))
            {
                MessageBox.Show("Nome é obrigatório", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNome.Focus();
                return false;
            }
            if (!DateTime.TryParse(txtData.Text, out DateTime _))
            {
                MessageBox.Show("Data é obrigatória", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtData.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtEndereco.Text))
            {
                MessageBox.Show("Endereço é obrigatório", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEndereco.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtBairro.Text))
            {
                MessageBox.Show("Bairro é obrigatório", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBairro.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCidade.Text))
            {
                MessageBox.Show("Cidade é obrigatória", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCidade.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtSenha.Text))
            {
                MessageBox.Show("Senha é obrigatória", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSenha.Focus();
                return false;
            }
            return true;
        }
        private void LimpaCampos()
        {
            isAlteracao = false;
            foreach (var control in tabPage1.Controls)
            {
                if (control is MaterialTextBoxEdit)
                {
                    ((MaterialTextBoxEdit)control).Clear();
                }
                if (control is MaterialMaskedTextBox)
                {
                    ((MaterialMaskedTextBox)control).Clear();
                }
            }
        }

        private void CarregaGrid()
        {
            var con = new MySqlConnection(cs);
            con.Open();
            var sql = "SELECT * FROM aluno";
            var sqlAd = new MySqlDataAdapter();
            sqlAd.SelectCommand = new MySqlCommand(sql, con);
            var dt = new DataTable();
            sqlAd.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_Enter(object sender, EventArgs e)
        {
            CarregaGrid();

        }
    }
}
