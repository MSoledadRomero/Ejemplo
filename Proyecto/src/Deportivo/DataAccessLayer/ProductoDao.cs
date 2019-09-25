

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Deportivo.Entities;
using System.Data;

namespace Deportivo.DataAccessLayer
{
    public class ProductoDao
    {
        public Producto GetProductoById(int idProducto)
        {
            var strSql = String.Concat("SELECT producto.id_producto as id, ",
                                      "        producto.nombre,",
                                      "        producto.id_marca,",
                                      "        marca.descripcion as marca, ",
                                      "        producto.cantidad, ",
                                      "        producto.precio_venta, ",
                                      "        producto.fecha_alta",
                                      "        FROM Productos as producto",
                                      "  INNER JOIN Marcas as marca ON  marca.id_marca = producto.id_marca",
                                      " WHERE producto.borrado=0 AND producto.id_producto = " + idProducto.ToString());

            return MappingProducto(DBHelper.GetDBHelper().ConsultaSQL(strSql).Rows[0]);
        }

        public IList<Producto> GetProductoByFiltersCondiciones(String condiciones)
        {
            List<Producto> listadoProductos = new List<Producto>();
            String sqlcondiciones = condiciones;

            var strSql = String.Concat("SELECT producto.id_producto as id, ",
                                      "        producto.nombre,",
                                      "        producto.id_marca,",
                                      "        marca.descripcion as marca, ",
                                      "        producto.cantidad,",
                                      "        producto.precio_venta, ",
                                      "        producto.fecha_alta",
                                      "        FROM Productos as producto",
                                      "  INNER JOIN Marcas as marca ON  marca.id_marca = producto.id_marca",
                                      " WHERE producto.borrado=0 ");

            strSql += sqlcondiciones;

            //sin parametros
            strSql += "ORDER BY producto.nombre DESC";

            var resultadoConsulta = (DataRowCollection)DBHelper.GetDBHelper().ConsultaSQL(strSql).Rows;

            foreach (DataRow row in resultadoConsulta)
            {
                listadoProductos.Add(MappingProducto(row));
            }

            return listadoProductos;
        }





        private Producto MappingProducto(DataRow row)
        {
            Producto oProducto = new Producto();
            //oProducto es el objeto instanciado
            //oProducto.IdProducto donde IdProducto es el nombre de la propiedad en entity
            //Convert.ToInt32(row["id_producto"].ToString());  convierte a entero, doble o fecha lo que obtuvo en el select
            oProducto.IdProducto = Convert.ToInt32(row["id"].ToString());
            oProducto.Nombre = row["nombre"].ToString();

            oProducto.Marca = new Marca();
            //convierte a entero
            oProducto.Marca.IdMarca = Convert.ToInt32(row["id_marca"].ToString());
            //no es necesario convertir
            oProducto.Marca.Descripcion = row["marca"].ToString();

            oProducto.Cantidad = Convert.ToInt32(row["cantidad"].ToString());
            //convierte a doble
            oProducto.Precio_Venta = Convert.ToDouble(row["precio_venta"].ToString());
            //convierte a fecha
            oProducto.Fecha_Alta = Convert.ToDateTime(row["fecha_alta"].ToString());

            return oProducto;
        }


        internal bool Create(Producto oProducto)
        {
            string str_sql = "INSERT INTO Productos (nombre, id_marca, cantidad, precio_venta,fecha_alta, borrado)" +
             " VALUES (" +
             "'" + oProducto.Nombre + "'" + "," +
             oProducto.Marca.IdMarca+ "," +
              oProducto.Cantidad + "," +
               oProducto.Precio_Venta + "," +
                "getdate()" + "," +
               " 0 "+
                ")";

            return (DBHelper.GetDBHelper().EjecutarSQL(str_sql) == 1);
        }

        internal bool Update(Producto oProducto)
        {
            
            string str_sql = "UPDATE Productos " +
                             "SET nombre=" + "'" + oProducto.Nombre + "'" + "," +
                             " cantidad=" + oProducto.Cantidad+ "," +
                             " precio_venta=" +   oProducto.Precio_Venta  + "," +
                             //" fecha_alta=" + "'" + oProducto.Fecha_Alta + "'" + "," +
                             " id_marca=" + oProducto.Marca.IdMarca +
                             " WHERE id_producto=" + oProducto.IdProducto;

            return (DBHelper.GetDBHelper().EjecutarSQL(str_sql) == 1);
        }


        internal bool Delete(Producto oProducto)
        {

            string str_sql = "UPDATE Productos " +
                             "SET borrado=1 "+
                             " WHERE id_producto=" + oProducto.IdProducto;

            return (DBHelper.GetDBHelper().EjecutarSQL(str_sql) == 1);
        }
    }

}