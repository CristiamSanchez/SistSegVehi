using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using VisitaVehicular.Forms;

namespace VisitaVehicular.Forms
{
    public partial class ListadoVehicular : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenarGVV();
                llenaDDLEstado();
                llenaDDL();
                // Inicialización de la página
            }
            else
            {
                #region codigo anterior

                //try
                //{
                //    string eventTarget = Request["__EVENTTARGET"];
                //    string eventArgument = Request["__EVENTARGUMENT"];
                //    if (!string.IsNullOrEmpty(eventTarget) && eventTarget.Contains("gvU"))
                //    {
                //        string[] args = eventArgument.Split(':');
                //        if (args.Length == 2)
                //        {
                //            string visitaId = args[0];
                //            string action = args[1];
                //            // Verifica el tipo de acción (confirmar o cancelar)
                //            if (action == "confirmar")
                //            {
                //                // Lógica para registrar la salida
                //                Response.Write("<script>alert('Visita ID: " + visitaId + " - Acción: " + action + "');</script>");
                //            }
                //            else if (action == "cancelar")
                //            {
                //                // Lógica para cancelar la acción
                //                Response.Write("<script>alert('Acción cancelada para Visita ID: " + visitaId + "');</script>");
                //            }
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    // Manejo del error
                //    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                //}
                #endregion

            }
            evalT();
        }

        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if (txtFiltro.Text.Length <= 0)
            {
                llenarGVV();
            }
            else
            {
                BusquedaTxt();
            }
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (txtFiltro.Text.Length <= 0)
            {
                llenarGVV();
            }
            else
            {
                BusquedaTxt();
            }
        }

        protected void BusquedaTxt()
        {
            string criterioBusqueda = txtFiltro.Text.Trim();

            //string placa = Placa.Value.Trim();
            // 1. Validar la cantidad de caracteres (debe ser 7)
            if (criterioBusqueda.Length != 7)
            {
                // Mostrar mensaje de error
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','La placa debe tener exactamente 7 caracteres!','warning')", true);

                return;
            }
            // 2. Validar que solo contenga números y letras
            if (!System.Text.RegularExpressions.Regex.IsMatch(criterioBusqueda, "^[a-zA-Z0-9]+$"))
            {
                // Mostrar mensaje de error
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','La placa solo puede contener letras y números!','warning')", true);
                return;
            }
            // 3. Convertir las letras a mayúsculas
            criterioBusqueda = criterioBusqueda.ToUpper();
            // 4. Validar que no haya espacios vacíos (ya cubierto por el Regex anterior)
            // Si la validación pasa, asigna el valor procesado de vuelta al control o úsalo como necesites
            txtFiltro.Text = criterioBusqueda;

            using (SqlConnection conexion = CNN.GetConnection())
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("VVLlenaParqueos", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        DataTable dtFiltrado = new DataTable();
                        var resultados = from row in dataTable.AsEnumerable()
                                         where row.Field<string>("Placa").Contains(criterioBusqueda)
                                         select row;
                        dtFiltrado = resultados.CopyToDataTable();
                        GVParqueoV.DataSource = dtFiltrado;
                        GVParqueoV.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }

        }

        private void evalT()
    {
        Lblusuario.InnerText = Convert.ToString(Session["NombUsu"]);
        switch (Convert.ToString(Session["Tipo"]))
        {
            //Usuario Admin TI
            case "A":
                //nrqm.Visible = false;
                //nuser.Visible = true;
                //Asig.Visible = false;
                //A2.Visible = true;
                break;
            case "P":
                //// Usuario TI
                //nrqm.Visible = false;
                //nuser.Visible = false;
                //Asig.Visible = true;
                //A2.Visible = false;
                break;
            case "U":
                //// Usuario Normal creador de Requerimiento
                //nrqm.Visible = true;
                //nuser.Visible = false;
                //Asig.Visible = false;
                //A2.Visible = false;
                break;
            default:
                Response.Redirect("../Index.aspx");
                break;
        }
    }

        protected void BtnNP_Click(object sender, EventArgs e)
        {
                try
                {
                    DateTime f1 = DateTime.Now;

                    if (string.IsNullOrEmpty(datetimePicker.Text))
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','Error, Por favor, selecciona una fecha y hora válida!','warning')", true);
                        return;
                    }                    

                    if (DateTime.TryParse(datetimePicker.Text, out f1))
                    {
                        Console.WriteLine("Fecha y Hora Seleccionada: " + f1);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','Error, Por favor, selecciona una fecha y hora válida!','warning')", true);
                    }


                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { }
            
                System.Threading.Thread.Sleep(5000);
                string respuesta = "0";

                if (ValidaCampos())
                {
                    try
                    {
                        using (SqlConnection conexion =  CNN.GetConnection())
                        {
                            conexion.Open();
                            using (SqlCommand comando = new SqlCommand("VVREGISTRAP", conexion))
                            {
                                comando.CommandType = CommandType.StoredProcedure;
                                comando.Parameters.Add("@p_NC", SqlDbType.VarChar, 80).Value = NombreCompleto.Value.Trim();
                                comando.Parameters.Add("@p_Tel", SqlDbType.VarChar, 20).Value = Tel.Value.Trim();
                                comando.Parameters.Add("@p_Placa", SqlDbType.VarChar, 15).Value = Placa.Value.Trim();
                                comando.Parameters.Add("@p_Depto", SqlDbType.Int).Value = ddDepto.SelectedValue;                                
                                comando.Parameters.Add("@p_F1", SqlDbType.DateTime).Value = datetimePicker.Text.Trim();
                                comando.Parameters.Add("@p_Marca", SqlDbType.VarChar, 25).Value = Marca.Value.Trim();
                                comando.Parameters.Add("@p_Respuesta", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                                comando.ExecuteNonQuery();
                                respuesta = Convert.ToString(comando.Parameters["@p_Respuesta"].Value.ToString());
                            }
                            conexion.Close();
                            CambiaVisibilidad();
                        }
                        if (respuesta == "1")
                        {
                            llenarGVV();
                            limpiarControles();
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Exito!','Registro creado con exito!','success')", true);
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Error, Registro ya existe BD!','warning')", true);
                        }

                        }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!',' Error al leer datos! " + ex.Message + "');", true);
                    }
                    finally {  }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Error, Favor llenar todos los campos!','warning')", true);
                }   
           
        }

        protected void BtnMP_Click(object sender, EventArgs e)
        {
            if (IdTrx.InnerText.Length <= 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','Error, Se debe de seleccionar un Registro!','warning')", true);
            }
            else
            {
                try
                {
                    System.Threading.Thread.Sleep(5000);
                    string respuesta = "0";
                    using (SqlConnection conexion = CNN.GetConnection())
                    {
                        conexion.Open();
                        using (SqlCommand comando = new SqlCommand("VVMODIFICAP", conexion))
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.Add("@p_nv", SqlDbType.Int).Value = Convert.ToInt32(IdTrx.InnerText);
                            comando.Parameters.Add("@p_Respuesta", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                            comando.ExecuteNonQuery();
                            respuesta = Convert.ToString(comando.Parameters["@p_Respuesta"].Value.ToString());
                        }
                        conexion.Close();
                    }
                    if (respuesta == "1")
                    {
                        llenarGVV();
                        limpiarControles();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Success!','Vehiculo modificado con exito!','success')", true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','Error, Error en registrar modificacion de salida!','warning')", true);
                    }

                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!',' Error al leer datos! " + ex.Message + "');", true);
                }
                finally { }
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            // habilitar opcion para habilitar estado del usuario y mostrar u ocultar botones            
            CambiaVisibilidad();
            llenarGVV();
            limpiarControles();
        }

        protected void limpiarControles()
        {
            Placa.Value = "";
            NombreCompleto.Value = "";
            Tel.Value = "";
            Marca.Value = "";
            datetimePicker.Text = "";
            //datetimePicker2.Text = ""; 
          

        }
      
        protected void llenarGVV()
        {
            using (SqlConnection con = CNN.GetConnection())
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("VVLlenaParqueos", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        if (dataTable.Columns.Count > 0)
                        {
                            GVParqueoV.DataSource = dataTable;
                            GVParqueoV.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!',' Error al leer datos! " + ex.Message + "');", true);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        protected bool ValidaCampos()
        {   
            string placa = Placa.Value.Trim();
            // 1. Validar la cantidad de caracteres (debe ser 7)
            if (placa.Length != 7)
            {
                // Mostrar mensaje de error
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','La placa debe tener exactamente 7 caracteres.','warning')", true);
                return false;
            }
            // 2. Validar que solo contenga números y letras
            if (!System.Text.RegularExpressions.Regex.IsMatch(placa, "^[a-zA-Z0-9]+$"))
            {
                // Mostrar mensaje de error
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','La placa solo puede contener letras y números.','warning')", true);
                return false;
            }
            // 3. Convertir las letras a mayúsculas
            placa = placa.ToUpper();
            // 4. Validar que no haya espacios vacíos (ya cubierto por el Regex anterior)
            // Si la validación pasa, asigna el valor procesado de vuelta al control o úsalo como necesites
            Placa.Value = placa;

            bool valid = false;
            if (NombreCompleto.Value.Length> 0 && Tel.Value.Length>0 && Placa.Value.Length>0 && Marca.Value.Length>0)
            {  valid = true;   } else   { valid= false;   }
            return valid;
        }

        protected void GVParqueoV_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtiene el índice de la fila seleccionada
            int rowIndex = GVParqueoV.SelectedIndex;
            if (rowIndex >= 0 && rowIndex < GVParqueoV.Rows.Count)
            {
                try
                {
                    DateTime dt = new DateTime();
                    Placa.Value = Convert.ToString(HttpUtility.HtmlDecode(GVParqueoV.Rows[rowIndex].Cells[5].Text));
                    IdTrx.InnerText = Convert.ToString(HttpUtility.HtmlDecode(GVParqueoV.Rows[rowIndex].Cells[1].Text));
                    NombreCompleto.Value = Convert.ToString(HttpUtility.HtmlDecode(GVParqueoV.Rows[rowIndex].Cells[2].Text));
                    Marca.Value = Convert.ToString(HttpUtility.HtmlDecode(GVParqueoV.Rows[rowIndex].Cells[7].Text));
                    dt = Convert.ToDateTime(Convert.ToString(HttpUtility.HtmlDecode(GVParqueoV.Rows[rowIndex].Cells[6].Text)));
                    datetimePicker.Text = dt.ToString("yyyy-MM-ddTHH:mm");
                    Tel.Value = Convert.ToString(HttpUtility.HtmlDecode(GVParqueoV.Rows[rowIndex].Cells[4].Text));

                    datetimePicker.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");

                    string valorSeleccionado = GVParqueoV.Rows[rowIndex].Cells[3].Text;
                    if (ddDepto.SelectedValue != valorSeleccionado)
                    {
                        ListItem itemSeleccionado = ddDepto.Items.FindByText(valorSeleccionado);
                        if (itemSeleccionado != null)
                        {
                            string valorItemSeleccionado = itemSeleccionado.Value; ddDepto.SelectedValue = valorItemSeleccionado;
                        }
                        else
                        { Page.ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Error inesperado/ Departamento no existe!');", true); }
                    }

                    string valorSeleccionado2 = GVParqueoV.Rows[rowIndex].Cells[8].Text;
                    if (ddEstado.SelectedValue != valorSeleccionado2)
                    {
                        ListItem itemSeleccionado2 = ddEstado.Items.FindByText(valorSeleccionado2);
                        if (itemSeleccionado2 != null)
                        {
                            string valorItemSeleccionado2 = itemSeleccionado2.Value; ddEstado.SelectedValue = valorItemSeleccionado2;
                        }
                        else
                        { Page.ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Error inesperado/ Tipo Salida no existe!');", true); }
                    }


                    CambiaVisibilidad();
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        protected void CambiaVisibilidad()
        {
            if(BtnMP.Visible == false)
            {
                BtnMP.Visible = true;
                BtnNP.Visible = false;
            }
            else
            {
                limpiarControles();
                BtnMP.Visible = false;
                BtnNP.Visible = true;
            }
        }

        protected void ddDepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string nuevoValor = ddDepto.SelectedValue;
            //if (nuevoValor == "35")
            //{
            //    divJefe.Visible = true;
            //    llenaDDLJefe();
            //}
            //else
            //{
            //    divJefe.Visible = false;
            //}
        }

        protected void ddEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string nuevoValor = ddDepto.SelectedValue;
            //if (nuevoValor == "35")
            //{
            //    divJefe.Visible = true;
            //    llenaDDLJefe();
            //}
            //else
            //{
            //    divJefe.Visible = false;
            //}
        }

        protected void llenaDDL()
        {
            using (SqlConnection con = CNN.GetConnection())
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("VVllenaDDLDepto", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        ddDepto.DataSource = dt;
                        ddDepto.DataTextField = "Descripcion";
                        ddDepto.DataValueField = "IDDepto";
                        ddDepto.DataBind();
                    }
                    ddDepto.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Departamento", "-1"));
                }
                catch (Exception ex)
                {
                    //reusa.ILogE("DDLU", ex.Message, Convert.ToString(Session["NombUsu"]));
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!',' Error al leer datos! " + ex.Message + "');", true);
                }
                finally
                { con.Close(); }
            }
        }

        protected void llenaDDLEstado()
        {
            using (SqlConnection con = CNN.GetConnection())
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("VVllenaDDLEstado", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        ddEstado.DataSource = dt;
                        ddEstado.DataTextField = "Descripcion";
                        ddEstado.DataValueField = "IDEstado";
                        ddEstado.DataBind();
                    }
                    ddEstado.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Estado", "-1"));
                }
                catch (Exception ex)
                {
                    //reusa.ILogE("DDLU", ex.Message, Convert.ToString(Session["NombUsu"]));
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!',' Error al leer datos! " + ex.Message + "');", true);
                }
                finally
                { con.Close(); }
            }
        }

        protected void lnkSalir_Click(object sender, EventArgs e)
        {
            // Eliminar todas las variables de sesión
            Session.Clear();
            // Abandonar la sesión actual
            Session.Abandon();
            // Redirigir al usuario a la página de inicio de sesión o cualquier otra página
            Response.Redirect("../Index.aspx");
        }

    }
}