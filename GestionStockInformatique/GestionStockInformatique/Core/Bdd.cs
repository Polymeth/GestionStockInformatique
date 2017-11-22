using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GestionStockInformatique
{
    public class Bdd
    {

        public int getQuantiteInt { get; set; }
        public int getEpuiseeInt { get; set; }
        public bool isAlreadyExisting { get; set; }

        private MySqlConnection connection;

        public bool sent;
        public string error;

        public Bdd()
        {
            this.InitConnexion();
        }

        private void InitConnexion()
        {
            // Création de la chaîne de connexion
            string connectionString = "SERVER=localhost; PORT=9999; DATABASE=gestion_stock; UID=root; PASSWORD=";
            this.connection = new MySqlConnection(connectionString);
        }


  #region "Get / Set"
        public int getQuantiteEpuisee(string _product)
        {

            MySqlDataReader dr;
            MySqlCommand cmd = connection.CreateCommand();
            string sql;
            sql = "SELECT * FROM stock" + " WHERE produit='" + _product + "'";
            if (this.connection.State == System.Data.ConnectionState.Closed)
            {
                this.connection.Open();
            }
            cmd = new MySqlCommand(sql, connection);
            dr = cmd.ExecuteReader();

            //getQuantiteInt = dr.GetString("quantite");
            while (dr.Read())
            {
                getEpuiseeInt = dr.GetInt32("quantite_epuisee");
            }

            dr.Close();
            this.connection.Close();
            return getEpuiseeInt;
        }

        public int getQuantite(string _product, string _db, string _name)
        {

            MySqlDataReader dr;
            MySqlCommand cmd = connection.CreateCommand();
            string sql;

            if (_db == "utilisateurs")
            {
                sql = "SELECT * FROM " + _db + " WHERE produit='" + _product + "' AND nom='" + _name + "'";
            }
            else
            {
                sql = "SELECT * FROM " + _db + " WHERE produit='" + _product + "'";
            }
            
 
            if (this.connection.State == System.Data.ConnectionState.Closed)
            {
                this.connection.Open();
            }
            cmd = new MySqlCommand(sql, connection);
            dr = cmd.ExecuteReader();

            //getQuantiteInt = dr.GetString("quantite");
            while (dr.Read())
            {
                getQuantiteInt = dr.GetInt32("quantite");
            }

            dr.Close();
            this.connection.Close();
            return getQuantiteInt;
        }
        
        // Optimisation du log des actions
        public bool isExisting(string _product, string _name)
        {

            MySqlDataReader dr;
            MySqlCommand cmd = connection.CreateCommand();
            string sql;
            sql = "SELECT * FROM utilisateurs" + " WHERE produit='" + _product + "' AND nom='" + _name + "'";
            if (this.connection.State == System.Data.ConnectionState.Closed)
            {
                this.connection.Open();
            }
            cmd = new MySqlCommand(sql, connection);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (String.IsNullOrEmpty(dr.GetString(0)))
                {
                    isAlreadyExisting = false;
                    MessageBox.Show("false");
                }
                else
                {
                    isAlreadyExisting = true;
                    MessageBox.Show("true");
                }
            }

            dr.Close();
            this.connection.Close();
            return isAlreadyExisting;
        }
  #endregion

       
        // Ajouter un produit à la liste
        public void AddProduit(Produit produit)
        {
            try
            {
                // Ouverture de la connexion SQL
                this.connection.Open();

                // Création d'une commande SQL en fonction de l'objet connection
                MySqlCommand cmd = this.connection.CreateCommand();

                // Requête SQL
                cmd.CommandText = "INSERT INTO stock (produit, quantite, quantite_epuisee) VALUES (@produit, @quantite, @quantite_epuisee)";

                // utilisation de l'objet contact passé en paramètre
                cmd.Parameters.AddWithValue("@produit", produit.Nom);
                cmd.Parameters.AddWithValue("@quantite", produit.Quantite);
                cmd.Parameters.AddWithValue("@quantite_epuisee", produit.QuantiteEpuisee);


                // Exécution de la commande SQL
                cmd.ExecuteNonQuery();

                // Fermeture de la connexion
                this.connection.Close();
                Main form = new Main();
                
                sent = true;
            }
            catch
            {
                // Gestion des erreurs :
                // Possibilité de créer un Logger pour les exceptions SQL reçus
                // Possibilité de créer une méthode vec un booléan en retour pour savoir si le contact à été ajouté correctement.
                sent = false;
                // error = MySqlScriptErrorEventArgs;
            }
        }

        // Modifier la quantité d'un produit dans la bdd 'stock'
        public void ModifyQuantite(string product, int quantite, int quantite_epuisee)
        {
            this.connection.Open();
            MySqlCommand cmd = this.connection.CreateCommand();
            if (quantite_epuisee >= 0)
            {
                cmd.CommandText = "UPDATE stock SET quantite=@quantite, quantite_epuisee=@epuisee" + " WHERE produit=@produit";
            }
            else
            {
                cmd.CommandText = "UPDATE stock SET quantite=@quantite" + " WHERE produit=@produit";
            }
            
            cmd.Parameters.AddWithValue("@quantite", quantite);
            cmd.Parameters.AddWithValue("@epuisee", quantite_epuisee);
            cmd.Parameters.AddWithValue("@produit", product);
            cmd.ExecuteNonQuery();
            this.connection.Close();
        }

        // Supprimer un produit
        public void DeleteProduit(string product)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Closed)
            {
                this.connection.Open();
            }
            MySqlCommand cmd = this.connection.CreateCommand();
            cmd.CommandText = "DELETE FROM stock WHERE produit=@product";
            cmd.Parameters.AddWithValue("@product", product);
            cmd.ExecuteNonQuery();
        }

        // Supprimer un utilisateur
        public void DeleteUtilisateur(string name)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Closed)
            {
                this.connection.Open();
            }
            MySqlCommand cmd = this.connection.CreateCommand();
            cmd.CommandText = "DELETE FROM utilisateurs WHERE nom=@nom";
            cmd.Parameters.AddWithValue("@nom", name);
            cmd.ExecuteNonQuery();
        }

        // Modifier le nom d'un produit
        public void ChangeNom(string product, string newName)
        {
            this.connection.Open();
            MySqlCommand cmd = this.connection.CreateCommand();
            MySqlCommand cmd2 = this.connection.CreateCommand();
            cmd.CommandText = "UPDATE stock SET produit=@produit " + "WHERE produit=@nom";
            cmd.Parameters.AddWithValue("@produit", newName);
            cmd.Parameters.AddWithValue("@nom", product);
            cmd.ExecuteNonQuery();
            cmd2.CommandText = "UPDATE utilisateurs SET produit=@produit " + "WHERE produit=@nom";
            cmd2.Parameters.AddWithValue("@produit", newName);
            cmd2.Parameters.AddWithValue("@nom", product);
            cmd2.ExecuteNonQuery();
            this.connection.Close();

            
        }

        // Transferer un produit qui n'existe pas chez un utilisateur
        // TODO: Fusionner avec TransferProduitExisting
        public void TransferProduitNotExisting(string name, string product, int quantite)
        {
            try
            {
                this.connection.Open();

                // Création d'une commande SQL en fonction de l'objet connection
                MySqlCommand cmd = this.connection.CreateCommand();

                // Requête SQL
                cmd.CommandText = "INSERT INTO utilisateurs (nom, produit, quantite) VALUES (@nom, @produit, @quantite)";

                // utilisation de l'objet contact passé en paramètre
                cmd.Parameters.AddWithValue("@nom", name);
                cmd.Parameters.AddWithValue("@produit", product);
                cmd.Parameters.AddWithValue("@quantite", quantite);

                cmd.ExecuteNonQuery();
                this.connection.Close();

                // QUANTITE EPUISSE += CELLE DAVANT
                int newQuantite = getQuantite(product, "stock", "") - quantite;
                int newEpuisee = getQuantiteEpuisee(product) + quantite;
                ModifyQuantite(product, newQuantite, newEpuisee);
 

            }
            catch
            {

            }
        }

        // Transferer un produit qui existe déjà chez un utilisateur
        // TODO: Fusionner avec TrasnferProduitNotExisting
        public void TransferProduitExisting(string name, string product, int quantite)
        {
            try
            {

                //MySqlCommand cmd = this.connection.CreateCommand();
                //cmd.CommandText = "UPDATE utilisateurs SET nom=@nom" + " WHERE id=@id";
                int _n = getQuantite(product, "utilisateurs", name) + quantite;
                MessageBox.Show(getQuantite(product, "utilisateurs", name) + "+" + quantite);
                string Query = "UPDATE utilisateurs SET quantite='" + _n.ToString() + "' WHERE produit='" + product + "' AND nom='" + name + "'" ;
                MySqlCommand cmd = new MySqlCommand(Query, this.connection);
                MySqlDataReader MyReader2;
                this.connection.Open();
                MyReader2 = cmd.ExecuteReader();
                this.connection.Close();

                // QUANTITE EPUISSE += CELLE DAVANT
                int newQuantite = getQuantite(product, "stock", "") - quantite;
                int newEpuisee = getQuantiteEpuisee(product) + quantite;
                ModifyQuantite(product, newQuantite, newEpuisee);


            }
            catch
            {

            }
        }

        // Initialiser les utilisateurs dans la combobox
        public void InitCombobox(ComboBox combo)
        {
            MySqlDataReader dr;
            MySqlCommand cmd = this.connection.CreateCommand();
            string sql;
            sql = "SELECT * FROM utilisateurs;";
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            cmd = new MySqlCommand(sql, this.connection);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (!combo.Items.Contains(dr.GetString(1))) {
                    combo.Items.Add(dr.GetString(1));
                }
                
            }

            dr.Close();
            connection.Close();
        }

        // Initiaiser la liste des produits des utilisateurs
        public void initUserList(ListView list, string username)
        {

            list.Items.Clear();
            MySqlDataReader dr;
            MySqlCommand cmd = connection.CreateCommand();
            string sql;
            sql = "SELECT * FROM utilisateurs WHERE nom='" + username + "';";
            if (this.connection.State == System.Data.ConnectionState.Closed)
            {
                this.connection.Open();
            }
            cmd = new MySqlCommand(sql, connection);
            dr = cmd.ExecuteReader();
            ListViewItem item;
            while (dr.Read())
            {
                item = new ListViewItem(dr.GetString(1));
                item.SubItems.Add(dr.GetString(2));
                item.SubItems.Add(dr.GetString(3));
                list.Items.Add(item);
            }

            dr.Close();
            this.connection.Close();
        }

        // Initialiser la ListView avec les produits
        public void initSQL(ListView list)
        {
            list.Items.Clear();
            MySqlDataReader dr;
            MySqlCommand cmd = connection.CreateCommand();
            string sql;
            sql = "SELECT * FROM stock;";
            if (this.connection.State == System.Data.ConnectionState.Closed)
            {
                this.connection.Open();
            }
            cmd = new MySqlCommand(sql, connection);
            dr = cmd.ExecuteReader();
            ListViewItem item;
            while (dr.Read())
            {
                item = new ListViewItem(dr.GetString(1));
                item.SubItems.Add(dr.GetString(2));
                item.SubItems.Add(dr.GetString(3));
                list.Items.Add(item);
            }
            
            dr.Close();
            this.connection.Close();
        }
    }
}
