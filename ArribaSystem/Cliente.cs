using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ArribaSystem
{
    public partial class frmCliente : Form
    {
        public frmCliente()
        {
            InitializeComponent();
        }

        //INSTANCIANDO A CLASSE CONEXAO
        Conexao con = new Conexao();
        MySqlCommand cmd = new MySqlCommand();

        // METODO HABILITAR CAMPOS
        private void habilitaCampos()
        {
            txtEmail.Enabled = true;
            txtNome.Enabled = true;
            txtTelefone.Enabled = true;
            btnNovo.Enabled = false;
            btnSalvar.Enabled = true;
            btnAlterar.Enabled = false;
            txtNome.Focus(); //DEIXA A SETINHA NO CAMPO NOME
        }
        private void desabiltaCampos()
        {
            txtEmail.Enabled = true;
            txtNome.Enabled = true;
            txtTelefone.Enabled = true;
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            btnAlterar.Enabled = false;
            txtPesquisar.Clear(); //LIMPA O CAMPO PESQUISAR
            txtPesquisar.Focus();
            limpaCampos(); //CHAMANDO O METODO LIMPA CAMPOS
        }

        // LIMPA CAMPOS
        private void limpaCampos()
        {
            txtEmail.Clear();
            txtNome.Clear();
            txtTelefone.Clear();
            txtPesquisar.Clear();
            dgvCli.DataSource = null; //LIMPA O DATAGRIDVIEW
        }

        public void carregarImagem(PictureBox pbFotoCliente)
        {
            //vai filtrar o tipo de imagem 
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Imagem|*.jpg;*.doc;*.xls;*.ppt;";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                //vai ler o tamanho do arquivo selecionado
                FileInfo file = new FileInfo(openFileDialog1.FileName);


                //testa se tem menos de 1MB (1MB em bytes = 1048576)
                if (file.Length <= 1048576)
                    pbFotoCliente.ImageLocation = openFileDialog1.FileName;
                else
                    MessageBox.Show("Arquivo Maior que 1MB!");

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Cliente_Load(object sender, EventArgs e)
        {

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            habilitaCampos();
        }

        private void btnCarregar_Click(object sender, EventArgs e)
        {
            carregarImagem(pbFotoCliente);
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        private void lblPesquisar_Click(object sender, EventArgs e)
        {

        }

        private void pbFotoCliente_Click(object sender, EventArgs e)
        {

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            //VERIFICA OS CAMPOS SE ESTÃO VAZIOS
            if (txtNome.Text == "")
            {
                MessageBox.Show("Obrigatório Preencher o campo nome");
                txtNome.Focus();
            }
            else if (txtEmail.Text == "")
            {
                MessageBox.Show("Obrigatório Preencher o campo Email");
                txtEmail.Focus();
            }

            else if (txtTelefone.Text == "")
            {
                MessageBox.Show("Obrigatório Preencher o campo Telefone");
                txtTelefone.Focus();
            }
            else
            {    //tratamento de erros evita bugar todo sistema apresenta mensagem erro TRY ELE TENTA 
                try
                {
                    //VARIAVEI SQL QUE RECEBE O INSERT(VAI INSERIR OS DADOS NO BANCO com imagem)
                    string sql = "insert into tbCliente(Nome, Email, Telefone, Imagem) values(@nome,@email,@telefone,'" + pbFotoCliente.Image + "')";
                    MySqlCommand cmd = new MySqlCommand(sql, con.ConnectarBD()); //CMD IDENTIFICA OS COMANDOS DO MYSQL NO c#
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = txtNome.Text; //PEGA O VALOR DO NOME
                    cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = txtEmail.Text; // PEGA O VALOR DO TELEFONE
                    cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = txtTelefone.Text;//
                    cmd.ExecuteNonQuery();//EXECUTA A AÇÃO DE INSERIR
                    MessageBox.Show("Dados Cadastrados com Sucesso"); //APRESENTE A MENSAGEM DE SUCESSO
                    limpaCampos();//CHAMA O METODO LIMPA CAMPOS
                    con.DesConnectarBD();//DESCONECTA DO BANCO DE DADOS
                }
                //CASO NÃO CONSIGA EM VEZ DE TRAVAR O PROJETO APARECE UMA MENSAGEM
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text == "")
            {
                MessageBox.Show("Obrigatório Preencher o Campo");

            }
            else if (txtEmail.Text == "")
            {
                MessageBox.Show("Obrigatório Preencher o Campo");
            }
            else if (txtTelefone.Text == "")
            {
                MessageBox.Show("Obrigatório Preencher o Campo");

            }
            try
            {
                //FAZ O UPDATE NO BANCO DE DADOS
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con.ConnectarBD();
                cmd.CommandText = "update tbcliente set nome=@nome, email=@email, telefone=@telefone where CodCli= " + txtCodCli.Text;
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = txtNome.Text;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = txtEmail.Text;
                cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = txtTelefone.Text;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Dados Alterados com sucesso");
                limpaCampos();
                desabiltaCampos();
                txtPesquisar.Focus();
                cmd.Parameters.Clear();
                con.DesConnectarBD();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // FAZ A PERGUNTA SE DESEJA EXCLUIR OU NÃO 
            DialogResult excluir = MessageBox.Show("Deseja excluir esse registro ?", "Exclusão de Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (excluir == DialogResult.No)
            {
                return;
            }
            else
            {
                try
                {
                    // EXCLUI DO BANCO DE DADOS
                    MySqlCommand cmd = new MySqlCommand();
                    con.ConnectarBD();
                    cmd.CommandText = "delete from tbcliente where CodCli= " + txtCodCli.Text;
                    cmd.Connection = con.ConnectarBD();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Dados apagados com sucesso");
                    limpaCampos();
                    desabiltaCampos();
                    txtPesquisar.Focus();
                    cmd.Parameters.Clear();
                    con.DesConnectarBD();


                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
            }
        }

    

        private void dgvCliente_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgvCli_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void CarregarClientes()
        {
            try
            {
                txtCodCli.Text = dgvCli.SelectedRows[0].Cells[0].Value.ToString(); //valor da posição da memoria  0 0
                txtNome.Text = dgvCli.SelectedRows[0].Cells[1].Value.ToString();//valor da posição da memoria  0 1
                txtEmail.Text = dgvCli.SelectedRows[0].Cells[2].Value.ToString();//valor da posição da memoria  0 2
                txtTelefone.Text = dgvCli.SelectedRows[0].Cells[3].Value.ToString();//valor da posição da memoria  0 3
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao clicar" + erro);
            }

        }

        private void dgvCli_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CarregarClientes();
        }

        private void txtPesquisar_TextChanged(object sender, EventArgs e)
        {
            if (txtPesquisar.Text != "")
            {
                //TENTA
                try
                {
                    con.ConnectarBD();
                    //COMO ESTAMOS USANDO MYSQL PRECISA SER CHAMANDO O MYSQLCOMMAND NA INSTANCIA
                    MySqlCommand cmd = new MySqlCommand();
                    //FAZENDO O SELECT NA TABELA DO BANCO
                    cmd.CommandText = "select * from tbcliente";

                    //ABRE A CONEXÃO DO c# COM MYSQL
                    cmd.Connection = con.ConnectarBD();
                    //MECANISMO QUE BUSCA OS DADOS DA TABELA NO BANCO
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    //LOCAL AONDE ARMAZENA OS DADOS TRAZIDOS 
                    DataTable dt = new DataTable();
                    //JUNTO  O DATA ADAPTER (da) com MySqlCommand(cmd)
                    da.SelectCommand = cmd;
                    //FILTTRA OS DADOS DENTRO DO DT
                    da.Fill(dt);
                    //APRESENTA OS DADOS NO DATAGRIDVIEW (dgv
                    //)
                    dgvCli.DataSource = dt;
                    //DESCONECTA O BANCO
                    con.DesConnectarBD();
                }
                //CASO DE ERRADO 
                catch (Exception erro)
                {
                    //MOSTRA UMA MENSAGEM
                    MessageBox.Show(erro.Message);
                }
            }
            else
            {
                //SENÃO TRAS O DATAGRID VAZIO
                dgvCli.DataSource = null;

            }
        }
    }
}
