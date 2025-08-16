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

namespace VisitaVehicular.Forms
{
    public partial class RegistroSalidaPersonal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenaDDL();
                llenaDDLTipo();
                llenarGVS();
            }
            evalT();
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
        protected void GVSalidaP_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtiene el índice de la fila seleccionada
            int rowIndex = GVSalidaP.SelectedIndex;
            if (rowIndex >= 0 && rowIndex < GVSalidaP.Rows.Count)
            {
                try
                {
                    DateTime dt = new DateTime();
                    DNI.Value = Convert.ToString(HttpUtility.HtmlDecode(GVSalidaP.Rows[rowIndex].Cells[7].Text));
                    NombreCompletoJefe.Value = Convert.ToString(HttpUtility.HtmlDecode(GVSalidaP.Rows[rowIndex].Cells[3].Text));
                    NombreCompleto.Value = Convert.ToString(HttpUtility.HtmlDecode(GVSalidaP.Rows[rowIndex].Cells[2].Text));
                    IdSalida.InnerText = Convert.ToString(HttpUtility.HtmlDecode(GVSalidaP.Rows[rowIndex].Cells[1].Text));
                    dt = Convert.ToDateTime(Convert.ToString(HttpUtility.HtmlDecode(GVSalidaP.Rows[rowIndex].Cells[8].Text)));
                    datetimePicker.Text = dt.ToString("yyyy-MM-ddTHH:mm");
                    Tel.Value = Convert.ToString(HttpUtility.HtmlDecode(GVSalidaP.Rows[rowIndex].Cells[4].Text));
                    PropositoSalida.Value = Convert.ToString(HttpUtility.HtmlDecode(GVSalidaP.Rows[rowIndex].Cells[5].Text));

                    string valorSeleccionado = GVSalidaP.Rows[rowIndex].Cells[6].Text;
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

                    string valorSeleccionado2 = GVSalidaP.Rows[rowIndex].Cells[9].Text;
                    if (ddTipoSalida.SelectedValue != valorSeleccionado2)
                    {
                        ListItem itemSeleccionado2 = ddTipoSalida.Items.FindByText(valorSeleccionado2);
                        if (itemSeleccionado2 != null)
                        {
                            string valorItemSeleccionado2 = itemSeleccionado2.Value; ddTipoSalida.SelectedValue = valorItemSeleccionado2;
                        }
                        else
                        { Page.ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Error inesperado/ Tipo Salida no existe!');", true); }
                    }
                    BtnMP.Visible =true;
                    BtnNV.Visible =false;

                    //CambiaVisibilidad();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        protected void BtnNS_Click(object sender, EventArgs e)
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

            if (NombreCompleto.Value.Length>0 && NombreCompletoJefe.Value.Length>0  && Tel.Value.Length>0 && DNI.Value.Length>0 && ddTipoSalida.SelectedIndex>0 && ddDepto.SelectedIndex>0)
            {
                try
                {
                    using (SqlConnection conexion = CNN.GetConnection())
                    {
                        conexion.Open();
                    using (SqlCommand comando = new SqlCommand("VVREGISTRASALIDA", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add("@p_NC", SqlDbType.VarChar, 80).Value = NombreCompleto.Value;
                        comando.Parameters.Add("@p_NCJefe", SqlDbType.VarChar, 80).Value = NombreCompletoJefe.Value;
                        comando.Parameters.Add("@p_Tel", SqlDbType.VarChar, 20).Value = Tel.Value.Trim();
                        comando.Parameters.Add("@PropositoSalida", SqlDbType.VarChar, 100).Value = PropositoSalida.Value.Trim();
                        comando.Parameters.Add("@p_Depto", SqlDbType.Char).Value = ddDepto.SelectedValue;
                        comando.Parameters.Add("@p_DNI", SqlDbType.VarChar, 15).Value = DNI.Value.Trim();
                        comando.Parameters.Add("@p_FecS", SqlDbType.DateTime).Value = Convert.ToDateTime(datetimePicker.Text.Trim());
                        comando.Parameters.Add("@p_TipoSal", SqlDbType.Char).Value = ddTipoSalida.SelectedValue;
                        comando.Parameters.Add("@p_Respuesta", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                        comando.ExecuteNonQuery();
                        respuesta = Convert.ToString(comando.Parameters["@p_Respuesta"].Value.ToString());
                    }
                    conexion.Close();
                    }
                    if (respuesta == "1")
                    {
                      llenarGVS();
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
                finally { }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Error, Favor llenar todos los campos!','warning')", true);
            }

        }

        protected void BtnMS_Click(object sender, EventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(5000);
                string respuesta = "0";
                using (SqlConnection conexion = CNN.GetConnection())
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand("VVMODIFICASALIDA", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add("@p_nv", SqlDbType.Int).Value = Convert.ToInt32(IdSalida.InnerText);
                        comando.Parameters.Add("@p_Respuesta", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                        comando.ExecuteNonQuery();
                        respuesta = Convert.ToString(comando.Parameters["@p_Respuesta"].Value.ToString());
                    }
                    conexion.Close();
                }
                if (respuesta == "1")
                {
                    llenarGVS();
                    limpiarControles();
                    BtnNV.Visible = true;
                    BtnMP.Visible = false;
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Success!','Registro Modificado con exito!','success')", true);
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

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            // habilitar opcion para habilitar estado del usuario y mostrar u ocultar botones            
            BtnNV.Visible = true;
            BtnMP.Visible = false;
            llenarGVS();
            //limpiarControles();
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

        protected void ddTipoSalida_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void llenarGVS()
        {
            using (SqlConnection con = CNN.GetConnection())
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("VVBuscarDatosS", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        GVSalidaP.DataSource = dataTable;
                        GVSalidaP.DataBind();
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

        protected void limpiarControles()
        {
            NombreCompletoJefe.Value = "";
            NombreCompleto.Value = "";
            Tel.Value = "";
            datetimePicker.Text = "";
            PropositoSalida.Value = "";
            ddTipoSalida.SelectedIndex = 0;
            ddDepto.SelectedIndex = 0;
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

        protected void llenaDDLTipo()
        {
            using (SqlConnection con = CNN.GetConnection())
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("VVllenaDDLTipo", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        ddTipoSalida.DataSource = dt;
                        ddTipoSalida.DataTextField = "Descripcion";
                        ddTipoSalida.DataValueField = "IDTipo";
                        ddTipoSalida.DataBind();
                    }
                    ddTipoSalida.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Tipo Salida", "-1"));
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