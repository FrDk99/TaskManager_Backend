using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaskManager.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pasa la solicitud al siguiente middleware
                await _next(context);
            }
            catch (SqlException sqlEx)
            {
                // Maneja errores específicos de SQL Server basados en el código de error
                _logger.LogError($"SQL Server Error: {sqlEx.Message}");
                await HandleSqlExceptionAsync(context, sqlEx);
            }
            catch (InvalidOperationException invOpEx)
            {
                // Errores relacionados con operaciones inválidas (ej. transacciones fallidas)
                _logger.LogError($"Invalid operation: {invOpEx.Message}");
                await HandleExceptionAsync(context, invOpEx, HttpStatusCode.BadRequest, "Invalid operation.");
            }
            catch (KeyNotFoundException notFoundEx)
            {
                _logger.LogError($"Resource not found: {notFoundEx.Message}");
                await HandleExceptionAsync(context, notFoundEx, HttpStatusCode.NotFound, "Recurso no encontrado.");
            }
            catch (ArgumentNullException argNullEx)
            {
                _logger.LogError($"Invalid input: {argNullEx.Message}");
                await HandleExceptionAsync(context, argNullEx, HttpStatusCode.BadRequest, "Faltan datos en la solicitud.");
            }
            catch (Exception ex)
            {
                // Maneja todos los demás errores
                _logger.LogError($"Something went wrong: {ex.Message}");
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private Task HandleSqlExceptionAsync(HttpContext context, SqlException sqlEx)
        {
            HttpStatusCode statusCode;
            string message;

            switch (sqlEx.Number)
            {
                // Violación de índice de clave duplicada
                case 2601:
                // Violación de restricción de clave única
                case 2627:
                    statusCode = HttpStatusCode.Conflict;
                    message = "Ya existe un registro con este valor.";
                    break;

                // Violación de restricción de clave externa
                case 547:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "No se puede eliminar o actualizar este registro debido a una restricción de clave externa.";
                    break;

                // Insertar NULL en una columna no nullable
                case 515:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Falta un campo requerido.";
                    break;

                // Desbordamiento numérico general
                case 8115:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Un campo numérico contiene un valor que está fuera del rango permitido. Verifique los valores e inténtelo de nuevo.";
                    break;

                // Desbordamiento de cadena (VARCHAR, NVARCHAR)
                case 8152:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "La cadena de entrada excede la longitud máxima permitida. Por favor, verifique su entrada.";
                    break;

                // Desbordamiento de número entero
                case 245:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Ocurrió un desbordamiento de datos numéricos. Verifique los valores e inténtelo de nuevo.";
                    break;

                // Error de conversión de datos
                case 241:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "La conversión de datos falló. Verifique los valores de entrada.";
                    break;

                // Desbordamiento de datos binarios (VARBINARY)
                case 2628:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "El valor binario excede el tamaño máximo permitido. Verifique los datos e inténtelo de nuevo.";
                    break;

                // Desbordamiento de tipo decimal
                case 8114:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "El valor decimal proporcionado está fuera del rango permitido.";
                    break;

                // Desbordamiento o error en campos de fecha (DATE, DATETIME)
                case 242: // Error de conversión de fecha
                    statusCode = HttpStatusCode.BadRequest;
                    message = "El valor de la fecha o la hora no es válido. Por favor, use el formato correcto y verifique los valores.";
                    break;

                // Error de conversión en tipo BIT (espera 0 o 1)
                case 206: // Error de conversión de BIT
                    statusCode = HttpStatusCode.BadRequest;
                    message = "El valor para el campo booleano (BIT) debe ser 0 o 1.";
                    break;

                // Violación de restricciones de chequeo
                case 50000:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Se violó una regla de negocio.";
                    break;

                // Error de tiempo de espera
                case 1205: // Deadlock
                    statusCode = HttpStatusCode.Conflict;
                    message = "Se produjo un deadlock. Por favor, intente su solicitud nuevamente.";
                    break;

                // Error de sesión cerrada
                case 40197: // Sesión cerrada
                case 40540: // Sesión cerrada debido a recursos insuficientes
                    statusCode = HttpStatusCode.ServiceUnavailable;
                    message = "El servidor SQL está temporalmente no disponible. Por favor, inténtelo más tarde.";
                    break;

                // Otros errores no específicos
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "Se produjo un error en la base de datos.";
                    break;
            }

            return HandleExceptionAsync(context, sqlEx, statusCode, message);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Detailed = exception.Message  // Puede eliminarse en producción para mayor seguridad
            };

            // Serializa la respuesta en formato JSON
            var jsonResult = JsonSerializer.Serialize(result);

            // Escribe manualmente la respuesta en el cuerpo
            return context.Response.WriteAsync(jsonResult);
        }
    }
}
