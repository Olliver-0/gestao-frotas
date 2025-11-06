using System;

namespace GestaoFrotas.ConsoleApp.Models
{
  public class Peca
  {
    public int id = 1;
    public string nome;
    public string quantidadeEstoque;
    public string descricao;
    public string pontoReposicao;
    //int posicao;


    public void adicionarEstoque(int quantidade)
    {

      if (quantidade > 0)
      {

        this.quantidadeEstoque += quantidade;
      }
    }
    
    /*public void removerEstoque(int quantidade)
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
    }*/
  }
}
