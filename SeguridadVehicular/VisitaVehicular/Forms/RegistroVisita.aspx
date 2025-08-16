<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistroVisita.aspx.cs" Inherits="VisitaVehicular.Forms.RegistroVisita" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Registro Visitas</title>
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
    
    <script src="../Scripts/sweetalert.min.js"></script>

    

    <form id="form1" class="bg-white" runat="server">
        
        <nav class="navbar navbar-expand-lg navbar-dark bg-success">
    <div class="container-fluid">
        <a class="navbar-brand" href="../Forms/Default.aspx"><i class="fa fa-home"></i> Entrada</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav">
                <li class="nav-item">
                    <a class="nav-link bg-gradient" href="../Forms/RegistroVisita.aspx"><i class="fa fa-address-card"></i> Registrar Visita</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="../Forms/RegistroSalidaPersonal.aspx"><i class="fa fa-user-plus"></i> Registrar Salida Personal</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="../Forms/ListadoVehicular.aspx"><i class="fa fa-car"></i> Registro Vehicular</a>
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




        <div class="row align-items-start">
            
            <!-- CARD 1 -->
            <div class="col-md-2">
            </div>

            <div class="col-md-8">

                <div>
                    <h1 class="text-center">Registro Visita</h1>
                </div>

                <div class="row">
                    <!-- Columna para DNI -->
                    <div class="col-md-3 mb-3">
                        <label for="DNI" class="control-label text-secondary fw-bold">DNI:</label><label id="Visita" runat="server" visible="false" class="control-label text-secondary fw-bold"></label>
                        <input id="DNI" name="DNI" type="text" runat="server" placeholder="00000000000" min="4" max="15" class="form-control"/>
                    </div>
                    <!-- Columna para Nombre Completo -->
                    <div class="col-md-9 mb-3">
                        <label for="NombreCompleto" class="control-label text-secondary fw-bold">Nombre Completo:</label>
                        <input id="NombreCompleto" name="NombreCompleto" type="text" runat="server" placeholder="Nombre Completo" min="4" max="80" class="form-control"/>
                    </div>
                </div>

                <!-- Segunda fila de inputs -->
                <div class="row">
                    <!-- Columna para Donde se dirige -->
                    <div class="col-md-4 mb-3">
                        <label for="LugarDirige" class="control-label text-secondary fw-bold">A donde se dirige:</label>
                        <input id="LugarDirige" name="LugarDirige" type="text" runat="server" placeholder="Area/Persona" min="4" max="80" class="form-control"/>
                    </div>
                    <!-- Columna para Fecha/Hora Ingreso -->
                    <div class="col-md-4 mb-3">
                        <label for="Cal1" class="control-label text-secondary fw-bold">Fecha Ingreso:</label>
                        <asp:TextBox ID="datetimePicker" runat="server" ClientIDMode="Static" CssClass="form-control" />
                    </div>
                    <div class="col-md-4 mb-3">
                        <label for="Cal2" class="control-label text-secondary fw-bold">Fecha/Hora Salida:</label>
                        <asp:TextBox ID="datetimePicker2" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>

                <!-- Cuarta fila de inputs -->
                <div class="row">
                    <!-- Columna para Telefono -->
                    <div class="col-md-3 mb-3">
                        <label for="Tel" class="control-label text-secondary fw-bold"># Telefono</label>
                        <input id="Tel" name="Tel" type="text" runat="server" placeholder="Telefono" min="4" max="20" onkeypress="return validarNumeros(event);" class="form-control"/>
                    </div>
                    <!-- Columna para Fecha/Hora Salida -->
                    <div class="col-md-9 mb-3">
                        <label for="PropositoTxt" class="control-label text-secondary fw-bold">Proposito de visita:</label>
                        <input id="PropositoTxt" name="Proposito" type="text" runat="server" placeholder="Proposito" min="4" max="80" class="form-control"/>
                        
                    </div>
                </div>
                
                 <div class="form-group m-2">
                     <div class="col-md-12 text-center">
                         <asp:Button class="btn btn-warning fw-bold  btn-lg text-white" id="BtnNV" runat="server" OnClick="BtnNV_Click" Text="Registrar" OnClientClick="return validarCampos();" /> 
                         <asp:Button class="btn btn-warning fw-bold btn-lg text-white" id="BtnMP" runat="server" OnClick="BtnMP_Click" Text="Registrar Salida" OnClientClick="return validarCampos();" Visible="false"/>
                         <asp:Button class="btn btn-danger fw-bold  btn-lg" id="BtnCancelar" runat="server" OnClick="BtnCancelar_Click" Text="Cancelar" Visible="true"/>
                     </div>                                               
                 </div>

            </div>
            
            <div class="col-md-2">
            </div>

            <!-- CARD 2 -->
            <div class="col-md-2">
            </div>

            <div class="col-md-8">
                <!-- Llenar GridView -->
                     <div>
                         <div class="table-responsive">
                              <%--<asp:GridView ID="gvDetalleT" runat="server" class="col-md-12 justify-content-center table table-bordered table-condensed table-striped table-hover"
                                                               AutoGenerateColumns="False"   AllowPaging="true" OnPageIndexChanging="gvDetalleT_PageIndexChanging"  Visible="true">--%>
                             <asp:GridView ID="gvU" runat="server" style="font-size: 12px;" class="col-md-12 justify-content-center table table-bordered table-condensed table-striped table-hover" AllowPaging="true" AutoGenerateColumns="False" AutoGenerateSelectButton="True" OnSelectedIndexChanged="gvU_SelectedIndexChanged">
                                 <Columns>
                                     <asp:BoundField DataField="#Visita" HeaderText="#Visita" />
                                     <asp:BoundField DataField="NombreVisita" HeaderText="NombreVisita" />                                                                                
                                     <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                     <asp:BoundField DataField="DestinoVisita" HeaderText="DestinoVisita" />
                                     <asp:BoundField DataField="DNI" HeaderText="DNI" />
                                     <asp:BoundField DataField="FechaIngreso" HeaderText="FechaIngreso" />
                                     <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                     <asp:BoundField DataField="Proposito" HeaderText="Proposito" />
                                 </Columns>
                             </asp:GridView>
                         </div>
                     </div>
            </div>
            
            <div class="col-md-2">
            </div>
            
        </div>
    </form>
    
    <script type="text/javascript">
        function confirmExitVisit(button) {
            // Detiene la ejecución del evento predeterminado
            //event.preventDefault();

            // Obtén la fila del botón
            var row = button.closest("tr");

            // Obtén el valor de la columna deseada (por ejemplo, #Visita)
            var visitaId = row.cells[0].innerText;

            // Muestra la alerta de confirmación
            Swal.fire({
                title: '¿Desea registrar salida de visita?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Sí, registrar',
                cancelButtonText: 'No, cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Si el usuario confirma, ejecuta el postback del botón con el ID de la visita y el resultado
                    __doPostBack(button.id, visitaId + ':confirmar');
                } else {
                    // Si el usuario cancela, ejecuta el postback del botón con el ID de la visita y el resultado
                    __doPostBack(button.id, visitaId + ':cancelar');
                }
            });

            return false;
        }
     
        function validarCampos() {
            var DNI = document.getElementById("DNI").value;
            var NombreCompleto = document.getElementById("NombreCompleto").value;
            var LugarDirige = document.getElementById("LugarDirige").value;
            var Tel = document.getElementById("Tel").value;
            if (Tel === "" || LugarDirige === "" || DNI === "" || NombreCompleto === "") {
                Swal.fire('¡Error!', 'Por favor, llene todos los campos requeridos.', 'error');
                return false;
            }
            return true;
        }

        document.getElementById("datetimePicker").setAttribute("type", "datetime-local");
        document.getElementById("datetimePicker2").setAttribute("type", "datetime-local");

        function updateDateTime() {
            const datetimePicker = document.getElementById("datetimePicker2");
            const now = new Date();
            const year = now.getFullYear();
            const month = String(now.getMonth() + 1).padStart(2, '0');
            const day = String(now.getDate()).padStart(2, '0');
            const hours = String(now.getHours()).padStart(2, '0');
            const minutes = String(now.getMinutes()).padStart(2, '0');

            // Formatear la fecha y hora actual en el formato correcto para datetime-local
            const formattedDateTime = `${year}-${month}-${day}T${hours}:${minutes}`;
            datetimePicker.value = formattedDateTime;
        }

        function validarNumeros(event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

    </script>
</body>
</html>
