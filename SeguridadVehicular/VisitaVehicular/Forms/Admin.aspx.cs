using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Security.Policy;
using System.Drawing;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;


namespace VisitaVehicular.Forms
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Lblusuario.InnerText = Convert.ToString(Session["NombUsu"]);
            llenarGVU();
            evalT();
            if (!IsPostBack)
            {
                llenaDDL();
                limpiarControles();
            }
        }

        private void evalT()
        {
            Lblusuario.InnerText = Convert.ToString(Session["NombUsu"]);
            switch (Convert.ToString(Session["Tipo"]))
            {
                //Usuario Admin TI
                case "A":
                   
                    break;
                case "P":
                    Response.Redirect("../Default.aspx");
                    break;
                case "U":
                    Response.Redirect("../Default.aspx");
                    break;
                default:
                    Response.Redirect("../Default.aspx");
                    break;
            }
        }

        protected void btnNU_Click(object sender, EventArgs e)
        {
            try
            {
                if (usuN.Value.Length > 0 && passN.Text.Length > 0 && fname.Value.Length > 0 && lname.Value.Length > 0 && email.Value.Length > 0 && phone.Value.Length > 0 && ddDepto.SelectedIndex > 0)
                {
                    if (validaU(usuN.Value.Trim()) == "0")
                    {
                        using (SqlConnection conexion = CNN.GetConnection())
                        {
                            conexion.Open();
                            using (SqlCommand comando = new SqlCommand("VVREGISTRAU", conexion))
                            {
                                comando.CommandType = CommandType.StoredProcedure;
                                comando.Parameters.Add("@p_U", SqlDbType.VarChar, 20).Value = usuN.Value.Trim();
                                string encryptedPassword = passN.Text.Trim();
                                string decryptedPassword = Forms.CNN.Encrypt(encryptedPassword); // Desencriptar la contraseña

                                comando.Parameters.Add("@p_pass", SqlDbType.VarChar, 255).Value = decryptedPassword.ToString().Trim();
                                comando.Parameters.Add("@p_nom", SqlDbType.VarChar, 80).Value = fname.Value.Trim();
                                comando.Parameters.Add("@p_ape", SqlDbType.VarChar, 80).Value = lname.Value.Trim();
                                comando.Parameters.Add("@p_tipo", SqlDbType.VarChar, 2).Value = tipoU_id.Value.ToString();
                                comando.Parameters.Add("@p_corr", SqlDbType.VarChar, 50).Value = email.Value.Trim();
                                comando.Parameters.Add("@p_depto", SqlDbType.VarChar, 70).Value = Convert.ToString(ddDepto.SelectedItem);
                                comando.Parameters.Add("@p_tel", SqlDbType.VarChar, 20).Value = Convert.ToString(phone.Value.Trim());

                                comando.Parameters.Add("@p_Respuesta", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                                comando.ExecuteNonQuery();
                            }
                            conexion.Close();
                        }
                        Task.Run(() => EnviarCorreo());
                        // EnviarCorreo();
                        llenarGVU();
                        limpiarControles();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Éxito!','Usuario creado con exito!','success')", true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Error, Usuario ya existe!','warning')", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Error, Usuario ya existe!');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Error, Debe llenar/Seleccionar todos los valores!','warning')", true);
                }
            }
            catch (Exception ex) { Page.ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Error, " + ex.Message + "');", true); }
            finally { }
            
        }

        protected void limpiarControles()
        {
            usuN.Value = "";
            passN.Text = "";
            fname.Value = "";
            lname.Value = "";
            email.Value = "";
            phone.Value = "";
            ddDepto.SelectedIndex = -1;

        }

        public string validaU(string usua)
        {
         
                try
                {
                using (SqlConnection conexion = CNN.GetConnection())
                {
                        conexion.Open();
                        using (SqlCommand comando = new SqlCommand("VVVALIDAU", conexion))
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.Add("@p_U", SqlDbType.VarChar, 15).Value = usua.Trim();
                            comando.Parameters.Add("@p_Respuesta", SqlDbType.Int, 3).Direction = ParameterDirection.Output;
                            comando.ExecuteNonQuery();
                            usua = Convert.ToString(comando.Parameters["@p_Respuesta"].Value.ToString());
                        }
                        conexion.Close();
                    }
                }
                catch (Exception ex) { Page.ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Error, " + ex.Message + "');", true); }
                
            return usua;
        }

        protected void llenarGVU()
        {
            using (SqlConnection conexion = CNN.GetConnection())
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("VVBuscarDatosU", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        gvU.DataSource = dataTable;
                        gvU.DataBind();
                    }
                    gvU.Visible = true;
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Error, Error al leer datos de Usuarios! " + ex.Message + "');", true);
                }
                finally
                {
                    conexion.Close();
                }
            }
        }

        protected void gvU_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtiene el índice de la fila seleccionada
            int rowIndex = gvU.SelectedIndex;
            limpiarControles();
            if (rowIndex >= 0 && rowIndex < gvU.Rows.Count)
            {
                try
                {
                    string valorSeleccionado = gvU.Rows[rowIndex].Cells[7].Text;

                    if (ddDepto.SelectedValue != valorSeleccionado)
                    {
                        // Buscar el valor del ListItem basado en el texto
                        ListItem itemSeleccionado = ddDepto.Items.FindByText(valorSeleccionado);

                        if (itemSeleccionado != null)
                        {
                            string valorItemSeleccionado = itemSeleccionado.Value;
                            ddDepto.SelectedValue = valorItemSeleccionado;
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Error inesperado/ Departamento de Usuario no existe!');", true);
                        }
                    }

                    usuN.Value = HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[1].Text);
                    string encryptedPassword = HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[2].Text);
                    string decryptedPassword = Forms.CNN.Decrypt(encryptedPassword); // Desencriptar la contraseña

                    passN.Text = decryptedPassword.ToString().Trim(); // Usar .Text en lugar de .Value
                    fname.Value = HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[3].Text);
                    lname.Value = HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[4].Text);
                    email.Value = HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[5].Text);
                    phone.Value = HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[6].Text);
                    var estado = HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[8].Text);
                    var tipo = HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[9].Text);
                    if(estado=="INACTIVO") { ddEstadoM.SelectedValue = "I"; } else { ddEstadoM.SelectedValue = "A"; }                    
                    tipoU_id.Value = tipo;
                    // habilitar opcion para habilitar estado del usuario y mostrar u ocultar botones
                    btnMU.Visible = true;
                    btnCancelar.Visible = true;
                    //ddEstadoM.Enabled = true;
                    btnNU.Visible = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public bool IsValidPassword(string password)
        {
            // Verificar la longitud
            if (password.Length <= 6)
            {
                return false;
            }

            // Verificar que contenga al menos un número
            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            // Verificar que contenga al menos una letra mayúscula
            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            // Verificar que contenga al menos una letra minúscula
            if (!password.Any(char.IsLower))
            {
                return false;
            }

            // Si pasa todas las verificaciones
            return true;
        }

        protected void btnMU_Click(object sender, EventArgs e)
        {
            try
            {
                var estado = "A";
                if (ddEstadoM.SelectedIndex == 0) { estado = "A"; } else { estado = "I"; }

                string password = passN.Text;

                if (IsValidPassword(password))
                {
                    using (SqlConnection conexion = CNN.GetConnection())
                    {
                        conexion.Open();
                        using (SqlCommand comando = new SqlCommand("VVMODIFICAU", conexion))
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.Add("@p_U", SqlDbType.VarChar, 20).Value = usuN.Value.Trim();
                            string encryptedPassword = passN.Text.Trim();
                            string decryptedPassword = Forms.CNN.Encrypt(encryptedPassword);
                            comando.Parameters.Add("@p_pass", SqlDbType.VarChar, 255).Value = decryptedPassword.ToString().Trim();
                            comando.Parameters.Add("@p_nom", SqlDbType.VarChar, 80).Value = fname.Value;
                            comando.Parameters.Add("@p_ape", SqlDbType.VarChar, 80).Value = lname.Value;
                            comando.Parameters.Add("@p_tipo", SqlDbType.VarChar, 2).Value = tipoU_id.Value.ToString();
                            comando.Parameters.Add("@p_corr", SqlDbType.VarChar, 50).Value = email.Value.Trim();
                            comando.Parameters.Add("@p_depto", SqlDbType.VarChar, 70).Value = Convert.ToString(ddDepto.SelectedItem);
                            comando.Parameters.Add("@p_tel", SqlDbType.VarChar, 20).Value = Convert.ToString(phone.Value.Trim());
                            comando.Parameters.Add("@p_estado", SqlDbType.VarChar, 100).Value = Convert.ToString(estado.Trim());
                            comando.Parameters.Add("@p_Respuesta", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                            comando.ExecuteNonQuery();
                            if (comando.Parameters["@p_Respuesta"].Value.ToString() == "0")
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Error en respuesta!','warning')", true);
                                return;
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Éxito!','Usuario Modificado!','success')", true);
                            }
                        }
                        conexion.Close();
                        llenarGVU();
                        limpiarControles();
                        btnCancelar.Visible = false;
                        btnMU.Visible = false;
                        ddEstadoM.SelectedIndex = 0;
                        btnNU.Visible = true;
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Contraseña no cumple requerimientos minimos! 1. Contenga un número. 2.Tenga al menos una letra mayúscula. 3.Tenga al menos una letra minúscula. 4.Su longitud sea mayor a 6 caracteres.  (','warning')", true);
                }

            }
            catch (Exception ex) { ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Error en " + ex.Message + "!','warning')", true); }
        }

        protected void gvU_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvU.PageIndex = e.NewPageIndex;
            llenarGVU();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // habilitar opcion para habilitar estado del usuario y mostrar u ocultar botones
            btnMU.Visible = false;
            btnCancelar.Visible = false;
            //ddEstadoM.Visible = false;
            btnNU.Visible = true;
            limpiarControles();
            llenarGVU();
        }

        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            string criterioBusqueda = txtFiltro.Text.Trim();
            using (SqlConnection conexion = CNN.GetConnection())
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("VVBuscarDatosU", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        DataTable dtFiltrado = new DataTable();
                        var resultados = from row in dataTable.AsEnumerable()
                                         where row.Field<string>("Nombre").Contains(criterioBusqueda)
                                         select row;
                        dtFiltrado = resultados.CopyToDataTable();
                        gvU.DataSource = dtFiltrado;
                        gvU.DataBind();
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

        public void EnviarCorreo()
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress("alertadesarrollo@banadesa.hn", "Desarrollo Aplicaciones TI");
                message.To.Add(new MailAddress(email.Value, fname.Value + " " + lname.Value));
                message.Subject = "Creacion/Modificacion de Usuario Sistema Requerimientos TI";

                string body = "<body>" +
                    "<h1 style='color:White; background:green; text-align: center; font-family: cursive;'> Sistema de Requerimientos Informáticos! </h1>" +
                    "<h3 style='text-align: center; font-family: cursive;'> Estimado <b>" + fname.Value + " " + lname.Value + "</b> </h3>" +
                    "<span> Se ha creado satisfactoriamente su Usuario: <b>" + usuN.Value + "</b> y Contraseña: <b>" + passN.Text + ".</b> </span>" +
                    "<span> Cualquier duda o consulta, no olvides ponerte en contacto con el departamento de Tecnología </span></br>" +
                    "</br></br><span> Saludos Cordiales </span>" +
                    "</body>";

                message.IsBodyHtml = true;
                message.Body = body;

                using (var client = new SmtpClient("correo.banadesa.hn", 587))
                {
                    client.Credentials = new NetworkCredential("alertadesarrollo@banadesa.hn", "MiQ-Tr9w!fr8S");
                    client.EnableSsl = true; // Habilitar SSL si es necesario

                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                 Console.WriteLine("Error al enviar el correo electrónico: " + ex.Message);
            }
        }

        protected void llenaDDL()
        {
            using (SqlConnection conexion = CNN.GetConnection())
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("VVllenaDDLDepto", conexion))
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
                { Page.ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Error, Error al leer datos de Requerimientos! " + ex.Message + "');", true); }
                finally
                { conexion.Close(); }
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