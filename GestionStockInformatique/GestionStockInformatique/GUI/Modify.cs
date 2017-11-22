using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GestionStockInformatique.GUI
{
    public partial class New : Form
    {

        public Main nForm { get; set; }
        public string nProduct { get; set; }
        public New(string _product, Main _form)
        {
            nForm = _form;
            nProduct = _product; 
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Logger.logger log = new Logger.logger(nForm.richTextLog);
            try
            {
                if (!String.IsNullOrWhiteSpace(textBox1.Text))
                {

                    Bdd bdd = new Bdd();
                    bdd.ChangeNom(nProduct, textBox1.Text);
                    bdd.initSQL(nForm.listStock);
                    log.addLog(Logger.LogType.Succes, "Le nom de " + nProduct + " a bien été modifié en " + textBox1.Text);
                }
                else
                {
                    log.addLog(Logger.LogType.Succes, "Le nom de " + nProduct + " n'a pas pu être modifié en " + textBox1.Text + "(Nom inadéquat)");
                }
            }
            catch
            {
                log.addLog(Logger.LogType.Succes, "Le nom de " + nProduct + " n'a pas pu être modifié en " + textBox1.Text);
            }
        }
    }
}
