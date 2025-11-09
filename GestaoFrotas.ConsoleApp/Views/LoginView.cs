using System;
using GestaoFrotas.ConsoleApp.Services;
using GestaoFrotas.ConsoleApp.Models;

namespace GestaoFrotas.ConsoleApp.Views
{
    public class LoginView
    {
        private readonly UsuarioService _usuarioService;

        public LoginView(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public Usuario TentarLogin()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine(" TELA DE LOGIN");
            Console.WriteLine("=========================================");

            Console.Write("Login (E-mail): ");
            string login = Console.ReadLine();

            Console.Write("Senha: ");
            string senha = Console.ReadLine();

            Usuario usuario = _usuarioService.ConsultarPorLogin(login);

            if (usuario != null && usuario.autenticar(senha))
            {
                return usuario;
            }

            Console.WriteLine("\nErro: Login ou senha inválidos. Pressione qualquer tecla para tentar novamente.");
            Console.ReadKey();
            return null;
        }
    }
}
