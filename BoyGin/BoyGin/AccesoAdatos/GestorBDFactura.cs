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
    public class GestorBDFactura
    {
        public int buscarUsuario()
        {
            var currentEmail = HttpContext.Current.User.Identity.Name;
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            
            var sql = @"SELECT C.IdCliente
                        FROM AspNetUsers a 
                        JOIN Clientes c ON A.Id=C.IdUsuario
                        WHERE a.Email = @email";

            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@email", currentEmail);

            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            var currentIdUser = Convert.ToInt32(dr["IdCliente"].ToString());
            dr.Close();
            conexion.Close();
            return currentIdUser;
        }

        public string agregarFactura()
        {
            try
            {
                SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
                var sql2 = "INSERT INTO Facturas (IdCliente, FechaFactura) VALUES (@idCliente, @fecha) SELECT SCOPE_IDENTITY();";
                conexion.Open();
                SqlCommand cmd2 = new SqlCommand(sql2, conexion);
                cmd2.Parameters.Clear();
                int id = buscarUsuario();
                Console.WriteLine("hola");
                //FALTA LOGICA DE OBTENCION DE ID DE CLIENTE (INTUYO QUE DEBE SER MEDIANTE LA SESION)
                //DE MOMENTO UTILIZO UNO CREADO EN LA BASE DE DATOS QUE SE IDENTIFICA CON EL ID 1
                cmd2.Parameters.AddWithValue("@idCliente", id);
                cmd2.Parameters.AddWithValue("@fecha", DateTime.UtcNow.ToString("dd-MM-yyyy"));

                //reemplazo el execute nonquery
                SqlDataReader dr = cmd2.ExecuteReader();
                dr.Read();
                //obtengo el id de la ultima fila creada en el historial de produccion
                string idFacturaCreado = dr[0].ToString();

                conexion.Close();

                return idFacturaCreado;
            }
            catch
            {
                throw;
            }
        }

        public void AgregarDetalleFactura(DetalleFactura detalleFactura, string idFacturaCreado)
        {
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);

            try
            {
                var sql = "INSERT INTO DetallesFactura (IdFactura, IdProducto, PrecioUnitario, Cantidad) VALUES (@idFactura, @idProducto, @precioUnitario, @cantidad)";
                conexion.Open();
                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@idFactura", Convert.ToInt32(idFacturaCreado));
                cmd.Parameters.AddWithValue("@idProducto", detalleFactura.idProducto);
                cmd.Parameters.AddWithValue("@precioUnitario", detalleFactura.precioUnitario);
                cmd.Parameters.AddWithValue("@cantidad", detalleFactura.cantidad);

                cmd.ExecuteNonQuery();

                conexion.Close();

                ActualizarStockProducto(conexion, detalleFactura.idProducto, detalleFactura.cantidad);

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


        public void ActualizarStockProducto(SqlConnection conexion, int idProducto, int cantidad)
        {

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

                nuevoStock = StockActual - cantidad;

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

        //combo de clientes
        public List<Cliente> ListaClientes()
        {
            var lista = new List<Cliente>();

            var sql = "SELECT IdCliente, Nombre + ' ' + Apellido AS nombreCliente FROM Clientes";
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Cliente cliente = new Cliente();

                cliente.idCliente = (int)dr["IdCliente"];
                cliente.nombreCliente = (string)dr["nombreCliente"];
            


                lista.Add(cliente);
            }

            dr.Close();
            conexion.Close();

            return lista;
        }
        //combo productos
        public List<Producto> ListaProducto()
        {
            var lista = new List<Producto>();

            var sql = "SELECT IdProducto, NombreProducto FROM Productos";
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

        public List<DTODetalleFactura> ListadoFacturacion()
        {
            var lista = new List<DTODetalleFactura>();

            var sql = @"SELECT f.IdFactura,f.FechaFactura, c.Nombre+' '+c.Apellido as nombreCliente, SUM(df.Cantidad*df.PrecioUnitario) as Total
                                FROM Facturas f
                                JOIN Clientes c ON f.IdCliente=c.IdCliente
                                join DetallesFactura df on f.IdFactura = df.IdFactura
                                group by f.IdFactura,f.FechaFactura, c.Nombre+' '+c.Apellido
                                order by f.IdFactura";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                DTODetalleFactura factura = new DTODetalleFactura();

                factura.idFactura = (int)dr["IdFactura"];
                factura.fechaFactura = (DateTime)dr["FechaFactura"];
                factura.nombreCliente = (string)dr["nombreCliente"];
                factura.total = Convert.ToDouble(dr["Total"].ToString());


                lista.Add(factura);
            }
            dr.Close();
            conexion.Close();

            return lista;
        }


        public Carrito CarritoProducto(int idProducto, int cantidad)
        {
            var lista = new List<Carrito>();

            var sql = @"SELECT IdProducto, NombreProducto, PrecioProducto, StockProducto
                        FROM Productos 
                        WHERE IdProducto =@IdProducto";
            
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("@IdProducto", idProducto);
            
            SqlDataReader dr = cmd.ExecuteReader();
            Carrito carritoProducto = new Carrito();

            while (dr.Read())
            {
                carritoProducto.idProducto = (int)dr["IdProducto"];
                carritoProducto.nombreProducto = (string)dr["NombreProducto"];
                carritoProducto.precioProducto = Convert.ToDouble(dr["PrecioProducto"].ToString());
                carritoProducto.stockProducto = (int)dr["StockProducto"];
                carritoProducto.cantidadRequerida = cantidad;
            }

            dr.Close();
            conexion.Close();

            return carritoProducto;
        }


        public List<DTODetalleFactura> ListadoDetalles(int idFactura)
        {
            var lista = new List<DTODetalleFactura>();

            var sql = @"SELECT f.IdFactura, c.Nombre +' '+ c.Apellido AS nombreCliente, f.FechaFactura, p.NombreProducto, df.PrecioUnitario, df.Cantidad, (df.PrecioUnitario*df.Cantidad) AS SUBTOTAL
                        FROM Facturas f
                        JOIN Clientes c ON c.IdCliente=f.IdCliente
                        JOIN DetallesFactura df ON f.IdFactura=df.IdFactura
                        JOIN Productos P ON DF.IdProducto=P.IdProducto
						where df.IdFactura= @IdFactura";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);

            cmd.Parameters.AddWithValue("@IdFactura", idFactura);

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                DTODetalleFactura detalleFactura = new DTODetalleFactura();
                string producto = (string)dr["NombreProducto"];
                detalleFactura.idFactura = (int)dr["IdFactura"];
                detalleFactura.nombreCliente = (string)dr["nombreCliente"];
                detalleFactura.fechaFactura = (DateTime)dr["FechaFactura"];
                detalleFactura.nombreProducto = (string)dr["NombreProducto"];
                detalleFactura.precioUnitario = Convert.ToDouble(dr["PrecioUnitario"].ToString());
                detalleFactura.cantidad = (int)dr["Cantidad"];
                detalleFactura.Subtotal = Convert.ToDouble(dr["SUBTOTAL"].ToString());

                lista.Add(detalleFactura);

            }

            dr.Close();
            conexion.Close();

            return lista;
        }
        //para listado de facturas por fecha
        public List<DTODetalleFactura> BuscarFactura(string cadena, DateTime fi, DateTime ff)
        {
            List<DTODetalleFactura > facturacion = new List<DTODetalleFactura>();

            try
            {
                var sql = @"SELECT f.IdFactura, c.Nombre+' '+c.Apellido as NombreCliente, f.FechaFactura, SUM(df.PrecioUnitario*df.Cantidad) As Total
                            FROM Facturas f
                            JOIN DetallesFactura df ON f.IdFactura=df.IdFactura
                            JOIN Clientes c ON f.IdCliente= c.IdCliente
                            where c.Nombre like @cadena AND f.FechaFactura between @fi and @ff
                            GROUP BY f.IdFactura, c.Nombre+' '+c.Apellido, f.FechaFactura";

                SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
                conexion.Open();
                SqlCommand cmd = new SqlCommand(sql, conexion);
                cmd.Parameters.Add("@cadena", SqlDbType.VarChar);
                cmd.Parameters.Add("@fi", SqlDbType.DateTime);
                cmd.Parameters.Add("@ff", SqlDbType.DateTime);

                cmd.Parameters["@cadena"].Value = /*"%" + */cadena + "%";
                cmd.Parameters["@fi"].Value = fi;
                cmd.Parameters["@ff"].Value = ff;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DTODetalleFactura detalleFactura = new DTODetalleFactura();
                    detalleFactura.idFactura = (int)dr["IdFactura"];
                    detalleFactura.nombreCliente = (string)dr["nombreCliente"];
                    detalleFactura.fechaFactura = (DateTime)dr["FechaFactura"];
                    detalleFactura.total = Convert.ToDouble(dr["Total"].ToString());
                    facturacion.Add(detalleFactura);
                }

                dr.Close();
                conexion.Close();

            }
            catch (Exception)
            {

                throw;
            }
          
            return facturacion;
        }

        public List<DTODetalleFactura> Recibo(string idFactura)
        {
            var lista = new List<DTODetalleFactura>();

            
            var sql = @"SELECT f.IdFactura, c.Nombre +' '+ c.Apellido AS nombreCliente, f.FechaFactura, p.NombreProducto, df.PrecioUnitario, df.Cantidad, (df.PrecioUnitario*df.Cantidad) AS SUBTOTAL
                        FROM Facturas f
                        JOIN Clientes c ON c.IdCliente=f.IdCliente
                        JOIN DetallesFactura df ON f.IdFactura=df.IdFactura
                        JOIN Productos P ON DF.IdProducto=P.IdProducto
						where df.IdFactura= @IdFactura";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);

            cmd.Parameters.AddWithValue("@IdFactura", idFactura);

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                DTODetalleFactura detalleFactura = new DTODetalleFactura();
                string producto = (string)dr["NombreProducto"];
                detalleFactura.idFactura = (int)dr["IdFactura"];
                detalleFactura.nombreCliente = (string)dr["nombreCliente"];
                detalleFactura.fechaFactura = (DateTime)dr["FechaFactura"];
                detalleFactura.nombreProducto = (string)dr["NombreProducto"];
                detalleFactura.precioUnitario = Convert.ToDouble(dr["PrecioUnitario"].ToString());
                detalleFactura.cantidad = (int)dr["Cantidad"];
                detalleFactura.Subtotal = Convert.ToDouble(dr["SUBTOTAL"].ToString());

                lista.Add(detalleFactura);

            }

            dr.Close();
            conexion.Close();

            return lista;
        }

        public string validadorStock(int id)
        {
            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            var sql = "SELECT StockProducto FROM Productos WHERE IdProducto = @id";
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            string currentStock = dr["StockProducto"].ToString();
            dr.Close();
            conexion.Close();
            return currentStock;
        }

        public List<ReporteFacturacion> ReporteCantidadVentasMes()
        {
            var lista = new List<ReporteFacturacion>();

            var sql = @"SELECT MONTH (FechaFactura) AS Mes, count (*) as Cantidad
                            FROM Facturas f
                            group by MONTH (FechaFactura)";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                ReporteFacturacion factura = new ReporteFacturacion();

                //factura.fecha = (int)dr["Mes"];
                //factura.cantidad = Convert.ToInt32(dr["Cantidad"].ToString());



                lista.Add(factura);
            }
            dr.Close();
            conexion.Close();

            return lista;
        }

        public ReporteFacturacion ReporteFacturadoPorMes(string ano)
        {
            var lista = new List<ReporteFacturacion>();

            var sql = @"select MONTH (FechaFactura) AS Mes, SUM (df.PrecioUnitario*df.Cantidad) Total
                        from DetallesFactura df
                        JOIN Facturas f ON df.IdFactura=f.IdFactura
                        where f.FechaFactura LIKE @ano
                        GROUP BY MONTH (FechaFactura)";

            SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            cmd.Parameters.Add("@ano", SqlDbType.VarChar);
           
            cmd.Parameters["@ano"].Value = "%" + ano + "%";
            SqlDataReader dr = cmd.ExecuteReader();

            Double[] ganancias = new Double[13];
            ReporteFacturacion factura = new ReporteFacturacion();
            factura.name = ano;
            while (dr.Read())
            {
                ganancias[(int)dr["Mes"]] = Convert.ToDouble(dr["Total"].ToString());
                //factura.fecha = (int)dr["Mes"];
                //factura.total= Convert.ToDouble(dr["Total"].ToString());

                //lista.Add(factura);
            }
            factura.data = ganancias;
            dr.Close();
            conexion.Close();

            return factura;
        }


    }
}







    
