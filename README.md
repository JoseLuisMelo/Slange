# Slange

Slange es un micro ORM progresivo que te ayuda a mapear modelos de datos y hace mas fácil la ejecución de transacciones SQL con C#.


# Micro ORM For C#

Slange genera el código **Sql** necesario para que puedas **Insertar**, **Eliminar**, **Actualizar** y **Buscar** los modelos de datos de tu aplicación en pocos pasos.
Al ser una herramienta progresiva NO es necesario que comiences a usarla desde la creación de tu proyecto, puedes añadirla en aplicaciones ya comenzadas aunque deberás cambiar ciertas cosas.

## Instalación

Descarga e incluye la **dll** de **Slange** en tu proyecto.


## Clase Request
Todas las instrucciones que Slange puede ejecutar (**Insert, Delete, Update y Select**) heredan de la clase **Slange.Sql.Request** la cual tiene más o menos esta estructura:
```sh
public class Request
{
    public string Statement { get; }
    public List<SqlParameter> Parameters { get; }
    public bool IsSotoreProcedure { get; }
}
```
| Propiedad | Función|
| ------ | ------ |
| Statement | Instrucción Sql o nombre de procedimiento almacenado |
| Parameters | Parámetros que utiliza la consulta para tomar valores |
| IsSotoreProcedure| La ejecución corresponde a un enunciado Sql o a un Procedimiento almacenado |

Aunque también puedes crear tu propia variante.

## Crea tu primer modelo

Recuerda que un modelo es una representación en código de una entidad como *Tabla* o *Vista*, las propiedades que tenga corresponden a los campos de la misma y en este punto es importante mencionar que para que la herramienta funcione, **las propiedades deben tener el mismo nombre en tu clase y en tu *Base de Datos*** (recuerda que el modelo debe ser una imagen idéntica) para nuestro ejemplo usaremos la clase/modelo **Usuario** (el nombre de tu modelo puede ser el que tu definas, pero muchas herramientas proponen usar el nombre original de tu tabla en **singular**, más adelante configuraremos el nombre original de la misma).
```sh
public class Usuario
{
    public string Correo{ get; set; }

    public string Contraseña{ get; set; }
}
```

## Crea un contexto personalizado

Como en otras herramientas y siguiendo el **principió de única responsabilidad**, necesitamos una clase que sea la encargada de *Conectar* tu modelo con la base de datos, llamemos a esta clase **UsuariosContext**.
```sh
public class UsuariosContext : ModelContext
{
    public static UsuariosContext Context = new UsuariosContext();

    private UsuariosContext() : base("usuarios.Usuarios")
    {
    }
}
```
Ya que esta clase será el único punto de contacto que tendremos del modelo con la base de datos, creamos un **Singleton** el cual cumplirá dicha función.

Observa que esta clase hereda de **ModelContext** y a su constructor pasamos un string. Este string es muy importante ya que aquí indicamos **cómo se llama** nuestro modelo en **base de datos**.

## Añade el generador de código Sql
La implementación de las siguientes interfaces, te permitirá comenzar a interactuar con la base de datos:

| Interfaz | Propósito |
| ------ | ------ |
| IInsert\<T> | Especifica el tipo de modelo que vas a usar  |
| IUpdate\<T> | Especifica el tipo de modelo que vas a usar  |
| ISelect\<T> | Especifica el tipo de modelo que vas a usar  |
| IDelete | N/A  |

Veamos la implementación en nuestro **UsuariosContext**:

```sh
public class UsuariosContext : ModelContext, ISelect<Usuario>, IInsert<Usuario>, IUpdate<Usuario>
{
    public static UsuariosContext Context = new UsuariosContext();

    private UsuariosContext() : base("usuarios.Usuarios")
    {
        Insert = new InsertAction<Usuario>(ModelName);
        Update = new UpdateAction<Usuario>(ModelName);
        Select = new SelectAction<Usuario>(ModelName);
	}
	
    public SelectAction<Usuario> Select { get; }

    public InsertAction<Usuario> Insert { get; }

    public UpdateAction<Usuario> Update { get; }
}
```

