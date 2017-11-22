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
    public partial class AddStock : Form
    {

        public string nProduct { get; set; }
        public Main nForm { get; set; }

        private string logProduit;

        public AddStock(string _product, Main _form)
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
                Bdd bdd = new Bdd();
                int newQuantite = bdd.getQuantite(nProduct, "stock", "") + Convert.ToInt32(numStock.Value);
                bdd.ModifyQuantite(nProduct, newQuantite, -1);
                logProduit = nProduct;
                bdd.initSQL(nForm.listStock);
                log.addLog(Logger.LogType.Succes, numStock.Value.ToString() + " '" + logProduit + "' ont été ajouté(e)s avec succès");
                this.Close();
            }
            catch
            {
                log.addLog(Logger.LogType.Erreur, numStock.Value.ToString() + " '" + logProduit + "' n'ont pas été ajoutés");
            }
            
        }

        private void AddStock_Load(object sender, EventArgs e)
        {

        }
    }
}
