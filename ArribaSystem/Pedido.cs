using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArribaSystem
{
    public partial class frmPedido : Form
    {
        public frmPedido()
        {
            InitializeComponent();
        }

        //INSTANCIANDO  A CLASSE CONEXÃO
        Conexao con = new Conexao();

        private void cmbTamanhoTaco_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void grpOpcionais_Enter(object sender, EventArgs e)
        {
            cmbTamanhoPedido.SelectedIndex = -1;
            txtValorPedido.Clear();
            txtValorOpcionais.Clear();
            chkQueijoDerretido.Checked = false;
            chkPimenta.Checked = false;
            chkNachos.Checked = false;
            chkGuacamole.Checked = false;
        }

        private void chkChile_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmPedido_Load(object sender, EventArgs e)
        {
            {
                cmbTamanhoPedido.Items.Add("Burrito de Carne");
                cmbTamanhoPedido.Items.Add("Taco de Carne");
                cmbTamanhoPedido.Items.Add("Combo de Tortillas");
                cmbTamanhoPedido.Items.Add("Polvoron");
                cmbTamanhoPedido.Items.Add("Enchiladas");
                cmbTamanhoPedido.Items.Add("Quesadillas");
            }
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            double valorPedido = 0, valorOpcao = 0, valorTotal = 0;

            if (cmbTamanhoPedido.SelectedIndex == 1)
            {
                valorPedido = 27;
            }
            else if (cmbTamanhoPedido.SelectedIndex == 2)
            {
                valorPedido = 19;
            }
            else if (cmbTamanhoPedido.SelectedIndex == 3)
            {
                valorPedido = 21;
            } 
            else if (cmbTamanhoPedido.SelectedIndex == 4){
                valorPedido = 8;
            } 
            else if (cmbTamanhoPedido.SelectedIndex == 5)
            {
                valorPedido = 17;
            }
            else if (cmbTamanhoPedido.SelectedIndex == 6)
            {
                valorPedido = 10;
            }

            if (chkQueijoDerretido.Checked == true)
            {
                valorOpcao = valorOpcao + 5;
            }
            if (chkPimenta.Checked == true)
            {
                valorOpcao = valorOpcao + 6;
            }
            if (chkNachos.Checked == true)
            {
                valorOpcao = valorOpcao + 3;
            }
            if (chkGuacamole.Checked == true)
            {
                valorOpcao = valorOpcao + 6;

            }
            
            else
            {
                //MessageBox.Show("Calculo");
            }
            valorTotal = valorPedido + valorOpcao;


            //Concatenação do preço com o BRL
            txtValorPedido.Text = Convert.ToString(valorPedido + " reais");
            txtValorOpcionais.Text = Convert.ToString(valorOpcao + " reais");
            txtValorTotal.Text = Convert.ToString(valorTotal + " reais");
        }

        //Botão Novo
        private void btnNovo_Click(object sender, EventArgs e)
        {
            chkQueijoDerretido.Checked = false;
            chkPimenta.Checked = false;
            chkGuacamole.Checked = false;
            chkNachos.Checked = false;
            
            txtValorOpcionais.Clear();
            txtValorPedido.Clear();
            txtValorTotal.Clear();
            cmbTamanhoPedido.SelectedIndex = 0;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            //verifica os campos 
            if (cmbTamanhoPedido.Text == "")
            {
                MessageBox.Show("Obrigatório Preencher o campo nome");
                cmbTamanhoPedido.Focus();
            }
            else
            {

                try
                {
                    //insere no banco de dados
                    string sql = "insert into tbPedido(ComidaSelecionada, valorComida, ValorAcompanhamentos, ValorFinal)values(@pedido,@vpedido,@opcao,@valor)";
                    MySqlCommand cmd = new MySqlCommand(sql, con.ConnectarBD());
                    cmd.Parameters.Add("@pedido", MySqlDbType.Text).Value = cmbTamanhoPedido.Text;
                    cmd.Parameters.Add("@vpedido", MySqlDbType.Text).Value = txtValorPedido.Text;
                    cmd.Parameters.Add("@opcao", MySqlDbType.Text).Value = txtValorOpcionais.Text;
                    cmd.Parameters.Add("@valor", MySqlDbType.Text).Value = txtValorTotal.Text;
                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Dados cadastrados com Sucesso !!!");
                    cmbTamanhoPedido.Text = "";
                    txtValorPedido.Text = "";
                    txtValorOpcionais.Text = "";
                    txtValorTotal.Text = "";
                    cmbTamanhoPedido.Focus();
                    con.DesConnectarBD();

                }
                catch (Exception erro)
                {

                    MessageBox.Show(erro.Message);

                }
            }
        }

        public void CarregarPedidos()
        {
            try
            {
                txtPesquisarCod.Text = dgvPedido.SelectedRows[0].Cells[0].Value.ToString(); //valor da posição da memoria  0 0
                cmbTamanhoPedido.Text = dgvPedido.SelectedRows[0].Cells[1].Value.ToString();//valor da posição da memoria  0 1
                txtValorPedido.Text = dgvPedido.SelectedRows[0].Cells[2].Value.ToString();//valor da posição da memoria  0 2
                txtValorOpcionais.Text = dgvPedido.SelectedRows[0].Cells[3].Value.ToString();//valor da posição da memoria  0 3
                txtValorTotal.Text = dgvPedido.SelectedRows[0].Cells[4].Value.ToString();//valor da posição da memoria  0 4
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao clicar" + erro);
            }

        }
        private void dgvPedido_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CarregarPedidos();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void txtPesquisarCod_TextChanged(object sender, EventArgs e)
        {
            if (txtPesquisarCod.Text != "")
            {
                try
                {
                    //conecta o banco
                    con.ConnectarBD();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "select * from tbpedido";

                    cmd.Connection = con.ConnectarBD();
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    DataTable dt = new DataTable();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    dgvPedido.DataSource = dt;
                    con.DesConnectarBD();
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
            }
            else
            {
                //dataGridView
                dgvPedido.DataSource = null;
            }
        }
    }
}
