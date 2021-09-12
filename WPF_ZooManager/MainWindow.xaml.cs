using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace WPF_ZooManager {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        SqlConnection sqlConnection;
        public MainWindow() {
            InitializeComponent();

            String connectionString = ConfigurationManager.ConnectionStrings["WPF_ZooManager.Properties.Settings.TutorialDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            ShowZoos();
        }

        private void ShowZoos() {
            try {
                string query = "select * from Zoo";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter) {
                    DataTable zooTable = new DataTable();

                    sqlDataAdapter.Fill(zooTable);

                    listZoos.DisplayMemberPath = "Location";
                    listZoos.SelectedValuePath = "Id";
                    listZoos.ItemsSource = zooTable.DefaultView;
                }

            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }

        private void listAssociatedAnimal_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ShowAssociatedAnimals();
        }

        private void ShowAssociatedAnimals() {
            try {
                string query = "select * from Animal a inner join ZooAnimal za on a.Id = za.AnimalId where za.ZooId = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter) {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    listAssociatedAnimal.DisplayMemberPath = "Name";
                    listAssociatedAnimal.SelectedValuePath = "Id";
                    listAssociatedAnimal.ItemsSource = animalTable.DefaultView;
                }

            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
