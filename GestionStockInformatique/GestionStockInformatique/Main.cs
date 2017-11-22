using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GestionStockInformatique
{
    public partial class Main : Form
    {

        public string ClickedItem { get; set; }
        private Bdd bdd = new Bdd();


        public Main()
        {
            InitializeComponent();

            bdd.initSQL(listStock);

        }

        private void ajouterUnProduitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add form = new Add(this);
            form.Show();
        }



        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void retirerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listStock.SelectedItems.Count == 0)
            {
                return;
            }
            else
            {
                Transfer form = new Transfer(listStock.SelectedItems[0].Text, this);
                form.Show();
            }

        }

        private void Main_Load(object sender, EventArgs e)
        {
            bdd.InitCombobox(cbUtilisateur);
        }

        private void cbUtilisateur_SelectedIndexChanged(object sender, EventArgs e)
        {
            bdd.initUserList(listUser, cbUtilisateur.SelectedItem.ToString());
           //MessageBox.Show(cbUtilisateur.SelectedItem.ToString());
        }

        private void ajouterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listStock.SelectedItems.Count == 0)
            {
                return;
            }
            else
            {
                GUI.AddStock form = new GUI.AddStock(listStock.SelectedItems[0].Text, this);
                form.Show();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            bdd.InitCombobox(cbUtilisateur);
            bdd.initUserList(listUser, cbUtilisateur.SelectedItem.ToString());
        }

        private void modifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listStock.SelectedItems.Count == 0)
            {
                return;
            }
            else
            {
                GUI.New form = new GUI.New(listStock.SelectedItems[0].Text, this);
                form.Show();
            }
        }

        private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listStock.SelectedItems.Count == 0)
            {
                return;
            }
            else
            {
                bdd.DeleteProduit(listStock.SelectedItems[0].Text);
                bdd.initSQL(listStock);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbUtilisateur.SelectedItem != null)
            {
                bdd.DeleteUtilisateur(cbUtilisateur.SelectedItem.ToString());
                cbUtilisateur.Items.Remove(cbUtilisateur.SelectedItem.ToString());
            }
            
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            if (checkRefresh.Checked)
            {
                bdd.InitCombobox(cbUtilisateur);
                if (cbUtilisateur.SelectedItem != null)
                {
                    bdd.initUserList(listUser, cbUtilisateur.SelectedItem.ToString());
                }
            }
        }
    }
}
