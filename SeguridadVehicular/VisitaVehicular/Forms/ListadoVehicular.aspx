<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListadoVehicular.aspx.cs" Inherits="VisitaVehicular.Forms.ListadoVehicular" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>ListadoVehicular</title>
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
    <script src="../Scripts/bootstrap.bundle.min.js"></script> <!-- Asegúrate de usar bootstrap.bundle.min.js -->
    <!-- SweetAlert JS -->
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
                        <a class="nav-link" href="../Forms/RegistroVisita.aspx"><i class="fa fa-address-card"></i> Registrar Visita</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="../Forms/RegistroSalidaPersonal.aspx"><i class="fa fa-user-plus"></i> Registrar Salida Personal</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link bg-gradient" href="../Forms/ListadoVehicular.aspx"><i class="fa fa-car"></i> Registro Vehicular</a>
                    </li>
                </ul>
                <ul class="navbar-nav ms-auto"> <!-- Agrega ms-auto aquí para alinear a la derecha -->
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
                    <h1 class="text-center">Registro Vehicular Parqueo Banadesa</h1>
                </div>

                <div class="row">
                    <div class="col-md-3 mb-3">
                           <label for="Placa" class="control-label text-secondary fw-bold">Placa Vehiculo:</label><label id="IdTrx" runat="server" visible="false" class="control-label text-secondary fw-bold"></label>
                           <input id="Placa" name="Placa" type="text" runat="server" placeholder="BND0000" min="7" max="7" class="form-control"/>
                    </div>
                    <div class="col-md-9 mb-3">
                        <label for="NombreCompleto" class="control-label text-secondary fw-bold">Nombre Empleado:</label>
                        <input id="NombreCompleto" name="NombreCompleto" type="text" runat="server" placeholder="Nombre Empleado" min="4" max="80" class="form-control"/>
                    </div>
                </div>

                <!-- Segunda fila de inputs -->
                <div class="row">                 
                    <div class="col-md-4 mb-3">
                        <label for="Marca" class="control-label text-secondary fw-bold">Marca Vehiculo:&nbsp; </label>
                        &nbsp;<input id="Marca" name="Marca" type="text" runat="server" placeholder="Marca" min="2" max="40" class="form-control"/>
                    </div>
                    <div class="col-md-4 mb-3">
                        <label for="Cal1" class="control-label text-secondary fw-bold">Fecha:</label>
                        <asp:TextBox ID="datetimePicker" runat="server" ClientIDMode="Static" CssClass="form-control" />
                    </div>
                    
                    <div class="col-md-4 mb-3">
                    <label for="tipoU_id" class="control-label text-secondary fw-bold">Estado:</label>
                    <asp:DropDownList ID="ddEstado" runat="server" class="form-control" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="ddEstado_SelectedIndexChanged">
                    </asp:DropDownList> 
                    </div>
                </div>

                <!-- Cuarta fila de inputs -->
                <div class="row">
                    <div class="col-md-4 mb-3">
                        <label for="Tel" class="control-label text-secondary fw-bold"># Telefono</label>
                        <input id="Tel" name="Tel" type="text" runat="server" placeholder="Telefono" min="4" max="20" onkeypress="return validarNumeros(event);" class="form-control"/>
                    </div>

                    <div class="col-md-8 mb-2">
                        <label for="Cal2" class="control-label text-secondary fw-bold">Departamento Empleado:</label>
                        <asp:DropDownList ID="ddDepto" runat="server" class="form-control" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="ddDepto_SelectedIndexChanged">
                        </asp:DropDownList> 
                    </div>  
                </div>
                
                 <div class="form-group m-2">
                     <div class="col-md-12 text-center">
                         <asp:Button class="btn btn-warning fw-bold  btn-lg text-white" id="BtnNP" runat="server" OnClick="BtnNP_Click" Text="Registrar" OnClientClick="return validarCampos();" /> 
                         <asp:Button class="btn btn-warning fw-bold btn-lg text-white" id="BtnMP" runat="server" OnClick="BtnMP_Click" Text="Modificar Salida" OnClientClick="return validarCampos();" Visible="false"/>
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
                <!-- Control para buscar por Placa -->
                    <div class="col-md-12 justify-content-center d-flex">  
                        <div class="row">
                            <div class="col-auto">
                                <asp:Label ID="Label1" runat="server" class="fw-bold text-dark" Text="Buscar Auto Por Placa: "></asp:Label>                                
                            </div>
                           <div class="col-auto">
                                <div class="input-group mb-1">
                                    <asp:TextBox ID="txtFiltro" class="form-control" AutoPostBack="true" min="7" max="7" type="text" OnTextChanged="txtFiltro_TextChanged" runat="server"></asp:TextBox>
                                    <button id="Btn_Submit" runat="server" onserverclick="BtnBuscar_Click" class="btn btn-success">
                                        <i class="fa fa-search"></i> Buscar
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>

                <!-- Llenar GridView -->
                     <div>
                         <div class="table-responsive">

                             <asp:GridView ID="GVParqueoV" runat="server" style="font-size: 12px;" class="col-md-12 justify-content-center table table-bordered table-condensed table-striped table-hover" AllowPaging="true" AutoGenerateColumns="False" AutoGenerateSelectButton="True" OnSelectedIndexChanged="GVParqueoV_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="NumRegistro" HeaderText="#Registro" />
                                    <asp:BoundField DataField="NombrePropietario" HeaderText="Nombre Empleado" />
                                    <asp:BoundField DataField="DeptoPertenece" HeaderText="Departamento" />
                                    <asp:BoundField DataField="Tel" HeaderText="Telefono" />
                                    <asp:BoundField DataField="Placa" HeaderText="Placa" />
                                    <asp:BoundField DataField="Fecha" HeaderText="FechaSalida" />
                                    <asp:BoundField DataField="Marca" HeaderText="Marca" />
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                </Columns>
                            </asp:GridView>

                             <%--<asp:GridView ID="GVParqueoV" runat="server" class="col-md-12 justify-content-center table table-bordered table-condensed table-striped table-hover" AllowPaging="true" AutoGenerateColumns="False" AutoGenerateSelectButton="True" OnSelectedIndexChanged="GVParqueoV_SelectedIndexChanged">
                                 <Columns>
                                     
                                     <asp:BoundField DataField="NumRegistro" HeaderText="#Registro" />
                                     <asp:BoundField DataField="NombrePropietario" HeaderText="Nombre Empleado" />                                                                                
                                     <asp:BoundField DataField="DeptoPertenece" HeaderText="Departamento" />                                                                                
                                     <asp:BoundField DataField="Tel" HeaderText="Telefono" />
                                     <asp:BoundField DataField="Placa" HeaderText="Placa" />
                                     <asp:BoundField DataField="Fecha" HeaderText="FechaSalida" />
                                     <asp:BoundField DataField="Marca" HeaderText="Marca" />                                     
                                     <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                 </Columns>
                             </asp:GridView>--%>
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
