using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GestionStockInformatique
{
    public partial class Transfer : Form
    {
        public string nProduct { get; set; }
        public Main FormMain { get; set; }

        public Transfer(string _product, Main form)
        {
            nProduct = _product;
            FormMain = form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtVersName.Text))
            {
                Bdd bdd = new Bdd();
             //   MessageBox.Show(bdd.isExisting(nProduct, txtVersName.Text).ToString());
                //bdd.TransferProduitExisting(txtVersName.Text, nProduct, Convert.ToInt32(numQuantite.Value));

                if (bdd.getQuantite(nProduct, "stock", "") < Convert.ToInt32(numQuantite.Value))
                {
                    MessageBox.Show("Impossible, le stock disponible pour ce produit est plus petit que le nombre indiqué", "Errur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (bdd.isExisting(nProduct, txtVersName.Text))
                    {
                        bdd.TransferProduitExisting(txtVersName.Text, nProduct, Convert.ToInt32(numQuantite.Value));
                    }
                    else
                    {
                        bdd.TransferProduitNotExisting(txtVersName.Text, nProduct, Convert.ToInt32(numQuantite.Value));
                    }

                    bdd.initSQL(FormMain.listStock);
                    // MessageBox.Show(txtVersName.Text + nProduct + Convert.ToInt32(numQuantite.Value).ToString());
                }
                
            } 
        }
    }
}
