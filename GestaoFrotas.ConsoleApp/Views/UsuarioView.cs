using System;
using System.Collections.Generic;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;
using System.Linq;

namespace GestaoFrotas.ConsoleApp.Views
{
    public class UsuarioView
    {
        private readonly UsuarioService _usuarioService;
        private readonly List<string> perfisDisponiveis = new List<string> { "Administrador", "Coordenador", "Motorista", "Mecânico" };

        public UsuarioView(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public void GerenciarUsuarios()
        {
            bool continuar = true;
            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("=================================================");
                Console.WriteLine(" ADMINISTRAÇÃO: GERENCIAR USUÁRIOS E PERFIS (RF04)");
                Console.WriteLine("=================================================");

                // Passo 2: Exibir a lista de usuários existentes
                ListarUsuariosResumido();

                Console.WriteLine("\n--- SELECIONE UMA OPÇÃO (Passo 3) ---");
                Console.WriteLine("1. Cadastrar Novo Usuário");
                Console.WriteLine("2. Editar Usuário / Atribuir Perfil");
                Console.WriteLine("3. Excluir Usuário");
                Console.WriteLine("0. Voltar ao Menu Principal");
                Console.WriteLine("-------------------------------------");

                Console.Write("Opção: ");
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        CadastrarUsuario();
                        break;
                    case "2":
                        EditarUsuarioEPerfil();
                        break;
                    case "3":
                        ExcluirUsuario();
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("\nOpção inválida.");
                        Pausar();
                        break;
                }
            }
        }

        private void CadastrarUsuario()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine(" CADASTRO DE NOVO USUÁRIO");
            Console.WriteLine("=================================");

            string nome = LerString("Nome: ");
            string login = LerString("Login (E-mail): ");
            string senha = LerString("Senha Inicial: ");
            string perfil = LerPerfil();

            string resultado = _usuarioService.CadastrarUsuario(nome, login, senha, perfil);

            Console.WriteLine("\n---------------------------------");

