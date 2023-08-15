using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace CRUD
{
    
    public partial class Form1 : Form
       
    {
        MySqlConnection myCon;

        private void conectar()
        {
            try
            {
                string server = "localhost";
                string database = "empresa";
                string username = "root";
                string pwd = "0801";
                string connectionString = "server=" + server + ";" + "database=" + database + ";" + "Uid=" + username + ";" + "password=" + pwd + ";";

                myCon = new MySqlConnection(connectionString);
                myCon.Open();
                
            }
            catch(Exception error) {
                MessageBox.Show("ERROR!!!"+ error);
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            conectar();
            MessageBox.Show("Conectado!!!");
            
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conectar();
        }

       

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
           
        }

       

    private void EmpleadosInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                llenarTabla();
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al llenar la tabla: " + ex.Message);
            }
        }

        private void llenarTabla()
        {
            string query = "SELECT numempleado, nombre, apellidopaterno, puesto, sueldo FROM empleados;";
            MySqlCommand comandoDB = new MySqlCommand(query, myCon);
            MySqlDataReader reader;

            try
            {
                reader = comandoDB.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int n = EmpleadosInfo.Rows.Add();
                        EmpleadosInfo.Rows[n].Cells[0].Value = reader.GetString(0);
                        EmpleadosInfo.Rows[n].Cells[1].Value = reader.GetString(1);
                        EmpleadosInfo.Rows[n].Cells[2].Value = reader.GetString(2);
                        EmpleadosInfo.Rows[n].Cells[3].Value = reader.GetString(3);
                        EmpleadosInfo.Rows[n].Cells[4].Value = reader.GetString(4); 
                    }
                }
                else
                {
                    Console.WriteLine("No hay empleados");
                }

                reader.Close(); // Close the reader when done
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO empleados(numempleado, nombre, apellidopaterno, puesto, sueldo) VALUES (@numempleado, @nombre, @apellidopaterno, @puesto, @sueldo);";
                MySqlCommand cmd = new MySqlCommand(query, myCon);
                cmd.Parameters.AddWithValue("@numempleado", txt_id.Text);
                cmd.Parameters.AddWithValue("@nombre", txt_nombre.Text);
                cmd.Parameters.AddWithValue("@apellidopaterno", txt_apellido.Text);

                // Convierte el valor del sueldo a decimal
                decimal sueldo;
                if (decimal.TryParse(txt_sueldo.Text, out sueldo))
                {
                    cmd.Parameters.AddWithValue("@sueldo", sueldo);
                }
                else
                {
                    MessageBox.Show("El valor del sueldo no es válido.");
                    return; // Salir del método si el valor del sueldo no es válido
                }

                cmd.Parameters.AddWithValue("@puesto", txt_puesto.Text);

                myCon.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Registro Ingresado con Éxito...");
                    llenarTabla();
                    EmpleadosInfo.Refresh();
                }
                else
                {
                    MessageBox.Show("No se Pudo Ingresar el Registro...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                myCon.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_id.Text))
            {
                MessageBox.Show("Por favor, ingresa el ID del empleado que deseas eliminar.");
                return;
            }

            try
            {
                string query = "DELETE FROM empleados WHERE numempleado = @numempleado;";
                MySqlCommand cmd = new MySqlCommand(query, myCon);
                cmd.Parameters.AddWithValue("@numempleado", txt_id.Text);

                myCon.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Registro Eliminado con Éxito...");
                    llenarTabla();
                }
                else
                {
                    MessageBox.Show("No se Pudo Eliminar el Registro. Asegúrate de que el ID sea válido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                myCon.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_id.Text))
            {
                MessageBox.Show("Por favor, ingresa el ID del empleado que deseas actualizar.");
                return;
            }

            try
            {
                string query = "UPDATE empleados SET nombre = @nombre, apellidopaterno = @apellidopaterno, sueldo = @sueldo, puesto = @puesto WHERE numempleado = @numempleado;";
                MySqlCommand cmd = new MySqlCommand(query, myCon);
                cmd.Parameters.AddWithValue("@numempleado", txt_id.Text);
                cmd.Parameters.AddWithValue("@nombre", txt_nombre.Text);
                cmd.Parameters.AddWithValue("@apellidopaterno", txt_apellido.Text);

                // Convierte el valor del sueldo a decimal
                decimal sueldo;
                if (decimal.TryParse(txt_sueldo.Text, out sueldo))
                {
                    cmd.Parameters.AddWithValue("@sueldo", sueldo);
                }
                else
                {
                    MessageBox.Show("El valor del sueldo no es válido.");
                    return;
                }

                cmd.Parameters.AddWithValue("@puesto", txt_puesto.Text);

                myCon.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Registro Actualizado con Éxito...");
                    llenarTabla();
                }
                else
                {
                    MessageBox.Show("No se Pudo Actualizar el Registro. Asegúrate de que el ID sea válido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                myCon.Close();
            }
        }
    }

}