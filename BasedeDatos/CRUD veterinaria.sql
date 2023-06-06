/* -------------------Procedimientos para usuarios -----------------*/


create Proc SP_REGISTRARUSUARIO(
@Documento varchar(10),
@NombreUsuario varchar(50),
@Correo varchar(50),
@Clave varchar(25),
@IdRol int,
@Estado bit,
@IdUsuarioResultado int output,
@Mensaje varchar(300) output
)
as
begin 
	set @IdUsuarioResultado = 0
	set @Mensaje= ''
	if not exists(select * from USUARIO where Documento = @Documento)
	begin
		insert into USUARIO(Documento, NombreUsuario, Correo, Clave, Idrol,Estado)values
		(@Documento, @NombreUsuario, @Correo, @Clave, @IdRol, @Estado)

		set @IdUsuarioResultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje= 'No se puede guardar ususarios con el mismo documento'

end
go


create Proc SP_EDITARUSUARIO(
@IdUsuario int,
@Documento varchar(10),
@NombreUsuario varchar(50),
@Correo varchar(50),
@Clave varchar(25),
@IdRol int,
@Estado bit,
@Respuesta bit output,
@Mensaje varchar(300) output
)
as
begin 
	set @Respuesta = 0
	set @Mensaje= ''
	if not exists(select * from USUARIO where Documento = @Documento and IdUsuario!= @IdUsuario)
	begin
		update USUARIO set 
		Documento = @Documento, 
		NombreUsuario = @NombreUsuario, 
		Correo = @Correo, 
		Clave = @Clave, 
		Idrol = @IdRol,
		Estado = @Estado
		where IdUsuario = @IdUsuario
		set @Respuesta = 1
	end
	else
		set @Mensaje= 'No se puede guardar ususarios con el mismo documento'

end
go



create Proc SP_ELIMINARUSUARIO(
@IdUsuario int,
@Respuesta bit output,
@Mensaje varchar(300) output

)
as
begin 
	set @Respuesta = 0
	set @Mensaje= ''
	declare @PasoRegla bit = 1

	if exists(select * from VENTA V 
	inner join USUARIO U ON U.IdUsuario = V.IdUsuario
	WHERE U.IdUsuario = @IdUsuario)
	begin
		set @PasoRegla = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar el usuario ya que se encuentra relacionado a una compra\n'
	end
	
	if(@PasoRegla =1)
	begin 
		delete from USUARIO WHERE IdUsuario = @IdUsuario
		set @Respuesta = 1
	end
	
end
go





/* -------------------Procedimientos para servicio -----------------*/

