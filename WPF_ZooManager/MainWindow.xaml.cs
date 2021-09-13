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
            ShowAllAnimals();
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
            ShowSelectedZooInTextBox();
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

        private void ShowAllAnimals() {
            try {
                string query = "select * from Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter) {
                    DataTable animalTable = new DataTable();
                    sqlDataAdapter.Fill(animalTable);

                    listAllAnimals.DisplayMemberPath = "Name";
                    listAllAnimals.SelectedValuePath = "Id";
                    listAllAnimals.ItemsSource = animalTable.DefaultView;
                }

            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e) {
            try {
                string query = "Delete from Zoo where id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.ExecuteScalar();

            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            } finally {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        private void AddZoo_Click(object sender, RoutedEventArgs e) {
            try {
                string query = "insert into Zoo values (@Location)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", MyTextBox.Text);
                sqlCommand.ExecuteScalar();

            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            } finally {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e) {
            try {
                string query = "insert into ZooAnimal values (@ZooId, @AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();

            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            } finally {
                sqlConnection.Close();
                ShowAssociatedAnimals();
            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e) {
            try {
                string query = "insert into Animal values (@Name)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", MyTextBox.Text);
                sqlCommand.ExecuteScalar();

            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            } finally {
                sqlConnection.Close();
                ShowAllAnimals();
            }
        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e) {
            try {
                string query = "Delete from Animal where id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();

            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            } finally {
                sqlConnection.Close();
                ShowAllAnimals();
                ShowAssociatedAnimals();
            }
        }

        private void ShowSelectedZooInTextBox() {
            try {
                string query = "select location from Zoo where Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter) {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                    DataTable zooDataTable = new DataTable();

                    sqlDataAdapter.Fill(zooDataTable);

                    MyTextBox.Text = zooDataTable.Rows[0]["Location"].ToString();
                }

            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }

        private void ShowSelectedAnimalInTextBox() {
            try {
                string query = "select name from Animal where Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter) {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                    DataTable animalDataTable = new DataTable();

                    sqlDataAdapter.Fill(animalDataTable);

                    MyTextBox.Text = animalDataTable.Rows[0]["Name"].ToString();
                }

            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }

        private void listAnimal_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ShowSelectedAnimalInTextBox();
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e) {
            try {
                string query = "update Zoo Set Location = @Location where Id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Location", MyTextBox.Text);
                sqlCommand.ExecuteScalar();

            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            } finally {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e) {
            try {
                string query = "update Animal Set Name = @Name where Id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", MyTextBox.Text);
                sqlCommand.ExecuteScalar();

            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            } finally {
                sqlConnection.Close();
                ShowAllAnimals();
            }
        }
    }
}
