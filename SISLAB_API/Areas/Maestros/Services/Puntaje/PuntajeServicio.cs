using Org.BouncyCastle.Crypto.Generators;
using SISLAB_API.Areas.Maestros.Models;

using System.Collections.Generic;
using System.Net.Mail;
using System.Net;

namespace SISLAB_API.Areas.Maestros.Services
{
    public class PuntajeServicio
    {
        private readonly PuntajeRepositorio _PuntajeRepositorio;

        public PuntajeServicio(PuntajeRepositorio UsuarioRepository)
        {
            _PuntajeRepositorio = UsuarioRepository;
        }
        public void InsertarHistorialDescuento(int idCliente, decimal puntosTotal, decimal puntosAntesDescuento,
        decimal puntosDescuento, string nivelAnterior, decimal descuentoAplicado, string nivelFinal)
        {
            // Aquí podemos agregar lógica de negocio antes de insertar el historial
            // Ejemplo: Verificar que el cliente tenga puntos suficientes, etc.

            if (puntosDescuento > puntosAntesDescuento)
            {
                throw new InvalidOperationException("Los puntos de descuento no pueden ser mayores que los puntos antes del descuento.");
            }

            // Crear el objeto historial
            var historialDescuento = new HistorialDescuento
            {
                IdCliente = idCliente,
                PuntosTotal = puntosTotal,
                PuntosAntesDescuento = puntosAntesDescuento,
                PuntosDescuento = puntosDescuento,
                NivelAnterior = nivelAnterior,
                DescuentoAplicado = descuentoAplicado,
                NivelFinal = nivelFinal,
                FechaAplicacion = DateTime.Now // Fecha actual
            };

            // Llamamos al repositorio para insertar los datos en la base de datos
            _PuntajeRepositorio.InsertarHistorialDescuento(historialDescuento);
        }

        // Obtener el historial de descuentos de un cliente
        public IEnumerable<HistorialDescuento> ObtenerHistorialPorCliente(int idCliente)
        {
            return _PuntajeRepositorio.ObtenerHistorialPorCliente(idCliente);
        }

        public async Task<IEnumerable<ProductosPuntos>> ObtenerTodosLosServicios()
        {
            try
            {
                return await _PuntajeRepositorio.ObtenerTodosServicios();
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudieron obtener los servicios. Por favor, intente nuevamente.", ex);
            }
        }

        public async Task<List<AcumulacionPuntos>> ObtenerVentasPuntos(int idUsuario)
        {
            try
            {
                return await _PuntajeRepositorio.ObtenerVentasPuntos(idUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener ventas puntos: {ex.Message}", ex);
            }
        }

        public async Task<List<ProductoCanjeable>> ObtenerProductosCanjeables(int? idUsuario)
        {
            try
            {
                return await _PuntajeRepositorio.ObtenerProductosCanjeables(idUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener productos canjeables: {ex.Message}", ex);
            }
        }

        public async Task<ResultadoCanje> CanjearProducto(int idUsuario, int codigoProducto,string tipomovimiento)
        {
            try
            {
                // Validaciones adicionales del servicio
                if (idUsuario <= 0)
                    throw new ArgumentException("ID de usuario inválido");

                if (codigoProducto <= 0)
                    throw new ArgumentException("Código de producto inválido");

                return await _PuntajeRepositorio.CanjearProducto(idUsuario, codigoProducto,tipomovimiento);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al canjear producto: {ex.Message}", ex);
            }
        }

        public async Task<decimal> ObtenerSaldoPuntos(int idUsuario)
        {
            try
            {
                if (idUsuario <= 0)
                    throw new ArgumentException("ID de usuario inválido");

                return await _PuntajeRepositorio.ObtenerSaldoPuntos(idUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener saldo de puntos: {ex.Message}", ex);
            }
        }

        public async Task<List<HistorialCompleto>> ObtenerHistorialCompleto(int idUsuario)
        {
            try
            {
                if (idUsuario <= 0)
                    throw new ArgumentException("ID de usuario inválido");

                return await _PuntajeRepositorio.ObtenerHistorialCompleto(idUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener historial completo: {ex.Message}", ex);
            }
        }

        public async Task<SaldoPuntos> ObtenerSaldoPuntosModel(int idUsuario)
        {
            try
            {
                var saldo = await ObtenerSaldoPuntos(idUsuario);
                return new SaldoPuntos { SaldoPunto = saldo };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener saldo de puntos: {ex.Message}", ex);
            }
        }
    }
}