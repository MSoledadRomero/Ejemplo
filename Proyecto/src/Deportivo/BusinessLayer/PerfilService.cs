using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deportivo.Entities;
using Deportivo.DataAccessLayer;

namespace Deportivo.BusinessLayer
{
    public class PerfilService
    {
        private PerfilDao oPerfilDao;
        public PerfilService()
        {
            oPerfilDao = new PerfilDao();
        }
        public IList<Perfil> ObtenerTodos()
        {
            return oPerfilDao.GetAll();
        }        
    }
}
