using BoyGin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BoyGin.AccesoAdatos
{
    public class GestorBDProveedor
    {
        public void AgregarProveedor(Proveedor proveedor)
        {
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);

            bool estado = true;
            try
            {
                var sql = "INSERT INTO Proveedores (Nombre_razonSocial, Apellido_TipoSoc, Direccion, NumeroDireccion, Localidad, IdProvincia, Email, Telefono, Estado) VALUES (@nombre, @apellido, @direccion, @numeroDireccion, @localidad, @idProvincia, @email, @telefono, @estado)";
                conexion.Open();
                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@nombre", proveedor.nombre_razonSocial);
                cmd.Parameters.AddWithValue("@apellido", proveedor.apellido_TipoSoc);
                cmd.Parameters.AddWithValue("@direccion", proveedor.direccion);
                cmd.Parameters.AddWithValue("@numeroDireccion", proveedor.numeroDireccion);
                cmd.Parameters.AddWithValue("@localidad", proveedor.localidad);
                cmd.Parameters.AddWithValue("@idProvincia", proveedor.idProvincia);
                cmd.Parameters.AddWithValue("@email", proveedor.email);
                cmd.Parameters.AddWithValue("@telefono", proveedor.telefono);
                cmd.Parameters.AddWithValue("@estado", estado);



                cmd.ExecuteNonQuery();
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



        public List<DTOProveedor> ListadoProveedores()
        {
            var lista = new List<DTOProveedor>();

            var sql = @"SELECT prov.IdProveedor, prov.Nombre_razonSocial, prov.Apellido_TipoSoc, prov.Direccion, prov.NumeroDireccion, prov.Localidad, p.NombreProvincia, prov.Email, prov.Telefono
                        FROM Proveedores prov
                        JOIN Provincias p ON prov.IdProvincia=p.IdProvincia
                        WHERE Estado= 'true'";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                DTOProveedor proveedor = new DTOProveedor();

                proveedor.idProveedor = (int)dr["IdProveedor"];
                proveedor.nombre_razonSocial = (string)dr["Nombre_razonSocial"];
                proveedor.apellido_TipoSoc = (string)dr["Apellido_TipoSoc"];
                proveedor.direccion = (string)dr["Direccion"];
                proveedor.numeroDireccion = (int)dr["NumeroDireccion"];
                proveedor.localidad = (string)dr["Localidad"];
                proveedor.nombreProvincia = (string)dr["NombreProvincia"];
                proveedor.email = (string)dr["Email"];
                proveedor.telefono = (string)dr["Telefono"];
                lista.Add(proveedor);
            }
            dr.Close();
            conexion.Close();

            return lista;
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

        public static DTOProveedor BuscarProveedor(int idProveedor)
        {
            DTOProveedor resultado = new DTOProveedor();

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);


            try
            {
                var sql = @"SELECT * FROM Proveedores WHERE IdProveedor =@idProveedor";
                conexion.Open();

                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.AddWithValue("@idProveedor", idProveedor);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {

                    while (dr.Read())
                    {
                        resultado.idProveedor = (int)dr["IdProveedor"];
                        resultado.nombre_razonSocial = (string)dr["Nombre_razonSocial"];
                        resultado.apellido_TipoSoc = (string)dr["Apellido_TipoSoc"];
                        resultado.direccion = (string)dr["Direccion"];
                        resultado.numeroDireccion = (int)dr["NumeroDireccion"];                 
                        resultado.localidad = (string)dr["Localidad"];
                        resultado.idProvincia = (int)dr["IdProvincia"];
                        resultado.email = (string)dr["Email"];
                        resultado.telefono = (string)dr["Telefono"];
                        resultado.estado= (Boolean)dr["Estado"];
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

        public static bool EditarProveedor(DTOProveedor proveedor)
        {
            bool resultado = false;

            var sql = "UPDATE Proveedores SET Nombre_razonSocial = @nombre, Apellido_TipoSoc = @apellido, Direccion = @direccion, NumeroDireccion = @NumeroDireccion, Localidad =@localidad, IdProvincia = @idProvincia, Email =@email,Telefono = @telefono, Estado = @estado WHERE IdProveedor = @idProveedor";


            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();

            try
            {
                SqlCommand cmd = new SqlCommand(sql, conexion);

                cmd.Parameters.AddWithValue("@idProveedor", proveedor.idProveedor);
                cmd.Parameters.AddWithValue("@nombre", proveedor.nombre_razonSocial);
                cmd.Parameters.AddWithValue("@apellido", proveedor.apellido_TipoSoc);
                cmd.Parameters.AddWithValue("@direccion", proveedor.direccion);
                cmd.Parameters.AddWithValue("@NumeroDireccion", proveedor.numeroDireccion);
                cmd.Parameters.AddWithValue("@localidad", proveedor.localidad);
                cmd.Parameters.AddWithValue("@idProvincia", proveedor.idProvincia);
                cmd.Parameters.AddWithValue("@email", proveedor.email);
                cmd.Parameters.AddWithValue("@telefono", proveedor.telefono);
                cmd.Parameters.AddWithValue("@estado", proveedor.estado);



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

        public static bool EliminarProducto(DTOProveedor proveedor)
        {
            bool resultado = false;

            var sql = "UPDATE Proveedores SET Estado= 'false' WHERE IdProveedor = @idProveedor";

         
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();



            try
            {
                SqlCommand cmd = new SqlCommand(sql, conexion);

                cmd.Parameters.AddWithValue("@idProveedor", proveedor.idProveedor);

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
    }
}