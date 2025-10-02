using Org.BouncyCastle.Crypto.Generators;
using SISLAB_API.Areas.Maestros.Models;

using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using API_MEGABOTIKAS.Areas.Maestros.Data.Almacen;

namespace SISLAB_API.Areas.Maestros.Services
{
    public class UsuarioServicio
    {
        private readonly UsuarioRepositorio _UsuarioRepository;

        public UsuarioServicio(UsuarioRepositorio UsuarioRepository)
        {
            _UsuarioRepository = UsuarioRepository;
        }
        public async Task<bool> validarUsuario_Login(string usuario, string dni)
        {
            return await _UsuarioRepository.validarUsuario_Login(usuario, dni);
        }
    }
}