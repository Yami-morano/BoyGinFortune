

using BoyGin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BoyGin.AccesoAdatos
{
    public class GestorBDProduccion
    {
        public string AgregarHistorial(HistorialProduccion historialProduccion)
        {
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);

            var sql2 = "INSERT INTO HistorialProduccion (FechaProduccion, CantidadProducida) VALUES (@fechaProduccion, @cantidadProducida) SELECT SCOPE_IDENTITY();";
            conexion.Open();
            SqlCommand cmd2 = new SqlCommand(sql2, conexion);
            cmd2.Parameters.Clear();

            cmd2.Parameters.AddWithValue("@fechaProduccion", DateTime.UtcNow.ToString("dd-MM-yyyy"));
            cmd2.Parameters.AddWithValue("@cantidadProducida", historialProduccion.cantidadProducida);


            //reemplazo el execute nonquery
            SqlDataReader dr = cmd2.ExecuteReader();
            dr.Read();
            //obtengo el id de la ultima fila creada en el historial de produccion
            string idProduccionCreado = dr[0].ToString();


            conexion.Close();

            return idProduccionCreado;
        }

        
        public void AgregarProduccion(Produccion_MateriasPrimas produccion, HistorialProduccion historialProduccion, int materiaPrimaId, int cantidadMateriaPrima, string idProduccionCreado)
        {
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);

            try
            {

                var sql = "INSERT INTO Produccion_MateriasPrimas (IdProduccion, IdMateriasPrimas, CantidadMateriaUsada, IdProducto) VALUES (@idProduccion, @idMateriasPrimas, @cantidadMateriaUsada, @idProducto)";
                conexion.Open();
                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@idProduccion", idProduccionCreado);
                cmd.Parameters.AddWithValue("@idMateriasPrimas", materiaPrimaId);
                cmd.Parameters.AddWithValue("@cantidadMateriaUsada", cantidadMateriaPrima);
                cmd.Parameters.AddWithValue("@idProducto", produccion.idProducto);

                cmd.ExecuteNonQuery();

                conexion.Close();

                ActualizarStockMateriaPrima(conexion, materiaPrimaId, cantidadMateriaPrima);

                //ActualizarStockProducto(conexion, produccion.idProducto, historialProduccion.cantidadProducida);


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


        public void ActualizarStockMateriaPrima(SqlConnection conexion, int id, int cantidadIngresada)
        {

            var sql1 = "SELECT StockMateriaPrima FROM MateriasPrimas WHERE @IdMateriaPrima = IdMateriaPrima";
            int StockActual = 0;
            int nuevoStock = 0;

            conexion.Open();
            try
            {

                SqlCommand cmd = new SqlCommand(sql1, conexion);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IdMateriaPrima", id);


                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {

                    while (dr.Read())
                    {

                        StockActual = (int)dr["StockMateriaPrima"];

                    }
                }

                nuevoStock = StockActual - cantidadIngresada;

                conexion.Close();

                conexion.Open();

                var sql2 = "UPDATE MateriasPrimas SET StockMateriaPrima = @nuevoStock WHERE IdMateriaPrima = @IdMateriaPrima";

                SqlCommand cmd2 = new SqlCommand(sql2, conexion);
                cmd2.Parameters.Clear();


                cmd2.Parameters.AddWithValue("@nuevoStock", nuevoStock);
                cmd2.Parameters.AddWithValue("@IdMateriaPrima", id);

                cmd2.ExecuteNonQuery();

                conexion.Close();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void ActualizarStockProducto( int idProducto, int cantidadProducida)
        {
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);

            var sql2 = "SELECT StockProducto FROM Productos WHERE @idProducto = IdProducto";
            int StockActual = 0;
            int nuevoStock = 0;

            conexion.Open();

            try
            {

                SqlCommand cmd2 = new SqlCommand(sql2, conexion);
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@idProducto", idProducto);


                SqlDataReader dr = cmd2.ExecuteReader();

                if (dr != null)
                {

                    while (dr.Read())
                    {

                        StockActual = (int)dr["StockProducto"];

                    }
                }

                nuevoStock = StockActual + cantidadProducida;

                conexion.Close();

                conexion.Open();

                var sql3 = "UPDATE Productos SET StockProducto = @nuevoStock WHERE IdProducto = @idProducto";

                SqlCommand cmd3 = new SqlCommand(sql3, conexion);
                cmd3.Parameters.Clear();


                cmd3.Parameters.AddWithValue("@nuevoStock", nuevoStock);
                cmd3.Parameters.AddWithValue("@idProducto", idProducto);

                cmd3.ExecuteNonQuery();

                conexion.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //combo de materias primas
        public List<MateriaPrima> ListaMateriaPrima()
        {
            var lista = new List<MateriaPrima>();

            var sql = "SELECT * FROM MateriasPrimas WHERE Estado= 'true'";
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                MateriaPrima MateriaPrima = new MateriaPrima();

                MateriaPrima.idMateriaPrima = (int)dr["IdMateriaPrima"];
                MateriaPrima.descripcionMateriaPrima = (string)dr["DescripcionMateriaPrima"];
                MateriaPrima.stockMateriaPrima = (int)dr["StockMateriaPrima"];


                lista.Add(MateriaPrima);
            }

            dr.Close();
            conexion.Close();

            return lista;
        }
        //combo productos
        public List<Producto> ListaProducto()
        {
            var lista = new List<Producto>();

            var sql = "SELECT IdProducto, NombreProducto FROM Productos WHERE Estado = 'true'";
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Producto producto = new Producto();

                producto.idProducto = (int)dr["IdProducto"];
                producto.nombreProducto = (string)dr["NombreProducto"];


                lista.Add(producto);
            }

            dr.Close();
            conexion.Close();

            return lista;
        }

      

        public List<DTOProduccion_MateriasPrimas> ListadoProduccion()
        {
            var lista = new List<DTOProduccion_MateriasPrimas>();

            var sql = @"SELECT h.IdProduccion, h.FechaProduccion, p.NombreProducto, h.CantidadProducida
                        FROM HistorialProduccion h
                        JOIN Produccion_MateriasPrimas pm ON h.IdProduccion=pm.IdProduccion
                        JOIN Productos p ON p.IdProducto=pm.IdProducto
                        group by h.IdProduccion, h.FechaProduccion, p.NombreProducto, h.CantidadProducida
                        order by h.IdProduccion";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                DTOProduccion_MateriasPrimas produccion = new DTOProduccion_MateriasPrimas();

                produccion.idProduccion = (int)dr["IdProduccion"];
                produccion.fechaProduccion = (DateTime)dr["FechaProduccion"];
                produccion.nombreProducto = (string)dr["NombreProducto"];
                produccion.cantidadProducida = (int)dr["CantidadProducida"];


                lista.Add(produccion);
            }
            dr.Close();
            conexion.Close();

            return lista;
        }

        public List<DTOProduccion_MateriasPrimas> ListadoDetallesProduccion(int idProduccion)
        {
            var lista = new List<DTOProduccion_MateriasPrimas>();

            var sql = @"SELECT h.IdProduccion,  m.DescripcionMateriaPrima, pm.CantidadMateriaUsada
                        FROM HistorialProduccion h
                        JOIN Produccion_MateriasPrimas pm ON h.IdProduccion=pm.IdProduccion
                        join MateriasPrimas m ON m.IdMateriaPrima=pm.IdMateriasPrimas
                        where h.IdProduccion=@IdProduccion";




            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);

            cmd.Parameters.AddWithValue("@IdProduccion", idProduccion);

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                DTOProduccion_MateriasPrimas detalleProduccion = new DTOProduccion_MateriasPrimas();
            
                detalleProduccion.idProduccion = (int)dr["IdProduccion"];
                detalleProduccion.descripcionMateriaPrima = (string)dr["DescripcionMateriaPrima"];
                detalleProduccion.cantidadMateriaPrimaUsada = (int)dr["CantidadMateriaUsada"];
            

                lista.Add(detalleProduccion);

            }

            dr.Close();
            conexion.Close();

            return lista;
        }

        public string verificadorStock(int id)
        {
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            var sql = "SELECT StockMateriaPrima FROM MateriasPrimas WHERE IdMateriaPrima = @id";
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            string currentStock = dr["StockMateriaPrima"].ToString();
            dr.Close();
            conexion.Close();
            return currentStock;
        }

        //public List<ReporteProducto> ReporteProduccion()
        //{
        //    var lista = new List<ReporteProducto>();

        //    var sql = @"SELECT  P.NombreProducto, SUM(HP.CantidadProducida) AS Cantidad
        //                FROM HistorialProduccion HP
        //                JOIN Produccion_MateriasPrimas MP ON MP.IdProduccion=HP.IdProduccion
        //                JOIN Productos P ON P.IdProducto=MP.IdProducto
        //                GROUP BY P.NombreProducto";

        //    SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
        //    conexion.Open();
        //    SqlCommand cmd = new SqlCommand(sql, conexion);
        //    SqlDataReader dr = cmd.ExecuteReader();

        //    while (dr.Read())
        //    {
        //        ReporteProducto produccion = new ReporteProducto();
        //        produccion.name = (string)dr["NombreProducto"];
        //        produccion.y = Convert.ToInt32(dr["Cantidad"].ToString());

        //        lista.Add(produccion);
        //    }
        //    dr.Close();
        //    conexion.Close();

        //    return lista;
        //}


        public List<ReporteProducto> ReporteMateriasPrimas()
        {
            var lista = new List<ReporteProducto>();

            var sql = @"SELECT m.DescripcionMateriaPrima, SUM(PM.CantidadMateriaUsada) AS cantidadUsada
                        from Produccion_MateriasPrimas pm
                        join MateriasPrimas m ON pm.IdMateriasPrimas = m.IdMateriaPrima
                        GROUP BY m.DescripcionMateriaPrima";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ReporteProducto materiaPrima = new ReporteProducto();
                materiaPrima.name = (string)dr["DescripcionMateriaPrima"];
                materiaPrima.y = Convert.ToInt32(dr["cantidadUsada"].ToString());

                lista.Add(materiaPrima);
            }
            dr.Close();
            conexion.Close();

            return lista;
        }

        //combo año
        //public List<años> años()
        //{
        //    var lista = new List<años>();

        //    var sql = "select DISTINCT YEAR(FechaProduccion) as ano from HistorialProduccion";
        //    SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
        //    conexion.Open();
        //    SqlCommand cmd = new SqlCommand(sql, conexion);
        //    SqlDataReader dr = cmd.ExecuteReader();

        //    while (dr.Read())
        //    {
        //        años fecha = new años();

        //        fecha.año = (int)dr["ano"];
           


        //        lista.Add(fecha);
        //    }

        //    dr.Close();
        //    conexion.Close();

        //    return lista;
        //}

        
    }
}