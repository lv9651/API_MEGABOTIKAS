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



        public async Task<IEnumerable<Puntaje>> ObtenerVentasPuntosAsync(int idUsuario)
        {
            return await _PuntajeRepositorio.ObtenerVentasPuntosAsync(idUsuario);
        }
    }
}