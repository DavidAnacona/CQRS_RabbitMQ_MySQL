using Microsoft.EntityFrameworkCore;
using CQRS_Un_Proyecto.Infrastructure.Model;

namespace MySQLService.Repositories
{
    public class ConsultaRepository
    {
        private readonly GestionSaludContext _context;

        public ConsultaRepository(GestionSaludContext context)
        {
            _context = context;
        }

        public async Task<ConsultaEntity> AddAsync(ConsultaEntity consulta)
        {
            Consultum consultaCrear = new Consultum();
            consultaCrear.IdConsulta = consulta.IdConsulta;
            consultaCrear.FechaHora = consulta.FechaHora;
            consultaCrear.IdPaciente = consulta.IdPaciente;
            consultaCrear.IdMedico = consulta.IdMedico;
            consultaCrear.IdEstadoConsulta = consulta.IdEstadoConsulta;
            consultaCrear.Notas = consulta.Notas;
            _context.Consulta.Add(consultaCrear);
            await _context.SaveChangesAsync();

            consulta.IdConsulta = consultaCrear.IdConsulta;
            return consulta;
        }

        public async Task<ConsultaEntity?> UpdateAsync(ConsultaEntity consulta)
        {
            var existingConsulta = await _context.Consulta.FindAsync(consulta.IdConsulta);
            if (existingConsulta == null) return null;

            existingConsulta.IdPaciente = consulta.IdPaciente;
            existingConsulta.FechaHora = consulta.FechaHora;
            existingConsulta.IdEstadoConsulta = consulta.IdEstadoConsulta;
            existingConsulta.IdMedico = consulta.IdMedico;
            existingConsulta.Notas = consulta.Notas;

            await _context.SaveChangesAsync();
            ConsultaEntity consultaRespuesta = new ConsultaEntity(existingConsulta);
            return consultaRespuesta;
        }

        public async Task<bool> DeleteAsync(int idConsulta)
        {
            var consulta = await _context.Consulta.FindAsync(idConsulta);
            if (consulta == null) return false;

            _context.Consulta.Remove(consulta);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}