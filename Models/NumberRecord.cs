namespace Parcil1_P4Luis.Models;

public record NumberRecordGet{
    public long Id { get; init; }
    public string Fecha { get; init; } = string.Empty;
    public long Numero { get; init; }
    public long Resultado { get; init; }
    }

public record NumberRecordSet(long Numero, long Resultado);