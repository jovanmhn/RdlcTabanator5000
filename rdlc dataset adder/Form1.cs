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

namespace rdlc_dataset_adder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBox1.Text = String.Empty;

                    string[] files = Directory.GetFiles(fbd.SelectedPath, "*", SearchOption.AllDirectories);
                    int i = 0;
                    foreach (string file in files/*.Where(qq=> Path.GetFileName(qq).StartsWith("Racun") && !Path.GetFileName(qq).Contains("RacunUslugeLista") && Path.GetExtension(qq) == ".rdlc")*/)
                    {
                        textBox1.Text += Replace(file) + System.Environment.NewLine;
                        textBox1.Text += ReplaceDataSource(file) + System.Environment.NewLine;
                        i++;
                    }
                    label1.Text = "Ukupno: " + i.ToString();
                }
            }
        }
        public string Replace(string path)
        {
            string text = File.ReadAllText(path);
            if(text.Contains("FiscalData")) return Path.GetFileName(path) + " ---> Vec postoji DataSet!";
            text = text.Replace("</DataSets>", FiscalData);
            File.WriteAllText(path, text);
            return Path.GetFileName(path) + " ---> Dataset OK!";
        }
        public string AddInvNum(string path)
        {
            string text = File.ReadAllText(path);
            if (text.Contains(@"<Field Name=""InvNum"">")) return Path.GetFileName(path) + " ---> Vec postoji InvNum polje!";
            if(!text.Contains(FiscalDataHeader)) return Path.GetFileName(path) + " ---> NIJE PRONADJENO is_fiscal POLJE?!";
            text = text.Replace(FiscalDataHeader, FiscalDataHeaderNew);
            File.WriteAllText(path, text);
            return Path.GetFileName(path) + " ---> InvNum dodat u FiscalData OK!";
        }
        public string ReplaceDataSource(string path)
        {
            string text = File.ReadAllText(path);
            if (text.Contains(@"<DataSource Name=""KnjigovodstvoReports"">")) return Path.GetFileName(path) + " ---> Vec postoji DataSource!";
            text = text.Replace("</DataSources>", DataConn);
            File.WriteAllText(path, text);
            return Path.GetFileName(path) + " ---> Datasource OK!";
        }
        public string ReplaceFields(string path)
        {
            string text = File.ReadAllText(path);
            if (text.Contains(@"<Field Name=""is_fiscal"">")) return Path.GetFileName(path) + " ---> Vec postoje polja!";
            text = text.Replace("</Fields>", FiscalFields);
            File.WriteAllText(path, text);
            return Path.GetFileName(path) + " ---> Fields OK!";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBox1.Text = String.Empty;

                    string[] files = Directory.GetFiles(fbd.SelectedPath, "*", SearchOption.AllDirectories);
                    int i = 0;
                    foreach (string file in files.Where(qq => Path.GetFileName(qq).StartsWith("RacunUslugeLista")  && Path.GetExtension(qq) == ".rdlc"))
                    {
                        textBox1.Text += ReplaceFields(file) + System.Environment.NewLine;
                        i++;
                    }
                    label1.Text = "Ukupno: " + i.ToString();
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBox1.Text = String.Empty;

                    string[] files = Directory.GetFiles(fbd.SelectedPath, "*", SearchOption.AllDirectories);
                    int i = 0;
                    foreach (string file in files/*.Where(qq => Path.GetFileName(qq).StartsWith("Racun") && !Path.GetFileName(qq).Contains("RacunUslugeLista") && Path.GetExtension(qq) == ".rdlc")*/)
                    {
                        textBox1.Text += AddInvNum(file) + System.Environment.NewLine;
                        i++;
                    }
                    label1.Text = "Ukupno: " + i.ToString();
                }
            }
        }


        public const string FiscalData = @"<DataSet Name=""FiscalData"">
      <Query>
        <DataSourceName>Database</DataSourceName>
        <CommandText>/* Local Query */</CommandText>
      </Query>
      <Fields>
        <Field Name =""IKOF"">
          <DataField>IKOF</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""is_fiscal"">
          <DataField>is_fiscal</DataField >
          <rd:TypeName>System.Boolean</rd:TypeName>
        </Field>
        <Field Name =""JIKR"">
          <DataField>JIKR</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""opcode"">
          <DataField>opcode</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""QR"">
          <DataField>QR</DataField>
          <rd:TypeName>System.Byte[]</rd:TypeName>
        </Field>
        <Field Name=""InvNum"">
          <DataField>InvNum</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name =""time"">
          <DataField>time</DataField>
          <rd:TypeName>System.DateTime</rd:TypeName>
        </Field>
      </Fields>
      <rd:DataSetInfo>
        <rd:DataSetName>Knjigovodstvo.Reports</rd:DataSetName>
        <rd:TableName>FiskalniPodaciRacun</rd:TableName>
        <rd:ObjectDataSourceType>Knjigovodstvo.Reports.frmIzvestaj+FiskalniPodaciRacun, Knjigovodstvo, Version=20.1225.1111.31030, Culture=neutral, PublicKeyToken=null</rd:ObjectDataSourceType>
      </rd:DataSetInfo>
    </DataSet>
  </DataSets>";
        public const string DataConn = @"<DataSource Name=""KnjigovodstvoReports"">
      <ConnectionProperties>
        <DataProvider>System.Data.DataSet</DataProvider>
        <ConnectString>/* Local Connection */</ConnectString>
      </ConnectionProperties>
      <rd:DataSourceID>2bc1b105-806b-49c1-8875-d2e798c5f87a</rd:DataSourceID>
    </DataSource>
</DataSources>";
        public const string FiscalFields = @"<Field Name=""broj_dok"">
          <DataField>broj_dok</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name =""IKOF"">
          <DataField>IKOF</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""is_fiscal"">
          <DataField>is_fiscal</DataField>
          <rd:TypeName>System.Boolean</rd:TypeName>
        </Field>
        <Field Name=""JIKR"">
          <DataField>JIKR</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""opcode"">
          <DataField>opcode</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""QR"">
          <DataField>QR</DataField>
          <rd:TypeName>System.Byte[]</rd:TypeName>
        </Field>
        <Field Name=""time"">
          <DataField>time</DataField>
          <rd:TypeName>System.DateTime</rd:TypeName>
        </Field>
      </Fields>";

        public const string FiscalDataHeader = @"<Field Name=""is_fiscal"">
          <DataField>is_fiscal</DataField>
          <rd:TypeName>System.Boolean</rd:TypeName>
        </Field>";
        public const string FiscalDataHeaderNew = @"<Field Name=""is_fiscal"">
          <DataField>is_fiscal</DataField>
          <rd:TypeName>System.Boolean</rd:TypeName>
        </Field>
        <Field Name=""InvNum"">
          <DataField>InvNum</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>";

        private void button4_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBox1.Text = String.Empty;

                    string[] files = Directory.GetFiles(fbd.SelectedPath, "*", SearchOption.AllDirectories);
                    int i = 0;
                    foreach (string file in files.Where(qq => Path.GetFileName(qq).StartsWith("RacunUslugeLista") && Path.GetExtension(qq) == ".rdlc"))
                    {
                        textBox1.Text += AddInvNum(file) + System.Environment.NewLine;
                        i++;
                    }
                    label1.Text = "Ukupno: " + i.ToString();
                }
            }
        }
    }
}
