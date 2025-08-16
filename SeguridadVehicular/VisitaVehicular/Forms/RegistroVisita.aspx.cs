using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web.Services;

namespace VisitaVehicular.Forms
{
    public partial class RegistroVisita : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenarGVV();
                // Inicialización de la página
            }
            else
            {
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

        protected void BtnNV_Click(object sender, EventArgs e)
        {
                try
                {
                    DateTime f1 = DateTime.Now;
                    //DateTime f2 = DateTime.Now;

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
                        using (SqlConnection conexion = CNN.GetConnection())
                        {
                            conexion.Open();
                            using (SqlCommand comando = new SqlCommand("VVREGISTRAVI", conexion))
                            {
                                comando.CommandType = CommandType.StoredProcedure;
                                comando.Parameters.Add("@p_NC", SqlDbType.VarChar, 80).Value = NombreCompleto.Value.Trim();
                                comando.Parameters.Add("@p_Tel", SqlDbType.VarChar, 20).Value = Tel.Value.Trim();
                                comando.Parameters.Add("@p_LD", SqlDbType.VarChar, 80).Value = LugarDirige.Value.Trim();
                                comando.Parameters.Add("@p_DNI", SqlDbType.VarChar, 15).Value = DNI.Value.Trim();
                                comando.Parameters.Add("@p_F1", SqlDbType.DateTime).Value = datetimePicker.Text.Trim();
                                comando.Parameters.Add("@p_P1", SqlDbType.VarChar, 80).Value = PropositoTxt.Value.Trim();
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
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!',' Error al leer datos! " + ex.Message + "');", true);
                    }
                    finally {  }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','Error, Favor llenar todos los campos!','warning')", true);
                }   
           
        }

        protected void BtnMP_Click(object sender, EventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(5000);
                string respuesta = "0";
                using (SqlConnection conexion = CNN.GetConnection())
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand("VVMODIFICAVI", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add("@p_nv", SqlDbType.Int).Value = Convert.ToInt32(Visita.InnerText);
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
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Success!','Usuario creado con exito!','success')", true);
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
            llenarGVV();
            limpiarControles();
        }

        protected void limpiarControles()
        {
            DNI.Value = "";
            NombreCompleto.Value = "";
            Tel.Value = "";
            LugarDirige.Value = "";
            datetimePicker.Text = "";
            datetimePicker2.Text = ""; 
        }
      
        protected void llenarGVV()
        {
            using (SqlConnection con = CNN.GetConnection())
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("VVBuscarDatosV", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        gvU.DataSource = dataTable;
                        gvU.DataBind();
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
            bool valid = false;
            if(NombreCompleto.Value.Length> 0 && Tel.Value.Length>0 && DNI.Value.Length>0 && LugarDirige.Value.Length>0 && PropositoTxt.Value.Length>1)
            {  valid = true;   } else   { valid= false;   }
            return valid;
        }

        protected void gvU_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtiene el índice de la fila seleccionada
            int rowIndex = gvU.SelectedIndex;
            if (rowIndex >= 0 && rowIndex < gvU.Rows.Count)
            {
                try
                {
                    DateTime dt = new DateTime();
                    DNI.Value = Convert.ToString(HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[5].Text));
                    Visita.InnerText = Convert.ToString(HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[1].Text));
                    NombreCompleto.Value = Convert.ToString(HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[2].Text));
                    LugarDirige.Value = Convert.ToString(HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[4].Text));
                    dt = Convert.ToDateTime(Convert.ToString(HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[6].Text)));
                    datetimePicker.Text = dt.ToString("yyyy-MM-ddTHH:mm");
                    Tel.Value = Convert.ToString(HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[3].Text));
                    PropositoTxt.Value = Convert.ToString(HttpUtility.HtmlDecode(gvU.Rows[rowIndex].Cells[8].Text));
                    datetimePicker2.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
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
                BtnNV.Visible = false;
            }
            else
            {
                BtnMP.Visible = false;
                BtnNV.Visible = true;
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