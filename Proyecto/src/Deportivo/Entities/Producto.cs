using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deportivo.Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
        public Marca Marca { get; set; }
        public int Cantidad{ get; set; }
        public double Precio_Venta { get; set; }
        public DateTime Fecha_Alta { get; set; }

    }
}
