namespace PowerPath.Domain.Interfaces.Security
{
    public interface IJWTSecurity
    {
        string GerarToken(string nome);
    }
}
