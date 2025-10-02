using API_MEGABOTIKAS.Areas.Maestros.Data.Almacen;
using API_MEGABOTIKAS.Areas.Maestros.Models.Almacen;

namespace API_MEGABOTIKAS.Areas.Maestros.Services.Almacen
{
    public class AlmacenServicio
    {

        private readonly AlmacenRepositorio _almacenRepositorio;

        public AlmacenServicio(AlmacenRepositorio almacenRepositorio)
        {
            _almacenRepositorio = almacenRepositorio;
        }
        public async Task<IEnumerable<StockLocalDto>> obtenerStock_X_Local(int codArticulo)
        {
            return await _almacenRepositorio.obtenerStock_X_Local(codArticulo);
        }


    }
}
