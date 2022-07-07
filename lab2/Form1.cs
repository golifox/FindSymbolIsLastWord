using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.ContextMenu contextMenu1;
            contextMenu1 = new System.Windows.Forms.ContextMenu();
            System.Windows.Forms.MenuItem menuItem1;
            menuItem1 = new System.Windows.Forms.MenuItem();
            System.Windows.Forms.MenuItem menuItem2;
            menuItem2 = new System.Windows.Forms.MenuItem();
            System.Windows.Forms.MenuItem menuItem3;
            menuItem3 = new System.Windows.Forms.MenuItem();
            contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {menuItem1, menuItem2, menuItem3 });
            menuItem1.Index = 0;
            menuItem1.Text = "Открыть";
            menuItem2.Index = 1;
            menuItem2.Text = "Сохранить";
            menuItem3.Index = 2;
            menuItem3.Text = "Сохранить как";
            richTextBox1.ContextMenu = contextMenu1;
            menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            this.KeyPreview = false;

        }


        string MyFName = "";
        string errStr = "Ошибка ввода! Повторите ввод!";
        string lastW = "";
        int current = 0; //позиция текущей буквы а
        int n = 0; //позиция начала последнего слова
        List<int> integer = new List<int>(); //лист который будет хранить значения индексов букв а
        int count = 0;
        private void menuItem1_Click(object sender, System.EventArgs e)
        {
            var fileContent=string.Empty;
            openFileDialog1.Filter = "Текстовые файлы (*.rtf, *.txt, *.dat)|*.rtf, *.txtm *.dat|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MyFName = openFileDialog1.FileName;
           
                fileContent = System.IO.File.ReadAllText(MyFName, System.Text.Encoding.GetEncoding(1251)); //важно!! декодировка для русских текстов
                richTextBox1.Text = fileContent;           
            }

        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (MyFName != "")
            {
                richTextBox1.SaveFile(MyFName);
            }
            else
            {
                saveFileDialog1.Filter = "Текстовые файлы (*.rtf; *.txt; *.dat) | *.rtf;*.txt; *.dat";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    MyFName = saveFileDialog1.FileName;
                    richTextBox1.SaveFile(MyFName);
                }
            }
        }
        private void menuItem3_Click(object sender, System.EventArgs e)
        {
            saveFileDialog1.Filter = "Текстовые файлы (*.rtf; *.txt; *.dat) | *.rtf;*.txt; *.dat";
        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MyFName = saveFileDialog1.FileName;
                richTextBox1.SaveFile(MyFName);
            }
        }

        private void Button1_Click(object sender, EventArgs e) //очистка всех боксов и обнуления переменных
        {
            textBox1.Clear();
            richTextBox1.Clear();
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
            current = 0; n = 0; c = 0;count = 0;
            integer.Clear();
            this.KeyPreview = false;
            richTextBox1.ReadOnly = false;

        }


        string findLW(System.String str, ref int n) //поиск последнего слова функция
        {
            int countWords = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length; //посчитать слова в тексте
            if (countWords == 1) //если всего1  слово
            {
                lastW = str.Substring(0);
                return "Последнее слово в тексте:" + lastW;
            }
            else if (str!= "" ) //если слов больш
            {
                lastW = str.Substring(str.LastIndexOf(" "));
                n = str.LastIndexOf(" ");
                return "Последнее слово в тексте:" +lastW;
            }
            else return errStr; //иначе ошибка ввода
        }



        private void Button2_Click(object sender, EventArgs e) //поиск последнего слова нажатие кнопки
        {
            button2.Enabled = false;

            if(findLW(richTextBox1.Text, ref n) != errStr) //если нет ошибки ввода
            {
                richTextBox1.ReadOnly = true;
                textBox1.Text = findLW(richTextBox1.Text, ref n) + Environment.NewLine; //выводим слово
                string need_symbol = textBox2.Text;

                for (int i = 0; i < lastW.Length; i++)
                {
                    if (lastW[i].ToString() == need_symbol) count++; //считаем количество букв
                }

               
                textBox1.Text += "Kоличество букв '"+ need_symbol+ "' в последнем слове: " + count + Environment.NewLine; //выводим еще

                int j = 0, cur = 0; //переменные
                while (j != count) 
                {
                    if (lastW[cur].ToString() == need_symbol) //узнаем позицию каждой буквы для дальнейшего выделения
                    {
                        integer.Add(cur); //добюавляем в лист
                        j++;
                    }
                    cur++;
                } 

                if(count != 0)
                {
                button3.Enabled = true; //включаем кнопку для действия
                this.KeyPreview = true; //а также ждем нажатия любой клавиши
                }
                
            }
            else
            {
                textBox1.Text = errStr; 
            }
            
        }

        void excretion(){ //выделение букв а
            int[] arr = integer.ToArray(); //считываем позиции букв а из листа в массив 
            richTextBox1.SelectionStart = n + arr[current]; //позиция каждой буквы это позиция слова + позиция буквы в слове
            richTextBox1.SelectionLength = 1; //длина 1, т.кк выделяем одну букву
            richTextBox1.SelectionBackColor = Color.Red; //цвет выделения
            c++;

            if (c != count)
            {
                current++; //если прошли еще не все буквы в слове идем дальше
            }
            else
            {
                button3.Enabled = false; //иначе делаем неактивными кнопки 
                button1.Focus();
                this.KeyPreview = false;

            }
        }
        protected override void OnKeyDown(KeyEventArgs e) //обработчик нажатия любой кнопки
        {
            excretion();
        }

        int c = 0;
        private void Button3_Click(object sender, EventArgs e) //обработчик кнопки для выделения
        {
            excretion();
        }
    }
}
