using Microsoft.EntityFrameworkCore;
using CQRS_Un_Proyecto.Infrastructure.Model;
using CQRS_Un_Proyecto.Domain.Entities;

namespace MySQLService.Repositories
{
    public class HistorialEstadoConsultaRepository
    {
        private readonly GestionSaludContext _context;

        public HistorialEstadoConsultaRepository(GestionSaludContext context)
        {
            _context = context;
        }

        public async Task<HistorialEstadoConsultaEntity?> GetByIdAsync(int idHistorialEstado)
        {
            var historial = await _context.HistorialEstadoConsulta.FindAsync(idHistorialEstado);
            return historial != null ? new HistorialEstadoConsultaEntity(historial) : null;
        }

        public async Task<HistorialEstadoConsultaEntity> AddAsync(HistorialEstadoConsultaEntity historialEntity)
        {
            var historialCrear = new HistorialEstadoConsultum
            {
                IdConsulta = historialEntity.IdConsulta,
                IdEstadoConsulta = historialEntity.IdEstadoConsulta,
                FechaCambio = historialEntity.FechaCambio,
                UsuarioResponsable = historialEntity.UsuarioResponsable,
                Comentario = historialEntity.Comentario
            };

            _context.HistorialEstadoConsulta.Add(historialCrear);
            await _context.SaveChangesAsync();

            historialEntity.IdHistorialEstado = historialCrear.IdHistorialEstado;
            return historialEntity;
        }

        public async Task<HistorialEstadoConsultaEntity?> UpdateAsync(HistorialEstadoConsultaEntity historialEntity)
        {
            var existingHistorial = await _context.HistorialEstadoConsulta.FindAsync(historialEntity.IdHistorialEstado);
            if (existingHistorial == null) return null;

            existingHistorial.IdConsulta = historialEntity.IdConsulta;
            existingHistorial.IdEstadoConsulta = historialEntity.IdEstadoConsulta;
            existingHistorial.FechaCambio = historialEntity.FechaCambio;
            existingHistorial.UsuarioResponsable = historialEntity.UsuarioResponsable;
            existingHistorial.Comentario = historialEntity.Comentario;

            await _context.SaveChangesAsync();
            return new HistorialEstadoConsultaEntity(existingHistorial);
        }

        public async Task<bool> DeleteAsync(int idHistorialEstado)
        {
            var historial = await _context.HistorialEstadoConsulta.FindAsync(idHistorialEstado);
            if (historial == null) return false;

            _context.HistorialEstadoConsulta.Remove(historial);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