Como puedes ver añadí todas las opciones excepto la de eliminar (aunque podría añadirla).

Con esto queda finalizada la configuración de nuestro modelo para realizar operaciones con nuestra base de datos, veámoslo en acción:

```sh
var miUsuario = new Usuario()
{
    Correo = "ejemplo@correo.com",
    Contraseña = "MyPass"
};

//Insertando todos los campos
UsuariosContext.Context.Insert.Save(u => u, miUsuario);

//Insertando solo el correo
UsuariosContext.Context.Insert.Save(u => new { u.Correo }, miUsuario);

//Actualizando todos los campos
UsuariosContext.Context.Update.Save(u => u, miUsuario, $"WHERE Correo = '{miUsuario.Correo}'");

//Actualizando solo la contraseña
UsuariosContext.Context.Update.Save(u => new { u.Contraseña }, miUsuario, $"WHERE Correo = '{miUsuario.Correo}'");

//Seleccionando todos los registros
UsuariosContext.Context.Select.All();

//Seleccionando todos los registros que coincidan con una condición
UsuariosContext.Context.Select.Where($"Correo = '{miUsuario.Correo}'");

//Seleccionando los primeros 10 registros que coincidan con una condición (observe que podemos incluir un "order by" en todos los tipos de Select)
UsuariosContext.Context.Select.Top(10, $"Correo = '{miUsuario.Correo}' order by Correo desc");

//Contamos todos los registros que coincidan con una condición
UsuariosContext.Context.Select.Count($"Correo = '{miUsuario.Correo}'");

//Eliminamos los registros que coincidan con una condición
UsuariosContext.Context.Delete.Where($"Correo = '{miUsuario.Correo}'");
```

Genial, ahora solo falta lo mas importante, **ejecutar los cambios**, así es, hasta el momento los resultados de las líneas anteriores solo nos proveen la estructura del enunciado Sql que vamos a utilizar, pero aun no lo ejecutamos. 

## Clase Db

La clase Db es la encargada de ejecutar cualquier instrucción del tipo **Request** que queramos, y podemos hacer hacer esta ejecución, esperando 4 diferentes tipos de resultados:

| Llamada| Descripción|
| ------ | ------ |
| Db.Exec.BoolRequest| Ejecuta una request y devuelve **false** si no se modificó ninguna fila de alguna entidad (o si falló un procedimiento), y **true** en caso contrario |
| Db.Exec.IntRequest| Ejecuta una request y devuelve el número de filas afectadas por la consulta o procedimiento |
| Db.Exec.TableRequest| Ejecuta una request y devuelve la DataTable resultante de una consulta o procedimiento |
| Db.Exec.DataSetRequest| Ejecuta una request y devuelve el DataSet resultante de una consulta o procedimiento|

Cada uno de estos métodos recibe como parámetro una **Request** y una **SqlConnection** por lo que te recomiendo echarle un ojo al Nuget de **Microsoft.Data.SqlClient**

## Ejecutando nuestras Request

Ya está, ahora que sabemos que pasa en nuestro código podemos ejecutar nuestras instrucciones únicamente poniendo un (**.**) y buscando alguno de los métodos anteriores para poder ejecutar nuestra consulta.


# Otras herramientas 

He creado algunas herramientas que pueden ser de utilidad no solo para usar con Slange si no también de manera individual, son algunos **métodos de extensión** que te ayudaran a manejar mejor los datos:

| Para | Método | Descripción |
| ------ | ------ | ------ |
| DataTable | ToList\<T>() | Para convertir un DataTable a lista de objetos |
| DataTable | FirstCell() | Para obtener el valor de la primera celda (posición[0,0]) de un DataTable (útil cuando se ejecuta un **count**) |

# Mis Redes
Sígueme en [LinkedIn](https://www.linkedin.com/in/melorojasluis/) si tienes alguna pregunta, con gusto te respondo

 