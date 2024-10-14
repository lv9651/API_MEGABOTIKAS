
using SISLAB_API.Areas.Maestros.Models;


namespace SISLAB_API.Areas.Maestros.Services
{
    public class EmpleadoService
    {
        private readonly EmpleadoRepository _empleadoRepository;

        public EmpleadoService(EmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }

        public async Task<IEnumerable<Empleado>> ObtenerEmpleadosAsync()
        {
            return await _empleadoRepository.ObtenerEmpleadosAsync();
        }
    }
}
