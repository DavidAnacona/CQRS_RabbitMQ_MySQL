using Microsoft.EntityFrameworkCore;
using CQRS_Un_Proyecto.Infrastructure.Model;
using CQRS_Un_Proyecto.Domain.Entities;

namespace MySQLService.Repositories
{
    public class RecetaRepository
    {
        private readonly GestionSaludContext _context;

        public RecetaRepository(GestionSaludContext context)
        {
            _context = context;
        }

        public async Task<RecetaEntity?> GetByIdAsync(int idReceta)
        {
            var receta = await _context.Receta.FindAsync(idReceta);
            return receta != null ? new RecetaEntity(receta) : null;
        }

        public async Task<RecetaEntity> AddAsync(RecetaEntity recetaEntity)
        {
            var recetaCrear = new Recetum
            {
                IdConsulta = recetaEntity.IdConsulta,
                Medicamentos = recetaEntity.Medicamentos,
                Indicaciones = recetaEntity.Indicaciones
            };

            _context.Receta.Add(recetaCrear);
            await _context.SaveChangesAsync();

            recetaEntity.IdReceta = recetaCrear.IdReceta;
            return recetaEntity;
        }

        public async Task<RecetaEntity?> UpdateAsync(RecetaEntity recetaEntity)
        {
            var existingReceta = await _context.Receta.FindAsync(recetaEntity.IdReceta);
            if (existingReceta == null) return null;

            existingReceta.IdConsulta = recetaEntity.IdConsulta;
            existingReceta.Medicamentos = recetaEntity.Medicamentos;
            existingReceta.Indicaciones = recetaEntity.Indicaciones;

            await _context.SaveChangesAsync();
            return new RecetaEntity(existingReceta);
        }

        public async Task<bool> DeleteAsync(int idReceta)
        {
            var receta = await _context.Receta.FindAsync(idReceta);
            if (receta == null) return false;

            _context.Receta.Remove(receta);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
