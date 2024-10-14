using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SISLAB_API.Areas.Maestros.Services
{
    public class DocumentoService
    {
        private readonly DocumentRepository _documentRepository;
        private readonly string _sharedPath;

        public DocumentoService(DocumentRepository documentRepository, string sharedPath)
        {
            _documentRepository = documentRepository;
            _sharedPath = sharedPath;
        }



        public async Task SavePdfsAsync(Dictionary<string, Stream> files, string descripcion, string mes, string anio)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("El diccionario de archivos no puede ser nulo o vacío.");
            }

            foreach (var file in files)
            {
                string fileName = file.Key;
                Stream fileStream = file.Value;

                // Crear la ruta del archivo usando año y mes
                string directoryPath = Path.Combine(_sharedPath, anio, mes);
                string filePath = Path.Combine(directoryPath, fileName);

                try
                {
                    // Crear el directorio si no existe
                    Directory.CreateDirectory(directoryPath);

                    // Guardar el archivo en la ruta compartida
                    using (var fileStreamToSave = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await fileStream.CopyToAsync(fileStreamToSave);
                    }

                    // Guardar el registro en la base de datos
                    await _documentRepository.InsertDocumentAsync(descripcion, mes, anio, fileName);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"No se pudo guardar el archivo PDF {fileName}.", ex);
                }
            }
        }








        public async Task SaveBenPdfsAsync(Dictionary<string, Stream> files, string dni, string descripcion, string beneficio)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("El diccionario de archivos no puede ser nulo o vacío.");
            }

            foreach (var file in files)
            {
                string fileName = file.Key;
                Stream fileStream = file.Value;

                // Crear la ruta del archivo usando año y mes
                string directoryPath = Path.Combine(_sharedPath, "empleado",dni,beneficio);
                string filePath = Path.Combine(directoryPath, fileName);

                try
                {
                    // Crear el directorio si no existe
                    Directory.CreateDirectory(directoryPath);

                    // Guardar el archivo en la ruta compartida
                    using (var fileStreamToSave = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await fileStream.CopyToAsync(fileStreamToSave);
                    }

                    // Guardar el registro en la base de datos
                    await _documentRepository.InsertBenefAsync(dni, descripcion, beneficio, fileName);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"No se pudo guardar el archivo PDF {fileName}.", ex);
                }
            }
        }




        public async Task SavefirmasAsync(Dictionary<string, Stream> files, string dni)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("El diccionario de archivos no puede ser nulo o vacío.");
            }

            foreach (var file in files)
            {
                string fileName = file.Key;
                Stream fileStream = file.Value;

                // Crear la ruta del archivo usando año y mes
                string directoryPath = Path.Combine(_sharedPath,"firmas", dni);
                string filePath = Path.Combine(directoryPath, fileName);

                try
                {
                    // Crear el directorio si no existe
                    Directory.CreateDirectory(directoryPath);

                    // Guardar el archivo en la ruta compartida
                    using (var fileStreamToSave = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await fileStream.CopyToAsync(fileStreamToSave);
                    }

                    // Guardar el registro en la base de datos
                    await _documentRepository.InsertFirmaAsync(dni, fileName);
                }
                catch (IOException ioEx)
                {
                    // Manejar errores de I/O específicos
                    throw new InvalidOperationException($"Error al intentar guardar el archivo {fileName}: {ioEx.Message}", ioEx);
                }
                catch (Exception ex)
                {
                    // Manejar otros errores
                    throw new InvalidOperationException($"No se pudo guardar el archivo {fileName}. Detalles: {ex.Message}", ex);
                }
            }
        }




        public async Task<bool> DeleteNewsByIdAsync(int ide)
        {
            return await _documentRepository.DeleteNewsByIdAsync(ide);
        }


        public async Task<IEnumerable<BeneficioEmp>> GetBenefemple()
        {
            return await _documentRepository.GetBenefemple();
        }






    }



   
}



