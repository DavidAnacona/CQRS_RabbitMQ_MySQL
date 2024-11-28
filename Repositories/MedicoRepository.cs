using Microsoft.EntityFrameworkCore;
using CQRS_Un_Proyecto.Infrastructure.Model;
using CQRS_Un_Proyecto.Domain.Entities;

namespace MySQLService.Repositories
{
    public class MedicoRepository
    {
        private readonly GestionSaludContext _context;

        public MedicoRepository(GestionSaludContext context)
        {
            _context = context;
        }

        public async Task<MedicoEntity?> GetByIdAsync(int idMedico)
        {
            var medico = await _context.Medicos.FindAsync(idMedico);
            return medico != null ? new MedicoEntity(medico) : null;
        }

        public async Task<MedicoEntity> AddAsync(MedicoEntity medicoEntity)
        {
            var medicoCrear = new Medico
            {
                Nombre = medicoEntity.Nombre,
                Apellido = medicoEntity.Apellido,
                Especialidad = medicoEntity.Especialidad,
                Telefono = medicoEntity.Telefono,
                Correo = medicoEntity.Correo
            };

            _context.Medicos.Add(medicoCrear);
            await _context.SaveChangesAsync();

            medicoEntity.IdMedico = medicoCrear.IdMedico;
            return medicoEntity;
        }

        public async Task<MedicoEntity?> UpdateAsync(MedicoEntity medicoEntity)
        {
            var existingMedico = await _context.Medicos.FindAsync(medicoEntity.IdMedico);
            if (existingMedico == null) return null;

            existingMedico.Nombre = medicoEntity.Nombre;
            existingMedico.Apellido = medicoEntity.Apellido;
            existingMedico.Especialidad = medicoEntity.Especialidad;
            existingMedico.Telefono = medicoEntity.Telefono;
            existingMedico.Correo = medicoEntity.Correo;

            await _context.SaveChangesAsync();
            return new MedicoEntity(existingMedico);
        }

        public async Task<bool> DeleteAsync(int idMedico)
        {
            var medico = await _context.Medicos.FindAsync(idMedico);
            if (medico == null) return false;

            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
