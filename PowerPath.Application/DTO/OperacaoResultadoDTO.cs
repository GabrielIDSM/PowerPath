namespace PowerPath.Application.DTO
{
    public class OperacaoResultadoDTO<Entity> where Entity : class
    {
        public bool Sucesso { get; set; }

        public string? Mensagem { get; set; }

        public Entity? Resultado { get; set; }
    }
}
