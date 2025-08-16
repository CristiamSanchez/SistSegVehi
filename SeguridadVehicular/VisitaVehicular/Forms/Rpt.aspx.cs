using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;

using iText.Layout;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using Org.BouncyCastle;

namespace VisitaVehicular.Forms
{
    public partial class Rpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                datetimePicker.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
                datetimePicker2.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
                datetimePicker3.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
                datetimePicker4.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
                datetimePicker5.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
                datetimePicker6.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
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
                    break;
                case "P":
                    Reporteria.Visible = true;
                    break;
                case "U":
                    break;
                default:
                    Response.Redirect("../Index.aspx");
                    break;
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

        protected void BtnRptVisitas_Click(object sender, EventArgs e)
        {
            MuestraOculta(0);
            //LlenaGvVisitas(1);
        }

        protected void BtnRptSalidasPer_Click(object sender, EventArgs e)
        {
            MuestraOculta(1);
            //LlenaGvVisitas(2);
        }

        protected void BtnRptListaPlacas_Click(object sender, EventArgs e)
        {
            MuestraOculta(2);
            //LlenaGvVisitas(3);
        }

        protected void MuestraOculta(int valor)
        {
            if (valor == 0) { MostrarGVParqueo.Visible = false; MostrarGVSalidas.Visible = false; MostrarGVVisitas.Visible = true; }
            else if (valor == 1) { MostrarGVParqueo.Visible = false; MostrarGVSalidas.Visible = true; MostrarGVVisitas.Visible = false; }
            else { MostrarGVParqueo.Visible = true; MostrarGVSalidas.Visible = false; MostrarGVVisitas.Visible = false; }
        }

