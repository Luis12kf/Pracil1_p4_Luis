namespace Parcil1_P4Luis.Models;

public record NumberRecordGet(int Id, int Number, DateTime Fecha, int Resultado);

public record NumberRecordSet(int Number, int Resultado);