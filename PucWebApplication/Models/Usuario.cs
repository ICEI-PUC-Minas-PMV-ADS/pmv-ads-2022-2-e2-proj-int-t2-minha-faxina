using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PucWebApplication.Models {
    [Table("Usuarios")]
    public class Usuario {

        [Display(Name = "Código")]
        [Column("Id")]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Column("Nome")]
        public string? Nome { get; set; }

        [Display(Name = "Email")]
        [Column("Email")]
        public string? Email { get; set; }

        [Display(Name = "CPF")]
        [Column("CPF")]
        public int cpf { get; set; }

        [Display(Name = "Idade")]
        [Column("Idade")]
        public int Idade { get; set; }

        [Display(Name = "CEP")]
        [Column("CEP")]
        public int cep { get; set; }

        [Display(Name = "Rua")]
        [Column("Rua")]
        public string? Rua { get; set; }

        [Display(Name = "Número")]
        [Column("Numero")]
        public int Numero { get; set; }

        [Display(Name = "Complemento")]
        [Column("Complemento")]
        public string? Complemento { get; set; }

        [Display(Name = "Bairro")]
        [Column("Bairro")]
        public string? Bairro { get; set; }

        [Display(Name = "Cidade")]
        [Column("Cidade")]
        public string? Cidade { get; set; }

        [Display(Name = "Senha")]
        [Column("Senha")]
        [DataType(DataType.Password)]
        public string? Senha { get; set; }
        [Required(ErrorMessage = "Obrigatório informar o Perfil")]
        public Perfil Perfil { get; set; }
    }

    public enum Perfil {
        Contratante,
        Empregado
    }
}
