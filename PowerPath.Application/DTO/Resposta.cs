namespace PowerPath.Application.DTO
{
    public class Resposta<Entity> where Entity : class
    {
        public bool IsSucesso { get; set; }
        public string? Mensagem { get; set; }
        public Entity? Resultado { get; set; }

        public static Resposta<Entity> Sucesso(Entity entity)
        {
            return new Resposta<Entity>()
            {
                IsSucesso = true,
                Resultado = entity
            };
        }

        public static Resposta<Entity> Erro(string mensagem)
        {
            return new Resposta<Entity>()
            {
                IsSucesso = false,
                Mensagem = mensagem
            };
        }
    }
}
