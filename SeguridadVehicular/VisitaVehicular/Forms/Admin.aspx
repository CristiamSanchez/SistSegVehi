<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="VisitaVehicular.Forms.Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Nuevo Usuario</title>
    <link rel="shortcut icon" href="../img/logo.png" />
    <link rel="stylesheet" href="../Content/fontawesome/css/all.min.css" />
        <!-- Referencia al archivo CSS de Bootstrap -->
     <link href="~/Content/bootstrap.min.css" rel="stylesheet" />    
     <link href="../Styles/sweetalert.css" rel="stylesheet" />
     <script src="../Scripts/sweetalert.min.js"></script>
    <style>
    /* Estilo para el cuerpo */
    body {
        margin: 0;
        padding: 0;
        /* Establece la imagen de fondo */
        background-image: url('../img/mainImg.png');
        background-size: cover;
        background-position: center;
        background-attachment: fixed;
    }
     /* Estilo para la capa de transparencia */
        .transparencia {
            /* Establece la posición relativa para que se alinee con el contenido */
            position: absolute;
            /* Ajusta las coordenadas y dimensiones según sea necesario */
            top: 56px; /* Ajusta la distancia desde la parte superior según tu barra de navegación */
            left: 0;
            width: 100%;
            height: calc(100% - 56px); /* Ajusta la altura para que coincida con el resto del contenido */
            /* Establece el color de fondo y la opacidad */
            background-color: rgba(255, 255, 255, 0.5);  /* Blanco con 75% de opacidad */
              /* Establece el z-index para que la capa esté detrás del contenido del formulario */
            z-index: -1;
        }
        .container {   /* Ajusta el margen superior para evitar la superposición con la barra de navegación */
            margin-top: 5px; /* Ajusta la distancia según sea necesario */
        }
        
        /* Cambia el color de fondo del encabezado del GridView */
        #gvNuevos th {
            background-color: #007bff; /* Color azul */
            color: white; /* Color blanco del texto */
        }
        #gvNuevos th a {
            color: inherit; /* El color del enlace será igual al color del texto del encabezado */
            text-decoration: none; /* Eliminar el subrayado */
        }
</style>
<script>
    // Función para establecer la altura de la capa de transparencia
    function setTransparencyHeight() {
        // Obtener la altura total del documento
        var documentHeight = Math.max(
            document.body.scrollHeight,
            document.body.offsetHeight,
            document.documentElement.clientHeight,
            document.documentElement.scrollHeight,
            document.documentElement.offsetHeight
        );

        // Establecer la altura de la capa de transparencia
        var transparency = document.querySelector('.transparencia');
        if (transparency) {
            transparency.style.height = documentHeight + 'px';
        }
    }

    // Llamar a la función cuando se carga la página y cuando cambia el tamaño de la ventana
    window.onload = setTransparencyHeight;
    window.onresize = setTransparencyHeight;
