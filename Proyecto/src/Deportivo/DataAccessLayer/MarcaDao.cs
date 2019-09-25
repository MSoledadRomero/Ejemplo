

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
    public class MarcaDao
    {
        public IList<Marca> GetAll()
        {
            List<Marca> listadoMarcas = new List<Marca>();

            var strSql = "SELECT id_marca, descripcion from Marcas where borrado=0";

            var resultadoConsulta = DBHelper.GetDBHelper().ConsultaSQL(strSql);

            foreach (DataRow row in resultadoConsulta.Rows)
            {
                listadoMarcas.Add(MappingMarcas(row));
            }

            return listadoMarcas;
        }

        private Marca MappingMarcas(DataRow row)
        {
            Marca oMarca = new Marca
            {
                IdMarca = Convert.ToInt32(row["id_marca"].ToString()),
                Descripcion = row["descripcion"].ToString()
            };

            return oMarca;
        }
    }

}