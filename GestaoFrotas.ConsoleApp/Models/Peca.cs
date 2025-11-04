using System;

namespace GestaoFrotas.ConsoleApp.Models
{
  public class Peca
  {
    public int id = 1;
    public string nome;
    public int quantidadeEstoque;
    public string descricao;
    public int pontoReposicao;
    
    //public bool isCritica;

    /**
    * Adiciona uma quantidade de itens ao estoque da peça.
    */
    public void adicionarEstoque(int quantidade)
    {
      // Apenas adiciona se a quantidade for um número positivo
      if (quantidade > 0)
      {
        // 'this.quantidadeEstoque += quantidade' é um atalho para
        // 'this.quantidadeEstoque = this.quantidadeEstoque + quantidade'
        this.quantidadeEstoque += quantidade;
      }
    }

    /**
    * Remove uma quantidade de itens do estoque (usado na OS - RN-004).
    */
    public void removerEstoque(int quantidade)
    {
      // Apenas remove se a quantidade for positiva
      // E se houver estoque suficiente para a remoção
      if (quantidade > 0 && this.quantidadeEstoque >= quantidade)
      {
        // 'this.quantidadeEstoque -= quantidade' é um atalho para
        // 'this.quantidadeEstoque = this.quantidadeEstoque - quantidade'
        this.quantidadeEstoque -= quantidade;
      }
    }

    /**
    * Verifica se o estoque atual está igual ou abaixo
    * do ponto de reposição (RN-009).
    */
    public bool verificarEstoqueBaixo()
    {
      if (this.quantidadeEstoque <= this.pontoReposicao)
      {
        return true; // Sim, o estoque está baixo
      }
      else
      {
        return false; // O estoque está ok
      }

      // Versão curta:
      // return this.quantidadeEstoque <= this.pontoReposicao;
    }
  }
}
