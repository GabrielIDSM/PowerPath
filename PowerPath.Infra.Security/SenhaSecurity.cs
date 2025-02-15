using PowerPath.Domain.Interfaces.Security;

namespace PowerPath.Infra.Security
{
    public class SenhaSecurity : ISenhaSecurity
    {
        public string GerarSenhaCriptografada(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool SenhaIsSenhaCriptografada(string senha, string senhaCriptografada)
        {
            return BCrypt.Net.BCrypt.Verify(senha, senhaCriptografada);
        }
    }
}
