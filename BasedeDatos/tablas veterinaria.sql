

create table ROL(
IdRol int primary key identity,
Descripcion varchar(50),
FechaRegistro datetime default getdate()
)

go

Create table PERMISOS(
IdPermiso int primary key identity,
Idrol int references ROL(Idrol),
NombreMenu varchar(50),
FechaRegistro datetime default getdate()
)

go

Create table CLIENTES(
IdCliente int primary key identity,
Documento varchar(10),
NombreCliente varchar(50),
Telefono varchar(12),
Estado bit,
FechaRegistro datetime default getdate()
)

go


create table MASCOTA(
IdMascota int primary key identity,
IdCliente int references CLIENTES(IdCliente),
NombreMascota varchar(50),
Tipo varchar(20),
Raza varchar(30),
Estado bit,
FechaRegistro datetime default getdate()
)

go

Create table USUARIO(
IdUsuario int primary key identity,
Documento varchar(10),
NombreUsuario varchar(50),
Correo varchar(50),
Clave varchar(25),
Idrol int references ROL(Idrol),
Estado bit,
FechaRegistro datetime default getdate()
)

go


Create table SERVICIO(
IdServicio int primary key identity,
Codigo varchar(10),
NombreServicio varchar(50),
PrecioVenta decimal(10,2) default 0,
Estado bit,
FechaRegistro datetime default getdate()
)


go



Create table VENTA(
IdVenta int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
NumeroDocumento varchar(10),
DocumentoCliente varchar(10),
NombreCliente varchar(50),
NombreMacota varchar(50),
TipoMascota varchar(20)
MontoTotal decimal(10,2),
FechaRegistro datetime default getdate()
)

go

Create table DETALLE_VENTA(
IdDetalleVenta int primary key identity,
IdVenta int references VENTA(IdVenta),
IdServicio int references SERVICIO(IdServicio),
PrecioVenta decimal(10,2) default 0,
Subtotal decimal(10,2),
FechaRegistro datetime default getdate()
)

go

create table NEGOCIO(
IdNegocio int primary key,
Nombre varchar(60),
RUC varchar(60),
Direccion varchar(60),
Logo varbinary(max) null
)
go

/*insertar uno por uno de darse cuenta que se funciona con un select a la tabla rol*/

insert into ROL(Descripcion) values('ADMINISTRADOR') 
insert into ROL(Descripcion) values('EMPLEADO')



/*insertar uno por uno de darse cuenta que se funciona con un select a la tabla permisos*/
/*el idrol tiene que ser igual a la de la tabla rol asegurarse de eso y cambiarlo si es el caso*/
/*el idrol '1'(uno) hace referencia al rol administrador, asegurarse que tanto el idrol de ambas tablas sean iguales en ambas tablas*/
/*el idrol '2'(dos) hace referencia al rol administrador, asegurarse que tanto el idrol de ambas tablas sean iguales en ambas tablas*/

insert into PERMISOS(Idrol,NombreMenu) values(1,'menuUsuarios')
insert into PERMISOS(Idrol,NombreMenu) values(1,'menuVentas')
insert into PERMISOS(Idrol,NombreMenu) values(1,'MenuClientes')
insert into PERMISOS(Idrol,NombreMenu) values(1,'MenuReportes')
insert into PERMISOS(Idrol,NombreMenu) values(1,'menuMantenedor')

insert into PERMISOS(Idrol,NombreMenu) values(2,'menuVentas')
insert into PERMISOS(Idrol,NombreMenu) values(2,'MenuClientes')
insert into PERMISOS(Idrol,NombreMenu) values(2,'MenuReportes')


/* creacion de un usuario para poder iniciar sesion en el fmr de login(cambiar el idrol de ser necesario, dependeiendo del que se cree anterioemente) */
insert into USUARIO(Documento, NombreUsuario, Correo, Clave, Idrol, Estado) values('101010','ADMIN', '@gmail.com', '123', 1, 1)