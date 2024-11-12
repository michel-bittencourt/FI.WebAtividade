using System.Threading.Tasks;

namespace FI.WebAtividadeEntrevista.Models.Interfaces
{
    public interface ICPFValidator
    {
        bool ValidaCPF(string cpf);
    }
}
