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
    public partial class Add : Form
    {
        public Main TheForm1 { get; set; }
        private string logProduit;

        public Add(Main _theForm)
        {
            TheForm1 = _theForm;
            InitializeComponent();
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            Logger.logger log = new Logger.logger(TheForm1.richTextLog);
            try
            {
                Produit produit = new Produit();
                produit.Nom = txtName.Text;
                logProduit = produit.Nom;
                produit.Quantite = Convert.ToInt32(numQuantite.Value);
                produit.QuantiteEpuisee = 0;
                produit.Acheteur = "";

                Bdd bdd = new Bdd();
                bdd.AddProduit(produit);
                bdd.initSQL(TheForm1.listStock);
                log.addLog(Logger.LogType.Succes, "Produit '" + logProduit + "' ajouté avec succès");
            }
            catch
            {
                log.addLog(Logger.LogType.Erreur, "Impossible d'ajouter le produit '" + logProduit + "', veuillez reessayer");
            }
            
        }

        private void Add_Load(object sender, EventArgs e)
        {

        }
    }
}
