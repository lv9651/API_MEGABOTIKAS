using SISLAB_API.Areas.Maestros.Models;

using System.Collections.Generic;

namespace SISLAB_API.Areas.Maestros.Services
{
    public class AgendaService
    {
        private readonly AgendaRepository _agendaRepository;

        public AgendaService(AgendaRepository AgendaRepository)
        {
            _agendaRepository = AgendaRepository;
        }

        public void AddAgendaItem(AgendaItem item)
        {
            _agendaRepository.AddAgendaItem(item);
        }

        public void DeleteAgendaItem(int id)
        {
            _agendaRepository.DeleteAgendaItem(id);
        }

        public List<AgendaItem> GetAgendaItems(DateTime date)
        {
            return _agendaRepository.GetAgendaItems(date);
        }
    }
}