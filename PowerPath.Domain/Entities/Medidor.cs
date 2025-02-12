namespace PowerPath.Domain.Entities;

public partial class Medidor
{
    public string Instalacao { get; set; } = null!;

    public int Lote { get; set; }

    public string Operadora { get; set; } = null!;

    public string Fabricante { get; set; } = null!;

    public int Modelo { get; set; }

    public int Versao { get; set; }

    public DateTime Criacao { get; set; }

    public DateTime? Alteracao { get; set; }

    public ulong Excluido { get; set; }
}
