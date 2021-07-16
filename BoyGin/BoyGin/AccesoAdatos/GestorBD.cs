using BoyGin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BoyGin.AccesoAdatos
{
    public class GestorBD
    {
        public void AgregarProducto(Producto productos)
        {
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            bool estado = true;
            try
            {
                var sql = "INSERT INTO Productos (NombreProducto, Descripcion, IdTipoProducto, PrecioProducto, StockProducto, Estado) VALUES (@nombre, @descripcion, @idTipoProducto, @precio, 0, @estado )";
                conexion.Open();
                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@nombre", productos.nombreProducto);
                cmd.Parameters.AddWithValue("@descripcion", productos.descripcion);
                cmd.Parameters.AddWithValue("@idTipoProducto", productos.idTipoProducto);
                cmd.Parameters.AddWithValue("@precio", productos.precioProducto);
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

      

        public List<DTOProducto> ListadoProducto()
        {
            var lista = new List<DTOProducto>();

            var sql = @"SELECT p.IdProducto, p.NombreProducto, p.Descripcion, t.NombreTipo, p.PrecioProducto, p.StockProducto
                        FROM Productos p
                        JOIN TiposProducto t ON p.IdTipoProducto=t.IdTipoProducto
                        WHERE P.Estado='true'";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                DTOProducto produ = new DTOProducto();

                produ.idProducto = (int)dr["IdProducto"];
                produ.nombreProducto = (string)dr["NombreProducto"];
                produ.descripcion = (string)dr["Descripcion"];
                produ.nombreTipo = (string)dr["NombreTipo"];
                //produ.Precio = (double)dr["precio"];
                produ.precioProducto = Convert.ToDouble(dr["PrecioProducto"].ToString());
                produ.stockProducto = (int)dr["StockProducto"];





                lista.Add(produ);
            }
            dr.Close();
            conexion.Close();

            return lista;
        }
        //combo de tipo de productos
        public List<TipoProducto> ListadoTipo()
        {
            var lista = new List<TipoProducto>();

            var sql = "SELECT * FROM TiposProducto";
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                TipoProducto tipo = new TipoProducto();

                tipo.idTipoProducto = (int)dr["IdTipoProducto"];
                tipo.nombreTipo = (string)dr["NombreTipo"];


                lista.Add(tipo);
            }

            dr.Close();
            conexion.Close();

            return lista;
        }

        public static DTOProducto BuscarProducto(int idProducto)
        {
            DTOProducto resultado = new DTOProducto();

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);


            try
            {
                var sql = @"SELECT * FROM Productos WHERE IdProducto =@IdProducto";
                conexion.Open();

                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.AddWithValue("@IdProducto", idProducto);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {

                    while (dr.Read())
                    {
                        resultado.idProducto = (int)dr["IdProducto"];
                        resultado.nombreProducto = (string)dr["NombreProducto"];
                        resultado.descripcion = (string)dr["Descripcion"];
                        resultado.idTipoProducto = (int)dr["IdTipoProducto"];
                        resultado.precioProducto = Convert.ToDouble(dr["PrecioProducto"].ToString());
                        resultado.stockProducto = (int)dr["StockProducto"];
                        resultado.estado = (Boolean)dr["Estado"];
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

        public static bool EditarProducto(DTOProducto producto)
        {
            bool resultado = false;

            var sql = " UPDATE Productos SET NombreProducto = @nombre, Descripcion = @descripcion, IdTipoProducto = @idTipoProducto, PrecioProducto =@precio, StockProducto = @stock, Estado = @estado  WHERE IdProducto = @idProducto";


            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();



            try
            {
                SqlCommand cmd = new SqlCommand(sql, conexion);

                cmd.Parameters.AddWithValue("@IdProducto", producto.idProducto);
                cmd.Parameters.AddWithValue("@nombre", producto.nombreProducto);
                cmd.Parameters.AddWithValue("@descripcion", producto.descripcion);
                cmd.Parameters.AddWithValue("@idTipoProducto", producto.idTipoProducto);
                cmd.Parameters.AddWithValue("@precio", producto.precioProducto);
                cmd.Parameters.AddWithValue("@stock", producto.stockProducto);
                cmd.Parameters.AddWithValue("@estado", producto.estado);


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

        public static bool EliminarProducto(DTOProducto productos)
        {
            bool resultado = false;

            var sql = "UPDATE Productos SET Estado = 'false' WHERE IdProducto = @idProducto";


            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();



            try
            {
                SqlCommand cmd = new SqlCommand(sql, conexion);

                cmd.Parameters.AddWithValue("@idProducto", productos.idProducto);

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

        public void ComprarMateriaPrima(CompraMateriaPrima compra)
        {
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);

            try
            {
                var sql = "INSERT INTO ComprasMateriaPrima (IdMateriaPrima, IdProveedor, FechaCompra, CantidadComprada) VALUES (@idMateriaPrima, @idProveedor, @fecha, @CantidadComprada)";
                conexion.Open();
                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@idMateriaPrima", compra.idMateriaPrima );
                cmd.Parameters.AddWithValue("@idProveedor", compra.idProveedor);
                cmd.Parameters.AddWithValue("@fecha", DateTime.UtcNow.ToString("dd-MM-yyyy"));
                cmd.Parameters.AddWithValue("@CantidadComprada", compra.cantidadComprada);

                cmd.ExecuteNonQuery();

                conexion.Close();
                ActualizarMateriaPrima(conexion, compra.idMateriaPrima, compra.cantidadComprada);


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

       
        public void ActualizarMateriaPrima(SqlConnection conexion, int Id, int CantidadUsuario)
        {

            
            var sql1 = "SELECT StockMateriaPrima FROM MateriasPrimas WHERE @IdMateriaPrima = IdMateriaPrima";
            int StockActual=0;
            int nuevoStock=0;

            conexion.Open();
            try
            {

                SqlCommand cmd = new SqlCommand(sql1, conexion);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IdMateriaPrima", Id);


                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {
 
                    while (dr.Read())
                    {
                       
                        StockActual = (int)dr["StockMateriaPrima"];
            
                    }
                }

                nuevoStock = StockActual + CantidadUsuario;

                conexion.Close();

                conexion.Open();

                var sql2 = "UPDATE MateriasPrimas SET StockMateriaPrima = @nuevoStock WHERE IdMateriaPrima = @IdMateriaPrima";

                SqlCommand cmd2 = new SqlCommand(sql2, conexion);
                cmd2.Parameters.Clear();

                
                cmd2.Parameters.AddWithValue("@nuevoStock", nuevoStock);
                cmd2.Parameters.AddWithValue("@IdMateriaPrima", Id);

                cmd2.ExecuteNonQuery();

                conexion.Close();
            }
            catch (Exception)
            {

                throw;
            }

        }

        //para combo de compra de materias primas
        public List<MateriaPrima> ComboMateriaPrima()
        {
            var lista = new List<MateriaPrima>();

            var sql = @"SELECT * FROM MateriasPrimas
                        WHERE Estado = 'true'";
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
                MateriaPrima.estado = (Boolean)dr["Estado"];

                lista.Add(MateriaPrima);
            }

            dr.Close();
            conexion.Close();

            return lista;
        }

        //para combo de compra de proveedor
        public List<Proveedor> ListadoProveedores()
        {
            var lista = new List<Proveedor>();

            var sql = "SELECT * FROM Proveedores WHERE Estado='true'";
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Proveedor proveedor = new Proveedor();

                proveedor.idProveedor = (int)dr["IdProveedor"];
                proveedor.nombre_razonSocial = (string)dr["Nombre_razonSocial"];
                proveedor.apellido_TipoSoc = (string)dr["Apellido_TipoSoc"];
                proveedor.direccion = (string)dr["Direccion"];
                proveedor.localidad = (string)dr["Localidad"];
                proveedor.idProvincia = (int)dr["IdProvincia"];
                proveedor.email = (string)dr["Email"];
                proveedor.telefono = (string)dr["Telefono"];
                proveedor.estado = (Boolean)dr["Estado"];

                lista.Add(proveedor);
            }

            dr.Close();
            conexion.Close();

            return lista;
        }
        //materias primas en stock
        public List<DTOMateriaPrima> ListadoMateriasPrimas()
        {
            var lista = new List<DTOMateriaPrima>();

            var sql = @"SELECT IdMateriaPrima, DescripcionMateriaPrima, StockMateriaPrima
                            FROM MateriasPrimas
                            WHERE Estado = 'true'
                            ORDER BY DescripcionMateriaPrima";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                DTOMateriaPrima materiales = new DTOMateriaPrima();

                materiales.idMateriaPrima = (int)dr["IdMateriaPrima"];
                materiales.descripcionMateriaPrima = (string)dr["DescripcionMateriaPrima"];
                materiales.stockMateriaPrima = (int)dr["StockMateriaPrima"];
            





                lista.Add(materiales);
            }
            dr.Close();
            conexion.Close();

            return lista;
        }

        public void AgregarMateriaPrima(MateriaPrima materiaPrima)
        {
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);

            try
            {
                var sql = "INSERT INTO MateriasPrimas (DescripcionMateriaPrima, StockMateriaPrima, Estado) VALUES (@Descripcion, 0, 'true')";
                conexion.Open();
                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.Clear();


                cmd.Parameters.AddWithValue("@Descripcion", materiaPrima.descripcionMateriaPrima);
                




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


        public static MateriaPrima BuscarMateriaPrima(int idMateriaPrima)
        {
            MateriaPrima resultado = new MateriaPrima();

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);


            try
            {
                var sql = @"SELECT * FROM MateriasPrimas WHERE IdMateriaPrima =@IdMateriaPrima";
                conexion.Open();

                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.AddWithValue("@IdMateriaPrima", idMateriaPrima);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {

                    while (dr.Read())
                    {
                        resultado.idMateriaPrima = (int)dr["IdMateriaPrima"];
                        resultado.descripcionMateriaPrima = (string)dr["DescripcionMateriaPrima"];
                        resultado.estado = (Boolean)dr["Estado"];


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

        public static bool EditarMateriaPrima(MateriaPrima materiaPrima)
        {
            bool resultado = false;

            var sql = " UPDATE MateriasPrimas SET DescripcionMateriaPrima = @descripcion, StockMateriaPrima = @stock, Estado = @estado WHERE IdMateriaPrima = @IdMateriaPrima";


            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();



            try
            {
                SqlCommand cmd = new SqlCommand(sql, conexion);

                cmd.Parameters.AddWithValue("@IdMateriaPrima", materiaPrima.idMateriaPrima);
                cmd.Parameters.AddWithValue("@descripcion", materiaPrima.descripcionMateriaPrima);
                cmd.Parameters.AddWithValue("@stock", materiaPrima.stockMateriaPrima);
                cmd.Parameters.AddWithValue("@estado", materiaPrima.estado);


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

        public static bool EliminaMateriaPrima(MateriaPrima materiaPrima)
        {
            bool resultado = false;

            var sql = "UPDATE MateriasPrimas SET Estado = 'false' WHERE IdMateriaPrima = @IdMateriaPrima";
           

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();



            try
            {
                SqlCommand cmd = new SqlCommand(sql, conexion);

                cmd.Parameters.AddWithValue("@IdMateriaPrima", materiaPrima.idMateriaPrima);

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

        //para descripcion de materia prima
        public List<MateriaPrima> ListaMateriaPrima()
        {
            var lista = new List<MateriaPrima>();

            var sql = @"SELECT IdMateriaPrima, DescripcionMateriaPrima
                        FROM MateriasPrimas
                        WHERE Estado = 'true'";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                MateriaPrima materiaPrima = new MateriaPrima();

                materiaPrima.idMateriaPrima = (int)dr["IdMateriaPrima"];
                materiaPrima.descripcionMateriaPrima = (string)dr["DescripcionMateriaPrima"];
          





                lista.Add(materiaPrima);
            }
            dr.Close();
            conexion.Close();

            return lista;
        }

        public List<Producto> ListadoPorTipoProducto( int IdTipoProducto)
        {
            var lista = new List<Producto>();

            var sql = @"SELECT NombreProducto, Descripcion, PrecioProducto, StockProducto
                        FROM Productos 
                        WHERE IdTipoProducto=@IdTipoProducto
                        AND Estado = 'true'";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);

            cmd.Parameters.AddWithValue("@IdTipoProducto", IdTipoProducto);


            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Producto produ = new Producto();
                //ListaPorTipoProducto produ = new ListaPorTipoProducto();

 
                produ.nombreProducto = (string)dr["NombreProducto"];
                produ.descripcion = (string)dr["Descripcion"];
                produ.precioProducto = Convert.ToDouble(dr["PrecioProducto"].ToString());
                produ.stockProducto = (int)dr["StockProducto"];
             

                lista.Add(produ);
            }
            dr.Close();
            conexion.Close();

            return lista;
        }

        public List<ReporteProducto> ReporteProductosVendidos()
        {
            var lista = new List<ReporteProducto>();

            var sql = @"select p.NombreProducto, sum(d.Cantidad) AS Cantidad
                            from DetallesFactura d
                            JOIN Productos p on d.IdProducto = p.IdProducto
                            group by p.NombreProducto";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                ReporteProducto ProductosVendidos = new ReporteProducto();

                ProductosVendidos.name = (string)dr["NombreProducto"];
                ProductosVendidos.y = (int)dr["Cantidad"];



                lista.Add(ProductosVendidos);
            }
            dr.Close();
            conexion.Close();

            return lista;
        }

        public int ReporteCantidadTotal()
        {
           

            var sql = @"SELECT sum (Cantidad) as cantidad
                        FROM DetallesFactura ";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            dr.Read();           
            int cantidadTotal = (int)dr["cantidad"];

            dr.Close();
            conexion.Close();

            return cantidadTotal;
        }


        public List<DTOProducto> CantidadPorTipoProducto()
        {
            var lista = new List<DTOProducto>();

            var sql = @"select t.NombreTipo, SUM(p.StockProducto) as stock
                        FROM Productos p
                        join TiposProducto t on p.IdTipoProducto=t.IdTipoProducto
                        where Estado='true'
                        group by t.NombreTipo";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                DTOProducto produ = new DTOProducto();
         
                produ.nombreTipo = (string)dr["NombreTipo"];
                produ.stockProducto = (int)dr["stock"];


                lista.Add(produ);
            }
            dr.Close();
            conexion.Close();

            return lista;
        }

        public List<ReporteProducto> StockProducto()
        {
            var lista = new List<ReporteProducto>();

            var sql = @"select NombreProducto, SUM(StockProducto) as stock
                        FROM Productos 
                        where estado='true'
                        group by NombreProducto";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ReporteProducto produ = new ReporteProducto();

                produ.name = (string)dr["NombreProducto"];
                produ.y = (int)dr["stock"];


                lista.Add(produ);
            }
            dr.Close();
            conexion.Close();

            return lista;
        }

    }

   

}