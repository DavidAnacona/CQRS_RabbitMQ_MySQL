using Microsoft.EntityFrameworkCore;
using CQRS_Un_Proyecto.Infrastructure.Model;
using CQRS_Un_Proyecto.Domain.Entities;

namespace MySQLService.Repositories
{
    public class PacienteRepository
    {
        private readonly GestionSaludContext _context;

        public PacienteRepository(GestionSaludContext context)
        {
            _context = context;
        }

        public async Task<PacienteEntity?> GetByIdAsync(int idPaciente)
        {
            var paciente = await _context.Pacientes.FindAsync(idPaciente);
            return paciente != null ? new PacienteEntity(paciente) : null;
        }

        public async Task<PacienteEntity> AddAsync(PacienteEntity pacienteEntity)
        {
            var pacienteCrear = new Paciente
            {
                Nombre = pacienteEntity.Nombre,
                Apellido = pacienteEntity.Apellido,
                FechaNacimiento = pacienteEntity.FechaNacimiento,
                Telefono = pacienteEntity.Telefono,
                Correo = pacienteEntity.Correo,
                Direccion = pacienteEntity.Direccion
            };

            _context.Pacientes.Add(pacienteCrear);
            await _context.SaveChangesAsync();

            pacienteEntity.IdPaciente = pacienteCrear.IdPaciente;
            return pacienteEntity;
        }

        public async Task<PacienteEntity?> UpdateAsync(PacienteEntity pacienteEntity)
        {
            var existingPaciente = await _context.Pacientes.FindAsync(pacienteEntity.IdPaciente);
            if (existingPaciente == null) return null;

            existingPaciente.Nombre = pacienteEntity.Nombre;
            existingPaciente.Apellido = pacienteEntity.Apellido;
            existingPaciente.FechaNacimiento = pacienteEntity.FechaNacimiento;
            existingPaciente.Telefono = pacienteEntity.Telefono;
            existingPaciente.Correo = pacienteEntity.Correo;
            existingPaciente.Direccion = pacienteEntity.Direccion;

            await _context.SaveChangesAsync();
            return new PacienteEntity(existingPaciente);
        }

        public async Task<bool> DeleteAsync(int idPaciente)
        {
            var paciente = await _context.Pacientes.FindAsync(idPaciente);
            if (paciente == null) return false;

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
