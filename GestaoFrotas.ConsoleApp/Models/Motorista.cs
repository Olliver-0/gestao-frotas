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

