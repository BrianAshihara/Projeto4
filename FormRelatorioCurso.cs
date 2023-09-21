using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ReaLTaiizor.Forms;
using Spire.Pdf.Graphics;
using Spire.Pdf.Tables;
using Spire.Pdf;
using System.Drawing.Printing;
using System.Diagnostics;

namespace Projeto4
{
    public partial class FormRelatorioCurso : MaterialForm
    {
        string cs = @"server=localhost;uid=root;pwd=;database=academico";
        public FormRelatorioCurso()
        {
            InitializeComponent();
            CarregaImpressoras();
        }

        private void FormRelatorioCurso_Load(object sender, EventArgs e)
        {

        }

        private void CarregaImpressoras()
        {
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                cboImpressora.Items.Add(printer);
            }
        }
        private void MontaRelatorio()
        {
            var con = new MySqlConnection(cs);
            con.Open();
            var sql = "SELECT * FROM curso WHERE 1 = 1";

            if (txtAno.Text != "")
            {
                sql += " and estado = @ano_criacao";
            }

            if (cboTitulo.Text != "")
            {
                sql += " and cidade = @tipo";
            }
            var sqlAd = new MySqlDataAdapter();
            sqlAd.SelectCommand = new MySqlCommand(sql, con);

            if (txtAno.Text != "")
            {
                sqlAd.SelectCommand.Parameters.AddWithValue("@ano_criacao", txtAno.Text);
            }

            if (cboTitulo.Text != "")
            {
                sqlAd.SelectCommand.Parameters.AddWithValue("@tipo", cboTitulo.Text);
            }
            var dt = new DataTable();
            sqlAd.Fill(dt);
            con.Close();

            PdfDocument doc = new PdfDocument();
            PdfSection sec = doc.Sections.Add();
            sec.PageSettings.Width = PdfPageSize.A4.Width;
            PdfPageBase page = sec.Pages.Add();
            int y = 20;
            PdfBrush brush1 = PdfBrushes.Black;
            PdfTrueTypeFont font1 = new PdfTrueTypeFont(new Font("Arial", 16f, FontStyle.Bold));
            PdfStringFormat format1 = new PdfStringFormat(PdfTextAlignment.Center);

            page.Canvas.DrawString("Relatório de Cursos", font1, brush1, page.Canvas.ClientSize.Width / 2, y, format1);

            PdfTable table = new PdfTable();
            table.Style.CellPadding = 2;
            table.Style.BorderPen = new PdfPen(brush1, 0.75f);
            table.Style.HeaderStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Center);
            table.Style.HeaderSource = PdfHeaderSource.ColumnCaptions;
            table.Style.HeaderRowCount = 1;
            table.Style.HeaderStyle.BackgroundBrush = PdfBrushes.CadetBlue;
            table.Style.ShowHeader = true;
            table.DataSource = dt;
            foreach (PdfColumn col in table.Columns)
            {
                col.StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            }
            table.Draw(page, new Point(0, y + 30));


            doc.SaveToFile("Relatório de Cursos.pdf");
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MontaRelatorio();
            //envia para impressora

            string pdfFilePath = @"D:\Brian Ashihara\Projeto4\bin\Debug\net7.0-windows\Relatório de Cursos.pdf";
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                // Carregar o conteúdo do arquivo PDF
                using (FileStream stream = new FileStream(pdfFilePath, FileMode.Open, FileAccess.Read))
                {
                    Image pdfImage = Image.FromStream(stream);
                    e.Graphics.DrawImage(pdfImage, e.PageBounds);
                }
            };
            printDocument.Print();
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@"Relatório de Cursos.pdf")
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
 }
