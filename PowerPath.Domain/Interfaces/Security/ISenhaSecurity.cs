namespace PowerPath.Domain.Interfaces.Security
{
    public interface ISenhaSecurity
    {
        string GerarSenhaCriptografada(string senha);
        bool SenhaIsSenhaCriptografada(string senha, string senhaCriptografada);
    }
}
