<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VisitaVehicular.Forms.Default" %>

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
            <a class="navbar-brand bg-gradient" href="#"><i class="fa fa-home"></i>Entrada</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link" href="../Forms/RegistroVisita.aspx"><i class="fa fa-address-card"></i>Registrar Visita</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="../Forms/RegistroSalidaPersonal.aspx"><i class="fa fa-user-plus"></i>Registrar Salida Personal</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="../Forms/ListadoVehicular.aspx"><i class="fa fa-car"></i>Registro Vehicular</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" visible="false" id="Admin" runat="server"  href="../Forms/Admin.aspx"><i class="fa fa-venus-mars"></i> Administracion Usuarios</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" visible="false" id="Reporteria" runat="server"  href="../Forms/Rpt.aspx"><i class="fa fa-clipboard"></i> Reportes</a>
                    </li>
                </ul>
                <ul class="navbar-nav ms-auto">
                          <li class="nav-item">
                             <a class="nav-link fw-bold" id="Lblusuario" runat="server"></a>
                          </li>
                    <!-- Agrega ms-auto aquí para alinear a la derecha -->
                    <li class="nav-item bg-danger">
                        <asp:LinkButton ID="lnkSalir" runat="server" CssClass="nav-link fw-bold" OnClick="lnkSalir_Click">
                            <i class="fa fa-sign-out" aria-hidden="true"></i> Salir
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

        <div class="container bg-white">
            <div class="container mt-4">
                <h1 class="text-center header text-white bg-secondary">Bienvenidos(as) al Sistema de Vigilancia Banadesa</h1>
            </div>

            <div class="row">

                <div class="row justify-content-center">

                   <div class="d-flex justify-content-center align-items-center" style="height: 20vh;">
                        <div class="col-md-6 text-center">
                            <div class="d-flex flex-column gap-2">
                                <asp:Button ID="BtnVisitas" class="btn btn-outline-warning fw-bold m-2" runat="server" OnClick="BtnVisitas_Click" Text="Ver Visitas a Banadesa" />
                                <asp:Button ID="BtnSalidasPer" class="btn btn-outline-warning fw-bold m-2" runat="server" OnClick="BtnSalidasPer_Click" Text="Ver Salidas Personal Banadesa" />
                                <asp:Button ID="BtnListaPlacas" class="btn btn-outline-warning fw-bold m-2" runat="server" OnClick="BtnListaPlacas_Click" Text="Ver Listado de Placas Parqueo" />
                            </div>
                        </div>
                    </div>

                    <br />
                    <br />

                    <div id="MostrarGVVisitas" runat="server" visible="false">
                        <h2 class="text-center header text-white bg-success">Registros de Visitas a BANADESA</h2>
                        <div class="col-md-12">
                            <asp:Button ID="BtnEPDFVisitas" class="btn btn-outline-success fw-bold m-2" runat="server" OnClick="BtnEPDFVisitas_Click" Text="Exportar a PDF" />
                            <div class="table-responsive">

                                <asp:GridView ID="GVVisitas" runat="server" style="font-size: 12px;" class="col-md-12 justify-content-center table table-bordered table-condensed table table-striped table-hover"
                                    AllowPaging="true" OnPageIndexChanging="GVVisitas_PageIndexChanging" Visible="true">
                                </asp:GridView>

                            </div>
                        </div>
                    </div>
                    <div id="MostrarGVSalidas" runat="server" visible="false">
                        <h2 class="text-center header text-white bg-success">Permisos/Salidas de Personal BANADESA</h2>
                        <div class="col-md-12">
                            <asp:Button ID="BtnEPDFSalidas" class="btn btn-outline-success fw-bold m-2" runat="server" OnClick="BtnEPDFSalidas_Click" Text="Exportar a PDF" />
                            <div class="table-responsive">

                                <asp:GridView ID="GVSalidas" runat="server" style="font-size: 12px;" class="col-md-12 justify-content-center table table-bordered table-condensed table table-striped table-hover"
                                    AllowPaging="true" OnPageIndexChanging="GVSalidas_PageIndexChanging" Visible="true">
                                </asp:GridView>

                            </div>
                        </div>
                    </div>

                    <div id="MostrarGVParqueo" runat="server" visible="false">
                        <h2 class="text-center header text-white bg-success">Listado de Vehiculos de Parqueo BANADESA</h2>
                        <div class="col-md-12">
                            <asp:Button ID="BtnEPDFParqueo" class="btn btn-outline-success fw-bold m-2" runat="server" OnClick="BtnEPDFParqueo_Click" Text="Exportar a PDF" />
                            <div class="table-responsive">
                                    <asp:GridView ID="GVParqueo" runat="server" style="font-size: 12px;" class="col-md-12 justify-content-center table table-bordered table-condensed table table-striped table-hover"
                                        AllowPaging="true" OnPageIndexChanging="GVParqueo_PageIndexChanging" Visible="true">
                                    </asp:GridView>                                
                            </div>
                        </div>
                    </div>


                </div>
            </div>

        </div>


    </form>
</body>
</html>