        protected void BtnLlena_Click(object sender, EventArgs e)
        {
            if (datetimePicker.Text.Length>0 && datetimePicker2.Text.Length>0){

                try
                {
                    // Define la ruta completa del archivo PDF que se generará
                    string rutaArchivo = Server.MapPath("~/ArchivosPDF/Archivo.pdf");
                    // Obtener los datos desde la base de datos
                    DataTable datos = ObtieneDatos(1, Convert.ToDateTime(datetimePicker.Text), Convert.ToDateTime(datetimePicker2.Text));
                    if (datos.Rows.Count > 0)
                    {
                        PageSize pageSize = PageSize.LETTER.Rotate();
                        using (PdfWriter writer = new PdfWriter(rutaArchivo))
                        {
                            using (PdfDocument pdf = new PdfDocument(writer))
                            {
                                Document documento = new Document(pdf, pageSize);
                                Paragraph titulo = new Paragraph("Lista de Visitas a Banadesa ")
                                    .SetTextAlignment(TextAlignment.CENTER)
                                    .SetFontSize(18)
                                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                                    .SetBold();
                                documento.Add(titulo);
                                // Crear la tabla y agregar los encabezados
                                iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(datos.Columns.Count);
                                tabla.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                                Color backgroundColor = ColorConstants.ORANGE; // Color de fondo de la cabecera
                                Color fontColor = ColorConstants.BLACK; // Color del texto de la cabecera
                                foreach (DataColumn columna in datos.Columns)
                                {
                                    //Cell celda = new Cell().Add(new Paragraph(columna.ColumnName));
                                    //tabla.AddHeaderCell(celda);
                                    Cell celda = new Cell()
                                    .Add(new Paragraph(columna.ColumnName))
                                    .SetBackgroundColor(backgroundColor) // Establece el color de fondo
                                    .SetFontColor(fontColor) // Establece el color de la fuente
                                    .SetTextAlignment(TextAlignment.CENTER) // Alineación del texto en la cabecera
                                    .SetBold(); // Establece el texto en negrita

                                    tabla.AddHeaderCell(celda);
                                }
                                //foreach (DataColumn columna in datos.Columns)
                                //{
                                //    Cell celda = new Cell().Add(new Paragraph(columna.ColumnName));
                                //    tabla.AddHeaderCell(celda);
                                //}

                                foreach (DataRow fila in datos.Rows)
                                {
                                    foreach (var item in fila.ItemArray)
                                    {
                                        Cell celda = new Cell().Add(new Paragraph(item.ToString()));
                                        tabla.AddCell(celda);
                                    }
                                }
                                documento.Add(tabla);
                            }
                        }

                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", $"attachment; filename=Archivo.pdf");
                        Response.WriteFile(rutaArchivo);
                        Response.End();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','No existen datos generados!','warning')", true);
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!',' Error al leer datos! " + ex.Message + "');", true);
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','Error, Por favor, selecciona una fecha!','warning')", true);
            }
        }

        protected void BtnLlena2_Click(object sender, EventArgs e)
        {
            if (datetimePicker3.Text.Length > 0 && datetimePicker4.Text.Length > 0)
            {
                try
                {
                    // Define la ruta completa del archivo PDF que se generará
                    string rutaArchivo = Server.MapPath("~/ArchivosPDF/Archivo.pdf");
                    // Obtener los datos desde la base de datos
                    DataTable datos = ObtieneDatos(2, Convert.ToDateTime(datetimePicker3.Text), Convert.ToDateTime( datetimePicker4.Text));
                    if (datos.Rows.Count > 0)
                    {
                        PageSize pageSize = PageSize.LETTER.Rotate();
                        using (PdfWriter writer = new PdfWriter(rutaArchivo))
                        {
                            using (PdfDocument pdf = new PdfDocument(writer))
                            {
                                Document documento = new Document(pdf, pageSize);
                                Paragraph titulo = new Paragraph("Lista de Salidas Personal Banadesa ")
                                    .SetTextAlignment(TextAlignment.CENTER)
                                    .SetFontSize(18)
                                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                                    .SetBold();
                                documento.Add(titulo);
                                // Crear la tabla y agregar los encabezados
                                iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(datos.Columns.Count);
                                tabla.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                                Color backgroundColor = ColorConstants.ORANGE; // Color de fondo de la cabecera
                                Color fontColor = ColorConstants.BLACK; // Color del texto de la cabecera

                                foreach (DataColumn columna in datos.Columns)
                                {
                                    //Cell celda = new Cell().Add(new Paragraph(columna.ColumnName));
                                    //tabla.AddHeaderCell(celda);
                                    Cell celda = new Cell()
                                    .Add(new Paragraph(columna.ColumnName))
                                    .SetBackgroundColor(backgroundColor) // Establece el color de fondo
                                    .SetFontColor(fontColor) // Establece el color de la fuente
                                    .SetTextAlignment(TextAlignment.CENTER) // Alineación del texto en la cabecera
                                    .SetBold(); // Establece el texto en negrita

                                    tabla.AddHeaderCell(celda);
                                }

                                foreach (DataRow fila in datos.Rows)
                                {
                                    foreach (var item in fila.ItemArray)
                                    {
                                        Cell celda = new Cell().Add(new Paragraph(item.ToString()));
                                        tabla.AddCell(celda);
                                    }
                                }
                                documento.Add(tabla);
                            }
                        }

                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", $"attachment; filename=Archivo.pdf");
                        Response.WriteFile(rutaArchivo);
                        Response.End();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','No existen datos generados!','warning')", true);
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!',' Error al leer datos! " + ex.Message + "');", true);
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','Error, Por favor, selecciona una fecha!','warning')", true);
            }
        }

        protected void BtnLlena3_Click(object sender, EventArgs e)
        {
            if (datetimePicker5.Text.Length > 0 && datetimePicker6.Text.Length > 0)
            {
                try
                {
                    // Define la ruta completa del archivo PDF que se generará
                    string rutaArchivo = Server.MapPath("~/ArchivosPDF/Archivo.pdf");
                    // Obtener los datos desde la base de datos
                    DataTable datos = ObtieneDatos(3, Convert.ToDateTime(datetimePicker5.Text), Convert.ToDateTime(datetimePicker6.Text));
                    if (datos.Rows.Count > 0)
                    {
                        PageSize pageSize = PageSize.LETTER.Rotate();
                        using (PdfWriter writer = new PdfWriter(rutaArchivo))
                        {
                            using (PdfDocument pdf = new PdfDocument(writer))
                            {
                                Document documento = new Document(pdf, pageSize);
                                Paragraph titulo = new Paragraph("Lista de Registro de Vehiculos en Parqueo Banadesa ")
                                    .SetTextAlignment(TextAlignment.CENTER)
                                    .SetFontSize(18)
                                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                                    .SetBold();
                                documento.Add(titulo);
                                // Crear la tabla y agregar los encabezados
                                iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(datos.Columns.Count);
                                tabla.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                                Color backgroundColor = ColorConstants.ORANGE; // Color de fondo de la cabecera
                                Color fontColor = ColorConstants.BLACK; // Color del texto de la cabecera
                                foreach (DataColumn columna in datos.Columns)
                                {
                                    //Cell celda = new Cell().Add(new Paragraph(columna.ColumnName));
                                    //tabla.AddHeaderCell(celda);
                                    Cell celda = new Cell()
                                    .Add(new Paragraph(columna.ColumnName))
                                    .SetBackgroundColor(backgroundColor) // Establece el color de fondo
                                    .SetFontColor(fontColor) // Establece el color de la fuente
                                    .SetTextAlignment(TextAlignment.CENTER) // Alineación del texto en la cabecera
                                    .SetBold(); // Establece el texto en negrita

                                    tabla.AddHeaderCell(celda);
                                }
                                //foreach (DataColumn columna in datos.Columns)
                                //{
                                //    Cell celda = new Cell().Add(new Paragraph(columna.ColumnName));
                                //    tabla.AddHeaderCell(celda);
                                //}

                                foreach (DataRow fila in datos.Rows)
                                {
                                    foreach (var item in fila.ItemArray)
                                    {
                                        Cell celda = new Cell().Add(new Paragraph(item.ToString()));
                                        tabla.AddCell(celda);
                                    }
                                }
                                documento.Add(tabla);
                            }
                        }

                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", $"attachment; filename=Archivo.pdf");
                        Response.WriteFile(rutaArchivo);
                        Response.End();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','No existen datos generados!','warning')", true);
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!',' Error al leer datos! " + ex.Message + "');", true);
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','Error, Por favor, selecciona una fecha!','warning')", true);
            }
        }


        protected void BtnEPDF1_Click(object sender, EventArgs e)
        {
            //BtnEPDF1.Visible = true;
        }

        private DataTable ObtieneDatos(int valor, DateTime f1 , DateTime f2)
        {
            DataTable datos = new DataTable();
            using (SqlConnection con = CNN.GetConnection())
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("VVImportarTbls", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@p_P", SqlDbType.Int).Value = valor;
                        cmd.Parameters.Add("@p_F1", SqlDbType.DateTime).Value = f1;
                        cmd.Parameters.Add("@p_F2", SqlDbType.DateTime).Value = f2;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        if (dataTable.Columns.Count > 0){      datos = dataTable;                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!',' Error al leer datos! " + ex.Message + "');", true);
                }
                finally
                {
                    con.Close();
                }
            }
            return datos;
        }


    }
}