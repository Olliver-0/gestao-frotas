using System; 

namespace GestaoFrotas.ConsoleApp.Models
{
  public class Motorista : Usuario
  {
    public string cnh;
    public DateTime validadeCNH;
    
    // Campos adicionados baseados no protótipo (RF02)
    public string categoriaCNH;
    public string telefone;
    public string endereco;
    public string status; // Ex: "Ativo", "Inativo"

    // O campo 'cpf' pode ser o 'login' da classe 'Usuario'
    public string cpf
    {
      get { return this.login; }
      set { this.login = value; }
    }


    /**
    * Verifica se a data de validade da CNH deste motorista
    * é anterior à data de hoje (RN-002).
    */
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