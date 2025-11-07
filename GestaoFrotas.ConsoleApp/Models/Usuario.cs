using System;

namespace GestaoFrotas.ConsoleApp.Models
{
  public class Usuario
  {
    public int id;
    public string nome;
    public string login;
    public string senha;
    public string perfil;
    public bool autenticar(string senhaFornecida)
    {
      if (this.senha == senhaFornecida)
      {
        return true;
      }
      else
      {
        return false; 
      }
    }
    public void alterarSenha(string novaSenha)
    {
      this.senha = novaSenha;
    }
  }
}
