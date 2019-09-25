using Deportivo.BusinessLayer;
using Deportivo.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Deportivo.GUILayer.Ventas
{
    public partial class frmABMProducto : Form
    {

        private FormMode formMode = FormMode.insert;

        private readonly ProductoService oProductoService;
        private readonly MarcaService oMarcaService;
        private Producto oProductoSelected;

        public frmABMProducto()
        {
            InitializeComponent();
            oProductoService = new ProductoService();
            oMarcaService = new MarcaService();
        }

        public enum FormMode
        {
            insert,
            update,
            delete
        }
        public void InicializarDetalleProducto(int idProducto)
        {
            var producto = oProductoService.ConsultarProductosPorId(idProducto);

            if (producto != null)
            {
                txtID.Text = producto.IdProducto.ToString();
                txtNombre.Text = producto.Nombre.ToString();
                txtCantidad.Text = producto.Cantidad.ToString();
                txtPrecioVenta.Text = Convert.ToString(producto.Precio_Venta);
                txtFechaAlta.Text = Convert.ToString(producto.Fecha_Alta);
                cboMarca.Text = producto.Marca.Descripcion;
               }

            
        }


        private void frmABMProducto_Load(System.Object sender, System.EventArgs e)
        {
            //carga el combo
            LlenarCombo(cboMarca, oMarcaService.ObtenerTodos(), "Descripcion", "IdMarca");
            switch (formMode)
            {
                case FormMode.insert:
                    {
                        this.Text = "Nuevo Producto";
                        txtID.Enabled = false;

                        break;
                    }

                case FormMode.update:
                    {
                        this.Text = "Actualizar Producto";
                        // Recuperar usuario seleccionado en la grilla 
                        MostrarDatos();
                        txtID.Enabled = false;
                        txtNombre.Enabled = true;
                        txtPrecioVenta.Enabled = true;
                        txtCantidad.Enabled = true;
                        txtFechaAlta.Enabled = true;
                        cboMarca.Enabled = true;
                        break;
                    }

                case FormMode.delete:
                    {
                        MostrarDatos();
                        this.Text = "Deshabilitar Producto";
                        txtID.Enabled = false;
                        txtNombre.Enabled = false;
                        txtPrecioVenta.Enabled = false;
                        txtCantidad.Enabled = false;
                        txtFechaAlta.Enabled = false;
                        cboMarca.Enabled = false;
                        break;
                    }
            }
        }

        public void SeleccionarProducto(FormMode op, Producto productoSelected)
        {
            formMode = op;
            oProductoSelected = productoSelected;
        }

        private void MostrarDatos()
        {
            if (oProductoSelected != null)
            {
                txtID.Text = oProductoSelected.IdProducto.ToString();
                txtNombre.Text = oProductoSelected.Nombre.ToString();
                txtCantidad.Text = oProductoSelected.Cantidad.ToString();
                txtPrecioVenta.Text = Convert.ToString(oProductoSelected.Precio_Venta);
                //ToShortDateString() deja solo la fecha sin la hora
                txtFechaAlta.Text = Convert.ToString(oProductoSelected.Fecha_Alta.ToShortDateString());
                cboMarca.Text = oProductoSelected.Marca.Descripcion;
            }
        }

        private void btnAceptar_Click(System.Object sender, System.EventArgs e)
        {
            switch (formMode)
            {
                case FormMode.insert:
                    {
                        
                            if (ValidarCampos())
                            {
                                var oProducto = new Producto();
                                oProducto.Nombre = txtNombre.Text;
                                oProducto.Cantidad = Convert.ToInt32(txtCantidad.Text);
                                oProducto.Precio_Venta = Convert.ToDouble(txtPrecioVenta.Text);
                                //oProducto.Fecha_Alta = fechaSinHora;
                                oProducto.Marca = new Marca();
                                oProducto.Marca.IdMarca = (int)cboMarca.SelectedValue;

                                if (oProductoService.CrearProducto(oProducto))
                                {
                                    MessageBox.Show("Producto creado", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                }
                            }
                            break;
                        
                    }

                case FormMode.update:
                    {
                        if (ValidarCampos())
                        {
                            oProductoSelected.Nombre = txtNombre.Text;
                            oProductoSelected.Cantidad = Convert.ToInt32(txtCantidad.Text);
                            oProductoSelected.Precio_Venta = Convert.ToDouble(txtPrecioVenta.Text);
                            //oProductoSelected.Fecha_Alta = fechaSinHora;
                            oProductoSelected.Marca.IdMarca = (int)cboMarca.SelectedValue;

                            if (oProductoService.ActualizarProducto(oProductoSelected))
                            {
                                MessageBox.Show("Producto actualizado!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Dispose();
                            }
                            else
                                MessageBox.Show("Error al actualizar el producto!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        break;
                    }

                case FormMode.delete:
                    {
                        if (MessageBox.Show("Seguro que desea habilitar/deshabilitar el producto seleccionado?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            
                            if (oProductoService.ModificarBorradoProducto(oProductoSelected))
                            {
                                MessageBox.Show("Producto Habilitado/Deshabilitado!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                            else
                                MessageBox.Show("Error al actualizar el producto!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        break;
                    }
            }
        }

        private bool ValidarCampos()
        {
            // campos obligatorios
            if (txtNombre.Text == string.Empty)
            {
                txtNombre.BackColor = Color.Red;
                txtNombre.Focus();
                return false;
            }
            else
                txtNombre.BackColor = Color.White;

            return true;
        }

        private bool ExisteProducto()
        {
            //revisar
            int varid = 2;
     
            return oProductoService.ConsultarProductosPorId(varid) != null;
        }


        private void LlenarCombo(ComboBox cbo, Object source, string display, String value)
        {
            cbo.DataSource = source;
            cbo.DisplayMember = display;
            cbo.ValueMember = value;
            cbo.SelectedIndex = -1;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSalir_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboMarca_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

    }
}
