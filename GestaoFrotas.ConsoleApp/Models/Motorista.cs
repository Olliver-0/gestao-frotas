using System; // Adicionamos isso para ter acesso ao "DateTime"

namespace GestaoFrotas.ConsoleApp.Models
{
  public class Motorista : Usuario
  {
    public string cnh;
    public DateTime validadeCNH;

    /**
    * Verifica se a data de validade da CNH deste motorista
    * é anterior à data de hoje (RN-002).
    */
    public bool isCnhVencida()
    {
      // "DateTime.Today" pega a data atual (ex: 28/10/2025)
      // Se a data de validade for MENOR (anterior) a hoje, 
      // significa que a CNH está vencida.
      if (this.validadeCNH < DateTime.Today)
      {
        return true; // Sim, está vencida
      }
      else
      {
        return false; // Não, está válida
      }

      // Uma forma mais curta de escrever esse 'if' seria:
      // return this.validadeCNH < DateTime.Today;
    }
  }
}
