using CQRS_Un_Proyecto.Infrastructure.Model;

public class ConsultaEntity
{
    public int IdConsulta { get; set; }
    public int IdPaciente { get; set; }
    public DateTime FechaHora { get; set; }
    public int IdEstadoConsulta { get; set; }
    public int IdMedico { get; set; }
    public string? Notas { get; set; }

    public ConsultaEntity() { }
    public ConsultaEntity(int idConsulta, int idPaciente, DateTime fechaHora, int idEstadoConsulta, int idMedico, string? notas)
    {
        IdConsulta = idConsulta;
        IdPaciente = idPaciente;
        FechaHora = fechaHora;
        IdEstadoConsulta = idEstadoConsulta;
        IdMedico = idMedico;
        Notas = notas;
    }

    public ConsultaEntity(Consultum existingConsulta)
    {
        this.IdConsulta = existingConsulta.IdConsulta;
        this.IdPaciente = existingConsulta.IdPaciente;
        this.FechaHora = existingConsulta.FechaHora;
        this.IdEstadoConsulta = existingConsulta.IdEstadoConsulta;
        this.IdMedico = existingConsulta.IdMedico;
        this.Notas = existingConsulta.Notas;
    }
}
