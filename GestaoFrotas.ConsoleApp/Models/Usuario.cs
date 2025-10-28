using System; // Adicionamos isso para ter acesso a classes do sistema

namespace GestaoFrotas.ConsoleApp.Models
{
  public class Usuario
  {
    public int id;
    public string nome;
    public string login;
    public string senha;
    public string perfil;

    /**
    * Verifica se a senha fornecida pelo usuário é a mesma senha
    * que está salva neste objeto.
    */
    public bool autenticar(string senhaFornecida)
    {
      // Compara a senha salva no 'this.senha' com a senha fornecida
      if (this.senha == senhaFornecida)
      {
        return true; // As senhas batem
      }
      else
      {
        return false; // As senhas são diferentes
      }

      // Uma forma mais curta de fazer o 'if/else' acima seria:
      // return this.senha == senhaFornecida;
    }

    /**
    * Atualiza a senha atual do usuário para uma nova senha.
    */
    public void alterarSenha(string novaSenha)
    {
      // Simplesmente atribui o novo valor ao campo 'senha'
      this.senha = novaSenha;
    }
  }
}