create PROC SP_RegistrarServicio(
@Codigo varchar(10),
@NombreServicio varchar(50),
@PrecioVenta decimal(10,2),
@Estado bit,
@Resultado int output,
@Mensaje varchar(300) output
)
as
begin
	set @Resultado = 0
	IF not exists(SELECT * from SERVICIO where Codigo = @Codigo)
	begin
		insert into SERVICIO(Codigo, NombreServicio, PrecioVenta, Estado) values (@Codigo, @NombreServicio, @PrecioVenta, @Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		set @Mensaje = 'Ya existe un producto con el mismo codigo'
end

go

create PROC SP_ModificarServicio(
@IdServicio int,
@Codigo varchar(10),
@NombreServicio varchar(50),
@PrecioVenta decimal(10,2),
@Estado bit,
@Resultado int output,
@Mensaje varchar(300) output
)
as
begin 
	set @Resultado = 1
	IF not exists(Select * from SERVICIO where Codigo = @Codigo and IdServicio != @IdServicio)
		
		update SERVICIO set 
		Codigo = @Codigo,
		NombreServicio = @NombreServicio,
		PrecioVenta = @PrecioVenta,
		Estado = @Estado
		where IdServicio = @IdServicio
	ELSE
	begin
		set @Resultado = 0
		set @Mensaje = 'Ya existe un producto con el mismo codigo'
	end
end
go


create PROC SP_EliminarServicio(
@IdServicio int,
@Respuesta bit output,
@Mensaje varchar(300) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''
	declare @pasoregla bit = 1

	IF exists (select * from DETALLE_VENTA dv
	inner join SERVICIO s ON s.IdServicio = dv.IdServicio
	where s.IdServicio = @IdServicio)

	begin 
		set @pasoregla = 0
		set @Respuesta = 0
		set @Mensaje = 'No se puede eliminar porque se encuentra relacionado a una venta'
	end

	If(@pasoregla = 1)
	begin
		delete from SERVICIO where IdServicio = @IdServicio
		set @Respuesta = 1
	end

end

go


/* -------------------Procedimientos para clientes -----------------*/

create PROC SP_RegistrarCliente(
@Documento varchar(10),
@NombreCliente varchar(50),
@Telefono varchar(12),
@Estado bit,
@Resultado int output,
@Mensaje varchar(300) output
)as
begin
	set @Resultado = 0
	declare @IdPersona int
	IF not exists (select * from CLIENTES where Documento = @Documento )
	begin
		insert into CLIENTES(Documento, NombreCliente, Telefono, Estado ) values (
		 @Documento, @NombreCliente, @Telefono, @Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
	begin
		set @Mensaje = 'El numero de documento ya existe'
	end
end

go

create PROC SP_ModificarCliente(
@IdCliente int,
@Documento varchar(10),
@NombreCliente varchar(50),
@Telefono varchar(12),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(300) output
)as
begin
	set @Resultado = 1
	declare @IdPersona int
	IF not exists (select * from CLIENTES where Documento = @Documento and IdCliente != @IdCliente)
	begin
		update CLIENTES set 
		Documento = @Documento,
		NombreCliente = @NombreCliente,
		Telefono = @Telefono,
		Estado = @Estado
		where IdCliente = @IdCliente
	end
	else
	begin
		set @Resultado = 0
		set @Mensaje = 'El numero de documento ya existe'
	end
		
end

go


create PROC SP_EliminarCliente(
@IdCliente int,
@Respuesta bit output,
@Mensaje varchar(300) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''
	declare @pasoregla bit = 1

	IF exists (select * from MASCOTA m inner join CLIENTES c on c.IdCliente = m.IdCliente
				where c.IdCliente = @IdCliente)

	begin 
		set @pasoregla = 0
		set @Respuesta = 0
		set @Mensaje = 'No se puede eliminar porque se encuentra relacionado a una mascota'
	end

	If(@pasoregla = 1)
	begin
		delete from CLIENTES where IdCliente = @IdCliente
		set @Respuesta = 1
	end

end

go


/* -------------------Procedimientos para mascotas -----------------*/

create PROC SP_RegistrarMascota(
@IdCliente int,
@Nombremascota varchar(50),
@Raza varchar(12),
@Estado bit,
@Tipo varchar(20),
@Resultado int output,
@Mensaje varchar(300) output
)as
begin
	set @Resultado = 0
	declare @IdPersona int
	IF not exists (select * from MASCOTA where NombreMascota = @Nombremascota and IdCliente = @IdCliente and Raza = @Raza)
	begin
		insert into MASCOTA(IdCliente, NombreMascota, Raza, Estado, Tipo ) values (
		 @IdCliente, @Nombremascota, @Raza, @Estado, @Tipo)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
	begin
		set @Mensaje = 'La mascota ya existe'
	end
end

go


create PROC SP_ModificarMascota(
@IdMascota int,
@IdCliente int,
@Nombremascota varchar(50),
@Raza varchar(12),
@Estado bit,
@Tipo varchar(20),
@Resultado bit output,
@Mensaje varchar(300) output
)as
begin
	
	set @Resultado = 1
	
	IF not exists (select * from MASCOTA where NombreMascota = @Nombremascota and Raza = @Raza and Tipo = @Tipo)
	begin
	
		update MASCOTA set 
		IdCliente = @IdCliente,
		NombreMascota = @Nombremascota,
		Raza = @Raza,
		Estado = @Estado,
		Tipo = @Tipo
		where IdMascota = @IdMascota
	end
	else
	begin
		set @Resultado = 0
		set @Mensaje = 'La mascota ya existe'
	end
	
		
end

go


/* -------------------Procedimientos para ventas -----------------*/

create type [dbo].[EDetalle_Venta] As table(
[IdServicio] int null,
[PrecioVenta] decimal(18,2) null
)
go


create PROC SP_RegistrarVenta(
@IdUsuario int,
@NumeroDocumento varchar(10),
@DocumentoCliente varchar(10),
@NombreCliente varchar(50),
@MontoTotal decimal(18,2),
@NombreMascota varchar(50),
@TipoMascota varchar(20),
@DetalleVenta [EDetalle_Venta] READONLY,                                      
@Resultado bit output,
@Mensaje varchar(300) output
)
as
begin
	
	begin try

		declare @idventa int = 0
		set @Resultado = 1
		set @Mensaje = ''

		begin  transaction registro

		insert into VENTA(IdUsuario,NumeroDocumento,DocumentoCliente,NombreCliente,MontoTotal, NombreMacota, TipoMascota)
		values(@IdUsuario,@NumeroDocumento,@DocumentoCliente,@NombreCliente,@MontoTotal,@NombreMascota,@TipoMascota)

		set @idventa = SCOPE_IDENTITY()

		insert into DETALLE_VENTA(IdVenta,IdServicio, PrecioVenta)
		select @idventa,IdServicio,PrecioVenta from @DetalleVenta

		commit transaction registro

	end try
	begin catch
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch

end

go






create PROC SP_ReporteVentas(
@FechaInicio varchar(10),
@FechaFin varchar(10)
)as

begin

set dateformat dmy;

select convert(char(10), v.FechaRegistro,103)[FechaRegistro], v.NumeroDocumento,
v.MontoTotal,u.NombreUsuario, v.DocumentoCliente, v.NombreCliente, v.NombreMacota, 
v.TipoMascota, s.Codigo[CodigoServicio], s.NombreServicio,dv.PrecioVenta
from VENTA v inner join USUARIO u on u.IdUsuario = v.IdUsuario
inner join DETALLE_VENTA dv on dv.IdVenta = v.IdVenta
inner join SERVICIO s on s.IdServicio = dv.IdServicio
where convert(date, v.FechaRegistro) between @FechaInicio and @FechaFin


end
go