            if (resultado.StartsWith("Sucesso:"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine(resultado);
            Console.ResetColor();

            Pausar();
        }

        private void ExcluirUsuario()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine(" EXCLUIR USUÁRIO");
            Console.WriteLine("=================================");
            ListarUsuariosResumido();
            Console.WriteLine();

            int id = LerIdUsuario("Digite o ID do usuário para exclusão: ");

            if (id == 0) return;

            Usuario usuario = _usuarioService.ConsultarPorId(id);
            if (usuario == null)
            {
                Console.WriteLine("\nErro: Usuário não encontrado.");
                Pausar();
                return;
            }

            Console.WriteLine($"\nUsuário selecionado: {usuario.nome} ({usuario.perfil})");
            Console.Write("Deseja realmente EXCLUIR este usuário? (S/N): ");
            string confirmacao = Console.ReadLine();

            if (confirmacao.ToUpper() != "S")
            {
                Console.WriteLine("\nExclusão cancelada pelo Administrador. (A.3)");
                Pausar();
                return;
            }

            string resultado = _usuarioService.ExcluirUsuario(id);

            Console.WriteLine("\n---------------------------------");
            if (resultado.StartsWith("Sucesso:"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(resultado);
            Console.ResetColor();

            Pausar();
        }

        private void EditarUsuarioEPerfil()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine(" EDITAR/ATRIBUIR PERFIL");
            Console.WriteLine("=================================");
            ListarUsuariosResumido();
            Console.WriteLine();

            int id = LerIdUsuario("Digite o ID do usuário para edição: ");

            if (id == 0) return;

            Usuario usuario = _usuarioService.ConsultarPorId(id);
            if (usuario == null)
            {
                Console.WriteLine("\nErro: Usuário não encontrado.");
                Pausar();
                return;
            }

            Console.WriteLine("\n--- DADOS ATUAIS ---");
            Console.WriteLine($"Nome atual: {usuario.nome}");
            Console.WriteLine($"Login atual: {usuario.login}");
            Console.WriteLine($"Perfil atual: {usuario.perfil}");
            Console.WriteLine("--------------------");

            Console.WriteLine("\n--- NOVOS DADOS --- (Deixe em branco para manter o valor atual)");
            string novoNome = LerString($"Novo Nome ({usuario.nome}): ", false);
            string novoLogin = LerString($"Novo Login/E-mail ({usuario.login}): ", false);
            string novoPerfil = LerPerfil($"Novo Perfil ({usuario.perfil}): ", false);

            string nomeFinal = string.IsNullOrWhiteSpace(novoNome) ? usuario.nome : novoNome;
            string loginFinal = string.IsNullOrWhiteSpace(novoLogin) ? usuario.login : novoLogin;
            string perfilFinal = string.IsNullOrWhiteSpace(novoPerfil) ? usuario.perfil : novoPerfil;

            string resultado = _usuarioService.EditarUsuario(id, nomeFinal, loginFinal, perfilFinal);

            Console.WriteLine("\n---------------------------------");
            if (resultado.StartsWith("Sucesso:"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(resultado);
            Console.ResetColor();

            Pausar();
        }

        private void ListarUsuariosResumido()
        {
            Console.WriteLine("\nUSUÁRIOS CADASTRADOS (ID | NOME | LOGIN | PERFIL):");

            var usuarios = _usuarioService.ConsultarTodos().OrderBy(u => u.id).ToList();

            if (usuarios == null || usuarios.Count == 0)
            {
                Console.WriteLine("Nenhum usuário cadastrado.");
                return;
            }

            foreach (var user in usuarios)
            {
                Console.WriteLine($"- ID: {user.id,-4} | Nome: {user.nome,-20} | Login: {user.login,-25} | Perfil: {user.perfil}");
            }
        }

        private string LerString(string prompt, bool obrigatorio = true)
        {
            string entrada;
            while (true)
            {
                Console.Write(prompt);
                entrada = Console.ReadLine();

                if (obrigatorio && string.IsNullOrWhiteSpace(entrada))
                {
                    Console.WriteLine("Erro: Este campo é obrigatório.");
                }
                else if (!obrigatorio && string.IsNullOrWhiteSpace(entrada))
                {
                    return string.Empty;
                }
                else
                {
                    return entrada.Trim();
                }
            }
        }
        private int LerIdUsuario(string prompt)
        {
            int id;
            while (true)
            {
                Console.Write(prompt);
                string entrada = Console.ReadLine();

                if (entrada == "0") return 0; // Opção de cancelar

                if (int.TryParse(entrada, out id) && id > 0)
                {
                    return id;
                }
                else
                {
                    Console.WriteLine("Erro: ID inválido. Digite um número inteiro positivo.");
                }
            }
        }

        private string LerPerfil(string prompt = "Perfil (Administrador, Coordenador, Motorista, Mecânico): ", bool obrigatorio = true)
        {
            if (!obrigatorio)
            {
                Console.Write(prompt);
                string entrada = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(entrada)) return string.Empty;

                if (perfisDisponiveis.Any(p => p.Equals(entrada.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    return entrada.Trim();
                }
                else
                {
                    Console.WriteLine("Erro: Perfil inválido. Manterá o perfil atual.");
                    return string.Empty;
                }
            }

            string perfil;
            while (true)
            {
                Console.Write(prompt);
                perfil = Console.ReadLine();

                if (perfisDisponiveis.Any(p => p.Equals(perfil.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    return perfil.Trim();
                }
                else
                {
                    Console.WriteLine("Erro: Perfil inválido. Escolha um dos perfis disponíveis.");
                }
            }
        }
        private void Pausar(string mensagem = "Pressione qualquer tecla para continuar...")
        {
            Console.WriteLine();
            Console.WriteLine(mensagem);
            Console.ReadKey();
        }
    }
}
