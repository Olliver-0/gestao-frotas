using System; // Necessário para o DateTime

namespace GestaoFrotas.ConsoleApp.Models
{
  public class OrdemDeServico
  {
    public int id;
    public DateTime dataAbertura;

    // --- Atributos Opcionais Corrigidos ---
    // Usamos '?' para permitir que esses campos sejam nulos,
    // pois uma OS recém-criada não tem esses dados.
    public DateTime? dataFechamento;
    public double hodometroEntrada;
    public double? hodometroSaida;
    // ---

    public string tipo;
    public int veiculoId;
    public int motoristaId;

    // --- Atributo Opcional Corrigido ---
    public int? mecanicoId;
    // ---

    /**
    * Finaliza a Ordem de Serviço, registrando a data/hora
    * e a quilometragem de saída (RN-005).
    */
    public void fecharOS(double hodometroSaida)
    {
      // Verifica se a OS já não foi fechada (para evitar fechar 2x)
      if (this.dataFechamento == null)
      {
        // Registra o momento exato do fechamento
        this.dataFechamento = DateTime.Now;

        // Registra o hodômetro de saída
        this.hodometroSaida = hodometroSaida;
      }
    }
  }
}
