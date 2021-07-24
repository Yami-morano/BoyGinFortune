using BoyGin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;



namespace BoyGin.AccesoAdatos
{
    public class GestorBDCliente
    {


        public string Usuario()
        {
            var currentEmail = HttpContext.Current.User.Identity.Name;
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);

            var sql = "SELECT Id FROM AspNetUsers WHERE Email = @Email";
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Email", currentEmail);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            var currentIdUser = dr["Id"].ToString();
            dr.Close();
            conexion.Close();

            return currentIdUser;
        }

        public string getCurrentUser()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        public void AgregarCliente(Cliente cliente)
        {
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            string estado = "Activo";
            try
            {
                var sql = "INSERT INTO Clientes (Nombre, Apellido, DNI, Domicilio, NumeroDomicilio, Localidad, IdProvincia, Telefono, IdUsuario, Estado) VALUES (@nombre, @apellido, @dni, @domicilio, @NumeroDomicilio, @localidad, @idProvincia, @telefono, @idUsuario, @estado)";

                conexion.Open();
                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.Clear();
                string currentId = Usuario();
                cmd.Parameters.AddWithValue("@nombre", cliente.nombreCliente);
                cmd.Parameters.AddWithValue("@apellido", cliente.apellidoCliente);
                cmd.Parameters.AddWithValue("@dni", cliente.DNI);
                cmd.Parameters.AddWithValue("@domicilio", cliente.domicilio);
                cmd.Parameters.AddWithValue("@NumeroDomicilio", cliente.numeroDomicilio);
                cmd.Parameters.AddWithValue("@localidad", cliente.localidad);
                cmd.Parameters.AddWithValue("@idProvincia", cliente.idProvincia);
                cmd.Parameters.AddWithValue("@telefono", cliente.telefono);
                cmd.Parameters.AddWithValue("@IdUsuario", currentId);
                cmd.Parameters.AddWithValue("@estado", estado);

                //reemplazo el execute nonquery
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                conexion.Close();
                setRolUser(currentId, conexion);
            }
            catch (Exception exc)
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }

        public void setRolUser(string idUser, SqlConnection conexion)
        {
            try
            {
                var sql = "INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@userId, @idRol)";
                conexion.Open();
                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@userId", idUser);
                cmd.Parameters.AddWithValue("@idRol", 2);
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }



        public List<Provincia> ListadoProvincias()
        {
            var lista = new List<Provincia>();

            var sql = "SELECT * FROM Provincias";
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Provincia provincia = new Provincia();

                provincia.idProvincia = (int)dr["IdProvincia"];
                provincia.nombreProvincia = (string)dr["NombreProvincia"];


                lista.Add(provincia);
            }

            dr.Close();
            conexion.Close();

            return lista;
        }

        public List<DTOCliente> ListadoClientes()
        {
            var lista = new List<DTOCliente>();
            
            var sql = @"SELECT C.IdCliente, C.Nombre+' '+C.Apellido AS nombreCliente, c.DNI, c.Domicilio, c.NumeroDomicilio, c.Localidad, p.NombreProvincia, c.Telefono, u.Email
                        FROM Clientes c
                        JOIN Provincias p ON c.IdProvincia=p.IdProvincia
                        JOIN AspNetUsers U ON C.IdUsuario=U.Id
                        where c.Estado= 'Activo'";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                DTOCliente cliente = new DTOCliente();

                cliente.idCliente = (int)dr["IdCliente"];
                cliente.nombreCliente = (string)dr["nombreCliente"];
                cliente.DNI = (string)dr["DNI"];
                cliente.domicilio = (string)dr["Domicilio"];
                cliente.numeroDomicilio = (int)dr["NumeroDomicilio"];
                cliente.localidad = (string)dr["Localidad"];
                cliente.nombreProvincia = (string)dr["NombreProvincia"];
                cliente.telefono = (string)dr["Telefono"];
                cliente.email = (string)dr["Email"];
               



                lista.Add(cliente);
            }
            dr.Close();
            conexion.Close();

