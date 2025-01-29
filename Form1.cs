using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace InsertMeSoft
{
    public partial class Form1 : Form
    {
        public static string neoprTableName = string.Format("НЕ ОПРЕДЕЛЕНО НАЗВАНИЕ ТАБЛИЦЫ В СХЕМЕ").ToUpper();
        public static string kc = "\r\n";
        public static string kc2 = "\n";
        public static string kw = "\"";
        public static string kv = "\'";
        public static string nullstr = "NULL";
        public static string pattern_rn = "\r\n";
        public DataSet ds;
        public static string ФайлНастроек = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ОбщиеНастройки.xml");
        public static string JavaBatname = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "bin"), "InsertMeSoft_JavaConn.bat");

        public List<string> listColumns = new List<string>();
        public List<string> listColumns2 = new List<string>();
        public static List<string> listPrimaryKeys;
        public static List<string> listExclude = new List<string> { "TRANS_ID", "SP_DEC_PNS_IS_ABRD_OLDNN" }; //исключить поля из скриптов
        public static List<string> list_accid = new List<string>();
        public static List<DataSet> list_dataset = new List<DataSet>();   //схемы
        public static List<string> list_Буфер = new List<string> { "", "Буфер_ОЧ", "Буфер_СЧ", "Буфер_ДСВ" };
        public static List<string> listIgnore = new List<string> {"SPU_ACC_MRG.TS_BEG", "SPU_ACC_MRG.TS_END"};


    public Form1()
        {
            InitializeComponent();
            Init_V1();
            Init_V5();
            Init_V6();
            Init_ЗапросВыборки();
            Init_asset();
        }

        private void button1_Click(object sender, EventArgs e) => Application.Exit();

        // выборка по одной таблице копируется в верхнее окно, результат обработки - инсерты в одну таблицу - выводится в нижнее окно
        private void button2_Click(object sender, EventArgs e)
        {
            v1.ЗаполнитьV1_class(ref textBox1, ref textBox2, ref textBox3, checkBox1, comboBox7);
            v1.СоздатьИнсертыПоВыборке();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void button3_Click(object sender, EventArgs e) => textBox2.Clear();
        private void button4_Click(object sender, EventArgs e) => textBox1.Clear();

        // Кнопка "Образец"
        private void button5_Click(object sender, EventArgs e)
        {
            v1.ЗаполнитьV1_class(ref textBox1, ref textBox2, ref textBox3, checkBox1, comboBox7);
            v1.НарисоватьОбразец();
        }

        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void button6_Click(object sender, EventArgs e) => v1.Подсказка_V1();
        private void button19_Click(object sender, EventArgs e) => Обработка_TBL_CREATE();
        private void checkBox3_CheckedChanged(object sender, EventArgs e) => textBox2.WordWrap = checkBox3.Checked;

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.A) && (e.Control))
            {
                textBox1.SelectAll();
                e.SuppressKeyPress = true;
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.A) && (e.Control))
            {
                textBox2.SelectAll();
                e.SuppressKeyPress = true;
            }
        }

        private void button28_Click(object sender, EventArgs e) => Clipboard.SetText(textBox2.Text);
        private void button29_Click(object sender, EventArgs e) => Clipboard.SetText(textBox1.Text);

        private void Button31_Click(object sender, EventArgs e)
        {
            Color buttonColor = ((Button)sender).BackColor;
            ((Button)sender).BackColor = Color.Yellow;
            Start_V5();
            ((Button)sender).BackColor = buttonColor;
        }

        private void Button30_Click(object sender, EventArgs e) => V5_Utils.АнкетаБолванчика(textBox10.Text,
                Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "SQL"), "АнкетаБолванчика.sql"),
                ref textBox2, checkBox12.Checked, checkBox16.Checked);

        private void Button32_Click(object sender, EventArgs e) => Clipboard.SetText(textBox12.Text);
        public static string SnilsToAcc(Match m) => m.Value.Substring(0, m.Value.IndexOf(" ")).Replace("-", "").TrimStart('0');

        private void TextBox11_TextChanged(object sender, EventArgs e)
        {
            textBox11.Text = new Regex(@"\d{3}-\d{3}-\d{3} \d{2}").Replace(textBox11.Text, SnilsToAcc);
            String[] s_arr = textBox11.Text.Trim().Split(new String[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries) ?? new string[1] { "" };
            textBox12.Text = string.Join(", ", (from string acc in s_arr where acc.Length > 2 select РассчитатьСНИЛС(acc)).ToList());
        }

        private void Button34_Click(object sender, EventArgs e) => СоздатьСкриптВыборки(textBox11.Text);
        private void Button38_Click(object sender, EventArgs e) => СоздатьСкриптУдаления(textBox11.Text);
        private void CheckBox10_CheckedChanged(object sender, EventArgs e) { }
        private void CheckBox9_CheckedChanged(object sender, EventArgs e) { }

        private void Button51_Click(object sender, EventArgs e)
        {
            Color buttonColor = ((Button)sender).BackColor;
            ((Button)sender).BackColor = Color.Yellow;
            Application.DoEvents();

            ЗапросВыборки(textBox11, textBox14, textBox4, textBox13, checkBox17, checkBox25,
                comboBox2, comboBox1, comboBox5, comboBox8, textBox12);

            ((Button)sender).BackColor = buttonColor;
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sss = comboBox2.SelectedItem.ToString();
            textBox4.Text = @"softwarecom\" + sss.Substring(0, sss.IndexOf("@"));
            if (textBox4.Text.Equals(myLogin))
                textBox13.Text = myPass;
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private void CheckBox13_CheckedChanged(object sender, EventArgs e) => КопиюЛочмелис = ((CheckBox)sender).Checked;

        private void textBox12_TextChanged(object sender, EventArgs e) { }
        private void textBox13_TextChanged(object sender, EventArgs e) { }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            label18.Text = "БД v.";
            Application.DoEvents();
            JТекущийСтендV5 = ((ComboBox)sender).Text;
            JStend.ПрочитатьНастройкиJStend(JТекущийСтендV5);

            string query = @"select SETTING_VAL from SPUMST.SPU_SETTING where SETTING_NAME='DB_VERSION';";
            var result = SQLUtils.JExecuteScalar(query);
            label18.Text = string.IsNullOrEmpty(result) ? "v. неизвестно" : $"БД v. {result}";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            Color buttonColor = ((Button)sender).BackColor;
            ((Button)sender).BackColor = Color.Yellow;
            Application.DoEvents();
            ПолучитьВерсииВсехМодулейОтмеченных(textBox1, listBox1);
            ((Button)sender).BackColor = buttonColor;
            ((Button)sender).Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            Color buttonColor = ((Button)sender).BackColor;
            ((Button)sender).BackColor = Color.Yellow;
            Application.DoEvents();
            ОбновитьСписокМодулей2(ref listBox1, ref assetList2, ref textBox1);
            ((Button)sender).BackColor = buttonColor;
            ((Button)sender).Enabled = true;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.BeginUpdate();
            for (int i = 0; i < listBox1.Items.Count; i++)
                listBox1.SetSelected(i, ((CheckBox)sender).Checked);
            listBox1.EndUpdate();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (((TabControl)sender).SelectedTab.Text.Equals("Модули"))
                ВыделитьМодулиВlistBox(listBox1, assetList2);
            else if (((TabControl)sender).SelectedTab.Text.Equals("V5"))
                JStend.ПрочитатьНастройкиJStend(JТекущийСтендV5);
        }

        private void button36_Click(object sender, EventArgs e) => СлучайныеФИО(ref textBox1);
        private void checkBox16_CheckedChanged(object sender, EventArgs e) { }

        private void button45_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            Color buttonColor = ((Button)sender).BackColor;
            ((Button)sender).BackColor = Color.Yellow;
            Application.DoEvents();

            if (!checkBox2.Checked)
                КопироватьСНИЛС_V6();
            else
                КлонироватьСНИЛСы();

            ((Button)sender).BackColor = buttonColor;
            ((Button)sender).Enabled = true;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e) => IniFilesWork.СохранитьНастройку(ФайлНастроек, "StendSaved", JТекущийСтендV5);

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            textBox5.Text = ((TextBox)sender).Text;
            int.TryParse(textBox5.Text, out int n0);
            int nClones = (int)numericUpDown1.Value;
            if (n0 > 0)
                textBox6.Text = (n0 + nClones - 1).ToString();
            else
            {
                textBox5.Text = "";
                textBox6.Text = "";
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox5.Text, out int n0);
            int nClones = (int)((NumericUpDown)sender).Value;
            if (n0 > 0)
                textBox6.Text = (n0 + nClones - 1).ToString();
            else
            {
                textBox5.Text = "";
                textBox6.Text = "";
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            label7.Text = ((TextBox)sender).Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