</script>
</head>
<body>

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
                    <a class="nav-link  bg-gradient" href="../Forms/Admin.aspx"><i class="fa fa-venus-mars"></i> Administracion Usuarios</a>
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
            <!-- Contenido de la página -->
            <div class="container bg-success-subtle col-md-12">
            <!-- Aquí puedes colocar el resto de tu contenido -->
                   <div class="row align-items-start">
                       <h2 class="text-center header text-white bg-secondary">Registrar Nuevo Usuario</h2>
                        <div class="col-md-2">      
                            
                        </div>

                        <div class="col-md-8">
                            
                            <div class="row">
                                <div class="col-md-6 mb-1">
                                     <div class="form-group m-2">
                                         <div class=" align-content-center">
                                             <input id="usuN" name="name" type="text" autocomplete="off" runat="server" placeholder="Usuario" min="4" max="15" class="form-control"/>
                                         </div>
                                     </div>
                                </div>
                                <div class="col-md-6 mb-1">
                                    <div class="form-group m-2">
                                        <div class="text-center">
                                            <asp:TextBox ID="passN" min="7" max="15" runat="server" type="text" CssClass="form-control" Placeholder="Contraseña" AutoCompleteType="Disabled" OnMouseOver="showPassword()" OnMouseOut="hidePassword()"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                 
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-1">
                                    <!-- Aquí puedes colocar el resto de tu contenido -->
                                    <div class="form-group m-2">
                                        <div class=" align-content-center">
                                            <input id="fname" name="name" type="text" runat="server" placeholder="Nombres" min="3" max="50" class="form-control"/>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 mb-1">
                                    <!-- Aquí puedes colocar el resto de tu contenido -->
                                     <div class="form-group m-2">
                                         <div class=" text-center">
                                             <input id="lname" name="name" type="text" runat="server" placeholder="Apellidos" min="3" max="50" class="form-control"/>
                                         </div>
                                     </div>
                                </div>
                         </div>


                            <div class="row">
                                   <div class="col-md-6 mb-1">
                                       <!-- Aquí puedes colocar el resto de tu contenido -->
                                       <div class="form-group m-2">
                                            <div class=" text-center">
                                                <asp:DropDownList ID="ddDepto" runat="server" class="form-control" Visible="true">
                                                </asp:DropDownList> 
                                            </div>
                                        </div>

                                   </div>
                                   <div class="col-md-6 mb-1">
                                       <!-- Aquí puedes colocar el resto de tu contenido -->
                                          <div class="form-group m-2">
                                               <div class=" text-center">
                                                   <input id="email" name="email" type="text" runat="server" placeholder="Correo@banadesa.hn" min="4" max="50" class="form-control"/>                                                       
                                               </div>
                                           </div>
                                   </div>                                   
                            </div>
                                                        
                            <div class="row">
                                <div class="col-md-5 mb-1">
                                      <!-- Aquí puedes colocar el resto de tu contenido -->
                                       <div class="form-group m-2"> <!-- State Button -->
                                            <select class="form-control text-secondary" runat="server"  id="tipoU_id">
                                                <option value="U">Usuario Operativo</option>
                                                <option value="P">Usuario Admin TI</option>
                                            </select>
                                        </div>
                                </div>
                                <div class="col-md-4 mb-1">
                                    <!-- Aquí puedes colocar el resto de tu contenido -->
                                    <div class="form-group m-2">
                                        <div class=" text-center">
                                            <input id="phone" name="phone" type="text" runat="server" placeholder="Telefono" min="4" max="20" onkeypress="return validarNumeros(event);" class="form-control"/>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 mb-1">
                                    <!-- Aquí puedes colocar el resto de tu contenido -->
                                   <div class="form-group m-2"> 
                                        <asp:DropDownList ID="ddEstadoM" runat="server" class="form-control text-secondary">
                                        <asp:ListItem Value="A">ACTIVO</asp:ListItem>
                                        <asp:ListItem Value="I">INACTIVO</asp:ListItem>
                                        </asp:DropDownList> 
                                    </div>
                                </div>
                            </div>
                                                
                        </div>

                        <div class="col-md-2">
                        </div>

                        <div class="form-group m-2">
                            <div class="col-md-12 text-center">
                                <asp:Button class="btn btn-warning fw-bold  btn-lg" id="btnNU" runat="server" OnClick="btnNU_Click" Text="Registrar" OnClientClick="return validarCampos();" />
                                <asp:Button class="btn btn-warning fw-bold btn-lg" id="btnMU" runat="server" OnClick="btnMU_Click" Text="Modificar" OnClientClick="return validarCampos();" Visible="false"/>
                                <asp:Button class="btn btn-danger fw-bold  btn-lg" id="btnCancelar" runat="server" OnClick="btnCancelar_Click" Text="Cancelar" Visible="false"/>
                            </div>                                               
                        </div>  
                       
                        <h2 class="text-center header text-white bg-success">Modificar Usuario</h2>

                        <div class="col-md-12 justify-content-center d-flex">  
                            <div class="row">
                                <div class="col-auto">
                                    <asp:Label ID="Label1" runat="server" class="fw-bold text-dark" Text="Buscar Usuario Por Nombre: "></asp:Label>
                                </div>
                                <div class="col-auto">
                                    <asp:TextBox ID="txtFiltro" class="form-control mb-1" AutoPostBack="true" MaxLength="20" type="text" OnTextChanged="txtFiltro_TextChanged" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">     
                            <div class="table-responsive">
                                <asp:GridView ID="gvU" runat="server" class="col-md-12 justify-content-center table table-bordered table-condensed table table-striped table-hover" 
                                    AutoGenerateSelectButton="True"  OnSelectedIndexChanged="gvU_SelectedIndexChanged" AllowPaging="true" OnPageIndexChanging="gvU_PageIndexChanging"
                                    Visible="true">
                                </asp:GridView>
                            </div>
                        </div>    
                 </div>
            </div>
         <div class="transparencia"></div>
    </form>

    <script>

        <%--function showPassword() {
            var passwordField = document.getElementById('<%= passN.ClientID %>');
            passwordField.setAttribute('type', 'text');
        }

        function hidePassword() {
            var passwordField = document.getElementById('<%= passN.ClientID %>');
            passwordField.setAttribute('type', 'password');
        }--%>

        function validarCampos() {
        var usuario = document.getElementById("usu").value;
        var contraseña = document.getElementById("pass").value;
        var fname = document.getElementById("fname").value;
        var lname = document.getElementById("lname").value;
        var email = document.getElementById("email").value;
        var phone = document.getElementById("phone").value;
        var depto = document.getElementById("depto").value;
            if (fname === "" || lname === "" || email === "" || phone === "" || usuario === "" || contraseña === "" || depto === "") {
            alert("Por favor, complete todos los campos.");
            return false;
            }
            validarEmail();
            return true;
        }
        function evitarEspacios(e) {
            if (e.which === 32) {
                e.preventDefault();
                return false;
            }
        }
        function validarNumeros(event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function validarEmail() {
            var emailInput = document.getElementById("email");
            var email = emailInput.value;

            // Expresión regular para validar el formato de un correo electrónico
            var regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

            if (regex.test(email)) {
                //document.getElementById("resultado").innerText = "El correo electrónico es válido.";
            } else {
                //document.getElementById("resultado").innerText = "El correo electrónico es inválido.";
                alert("El correo electrónico es inválido.");
            }
        }

    </script>

</body>
</html>

