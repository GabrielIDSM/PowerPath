namespace PowerPath.Domain.Entities
{
    public class Log
    {
        public string Acao { get; set; } = null!;

        public string Mensagem { get; set; } = null!;

        public DateTime DataHora { get; set; }
    }
}
