using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deportivo.Entities;
using Deportivo.DataAccessLayer;

namespace Deportivo.BusinessLayer
{
    public class MarcaService
    {
        private MarcaDao oMarcaDao;
        public MarcaService()
        {
            oMarcaDao = new MarcaDao();
        }
        public IList<Marca> ObtenerTodos()
        {
            return oMarcaDao.GetAll();
        }

    }
}
