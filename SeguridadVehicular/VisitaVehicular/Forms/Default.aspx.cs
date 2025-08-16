using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using Org.BouncyCastle;


namespace VisitaVehicular.Forms
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                //ingresoN = true;
                //llenaDDLAnalistas();
                //llenarGVXU(0);
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
                    Admin.Visible = true;
                    break;
                case "P":
                    Reporteria.Visible = true;
                    break;
                case "U":
                    BtnEPDFVisitas.Visible = false;
                    BtnEPDFSalidas.Visible = false;
                    BtnEPDFParqueo.Visible = false;
                    break;
                default:
                    Response.Redirect("../Index.aspx");
                    break;
            }
        }

        protected void BtnVisitas_Click(object sender, EventArgs e)
        {
            MuestraOculta(0);
            LlenaGvVisitas(1); 
        }

        protected void BtnSalidasPer_Click(object sender, EventArgs e)
        {
            MuestraOculta(1);
            LlenaGvVisitas(2);            
        }

        protected void BtnListaPlacas_Click(object sender, EventArgs e)
        {
            MuestraOculta(3);
            LlenaGvVisitas(3);
        }

        protected void MuestraOculta(int valor)
        {
            if(valor==0) { MostrarGVParqueo.Visible = false; MostrarGVSalidas.Visible = false; MostrarGVVisitas.Visible = true; } 
            else if(valor==1) { MostrarGVParqueo.Visible = false; MostrarGVSalidas.Visible = true; MostrarGVVisitas.Visible = false; } 
            else { MostrarGVParqueo.Visible = true; MostrarGVSalidas.Visible = false; MostrarGVVisitas.Visible = false; }
        }

        protected void LlenaGvVisitas(int valorT)
        {
            int cantidad = 0;
            using (SqlConnection con = CNN.GetConnection())
            {
                try
                {

                    if (valorT == 1)
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("VVLlenaVisitas", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            GVVisitas.DataSource = dataTable;
                            GVVisitas.DataBind();
                            GVVisitas.Visible = true;
                            cantidad = dataTable.Rows.Count;
                        }
                    }else if (valorT == 2) {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("VVLlenaSalidas", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            GVSalidas.DataSource = dataTable;
                            GVSalidas.DataBind();
                            GVSalidas.Visible = true;
                            cantidad = dataTable.Rows.Count;
                        }
                    }
                    else
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("VVLlenaParqueos", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            GVParqueo.DataSource = dataTable;
                            GVParqueo.DataBind();
                            GVParqueo.Visible = true;
                            cantidad = dataTable.Rows.Count;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!',' Error al leer datos! " + ex.Message + "');", true);
                }
            }
        }
        protected void GVVisitas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVVisitas.PageIndex = e.NewPageIndex;
        }
        protected void GVSalidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVSalidas.PageIndex = e.NewPageIndex;            
        }
        protected void GVParqueo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVParqueo.PageIndex = e.NewPageIndex;
        }
        protected void BtnEPDFVisitas_Click(object sender, EventArgs e)
        {
            try
            {
                // Define la ruta completa del archivo PDF que se generará
                string rutaArchivo = Server.MapPath("~/ArchivosPDF/Archivo.pdf");
                // Obtener los datos desde la base de datos
                DataTable datos = ObtenerDatosDesdeBaseDeDatos(1);
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
                                .SetBold();
                            documento.Add(titulo);
                            // Crear la tabla y agregar los encabezados
                            iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(datos.Columns.Count);
                            tabla.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                            foreach (DataColumn columna in datos.Columns)
                            {
                                Cell celda = new Cell().Add(new Paragraph(columna.ColumnName));
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

            //try
            //{
            //    // Define la ruta completa del archivo PDF que se generará
            //    string rutaArchivo = Server.MapPath("~/ArchivosPDF/Archivo.pdf");
            //    // Obtener los datos desde la base de datos
            //    DataTable datos = ObtenerDatosDesdeBaseDeDatos(1);
            //    if (datos.Rows.Count > 0)
            //    {
            //        PageSize pageSize = PageSize.LETTER.Rotate();
            //        using (PdfWriter writer = new PdfWriter(rutaArchivo))
            //        {
            //            using (PdfDocument pdf = new PdfDocument(writer))
            //            {
            //                Document documento = new Document(pdf, pageSize);
            //                Paragraph titulo = new Paragraph("Lista de Visitas ")
            //                    .SetTextAlignment(TextAlignment.CENTER)
            //                    .SetFontSize(18)
            //                    .SetBold();
            //                documento.Add(titulo);
            //                // Crear la tabla y agregar los encabezados
            //                iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(datos.Columns.Count);
            //                tabla.SetHorizontalAlignment(HorizontalAlignment.CENTER);

            //                foreach (DataColumn columna in datos.Columns)
            //                {
            //                    Cell celda = new Cell().Add(new Paragraph(columna.ColumnName));
            //                    tabla.AddHeaderCell(celda);
            //                }
            //                foreach (DataRow fila in datos.Rows)
            //                {
            //                    foreach (var item in fila.ItemArray)
            //                    {
            //                        Cell celda = new Cell().Add(new Paragraph(item.ToString()));
            //                        tabla.AddCell(celda);
            //                    }
            //                }
            //                documento.Add(tabla);
            //            }
            //        }
            //        Response.Clear();
            //        Response.ContentType = "application/pdf";
            //        Response.AddHeader("Content-Disposition", $"attachment; filename=Archivo.pdf");
            //        Response.WriteFile(rutaArchivo);
            //        Response.Write("<script>alert('Se ha exportado a PDF correctamente.');</script>");
            //    }
            //    else
            //    {
            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','No existen datos generados!','warning')", true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!',' Error al leer datos! " + ex.Message + "');", true);
            //}
        }
        protected void BtnEPDFSalidas_Click(object sender, EventArgs e)
        {
            try
            {
                // Define la ruta completa del archivo PDF que se generará
                string rutaArchivo = Server.MapPath("~/ArchivosPDF/Archivo.pdf");
                // Obtener los datos desde la base de datos
                DataTable datos = ObtenerDatosDesdeBaseDeDatos(2);
                if (datos.Rows.Count > 0)
                {
                    PageSize pageSize = PageSize.LETTER.Rotate();
                    using (PdfWriter writer = new PdfWriter(rutaArchivo))
                    {
                        using (PdfDocument pdf = new PdfDocument(writer))
                        {
                            Document documento = new Document(pdf, pageSize);
                            Paragraph titulo = new Paragraph("Lista de Salidas de Personal Banadesa ")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(18)
                                .SetBold();
                            documento.Add(titulo);
                            // Crear la tabla y agregar los encabezados
                            iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(datos.Columns.Count);
                            tabla.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                            foreach (DataColumn columna in datos.Columns)
                            {
                                Cell celda = new Cell().Add(new Paragraph(columna.ColumnName));
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

            //try
            //{
            //    // Define la ruta completa del archivo PDF que se generará
            //    string rutaArchivo = Server.MapPath("~/ArchivosPDF/Archivo.pdf");
            //    // Obtener los datos desde la base de datos
            //    DataTable datos = ObtenerDatosDesdeBaseDeDatos(2);
            //    if (datos.Rows.Count > 0)
            //    {
            //        PageSize pageSize = PageSize.LETTER.Rotate();
            //        using (PdfWriter writer = new PdfWriter(rutaArchivo))
            //        {
            //            using (PdfDocument pdf = new PdfDocument(writer))
            //            {
            //                Document documento = new Document(pdf, pageSize);
            //                Paragraph titulo = new Paragraph("Lista de Salidas ")
            //                    .SetTextAlignment(TextAlignment.CENTER)
            //                    .SetFontSize(18)
            //                    .SetBold();
            //                documento.Add(titulo);
            //                // Crear la tabla y agregar los encabezados
            //                iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(datos.Columns.Count);
            //                tabla.SetHorizontalAlignment(HorizontalAlignment.CENTER);

            //                foreach (DataColumn columna in datos.Columns)
            //                {
            //                    Cell celda = new Cell().Add(new Paragraph(columna.ColumnName));
            //                    tabla.AddHeaderCell(celda);
            //                }
            //                foreach (DataRow fila in datos.Rows)
            //                {
            //                    foreach (var item in fila.ItemArray)
            //                    {
            //                        Cell celda = new Cell().Add(new Paragraph(item.ToString()));
            //                        tabla.AddCell(celda);
            //                    }
            //                }
            //                documento.Add(tabla);
            //            }
            //        }
            //        Response.Clear();
            //        Response.ContentType = "application/pdf";
            //        Response.AddHeader("Content-Disposition", $"attachment; filename=Archivo.pdf");
            //        Response.WriteFile(rutaArchivo);
            //        Response.Write("<script>alert('Se ha exportado a PDF correctamente.');</script>");
            //    }
            //    else
            //    {
            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','No existen datos generados!','warning')", true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!',' Error al leer datos! " + ex.Message + "');", true);
            //}
        }
        protected void BtnEPDFParqueo_Click(object sender, EventArgs e)
        {
            try
            {
                // Define la ruta completa del archivo PDF que se generará
                string rutaArchivo = Server.MapPath("~/ArchivosPDF/Archivo.pdf");
                // Obtener los datos desde la base de datos
                DataTable datos = ObtenerDatosDesdeBaseDeDatos(3);
                if (datos.Rows.Count > 0)
                {
                    PageSize pageSize = PageSize.LETTER.Rotate();
                    using (PdfWriter writer = new PdfWriter(rutaArchivo))
                    {
                        using (PdfDocument pdf = new PdfDocument(writer))
                        {
                            Document documento = new Document(pdf, pageSize);
                            Paragraph titulo = new Paragraph("Lista de Vehiculos/Parqueo ")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(18)
                                .SetBold();
                            documento.Add(titulo);
                            // Crear la tabla y agregar los encabezados
                            iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(datos.Columns.Count);
                            tabla.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                            foreach (DataColumn columna in datos.Columns)
                            {
                                Cell celda = new Cell().Add(new Paragraph(columna.ColumnName));
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

            //try
            //{
            //    // Define la ruta completa del archivo PDF que se generará
            //    string rutaArchivo = Server.MapPath("~/ArchivosPDF/Archivo.pdf");
            //    // Obtener los datos desde la base de datos
            //    DataTable datos = ObtenerDatosDesdeBaseDeDatos(3);
            //    if (datos.Rows.Count > 0)
            //    {
            //        PageSize pageSize = PageSize.LETTER.Rotate();
            //        using (PdfWriter writer = new PdfWriter(rutaArchivo))
            //        {
            //            using (PdfDocument pdf = new PdfDocument(writer))
            //            {
            //                Document documento = new Document(pdf, pageSize);
            //                Paragraph titulo = new Paragraph("Lista de Vehiculos/Parqueo ")
            //                    .SetTextAlignment(TextAlignment.CENTER)
            //                    .SetFontSize(18)
            //                    .SetBold();
            //                documento.Add(titulo);
            //                // Crear la tabla y agregar los encabezados
            //                iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(datos.Columns.Count);
            //                tabla.SetHorizontalAlignment(HorizontalAlignment.CENTER);

            //                foreach (DataColumn columna in datos.Columns)
            //                {
            //                    Cell celda = new Cell().Add(new Paragraph(columna.ColumnName));
            //                    tabla.AddHeaderCell(celda);
            //                }
            //                foreach (DataRow fila in datos.Rows)
            //                {
            //                    foreach (var item in fila.ItemArray)
            //                    {
            //                        Cell celda = new Cell().Add(new Paragraph(item.ToString()));
            //                        tabla.AddCell(celda);
            //                    }
            //                }
            //                documento.Add(tabla);
            //            }
            //        }
            //        Response.Clear();
            //        Response.ContentType = "application/pdf";
            //        Response.AddHeader("Content-Disposition", $"attachment; filename=Archivo.pdf");
            //        Response.WriteFile(rutaArchivo);
            //        Response.Write("<script>alert('Se ha exportado a PDF correctamente.');</script>");
            //    }
            //    else
            //    {
            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!','No existen datos generados!','warning')", true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Warning!',' Error al leer datos! " + ex.Message + "');", true);
            //}
        }

        private DataTable ObtenerDatosDesdeBaseDeDatos(int Tipo)
        {
            DataTable datos = new DataTable();

            if (GVParqueo.Rows.Count > 0 || GVSalidas.Rows.Count>0 || GVVisitas.Rows.Count>0)
            {

                using (SqlConnection con = CNN.GetConnection())
                {
                    try
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("VVBuscarDatosTbls", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@p_Tipo", SqlDbType.Int).Value = Tipo;
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            adapter.Fill(datos);
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
            return datos;
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