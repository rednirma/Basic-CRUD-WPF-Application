using System;
using System.Collections.Generic;
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
using System.Configuration;
using System.Data;

namespace ESS_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }
        //static string myconnstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

        SqlConnection conn = new SqlConnection("data source=7231AMRIND4168L;initial catalog=demo;trusted_connection=true;TrustServerCertificate=True;");

        public void ClearData()
        {
            name_txt.Clear();
            age_txt.Clear();
            gender_txt.Clear();
            city_txt.Clear();   
            Search_txt.Clear();
        }

        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM FirstTable",conn);
            DataTable   dt = new DataTable();
            conn.Open(); 
            SqlDataReader sdr= cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            datagrid.ItemsSource = dt.DefaultView;
        }
        private void ClearDataBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        public bool IsValid()
        {
            if (name_txt.Text == string.Empty)
            {
                MessageBox.Show("The name field is empty", "failed", MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }
            if (age_txt.Text == string.Empty)
            {
                MessageBox.Show("The age field is empty", "failed", MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }
            if (gender_txt.Text == string.Empty)
            {
                MessageBox.Show("The gender field is empty", "failed", MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }
            if (city_txt.Text == string.Empty)
            {
                MessageBox.Show("The city field is empty", "failed", MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool IsValidForDelete()
        {
            if (Search_txt.Text == string.Empty)   
            {
                MessageBox.Show("The id field is empty", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public bool IsValidForUpdate()
        {
            if (name_txt.Text == string.Empty)
            {
                MessageBox.Show("The name field is empty", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (age_txt.Text == string.Empty)
            {
                MessageBox.Show("The age field is empty", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (gender_txt.Text == string.Empty)
            {
                MessageBox.Show("The gender field is empty", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (city_txt.Text == string.Empty)
            {
                MessageBox.Show("The city field is empty", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }if (Search_txt.Text == string.Empty)
            {
                MessageBox.Show("The ID field is empty", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                DataTable dt = new DataTable();

                if (IsValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO FirstTable(Name,Age,Gender,City) VALUES (@Name,@Age,@Gender, @City)", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", name_txt.Text);
                    cmd.Parameters.AddWithValue("@Age", age_txt.Text);
                    cmd.Parameters.AddWithValue("@Gender", gender_txt.Text);
                    cmd.Parameters.AddWithValue("@City", city_txt.Text);
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);

                    conn.Close();
                    LoadGrid();
                    MessageBox.Show("entry updated", "saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearData();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidForDelete())
            {
                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM FirstTable where ID=@search", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@search", Search_txt.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("record has been deleted", "deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    conn.Close();
                    ClearData();
                    LoadGrid();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("not deleted" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidForUpdate()) 
            {
                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("UPDATE FirstTable set Name= @name, Age=@age,Gender=@Gender,City=@City WHERE ID= @id", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@name", name_txt.Text);
                    cmd.Parameters.AddWithValue("@age", age_txt.Text);
                    cmd.Parameters.AddWithValue("@gender", gender_txt.Text);
                    cmd.Parameters.AddWithValue("@city", city_txt.Text);
                    cmd.Parameters.AddWithValue("@id", Search_txt.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("record has been updated", "updated", MessageBoxButton.OK, MessageBoxImage.Information);
                    conn.Close();
                    ClearData();
                    LoadGrid();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("not updated" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
