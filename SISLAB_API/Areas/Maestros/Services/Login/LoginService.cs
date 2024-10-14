
using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;


namespace SISLAB_API.Areas.Maestros.Services
{
    public class LoginService
    {
        private readonly LoginRepository _loginRepository;

        public LoginService(LoginRepository LoginRepository)
        {
            _loginRepository = LoginRepository;
        }

        public async Task<Login> AuthenticateUserAsync(string username, string password)
        {
            return await _loginRepository.AuthenticateUserAsync(username, password);
        }

        public async Task AddUserAsync(Usuario usuario)
        {
            // Aquí puedes agregar lógica adicional, como validaciones o hashing de contraseñas
            await _loginRepository.AddAsync(usuario);
        }

    }
}
























