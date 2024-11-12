using FI.WebAtividadeEntrevista.Models.Interfaces;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FI.WebAtividadeEntrevista.Models.Services
{
    public class CPFValidator : ICPFValidator
    {
        public bool ValidaCPF(string cpf)
        {
            try
            {
                cpf = Regex.Replace(cpf, "[^0-9]", "");

                if (cpf.Length != 11 || new string(cpf[0], 11) == cpf)
                    return false;

                int soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += (cpf[i] - '0') * (10 - i);

                int primeiroDigitoVerificador = soma % 11 < 2 ? 0 : 11 - (soma % 11);

                if (cpf[9] - '0' != primeiroDigitoVerificador)
                    return false;

                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += (cpf[i] - '0') * (11 - i);

                int segundoDigitoVerificador = soma % 11 < 2 ? 0 : 11 - (soma % 11);

                if (cpf[10] - '0' != segundoDigitoVerificador)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
