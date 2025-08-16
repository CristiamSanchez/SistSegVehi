<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rpt.aspx.cs" Inherits="VisitaVehicular.Forms.Rpt" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Principal</title>
    <!-- Bootstrap CSS -->
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <!-- SweetAlert CSS -->
    <link href="../Styles/sweetalert.css" rel="stylesheet" />
    <!-- FontAwesome CSS -->
    <link rel="stylesheet" href="../Content/font-awesome.min.css" />

    <style>
        .calendar {
            position: absolute;
            z-index: 1000;
            background-color: white;
            border: 1px solid #ccc;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
            }

        .calendar td {
            cursor: pointer;
        }

        .calendar td.SelectedDate {
            background-color: #007bff;
            color: white;
        }
    </style>

</head>
<body class="bg-light">
    <!-- jQuery y Bootstrap JS -->
    <script src="../Scripts/jquery.min.js"></script>
    <script src="../Scripts/bootstrap.bundle.min.js"></script>
    <!-- Asegúrate de usar bootstrap.bundle.min.js -->
    <!-- SweetAlert JS -->
    <script src="../Scripts/sweetalert.min.js"></script>

    <form id="form1" runat="server">

        
            <nav class="navbar navbar-expand-lg navbar-dark bg-success">
                <div class="container-fluid">
                    <a class="navbar-brand" href="../Forms/Default.aspx"><i class="fa fa-home"></i> Entrada</a>
                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                        <span class="navbar-toggler-icon"></span>
                    </button>
                    <div class="collapse navbar-collapse" id="navbarNav">
                        <ul class="navbar-nav">
                            <li class="nav-item">
                                <a class="nav-link" href="../Forms/RegistroVisita.aspx"><i class="fa fa-address-card"></i> Registrar Visita</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="../Forms/RegistroSalidaPersonal.aspx"><i class="fa fa-user-plus"></i> Registrar Salida Personal</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="../Forms/ListadoVehicular.aspx"><i class="fa fa-car"></i> Registro Vehicular</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" visible="false" id="Admin" runat="server"  href="../Forms/Admin.aspx"><i class="fa fa-venus-mars"></i> Administracion Usuarios</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link bg-gradient"  id="Reporteria" runat="server"  href="../Forms/Rpt.aspx"><i class="fa fa-clipboard"></i> Reportes</a>
                            </li>
                        </ul>
                        <ul class="navbar-nav ms-auto"> 
                            <li class="nav-item">
                                <a class="nav-link fw-bold" id="Lblusuario" runat="server"></a>
                            </li>
                            <li class="nav-item bg-danger">
                                <asp:LinkButton ID="lnkSalir" runat="server" CssClass="nav-link fw-bold" OnClick="lnkSalir_Click">
                                    <i class="fa fa-sign-out" aria-hidden="true"></i> Salir
                                </asp:LinkButton>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
            <div class="transparencia"></div>
    
            <div class="container bg-white">
                <div class="container mt-4">
                    <h1 class="text-center header text-white bg-secondary">Bienvenido a la Administración de Reportes</h1>
                </div>

                <div class="row justify-content-center">
                    <div class="col-md-6 d-flex justify-content-center align-items-center" style="height: 22vh;">
                        <div class="d-flex flex-wrap justify-content-center gap-2">
                            <asp:Button ID="BtnRptVisitas" class="btn btn-outline-warning fw-bold m-2" runat="server" OnClick="BtnRptVisitas_Click" Text="Reporte Visitas a Banadesa" />
                            <asp:Button ID="BtnRptSalidasPer" class="btn btn-outline-warning fw-bold m-2" runat="server" OnClick="BtnRptSalidasPer_Click" Text="Reporte Salidas Personal Banadesa" />
                            <asp:Button ID="BtnRptListaPlacas" class="btn btn-outline-warning fw-bold m-2" runat="server" OnClick="BtnRptListaPlacas_Click" Text="Reporte Listado de Placas Parqueo" />
                        </div>
                    </div>
                </div>

                <div id="MostrarGVVisitas" runat="server" visible="false">
                    <h2 class="text-center header text-white bg-success">Registros de Visitas a BANADESA</h2>
                    <div class="col-md-12">
                        <div class="row">
                             <div class="col-md-4">
                                  <label for="Cal1" class="control-label text-secondary fw-bold">Fecha Inicio:</label>
                                  <asp:TextBox ID="datetimePicker" runat="server" ClientIDMode="Static" CssClass="form-control" />
                             </div>
                             <div class="col-md-4">
                                  <label for="Cal1" class="control-label text-secondary fw-bold">Fecha Fin:</label>
                                  <asp:TextBox ID="datetimePicker2" runat="server" ClientIDMode="Static" CssClass="form-control" />
                             </div>
                                <div class="col-md-4">
                                <asp:Button ID="BtnLlenaPlacas" class="btn btn-warning fw-bold m-2" runat="server" OnClick="BtnLlena_Click" Text="Llenar Datos" />
                                <asp:Button ID="BtnEPDF1"  class="btn btn-outline-success fw-bold m-2" runat="server" visible="false" OnClick="BtnEPDF1_Click" Text="Exportar a PDF" />
                            </div>
                        </div>                       
                       
                    </div>
                </div>
                <div id="MostrarGVSalidas" runat="server" visible="false">
                    <h2 class="text-center header text-white bg-success">Permisos/Salidas de Personal BANADESA</h2>
                    <div class="col-md-12">
                           <div class="row">
                            <div class="col-md-4">
                                <label for="Cal2" class="control-label text-secondary fw-bold">Fecha Inicial:</label>
                                <asp:TextBox ID="datetimePicker3" runat="server" ClientIDMode="Static" CssClass="form-control" />
                            </div>
                            <div class="col-md-4">
                                <label for="Cal2" class="control-label text-secondary fw-bold">Fecha Salida:</label>
                                <asp:TextBox ID="datetimePicker4" runat="server" ClientIDMode="Static" CssClass="form-control" />
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="BtnLlena2" class="btn btn-warning fw-bold m-2" runat="server" OnClick="BtnLlena2_Click" Text="Llenar Datos" />
                            </div>
                        </div> 
                    </div>
                </div>

                <div id="MostrarGVParqueo" runat="server" visible="false">
                    <h2 class="text-center header text-white bg-success">Listado de Vehiculos de Parqueo BANADESA</h2>
                    <div class="col-md-12">
                     <div class="row">
                        <div class="col-md-4">
                            <label for="Cal3" class="control-label text-secondary fw-bold">Fecha Inicial:</label>
                            <asp:TextBox ID="datetimePicker5" runat="server" CssClass="form-control" ClientIDMode="Static" />
                        </div>
                        <div class="col-md-4">
                            <label for="Cal3" class="control-label text-secondary fw-bold">Fecha Salida:</label>
                            <asp:TextBox ID="datetimePicker6" runat="server" CssClass="form-control" ClientIDMode="Static" />
                        </div>
                        <div class="col-md-4">
                            <asp:Button ID="BtnLlena3" class="btn btn-warning fw-bold m-2" runat="server" OnClick="BtnLlena3_Click" Text="Llenar Datos" />
                        </div>
                      </div> 
                    </div>
                </div>
              
            </div>
    
    </form>

    <script type="text/javascript">
        
        document.addEventListener("DOMContentLoaded", function () {
            document.getElementById("datetimePicker").setAttribute("type", "datetime-local");
            document.getElementById("datetimePicker2").setAttribute("type", "datetime-local");
            document.getElementById("datetimePicker3").setAttribute("type", "datetime-local");
            document.getElementById("datetimePicker4").setAttribute("type", "datetime-local");
            document.getElementById("datetimePicker5").setAttribute("type", "datetime-local");
            document.getElementById("datetimePicker6").setAttribute("type", "datetime-local");
        });

        document.addEventListener("DOMContentLoaded", function () {
            let ids = ["datetimePicker", "datetimePicker2", "datetimePicker3", "datetimePicker4", "datetimePicker5", "datetimePicker6"];

            ids.forEach(function (id) {
                let element = document.getElementById(id);
                if (element) {
                    element.setAttribute("type", "datetime-local");
                } else {
                    console.error("Element with ID " + id + " not found.");
                }
            });
        });




    </script>

</body>
</html>


