using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserService
{
    private readonly UserRepository _userRepository;

    // Inyección de dependencias para el repositorio
    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Obtener todos los usuarios
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    // Obtener todos los roles
    public async Task<IEnumerable<Role>> GetAllRolsAsync()
    {
        return await _userRepository.GetAllRolAsync();
    }

    // Método para actualizar un usuario
    public async Task<bool> UpdateUserAsync(User user)
    {
        // Llamar al repositorio para actualizar el usuario en la base de datos
        var result = await _userRepository.UpdateUserAsync(user);
        return result;
    }

    // Aquí podrías agregar más métodos según lo necesites (ej. agregar, eliminar usuarios)
}