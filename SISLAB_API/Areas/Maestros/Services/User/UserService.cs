



using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;


public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }



    public async Task<IEnumerable<Role>> GetAllRolsAsync()
    {
        return await _userRepository.GetAllRolAsync();
    }
    // Puedes agregar más métodos aquí para manejar otras operaciones (ej. agregar, actualizar, eliminar usuarios)
}























