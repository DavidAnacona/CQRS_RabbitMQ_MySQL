using Microsoft.EntityFrameworkCore;
using CQRS_Un_Proyecto.Infrastructure.Model;
using CQRS_Un_Proyecto.Domain.Entities;

namespace MySQLService.Repositories
{
    public class EstadoConsultaRepository
    {
        private readonly GestionSaludContext _context;

        public EstadoConsultaRepository(GestionSaludContext context)
        {
            _context = context;
        }

        public async Task<EstadoConsultaEntity?> GetByIdAsync(int idEstadoConsulta)
        {
            var estado = await _context.EstadoConsulta.FindAsync(idEstadoConsulta);
            return estado != null ? new EstadoConsultaEntity(estado) : null;
        }

        public async Task<EstadoConsultaEntity> AddAsync(EstadoConsultaEntity estadoConsulta)
        {
            var estadoCrear = new EstadoConsultum
            {
                NombreEstado = estadoConsulta.NombreEstado
            };

            _context.EstadoConsulta.Add(estadoCrear);
            await _context.SaveChangesAsync();

            estadoConsulta.IdEstadoConsulta = estadoCrear.IdEstadoConsulta;
            return estadoConsulta;
        }

        public async Task<EstadoConsultaEntity?> UpdateAsync(EstadoConsultaEntity estadoConsulta)
        {
            var existingEstado = await _context.EstadoConsulta.FindAsync(estadoConsulta.IdEstadoConsulta);
            if (existingEstado == null) return null;

            existingEstado.NombreEstado = estadoConsulta.NombreEstado;
            await _context.SaveChangesAsync();

            return new EstadoConsultaEntity(existingEstado);
        }

        public async Task<bool> DeleteAsync(int idEstadoConsulta)
        {
            var estado = await _context.EstadoConsulta.FindAsync(idEstadoConsulta);
            if (estado == null) return false;

            _context.EstadoConsulta.Remove(estado);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
