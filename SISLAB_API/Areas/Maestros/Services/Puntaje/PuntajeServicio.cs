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


        public async Task<ClientePuntosResponse> ActualizarPuntosCliente(string correo)
        {
            return await _PuntajeRepositorio.ActualizarPuntos(correo);
        }


        public async Task<NivelCompletoResponse> ObtenerNivelCliente(string correo)
        {
            // 1. Obtener los puntos del cliente
            var puntosResp = await _PuntajeRepositorio.ActualizarPuntos(correo);
            if (puntosResp == null)
            {
                return null;
            }

            decimal puntos = puntosResp.PuntosTotales; // <- puedes usar también Disponibles si la lógica cambia
            int idCliente = puntosResp.IdCliente;      // <- asegúrate que ActualizarPuntos devuelva el ID del cliente

            // 2. Con esos puntos e IdCliente, llamar al SP
            var nivelCompleto = await _PuntajeRepositorio.ObtenerNivelPorPuntos(puntos, idCliente);
            return nivelCompleto;
        }

        public async Task<IEnumerable<HistorialCanje>> ObtenerHistorialAsync(int idCliente)
        {
            return await _PuntajeRepositorio.ObtenerHistorialAsync(idCliente);
        }

        public async Task<CanjeResponse> ProcesarCanje(CanjeRequest request)
        {
            return await _PuntajeRepositorio.Canjear(request);
        }






    }
}