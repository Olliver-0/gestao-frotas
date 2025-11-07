using System; 

namespace GestaoFrotas.ConsoleApp.Models
{
  public class Motorista : Usuario
  {
    public string cnh;
    public DateTime validadeCNH;
    public string categoriaCNH;
    public string telefone;
    public string endereco;
    public string status; 
    public string cpf
    {
      get { return this.login; }
      set { this.login = value; }
    }

    public bool isCnhVencida()
    {
      if (this.validadeCNH < DateTime.Today)
      {
        return true; 
      }
      else
      {
        return false; 
      }
    }
  }
}