            return lista;
        }

        public static DTOCliente BuscarCliente(int idCliente)
        {
            DTOCliente resultado = new DTOCliente();

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);


            try
            {
                var sql = @"SELECT * FROM Clientes WHERE IdCliente =@IdCliente";
                conexion.Open();

                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.AddWithValue("@IdCliente", idCliente);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {

                    while (dr.Read())
                    {
                        resultado.idCliente = (int)dr["IdCliente"];
                        resultado.nombreCliente = (string)dr["Nombre"];
                        resultado.apellidoCliente = (string)dr["Apellido"];
                        resultado.DNI = (string)dr["DNI"];
                        resultado.domicilio = (string)dr["Domicilio"];
                        resultado.numeroDomicilio = (int)dr["NumeroDomicilio"];
                        resultado.localidad = (string)dr["Localidad"];
                        resultado.idProvincia = (int)dr["IdProvincia"];
                        resultado.telefono = (string)dr["Telefono"];
                        resultado.idUsuario = (string)dr["IdUsuario"];
                        resultado.estado = (string)dr["Estado"];

                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                conexion.Close();
            }

            return resultado;
        }

        public static bool EditarCliente(DTOCliente cliente)
        {
            bool resultado = false;

            var sql = " UPDATE Clientes SET Nombre = @nombre, Apellido = @apellido, DNI = @dni, Domicilio =@domicilio, NumeroDomicilio = @NumeroDomicilio, Localidad = @localidad, IdProvincia= @idProvincia, Telefono = @telefono, IdUsuario = @idUsuario, Estado = @estado   WHERE IdCliente = @idCliente";


            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();



            try
            {
                SqlCommand cmd = new SqlCommand(sql, conexion);

                cmd.Parameters.AddWithValue("@nombre", cliente.nombreCliente);
                cmd.Parameters.AddWithValue("@apellido", cliente.apellidoCliente);
                cmd.Parameters.AddWithValue("@dni", cliente.DNI);
                cmd.Parameters.AddWithValue("@domicilio", cliente.domicilio);
                cmd.Parameters.AddWithValue("@NumeroDomicilio", cliente.numeroDomicilio);
                cmd.Parameters.AddWithValue("@localidad", cliente.localidad);
                cmd.Parameters.AddWithValue("@idProvincia", cliente.idProvincia);
                cmd.Parameters.AddWithValue("@telefono", cliente.telefono);
                cmd.Parameters.AddWithValue("@idUsuario", cliente.idUsuario);
                cmd.Parameters.AddWithValue("@idCliente", cliente.idCliente);
                cmd.Parameters.AddWithValue("@estado", cliente.estado);
               


                cmd.ExecuteNonQuery();

                resultado = true;

            }
            catch (Exception)
            {

                throw;
            }

            finally
            {
                conexion.Close();
            }

            return resultado;

        }

     
        public static bool EliminarCliente(DTOCliente clientes, int idCliente)
        {

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            var sql1 = "SELECT IdUsuario FROM Clientes WHERE IdCliente = @idCliente";

            string idUsuario = "";
          
            conexion.Open();

            try
            {

                SqlCommand cmd1 = new SqlCommand(sql1, conexion);
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@IdCliente", idCliente);


                SqlDataReader dr = cmd1.ExecuteReader();

                if (dr != null)
                {

                    while (dr.Read())
                    {

                        idUsuario = (string)dr["IdUsuario"];

                    }
                }



                conexion.Close();

                conexion.Open();

                var sql2 = "UPDATE AspNetUsers SET Estado = 'Baja' where Id= @idUsuario";

                SqlCommand cmd2 = new SqlCommand(sql2, conexion);
                cmd2.Parameters.Clear();


                cmd2.Parameters.AddWithValue("@idUsuario", idUsuario);


                cmd2.ExecuteNonQuery();

                conexion.Close();
           

            bool resultado = false;

            var sql3 = "UPDATE Clientes SET Estado = 'Baja' where IdCliente= @IdCliente;";

            conexion.Open();

            try
            {

                SqlCommand cmd3 = new SqlCommand(sql3, conexion);

                cmd3.Parameters.AddWithValue("@IdCliente", idCliente);

                cmd3.ExecuteNonQuery();

                resultado = true;

            }
            catch (Exception)
            {

                throw;
            }

            finally
            {
                conexion.Close();
            }

            return resultado;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DTOCliente> BuscarClienteFiltro(string nombre, string apellido)
        {
            List<DTOCliente> buscarCliente = new List<DTOCliente>();

            try
            {
                var sql = @"SELECT C.IdCliente, C.Nombre+' '+C.Apellido AS nombreCliente, c.DNI, c.Domicilio, c.NumeroDomicilio, c.Localidad, p.NombreProvincia, c.Telefono, u.Email
                        FROM Clientes c
                        JOIN Provincias p ON c.IdProvincia=p.IdProvincia
                        JOIN AspNetUsers U ON C.IdUsuario=U.Id
                        where c.Estado= 'Activo'
                        AND C.Nombre like @nombre
                        AND c.Apellido like @apellido";




                SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
                conexion.Open();
                SqlCommand cmd = new SqlCommand(sql, conexion);

                cmd.Parameters.Add("@nombre", SqlDbType.VarChar);
                cmd.Parameters["@nombre"].Value = /*"%" + */nombre + "%";
                cmd.Parameters.Add("@apellido", SqlDbType.VarChar);
                cmd.Parameters["@apellido"].Value = /*"%" + */apellido + "%";


                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DTOCliente cliente = new DTOCliente();

                    cliente.idCliente = (int)dr["IdCliente"];
                    cliente.nombreCliente = (string)dr["nombreCliente"];
                    cliente.DNI = (string)dr["DNI"];
                    cliente.domicilio = (string)dr["Domicilio"];
                    cliente.numeroDomicilio = (int)dr["NumeroDomicilio"];
                    cliente.localidad = (string)dr["Localidad"];
                    cliente.nombreProvincia = (string)dr["NombreProvincia"];
                    cliente.telefono = (string)dr["Telefono"];
                    cliente.email = (string)dr["Email"];
             

                    buscarCliente.Add(cliente);

                }

                dr.Close();
                conexion.Close();

            }
            catch (Exception)
            {

                throw;
            }

            return buscarCliente;
        }

    }

}