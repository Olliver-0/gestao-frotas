using System;

namespace GestaoFrotas.ConsoleApp.Models
{
  public class Peca
  {
    public int id;
    public string nome;
    public int quantidadeEstoque;
    public int pontoReposicao;
    public bool isCritica;

    public void adicionarEstoque(int quantidade)
    {
      if (quantidade > 0)
      {
        this.quantidadeEstoque += quantidade;
      }
    }
    public void removerEstoque(int quantidade)
    {
      if (quantidade > 0 && this.quantidadeEstoque >= quantidade)
      {
        this.quantidadeEstoque -= quantidade;
      }
    }
    public bool verificarEstoqueBaixo()
    {
      if (this.quantidadeEstoque <= this.pontoReposicao)
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
